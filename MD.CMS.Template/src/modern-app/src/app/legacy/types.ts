export type NavKind =
  | 'stub'
  | 'mail'
  | 'calendar'
  | 'dashboardProject'
  | 'dashboardServer'
  | 'dashboardAnalytics'
  | 'scrumboard'
  | 'gantt'
  | 'fileManager'
  | 'todo'
  | 'profile'
  | 'search'
  | 'invoice';

export interface NavLeaf {
  path: string;
  kind: NavKind;
  label: string;
  legacy: string;
  icon: string;
}
