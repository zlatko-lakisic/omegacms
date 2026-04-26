/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./user.ts" />
/// <reference path="./message.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class messageFolder extends base.BaseEntity implements base.IBaseEntity<messageFolder> {
        public Name: string;
        public Icon: string;
        public Author: user;
        public IsGlobal: boolean;
        public Messages: Array<message>;
        public MessagesCount: number;

        constructor(obj?: messageFolder) {
            super(obj);
            this.Name = '';
            this.Icon = ''
            this.Author = new user();
            this.IsGlobal = false;
            this.Messages = new Array<message>();
            this.MessagesCount = 0;
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.Name = this.getValue<string>(data, "Name", '');
            this.Icon = this.getValue<string>(data, "Icon", '');
            this.Author = this.getConstructEntityValue<user>(data, "Author", new user());
            this.IsGlobal = this.getValue<boolean>(data, "IsGlobal", false);
            this.Messages = this.getArrayConstructEntityValue<message>(data, "Messages", new Array<message>(), new message());
            this.MessagesCount = this.getValue<number>(data, "MessagesCount", 0);
        }

        public clone(): messageFolder {
            return new messageFolder(this);
        }

    }
}