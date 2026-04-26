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
            var menu = (function (_super) {
                __extends(menu, _super);
                function menu() {
                    var _this = _super.call(this) || this;
                    _this.ParentId = 0;
                    _this.Name = '';
                    _this.Description = '';
                    _this.Parent = null;
                    _this.Children = [];
                    _this.Items = [];
                    _this.FreeTextField = '';
                    _this.Lcid = 0;
                    _this.FolderId = 0;
                    _this.MenuPath = '';
                    _this.Contents = [];
                    _this.Options = '';
                    return _this;
                }
                menu.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ParentId = this.getValue(data, "ParentId", 0);
                    this.Name = this.getValue(data, "Name", '');
                    this.Description = this.getValue(data, "Description", '');
                    this.Parent = this.getConstructEntityValue(data, "Parent", new menu());
                    this.Children = this.getArrayConstructEntityValue(data, "Children", new Array(), new menu());
                    this.Items = this.getArrayConstructEntityValue(data, "Items", new Array(), new entities.menuContent());
                    this.Contents = this.getArrayConstructEntityValue(data, "Contents", new Array(), new entities.menuContent());
                    this.FreeTextField = this.getValue(data, "FreeTextField", '');
                    this.Lcid = this.getValue(data, "Lcid", 0);
                    this.FolderId = this.getValue(data, "FolderId", 0);
                    this.MenuPath = this.getValue(data, "MenuPath", '');
                    this.Options = this.getValue(data, "Options", '');
                };
                return menu;
            }(entities.base.BaseEntity));
            entities.menu = menu;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=menu.js.map