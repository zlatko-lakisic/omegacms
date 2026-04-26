import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';

type Cl = { data: { title: string; url: string; excerpt: string }[] };

@Component({
  selector: 'app-search-page',
  imports: [MatCardModule, MatListModule],
  template: `
    <mat-card>
      <mat-card-title>Search (classic results)</mat-card-title>
      <mat-card-content>
        @if (err()) {
          <p class="e">{{ err() }}</p>
        } @else {
          <mat-list>
            @for (r of rows(); track r.url) {
              <mat-list-item>
                <span matListItemTitle>{{ r.title }}</span>
                <span matListItemLine class="url">{{ r.url }}</span>
                <span matListItemLine>{{ r.excerpt }}</span>
              </mat-list-item>
            }
          </mat-list>
        }
      </mat-card-content>
    </mat-card>
  `,
  styles: `
    .url {
      color: var(--mat-sys-primary);
    }
    .e {
      color: var(--mat-sys-error);
    }
  `,
})
export class SearchPage {
  private readonly http = inject(HttpClient);
  readonly rows = signal<Cl['data']>([]);
  readonly err = signal<string | null>(null);

  constructor() {
    this.http.get<Cl>('legacy-data/search/classic.json').subscribe({
      next: (c) => this.rows.set(c.data ?? []),
      error: (e: Error) => this.err.set(e.message),
    });
  }
}
