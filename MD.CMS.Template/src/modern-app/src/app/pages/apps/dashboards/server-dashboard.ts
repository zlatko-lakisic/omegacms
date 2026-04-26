import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';

type ServerData = { widget1: { title: string; chart: { key: string; values: { y: number }[] }[] } };

@Component({
  selector: 'app-server-dashboard',
  imports: [MatCardModule],
  template: `
    @if (error()) {
      <p class="err">{{ error() }}</p>
    } @else {
      <h1 class="title">Server dashboard</h1>
      <mat-card>
        <mat-card-title>{{ w()?.title ?? '—' }}</mat-card-title>
        <mat-card-content>
          <p>Sample data points: {{ w()?.chart?.[0]?.values?.length ?? 0 }} (from legacy JSON)</p>
        </mat-card-content>
      </mat-card>
    }
  `,
  styles: `
    .title {
      font: var(--mat-sys-headline-medium);
    }
    .err {
      color: var(--mat-sys-error);
    }
  `,
})
export class ServerDashboard {
  private readonly http = inject(HttpClient);
  readonly w = signal<ServerData['widget1'] | null>(null);
  readonly error = signal<string | null>(null);

  constructor() {
    this.http.get<ServerData>('legacy-data/dashboard/server/data.json').subscribe({
      next: (d) => this.w.set(d.widget1),
      error: (e: Error) => this.error.set(e.message),
    });
  }
}
