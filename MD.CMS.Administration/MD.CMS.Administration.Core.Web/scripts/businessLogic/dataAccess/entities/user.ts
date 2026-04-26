/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/BaseEntity.ts" />
namespace mdBusinessLogic.dataAccess.entities {
    export class user extends base.BaseEntity implements base.IBaseEntity<user>{
        public Username: string;
        public ProfileTypes: Array<profileType>;
        public ProfileTypeId: number;
        public Token: string;
        public DateRefreshToken: Date;
        public RWDPermissions: Array<any>;
        public AdministrationAllowed: boolean;
        public IsRoot: boolean;
        public AuthenticationProvider: string;
        public ReferenceId: string;

        constructor(obj?: user) {
            super(obj);
            this.Username = '';
            this.ProfileTypes = new Array<any>();
            this.ProfileTypeId = 0;
            this.Token = '';
            this.DateRefreshToken = new Date();
            this.RWDPermissions = new Array<any>();
            this.AdministrationAllowed = false;
            this.IsRoot = false;
            this.AuthenticationProvider = '';
            this.ReferenceId = '';
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any) {
            super.construct(data);
            this.Username = this.getValue<string>(data, 'Username', '');
            this.ProfileTypes = this.getArrayConstructEntityValue<profileType>(data, 'ProfileTypes', new Array<profileType>(), new profileType());
            this.ProfileTypeId = this.getValue<number>(data, 'ProfileTypeId', 0);
            this.Token = this.getValue<string>(data, 'Token', '');
            this.DateRefreshToken = this.getValue<Date>(data, 'DateRefresh', new Date());
            this.RWDPermissions = this.getValue<Array<any>>(data, 'RWDPermissions', new Array<any>());
            this.AdministrationAllowed = this.getValue<boolean>(data, 'AdministrationAllowed', false);
            this.IsRoot = this.getValue<boolean>(data, 'IsRoot', false);
            this.AuthenticationProvider = this.getValue<string>(data, 'AuthenticationProvider', '');
            this.ReferenceId = this.getValue<string>(data, 'ReferenceId', '');
        }

        public clone(): user {
            return new user(this);
        }

        public getProfileType(query: any): profileType {
            if (isNaN(query)) {
                return this.ProfileTypes.filter(profile => { return profile.Name == query; })[0];
            }
            return this.ProfileTypes.filter(profile => { return profile.Id == query })[0];
        }
    }
}