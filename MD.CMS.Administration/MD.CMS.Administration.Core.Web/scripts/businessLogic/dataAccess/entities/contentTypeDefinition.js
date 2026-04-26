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
/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/BaseEntity.ts" />
/// <reference path="./base/BaseEntity.ts" />
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var contentTypeDefinition = (function (_super) {
                __extends(contentTypeDefinition, _super);
                function contentTypeDefinition() {
                    var _this = _super.call(this) || this;
                    _this.Name = '';
                    _this.Description = '';
                    _this.Fields = new Array();
                    _this.Options = '';
                    _this.IsEditable = true;
                    _this.Icon = '';
                    return _this;
                }
                contentTypeDefinition.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Name = this.getValue(data, 'Name', '');
                    this.Description = this.getValue(data, 'Description', '');
                    this.Fields = this.getArrayConstructEntityValue(data, 'Fields', new Array(), new entities.contentTypeDefinitionFieldValue());
                    this.Options = this.getValue(data, 'Options', '');
                    this.IsEditable = this.getValue(data, 'IsEditable', true);
                    this.Icon = this.getValue(data, 'Icon', '');
                };
                return contentTypeDefinition;
            }(entities.base.BaseEntity));
            entities.contentTypeDefinition = contentTypeDefinition;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=contentTypeDefinition.js.map