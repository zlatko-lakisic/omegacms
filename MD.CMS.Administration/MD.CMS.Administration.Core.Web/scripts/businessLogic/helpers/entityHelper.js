/// <reference path="../dataAccess/entities/base/iBaseEntity.ts" />
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var helpers;
    (function (helpers) {
        var entityHelper = (function () {
            function entityHelper() {
            }
            entityHelper.getValue = function (data, fieldName, defaultValue) {
                var returnValue = defaultValue;
                if (data !== undefined && data[fieldName] !== undefined) {
                    if (defaultValue instanceof Date) {
                        returnValue = moment(data[fieldName]).toDate();
                    }
                    else {
                        returnValue = data[fieldName];
                    }
                }
                return returnValue;
            };
            entityHelper.getConstructEntityValue = function (data, fieldName, defaultValue, returnNullIfInvalid) {
                if (returnNullIfInvalid === undefined) {
                    returnNullIfInvalid = true;
                }
                var result = this.getConstructValue(data, fieldName, defaultValue);
                if (returnNullIfInvalid && (result.Id == null || result.Id == 0)) {
                    result = null;
                }
                return result;
            };
            entityHelper.getConstructValue = function (data, fieldName, defaultValue) {
                var returnValue = defaultValue;
                var parsedJson = this.getValue(data, fieldName, null);
                if (parsedJson != null) {
                    returnValue.construct(parsedJson);
                }
                return returnValue;
            };
            entityHelper.getArrayConstructEntityValue = function (data, fieldName, defaultValue, defaultTypeValue, returnNullIfInvalid) {
                if (returnNullIfInvalid === undefined) {
                    returnNullIfInvalid = true;
                }
                var returnValue = defaultValue;
                var parsedJson = this.getValue(data, fieldName, null);
                if (parsedJson != null && parsedJson instanceof Array) {
                    for (var i = 0; i < parsedJson.length; i++) {
                        returnValue.push(this.getConstructEntityValue(parsedJson, i.toString(), defaultTypeValue, returnNullIfInvalid));
                    }
                }
                return returnValue.filter(function (item) {
                    return item !== undefined && item != null;
                });
            };
            entityHelper.getArrayConstructValue = function (data, fieldName, defaultValue, defaultTypeValue) {
                var returnValue = defaultValue;
                var parsedJson = this.getValue(data, fieldName, null);
                if (parsedJson != null && parsedJson instanceof Array) {
                    for (var i = 0; i < parsedJson.length; i++) {
                        returnValue.push(this.getConstructValue(parsedJson, i.toString(), defaultTypeValue));
                    }
                }
                return returnValue.filter(function (item) {
                    return item !== undefined && item != null;
                });
            };
            return entityHelper;
        }());
        helpers.entityHelper = entityHelper;
    })(helpers = mdBusinessLogic.helpers || (mdBusinessLogic.helpers = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=entityHelper.js.map