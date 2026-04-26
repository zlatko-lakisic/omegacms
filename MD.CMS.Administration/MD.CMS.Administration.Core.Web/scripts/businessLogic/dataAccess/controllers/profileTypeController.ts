/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/profileType.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class profileTypeController extends base.BaseController<profileTypeController, entities.profileType | entities.primitiveType<number>> {

        constructor() {
            super('ProfileType/');
        }

        public getById(id: number, onSuccess: (obj: entities.profileType) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<profileTypeController, entities.profileType> = new base.AjaxMethodOptions<profileTypeController, entities.profileType>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetById', [id]);
            options.responseData = new entities.profileType();
            options.onSuccess = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByIdAndTransformExpression(id: number, transform: boolean, onSuccess: (obj: entities.profileType) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<profileTypeController, entities.profileType> = new base.AjaxMethodOptions<profileTypeController, entities.profileType>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByIdAndTransformExpression', [id, transform]);
            options.responseData = new entities.profileType();
            options.onSuccess = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAll(sort: string, onSuccess: (obj: Array<entities.profileType>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<profileTypeController, entities.profileType> = new base.AjaxMethodOptions<profileTypeController, entities.profileType>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetAll', [sort]);
            options.responseData = new entities.profileType();
            options.onSuccess = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByUser(userId: number, onSuccess: (obj: Array<entities.profileType>) => void, onError: (error: helpers.mdException) => void): void {
            userId = mdBusinessLogic.helpers.typeConversion.toInt(userId);

            let options: base.AjaxMethodOptions<profileTypeController, entities.profileType> = new base.AjaxMethodOptions<profileTypeController, entities.profileType>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByUser', [userId]);
            options.responseData = new entities.profileType();
            options.onSuccess = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAllWithPagination(paginationData: any, onSuccess: (obj: Array<entities.profileType>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<profileTypeController, entities.profileType> = new base.AjaxMethodOptions<profileTypeController, entities.profileType>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetAllWitPagination', paginationData);
            options.responseData = new entities.profileType();
            options.onSuccess = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAllCount(countData: any, onSuccess: (obj: entities.primitiveType<number>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<profileTypeController, entities.primitiveType<number>> = new base.AjaxMethodOptions<profileTypeController, entities.primitiveType<number>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetAllCount', countData);
            options.responseData = new entities.primitiveType<number>();
            options.onSuccess = (options: base.AjaxMethodOptions<profileTypeController, entities.primitiveType<number>>): void => {
                onSuccess(options.responseData.Value);
            }
            options.onError = (options: base.AjaxMethodOptions<profileTypeController, entities.primitiveType<number>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getNotBelonging(userId: number, onSuccess: (obj: Array<entities.profileType>) => void, onError: (error: helpers.mdException) => void): void {
            userId = mdBusinessLogic.helpers.typeConversion.toInt(userId);

            let options: base.AjaxMethodOptions<profileTypeController, entities.profileType> = new base.AjaxMethodOptions<profileTypeController, entities.profileType>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetNotBelonging', [userId]);
            options.responseData = new entities.profileType();
            options.onSuccess = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public save(profileType: entities.profileType, onSuccess: (obj: entities.profileType) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<profileTypeController, entities.profileType> = new base.AjaxMethodOptions<profileTypeController, entities.profileType>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.profileType();
            options.requestData = profileType;
            options.onSuccess = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public update(profileType: entities.profileType, onSuccess: (obj: entities.profileType) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<profileTypeController, entities.profileType> = new base.AjaxMethodOptions<profileTypeController, entities.profileType>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('UpdateProfileTypePermissionsByFolder');
            options.responseData = new entities.profileType();
            options.requestData = profileType;
            options.onSuccess = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public saveProfileTypeWithProfileTypeFieldValues(profileType: entities.profileType, onSuccess: (obj: entities.profileType) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<profileTypeController, entities.profileType> = new base.AjaxMethodOptions<profileTypeController, entities.profileType>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('SaveProfileTypeWithProfileTypeFieldValues');
            options.responseData = new entities.profileType();
            options.requestData = profileType;
            options.onSuccess = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public del(id: number, onSuccess: (obj: entities.profileType) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<profileTypeController, entities.profileType> = new base.AjaxMethodOptions<profileTypeController, entities.profileType>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete', [id]);
            options.responseData = new entities.profileType();
            options.onSuccess = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }

        public getAllProfileTypesWithPermissions(profileTypeData: any, onSuccess: (obj: Array<entities.profileType>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<profileTypeController, entities.profileType> = new base.AjaxMethodOptions<profileTypeController, entities.profileType>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetAllProfileTypesWithPermissions', profileTypeData);
            options.responseData = new entities.profileType();
            options.onSuccess = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public saveProfileTypePermissions(profileTypesData: Array<entities.profileType>, onSuccess: (obj: Array<entities.profileType>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<profileTypeController, entities.profileType> = new base.AjaxMethodOptions<profileTypeController, entities.profileType>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('SaveProfileTypePermissions');
            options.responseData = new entities.profileType();
            options.requestData = profileTypesData;
            options.onSuccess = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public search(searchData: any, onSuccess: (obj: Array<entities.profileType>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<profileTypeController, entities.profileType> = new base.AjaxMethodOptions<profileTypeController, entities.profileType>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('Search', searchData);
            options.responseData = new entities.profileType();
            options.onSuccess = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<profileTypeController, entities.profileType>): void => {
                onError(options.exception);
            }
            this._get(options);
        }
    }
}
