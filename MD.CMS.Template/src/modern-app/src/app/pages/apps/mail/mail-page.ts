import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';

type Inbox = { data: { subject: string; from: { name: string; email: string }; time: string }[] };

@Component({
  selector: 'app-mail-page',
  imports: [MatCardModule, MatTableModule],
  template: `
    <mat-card>
      <mat-card-title>Mail</mat-card-title>
      <mat-card-content>
        @if (error()) {
          <p class="err">{{ error() }}</p>
        } @else {
          <table mat-table [dataSource]="rows()" class="mail-table">
            <ng-container matColumnDef="from">
              <th mat-header-cell *matHeaderCellDef>From</th>
              <td mat-cell *matCellDef="let r">{{ r.from }}</td>
            </ng-container>
            <ng-container matColumnDef="subject">
              <th mat-header-cell *matHeaderCellDef>Subject</th>
              <td mat-cell *matCellDef="let r">{{ r.subject }}</td>
            </ng-container>
            <ng-container matColumnDef="time">
              <th mat-header-cell *matHeaderCellDef>Time</th>
              <td mat-cell *matCellDef="let r">{{ r.time }}</td>
            </ng-container>
            <tr mat-header-row *matHeaderRowDef="cols"></tr>
            <tr mat-row *matRowDef="let row; columns: cols"></tr>
          </table>
        }
      </mat-card-content>
    </mat-card>
  `,
  styles: `
    .mail-table {
      width: 100%;
    }
    .err {
      color: var(--mat-sys-error);
    }
  `,
})
export class MailPage {
  private readonly http = inject(HttpClient);
  readonly cols = ['from', 'subject', 'time'];
  readonly rows = signal<{ from: string; subject: string; time: string }[]>([]);
  readonly error = signal<string | null>(null);

  constructor() {
    this.http.get<Inbox>('legacy-data/mail/inbox.json').subscribe({
      next: (j) => {
        this.rows.set(
          (j.data ?? []).map((m) => ({
            from: m.from?.name ?? m.from?.email ?? '',
            subject: m.subject ?? '',
            time: m.time ?? '',
          })),
        );
      },
      error: (e: Error) => this.error.set(e.message),
    });
  }
}
