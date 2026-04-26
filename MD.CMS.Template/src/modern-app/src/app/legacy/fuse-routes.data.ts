import type { NavKind, NavLeaf } from './types';

const s = (
  path: string,
  label: string,
  legacy: string,
  icon: string,
  kind: NavKind = 'stub',
): NavLeaf => ({ path, kind, label, legacy, icon });

const APPS: NavLeaf[] = [
  s('apps/dashboards/project', 'Project', 'app.dashboards_project', 'folder_open', 'dashboardProject'),
  s('apps/dashboards/server', 'Server', 'app.dashboards_server', 'dns', 'dashboardServer'),
  s('apps/dashboards/analytics', 'Analytics', 'app.dashboards_analytics', 'analytics', 'dashboardAnalytics'),
  s('apps/mail', 'Mail', 'app.mail', 'mail', 'mail'),
  s('apps/calendar', 'Calendar', 'app.calendar', 'event', 'calendar'),
  s('apps/file-manager', 'File manager', 'app.file-manager', 'folder', 'fileManager'),
  s('apps/scrumboard', 'Scrumboard', 'app.scrumboard', 'view_kanban', 'scrumboard'),
  s('apps/gantt-chart', 'Gantt chart', 'app.gantt-chart', 'calendar_view_month', 'gantt'),
  s('apps/todo', 'To-Do', 'app.to-do', 'checklist', 'todo'),
];

const PAGES: NavLeaf[] = [
  s('pages/auth/login', 'Login', 'app.pages_auth_login', 'login'),
  s('pages/auth/login-v2', 'Login v2', 'app.pages_auth_login-v2', 'login'),
  s('pages/auth/register', 'Register', 'app.pages_auth_register', 'person_add'),
  s('pages/auth/register-v2', 'Register v2', 'app.pages_auth_register-v2', 'person_add'),
  s('pages/auth/forgot-password', 'Forgot password', 'app.pages_auth_forgot-password', 'lock_open'),
  s('pages/auth/reset-password', 'Reset password', 'app.pages_auth_reset-password', 'lock'),
  s('pages/auth/lock', 'Lock', 'app.pages_auth_lock', 'lock'),
  s('pages/coming-soon', 'Coming soon', 'app.pages_coming-soon', 'hourglass_empty'),
  s('pages/errors/404', 'Error 404', 'app.pages_errors_error-404', 'error'),
  s('pages/errors/500', 'Error 500', 'app.pages_errors_error-500', 'error'),
  s('pages/invoice', 'Invoice', 'app.pages_invoice', 'receipt', 'invoice'),
  s('pages/maintenance', 'Maintenance', 'app.pages_maintenance', 'build'),
  s('pages/profile', 'Profile', 'app.pages_profile', 'person', 'profile'),
  s('pages/search', 'Search', 'app.pages_search', 'search', 'search'),
  // No standalone `pages/timeline` leaf: it conflicts with nested `pages/timeline/left|right` in build-tree.
  s('pages/timeline/left', 'Timeline (left)', 'app.pages_timeline_left', 'timeline'),
  s('pages/timeline/right', 'Timeline (right)', 'app.pages_timeline_right', 'timeline'),
];

const UI_BASIC: NavLeaf[] = [
  s('ui/forms', 'Forms', 'app.ui_forms', 'list_alt'),
  s('ui/icons', 'Icons', 'app.ui_icons', 'image'),
  s('ui/typography', 'Typography', 'app.ui_typography', 'text_fields'),
  s('ui/theme-colors', 'Theme colors', 'app.ui_theme-colors', 'palette'),
  s('ui/material-colors', 'Material colors', 'app.ui_material-colors', 'color_lens'),
];

const UI_PAGE_LAYOUTS: [string, string, string, string][] = [
  ['ui/page-layouts/carded-fullwidth', 'app.ui_page-layouts_carded_fullwidth', 'Carded — full width', 'view_quilt'],
  ['ui/page-layouts/carded-fullwidth-ii', 'app.ui_page-layouts_carded_fullwidth-ii', 'Carded — full width II', 'view_quilt'],
  ['ui/page-layouts/carded-left-sidenav', 'app.ui_page-layouts_carded_left-sidenav', 'Carded — left sidenav', 'view_quilt'],
  ['ui/page-layouts/carded-left-sidenav-ii', 'app.ui_page-layouts_carded_left-sidenav-ii', 'Carded — left sidenav II', 'view_quilt'],
  ['ui/page-layouts/carded-right-sidenav', 'app.ui_page-layouts_carded_right-sidenav', 'Carded — right sidenav', 'view_quilt'],
  ['ui/page-layouts/carded-right-sidenav-ii', 'app.ui_page-layouts_carded_right-sidenav-ii', 'Carded — right sidenav II', 'view_quilt'],
  ['ui/page-layouts/simple-fullwidth', 'app.ui_page-layouts_simple_fullwidth', 'Simple — full width', 'crop_free'],
  ['ui/page-layouts/simple-left-sidenav', 'app.ui_page-layouts_simple_left-sidenav', 'Simple — left sidenav', 'crop_free'],
  ['ui/page-layouts/simple-left-sidenav-ii', 'app.ui_page-layouts_simple_left-sidenav-ii', 'Simple — left sidenav II', 'crop_free'],
  ['ui/page-layouts/simple-right-sidenav', 'app.ui_page-layouts_simple_right-sidenav', 'Simple — right sidenav', 'crop_free'],
  ['ui/page-layouts/simple-right-sidenav-ii', 'app.ui_page-layouts_simple_right-sidenav-ii', 'Simple — right sidenav II', 'crop_free'],
  ['ui/page-layouts/simple-tabbed', 'app.ui_page-layouts_simple_tabbed', 'Simple — tabbed', 'tab'],
  ['ui/page-layouts/blank', 'app.ui_page-layouts_blank', 'Blank', 'inbox'],
];

const UI_LAYOUTS: NavLeaf[] = UI_PAGE_LAYOUTS.map(
  ([path, legacy, label, icon]) => s(path, label, legacy, icon, 'stub'),
);

const COMPONENTS: NavLeaf[] = [
  s('components/cards', 'Cards', 'app.components_cards', 'credit_card'),
  s('components/charts/c3', 'Charts — C3', 'app.components_charts_c3', 'show_chart'),
  s('components/charts/chart-js', 'Charts — Chart.js', 'app.components_charts_chart-js', 'show_chart'),
  s('components/charts/chartist', 'Charts — Chartist', 'app.components_charts_chartist', 'show_chart'),
  s('components/charts/nvd3', 'Charts — nvD3', 'app.components_charts_nvd3', 'show_chart'),
  s('components/maps', 'Maps', 'app.components_maps', 'map'),
  s('components/price-tables', 'Price tables', 'app.components_price-tables', 'sell'),
  s('components/tables/simple-table', 'Simple table', 'app.components_tables_simple-table', 'table_rows'),
  s('components/tables/datatable', 'Datatable', 'app.components_tables_datatable', 'table_chart'),
  s('components/widgets', 'Widgets', 'app.components_widgets', 'widgets'),
];

export const FUSE_LEAVES: ReadonlyArray<NavLeaf> = [
  ...APPS,
  ...PAGES,
  ...UI_BASIC,
  ...UI_LAYOUTS,
  ...COMPONENTS,
];
