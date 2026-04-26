import { Component, computed, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { ActivatedRoute } from '@angular/router';
import { map } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { AmElementGroups, AmLayoutItems } from '../../legacy/material-legacy.data';

function findMeta(section: string, item: string): { name: string; state: string } | null {
  if (section === 'layout') {
    const f = AmLayoutItems.find((x) => x.slug === item);
    return f ? { name: f.name, state: f.state } : null;
  }
  const g = AmElementGroups.find((x) => x.group === section);
  const i = g?.items.find((t) => t.slug === item);
  return i ? { name: i.name, state: i.state } : null;
}

@Component({
  selector: 'app-material-legacy-page',
  imports: [MatCardModule],
  template: `
    <mat-card>
      <mat-card-title>{{ label() }}</mat-card-title>
      <mat-card-content>
        <p>
          This screen mapped the <strong>AngularJS Material 1.x</strong> style guide demo in Fuse. The modern app uses
          <strong>Angular Material (M3)</strong> — reimplement the interaction with current components when you need
          parity.
        </p>
        <p>Legacy <code>ui-sref</code> / state: <code>{{ st() ?? '—' }}</code></p>
        @if (!st()) {
          <p class="warn">Unknown section or slug for <code>am-legacy</code> route.</p>
        }
      </mat-card-content>
    </mat-card>
  `,
  styles: `
    p {
      font: var(--mat-sys-body-large);
    }
    code {
      font-family: ui-monospace, Consolas, monospace;
    }
    .warn {
      color: var(--mat-sys-error);
    }
  `,
})
export class MaterialLegacyPage {
  private readonly route = inject(ActivatedRoute);
  private readonly meta = toSignal(
    this.route.paramMap.pipe(
      map((p) => findMeta(p.get('section') ?? '', p.get('item') ?? '')),
    ),
    { initialValue: null as { name: string; state: string } | null },
  );
  readonly st = computed(() => this.meta()?.state ?? null);
  readonly label = computed(() => this.meta()?.name ?? 'AngularJS Material (legacy)');
}
