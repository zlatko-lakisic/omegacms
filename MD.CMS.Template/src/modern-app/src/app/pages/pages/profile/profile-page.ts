import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';

type About = { data: { general: { about: string; birthday: string }; work: { occupation: string } } };

@Component({
  selector: 'app-profile-page',
  imports: [MatCardModule, MatListModule],
  template: `
    @if (err()) {
      <p class="e">{{ err() }}</p>
    } @else {
      <h1 class="h">Profile</h1>
      <mat-card>
        <mat-card-title>About</mat-card-title>
        <mat-card-content>
          <p>
            {{ d()?.general?.about?.slice(0, 500) }}{{
              (d()?.general?.about?.length ?? 0) > 500 ? '…' : ''
            }}
          </p>
        </mat-card-content>
      </mat-card>
      <mat-card class="mt">
        <mat-card-title>Details</mat-card-title>
        <mat-card-content>
          <mat-list>
            <mat-list-item>
              <span matListItemTitle>Birthday</span>
              <span matListItemLine>{{ d()?.general?.birthday ?? '—' }}</span>
            </mat-list-item>
            <mat-list-item>
              <span matListItemTitle>Occupation</span>
              <span matListItemLine>{{ d()?.work?.occupation ?? '—' }}</span>
            </mat-list-item>
          </mat-list>
        </mat-card-content>
      </mat-card>
    }
  `,
  styles: `
    .h {
      font: var(--mat-sys-headline-medium);
    }
    .mt {
      margin-top: 1rem;
    }
    .e {
      color: var(--mat-sys-error);
    }
  `,
})
export class ProfilePage {
  private readonly http = inject(HttpClient);
  readonly d = signal<About['data'] | null>(null);
  readonly err = signal<string | null>(null);

  constructor() {
    this.http.get<About>('legacy-data/profile/about.json').subscribe({
      next: (a) => this.d.set(a.data),
      error: (e: Error) => this.err.set(e.message),
    });
  }
}
