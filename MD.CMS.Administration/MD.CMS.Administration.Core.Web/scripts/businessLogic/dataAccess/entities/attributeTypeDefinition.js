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
            var attributeTypeDefinition = (function (_super) {
                __extends(attributeTypeDefinition, _super);
                function attributeTypeDefinition() {
                    var _this = _super.call(this) || this;
                    _this.Name = '';
                    _this.DefaultValue = '';
                    _this.Type = null;
                    _this.InputType = null;
                    return _this;
                }
                attributeTypeDefinition.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Name = this.getValue(data, 'Name', '');
                    this.DefaultValue = this.getValue(data, 'DefaultValue', '');
                    this.Type = this.getValue(data, 'Type', 0);
                    this.InputType = this.getValue(data, 'InputType', 0);
                };
                return attributeTypeDefinition;
            }(entities.base.BaseEntity));
            entities.attributeTypeDefinition = attributeTypeDefinition;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=attributeTypeDefinition.js.map