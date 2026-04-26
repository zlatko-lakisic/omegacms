(function () {
    'use strict';

    angular
        .module('app.support.assembly_documentation', [])
        .config(['$stateProvider', 'msNavigationServiceProvider', 'msApiProvider', config]);

    /** @ngInject */
    function config($stateProvider, msNavigationServiceProvider, msApiProvider) {

        // State       
        $stateProvider.state('app.support_assembly_documentation', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/support/assembly-documentation/',
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/support/assembly-documentation/assembly-documentation.html',
                    controller: 'AssemblyDocumentationController as vm'
                }
            },
            params: {
                currentView: ''
            },
            bodyClass: 'file-manager',
            resolve: {
                documentationData: ['msApi', function (msApi)
                {
                    return msApi.resolve('assemblydocumentation@get');
                }]
            }
        });

        msApiProvider.register('assemblydocumentation', ['scripts/app/main/support/assembly-documentation/data.json']);

        // Navigation
        msNavigationServiceProvider.saveItem('support.assembly_documentation', {
            title: 'Menus.MainSupportAssemblyDoc',
            icon: 'icon-language-csharp',
            state: 'app.support_assembly_documentation',
            weight: 4
        });

    }
})();
