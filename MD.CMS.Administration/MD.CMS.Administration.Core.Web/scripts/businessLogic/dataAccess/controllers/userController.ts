/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/user.ts" />
/// <reference path="../entities/loggedOnUser.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />
namespace mdBusinessLogic.dataAccess.controllers {
    export class userController extends base.BaseController<userController, entities.user | providers.authentication.authData | entities.primitiveType<any> | entities.paginationEntity<entities.user>> {

        constructor() {
            super('User/');
        }

        public getAuthData(id: number, onSuccess: (obj: providers.authentication.authData) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<userController, providers.authentication.authData> = new base.AjaxMethodOptions<userController, providers.authentication.authData>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetAuthData', [id]);
            options.responseData = new providers.authentication.authData();
            options.onSuccess = (options: base.AjaxMethodOptions<userController, providers.authentication.authData>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<userController, providers.authentication.authData>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getById(id: number, onSuccess: (obj: entities.user) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<userController, entities.user> = new base.AjaxMethodOptions<userController, entities.user>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetById', [id]);
            options.responseData = new entities.user();
            options.onSuccess = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAllUserWithPermissions(usersData: any, onSuccess: (obj: Array<entities.user>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<userController, entities.user> = new base.AjaxMethodOptions<userController, entities.user>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetAllUserWithPermissions', usersData);
            options.responseData = new entities.user();
            options.onSuccess = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getOnlyNotAuthorizedUsers(usersData: any, onSuccess: (obj: Array<entities.user>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<userController, entities.user> = new base.AjaxMethodOptions<userController, entities.user>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetOnlyNotAuthorizedUsers', usersData);
            options.responseData = new entities.user();
            options.onSuccess = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onSuccess(options.responseDataArray); 
            }
            options.onError = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAll(onSuccess: (obj: Array<entities.user>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<userController, entities.user> = new base.AjaxMethodOptions<userController, entities.user>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetAll');
            options.responseData = new entities.user();
            options.onSuccess = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onSuccess(options.responseDataArray.filter((user) => { return user.Id != 0; }));
            }
            options.onError = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public paginationGetAll(paginationData: any, onSuccess: (obj: entities.paginationEntity<entities.user>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<userController, entities.paginationEntity<entities.user>> = new base.AjaxMethodOptions<userController, entities.paginationEntity<entities.user>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('PaginationGetAll', paginationData);
            options.responseData = new entities.paginationEntity<entities.user>(entities.user);
            options.onSuccess = (options: base.AjaxMethodOptions<userController, entities.paginationEntity<entities.user>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<userController, entities.paginationEntity<entities.user>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAllCount(countData: any, onSuccess: (obj: entities.primitiveType<number>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<userController, entities.primitiveType<number>> = new base.AjaxMethodOptions<userController, entities.primitiveType<number>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetAllCount', countData);
            options.responseData = new entities.primitiveType<number>();
            options.onSuccess = (options: base.AjaxMethodOptions<userController, entities.primitiveType<number>>): void => {
                onSuccess(options.responseData.Value);
            }
            options.onError = (options: base.AjaxMethodOptions<userController, entities.primitiveType<number>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public updateUserPermission(users: Array<entities.user>, onSuccess: (obj: entities.user) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<userController, entities.user> = new base.AjaxMethodOptions<userController, entities.user>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('UpdateUserPermissionsByFolder');
            options.responseData = new entities.user();
            options.requestData = users;
            options.onSuccess = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public save(user: entities.user, onSuccess: (obj: entities.user) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<userController, entities.user> = new base.AjaxMethodOptions<userController, entities.user>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.user();
            options.requestData = user;
            options.onSuccess = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public assignProfileTypeToUser(profileTypeId: number, userId: number, onSuccess: (obj: entities.user) => void, onError: (error: helpers.mdException) => void): void {
            profileTypeId = mdBusinessLogic.helpers.typeConversion.toInt(profileTypeId);
            userId = mdBusinessLogic.helpers.typeConversion.toInt(userId);

            let options: base.AjaxMethodOptions<userController, entities.user> = new base.AjaxMethodOptions<userController, entities.user>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('AssignProfileTypeToUser', [profileTypeId, userId]);
            options.responseData = new entities.user();
            options.onSuccess = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public del(userId: number, onSuccess: (obj: entities.user) => void, onError: (error: helpers.mdException) => void): void {
            userId = mdBusinessLogic.helpers.typeConversion.toInt(userId);

            let options: base.AjaxMethodOptions<userController, entities.user> = new base.AjaxMethodOptions<userController, entities.user>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete', [userId]);
            options.responseData = new entities.user();
            options.onSuccess = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }

        public login(username: string, password: string, onSuccess: (obj: entities.loggedOnUser) => void, onError: (error: helpers.mdException) => void): string {
            let options: base.AjaxMethodOptions<userController, entities.loggedOnUser> = new base.AjaxMethodOptions<userController, entities.loggedOnUser>();
            options.includeAuthHeader = false;
            options.address = this.getAddress('Login');
            options.responseData = new entities.loggedOnUser();
            options.requestData = new entities.user();
            options.requestData.Username = username;
            options.requestData.Password = password;
            options.requestData.Token = mdBusinessLogic.helpers.Guid.create().toString();
            options.onSuccess = (options: base.AjaxMethodOptions<userController, entities.loggedOnUser>): void => {
                mdBusinessLogic.globals.loggedOnUser = options.responseData;
                mdBusinessLogic.globals.loggedOnUser.Token = mdBusinessLogic.helpers.encoder.base64.encode(mdBusinessLogic.globals.loggedOnUser.Username + ':' + mdBusinessLogic.globals.loggedOnUser.SessionId);
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<userController, entities.loggedOnUser>): void => {
                onError(options.exception);
            }
            this._post(options);
            return options.getRequestId();
        }

        public loginAuthData(data: providers.authentication.authData, onSuccess: (obj: entities.loggedOnUser) => void, onError: (error: helpers.mdException) => void): string {
            let options: base.AjaxMethodOptions<userController, entities.loggedOnUser> = new base.AjaxMethodOptions<userController, entities.loggedOnUser>();
            options.includeAuthHeader = false;
            options.address = this.getAddress('LoginAuthData');
            options.responseData = new entities.loggedOnUser();
            options.requestData = data;
            options.onSuccess = (options: base.AjaxMethodOptions<userController, entities.loggedOnUser>): void => {
                mdBusinessLogic.globals.loggedOnUser = options.responseData;
                mdBusinessLogic.globals.loggedOnUser.Token = mdBusinessLogic.helpers.encoder.base64.encode(mdBusinessLogic.globals.loggedOnUser.Username + ':' + mdBusinessLogic.globals.loggedOnUser.SessionId);
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<userController, entities.loggedOnUser>): void => {
                onError(options.exception);
            }
            this._post(options);
            return options.getRequestId();
        }

        public logout(userLoggingOut: entities.loggedOnUser, onSuccess: () => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<userController, entities.loggedOnUser> = new base.AjaxMethodOptions<userController, entities.loggedOnUser>();
            options.includeAuthHeader = false;
            options.address = this.getAddress('Logout');
            options.responseData = new entities.loggedOnUser();
            options.requestData = userLoggingOut;
            options.onSuccess = (options: base.AjaxMethodOptions<userController, entities.loggedOnUser>): void => {
                if (onSuccess != undefined) {
                    onSuccess();
                }
                settings.ajax.connections.closeAll();
            }
            options.onError = (options: base.AjaxMethodOptions<userController, entities.loggedOnUser>): void => {
                if (onError != undefined) {
                    onError(options.exception);
                }
                settings.ajax.connections.closeAll();
            }
            this._post(options);
        }

        public getByToken(token: string, onSuccess: (obj: entities.user) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<userController, entities.user> = new base.AjaxMethodOptions<userController, entities.user>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByToken', [encodeURIComponent(token)]);
            options.responseData = new entities.user();
            options.onSuccess = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public resetAccount(username: string, onSuccess: (obj: entities.user) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<userController, entities.user> = new base.AjaxMethodOptions<userController, entities.user>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('ResetAccount');
            options.responseData = new entities.loggedOnUser();
            options.requestData = new entities.user();
            options.requestData.Username = username;
            options.onSuccess = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public saveUserPermissions(permissionsData: any, onSuccess: (obj: Array<entities.primitiveType<string>>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<userController, entities.primitiveType<string>> = new base.AjaxMethodOptions<userController, entities.primitiveType<string>>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.contentType = new base.AjaxMethodHeader('Content-Type', 'application/json; charset=UTF-8');
            options.address = this.getAddress('SaveUserPermissions');
            options.responseData = new entities.primitiveType<string>();
            options.requestData = permissionsData;
            options.onSuccess = (options: base.AjaxMethodOptions<userController, entities.primitiveType<string>>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<userController, entities.primitiveType<string>>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public updateUser(user: entities.user, onSuccess: (obj: Array<entities.user>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<userController, entities.user> = new base.AjaxMethodOptions<userController, entities.user>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('UpdateUser');
            options.responseData = new entities.user();
            options.requestData = user;
            options.onSuccess = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public updateAuthData(user: entities.user, onSuccess: (obj: entities.user) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<userController, entities.user> = new base.AjaxMethodOptions<userController, entities.user>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('UpdateAuthData');
            options.responseData = new entities.user();
            options.requestData = user;
            options.onSuccess = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public search(searchData: any, onSuccess: (obj: Array<entities.user>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<userController, entities.user> = new base.AjaxMethodOptions<userController, entities.user>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('Search', searchData);
            options.responseData = new entities.user();
            options.onSuccess = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public passwordReset(token: string, email: string, password: string, onSuccess: (obj: entities.user) => void, onError: (error: helpers.mdException) => string): void {
            let options: base.AjaxMethodOptions<userController, entities.user> = new base.AjaxMethodOptions<userController, entities.user>();
            options.includeAuthHeader = false;
            options.address = this.getAddress('PasswordReset', [token, email, password]);
            options.responseData = new entities.user();
            options.onSuccess = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<userController, entities.user>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public validateTokenSocket(requestId: string, token: string, onSuccess: (obj: entities.user, socket: WebSocket) => void, onClose: (socket: WebSocket) => void, onError: (error: helpers.mdException, socket: WebSocket) => void): string {
            let options: base.AjaxMethodOptions<userController, entities.user> = new base.AjaxMethodOptions<userController, entities.user>(requestId);
            options.includeAuthHeader = true;
            options.address = this.getAddress('ValidateTokenSocket');
            options.responseData = new entities.user();
            options.requestData = token;
            options.onSuccess = (response: base.AjaxMethodDataSocket<userController, entities.user>): void => {
                onSuccess(response.responseData, response.socket);
            }
            options.onClose = (response: base.AjaxMethodDataSocket<userController, entities.user>): void => {
                onClose(response.socket);
            }
            options.onError = (response: base.AjaxMethodDataSocket<userController, entities.user>): void => {
                onError(response.exception, response.socket);
            }
            this._socket(options);
            return options.getRequestId();
        }
    }
}