import type { Route, Routes } from '@angular/router';
import type { NavLeaf } from './types';

type TreeNode = Map<string, TreeValue>;
type TreeValue = TreeNode | (Omit<Route, 'path'> & { _leaf: true });

function isTreeNode(v: TreeValue): v is TreeNode {
  return v instanceof Map;
}

export function addLeavesToTree(leaves: ReadonlyArray<NavLeaf>, resolveRoute: (leaf: NavLeaf) => Route): TreeNode {
  const root: TreeNode = new Map();
  for (const leaf of leaves) {
    const segs = leaf.path.split('/').filter((s) => s.length > 0);
    if (segs.length === 0) continue;
    let cur = root;
    for (let i = 0; i < segs.length; i++) {
      const key = segs[i]!;
      const isLast = i === segs.length - 1;
      if (isLast) {
        const r = resolveRoute(leaf);
        cur.set(key, { _leaf: true, ...r } as TreeValue);
      } else {
        const existing = cur.get(key);
        if (existing === undefined) {
          const next = new Map<string, TreeValue>();
          cur.set(key, next);
          cur = next;
        } else if (isTreeNode(existing)) {
          cur = existing;
        } else {
          throw new Error(`Route conflict: "${leaf.path}" — "${key}" is already a page`);
        }
      }
    }
  }
  return root;
}

function treeToRoutes(m: TreeNode): Routes {
  const routes: Routes = [];
  for (const [key, val] of m) {
    if (isTreeNode(val)) {
      routes.push({ path: key, children: treeToRoutes(val) });
    } else {
      if (!('_leaf' in val) || !val._leaf) {
        throw new Error('Invalid node');
      }
      const { _leaf: _, ...rest } = val;
      routes.push({ path: key, ...rest });
    }
  }
  return routes;
}

export function buildNestedRoutes(
  leaves: ReadonlyArray<NavLeaf>,
  resolveRoute: (leaf: NavLeaf) => Route,
): Routes {
  const tree = addLeavesToTree(leaves, resolveRoute);
  return treeToRoutes(tree);
}

