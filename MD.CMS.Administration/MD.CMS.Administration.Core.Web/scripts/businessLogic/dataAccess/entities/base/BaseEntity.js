/// <reference path="../../../helpers/entityHelper.ts" />
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var base;
            (function (base) {
                var BaseEntity = (function () {
                    function BaseEntity() {
                        this.Id = 0;
                        this.IsDeleted = false;
                    }
                    BaseEntity.getValue = function (data, fieldName, defaultValue) {
                        return mdBusinessLogic.helpers.entityHelper.getValue(data, fieldName, defaultValue);
                    };
                    BaseEntity.prototype.getValue = function (data, fieldName, defaultValue) {
                        return mdBusinessLogic.helpers.entityHelper.getValue(data, fieldName, defaultValue);
                    };
                    BaseEntity.getConstructValue = function (data, fieldName, defaultValue) {
                        return mdBusinessLogic.helpers.entityHelper.getConstructValue(data, fieldName, defaultValue);
                    };
                    BaseEntity.prototype.getConstructValue = function (data, fieldName, defaultValue) {
                        return mdBusinessLogic.helpers.entityHelper.getConstructValue(data, fieldName, defaultValue);
                    };
                    BaseEntity.getConstructEntityValue = function (data, fieldName, defaultValue, returnNullIfInvalid) {
                        return mdBusinessLogic.helpers.entityHelper.getConstructEntityValue(data, fieldName, defaultValue, returnNullIfInvalid);
                    };
                    BaseEntity.prototype.getConstructEntityValue = function (data, fieldName, defaultValue, returnNullIfInvalid) {
                        return mdBusinessLogic.helpers.entityHelper.getConstructEntityValue(data, fieldName, defaultValue, returnNullIfInvalid);
                    };
                    BaseEntity.getArrayConstructValue = function (data, fieldName, defaultValue, defaultTypeValue) {
                        return mdBusinessLogic.helpers.entityHelper.getArrayConstructValue(data, fieldName, defaultValue, defaultTypeValue);
                    };
                    BaseEntity.prototype.getArrayConstructValue = function (data, fieldName, defaultValue, defaultTypeValue) {
                        return mdBusinessLogic.helpers.entityHelper.getArrayConstructValue(data, fieldName, defaultValue, defaultTypeValue);
                    };
                    BaseEntity.getArrayConstructEntityValue = function (data, fieldName, defaultValue, defaultTypeValue, returnNullIfInvalid) {
                        return mdBusinessLogic.helpers.entityHelper.getArrayConstructEntityValue(data, fieldName, defaultValue, defaultTypeValue, returnNullIfInvalid);
                    };
                    BaseEntity.prototype.getArrayConstructEntityValue = function (data, fieldName, defaultValue, defaultTypeValue, returnNullIfInvalid) {
                        return mdBusinessLogic.helpers.entityHelper.getArrayConstructEntityValue(data, fieldName, defaultValue, defaultTypeValue, returnNullIfInvalid);
                    };
                    BaseEntity.prototype.construct = function (data) {
                        this.Id = this.getValue(data, 'Id', 0);
                        this.IsDeleted = this.getValue(data, 'IsDeleted', false);
                    };
                    return BaseEntity;
                }());
                base.BaseEntity = BaseEntity;
            })(base = entities.base || (entities.base = {}));
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=BaseEntity.js.map