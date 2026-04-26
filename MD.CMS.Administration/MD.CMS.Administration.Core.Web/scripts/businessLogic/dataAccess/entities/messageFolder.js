/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./user.ts" />
/// <reference path="./message.ts" />
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
            var messageFolder = (function (_super) {
                __extends(messageFolder, _super);
                function messageFolder() {
                    var _this = _super.call(this) || this;
                    _this.Name = '';
                    _this.Icon = '';
                    _this.Author = new entities.user();
                    _this.IsGlobal = false;
                    _this.Messages = new Array();
                    _this.MessagesCount = 0;
                    return _this;
                }
                messageFolder.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.Name = this.getValue(data, "Name", '');
                    this.Icon = this.getValue(data, "Icon", '');
                    this.Author = this.getConstructEntityValue(data, "Author", new entities.user());
                    this.IsGlobal = this.getValue(data, "IsGlobal", false);
                    this.Messages = this.getArrayConstructEntityValue(data, "Messages", new Array(), new entities.message());
                    this.MessagesCount = this.getValue(data, "MessagesCount", 0);
                };
                return messageFolder;
            }(entities.base.BaseEntity));
            entities.messageFolder = messageFolder;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=messageFolder.js.map