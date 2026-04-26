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
            var secureMessage = (function (_super) {
                __extends(secureMessage, _super);
                function secureMessage() {
                    var _this = _super.call(this) || this;
                    _this.EndPoint = '';
                    _this.Message = '';
                    _this.IsEncripted = false;
                    return _this;
                }
                secureMessage.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.EndPoint = this.getValue(data, "EndPoint", '');
                    this.Message = this.getValue(data, "Message", '');
                    this.IsEncripted = this.getValue(data, "IsEncripted", false);
                };
                return secureMessage;
            }(entities.base.BaseEntity));
            entities.secureMessage = secureMessage;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=secureMessage.js.map