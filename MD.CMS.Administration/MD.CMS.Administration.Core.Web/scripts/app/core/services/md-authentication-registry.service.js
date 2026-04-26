(function () {
    'use strict';

    angular
        .module('app.core')
        .factory('mdAuthenticationRegistryService', [mdAuthenticationRegistryService])
        .provider('mdAuthenticationRegistry', mdAuthenticationRegistryProvider);

    var service = {
        add: function (obj) {
            mdBusinessLogic.dataAccess.providers.authentication.authenticationProviderRegistry.add(obj);
        },
        get: function (key) {
            return mdBusinessLogic.dataAccess.providers.authentication.authenticationProviderRegistry.get(key);
        },
        getAll: function () {
            return mdBusinessLogic.dataAccess.providers.authentication.authenticationProviderRegistry.getAll();
        }
    };

    /** @ngInject */
    function mdAuthenticationRegistryService() {
        return service;
    }

    function mdAuthenticationRegistryProvider() {

        this.add = service.add;

        this.get = service.get;

        this.getAll = service.getAll;

        this.$get = [function() {
            return service;
        }];
    }
}());
