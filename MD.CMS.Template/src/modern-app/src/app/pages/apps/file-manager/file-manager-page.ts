import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';

type FM = { data: { path: string[]; folders: { name: string; type: string; owner: string }[] } };

@Component({
  selector: 'app-file-manager-page',
  imports: [MatCardModule, MatTableModule],
  template: `
    <mat-card>
      <mat-card-title>File manager</mat-card-title>
      <mat-card-content>
        @if (err()) {
          <p class="e">{{ err() }}</p>
        } @else {
          <p class="crumb">{{ crumb() }}</p>
          <table mat-table [dataSource]="rows()">
            <ng-container matColumnDef="name">
              <th mat-header-cell *matHeaderCellDef>Name</th>
              <td mat-cell *matCellDef="let r">{{ r.name }}</td>
            </ng-container>
            <ng-container matColumnDef="type">
              <th mat-header-cell *matHeaderCellDef>Type</th>
              <td mat-cell *matCellDef="let r">{{ r.type }}</td>
            </ng-container>
            <ng-container matColumnDef="owner">
              <th mat-header-cell *matHeaderCellDef>Owner</th>
              <td mat-cell *matCellDef="let r">{{ r.owner }}</td>
            </ng-container>
            <tr mat-header-row *matHeaderRowDef="cols"></tr>
            <tr mat-row *matRowDef="let r; columns: cols"></tr>
          </table>
        }
      </mat-card-content>
    </mat-card>
  `,
  styles: `
    .crumb {
      margin-bottom: 0.5rem;
      font: var(--mat-sys-title-medium);
    }
    .e {
      color: var(--mat-sys-error);
    }
  `,
})
export class FileManagerPage {
  private readonly http = inject(HttpClient);
  readonly cols = ['name', 'type', 'owner'];
  readonly rows = signal<FM['data']['folders']>([]);
  readonly crumb = signal('');
  readonly err = signal<string | null>(null);

  constructor() {
    this.http.get<FM>('legacy-data/file-manager/documents.json').subscribe({
      next: (d) => {
        this.rows.set(d.data?.folders?.slice(0, 20) ?? []);
        this.crumb.set((d.data?.path ?? []).join(' › '));
      },
      error: (e: Error) => this.err.set(e.message),
    });
  }
}
