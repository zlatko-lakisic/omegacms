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
/// <reference path="./user.ts" />
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            var loggedOnUser = (function (_super) {
                __extends(loggedOnUser, _super);
                function loggedOnUser() {
                    var _this = _super.call(this) || this;
                    _this.SessionId = '';
                    return _this;
                }
                loggedOnUser.prototype.construct = function (data) {
                    _super.prototype.construct.call(this, data);
                    this.SessionId = this.getValue(data, 'SessionId', '');
                };
                return loggedOnUser;
            }(entities.user));
            entities.loggedOnUser = loggedOnUser;
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=loggedOnUser.js.map