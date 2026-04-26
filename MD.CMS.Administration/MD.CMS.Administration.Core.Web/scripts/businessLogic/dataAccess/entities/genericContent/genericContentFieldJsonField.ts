/// <reference path="../base/iBaseEntity.ts" />
/// <reference path="../base/baseEntity.ts" />
/// <reference path="../grid/gridTileData.ts" />
/// <reference path="../generic/genericKeyValuePair.ts" />
/// <reference path="../generic/keyValuePair.ts" />
/// <reference path="../generic/genericCollection.ts" />

namespace mdBusinessLogic.dataAccess.entities.genericContent {
    export class genericContentFieldJsonField implements base.IBaseEntity<genericContentFieldJsonField> {
        public validation: fieldValidation;
        public helpText: string;
        public access: string;
        public cssClass: string;
        public toggle: string;
        public hidden: boolean;
        public enabled: boolean;
        public gridTileData: mdBusinessLogic.dataAccess.entities.grid.gridTileData;
        public style: any;
        public metadata: Array<generic.keyValuePair>;
        public constraints: generic.genericCollection<iGenericContentFieldJsonFieldConstraint>;
        public linkToTitle: boolean;

        constructor(obj?: genericContentFieldJsonField) {
            this.validation = new fieldValidation();
            this.helpText = '';
            this.access = '';
            this.cssClass = '';
            this.toggle = '';
            this.hidden = false;
            this.enabled = true;
            this.gridTileData = new mdBusinessLogic.dataAccess.entities.grid.gridTileData();
            this.style = {};
            this.constraints = new generic.genericCollection<iGenericContentFieldJsonFieldConstraint>();
            this.metadata = new Array<generic.keyValuePair>();
            this.linkToTitle = false;
            if (obj != undefined && obj != null) {
                this.construct(obj);
                if (this.style === undefined || this.style == null) {
                    this.style = {};
                }
                if (this.constraints === undefined || this.constraints == null) {
                    this.constraints = new generic.genericCollection<iGenericContentFieldJsonFieldConstraint>();
                }
            }
        }

        public construct(data: any) {
            this.validation = helpers.entityHelper.getConstructValue<fieldValidation>(data, 'validation', new fieldValidation());
            this.helpText = helpers.entityHelper.getValue<string>(data, 'helpText', '');
            this.access = helpers.entityHelper.getValue<string>(data, 'access', '');
            this.cssClass = helpers.entityHelper.getValue<string>(data, 'cssClass', '');
            this.toggle = helpers.entityHelper.getValue<string>(data, 'toggle', '');
            this.hidden = helpers.entityHelper.getValue<boolean>(data, 'hidden', false);
            this.enabled = helpers.entityHelper.getValue<boolean>(data, 'enabled', true);
            this.gridTileData = helpers.entityHelper.getConstructValue<mdBusinessLogic.dataAccess.entities.grid.gridTileData>(data, 'gridTileData', new mdBusinessLogic.dataAccess.entities.grid.gridTileData());
            this.style = helpers.entityHelper.getValue<any>(data, 'style', {});
            this.constraints = helpers.entityHelper.getConstructValue<generic.genericCollection<iGenericContentFieldJsonFieldConstraint>>(data, 'constraints', new generic.genericCollection<iGenericContentFieldJsonFieldConstraint>());
            this.metadata = helpers.entityHelper.getValue<Array<generic.keyValuePair>>(data, 'metadata', new Array<generic.keyValuePair>());
            this.linkToTitle = helpers.entityHelper.getValue<boolean>(data, 'linkToTitle', false);
        }

        public clone(): genericContentFieldJsonField {
            return new genericContentFieldJsonField(this);
        }

        public getStyle(attributeType: entities.attributeTypeEnum): any {
            return this.style[entities.attributeTypeEnum[attributeType]];
        }

        public getConstraint(key: string): iGenericContentFieldJsonFieldConstraint {
            let constraint: iGenericContentFieldJsonFieldConstraint = this.constraints.get(key);
            if (constraint) {
                if (!constraint.contentIds) {
                    constraint.contentIds = [];
                }
                if (!constraint.contentTypeId) {
                    constraint.contentTypeId = '';
                }
                if (!constraint.folderPaths) {
                    constraint.folderPaths = [];
                }
                if (!constraint.menuPaths) {
                    constraint.menuPaths = [];
                }
                if (!constraint.profileId) {
                    constraint.profileId = '';
                }
                if (!constraint.taxonomyIds) {
                    constraint.taxonomyIds = [];
                }
                if (!constraint.userIds) {
                    constraint.userIds = [];
                }
                return constraint;
            }
            return null;
        }

        public getDefaultConstraint(): iGenericContentFieldJsonFieldConstraint {
            let constraint: iGenericContentFieldJsonFieldConstraint = this.getConstraint('default');
            if (!constraint) {
                constraint = {
                    contentIds: [],
                    contentTypeId: '',
                    folderPaths: [],
                    menuPaths: [],
                    profileId: '',
                    taxonomyIds: [],
                    userIds: []
                };
                this.constraints.add('default', constraint);
            } else {
                constraint.contentIds = constraint.contentIds.filter((c) => { return c.length; });
                constraint.folderPaths = constraint.folderPaths.filter((c) => { return c.length; });
                constraint.menuPaths = constraint.menuPaths.filter((c) => { return c.length; });
                constraint.taxonomyIds = constraint.taxonomyIds.filter((c) => { return c.length; });
                constraint.userIds = constraint.userIds.filter((c) => { return c.length; });
            }
            return constraint;
        }

        public setDefaultConstraint(value: iGenericContentFieldJsonFieldConstraint): void {
            this.constraints.add('default', value);
        }

        public getRelevantConstraint(): iGenericContentFieldJsonFieldConstraint {
            let constraint: iGenericContentFieldJsonFieldConstraint = this.getDefaultConstraint();
            return constraint;
        }
    }
}
