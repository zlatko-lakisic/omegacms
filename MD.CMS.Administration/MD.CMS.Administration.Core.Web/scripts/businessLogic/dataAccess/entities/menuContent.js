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
            var menuContent = (function (_super) {
                __extends(menuContent, _super);
                function menuContent() {
                    var _this = _super.call(this) || this;
                    _this.LCID = 0;
                    _this.DateCreated = new Date();
                    _this.MenuId = 0;
                    _this.Title = '';
                    _this.MenuContentPath = '';
                    return _this;
                }
                menuContent.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.LCID = this.getValue(data, "LCID", 0);
                    this.DateCreated = this.getValue(data, "DateCreated", new Date());
                    this.MenuId = this.getValue(data, "MenuId", 0);
                    this.Title = this.getValue(data, "Title", '');
                    this.MenuContentPath = this.getValue(data, "MenuContentPath", '');
                };
                return menuContent;
            }(entities.base.BaseEntity));
            entities.menuContent = menuContent;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=menuContent.js.map