import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { MatCheckboxModule } from '@angular/material/checkbox';

type Tasks = { data: { title: string; completed: boolean; important: boolean }[] };

@Component({
  selector: 'app-todo-page',
  imports: [MatCardModule, MatListModule, MatCheckboxModule],
  template: `
    <mat-card>
      <mat-card-title>To-Do</mat-card-title>
      <mat-card-content>
        @if (err()) {
          <p class="e">{{ err() }}</p>
        } @else {
          <mat-list>
            @for (t of tasks(); track t.title) {
              <mat-list-item>
                <mat-checkbox [checked]="t.completed" disabled />
                <span matListItemTitle [class.important]="t.important">{{ t.title }}</span>
              </mat-list-item>
            }
          </mat-list>
        }
      </mat-card-content>
    </mat-card>
  `,
  styles: `
    .important {
      font-weight: 600;
    }
    .e {
      color: var(--mat-sys-error);
    }
  `,
})
export class TodoPage {
  private readonly http = inject(HttpClient);
  readonly tasks = signal<Tasks['data']>([]);
  readonly err = signal<string | null>(null);

  constructor() {
    this.http.get<Tasks>('legacy-data/todo/tasks.json').subscribe({
      next: (d) => this.tasks.set((d.data ?? []).slice(0, 25)),
      error: (e: Error) => this.err.set(e.message),
    });
  }
}
