(function () {
    'use strict';

    angular
        .module('app.support.typescript_documentation', [])
        .config(['$stateProvider', 'msNavigationServiceProvider', 'msApiProvider', config]);

    /** @ngInject */
    function config($stateProvider, msNavigationServiceProvider, msApiProvider) {

        // State       
        $stateProvider.state('app.support_typescript_documentation', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/support/typescript-documentation/',
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/support/documentation/documentationRoot.html',
                    controller: 'TypescriptDocumentationController as vm'
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
        msNavigationServiceProvider.saveItem('support.typescript_documentation', {
            title: 'Menus.MainSupportTsDoc',
            icon: 'icon-language-javascript',
            state: 'app.support_typescript_documentation',
            weight: 4
        });

    }
})();
