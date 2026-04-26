(function () {
    'use strict';

    angular
        .module('app.core')
        .provider('mdPermissionAuthenticate', ['mdPermissionEntitiesProvider', 'mdSavedDataProvider', 'mdSavedDataKeys', mdPermissionAuthenticateProvider])
        .factory('mdPermissionAuthenticateService', ['$q', 'mdPermissionAuthenticate', mdPermissionAuthenticateService]);

    /** @ngInject */
    function mdPermissionAuthenticateService($q, mdPermissionAuthenticateProvider) {

        return {
            isAuthorized: mdPermissionAuthenticateProvider.isAuthorized,
            getLoggedOnUserPermissions: mdPermissionAuthenticateProvider.getLoggedOnUserPermissions,
            setLoggedOnUserPermissions: mdPermissionAuthenticateProvider.setLoggedOnUserPermissions,
            getLoggedOnProfileTypePermissions: mdPermissionAuthenticateProvider.getLoggedOnProfileTypePermissions,
            setLoggedOnProfileTypePermissions: mdPermissionAuthenticateProvider.setLoggedOnProfileTypePermissions,
            onPermissionsInitialLoadedPromise: mdPermissionAuthenticateProvider.onPermissionsInitialLoadedPromise,
            authenticateByUser: function (groupName) {
                if (!mdPermissionAuthenticateProvider.isAuthorized(groupName)) {
                    return $q.reject(403);
                }
                return $q.resolve(200);
            }
        }
    }

    function mdPermissionAuthenticateProvider(mdPermissionEntitiesProvider, mdSavedDataProvider, mdSavedDataKeys) {
        var mdPermissionAuthenticatorObj = new mdPermissionAuthenticator();
        var loggedOnProfileTypePermissionsInitialLoaded = false;
        var loggedOnUserPermissionsInitialLoaded = false;

        function mdPermissionAuthenticator() {

            function getLoggedOnUserPermissions() {
                return mdBusinessLogic.globals.loggedOnUserPermissions;
            }

            function setLoggedOnUserPermissions(val) {
                if (val === undefined || val == null || !Array.isArray(val)) {
                    throw 'The argument "val" must be an array when setting logged on user permissions!';
                }

                mdBusinessLogic.globals.loggedOnUserPermissions = val;
                loggedOnUserPermissionsInitialLoaded = true;
            }

            function getLoggedOnProfileTypePermissions() {
                return mdBusinessLogic.globals.loggedOnProfileTypePermissions;
            }

            function setLoggedOnProfileTypePermissions(val) {
                if (val === undefined || val == null || !Array.isArray(val)) {
                    throw 'The argument "val" must be an array when setting logged on profile type permissions!';
                }

                mdBusinessLogic.globals.loggedOnProfileTypePermissions = val;
                loggedOnProfileTypePermissionsInitialLoaded = true;
            }

            function isAuthorized(groupName) {

                var isAuthorized = false;

                var group = mdPermissionEntitiesProvider.groups().filter(function (group) { return groupName == group.name; })[0];
                if (group !== undefined) {
                    isAuthorized = checkPermissions(group, getLoggedOnUserPermissions(), isAuthorized);
                    isAuthorized = checkPermissions(group, getLoggedOnProfileTypePermissions(), isAuthorized, 'Entity');
                }

                return isAuthorized;
            }

            function checkPermissions(group, permissions, isAuthorized, propertyToCheck) {
                if (propertyToCheck === undefined) {
                    propertyToCheck = 'Object';
                }

                for (var i = 0; i < permissions.length && !isAuthorized; i++) {
                    var permission = permissions[i];
                    var numberOfPermissionsToMatch = group.entities.length;
                    var numberOfPermissionsMatched = 0;

                    for (var pep = 0; pep < permission.EntityPermissions.length; pep++) {
                        var entityPermission = permission.EntityPermissions[pep];
                        for (var ge = 0; ge < group.entities.length; ge++) {
                            var en = group.entities[ge];
                            if (entityPermission[propertyToCheck] == en.id) {
                                numberOfPermissionsMatched++
                            }
                        }
                    }

                    if (numberOfPermissionsToMatch == numberOfPermissionsMatched) {
                        isAuthorized = true;
                        break;
                    }
                }
                return isAuthorized;
            }

            function onStateEnter(groupNames) {
                return ['$state', '$q', function ($state, $q) {
                    if (groupNames === undefined) {
                        groupNames = [];
                    }

                    if (Array.isArray(groupNames)) {

                        return $q.all([
                            $state.transition,
                            onPermissionsInitialLoadedPromise()
                        ]).then(function () {
                            if (!mdSavedDataProvider.getData(mdSavedDataKeys.globals.loggedOnUser)) {
                                return $state.go('app.login');
                            }

                            if (groupNames.filter(function (group) { return !isAuthorized(group); }).length > 0) {
                                return $state.go('app.errors_error-403');
                            }
                        });
                    }
                }]
            }

            function onPermissionsInitialLoadedPromise() {
                return new Promise(function (resolve, reject) {
                    var check = setInterval(function () {
                        if (loggedOnProfileTypePermissionsInitialLoaded && loggedOnUserPermissionsInitialLoaded) {
                            resolve();
                            clearInterval(check);
                        }
                    }, 200);
                });
            }

            this.isAuthorized = isAuthorized;
            this.getLoggedOnUserPermissions = getLoggedOnUserPermissions;
            this.setLoggedOnUserPermissions = setLoggedOnUserPermissions;
            this.getLoggedOnProfileTypePermissions = getLoggedOnProfileTypePermissions;
            this.setLoggedOnProfileTypePermissions = setLoggedOnProfileTypePermissions;
            this.onStateEnter = onStateEnter;
            this.onPermissionsInitialLoadedPromise = onPermissionsInitialLoadedPromise;
        }


        this.isAuthorized = mdPermissionAuthenticatorObj.isAuthorized;
        this.getLoggedOnUserPermissions = mdPermissionAuthenticatorObj.getLoggedOnUserPermissions;
        this.setLoggedOnUserPermissions = mdPermissionAuthenticatorObj.setLoggedOnUserPermissions;
        this.getLoggedOnProfileTypePermissions = mdPermissionAuthenticatorObj.getLoggedOnProfileTypePermissions;
        this.setLoggedOnProfileTypePermissions = mdPermissionAuthenticatorObj.setLoggedOnProfileTypePermissions;
        this.onStateEnter = mdPermissionAuthenticatorObj.onStateEnter;

        this.$get = function () {
            return mdPermissionAuthenticatorObj;
        };
    }
}());