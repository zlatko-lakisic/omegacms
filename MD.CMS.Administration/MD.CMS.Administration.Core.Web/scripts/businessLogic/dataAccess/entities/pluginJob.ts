/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class pluginJob extends base.BaseEntity implements base.IBaseEntity<pluginJob> {
        public PluginName: string;
        public Message: string;
        public StartedOn: Date;

        constructor(obj?: pluginJob) {
            super(obj);
            this.PluginName = '';
            this.Message = '';
            this.StartedOn = null;
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.PluginName = this.getValue<string>(data, "PluginName", '');
            this.Message = this.getValue<string>(data, "Message", '');
            this.StartedOn = this.getValue<Date>(data, "StartedOn", null);
        }

        public clone(): pluginJob {
            return new pluginJob(this);
        }

    }
}