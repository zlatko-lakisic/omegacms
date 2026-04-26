/// <reference path="../base/iBaseEntity.ts" />
/// <reference path="../base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities.grid {
    export class gridTileData implements base.IBaseEntity<gridTileData> {
        public width: number;
        public width_medium: number;
        public width_small: number;
        public height: number;
        public height_medium: number;
        public height_small: number;
        public minWidth: number;
        public minHeight: number;
        public id: string;
        public parentId: string;
        public uniqueId: string;
        public index: number;
        public layout: gridTileLayout;
        public whiteframe: number;
        public layoutPadding: boolean;
        public layoutMargin: boolean;
        public layoutWrap: boolean;
        public x: number;
        public y: number;

        constructor(obj?: gridTileData) {
            this.width = 0;
            this.width_medium = 0;
            this.width_small = 0;
            this.height = 0;
            this.height_medium = 0;
            this.height_small = 0;
            this.minWidth = 10;
            this.minHeight = 100;
            this.id = mdBusinessLogic.helpers.Guid.create().toString();
            this.parentId = undefined;
            this.uniqueId = this.id;
            this.index = 0;
            this.layout = gridTileLayout.Row;
            this.whiteframe = 4;
            this.layoutPadding = true;
            this.layoutMargin = true;
            this.layoutWrap = true;
            this.x = 0;
            this.y = 0;
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any) {
            this.width = helpers.entityHelper.getValue<number>(data, 'width', 0);
            this.width_medium = helpers.entityHelper.getValue<number>(data, 'width_medium', 0);
            this.width_small = helpers.entityHelper.getValue<number>(data, 'width_small', 0);
            this.height = helpers.entityHelper.getValue<number>(data, 'height', 0);
            this.height_medium = helpers.entityHelper.getValue<number>(data, 'height_medium', 0);
            this.height_small = helpers.entityHelper.getValue<number>(data, 'height_small', 0);
            this.minWidth = helpers.entityHelper.getValue<number>(data, 'minWidth', this.minWidth);
            this.minHeight = helpers.entityHelper.getValue<number>(data, 'minHeight', this.minHeight);
            this.id = helpers.entityHelper.getValue<string>(data, 'id', '');
            if (this.id == '') {
                this.id = mdBusinessLogic.helpers.Guid.create().toString();
            }
            this.uniqueId = this.id;
            this.parentId = helpers.entityHelper.getValue<string>(data, 'parentId', '');
            if (this.parentId == '') {
                this.parentId = undefined;
            }
            this.index = helpers.entityHelper.getValue<number>(data, 'index', 0);
            this.layout = helpers.entityHelper.getValue<gridTileLayout>(data, 'layout', gridTileLayout.Row);
            this.whiteframe = helpers.entityHelper.getValue<number>(data, 'whiteframe', 0);
            this.layoutPadding = helpers.entityHelper.getValue<boolean>(data, 'layoutPadding', true);
            this.layoutMargin = helpers.entityHelper.getValue<boolean>(data, 'layoutMargin', true);
            this.layoutWrap = helpers.entityHelper.getValue<boolean>(data, 'layoutWrap', true);
            this.x = helpers.entityHelper.getValue<number>(data, 'x', 0);
            this.y = helpers.entityHelper.getValue<number>(data, 'y', 0);
        }

        public clone(): gridTileData {
            return new gridTileData(this);
        }

        public setMinHeight(val: number): void {
            this.minHeight = val;
        }

        public setMinWidth(val: number): void {
            this.minWidth = val;
        }

        public getWidth(size: string): number {
            if (size === undefined || size == '') {
                size = '';
            } else {
                size = '_' + size;
            }

            let value = this['width' + size];
            if (value <= 10) {
                value = value * 10;
            }

            if (value > this.minWidth) {
                return value;
            }
            return this.minWidth;
        }

        public getHeight(size: string): number {
            if (size === undefined || size == '') {
                size = '';
            } else {
                size = '_' + size;
            }

            let value = this['height' + size];
            if (value <= 10) {
                value = value * 10;
            }

            if (value > this.minHeight) {
                return value;
            }
            return this.minHeight;
        }

        public setWidth(width: number, size: string) {
            if (isNaN(width)) {
                return;
            }

            if (size === undefined || size == '') {
                size = '';
            } else {
                size = '_' + size;
            }

            if (width < this.minWidth) {
                this['width' + size] = this.minWidth;
            } else {
                this['width' + size] = width;
            }
        }

        public setHeight(height: number, size: string) {
            if (isNaN(height)) {
                return;
            }
            if (size === undefined || size == '') {
                size = '';
            } else {
                size = '_' + size;
            }

            if (height < this.minHeight) {
                this['height' + size] = this.minHeight;
            } else {
                this['height' + size] = height;
            }
        }
    }

    export enum gridTileLayout {
        Row = 1,
        Column = 2
    }
}
