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
/// <reference path="../../../globalVariables.ts"/>
/// <reference path="../../../helpers/mdException.ts" />
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var base;
            (function (base) {
                var AjaxMethodData = (function () {
                    function AjaxMethodData() {
                        this.responseData = null;
                        this.requestData = null;
                        this.controller = null;
                        this.exception = null;
                        this.responseDataArray = null;
                    }
                    return AjaxMethodData;
                }());
                base.AjaxMethodData = AjaxMethodData;
                var AjaxMethodDataSocket = (function (_super) {
                    __extends(AjaxMethodDataSocket, _super);
                    function AjaxMethodDataSocket() {
                        var _this = _super.call(this) || this;
                        _this.socket = null;
                        return _this;
                    }
                    return AjaxMethodDataSocket;
                }(AjaxMethodData));
                base.AjaxMethodDataSocket = AjaxMethodDataSocket;
                var AjaxMethodOptions = (function (_super) {
                    __extends(AjaxMethodOptions, _super);
                    function AjaxMethodOptions() {
                        var _this = _super.call(this) || this;
                        _this.onSuccess = function (data) {
                        };
                        _this.onError = function (data) {
                        };
                        _this.includeAuthHeader = false;
                        _this.isJsonArray = false;
                        _this.isAdministration = false;
                        _this.showLoading = true;
                        _this.address = '';
                        _this.method = null;
                        _this.headers = new Array();
                        _this.lcid = mdBusinessLogic.settings.lcid;
                        return _this;
                    }
                    AjaxMethodOptions.prototype.getFullUrl = function () {
                        return mdBusinessLogic.settings.apiBase + this.address;
                    };
                    AjaxMethodOptions.prototype.getMethodTypeString = function () {
                        switch (this.method) {
                            case AjaxMethodType.GET:
                                return 'GET';
                            case AjaxMethodType.POST:
                                return 'POST';
                            case AjaxMethodType.PUT:
                                return 'PUT';
                            case AjaxMethodType.DELETE:
                                return 'DELETE';
                            case AjaxMethodType.SOCKET:
                                return 'SOCKET';
                        }
                    };
                    return AjaxMethodOptions;
                }(AjaxMethodData));
                base.AjaxMethodOptions = AjaxMethodOptions;
                var AjaxMethodType;
                (function (AjaxMethodType) {
                    AjaxMethodType[AjaxMethodType["GET"] = 1] = "GET";
                    AjaxMethodType[AjaxMethodType["POST"] = 2] = "POST";
                    AjaxMethodType[AjaxMethodType["PUT"] = 3] = "PUT";
                    AjaxMethodType[AjaxMethodType["DELETE"] = 4] = "DELETE";
                    AjaxMethodType[AjaxMethodType["SOCKET"] = 5] = "SOCKET";
                })(AjaxMethodType = base.AjaxMethodType || (base.AjaxMethodType = {}));
                var AjaxMethodHeader = (function () {
                    function AjaxMethodHeader(name, value) {
                        this.name = '';
                        this.value = '';
                        this.name = name;
                        this.value = value;
                    }
                    return AjaxMethodHeader;
                }());
                base.AjaxMethodHeader = AjaxMethodHeader;
            })(base = controllers.base || (controllers.base = {}));
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=baseController.options.js.map