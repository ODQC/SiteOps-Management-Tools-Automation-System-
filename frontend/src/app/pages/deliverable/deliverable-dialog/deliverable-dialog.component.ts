import { Component, Inject, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DeliverableService } from 'src/app/services/Deliverable/deliverable.service';
import { TaskService } from 'src/app/services/Task/task.service';
import { DocumentService } from 'src/app/services/Document/document.service';
import { ToastService } from 'src/app/services/Toast/toast.service';
import { IDeliverable } from 'src/app/interfaces/IDeliverable';
import { ITask } from 'src/app/interfaces/ITask';

@Component({
  selector: 'app-deliverable-dialog',
  templateUrl: './deliverable-dialog.component.html',
  styleUrls: ['./deliverable-dialog.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false
})
export class DeliverableDialogComponent implements OnInit {
  formulario: UntypedFormGroup;
  esEdicion: boolean;
  taskesList: ITask[] = [];
  documentSeleccionado: File | null = null;
  subiendoDocument = false;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: IDeliverable | null,
    private dialogRef: MatDialogRef<DeliverableDialogComponent>,
    private fb: UntypedFormBuilder,
    private deliverableService: DeliverableService,
    private taskService: TaskService,
    private documentService: DocumentService,
    private toastr: ToastService
  ) {
    this.esEdicion = !!data;
    this.formulario = this.fb.group({
      code: ['', Validators.required],
      name: ['', Validators.required],
      description: ['', Validators.required],
      fK_idTask1: ['', Validators.required],
      status: ['Activo', Validators.required]
    });

    if (data) {
      this.formulario.patchValue(data);
    }
  }

  ngOnInit(): void {
    this.taskService.obtenerTaskes().subscribe(a => this.taskesList = a);
  }

  onDocumentSeleccionado(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.documentSeleccionado = input.files?.[0] || null;
  }

  esCampoValido(field: string): boolean {
    const control = this.formulario.get(field);
    return !!(control?.touched && control?.invalid);
  }

  cancelar(): void {
    this.dialogRef.close(false);
  }

  guardar(): void {
    if (this.formulario.invalid) {
      this.formulario.markAllAsTouched();
      return;
    }
    if (!this.esEdicion && !this.documentSeleccionado) {
      this.toastr.warning('Select a deliverable document', 'Missing document');
      return;
    }

    if (this.documentSeleccionado) {
      this.subiendoDocument = true;
      this.documentService.subirDocument(this.documentSeleccionado).subscribe({
        next: (res: any) => {
          this.subiendoDocument = false;
          const documentId = res?.object?.documentId ?? res?.object?.DocumentId;
          this.guardarDeliverable(documentId);
        },
        error: () => {
          this.subiendoDocument = false;
          this.toastr.error('Could not upload the document', 'Error');
        }
      });
    } else {
      this.guardarDeliverable(this.data?.fK_idDocument);
    }
  }

  private guardarDeliverable(documentId?: number): void {
    const payload = {
      ...(this.data || {}),
      ...this.formulario.value,
      fK_idDocument: documentId,
      documentName: this.documentSeleccionado?.name || this.data?.documentName
    };

    const request$ = this.esEdicion
      ? this.deliverableService.actualizarDeliverable(this.data!.pK_idDeliverable, payload)
      : this.deliverableService.guardarDeliverable(payload);

    request$.subscribe({
      next: () => {
        this.toastr.success(this.esEdicion ? 'Deliverable updated' : 'Deliverable created', 'Success');
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.toastr.error(err?.error?.mensaje || 'Internal server error', 'Could not save');
      }
    });
  }
}
