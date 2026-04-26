(function () {
    'use strict';

    angular
        .module('app.support.javascript_documentation', [])
        .config(['$stateProvider', 'msNavigationServiceProvider', 'msApiProvider', config]);

    /** @ngInject */
    function config($stateProvider, msNavigationServiceProvider, msApiProvider) {

        // State       
        $stateProvider.state('app.support_javascript_documentation', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/support/javascript-documentation/',
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/support/documentation/documentationRoot.html',
                    controller: 'JavascriptDocumentationController as vm'
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
        msNavigationServiceProvider.saveItem('support.javascript_documentation', {
            title: 'Menus.MainSupportJsDoc',
            icon: 'icon-language-html5',
            state: 'app.support_javascript_documentation',
            weight: 4
        });

    }
})();
