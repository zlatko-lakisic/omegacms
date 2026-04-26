import { HttpClient } from '@angular/common/http';
import { Component, computed, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleChange, MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';

type RangeKey = 'TW' | 'LW' | '2W';

type Pt = { x: string; y: number };

type MainChartEntry = { key: string; values: Record<RangeKey, Pt[]> };
type SupEntry = { label: string; count: Record<RangeKey, number> };
type SupKey = 'fixed' | 'wontFix' | 'reOpened' | 'needsTest' | 'created' | 'closed';

type Widget1 = {
  data?: { label?: string; count?: { DY: number; DT: number; DTM: number } };
};

type WData = { title: string; data?: { count: number; extra?: { count: number; label: string } } };
type W6 = {
  mainChart: { label: string; values: Record<RangeKey, number> }[];
  footerLeft?: { title: string; count: Record<RangeKey, number> };
  footerRight?: { title: string; count: Record<RangeKey, number> };
};

type W7 = {
  schedule: {
    T: { title: string; time: string; location?: string }[];
  };
};

type ProjectPayload = {
  projects?: { name: string }[];
  widget1?: Widget1;
  widget2?: WData;
  widget3?: WData;
  widget4?: WData;
  widget5?: { mainChart: MainChartEntry[]; supporting: Record<SupKey, SupEntry> };
  widget6?: W6;
  widget7?: W7;
};

type ChartBuild = {
  bars: { x: number; y: number; w: number; h: number }[];
  xLabels: string[];
  linePoints: { x: number; y: number }[];
  dayCenters: number[];
};

const SUP_ORDER: { k: SupKey; label: string }[] = [
  { k: 'fixed', label: 'Fixed' },
  { k: 'wontFix', label: "Won't fix" },
  { k: 'reOpened', label: 'Re-opened' },
  { k: 'needsTest', label: 'Needs triage' },
  { k: 'created', label: 'In progress' },
  { k: 'closed', label: 'Closed' },
];

@Component({
  selector: 'app-project-dashboard',
  imports: [
    MatCardModule,
    MatListModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatIconModule,
  ],
  templateUrl: './project-dashboard.html',
  styleUrl: './project-dashboard.scss',
})
export class ProjectDashboard {
  private readonly http = inject(HttpClient);

  readonly data = signal<ProjectPayload | null>(null);
  readonly error = signal<string | null>(null);
  readonly range = signal<RangeKey>('TW');

  private readonly w = computed(() => this.data() ?? undefined);

  readonly w1 = computed(() => this.w()?.widget1);
  readonly w2 = computed(() => this.w()?.widget2);
  readonly w3 = computed(() => this.w()?.widget3);
  readonly w4 = computed(() => this.w()?.widget4);
  readonly w6 = computed(() => this.w()?.widget6);
  readonly w7 = computed(() => this.w()?.widget7);

  readonly w1Label = computed(() => this.w1()?.data?.label ?? 'DUE TASKS');
  readonly w1Value = computed(() => {
    const c = this.w1()?.data?.count;
    if (c == null) {
      return '—';
    }
    return String(c.DT);
  });

  /** Trends vs prior bucket / yesterday — mirrors Fuse-style KPI sublines. */
  readonly k1Good = computed(() => {
    const c = this.w1()?.data?.count;
    if (!c) {
      return true;
    }
    return c.DT <= c.DY;
  });
  readonly k1Icon = computed(() => (this.k1Good() ? 'trending_down' : 'trending_up'));
  readonly k1Delta = computed(() => {
    const c = this.w1()?.data?.count;
    if (!c) {
      return '';
    }
    const d = c.DT - c.DY;
    if (d === 0) {
      return 'Same as yesterday';
    }
    return `${d > 0 ? '+' : ''}${d} since yesterday`;
  });

  readonly k2Good = computed(() => {
    const w = this.w2();
    if (!w?.data) {
      return true;
    }
    return w.data.count <= (w.data.extra?.count ?? 99);
  });
  readonly k2Icon = computed(() => (this.k2Good() ? 'trending_down' : 'trending_up'));
  readonly k2Delta = computed(() => {
    const w = this.w2();
    if (!w?.data) {
      return '';
    }
    const ex = w.data.extra?.count;
    if (ex === undefined) {
      return '';
    }
    const d = w.data.count - ex;
    return `${d > 0 ? '+' : ''}${d} vs ${w.data.extra?.label?.toLowerCase() ?? 'prior'}`;
  });

  readonly k3Good = computed(() => (this.w3()?.data?.extra?.count ?? 0) > 0);
  readonly k3Icon = computed(() =>
    this.k3Good() ? 'trending_down' : this.w3()?.data?.extra?.count === 0 ? 'trending_flat' : 'trending_up',
  );
  readonly k3Delta = computed(() => {
    const w = this.w3();
    if (!w?.data) {
      return '';
    }
    return `${w.data.extra?.count ?? 0} closed today`;
  });

  readonly k4Good = computed(() => (this.w4()?.data?.extra?.count ?? 0) > 0);
  readonly k4Icon = computed(() => (this.k4Good() ? 'trending_up' : 'trending_flat'));
  readonly k4Delta = computed(() => {
    const w = this.w4();
    if (!w?.data) {
      return '';
    }
    return `${w.data.extra?.count ?? 0} implemented`;
  });

  readonly chartLayout = computed((): ChartBuild | null => {
    const d = this.w();
    const r = this.range();
    const mc = d?.widget5?.mainChart;
    if (!mc || mc.length < 2) {
      return null;
    }
    const issues = mc[1].values[r];
    const closed = mc[0].values[r];
    if (!issues?.length || !closed?.length) {
      return null;
    }
    const n = issues.length;
    const maxY = Math.max(1, ...issues.map((p) => p.y), ...closed.map((p) => p.y)) * 1.08;
    const vbW = 700;
    const top = 28;
    const bottom = 24;
    const left = 40;
    const right = 20;
    const innerW = vbW - left - right;
    const innerH = 200 - top - bottom;
    const colW = innerW / n;
    const barW = colW * 0.42;
    const bars: ChartBuild['bars'] = [];
    const linePoints: { x: number; y: number }[] = [];
    const dayCenters: number[] = [];
    for (let i = 0; i < n; i++) {
      const cx = left + i * colW + colW / 2;
      dayCenters.push(cx);
      const yL = (closed[i]!.y / maxY) * innerH;
      linePoints.push({ x: cx, y: top + innerH - yL });
      const barH = (issues[i]!.y / maxY) * innerH;
      const bx = left + i * colW + (colW - barW) / 2;
      bars.push({ x: bx, y: top + innerH - barH, w: barW, h: barH });
    }
    return { bars, xLabels: issues.map((p) => p.x), linePoints, dayCenters };
  });

  readonly linePoints = computed(() => this.chartLayout()?.linePoints ?? []);
  readonly linePointsStr = computed(
    () => this.linePoints().map((p) => `${p.x},${p.y}`).join(' ') + '',
  );

  /** Horizontal grid (behind the chart) */
  readonly gridLines = computed(() => {
    const innerH = 200 - 28 - 24;
    const top = 28;
    return [0, 0.25, 0.5, 0.75, 1].map((k) => top + innerH * k);
  });

  readonly supportRows = computed(() => {
    const d = this.w();
    const r = this.range();
    const sup = d?.widget5?.supporting;
    if (!sup) {
      return [] as { key: string; label: string; value: number; isTotal: boolean }[];
    }
    const rows: { key: string; label: string; value: number; isTotal: boolean }[] = [];
    let tot = 0;
    for (const o of SUP_ORDER) {
      const c = sup[o.k]?.count[r];
      if (c === undefined) {
        continue;
      }
      tot += c;
      rows.push({ key: o.k, label: o.label, value: c, isTotal: false });
    }
    rows.push({ key: 'total', label: 'Total', value: tot, isTotal: true });
    return rows;
  });

  readonly taskDistBars = computed(() => {
    const w6 = this.w6();
    const r = this.range();
    if (!w6?.mainChart?.length) {
      return [] as { label: string; value: number; pct: number }[];
    }
    const slice = w6.mainChart.map((m) => ({
      label: m.label,
      value: m.values[r] ?? 0,
    }));
    const max = Math.max(1, ...slice.map((s) => s.value));
    return slice.map((s) => ({ ...s, pct: (s.value / max) * 100 }));
  });

  readonly scheduleItems = computed(() => {
    const t = this.w7()?.schedule?.T;
    return (t ?? []).slice(0, 6);
  });

  constructor() {
    this.load();
  }

  onRangeChange(e: MatButtonToggleChange) {
    const v = e.value as RangeKey;
    if (v === 'TW' || v === 'LW' || v === '2W') {
      this.range.set(v);
    }
  }

  dayLabelX(i: number): number {
    return this.chartLayout()?.dayCenters[i] ?? 0;
  }

  private load() {
    this.http
      .get<ProjectPayload>('legacy-data/dashboard/project/data.json')
      .subscribe({
        next: (d) => this.data.set(d),
        error: (e: Error) => this.error.set(e.message),
      });
  }
}
