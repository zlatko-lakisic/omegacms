/// <reference path="../globalVariables.ts" />
/// <reference path="../settings.ts" />
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var helpers;
    (function (helpers) {
        var mdException = (function () {
            function mdException(message, errorData, innerException, stackTrace) {
                this.settings = mdBusinessLogic.settings;
                this.errorData = errorData;
                this.innerException = innerException;
                this.stackTrace = stackTrace;
                this.message = message;
                if (this.settings.debug) {
                    console.log('Error occurred: ' + this.message + (this.errorData !== undefined && this.errorData != null ? ' data(' + JSON.stringify(this.errorData) + ')' + (this.stackTrace !== undefined ? ', stacktrace(' + this.stackTrace + ')' : '') : ''));
                }
            }
            return mdException;
        }());
        helpers.mdException = mdException;
    })(helpers = mdBusinessLogic.helpers || (mdBusinessLogic.helpers = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=mdException.js.map