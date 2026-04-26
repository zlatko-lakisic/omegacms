(function () {
    'use strict';

    angular
        .module('app.support.webapi_documentation')
        .controller('WebapiDocumentationController', ['$scope', '$mdSidenav', WebapiDocumentationController]);


    /** @ngInject */
    function WebapiDocumentationController($scope, $mdSidenav) {
        var vm = this;

        vm.iframeSrc = mdBusinessLogic.settings.appBase + 'scripts/app/main/support/webapi-documentation/swagger.html';

        window.getSwaggerConfig = getSwaggerConfig;

        function getSwaggerConfig() {
            return {
                "urls": [
                    {
                        "url": mdBusinessLogic.settings.apiBase + "swagger/v" + mdBusinessLogic.globals.systemVersion.toString() + "/swagger.json?Authorization=" + mdBusinessLogic.globals.loggedOnUserToken,
                        "name": "Omega CMS Web Api v" + mdBusinessLogic.globals.systemVersion
                    }
                ],
                "deepLinking": true,
                "displayOperationId": true,
                "defaultModelsExpandDepth": 1,
                "defaultModelExpandDepth": 1,
                "defaultModelRendering": "example",
                "displayRequestDuration": true,
                "docExpansion": "none",
                "showExtensions": true,
                "showCommonExtensions": true,
                "supportedSubmitMethods": ["get", "put", "post", "delete", "options"]
            };
        }
    }
})();