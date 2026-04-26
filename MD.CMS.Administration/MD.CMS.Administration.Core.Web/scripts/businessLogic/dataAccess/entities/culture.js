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
            var culture = (function (_super) {
                __extends(culture, _super);
                function culture() {
                    var _this = _super.call(this) || this;
                    _this.LCID = 0;
                    _this.Name = '';
                    _this.Code = '';
                    _this.IsoCode = '';
                    _this.IsApproved = false;
                    return _this;
                }
                culture.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.LCID = this.getValue(data, "LCID", 0);
                    this.Name = this.getValue(data, "Name", '');
                    this.Code = this.getValue(data, "Code", '');
                    this.IsoCode = this.getValue(data, "IsoCode", '');
                    this.IsApproved = this.getValue(data, "IsApproved", false);
                };
                return culture;
            }(entities.base.BaseEntity));
            entities.culture = culture;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=culture.js.map