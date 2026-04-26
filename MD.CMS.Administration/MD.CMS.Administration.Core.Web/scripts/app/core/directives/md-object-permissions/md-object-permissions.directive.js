(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdObjectPermissions', ['$q', 'mdFeedbackService', 'mdPermissionsService', mdObjectPermissions]);
    /** @ngInject */
    function mdObjectPermissions($q, $mdFeedbackService, mdPermissionsService) {
        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-object-permissions/md-object-permissions.template.html',
            scope: {
                permissions: "=?",
                type: "=",
                object: "=",
                objectId: "=",
                displayPropertyHeader: "=",
                displayProperty: "@",
                save: "&"
            },
            link: function (scope, element, attrs) {
                //Directive variables
                scope.uniqueId = mdBusinessLogic.helpers.Guid.create().value;
                var profileTypeController = new mdBusinessLogic.dataAccess.controllers.profileTypeController();
                var userController = new mdBusinessLogic.dataAccess.controllers.userController();

                //Directive methods
                scope.querySearch = querySearch;
                scope.selectedItemChange = selectedItemChange;
                scope.removePermissionFromList = removePermissionFromList;
                scope.toggleValue = toggleValue;
                scope.getPermissionEntityName = mdPermissionsService.permissions.getEntityName;
                scope.isPermissionSet = mdPermissionsService.object.isPermissionSet;

                //Autocomplete query search
                function querySearch(query) {
                    return mdPermissionsService.generic.queryAllEntities(scope.type, scope.permissions, query);
                }

                function removePermissionFromList(index) {
                    mdPermissionsService.permissions.removePermission(scope.permissions, index);
                }

                function selectedItemChange(entity) {
                    if (entity != null) {
                        if (mdPermissionsService.object.getPermissionForEntity(entity, scope.permissions) == null) {
                            if (entity.RWDPermissions.length == 0) {
                                var permissionEntity = mdPermissionsService.permissions.setupPermission(scope.type, entity);
                                if (permissionEntity != null) {
                                    mdPermissionsService.object.setupPermission(permissionEntity.ObjectPermissions, scope.object, scope.objectId);
                                    scope.permissions.push(permissionEntity);
                                }
                            }
                        } else {
                            if (entity.IsDeleted) {
                                entity.IsDeleted = false;
                            }
                        }
                    } 
                    scope.searchText = '';
                }

                function toggleValue(permission, accessType) {
                    mdPermissionsService.object.togglePermission(mdPermissionsService.permissions.getPermission(scope.type, permission, scope.permissions), scope.object, scope.objectId, accessType);
                }

                function save() {
                    var deferred = $q.defer();

                    switch (scope.type) {
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.ProfileType:
                            (new mdBusinessLogic.dataAccess.controllers.permissionControllerProfileType()).savePermissions(scope.permissions, function (data) {
                                deferred.resolve(data);
                            }, function (error) {
                                $mdFeedbackService.reportError('save', error);
                                deferred.resolve(false);
                            });
                            break;
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.User:
                            (new mdBusinessLogic.dataAccess.controllers.permissionControllerUser()).savePermissions(scope.permissions, function (data) {
                                deferred.resolve(data);
                            }, function (error) {
                                $mdFeedbackService.reportError('save', error);
                                deferred.resolve(false);
                            });
                            break;
                    }

                    return deferred.promise;
                }

                function init() {

                    scope.save()(save);

                    switch (scope.type) {
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.ProfileType:
                            (new mdBusinessLogic.dataAccess.controllers.permissionControllerProfileType()).getProfileTypePermissionsByObject(
                                scope.object,
                                scope.objectId,
                                function (data) {
                                    scope.permissions = data;
                                    mdPermissionsService.generic.queryAllEntities(scope.type, scope.permissions);
                                },
                                function (error) {
                                    $mdFeedbackService.reportError('load', error);
                                });
                            break;
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.User:
                            (new mdBusinessLogic.dataAccess.controllers.permissionControllerUser()).getUserPermissionsByObject(
                                scope.object,
                                scope.objectId,
                                function (data) {
                                    scope.permissions = data;
                                    mdPermissionsService.generic.queryAllEntities(scope.type, scope.permissions);
                                },
                                function (error) {
                                    $mdFeedbackService.reportError('load', error);
                                });
                            break;
                    }
                }

                init();
            }
        };
    }
})();
