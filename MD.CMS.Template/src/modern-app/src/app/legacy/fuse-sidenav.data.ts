import { FUSE_LEAVES } from './fuse-routes.data';
import { AmElementGroups, AmLayoutItems } from './material-legacy.data';
import type { NavLeaf } from './types';

const pf = (p: string) => (l: NavLeaf) => l.path.startsWith(p);

export const FUSE_MENU_SECTIONS: { name: string; items: NavLeaf[] }[] = [
  { name: 'APPS', items: FUSE_LEAVES.filter(pf('apps/')) },
  { name: 'PAGES', items: FUSE_LEAVES.filter(pf('pages/')) },
  { name: 'UI', items: FUSE_LEAVES.filter(pf('ui/')) },
  { name: 'COMPONENTS', items: FUSE_LEAVES.filter(pf('components/')) },
];

export { AmElementGroups, AmLayoutItems };

/** Build router array for a legacy nav path, e.g. "apps/mail" → ['apps', 'mail'] */
export function pathToLink(path: string): string[] {
  return path.split('/').filter(Boolean);
}

export function amElementLink(g: { group: string }, item: { slug: string }): string[] {
  return ['am-legacy', g.group, item.slug];
}

export function amLayoutLink(item: { slug: string }): string[] {
  return ['am-legacy', 'layout', item.slug];
}
