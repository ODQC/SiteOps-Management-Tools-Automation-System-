import { Injectable } from '@angular/core';

export type Tema = 'light' | 'dark';

const STORAGE_KEY = 'saih-tema';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  private temaActual: Tema = 'light';

  constructor() {
    const guardado = localStorage.getItem(STORAGE_KEY) as Tema | null;
    this.aplicarTema(guardado === 'dark' ? 'dark' : 'light');
  }

  get tema(): Tema {
    return this.temaActual;
  }

  alternarTema(): void {
    this.aplicarTema(this.temaActual === 'dark' ? 'light' : 'dark');
  }

  private aplicarTema(tema: Tema): void {
    this.temaActual = tema;
    document.body.style.colorScheme = tema;
    localStorage.setItem(STORAGE_KEY, tema);
  }
}
