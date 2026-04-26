/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/BaseEntity.ts" />
namespace mdBusinessLogic.dataAccess.entities {
    export abstract class editable implements base.IBaseEntity<editable> {
        public Edit: boolean;
        constructor(edit: boolean) {
            this.Edit = edit;
        }
        public construct(data: any): void {
            this.Edit = base.BaseEntity.getValue<boolean>(data, 'Edit', false);
        }

        public clone(): editable {
            return this;
        }
    }

    export class length extends editable implements base.IBaseEntity<length> {
        public Length: number;
        constructor(obj?: length) {
            super(false);
            this.Length = 1;
            if (obj != undefined && obj != null) {
                super(obj.Edit);
                this.construct(obj);
            }
        }
        public construct(data: any): void {
            super.construct(data);
            this.Length = base.BaseEntity.getValue<number>(data, 'Length', 1);
        }

        public clone(): length {
            return new length(this);
        }
    }

    export class casing extends editable implements base.IBaseEntity<casing> {
        public UpperCase: boolean;
        public LowerCase: boolean;
        constructor(obj?: casing) {
            super(false);
            this.UpperCase = true;
            this.LowerCase = true;
            if (obj != undefined && obj != null) {
                super(obj.Edit);
                this.construct(obj);
            }
        }
        public construct(data: any): void {
            super.construct(data);
            this.UpperCase = base.BaseEntity.getValue<boolean>(data, 'UpperCase', true);
            this.LowerCase = base.BaseEntity.getValue<boolean>(data, 'LowerCase', true);
        }

        public clone(): casing {
            return new casing(this);
        }
    }

    export class specialCharacters extends editable implements base.IBaseEntity<specialCharacters> {
        public Included: Array<string>;
        constructor(obj?: specialCharacters) {
            super(false);
            this.Included = new Array<string>();
            if (obj != undefined && obj != null) {
                super(obj.Edit);
                this.construct(obj);
            }
        }
        public construct(data: any): void {
            super.construct(data);
            this.Included = base.BaseEntity.getValue<Array<string>>(data, 'Included', new Array<string>());
        }

        public clone(): specialCharacters {
            return new specialCharacters(this);
        }
    }

    export class numbers extends editable implements base.IBaseEntity<numbers> {
        public From: number;
        public To: number;
        constructor(obj?: numbers) {
            super(false);
            this.From = 0;
            this.To = 1;
            if (obj != undefined && obj != null) {
                super(obj.Edit);
                this.construct(obj);
            }
        }

        public construct(data: any): void {
            super.construct(data);
            this.From = base.BaseEntity.getValue<number>(data, 'From', 0);
            this.To = base.BaseEntity.getValue<number>(data, 'To', 1);
        }

        public clone(): numbers {
            return new numbers(this);
        }
    }

    export class characterTypes extends editable implements base.IBaseEntity<characterTypes> {
        public Letters: boolean;
        public Casing: casing;
        public SpecialCharacters: specialCharacters;
        public Numbers: numbers;

        constructor(obj?: characterTypes) {
            super(false);
            this.Casing = new casing();
            this.SpecialCharacters = new specialCharacters();
            this.Numbers = new numbers();
            if (obj != undefined && obj != null) {
                super(obj.Edit)
                this.construct(obj);
            }
        }

        public construct(data: any): void {
            super.construct(data);
            this.Casing = base.BaseEntity.getConstructValue<casing>(data, 'Casing', new casing());
            this.SpecialCharacters = base.BaseEntity.getConstructValue<specialCharacters>(data, 'SpecialCharacters', new specialCharacters());
            this.Numbers = base.BaseEntity.getConstructValue<numbers>(data, 'Numbers', new numbers());
        }

        public clone(): characterTypes {
            return new characterTypes(this);
        }
    }

    export class email extends editable implements base.IBaseEntity<email> {
        public Domain: string;
        public Extension: string;

        constructor(obj?: email) {
            super(false);
            this.Domain = '';
            this.Extension = '';
            if (obj != undefined && obj != null) {
                super(obj.Edit);
                this.construct(obj);
            }
        }

        public construct(data: any): void {
            super.construct(data);
            this.Domain = base.BaseEntity.getValue<string>(data, 'Domain', '');
            this.Extension = base.BaseEntity.getValue<string>(data, 'Extension', '');
        }

        public clone(): email {
            return new email(this);
        }
    }

    export class webAddress extends editable implements base.IBaseEntity<webAddress> {
        public Includes: Array<string>;
        public Protocols: Array<string>;

        constructor(obj?: webAddress) {
            super(false);
            this.Includes = new Array<string>();
            this.Protocols = new Array<string>();
            if (obj != undefined && obj != null) {
                super(obj.Edit)
                this.construct(obj);
            }
        }

        public construct(data: any): void {
            super.construct(data);
            this.Includes = base.BaseEntity.getValue<Array<string>>(data, 'Includes', new Array<string>());
            this.Protocols = base.BaseEntity.getValue<Array<string>>(data, 'Protocols', new Array<string>());
        }

        public clone(): webAddress {
            return new webAddress(this);
        }
    }

    export class typeValidation extends editable implements base.IBaseEntity<typeValidation> {
        public Email: email;
        public WebAddress: webAddress;
        constructor(obj?: typeValidation) {
            super(false);
            this.Email = new email();
            this.WebAddress = new webAddress();
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }
        public construct(data: any): void {
            super.construct(data);
            this.Email = base.BaseEntity.getConstructValue<email>(data, 'Email', new email());
            this.WebAddress = base.BaseEntity.getConstructValue<webAddress>(data, 'WebAddress', new webAddress());
        }

        public clone(): typeValidation {
            return new typeValidation(this);
        }
    }

    export class fieldValidation implements base.IBaseEntity<fieldValidation>{
        public MinLength: length;
        public MaxLength: length;
        public CharacterTypes: characterTypes;
        public TypeValidation: typeValidation;
        public Regex: string;
        public Required: boolean;
        public Repeatable: boolean;

        constructor(obj?: fieldValidation) {
            this.MinLength = new length();
            this.MaxLength = new length();
            this.CharacterTypes = new characterTypes();
            this.TypeValidation = new typeValidation();
            this.Regex = '';
            this.Required = false;
            this.Repeatable = false;
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any) {
            this.MinLength = base.BaseEntity.getConstructValue<length>(data, 'MinLength', new length());
            this.MaxLength = base.BaseEntity.getConstructValue<length>(data, 'MaxLength', new length());
            this.CharacterTypes = base.BaseEntity.getConstructValue<characterTypes>(data, 'CharacterTypes', new characterTypes());
            this.TypeValidation = base.BaseEntity.getConstructValue<typeValidation>(data, 'TypeValidation', new typeValidation());
            this.Regex = base.BaseEntity.getValue<string>(data, 'Regex', '');
            this.Required = base.BaseEntity.getValue<boolean>(data, 'Required', false);
            this.Repeatable = base.BaseEntity.getValue<boolean>(data, 'Repeatable', false);
        }

        public clone(): fieldValidation {
            return new fieldValidation(this);
        }
    }
}
