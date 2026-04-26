(function () {
    'use strict';
    angular
        .module('app.core')
        .directive('mdCmsAuthenticationProviderBuiltin', ['$compile', '$timeout', mdCmsAuthenticationProviderBuiltin]);
    /** @ngInject */
    function mdCmsAuthenticationProviderBuiltin($compile, $timeout) {

        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-cms-authentication-provider-builtin/md-cms-authentication-provider-builtin.template.html',
            required: ['processAuthentication'],
            scope: {
                processAuthentication: '@',
                onRememberMe: '@',
                onSave: '@',
                mode: '@',
                referenceId: '@'
            },
            controller: 'mdCmsAuthenticationProviderBuiltinController as vm'
        }
    }
})();
