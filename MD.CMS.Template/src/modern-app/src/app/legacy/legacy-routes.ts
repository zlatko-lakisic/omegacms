import type { Route, Routes } from '@angular/router';
import type { NavLeaf } from './types';
import { FUSE_LEAVES } from './fuse-routes.data';
import { buildNestedRoutes } from './build-tree';

/** When a parent path has only deeper children (e.g. timeline/left, timeline/right), add a default child redirect. */
function injectDefaultChildRedirects(routes: Routes): Routes {
  return routes.map((r) => {
    if (r.path !== 'pages' || !r.children?.length) {
      return r;
    }
    return {
      ...r,
      children: r.children.map((c) => {
        if (c.path !== 'timeline' || !c.children?.length) {
          return c;
        }
        const hasLeft = c.children.some((x) => x.path === 'left');
        if (!hasLeft) {
          return c;
        }
        return {
          ...c,
          children: [{ path: '', pathMatch: 'full', redirectTo: 'left' }, ...c.children],
        };
      }),
    };
  });
}

function resolveLeaf(l: NavLeaf): Route {
  const data: Record<string, string> = { label: l.label, legacy: l.legacy };
  switch (l.kind) {
    case 'stub':
      return { loadComponent: () => import('../pages/stub/fuse-stub').then((m) => m.FuseStubPage), data };
    case 'mail':
      return { loadComponent: () => import('../pages/apps/mail/mail-page').then((m) => m.MailPage), data };
    case 'calendar':
      return { loadComponent: () => import('../pages/apps/calendar/calendar-page').then((m) => m.CalendarPage), data };
    case 'dashboardProject':
      return { loadComponent: () => import('../pages/apps/dashboards/project-dashboard').then((m) => m.ProjectDashboard), data };
    case 'dashboardServer':
      return { loadComponent: () => import('../pages/apps/dashboards/server-dashboard').then((m) => m.ServerDashboard), data };
    case 'dashboardAnalytics':
      return { loadComponent: () => import('../pages/apps/dashboards/analytics-dashboard').then((m) => m.AnalyticsDashboard), data };
    case 'scrumboard':
      return { loadComponent: () => import('../pages/apps/scrumboard/scrumboard-page').then((m) => m.ScrumboardPage), data };
    case 'gantt':
      return { loadComponent: () => import('../pages/apps/gantt/gantt-page').then((m) => m.GanttPage), data };
    case 'fileManager':
      return { loadComponent: () => import('../pages/apps/file-manager/file-manager-page').then((m) => m.FileManagerPage), data };
    case 'todo':
      return { loadComponent: () => import('../pages/apps/todo/todo-page').then((m) => m.TodoPage), data };
    case 'profile':
      return { loadComponent: () => import('../pages/pages/profile/profile-page').then((m) => m.ProfilePage), data };
    case 'search':
      return { loadComponent: () => import('../pages/pages/search/search-page').then((m) => m.SearchPage), data };
    case 'invoice':
      return { loadComponent: () => import('../pages/pages/invoice/invoice-page').then((m) => m.InvoicePage), data };
  }
}

const amLegacyRoute: Route = {
  path: 'am-legacy',
  children: [
    {
      path: ':section',
      children: [
        {
          path: ':item',
          loadComponent: () =>
            import('../pages/legacy/material-legacy-page').then((m) => m.MaterialLegacyPage),
        },
      ],
    },
  ],
};

export function buildFuseRoutes(): Routes {
  return [
    { path: '', pathMatch: 'full', loadComponent: () => import('../pages/home/home').then((m) => m.Home) },
    ...injectDefaultChildRedirects(buildNestedRoutes(FUSE_LEAVES, resolveLeaf)),
    amLegacyRoute,
  ];
}
