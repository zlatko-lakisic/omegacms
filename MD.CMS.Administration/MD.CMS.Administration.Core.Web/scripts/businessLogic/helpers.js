/// <reference path="./helpers/CheckType.ts" />
/// <reference path="./helpers/Encoder.ts" />
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var helpers;
    (function (helpers) {
        helpers.oopHelper = function (child, parent) {
            child.prototype = Object.create(parent.prototype);
        };
    })(helpers = mdBusinessLogic.helpers || (mdBusinessLogic.helpers = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=helpers.js.map