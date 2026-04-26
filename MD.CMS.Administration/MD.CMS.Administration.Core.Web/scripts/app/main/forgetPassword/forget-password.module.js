(function () {
    'use strict';

    angular
        .module('app.forgetPassword', [])
      .config(['$stateProvider', '$translatePartialLoaderProvider', 'msApiProvider', 'msNavigationServiceProvider', config]);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msApiProvider, msNavigationServiceProvider) {
        // State
        $stateProvider.state('app.forgetPassword', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/changepassword?token',
   
          

            views    : {
            'main@'                                : {
                templateUrl: 'scripts/app/core/layouts/content-only.html',
                controller: 'MainController as vm'
            },
            'content@app.forgetPassword': {
                   templateUrl: 'scripts/app/main/forgetPassword/forget-password.html',
                   controller: 'ForgetPasswordController as vm'
               }
        },
        bodyClass: 'reset-password'
        });


        // Translation
        $translatePartialLoaderProvider.addPart('scripts/app/main/forgetPassword');

    }

})();