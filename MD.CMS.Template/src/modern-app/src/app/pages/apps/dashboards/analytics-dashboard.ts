import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';

type Analytics = { widget1: { onlineUsers: number; title: string } };

@Component({
  selector: 'app-analytics-dashboard',
  imports: [MatCardModule],
  template: `
    @if (error()) {
      <p class="err">{{ error() }}</p>
    } @else {
      <h1 class="title">Analytics</h1>
      <mat-card>
        <mat-card-title>{{ w()?.title ?? 'Overview' }}</mat-card-title>
        <mat-card-content>
          <p class="big">Online users: {{ w()?.onlineUsers ?? '—' }}</p>
        </mat-card-content>
      </mat-card>
    }
  `,
  styles: `
    .title {
      font: var(--mat-sys-headline-medium);
    }
    .big {
      font: var(--mat-sys-headline-small);
    }
    .err {
      color: var(--mat-sys-error);
    }
  `,
})
export class AnalyticsDashboard {
  private readonly http = inject(HttpClient);
  readonly w = signal<Analytics['widget1'] | null>(null);
  readonly error = signal<string | null>(null);

  constructor() {
    this.http.get<Analytics>('legacy-data/dashboard/analytics/data.json').subscribe({
      next: (d) => this.w.set(d.widget1),
      error: (e: Error) => this.error.set(e.message),
    });
  }
}
