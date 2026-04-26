/// <reference path="../globalVariables.ts" />
var mdBusinessLogic;
(function (mdBusinessLogic) {
    var settings;
    (function (settings) {
        var secureApi;
        (function (secureApi) {
            var forge;
            secureApi.enabled = false;
            secureApi.rsaKeys = new Object();
            secureApi.aesKey = '';
            secureApi.aesIV = '';
            secureApi.crypto = {
                rsa: (forge !== undefined ? forge.rsa : null),
                aes: null
            };
            secureApi.token = '';
        })(secureApi = settings.secureApi || (settings.secureApi = {}));
    })(settings = mdBusinessLogic.settings || (mdBusinessLogic.settings = {}));
})(mdBusinessLogic || (mdBusinessLogic = {}));
//# sourceMappingURL=secureApi.js.map