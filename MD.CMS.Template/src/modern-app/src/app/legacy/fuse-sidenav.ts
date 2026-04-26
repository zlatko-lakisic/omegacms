import { ChangeDetectionStrategy, Component } from '@angular/core';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink, RouterLinkActive } from '@angular/router';
import {
  FUSE_MENU_SECTIONS,
  AmElementGroups,
  AmLayoutItems,
  pathToLink,
  amElementLink,
  amLayoutLink,
} from './fuse-sidenav.data';

@Component({
  selector: 'app-fuse-sidenav',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [MatExpansionModule, MatListModule, MatIconModule, RouterLink, RouterLinkActive],
  template: `
    <mat-accordion [multi]="true" class="nav max-w-[18rem]">
      <mat-nav-list>
        <a
          mat-list-item
          [routerLink]="['']"
          routerLinkActive="fuse-nav-active"
          [routerLinkActiveOptions]="{ exact: true }"
        >
          <mat-icon matListItemIcon>home</mat-icon>
          <span matListItemTitle>Home</span>
        </a>
      </mat-nav-list>
      @for (sec of fuseSections; track sec.name) {
        <mat-expansion-panel [expanded]="true" class="fuse-nav-panel">
          <mat-expansion-panel-header>
            <mat-panel-title>{{ sec.name }}</mat-panel-title>
          </mat-expansion-panel-header>
          <mat-nav-list>
            @for (item of sec.items; track item.path) {
              <a
                mat-list-item
                [routerLink]="pathToLink(item.path)"
                routerLinkActive="fuse-nav-active"
              >
                <mat-icon matListItemIcon>{{ item.icon }}</mat-icon>
                <span matListItemTitle>{{ item.label }}</span>
              </a>
            }
          </mat-nav-list>
        </mat-expansion-panel>
      }

      <mat-expansion-panel [expanded]="false" class="fuse-nav-panel">
        <mat-expansion-panel-header>
          <mat-panel-title>AngularJS Material (legacy)</mat-panel-title>
        </mat-expansion-panel-header>
        <mat-accordion [multi]="true" class="sub-acc">
          @for (g of amGroups; track g.label) {
            <mat-expansion-panel [expanded]="false" class="fuse-nav-sub">
              <mat-expansion-panel-header>
                <mat-panel-title>{{ g.label }}</mat-panel-title>
              </mat-expansion-panel-header>
              <mat-nav-list>
                @for (it of g.items; track it.slug) {
                  <a mat-list-item [routerLink]="amElementLink(g, it)">
                    <span matListItemTitle>{{ it.name }}</span>
                  </a>
                }
              </mat-nav-list>
            </mat-expansion-panel>
          }
          <mat-expansion-panel [expanded]="false" class="fuse-nav-sub">
            <mat-expansion-panel-header>
              <mat-panel-title>Layout (legacy)</mat-panel-title>
            </mat-expansion-panel-header>
            <mat-nav-list>
              @for (l of amLayouts; track l.slug) {
                <a mat-list-item [routerLink]="amLayoutLink(l)">
                  <span matListItemTitle>{{ l.name }}</span>
                </a>
              }
            </mat-nav-list>
          </mat-expansion-panel>
        </mat-accordion>
      </mat-expansion-panel>
    </mat-accordion>
  `,
  styles: `
    :host {
      display: block;
      color: inherit;
    }
  `,
})
export class FuseSidenav {
  readonly fuseSections = FUSE_MENU_SECTIONS;
  readonly amGroups = AmElementGroups;
  readonly amLayouts = AmLayoutItems;
  protected readonly pathToLink = pathToLink;
  protected readonly amElementLink = amElementLink;
  protected readonly amLayoutLink = amLayoutLink;
}
