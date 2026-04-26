/// <reference path="../../entities/base/iBaseEntity.ts" />
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var controllers;
        (function (controllers) {
            var base;
            (function (base) {
                var BaseController_helpers = (function () {
                    function BaseController_helpers() {
                    }
                    BaseController_helpers.prototype.loadParentNamesAsArray = function (nameArray, obj, parentName, parentLinkName) {
                        if (parentName === undefined) {
                            parentName = 'Name';
                        }
                        if (obj[parentName] !== undefined && obj[parentName] != null) {
                            if (parentLinkName !== undefined && obj[parentLinkName] !== undefined && obj[parentLinkName] != null) {
                                var customObj = new Object();
                                customObj[parentName] = obj[parentName];
                                customObj[parentLinkName] = obj[parentLinkName];
                                nameArray.push(customObj);
                            }
                            else {
                                nameArray.push(obj[parentName]);
                            }
                        }
                        if (obj.Parent !== undefined && obj.Parent != null) {
                            this.loadParentNamesAsArray(nameArray, obj.Parent, parentName, parentLinkName);
                        }
                    };
                    BaseController_helpers.prototype.parseUrl = function (url) {
                        var l = document.createElement("a");
                        l.href = url;
                        return l;
                    };
                    BaseController_helpers.prototype.getAddress = function (endpoint, data) {
                        var address = endpoint;
                        if (address[address.length - 1] != '/') {
                            address += '/';
                        }
                        if (data !== undefined) {
                            if (data instanceof Array) {
                                for (var i = 0; i < data.length; i++) {
                                    address += data[i].toString() + '/';
                                }
                            }
                            else {
                                if (address[address.length - 1] != '?') {
                                    address += '?';
                                }
                                var counter = 0;
                                for (var i in data) {
                                    if (counter > 0) {
                                        address += '&';
                                    }
                                    address += i + '=' + data[i];
                                    counter++;
                                }
                            }
                        }
                        return address;
                    };
                    return BaseController_helpers;
                }());
                base.BaseController_helpers = BaseController_helpers;
            })(base = controllers.base || (controllers.base = {}));
        })(controllers = dataAccess.controllers || (dataAccess.controllers = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=baseController.helpers.js.map