import { Component, signal } from '@angular/core';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatCalendar, MatDatepickerModule } from '@angular/material/datepicker';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-calendar-page',
  imports: [MatCardModule, MatDatepickerModule, MatCalendar],
  providers: [provideNativeDateAdapter()],
  template: `
    <mat-card>
      <mat-card-title>Calendar</mat-card-title>
      <mat-card-content>
        <mat-calendar
          [selected]="selected()"
          (selectedChange)="selected.set($event)"
        />
      </mat-card-content>
    </mat-card>
  `,
  styles: `
    mat-card {
      max-width: 22rem;
    }
  `,
})
export class CalendarPage {
  readonly selected = signal<Date | null>(new Date());
}
