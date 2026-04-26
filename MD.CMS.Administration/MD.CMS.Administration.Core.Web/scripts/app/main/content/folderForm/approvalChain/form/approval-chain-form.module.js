(function () {
    'use strict';

    angular
        .module('app.approval.chain.form', [])
        .config(['$stateProvider', '$translatePartialLoaderProvider', 'msApiProvider', 'msNavigationServiceProvider', config]);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msApiProvider, msNavigationServiceProvider) {
        $stateProvider.state('app.approval-chain-form', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/approvalChain/form',
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/content/folderForm/approvalChain/form/approval-chain-form.html',
                    controller: 'ApprovalChainFormController as vm'
                }
            },
            params: {
                currentView: 'list'
            },
            bodyClass: 'file-manager'
        });
    }
})();