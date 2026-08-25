import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({ providedIn: 'root' })
export class ToastService {

  constructor(private snackBar: MatSnackBar) {}

  private fire(panelClass: string, message: string, title?: string): void {
    const text = title ? `${title} — ${message}` : message;
    this.snackBar.open(text, 'Close', {
      duration: 4000,
      horizontalPosition: 'end',
      verticalPosition: 'top',
      panelClass: ['siteops-snackbar', panelClass]
    });
  }

  success(message: string, title?: string): void {
    this.fire('siteops-snackbar--success', message, title);
  }

  error(message: string, title?: string): void {
    this.fire('siteops-snackbar--error', message, title);
  }

  warning(message: string, title?: string): void {
    this.fire('siteops-snackbar--warning', message, title);
  }
}
