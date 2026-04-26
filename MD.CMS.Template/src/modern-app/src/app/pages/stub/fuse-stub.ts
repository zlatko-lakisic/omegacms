import { Component, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { ActivatedRoute } from '@angular/router';
import { map } from 'rxjs';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-fuse-stub',
  imports: [MatCardModule],
  template: `
    <mat-card class="stub">
      <mat-card-title>{{ title() }}</mat-card-title>
      <mat-card-content>
        <p>Legacy AngularJS UI state: <code>{{ legacy() }}</code></p>
        <p class="hint">Port business logic and templates from the Fuse theme as needed.</p>
      </mat-card-content>
    </mat-card>
  `,
  styles: `
    .stub {
      max-width: 40rem;
    }
    .hint {
      opacity: 0.85;
      font: var(--mat-sys-body-medium);
    }
    code {
      font-family: ui-monospace, Consolas, monospace;
      font-size: 0.9em;
    }
  `,
})
export class FuseStubPage {
  private readonly route = inject(ActivatedRoute);
  readonly title = toSignal(this.route.data.pipe(map((d) => (d['label'] as string) ?? 'Page')), {
    initialValue: 'Page',
  });
  readonly legacy = toSignal(this.route.data.pipe(map((d) => (d['legacy'] as string) ?? '')), {
    initialValue: '',
  });
}
