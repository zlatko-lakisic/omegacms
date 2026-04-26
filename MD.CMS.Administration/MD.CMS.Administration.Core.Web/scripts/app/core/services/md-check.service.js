(function () {
    'use strict';

    angular
        .module('app.core')
        .factory('mdCheckService', ['$state', 'mdSavedDataService', 'mdSavedDataKeys', mdCheckService]);

    /** @ngInject */
    function mdCheckService($state, mdSavedDataService, mdSavedDataKeys) {
        var checkService = {
            clearLogin: function (e) {
                mdSavedDataService.deleteData(mdSavedDataKeys.globals.loggedOnUser);
                mdSavedDataService.deleteData(mdSavedDataKeys.globals.loggedOnUserToken);
                mdSavedDataService.deleteData(mdSavedDataKeys.settings.lcid);
                mdBusinessLogic.globals.loggedOnUser = null;
                mdBusinessLogic.globals.loggedOnUserToken = null;
                if (mdBusinessLogic.settings.lcid == undefined) {
                    mdBusinessLogic.settings.lcid = 0;
                }
                if (e !== undefined) {
                    if (typeof e === "function") {
                        e();
                    } else {
                        e.preventDefault();
                    }
                }
                $state.go('app.login');
            }
        };

        return checkService;
    }
}());
