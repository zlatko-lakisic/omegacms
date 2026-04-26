/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./user.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class message extends base.BaseEntity implements base.IBaseEntity<message> {
        public Subject: string;
        public MessageContent: string;
        public ParentId: number;
        public IsRead: boolean;
        public MessageFolderId: number;
        public DateAdded: Date;
        public Type: number;
        public FromUserId: number;
        public ToUserId: number;
        public FromUser: user;
        public ToUser: user;
        public MainThread: number;

        constructor(obj?: message) {
            super(obj);
            this.Subject = '';
            this.MessageContent = '';
            this.ParentId = 0;
            this.IsRead = false;
            this.MessageFolderId = 0;
            this.DateAdded = null;
            this.Type = 0;
            this.FromUserId = 0;
            this.ToUserId = 0;
            this.FromUser = new user();
            this.ToUser = new user();
            this.MainThread = 0;
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.Subject = this.getValue<string>(data, "Subject", '');
            this.MessageContent = this.getValue<string>(data, "MessageContent", '');
            this.ParentId = this.getValue<number>(data, "ParentId", 0);
            this.IsRead = this.getValue<boolean>(data, "IsRead", false);
            this.MessageFolderId = this.getValue<number>(data, "MessageFolderId", 0);
            this.DateAdded = this.getValue<Date>(data, "DateAdded", null);
            this.Type = this.getValue<number>(data, "Type", 0);
            this.FromUserId = this.getValue<number>(data, "FromUserId", 0);
            this.ToUserId = this.getValue<number>(data, "ToUserId", 0);
            this.FromUser = this.getConstructEntityValue<user>(data, "FromUser", new user());
            this.ToUser = this.getConstructEntityValue<user>(data, "ToUser", new user());
            this.MainThread = this.getValue<number>(data, "MainThread", 0);
        }

        public clone(): message {
            return new message(this);
        }

    }
}