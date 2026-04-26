namespace mdBusinessLogic.dataAccess.entities.generic {

    export class extendedDateTime implements base.IBaseEntity<extendedDateTime> {
        public value;
        public timezone;
        public maxDateTime;
        public minDateTime;

        constructor(data?: any) {
            this.maxDateTime = null;
            this.minDateTime = null;
            if (data) {
                this.construct(data);
            }
        }

        public toDate(): Date {
            return helpers.entityHelper.parseDateValue(this.toString());
        }

        public toString(): string {
            return helpers.entityHelper.parseDateAndTimezoneToString(this.value, this.timezone);
        }

        public construct(data: any): void {
            this.value = helpers.entityHelper.parseDateStringValue(data);
            this.timezone = helpers.entityHelper.parseTimeZoneValue(data);
        }

        public clone(): extendedDateTime {
            return new extendedDateTime(this);
        }
        
    }
}
