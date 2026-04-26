(function () {
    'use strict';

    angular
        .module('app.core')
        .factory('mdUiVisibilityService', [mdUiVisibilityService])
        .provider('mdUiVisibility', mdUiVisibility);

    var service = {
        add: function (obj) {
            mdBusinessLogic.dataAccess.providers.uiVisibility.uiVisibilityProviderRegistry.add(obj);
        },
        get: function (key, type, id) {
            return mdBusinessLogic.dataAccess.providers.uiVisibility.uiVisibilityProviderRegistry.get(mdBusinessLogic.dataAccess.providers.uiVisibility.uiVisibilityProviderRegistry.getUniqueName(key, type, id));
        },
        getAll: function () {
            return mdBusinessLogic.dataAccess.providers.uiVisibility.uiVisibilityProviderRegistry.getAll();
        }
    };

    /** @ngInject */
    function mdUiVisibilityService() {
        return service;
    }

    function mdUiVisibility() {

        this.add = service.add;

        this.get = service.get;

        this.getAll = service.getAll;

        this.$get = [function() {
            return service;
        }];
    }
}());
