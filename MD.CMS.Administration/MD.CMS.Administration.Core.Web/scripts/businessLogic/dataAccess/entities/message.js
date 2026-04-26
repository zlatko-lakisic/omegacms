/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./user.ts" />
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
            var message = (function (_super) {
                __extends(message, _super);
                function message() {
                    var _this = _super.call(this) || this;
                    _this.Subject = '';
                    _this.MessageContent = '';
                    _this.ParentId = 0;
                    _this.IsRead = false;
                    _this.MessageFolderId = 0;
                    _this.DateAdded = null;
                    _this.Type = 0;
                    _this.FromUserId = 0;
                    _this.ToUserId = 0;
                    _this.FromUser = new entities.user();
                    _this.ToUser = new entities.user();
                    _this.MainThread = 0;
                    return _this;
                }
                message.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Subject = this.getValue(data, "Subject", '');
                    this.MessageContent = this.getValue(data, "MessageContent", '');
                    this.ParentId = this.getValue(data, "ParentId", 0);
                    this.IsRead = this.getValue(data, "IsRead", false);
                    this.MessageFolderId = this.getValue(data, "MessageFolderId", 0);
                    this.DateAdded = this.getValue(data, "DateAdded", null);
                    this.Type = this.getValue(data, "Type", 0);
                    this.FromUserId = this.getValue(data, "FromUserId", 0);
                    this.ToUserId = this.getValue(data, "ToUserId", 0);
                    this.FromUser = this.getConstructEntityValue(data, "FromUser", new entities.user());
                    this.ToUser = this.getConstructEntityValue(data, "ToUser", new entities.user());
                    this.MainThread = this.getValue(data, "MainThread", 0);
                };
                return message;
            }(entities.base.BaseEntity));
            entities.message = message;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=message.js.map