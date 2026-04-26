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
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var user = (function (_super) {
                __extends(user, _super);
                function user() {
                    var _this = _super.call(this) || this;
                    _this.Username = '';
                    _this.Password = '';
                    _this.ProfileTypes = new Array();
                    _this.ProfileTypeId = 0;
                    _this.OldPassword = '';
                    _this.Token = '';
                    _this.DateRefreshToken = new Date();
                    _this.RWDPermissions = new Array();
                    return _this;
                }
                user.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Username = this.getValue(data, 'Username', '');
                    this.Password = this.getValue(data, 'Password', '');
                    this.ProfileTypes = this.getValue(data, 'ProfileTypes', new Array());
                    this.ProfileTypeId = this.getValue(data, 'ProfileTypeId', 0);
                    this.OldPassword = this.getValue(data, 'OldPassword', '');
                    this.Token = this.getValue(data, 'Token', '');
                    this.DateRefreshToken = this.getValue(data, 'DateRefresh', new Date());
                    this.RWDPermissions = this.getValue(data, 'RWDPermissions', new Array());
                };
                return user;
            }(entities.base.BaseEntity));
            entities.user = user;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=user.js.map