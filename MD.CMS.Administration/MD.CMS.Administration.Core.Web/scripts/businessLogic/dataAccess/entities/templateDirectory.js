/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./templateFile.ts" />
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
            var templateDirectory = (function (_super) {
                __extends(templateDirectory, _super);
                function templateDirectory() {
                    var _this = _super.call(this) || this;
                    _this.Path = '';
                    _this.Children = new Array();
                    _this.Files = new Array();
                    _this.Name = '';
                    _this.RootPath = '';
                    return _this;
                }
                templateDirectory.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Path = this.getValue(data, "Path", '');
                    this.Children = this.getArrayConstructEntityValue(data, "Children", new Array(), new templateDirectory());
                    this.Files = this.getArrayConstructEntityValue(data, "Files", new Array(), new entities.templateFile());
                    this.Name = this.getValue(data, "Name", '');
                    this.RootPath = this.getValue(data, "RootPath", '');
                };
                return templateDirectory;
            }(entities.base.BaseEntity));
            entities.templateDirectory = templateDirectory;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=templateDirectory.js.map