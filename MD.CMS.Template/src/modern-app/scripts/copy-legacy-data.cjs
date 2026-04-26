/* Copies demo JSON from the legacy AngularJS app (sibling `../app/data`) into `public/legacy-data`. */
const fs = require('fs');
const path = require('path');

const src = path.join(__dirname, '../../app/data');
const dest = path.join(__dirname, '../public/legacy-data');

if (!fs.existsSync(src)) {
  console.warn('copy-legacy-data: source not found, skip:', src);
  process.exit(0);
}
fs.mkdirSync(path.dirname(dest), { recursive: true });
fs.cpSync(src, dest, { recursive: true });
console.log('copy-legacy-data: synced to', dest);
