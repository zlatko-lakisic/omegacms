/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./attributeTypeDefinition.ts" />
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
            var profileTypeField = (function (_super) {
                __extends(profileTypeField, _super);
                function profileTypeField() {
                    var _this = _super.call(this) || this;
                    _this.ProfileTypeId = 0;
                    _this.AttributeTypeDefinitionId = 0;
                    _this.Name = '';
                    _this.FriendlyName = '';
                    _this.Description = '';
                    _this.DefaultValue = '';
                    _this.ValidationXml = null;
                    _this.AttributeTypeDefinition = new entities.attributeTypeDefinition();
                    _this.ListValue = '';
                    _this.Delimiter = '';
                    _this.Order = 0;
                    _this.Options = '';
                    return _this;
                }
                profileTypeField.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.ProfileTypeId = this.getValue(data, "ProfileTypeId", 0);
                    this.AttributeTypeDefinitionId = this.getValue(data, "AttributeTypeDefinitionId", 0);
                    this.Name = this.getValue(data, "Name", '');
                    this.FriendlyName = this.getValue(data, "FriendlyName", '');
                    this.Description = this.getValue(data, "Description", '');
                    this.DefaultValue = this.getValue(data, "DefaultValue", '');
                    this.ValidationXml = this.getValue(data, "ValidationXml", null);
                    this.AttributeTypeDefinition = this.getConstructEntityValue(data, "AttributeTypeDefinition", null);
                    this.ListValue = this.getValue(data, "ListValue", '');
                    this.Delimiter = this.getValue(data, "Delimiter", '');
                    this.Order = this.getValue(data, "Order", 0);
                    this.Options = this.getValue(data, "Options", '');
                };
                return profileTypeField;
            }(entities.base.BaseEntity));
            entities.profileTypeField = profileTypeField;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=profileTypeField.js.map