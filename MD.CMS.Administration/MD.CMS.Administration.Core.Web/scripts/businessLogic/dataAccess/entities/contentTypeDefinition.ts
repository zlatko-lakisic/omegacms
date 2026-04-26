/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/BaseEntity.ts" />
/// <reference path="./base/BaseEntity.ts" />
namespace mdBusinessLogic.dataAccess.entities {
    export class contentTypeDefinition<T extends genericContent.genericContentField & base.IBaseEntity<T>> extends base.BaseEntity implements base.IBaseEntity<contentTypeDefinition<T>>{
        public Name: string;
        public Description: string;
        public Fields: Array<T>;
        public Options: string;
        public JsonOptions: any;
        public IsEditable: boolean;
        public Icon: string;
        public DataSources: Array<contentTypeDataSource>;
        public Joins: Array<contentTypeDataSourceJoin>;
        private Instance: T;

        constructor(instance?: T, obj?: contentTypeDefinition<T>) {
            super(obj);
            this.Name = '';
            this.Description = '';
            this.Fields = new Array<T>();
            this.Options = '';
            this.JsonOptions = null;
            this.IsEditable = true;
            this.Icon = '';
            this.Instance = new contentTypeDefinitionField() as any;
            this.DataSources = new Array<contentTypeDataSource>();
            this.Joins = new Array<contentTypeDataSourceJoin>();
            if (instance !== undefined) {
                this.Instance = instance;
            }
            if (obj !== undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any) {
            super.construct(data);
            this.Name = this.getValue<string>(data, 'Name', '');
            this.Description = this.getValue<string>(data, 'Description', '');
            this.Fields = this.getArrayConstructEntityValue<T>(data, 'Fields', new Array<T>(), this.Instance);
            this.Options = this.getValue<string>(data, 'Options', '');
            this.JsonOptions = this.getValue<any>(data, 'JsonOptions', null);
            this.IsEditable = this.getValue<boolean>(data, 'IsEditable', true);
            this.Icon = this.getValue<string>(data, 'Icon', '');
            this.DataSources = this.getArrayConstructEntityValue<contentTypeDataSource>(data, 'DataSources', new Array<contentTypeDataSource>(), new contentTypeDataSource());
            this.Joins = this.getArrayConstructEntityValue<contentTypeDataSourceJoin>(data, 'Joins', new Array<contentTypeDataSourceJoin>(), new contentTypeDataSourceJoin());
        }

        public clone(): contentTypeDefinition<T> {
            return new contentTypeDefinition<T>(this.Instance as T, this);
        }

        public convertToFieldValue() {
            this.Fields = this.Fields.map((item: T): any => {
                let fieldValue: contentTypeDefinitionFieldValue = new contentTypeDefinitionFieldValue();
                fieldValue.construct(item);
                fieldValue.ContentTypeDefinitionFieldId = item.Id;
                return fieldValue;
            });
            return this;
        }

        public setFieldValue(value: string, fieldName: string): void {
            if (this.Fields != null) {
                for (var i in this.Fields) {
                    if (this.Fields[i].Name == fieldName) {
                        this.Fields[i]['Value'] = value;
                        break;
                    }
                }
            }
        }

        public getFieldValue(fieldName: string): string {
            if (this.Fields != null) {
                for (var i in this.Fields) {
                    if (this.Fields[i].Name == fieldName && this.Fields[i]['Value'] !== undefined) {
                        return this.Fields[i]['Value'];
                    }
                }
            }
            return null;
        }

        public getField(fieldName: string): T {
            if (this.Fields != null) {
                for (var i in this.Fields) {
                    if (this.Fields[i].Name == fieldName) {
                        return this.Fields[i];
                    }
                }
            }
            return null;
        }

        public hasLinkToTitle(): boolean {
            return this.Fields.filter((f) => { return f.JsonField.linkToTitle; }).length > 0;
        }

        public getLinkToTitle(): T {
            if (this.hasLinkToTitle()) {
                return this.Fields.filter((f) => { return f.JsonField.linkToTitle; })[0];
            }
            return null;
        }

        public setJsonOptions(jsonOptions: any): void {
            this.JsonOptions = jsonOptions;
            this.Options = JSON.stringify(jsonOptions);
        }
    }
}
