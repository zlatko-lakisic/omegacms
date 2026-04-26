/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/permissions/profileTypePermissions.ts" />
/// <reference path="../entities.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class permissionControllerProfileType extends base.BaseController<permissionControllerProfileType, entities.permissions.profileTypePermissions> {

        constructor() {
            super('Permissions/');
        }

        public getProfileTypePermissionsByObject(object: entities.entitiesEnum, objectId: number, onSuccess: (obj: Array<entities.permissions.profileTypePermissions>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<permissionControllerProfileType, entities.permissions.profileTypePermissions> = new base.AjaxMethodOptions<permissionControllerProfileType, entities.permissions.profileTypePermissions>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetProfileTypePermissionsByObject', [object, objectId]);
            options.responseData = new entities.permissions.profileTypePermissions();
            options.isJsonArray = true;
            options.onSuccess = (options: base.AjaxMethodOptions<permissionControllerProfileType, entities.permissions.profileTypePermissions>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<permissionControllerProfileType, entities.permissions.profileTypePermissions>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getProfileTypePermissionsByEntity(entity: entities.entitiesEnum, entityId: number, onSuccess: (obj: Array<entities.permissions.profileTypePermissions>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<permissionControllerProfileType, entities.permissions.profileTypePermissions> = new base.AjaxMethodOptions<permissionControllerProfileType, entities.permissions.profileTypePermissions>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetProfileTypePermissionsByEntity', [entity, entityId]);
            options.responseData = new entities.permissions.profileTypePermissions();
            options.isJsonArray = true;
            options.onSuccess = (options: base.AjaxMethodOptions<permissionControllerProfileType, entities.permissions.profileTypePermissions>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<permissionControllerProfileType, entities.permissions.profileTypePermissions>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getProfileTypePermissionsByEntities(entity: entities.entitiesEnum, entityIds: Array<number>, onSuccess: (obj: Array<entities.permissions.profileTypePermissions>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<permissionControllerProfileType, entities.permissions.profileTypePermissions> = new base.AjaxMethodOptions<permissionControllerProfileType, entities.permissions.profileTypePermissions>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetProfileTypePermissionsByEntities', [entity, entityIds.join('-')]);
            options.responseData = new entities.permissions.profileTypePermissions();
            options.isJsonArray = true;
            options.onSuccess = (options: base.AjaxMethodOptions<permissionControllerProfileType, entities.permissions.profileTypePermissions>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<permissionControllerProfileType, entities.permissions.profileTypePermissions>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public savePermissions(permissions: Array<entities.permissions.profileTypePermissions>, onSuccess: (obj: Array<entities.permissions.profileTypePermissions>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<permissionControllerProfileType, entities.permissions.profileTypePermissions> = new base.AjaxMethodOptions<permissionControllerProfileType, entities.permissions.profileTypePermissions>();
            options.includeAuthHeader = true;
            options.contentType = new base.AjaxMethodHeader('Content-Type', 'application/json; charset=UTF-8');
            options.address = this.getAddress('SaveProfileTypePermissionsByObject');
            options.responseData = new entities.permissions.profileTypePermissions();
            options.isJsonArray = true;
            options.requestData = permissions;
            options.onSuccess = (options: base.AjaxMethodOptions<permissionControllerProfileType, entities.permissions.profileTypePermissions>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<permissionControllerProfileType, entities.permissions.profileTypePermissions>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public getLoggedOnProfileTypePermissionsSocket(requestId: string, token: string, onSuccess: (obj: Array<entities.permissions.profileTypePermissions>, socket: WebSocket) => void, onError: (error: helpers.mdException, socket: WebSocket) => void): string {
            let options: base.AjaxMethodOptions<permissionControllerProfileType, entities.permissions.profileTypePermissions> = new base.AjaxMethodOptions<permissionControllerProfileType, entities.permissions.profileTypePermissions>(requestId);
            options.includeAuthHeader = true;
            options.address = this.getAddress('ProfileTypePermissionsSocket');
            options.responseData = new entities.permissions.profileTypePermissions();
            options.requestData = token;
            options.isJsonArray = true;
            options.onSuccess = (response: base.AjaxMethodDataSocket<permissionControllerProfileType, entities.permissions.profileTypePermissions>): void => {
                onSuccess(response.responseDataArray, response.socket);
            }
            options.onError = (response: base.AjaxMethodDataSocket<permissionControllerProfileType, entities.permissions.profileTypePermissions>): void => {
                onError(response.exception, response.socket);
            }
            this._socket(options);
            return options.getRequestId();
        }

        public getLoggedOnProfileTypePermissions(onSuccess: (obj: Array<entities.permissions.profileTypePermissions>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<permissionControllerProfileType, entities.permissions.profileTypePermissions> = new base.AjaxMethodOptions<permissionControllerProfileType, entities.permissions.profileTypePermissions>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetAllPermissionsByProfileType');
            options.responseData = new entities.permissions.profileTypePermissions();
            options.isJsonArray = true;
            options.onSuccess = (response: base.AjaxMethodDataSocket<permissionControllerProfileType, entities.permissions.profileTypePermissions>): void => {
                onSuccess(response.responseDataArray);
            }
            options.onError = (response: base.AjaxMethodDataSocket<permissionControllerProfileType, entities.permissions.profileTypePermissions>): void => {
                onError(response.exception);
            }
            this._get(options);
        }
    }
}
