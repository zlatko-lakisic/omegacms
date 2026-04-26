/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./profileTypeFieldValue.ts" />
/// <reference path="./rwdPermission.ts" />
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
            var profileType = (function (_super) {
                __extends(profileType, _super);
                function profileType() {
                    var _this = _super.call(this) || this;
                    _this.Name = '';
                    _this.PermissionXmlText = '';
                    _this.Fields = new Array();
                    _this.IsAssigned = false;
                    _this.RWDPermission = new Array();
                    return _this;
                }
                profileType.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Name = this.getValue(data, "Name", '');
                    this.PermissionXmlText = this.getValue(data, "PermissionXmlText", '');
                    this.Fields = this.getArrayConstructEntityValue(data, "Fields", new Array(), new entities.profileTypeFieldValue());
                    this.IsAssigned = this.getValue(data, "RWDPermission", false);
                    this.RWDPermission = this.getArrayConstructEntityValue(data, "Fields", new Array(), new entities.rwdPermission());
                };
                return profileType;
            }(entities.base.BaseEntity));
            entities.profileType = profileType;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=profileType.js.map