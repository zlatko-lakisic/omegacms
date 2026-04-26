/// <reference path="./baseController.helpers.ts" />
/// <reference path="./baseController.options.ts" />
/// <reference path="../../../globalVariables.ts" />
/// <reference path="../../entities/base/iBaseEntity.ts" />
/// <reference path="../../../helpers/mdException.ts" />
/// <reference path="../../entities/exceptions/errorDetails.ts" />
/// <reference path="../../entities/comm/socketModel.ts" />
/// <reference path="../../entities/comm/awsSocketModel.ts" />

/**
 * Base controllers namespace
 */
namespace mdBusinessLogic.dataAccess.controllers.base {
    /** 
     *  Base Controller Class 
     *  Implements [[`BaseController_helpers`]]
     *  @category Controllers
     */
    export abstract class BaseController<C, E extends entities.base.IBaseEntity<E>> extends BaseController_helpers {
        private controllerBase: string;

        /**
         * Construct a controller base
         * @param {string} controllerBase
         */
        constructor(controllerBase: string) {
            super();
            this.controllerBase = controllerBase;
        }

        /**
         * Get endpoint address
         * @param {string} endpoint - The endpoint address
         * @param {object} data - The endpoint request data
         * @param {boolean} includeBase - Include the base
         * @returns {string} Endpoint address
         */
        public getAddress(endpoint: string, data?: any, includeBase?: boolean): string {
            if (includeBase === undefined) {
                includeBase = true;
            }
            if (includeBase && this.controllerBase === undefined || this.controllerBase == '') {
                throw new helpers.mdException('The controllerBase property is missing!');
            }

            if (endpoint === undefined) {
                throw new helpers.mdException('The endpoint argument is missing!');
            }

            let address: string = includeBase ? this.controllerBase + endpoint : endpoint;
            return super.getAddress(address, data);
        }

        /**
         * Generate a non-secure socket request
         * @param options Ajax method options, see [[`AjaxMethodOptions`]]
         * @param requestId request ID from ajax method options
         */
        private generateNonSecureRequestSocket(options: AjaxMethodOptions<C, E>, requestId: string) {
            let socket: WebSocket;
            try {
                let numberOfTries = 0;
                let response: AjaxMethodDataSocket<C, E> = new AjaxMethodDataSocket<C, E>();
                response.controller = options.controller;
                let parsedUrl = settings.packageWebSocketInBody ? this.parseUrl(options.getPartialUrl('web-sockets/')) : this.parseUrl(options.getFullUrl('web-sockets/'));
                let url = parsedUrl.href.replace(parsedUrl.protocol, 'https:' == document.location.protocol ? 'wss:' : 'ws:');
                if (!settings.packageWebSocketInBody) {
                    let headers: Array<AjaxMethodHeader> = options.headers;
                    headers.push(new AjaxMethodHeader("connectionId", requestId));

                    if (url.indexOf('?') >= 0) {
                        url += '&' + headers.map(function (header) {
                            return header.name + '=' + header.value;
                        }).join('&');
                    } else {
                        url += '?' + headers.map(function (header) {
                            return header.name + '=' + header.value;
                        }).join('&');
                    }
                }
                socket = settings.ajax.connections.getSocket(requestId);
                if (socket === undefined || socket == null) {
                    socket = new WebSocket(url);
                }
                socket.onmessage = function (data: MessageEvent<any>) {
                    let shouldRetrySocket = false;
                    try {
                        let awsSocketResponse = new entities.comm.awsSocketModel(JSON.parse(data.data));
                        if (awsSocketResponse && awsSocketResponse.message == 'Endpoint request timed out' && numberOfTries < globals.numberAwsSocketRetries) {
                            shouldRetrySocket = true;
                            numberOfTries++;
                        }
                    } catch (e) {
                        shouldRetrySocket = false;
                    }

                    if (shouldRetrySocket) {
                        sendCallback();
                    } else {
                        let socketModelResponse = new entities.comm.socketModel(JSON.parse(data.data));
                        if (options.isJsonArray) {
                            let jsonData: any = JSON.parse(socketModelResponse.message);
                            response.responseData = options.responseData.clone();
                            response.responseDataArray = new Array<E>();
                            for (let i = 0; i < jsonData.length; i++) {
                                response.responseData.construct(jsonData[i]);
                                response.responseDataArray.push(response.responseData);
                                response.responseData = options.responseData.clone();
                            }
                        } else {
                            response.responseData = options.responseData;
                            response.responseData.construct(JSON.parse(socketModelResponse.message));
                        }
                        response.socket = socket;
                        options.onSuccess(response);
                        numberOfTries = 0;
                    }
                }
                socket.onclose = function (data: CloseEvent) {
                    response.socket = socket;
                    options.onClose(response);
                    settings.ajax.connections.removeSocket(requestId);
                }
                socket.onerror = function (data: Event) {
                    response.socket = socket;
                    response.exception = new mdBusinessLogic.helpers.mdException('The web socket has closed!', data, new Error());
                    options.onError(response);
                    settings.ajax.connections.removeSocket(requestId);
                }
                settings.ajax.connections.addSocket({ id: requestId, obj: socket });
                let sendCallback = (): void => {
                    setTimeout(function () {
                        let socketModelData: entities.comm.socketModel = new entities.comm.socketModel();
                        socketModelData.connectionId = requestId;
                        socketModelData.message = (typeof options.requestData == 'string' || options.requestData instanceof String) ? options.requestData.toString() : JSON.stringify(options.requestData);
                        if (socket.readyState == WebSocket.OPEN) {
                            if (settings.packageWebSocketInBody) {
                                let queryStrings = {};
                                for (let i = 0; i < options.headers.length; i++) {
                                    queryStrings[options.headers[i].name] = options.headers[i].value;
                                }
                                socket.send(JSON.stringify({
                                    action: 'sendmessage',
                                    data: {
                                        address: options.address,
                                        queryStrings: queryStrings,
                                        data: options.requestData
                                    }
                                }));
                            } else {
                                socket.send(JSON.stringify(socketModelData));
                            }
                        } else {
                            sendCallback();
                        }
                    }, 100);
                }
                sendCallback();
            } catch (e) {
                settings.ajax.connections.removeSocket(requestId);
            }
        }


        /**
         * Generate a non-secure xhr request
         * @param options Ajax method options, see [[`AjaxMethodOptions`]]
         * @param requestId request ID from ajax method options
         */
        private generateNonSecureRequestXhr(options: AjaxMethodOptions<C, E>, requestId: string) {
            let xhrExists: boolean = true;
            let xhr: settings.ajax.connectionObject<XMLHttpRequest> = settings.ajax.connections.getRequestObject(requestId);
            if (xhr == null) {
                xhr = {
                    id: requestId,
                    obj: new XMLHttpRequest(),
                    successEvents: [options.onSuccess],
                    errorEvents: [options.onError]
                };
                xhrExists = false;
            }
            if (xhrExists) {
                if (xhr.successEvents === undefined) {
                    xhr.successEvents = [];
                }
                xhr.successEvents.push(options.onSuccess);

                if (xhr.errorEvents === undefined) {
                    xhr.errorEvents = [];
                }
                xhr.errorEvents.push(options.onError);
            } else {
                let response: AjaxMethodData<C, E> = new AjaxMethodData<C, E>();
                response.controller = options.controller;
                xhr.obj.open(options.getMethodTypeString(), options.getFullUrl(), true);
                if (!options.isFormData) {
                    if (options.contentType === undefined || options.contentType == null) {
                        options.contentType = new AjaxMethodHeader('Content-Type', 'application/json; charset=UTF-8');
                    }
                    options.headers.push(options.contentType);
                }
                for (let i: number = 0; i < options.headers.length; i++) {
                    xhr.obj.setRequestHeader(options.headers[i].name, options.headers[i].value);
                }
                //mdBusinessLogic.settings.ajax.onBeforeSend(xhr.obj);
                xhr.obj.addEventListener('load', function (event) {
                    switch (this.status) {
                        case 401:
                            let returnedExceptionUnauthorized: entities.exceptions.errorDetails = new entities.exceptions.errorDetails(JSON.parse(this.responseText));
                            let errorUnauthorized: mdBusinessLogic.helpers.mdException = new mdBusinessLogic.helpers.mdException(returnedExceptionUnauthorized.Message, event, returnedExceptionUnauthorized);
                            mdBusinessLogic.settings.admin.onEvent(mdBusinessLogic.settings.adminEventTypes.ajaxOnUnauthorized, this, event, errorUnauthorized);
                            break;
                        case 403:
                            let returnedExceptionForbidden: entities.exceptions.errorDetails = new entities.exceptions.errorDetails(JSON.parse(this.responseText));
                            let errorForbidden: mdBusinessLogic.helpers.mdException = new mdBusinessLogic.helpers.mdException(returnedExceptionForbidden.Message, event, returnedExceptionForbidden);
                            mdBusinessLogic.settings.admin.onEvent(mdBusinessLogic.settings.adminEventTypes.ajaxOnForbidden, this, event, errorForbidden);
                            break;
                        case 404:
                            let returnedExceptionNotFound: entities.exceptions.errorDetails = new entities.exceptions.errorDetails(JSON.parse(this.responseText));
                            response.exception = new mdBusinessLogic.helpers.mdException(returnedExceptionNotFound.Message, event, returnedExceptionNotFound);
                            break;
                        case 200:
                            try {
                                if (this.responseText != undefined && this.responseText.length > 0) {
                                    if (options.isJsonArray) {
                                        let jsonData: any = JSON.parse(this.responseText);
                                        response.responseDataArray = new Array<E>();
                                        for (let i = 0; i < jsonData.length; i++) {
                                            let responseObj: E = options.responseData.clone();
                                            responseObj.construct(jsonData[i]);
                                            if (responseObj instanceof entities.primitiveType) {
                                                response.responseDataArray.push((responseObj as any).Value);//this is a bit dirty, should be done diffrently if possible
                                            } else {
                                                response.responseDataArray.push(_.cloneDeep(responseObj));
                                            }
                                        }
                                    } else {
                                        response.responseData = options.responseData;
                                        if (options.responseData instanceof entities.primitiveType) {
                                            response.responseData.construct(this.responseText);
                                        } else {
                                            response.responseData.construct(JSON.parse(this.responseText));
                                        }
                                    }
                                }
                            } catch (exception) {
                                response.exception = new mdBusinessLogic.helpers.mdException(exception.message, event, exception);
                                throw response.exception;
                            }

                            if (xhr.successEvents !== undefined) {
                                for (var i = 0; i < xhr.successEvents.length; i++) {
                                    xhr.successEvents[i](response);
                                }
                            }
                            break;
                        default:
                            let returnExceptionOther: entities.exceptions.errorDetails = new entities.exceptions.errorDetails(JSON.parse(this.responseText));
                            response.exception = new mdBusinessLogic.helpers.mdException(returnExceptionOther.Message, event, returnExceptionOther);
                    }
                    settings.ajax.connections.removeRequest(requestId);
                });
                xhr.obj.addEventListener('loadend', function (event) {
                    mdBusinessLogic.settings.admin.onEvent(mdBusinessLogic.settings.adminEventTypes.ajaxOnComplete, this, event);
                });
                xhr.obj.addEventListener('error', function (event) {
                    let returnedException: entities.exceptions.errorDetails = new entities.exceptions.errorDetails(JSON.parse(this.responseText));
                    let error: mdBusinessLogic.helpers.mdException = new mdBusinessLogic.helpers.mdException(returnedException.Message, event, returnedException);
                    switch (this.status) {
                        case 401:
                            mdBusinessLogic.globals.loggedOnUser = null;
                            mdBusinessLogic.settings.admin.onEvent(mdBusinessLogic.settings.adminEventTypes.ajaxOnUnauthorized, this, event, error);
                            break;
                        case 403:
                            mdBusinessLogic.settings.admin.onEvent(mdBusinessLogic.settings.adminEventTypes.ajaxOnForbidden, this, event, error);
                            break;
                        default:
                            response.exception = error;
                            if (xhr.errorEvents !== undefined) {
                                for (var i = 0; i < xhr.errorEvents.length; i++) {
                                    xhr.errorEvents[i](response);
                                }
                            }
                            break
                    }
                    settings.ajax.connections.removeRequest(requestId);
                });
                xhr.obj.addEventListener('readystatechange', function (event) {
                    if (this.readyState === 4) {
                        if (this.status !== 200) {
                            if ((this.responseText !== undefined && this.responseText.length > 0)) {
                                let returnedException: entities.exceptions.errorDetails = new entities.exceptions.errorDetails(JSON.parse(this.responseText));
                                response.exception = new mdBusinessLogic.helpers.mdException(returnedException.Message, event, returnedException);
                            } else {
                                response.exception = new mdBusinessLogic.helpers.mdException('An error occurred while executing the ' + options.getMethodTypeString() + ' request! status(' + this.status.toString() + ')', event, (this.responseText !== undefined && this.responseText.length > 0) ? JSON.parse(this.responseText) : new Error());
                            }
                            options.onError(response);
                        }
                    }

                });
                mdBusinessLogic.settings.admin.onEvent(mdBusinessLogic.settings.adminEventTypes.ajaxOnBeforeSend, xhr.obj).then(function (data) {
                    switch (options.method) {
                        case AjaxMethodType.POST:
                        case AjaxMethodType.DELETE:
                            if (!options.isFormData) {
                                if (options.contentType != null && options.contentType.value.indexOf("application/json") >= 0) {
                                    mdBusinessLogic.settings.admin.onEvent(mdBusinessLogic.settings.adminEventTypes.ajaxOnJsonSerialize, options.requestData).then(function (data) {
                                        let resultData = data[0];
                                        if (resultData == null) {
                                            resultData = undefined;
                                        }
                                        xhr.obj.send(JSON.stringify(resultData));
                                    });
                                } else {
                                    xhr.obj.send(this.prepareFormData(options.requestData));
                                }
                            } else {
                                xhr.obj.send(options.requestData);
                            }
                            break;
                        default:
                            xhr.obj.send();
                    }
                });
                settings.ajax.connections.addRequest(xhr);
            }
        }

        /**
         * Generate a non-secure request
         * Non-secure requests are requests that are sent in plain text over the desired http(s) protocol
         * @param options Ajax method options, see [[`AjaxMethodOptions`]]
         */
        private generateNonSecureRequest(options: AjaxMethodOptions<C, E>): void {
            options = this.setHeaders(options);
            let requestId = options.getRequestId();
            let obj = this;
            if (options.method == AjaxMethodType.SOCKET) {
                obj.generateNonSecureRequestSocket(options, requestId);
            } else {
                if (!options.isInitCall) {
                    systemInfoController.processPreInit(function () {
                        obj.generateNonSecureRequestXhr(options, requestId);
                    });
                } else {
                    obj.generateNonSecureRequestXhr(options, requestId);
                }
            }
        }

        private setHeaders(options: AjaxMethodOptions<C, E>): AjaxMethodOptions<C, E> {
            if (options.includeAuthHeader && mdBusinessLogic.globals.loggedOnUserToken != null) {
                options.headers.push(new AjaxMethodHeader(mdBusinessLogic.settings.authorizationHeader, mdBusinessLogic.globals.loggedOnUserToken));
            }
            if (mdBusinessLogic.settings.apiAllowCrossOrigin) {
                options.headers.push(new AjaxMethodHeader('Access-Control-Allow-Origin', '*'));
                options.headers.push(new AjaxMethodHeader('Access-Control-Allow-Methods', '*'));
            }
            if (mdBusinessLogic.settings.lcid != undefined && mdBusinessLogic.settings.lcid != 0) {
                options.headers.push(new AjaxMethodHeader('LCID', mdBusinessLogic.settings.lcid.toString()));
            }
            if (options.isAdministration) {
                options.headers.push(new AjaxMethodHeader('Administration', 'true'));
            }
            return options;
        }

        private prepareFormData(data: any): string {
            var formData = new Array<any>();
            if (data !== null && typeof data === 'object') {
                for (var key in data) {
                    if (data !== null && !(data[key] instanceof Date) && (typeof data[key] === 'object' || Array.isArray(data))) {
                        this.prepareSubFormItems(formData, data[key], key);
                    } else if (data !== null && (data[key] instanceof Date || typeof data[key] === 'string' || typeof data[key] === 'number' || typeof data[key] === 'boolean')) {
                        formData.push({
                            name: key,
                            value: encodeURIComponent((data[key] instanceof Date) ? moment(data[key]).format('YYYY-MM-DD HH:mm:ss') : data[key])
                        });
                    }
                }
            }
            return formData.map(function (item) {
                return item.name + '=' + item.value;
            }).join('&');
        }

        private prepareSubFormItems(formData: Array<any>, data: any, namePrefix: string): void {
            if (data !== null && typeof data === 'object') {
                for (var key in data) {
                    if (data !== null && !(data[key] instanceof Date) && (typeof data[key] === 'object' || Array.isArray(data[key]))) {
                        this.prepareSubFormItems(formData, data[key], namePrefix + '[' + key + ']');
                    } else if (data !== null && (data[key] instanceof Date || typeof data[key] === 'string' || typeof data[key] === 'number' || typeof data[key] === 'boolean')) {
                        formData.push({
                            name: namePrefix + '[' + key + ']',
                            value: encodeURIComponent((data[key] instanceof Date) ? moment(data[key]).format('YYYY-MM-DD HH:mm:ss') : data[key])
                        });
                    }
                }
            }
        }

        public _get(options: AjaxMethodOptions<C, E>): void {
            options.method = AjaxMethodType.GET;
            this.generateNonSecureRequest(options);
        }

        public _post(options: AjaxMethodOptions<C, E>): void {
            options.method = AjaxMethodType.POST;
            this.generateNonSecureRequest(options);
        }

        public _put(options: AjaxMethodOptions<C, E>): void {
            options.method = AjaxMethodType.PUT;
            this.generateNonSecureRequest(options);
        }

        public _delete(options: AjaxMethodOptions<C, E>): void {
            options.method = AjaxMethodType.DELETE;
            this.generateNonSecureRequest(options);
        }

        public _socket(options: AjaxMethodOptions<C, E>): void {
            options.method = AjaxMethodType.SOCKET;
            this.generateNonSecureRequest(options);
        }
    }
}