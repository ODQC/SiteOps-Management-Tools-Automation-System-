import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { EmployeesService } from 'src/app/services/Employees/employees.service';
import { DocumentService } from 'src/app/services/Document/document.service';
import { ToastService } from 'src/app/services/Toast/toast.service';
import { Employees } from 'src/app/models/employees';

@Component({
  selector: 'app-my-profile',
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false
})
export class MyProfileComponent implements OnInit {
  formulario: UntypedFormGroup;
  employee: Employees | null = null;
  cedula = '';
  cargando = true;
  subiendoDocument = false;
  documentSeleccionado: File | null = null;
  fotoActual: string | null = null;
  fotoPreview: string | null = null;

  constructor(
    private fb: UntypedFormBuilder,
    private employeesService: EmployeesService,
    private documentService: DocumentService,
    private toastr: ToastService
  ) {
    this.formulario = this.fb.group({
      phone: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.cedula = this.employeesService.cargarProfileEmployee().NationalId;

    this.employeesService.obtenerEmployee(this.cedula).subscribe({
      next: (res: any) => {
        this.employee = res?.object ?? null;
        this.formulario.patchValue({ phone: this.employee?.phone });
        this.cargando = false;
      },
      error: () => {
        this.cargando = false;
        this.toastr.error('No se pudo cargar tu profile', 'Error');
      }
    });

    this.documentService.obtenerImagenEmployee(this.cedula).subscribe({
      next: (res: any) => {
        this.fotoActual = res?.object ? 'data:image/jpeg;base64,' + res.object : null;
      },
      error: () => {
        this.fotoActual = null;
      }
    });
  }

  onFotoSeleccionada(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0] || null;
    this.documentSeleccionado = file;

    if (file) {
      const reader = new FileReader();
      reader.onload = () => this.fotoPreview = reader.result as string;
      reader.readAsDataURL(file);
    }
  }

  esCampoValido(field: string): boolean {
    const control = this.formulario.get(field);
    return !!(control?.touched && control?.invalid);
  }

  guardar(): void {
    if (this.formulario.invalid) {
      this.formulario.markAllAsTouched();
      return;
    }

    if (this.documentSeleccionado) {
      this.subiendoDocument = true;
      this.documentService.subirDocument(this.documentSeleccionado).subscribe({
        next: (res: any) => {
          this.subiendoDocument = false;
          const documentId = res?.object?.documentId ?? res?.object?.DocumentId;
          this.guardarProfile(documentId);
        },
        error: () => {
          this.subiendoDocument = false;
          this.toastr.error('No se pudo subir la foto', 'Error');
        }
      });
    } else {
      this.guardarProfile();
    }
  }

  private guardarProfile(documentId?: number): void {
    const payload: { phone: string, fK_idDocument1?: number } = {
      phone: this.formulario.value.phone
    };
    if (documentId) {
      payload.fK_idDocument1 = documentId;
    }

    this.employeesService.actualizarMiProfile(payload).subscribe({
      next: () => {
        this.toastr.success('Profile actualizado', 'Éxito');
        if (documentId) {
          this.fotoActual = this.fotoPreview;
          this.fotoPreview = null;
          this.documentSeleccionado = null;
        }
      },
      error: (err) => {
        this.toastr.error(err?.error?.mensaje || 'Error interno del servidor', 'No se pudo guardar');
      }
    });
  }
}
