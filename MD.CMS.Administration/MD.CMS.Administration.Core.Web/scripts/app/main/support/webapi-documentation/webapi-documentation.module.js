(function () {
    'use strict';

    angular
        .module('app.support.webapi_documentation', [])
        .config(['$stateProvider', 'msNavigationServiceProvider', 'msApiProvider', config]);

    /** @ngInject */
    function config($stateProvider, msNavigationServiceProvider, msApiProvider) {

        // State       
        $stateProvider.state('app.support_webapi_documentation', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/support/webapi-documentation/',
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/support/documentation/documentationRoot.html',
                    controller: 'WebapiDocumentationController as vm'
                }
            },
            params: {
                currentView: ''
            },
            bodyClass: 'file-manager',
            resolve: {
            }
        });

        // Navigation
        msNavigationServiceProvider.saveItem('support.webapi_documentation', {
            title: 'Menus.MainSupportWsDoc',
            icon: 'icon-server-network',
            state: 'app.support_webapi_documentation',
            weight: 4
        });

    }
})();
