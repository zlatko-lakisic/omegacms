import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';

type Gantt = { data: { name: string; tasks: { name: string; from: string; to: string }[] }[] };

@Component({
  selector: 'app-gantt-page',
  imports: [MatCardModule, MatTableModule],
  template: `
    <mat-card>
      <mat-card-title>Gantt chart (sample)</mat-card-title>
      <mat-card-content>
        @if (err()) {
          <p class="e">{{ err() }}</p>
        } @else {
          <p class="h">{{ groupName() }}</p>
          <table mat-table [dataSource]="rows()" class="gantt">
            <ng-container matColumnDef="name">
              <th mat-header-cell *matHeaderCellDef>Task</th>
              <td mat-cell *matCellDef="let r">{{ r.name }}</td>
            </ng-container>
            <ng-container matColumnDef="from">
              <th mat-header-cell *matHeaderCellDef>From</th>
              <td mat-cell *matCellDef="let r">{{ r.from.substring(0, 10) }}</td>
            </ng-container>
            <ng-container matColumnDef="to">
              <th mat-header-cell *matHeaderCellDef>To</th>
              <td mat-cell *matCellDef="let r">{{ r.to.substring(0, 10) }}</td>
            </ng-container>
            <tr mat-header-row *matHeaderRowDef="cols"></tr>
            <tr mat-row *matRowDef="let r; columns: cols"></tr>
          </table>
        }
      </mat-card-content>
    </mat-card>
  `,
  styles: `
    .gantt {
      width: 100%;
    }
    .h {
      margin-bottom: 0.5rem;
      font: var(--mat-sys-title-large);
    }
    .e {
      color: var(--mat-sys-error);
    }
  `,
})
export class GanttPage {
  private readonly http = inject(HttpClient);
  readonly cols = ['name', 'from', 'to'];
  readonly rows = signal<{ name: string; from: string; to: string }[]>([]);
  readonly groupName = signal('');
  readonly err = signal<string | null>(null);

  constructor() {
    this.http.get<Gantt>('legacy-data/gantt-chart/tasks.json').subscribe({
      next: (j) => {
        const g = j.data?.[0];
        this.groupName.set(g?.name ?? '');
        this.rows.set((g?.tasks ?? []).slice(0, 12).map((t) => ({ name: t.name, from: t.from, to: t.to })));
      },
      error: (e: Error) => this.err.set(e.message),
    });
  }
}
