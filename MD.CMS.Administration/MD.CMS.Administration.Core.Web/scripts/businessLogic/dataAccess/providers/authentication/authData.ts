namespace mdBusinessLogic.dataAccess.providers.authentication {
    export class authData implements entities.base.IBaseEntity<authData> {
        public Values: any;
        public AuthenticationProviderName: string;

        public constructor(obj?: authData) {
            this.Values = {};
            this.AuthenticationProviderName = '';

            if (obj !== undefined && obj != null) {
                this.Values = obj.Values;
                this.AuthenticationProviderName = obj.AuthenticationProviderName;
            }
        }

        public construct(data: any): void {
            this.Values = helpers.entityHelper.getValue<any>(data, 'Values', {});
            this.AuthenticationProviderName = helpers.entityHelper.getValue<string>(data, 'AuthenticationProviderName', '');
        }
        public clone(): authData {
            return new authData(this);
        }

        public GetData<T>(key: string, defaultValue: any = ''): T {
            return mdBusinessLogic.helpers.entityHelper.getValue<T>(this.Values, key, defaultValue);
        }

        public SetData<T>(key: string, value: T): void {
            this.Values[key] = value;
        }
    }
}
