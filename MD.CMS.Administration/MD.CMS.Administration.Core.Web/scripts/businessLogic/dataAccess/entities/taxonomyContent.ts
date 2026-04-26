/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/BaseEntity.ts" />
namespace mdBusinessLogic {
    export namespace dataAccess {
        export namespace entities {
            export class taxonomyContent extends base.BaseEntity implements base.IBaseEntity<taxonomyContent>{
                public LCID: number;
                public DateCreated: Date;
                public TaxonomyId: number;
                public Title: string;
                public Type: string;
                public Path: string;

                constructor(obj?: taxonomyContent) {
                    super(obj);
                    this.LCID = 0;
                    this.DateCreated = new Date();
                    this.TaxonomyId = 0;
                    this.Title = '';
                    this.Type = '';
                    this.Path = '';
                    if (obj != undefined && obj != null) {
                        this.construct(obj);
                    }
                }

                public construct(data: any) {
                    super.construct(data);
                    this.LCID = this.getValue<number>(data, 'LCID', 0);
                    this.DateCreated = this.getValue<Date>(data, 'DateCreated', null);
                    this.TaxonomyId = this.getValue<number>(data, 'TaxonomyId', 0);
                    this.Title = this.getValue<string>(data, 'Title', '');
                    this.Type = this.getValue<string>(data, 'Type', '');
                    this.Path = this.getValue<string>(data, 'Path', '');
                }

                public clone(): taxonomyContent {
                    return new taxonomyContent(this);
                }
            }
        }
    }
}