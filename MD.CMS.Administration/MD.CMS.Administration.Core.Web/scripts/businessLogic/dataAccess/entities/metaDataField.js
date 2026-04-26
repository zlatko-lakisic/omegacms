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
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./attributeTypeDefinition.ts" />
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var metaDataField = (function (_super) {
                __extends(metaDataField, _super);
                function metaDataField() {
                    var _this = _super.call(this) || this;
                    _this.AttributeTypeDefinitionId = 0;
                    _this.AttributeTypeDefinition = null;
                    _this.Name = '';
                    _this.ValidationXml = null;
                    _this.IsRequired = false;
                    _this.DefaultValue = '';
                    _this.FriendlyName = '';
                    return _this;
                }
                metaDataField.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.AttributeTypeDefinitionId = this.getValue(data, 'AttributeTypeDefinitionId', 0);
                    this.AttributeTypeDefinition = this.getConstructEntityValue(data, 'AttributeTypeDefinition', new entities.attributeTypeDefinition());
                    this.Name = this.getValue(data, 'Name', '');
                    this.ValidationXml = this.getValue(data, 'ValidationXml', null);
                    this.IsRequired = this.getValue(data, 'IsRequired', false);
                    this.DefaultValue = this.getValue(data, 'DefaultValue', '');
                    this.FriendlyName = this.getValue(data, 'FriendlyName', '');
                };
                return metaDataField;
            }(entities.base.BaseEntity));
            entities.metaDataField = metaDataField;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=metaDataField.js.map