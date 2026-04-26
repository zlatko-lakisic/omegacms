(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdEntityPermissions', ['$q', 'mdFeedbackService', 'mdPermissionsService', mdEntityPermissions]);
    /** @ngInject */
    function mdEntityPermissions($q, $mdFeedbackService, mdPermissionsService) {
        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-entity-permissions/md-entity-permissions.template.html',
            scope: {
                permissions: "=?",
                type: "=",
                group: "=",
                entities: "=",
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
                            var permissionEntity = mdPermissionsService.permissions.setupPermission(scope.type, entity);
                            if (permissionEntity != null) {
                                for (var i in scope.entities) {
                                    var object = scope.entities[i];
                                    var perm = mdPermissionsService.entity.setupPermission(object.id, scope.entity.Id);
                                    perm.AccessTypes.push(mdBusinessLogic.dataAccess.entities.permissions.permissionAccessTypeEnum.Read);
                                    perm.AccessTypes.push(mdBusinessLogic.dataAccess.entities.permissions.permissionAccessTypeEnum.Write);
                                    perm.AccessTypes.push(mdBusinessLogic.dataAccess.entities.permissions.permissionAccessTypeEnum.Delete);
                                    permissionEntity.EntityPermissions.push(perm);
                                }
                                scope.permissions.push(permissionEntity);
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

                function filterPermissions(permissions) {
                    for (var p = 0; p < permissions.length; p++) {
                        for (var i = permissions[p].EntityPermissions.length - 1; i >= 0; i--) {
                            if (permissions[p].EntityPermissions[i].Entity === undefined) {
                                permissions[p].EntityPermissions.splice(i, 1);
                                break;
                            }
                        }
                    }
                    return permissions;
                }

                function save() {
                    var deferred = $q.defer();

                    switch (scope.type) {
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.ProfileType:
                            (new mdBusinessLogic.dataAccess.controllers.permissionControllerProfileType()).savePermissions(filterPermissions(scope.permissions), function (data) {
                                deferred.resolve(data);
                            }, function (error) {
                                $mdFeedbackService.reportError('save', error);
                                deferred.resolve(false);
                            });
                            break;
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.User:
                            (new mdBusinessLogic.dataAccess.controllers.permissionControllerUser()).savePermissions(filterPermissions(scope.permissions), function (data) {
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
                            (new mdBusinessLogic.dataAccess.controllers.permissionControllerProfileType()).getProfileTypePermissionsByEntities(
                                scope.type,
                                scope.entities.map(function (entity) { return entity.id; }),
                                function (data) {
                                    scope.permissions = data;
                                    mdPermissionsService.generic.queryAllEntities(scope.type, scope.permissions);
                                },
                                function (error) {
                                    $mdFeedbackService.reportError('load', error);
                                });
                            break;
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.User:
                            (new mdBusinessLogic.dataAccess.controllers.permissionControllerUser()).getUserPermissionsByEntities(
                                scope.type,
                                scope.entities.map(function (entity) { return entity.id; }),
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
