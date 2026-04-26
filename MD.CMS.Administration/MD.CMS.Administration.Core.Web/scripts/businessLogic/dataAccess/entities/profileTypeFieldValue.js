/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
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
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var profileTypeFieldValue = (function (_super) {
                __extends(profileTypeFieldValue, _super);
                function profileTypeFieldValue() {
                    var _this = _super.call(this) || this;
                    _this.ValueProfileTypeFieldId = 0;
                    _this.ValueProfileTypeId = 0;
                    _this.UserId = 0;
                    _this.Value = '';
                    return _this;
                }
                profileTypeFieldValue.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ValueProfileTypeFieldId = this.getValue(data, "ValueProfileTypeFieldId", 0);
                    this.ValueProfileTypeId = this.getValue(data, "ValueProfileTypeId", 0);
                    this.UserId = this.getValue(data, "UserId", 0);
                    this.Value = this.getValue(data, "Value", '');
                };
                return profileTypeFieldValue;
            }(entities.base.BaseEntity));
            entities.profileTypeFieldValue = profileTypeFieldValue;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=profileTypeFieldValue.js.map