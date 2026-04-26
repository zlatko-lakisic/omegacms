(function () {
    'use strict';

    angular
        .module('app.core')
        .factory('mdDataAccessRegistryService', [mdDataAccessRegistryService])
        .provider('mdDataAccessRegistry', mdDataAccessRegistryProvider);

    var service = {
        add: function (obj) {
            mdBusinessLogic.dataAccess.providers.dataAccess.dataAccessPluginRegistry.add(obj);
        },
        get: function (key) {
            return mdBusinessLogic.dataAccess.providers.dataAccess.dataAccessPluginRegistry.get(key);
        },
        getAll: function () {
            return mdBusinessLogic.dataAccess.providers.dataAccess.dataAccessPluginRegistry.getAll();
        }
    };

    /** @ngInject */
    function mdDataAccessRegistryService() {
        return service;
    }

    function mdDataAccessRegistryProvider() {

        this.add = service.add;

        this.get = service.get;

        this.getAll = service.getAll;

        this.$get = [function() {
            return service;
        }];
    }
}());
