var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/// <reference path="./baseController.helpers.ts" />
/// <reference path="./baseController.options.ts" />
/// <reference path="../../../globalVariables.ts" />
/// <reference path="../../entities/base/iBaseEntity.ts" />
/// <reference path="../../../helpers/mdException.ts" />
/// <reference path="../../../moment.shim.d.ts" />
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var base;
            (function (base) {
                var BaseController = (function (_super) {
                    __extends(BaseController, _super);
                    function BaseController(controllerBase) {
                        var _this = _super.call(this) || this;
                        _this.controllerBase = controllerBase;
                        return _this;
                    }
                    BaseController.prototype.getAddress = function (endpoint, data) {
                        console.log(this.controllerBase);
                        if (this.controllerBase === undefined || this.controllerBase == '') {
                            throw new mdBusinessLogic.helpers.mdException('The controllerBase property is missing!');
                        }
                        if (endpoint === undefined || endpoint == '') {
                            throw new mdBusinessLogic.helpers.mdException('The endpoint argument is missing!');
                        }
                        var address = this.controllerBase + endpoint;
                        return _super.prototype.getAddress.call(this, address, data);
                    };
                    BaseController.prototype.generateNonSecureRequest = function (options) {
                        options = this.setHeaders(options);
                        if (options.method == base.AjaxMethodType.SOCKET) {
                            var socket_1;
                            try {
                                var response_1 = new base.AjaxMethodDataSocket();
                                response_1.controller = options.controller;
                                var parsedUrl = this.parseUrl(options.getFullUrl());
                                var url = parsedUrl.href.replace(parsedUrl.protocol, 'ws:');
                                if (url.indexOf('?') >= 0) {
                                    url += '&' + options.headers.map(function (header) {
                                        return header.name + '=' + header.value;
                                    }).join('&');
                                }
                                else {
                                    url += '?' + options.headers.map(function (header) {
                                        return header.name + '=' + header.value;
                                    }).join('&');
                                }
                                socket_1 = new WebSocket(url);
                                socket_1.onmessage = function (data) {
                                    if (options.isJsonArray) {
                                        var jsonData = JSON.parse(data.data);
                                        response_1.responseData = options.responseData;
                                        response_1.responseDataArray = new Array();
                                        for (var i = 0; i < jsonData.length; i++) {
                                            response_1.responseData.construct(jsonData[i]);
                                            response_1.responseDataArray.push(response_1.responseData);
                                        }
                                        response_1.responseData = options.responseData;
                                    }
                                    else {
                                        response_1.responseData = options.responseData;
                                        response_1.responseData.construct(JSON.parse(data.data));
                                    }
                                    response_1.socket = socket_1;
                                    options.onSuccess(response_1);
                                };
                                socket_1.onclose = function (data) {
                                    response_1.socket = socket_1;
                                    response_1.exception = new mdBusinessLogic.helpers.mdException('The web socket has closed!', data, new Error());
                                    options.onError(response_1);
                                };
                            }
                            catch (e) {
                            }
                        }
                        else {
                            var xhr = new XMLHttpRequest();
                            var response_2 = new base.AjaxMethodData();
                            response_2.controller = options.controller;
                            xhr.open(options.getMethodTypeString(), options.getFullUrl(), true);
                            switch (options.method) {
                                case base.AjaxMethodType.POST:
                                    options.headers.push(new base.AjaxMethodHeader('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8'));
                                    break;
                                default:
                                    if (options.isJsonArray) {
                                        options.headers.push(new base.AjaxMethodHeader('Content-Type', 'application/json; charset=UTF-8'));
                                    }
                            }
                            for (var i = 0; i < options.headers.length; i++) {
                                xhr.setRequestHeader(options.headers[i].name, options.headers[i].value);
                            }
                            xhr.addEventListener('loadstart', function (event) {
                                mdBusinessLogic.settings.ajax.onBeforeSend();
                            });
                            xhr.addEventListener('load', function (event) {
                                if (this.status == 200) {
                                    if (options.isJsonArray) {
                                        var jsonData = JSON.parse(this.responseText);
                                        response_2.responseData = options.responseData;
                                        response_2.responseDataArray = new Array();
                                        for (var i = 0; i < jsonData.length; i++) {
                                            response_2.responseData.construct(jsonData[i]);
                                            response_2.responseDataArray.push(response_2.responseData);
                                        }
                                        response_2.responseData = options.responseData;
                                    }
                                    else {
                                        response_2.responseData = options.responseData;
                                        response_2.responseData.construct(JSON.parse(this.responseText));
                                    }
                                    options.onSuccess(response_2);
                                }
                                else {
                                    response_2.exception = new mdBusinessLogic.helpers.mdException('An error occurred while executing the ' + options.getMethodTypeString() + ' request! status(' + this.status.toString() + ')', event, new Error());
                                }
                            });
                            xhr.addEventListener('loadend', function (event) {
                                mdBusinessLogic.settings.ajax.onComplete(this);
                            });
                            xhr.addEventListener('error', function (event) {
                                if (this.status == 403) {
                                    mdBusinessLogic.globals.loggedOnUser = null;
                                }
                                response_2.exception = new mdBusinessLogic.helpers.mdException('An error occurred while executing the ' + options.getMethodTypeString() + ' request! status(' + this.status.toString() + ')', event, new Error());
                                options.onError(response_2);
                            });
                            switch (options.method) {
                                case base.AjaxMethodType.POST:
                                    xhr.send(this.prepareFormData(options.requestData));
                                    break;
                                default:
                                    xhr.send();
                            }
                        }
                    };
                    BaseController.prototype.setHeaders = function (options) {
                        if (options.includeAuthHeader && mdBusinessLogic.globals.loggedOnUser != null) {
                            options.headers.push(new base.AjaxMethodHeader('Authorization', mdBusinessLogic.globals.loggedOnUser.Token));
                        }
                        if (mdBusinessLogic.settings.apiAllowCrossOrigin) {
                            options.headers.push(new base.AjaxMethodHeader('Access-Control-Allow-Origin', '*'));
                            options.headers.push(new base.AjaxMethodHeader('Access-Control-Allow-Methods', '*'));
                        }
                        if (mdBusinessLogic.settings.lcid != 0) {
                            options.headers.push(new base.AjaxMethodHeader('LCID', mdBusinessLogic.settings.lcid.toString()));
                        }
                        if (options.isAdministration) {
                            options.headers.push(new base.AjaxMethodHeader('Administration', 'true'));
                        }
                        return options;
                    };
                    BaseController.prototype.prepareFormData = function (data) {
                        var formData = new Array();
                        if (data !== null && typeof data === 'object') {
                            for (var key in data) {
                                if (data !== null && !(data[key] instanceof Date) && (typeof data[key] === 'object' || Array.isArray(data))) {
                                    this.prepareSubFormItems(formData, data[key], key);
                                }
                                else if (data !== null && (data[key] instanceof Date || typeof data[key] === 'string' || typeof data[key] === 'number' || typeof data[key] === 'boolean')) {
                                    formData.push({
                                        name: key,
                                        value: (data[key] instanceof Date) ? moment(data[key]).format('YYYY-MM-DD HH:mm:ss') : data[key]
                                    });
                                }
                            }
                        }
                        return formData.map(function (item) {
                            return item.name + '=' + item.value;
                        }).join('&');
                    };
                    BaseController.prototype.prepareSubFormItems = function (formData, data, namePrefix) {
                        if (data !== null && typeof data === 'object') {
                            for (var key in data) {
                                if (data !== null && !(data[key] instanceof Date) && (typeof data[key] === 'object' || Array.isArray(data))) {
                                    this.prepareSubFormItems(formData, data[key], namePrefix + '[' + key + ']');
                                }
                                else if (data !== null && (data[key] instanceof Date || typeof data[key] === 'string' || typeof data[key] === 'number' || typeof data[key] === 'boolean')) {
                                    formData.push({
                                        name: namePrefix + '[' + key + ']',
                                        value: (data[key] instanceof Date) ? moment(data[key]).format('YYYY-MM-DD HH:mm:ss') : data[key]
                                    });
                                }
                            }
                        }
                    };
                    BaseController.prototype._get = function (options) {
                        options.method = base.AjaxMethodType.GET;
                        this.generateNonSecureRequest(options);
                    };
                    BaseController.prototype._post = function (options) {
                        options.method = base.AjaxMethodType.POST;
                        this.generateNonSecureRequest(options);
                    };
                    BaseController.prototype._put = function (options) {
                        options.method = base.AjaxMethodType.PUT;
                        this.generateNonSecureRequest(options);
                    };
                    BaseController.prototype._delete = function (options) {
                        options.method = base.AjaxMethodType.DELETE;
                        this.generateNonSecureRequest(options);
                    };
                    BaseController.prototype._socket = function (options) {
                        options.method = base.AjaxMethodType.SOCKET;
                        this.generateNonSecureRequest(options);
                    };
                    return BaseController;
                }(base.BaseController_helpers));
                base.BaseController = BaseController;
            })(base = controllers.base || (controllers.base = {}));
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=baseController.js.map