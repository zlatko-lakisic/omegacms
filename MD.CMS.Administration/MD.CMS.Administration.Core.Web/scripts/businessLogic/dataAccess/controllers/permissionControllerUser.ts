/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/permissions/userPermissions.ts" />
/// <reference path="../entities.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class permissionControllerUser extends base.BaseController<permissionControllerUser, entities.permissions.userPermissions> {

        constructor() {
            super('Permissions/');
        }

        public getUserPermissionsByObject(object: entities.entitiesEnum, objectId: number, onSuccess: (obj: Array<entities.permissions.userPermissions>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<permissionControllerUser, entities.permissions.userPermissions> = new base.AjaxMethodOptions<permissionControllerUser, entities.permissions.userPermissions>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetUserPermissionssByObject', [object, objectId]);
            options.responseData = new entities.permissions.userPermissions();
            options.isJsonArray = true;
            options.onSuccess = (options: base.AjaxMethodOptions<permissionControllerUser, entities.permissions.userPermissions>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<permissionControllerUser, entities.permissions.userPermissions>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getUserPermissionsByEntity(entity: entities.entitiesEnum, entityId: number, onSuccess: (obj: Array<entities.permissions.userPermissions>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<permissionControllerUser, entities.permissions.userPermissions> = new base.AjaxMethodOptions<permissionControllerUser, entities.permissions.userPermissions>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetUserPermissionssByEntity', [entity, entityId]);
            options.responseData = new entities.permissions.userPermissions();
            options.isJsonArray = true;
            options.onSuccess = (options: base.AjaxMethodOptions<permissionControllerUser, entities.permissions.userPermissions>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<permissionControllerUser, entities.permissions.userPermissions>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getUserPermissionsByEntities(entity: entities.entitiesEnum, entityIds: Array<number>, onSuccess: (obj: Array<entities.permissions.userPermissions>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<permissionControllerUser, entities.permissions.userPermissions> = new base.AjaxMethodOptions<permissionControllerUser, entities.permissions.userPermissions>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetUserPermissionsByEntities', [entity, entityIds.join('-')]);
            options.responseData = new entities.permissions.userPermissions();
            options.isJsonArray = true;
            options.onSuccess = (options: base.AjaxMethodOptions<permissionControllerUser, entities.permissions.userPermissions>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<permissionControllerUser, entities.permissions.userPermissions>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public savePermissions(permissions: Array<entities.permissions.userPermissions>, onSuccess: (obj: Array<entities.permissions.userPermissions>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<permissionControllerUser, entities.permissions.userPermissions> = new base.AjaxMethodOptions<permissionControllerUser, entities.permissions.userPermissions>();
            options.includeAuthHeader = true;
            options.contentType = new base.AjaxMethodHeader('Content-Type', 'application/json; charset=UTF-8');
            options.address = this.getAddress('SaveUserPermissionsByObject');
            options.responseData = new entities.permissions.userPermissions();
            options.isJsonArray = true;
            options.requestData = permissions;
            options.onSuccess = (options: base.AjaxMethodOptions<permissionControllerUser, entities.permissions.userPermissions>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<permissionControllerUser, entities.permissions.userPermissions>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public getLoggedOnUserPermissionsSocket(requestId: string, token: string, onSuccess: (obj: Array<entities.permissions.userPermissions>, socket: WebSocket) => void, onError: (error: helpers.mdException, socket: WebSocket) => void): string {
            let options: base.AjaxMethodOptions<permissionControllerUser, entities.permissions.userPermissions> = new base.AjaxMethodOptions<permissionControllerUser, entities.permissions.userPermissions>(requestId);
            options.includeAuthHeader = true;
            options.address = this.getAddress('UserPermissionsSocket');
            options.responseData = new entities.permissions.userPermissions();
            options.requestData = token;
            options.isJsonArray = true;
            options.onSuccess = (response: base.AjaxMethodDataSocket<permissionControllerUser, entities.permissions.userPermissions>): void => {
                onSuccess(response.responseDataArray, response.socket);
            }
            options.onError = (response: base.AjaxMethodDataSocket<permissionControllerUser, entities.permissions.userPermissions>): void => {
                onError(response.exception, response.socket);
            }
            this._socket(options);
            return options.getRequestId();
        }

        public getLoggedOnUserPermissions(onSuccess: (obj: Array<entities.permissions.userPermissions>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<permissionControllerUser, entities.permissions.userPermissions> = new base.AjaxMethodOptions<permissionControllerUser, entities.permissions.userPermissions>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetAllPermissionsByUser');
            options.responseData = new entities.permissions.userPermissions();
            options.isJsonArray = true;
            options.onSuccess = (response: base.AjaxMethodDataSocket<permissionControllerUser, entities.permissions.userPermissions>): void => {
                onSuccess(response.responseDataArray);
            }
            options.onError = (response: base.AjaxMethodDataSocket<permissionControllerUser, entities.permissions.userPermissions>): void => {
                onError(response.exception);
            }
            this._get(options);
        }
    }
}
