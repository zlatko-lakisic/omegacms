/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./attributeTypeDefinition.ts" />
/// <reference path="./contentTypeDefinitionFieldValidation.ts" />
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
            var jsonField = (function () {
                function jsonField() {
                    this.validation = new entities.contentTypeDefinitionFieldValidation();
                }
                jsonField.prototype.construct = function (data) {
                    this.validation = entities.base.BaseEntity.getConstructValue(data, 'validation', new entities.contentTypeDefinitionFieldValidation());
                };
                return jsonField;
            }());
            var contentTypeDefinitionField = (function (_super) {
                __extends(contentTypeDefinitionField, _super);
                function contentTypeDefinitionField() {
                    var _this = _super.call(this) || this;
                    _this.ContentTypeDefinitionId = 0;
                    _this.AttributeTypeDefinitionId = 0;
                    _this.AttributeTypeDefinition = null;
                    _this.Name = '';
                    _this.SafeName = '';
                    _this.FriendlyName = '';
                    _this.ValidationXml = null;
                    _this.IsRequired = false;
                    _this.DefaultValue = '';
                    _this.Order = 0;
                    _this.Options = '';
                    _this.Delimiter = '';
                    _this.ListValue = '';
                    _this.JsonField = new jsonField();
                    _this.OptionsJson = {};
                    return _this;
                }
                contentTypeDefinitionField.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ContentTypeDefinitionId = this.getValue(data, 'ContentTypeDefinitionId', 0);
                    this.AttributeTypeDefinitionId = this.getValue(data, 'AttributeTypeDefinitionId', 0);
                    this.AttributeTypeDefinition = this.getConstructEntityValue(data, 'AttributeTypeDefinition', new entities.attributeTypeDefinition());
                    this.Name = this.getValue(data, 'Name', '');
                    this.SafeName = this.getValue(data, 'SafeName', '');
                    this.FriendlyName = this.getValue(data, 'FriendlyName', '');
                    this.ValidationXml = this.getValue(data, 'ValidationXml', null);
                    this.IsRequired = this.getValue(data, 'IsRequired', false);
                    this.DefaultValue = this.getValue(data, 'DefaultValue', '');
                    this.Order = this.getValue(data, 'Order', 0);
                    this.Options = this.getValue(data, 'Options', '');
                    this.Delimiter = this.getValue(data, 'Delimiter', '');
                    this.ListValue = this.getValue(data, 'ListValue', '');
                    this.JsonField = this.getConstructValue(data, 'JsonField', new jsonField());
                    this.OptionsJson = this.getValue(data, 'OptionsJson', {});
                };
                return contentTypeDefinitionField;
            }(entities.base.BaseEntity));
            entities.contentTypeDefinitionField = contentTypeDefinitionField;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=contentTypeDefinitionField.js.map