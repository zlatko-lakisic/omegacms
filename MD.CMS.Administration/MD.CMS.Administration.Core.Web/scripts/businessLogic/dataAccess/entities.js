var mdBusinessLogic;
(function (mdBusinessLogic) {
    var dataAccess;
    (function (dataAccess) {
        var entities;
        (function (entities) {
            entities.isFunction = function (functionToCheck) {
                var getType = {};
                return functionToCheck && getType.toString.call(functionToCheck) === '[object Function]';
            };
            entities.isArray = function (obj) {
                return (!!obj) && (obj.constructor === Array);
            };
            entities.isObject = function (obj) {
                return (!!obj) && (obj.constructor === Object);
            };
        })(entities = dataAccess.entities || (dataAccess.entities = {}));
    })(dataAccess = mdBusinessLogic.dataAccess || (mdBusinessLogic.dataAccess = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=entities.js.map