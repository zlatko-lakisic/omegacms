import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { MatTableModule } from '@angular/material/table';

type Inv = {
  data: {
    number: string;
    date: string;
    dueDate: string;
    from: { title: string; email: string };
    client: { title: string; email: string };
    services: { title: string; total: string }[];
  };
};

@Component({
  selector: 'app-invoice-page',
  imports: [MatCardModule, MatListModule, MatTableModule],
  template: `
    @if (err()) {
      <p class="e">{{ err() }}</p>
    } @else {
      <h1 class="h">Invoice {{ inv()?.number }}</h1>
      <p class="meta">Date {{ inv()?.date }} — Due {{ inv()?.dueDate }}</p>
      <div class="grid">
        <mat-card>
          <mat-card-title>From</mat-card-title>
          <mat-card-content>
            <p>{{ inv()?.from?.title }}</p>
            <p class="m">{{ inv()?.from?.email }}</p>
          </mat-card-content>
        </mat-card>
        <mat-card>
          <mat-card-title>Client</mat-card-title>
          <mat-card-content>
            <p>{{ inv()?.client?.title }}</p>
            <p class="m">{{ inv()?.client?.email }}</p>
          </mat-card-content>
        </mat-card>
      </div>
      <mat-card class="mt">
        <mat-card-title>Line items</mat-card-title>
        <mat-card-content>
          <table mat-table [dataSource]="rows()">
            <ng-container matColumnDef="title">
              <th mat-header-cell *matHeaderCellDef>Service</th>
              <td mat-cell *matCellDef="let r">{{ r.title }}</td>
            </ng-container>
            <ng-container matColumnDef="total">
              <th mat-header-cell *matHeaderCellDef>Total</th>
              <td mat-cell *matCellDef="let r">{{ r.total }}</td>
            </ng-container>
            <tr mat-header-row *matHeaderRowDef="cols"></tr>
            <tr mat-row *matRowDef="let r; columns: cols"></tr>
          </table>
        </mat-card-content>
      </mat-card>
    }
  `,
  styles: `
    .h {
      font: var(--mat-sys-headline-medium);
    }
    .meta {
      opacity: 0.8;
    }
    .grid {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 1rem;
    }
    @media (max-width: 800px) {
      .grid {
        grid-template-columns: 1fr;
      }
    }
    .m {
      opacity: 0.85;
    }
    .mt {
      margin-top: 1rem;
    }
    .e {
      color: var(--mat-sys-error);
    }
  `,
})
export class InvoicePage {
  private readonly http = inject(HttpClient);
  readonly inv = signal<Inv['data'] | null>(null);
  readonly rows = signal<Inv['data']['services']>([]);
  readonly cols = ['title', 'total'];
  readonly err = signal<string | null>(null);

  constructor() {
    this.http.get<Inv>('legacy-data/invoice/invoice.json').subscribe({
      next: (d) => {
        this.inv.set(d.data);
        this.rows.set(d.data?.services ?? []);
      },
      error: (e: Error) => this.err.set(e.message),
    });
  }
}
