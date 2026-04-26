/// <reference path="./user.ts" />
namespace mdBusinessLogic {
    export namespace dataAccess {
        export namespace entities {
            export class loggedOnUser extends user {
                public SessionId: string = '';

                constructor(obj?: loggedOnUser) {
                    super(obj);
                    this.SessionId = '';
                    if (obj != undefined && obj != null) {
                        this.construct(obj);
                    }
                }

                public construct(data: any) {
                    super.construct(data);
                    this.SessionId = this.getValue<string>(data, 'SessionId', '');
                }

                public clone(): loggedOnUser {
                    return new loggedOnUser(this);
                }

                public toString(): string {
                    return JSON.stringify({
                        SessionId: this.SessionId,
                        Id: this.Id,
                        Username: this.Username,
                        Token: this.Token,
                        DateRefreshToken: this.DateRefreshToken
                    });
                }
            }
        }
    }
}