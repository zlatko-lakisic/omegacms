/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/message.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class messageController extends base.BaseController<messageController, entities.message | entities.paginationEntity<entities.message>> {

        constructor() {
            super('Message/');
        }

        public getByIdAndUserId(id: number, onSuccess: (obj: entities.message) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<messageController, entities.message> = new base.AjaxMethodOptions<messageController, entities.message>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByIdAndUserId', [id]);
            options.responseData = new entities.message();
            options.onSuccess = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAll(onSuccess: (obj: Array<entities.message>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<messageController, entities.message> = new base.AjaxMethodOptions<messageController, entities.message>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetAll');
            options.responseData = new entities.message();
            options.onSuccess = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByMessageFolder(messageFolderId: number, onSuccess: (obj: Array<entities.message>) => void, onError: (error: helpers.mdException) => void): void {
            messageFolderId = mdBusinessLogic.helpers.typeConversion.toInt(messageFolderId);

            let options: base.AjaxMethodOptions<messageController, entities.message> = new base.AjaxMethodOptions<messageController, entities.message>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByMessageFolder', [messageFolderId]);
            options.responseData = new entities.message();
            options.onSuccess = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByMessageFolderAndUser(data: any, onSuccess: (obj: entities.paginationEntity<entities.message>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<messageController, entities.paginationEntity<entities.message>> = new base.AjaxMethodOptions<messageController, entities.paginationEntity<entities.message>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByMessageFolderAndUserWithPagination', data);
            options.responseData = new entities.paginationEntity<entities.message>(entities.message);
            options.onSuccess = (options: base.AjaxMethodOptions<messageController, entities.paginationEntity<entities.message>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<messageController, entities.paginationEntity<entities.message>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByParent(parentId: number, onSuccess: (obj: Array<entities.message>) => void, onError: (error: helpers.mdException) => void): void {
            parentId = mdBusinessLogic.helpers.typeConversion.toInt(parentId);

            let options: base.AjaxMethodOptions<messageController, entities.message> = new base.AjaxMethodOptions<messageController, entities.message>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByParent', [parentId]);
            options.responseData = new entities.message();
            options.onSuccess = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByUserId(onSuccess: (obj: Array<entities.message>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<messageController, entities.message> = new base.AjaxMethodOptions<messageController, entities.message>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByUserId');
            options.responseData = new entities.message();
            options.onSuccess = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByMainThread(mainThread: number, onSuccess: (obj: Array<entities.message>) => void, onError: (error: helpers.mdException) => void): void {
            mainThread = mdBusinessLogic.helpers.typeConversion.toInt(mainThread);

            let options: base.AjaxMethodOptions<messageController, entities.message> = new base.AjaxMethodOptions<messageController, entities.message>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByMainThread', [mainThread]);
            options.responseData = new entities.message();
            options.onSuccess = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public save(message: entities.message, onSuccess: (obj: entities.message) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<messageController, entities.message> = new base.AjaxMethodOptions<messageController, entities.message>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.message();
            options.requestData = message;
            options.onSuccess = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public messageRead(message: entities.message, onSuccess: (obj: entities.message) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<messageController, entities.message> = new base.AjaxMethodOptions<messageController, entities.message>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('MessageRead');
            options.responseData = new entities.message();
            options.requestData = message;
            options.onSuccess = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public replace(message: entities.message, onSuccess: (obj: entities.message) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<messageController, entities.message> = new base.AjaxMethodOptions<messageController, entities.message>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Replace');
            options.responseData = new entities.message();
            options.requestData = message;
            options.onSuccess = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public 'delete'(id: number, onSuccess: (obj: entities.message) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<messageController, entities.message> = new base.AjaxMethodOptions<messageController, entities.message>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete', [id]);
            options.responseData = new entities.message();
            options.onSuccess = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }

        public deleteMultiple(messages: any, onSuccess: (obj: Array<entities.message>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<messageController, entities.message> = new base.AjaxMethodOptions<messageController, entities.message>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('DeleteMultiple');
            options.responseData = new entities.message();
            options.requestData = messages;
            options.onSuccess = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public replaceMultiple(messages: any, onSuccess: (obj: Array<entities.message>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<messageController, entities.message> = new base.AjaxMethodOptions<messageController, entities.message>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('ReplaceMultiple');
            options.responseData = new entities.message();
            options.requestData = messages;
            options.onSuccess = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public search(searchData: any, onSuccess: (obj: Array<entities.message>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<messageController, entities.message> = new base.AjaxMethodOptions<messageController, entities.message>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('Search', searchData);
            options.responseData = new entities.message();
            options.onSuccess = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<messageController, entities.message>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getUnreadByUser(requestId: string, onSuccess: (obj: Array<entities.message>, socket: WebSocket) => void, onError: (error: helpers.mdException, socket: WebSocket) => void): string {
            let options: base.AjaxMethodOptions<messageController, entities.message> = new base.AjaxMethodOptions<messageController, entities.message>(requestId);
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetUnreadByUserSocket');
            options.responseData = new entities.message();
            options.onSuccess = (options: base.AjaxMethodDataSocket<messageController, entities.message>): void => {
                onSuccess(options.responseDataArray, options.socket);
            }
            options.onError = (options: base.AjaxMethodDataSocket<messageController, entities.message>): void => {
                onError(options.exception, options.socket);
            }
            this._socket(options);
            return options.getRequestId();
        }

        public getAllChats(onSuccess: (obj: entities.paginationEntity<entities.message>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<messageController, entities.paginationEntity<entities.message>> = new base.AjaxMethodOptions<messageController, entities.paginationEntity<entities.message>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetAllChats');
            options.responseData = new entities.paginationEntity<entities.message>(entities.message);
            options.onSuccess = (options: base.AjaxMethodOptions<messageController, entities.paginationEntity<entities.message>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<messageController, entities.paginationEntity<entities.message>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }
    }
}
