import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';

type Boards = { data: { name: string; id: string; uri: string }[] };

@Component({
  selector: 'app-scrumboard-page',
  imports: [MatCardModule, MatListModule],
  template: `
    <mat-card>
      <mat-card-title>Scrumboard</mat-card-title>
      <mat-card-content>
        @if (err()) {
          <p class="e">{{ err() }}</p>
        } @else {
          <mat-list>
            @for (b of boards(); track b.id) {
              <mat-list-item>
                <span matListItemTitle>{{ b.name }}</span>
                <span matListItemLine class="sub">{{ b.uri }}</span>
              </mat-list-item>
            }
          </mat-list>
        }
      </mat-card-content>
    </mat-card>
  `,
  styles: `
    .sub {
      opacity: 0.7;
    }
    .e {
      color: var(--mat-sys-error);
    }
  `,
})
export class ScrumboardPage {
  private readonly http = inject(HttpClient);
  readonly boards = signal<Boards['data']>([]);
  readonly err = signal<string | null>(null);

  constructor() {
    this.http.get<Boards>('legacy-data/scrumboard/board-list.json').subscribe({
      next: (j) => this.boards.set(j.data ?? []),
      error: (e: Error) => this.err.set(e.message),
    });
  }
}
