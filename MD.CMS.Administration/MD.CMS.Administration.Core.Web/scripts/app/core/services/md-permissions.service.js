(function () {
    'use strict';

    angular
        .module('app.core')
        .factory('mdPermissionsService', ['$q', mdPermissionsService]);

    /** @ngInject */
    function mdPermissionsService($q) {

        //Private attributes
        var controllers = {
            profileTypeController: new mdBusinessLogic.dataAccess.controllers.profileTypeController(),
            userController: new mdBusinessLogic.dataAccess.controllers.userController(),
            permissionControllerProfileType: new mdBusinessLogic.dataAccess.controllers.permissionControllerProfileType(),
            permissionControllerUser: new mdBusinessLogic.dataAccess.controllers.permissionControllerUser()
        };
        var enums = {
            permissionAccessTypeEnum: mdBusinessLogic.dataAccess.entities.permissions.permissionAccessTypeEnum,
            entitiesEnum: mdBusinessLogic.dataAccess.entities.entitiesEnum
        };
        var allEntities = {};

        function getEntityGroupName(type) {
            return enums.entitiesEnum[type];
        }

        function getAllEntities(type) {
            if (allEntities[getEntityGroupName(type)] !== undefined) {
                return allEntities[getEntityGroupName(type)];
            }
            return [];
        }

        function addToEntities(type, data) {
            if (allEntities[getEntityGroupName(type)] === undefined) {
                allEntities[getEntityGroupName(type)] = [];
            }

            allEntities[getEntityGroupName(type)].push.apply(allEntities[getEntityGroupName(type)], data);
        }

        function queryAllEntities(type, permissions, query) {
            var deferred = $q.defer();

            if (query && query.length > 0) {
                switch (type) {
                    case enums.entitiesEnum.ProfileType:
                        controllers.profileTypeController.search({ searchTerm: query }, function (data) {
                            deferred.resolve(getUnusedProfileTypes(permissions, data));
                        }, function (error) {
                            deferred.resolve([]);
                        });
                        break;
                    case enums.entitiesEnum.User:
                        controllers.userController.search({ searchTerm: query }, function (data) {
                            deferred.resolve(getUnusedUsers(permissions, data));
                        }, function (error) {
                            deferred.resolve([]);
                        });
                        break;
                }
            } else {
                if (getAllEntities(type).length == 0) {
                    switch (type) {
                        case enums.entitiesEnum.ProfileType:
                            controllers.profileTypeController.getAll('', function (data) {
                                addToEntities(type, data);
                                deferred.resolve(getUnusedProfileTypes(permissions, data));
                            }, function (error) {
                                deferred.resolve([]);
                            });
                            break;
                        case enums.entitiesEnum.User:
                            controllers.userController.getAll(function (data) {
                                addToEntities(type, data);
                                deferred.resolve(getUnusedUsers(permissions, data));
                            }, function (error) {
                                deferred.resolve([]);
                            });
                            break;
                    }
                } else {
                    switch (type) {
                        case enums.entitiesEnum.ProfileType:
                            deferred.resolve(getUnusedProfileTypes(permissions));
                            break;
                        case enums.entitiesEnum.User:
                            deferred.resolve(getUnusedUsers(permissions));
                            break;
                    }
                }
            }

            return deferred.promise;
        }

        function getAccessTypeName(accessType) {
            return enums.permissionAccessTypeEnum[accessType];
        }

        function getAccessType(accessTypeName) {
            return enums.permissionAccessTypeEnum[accessTypeName];
        }

        function isObjectPermissionSet(permission, accessType) {
            if (permission.ObjectPermissions == null || permission.ObjectPermissions.length == 0) {
                return false;
            }
            return permission.ObjectPermissions[0].AccessTypes.indexOf(accessType) >= 0;
        }

        function isEntityPermissionSet(permission, accessType) {
            if (permission.EntityPermissions == null || permission.EntityPermissions.length == 0) {
                return false;
            }
            return permission.EntityPermissions[0].AccessTypes.indexOf(accessType) >= 0;
        }

        function getPermission(type, permission, permissions) {
            for (var i = 0; i < permissions.length; i++) {
                switch (type) {
                    case enums.entitiesEnum.ProfileType:
                        if (permissions[i].ProfileId == permission.ProfileId) {
                            return permissions[i];
                        }
                        break;
                    case enums.entitiesEnum.User:
                        if (permissions[i].UserId == permission.UserId) {
                            return permissions[i];
                        }
                        break;
                }
            }
            return null;
        }

        function getEntityForObjectPermission(type, permission) {
            var resultArray = getAllEntities(type).filter(function (entity) {
                return permission.ObjectPermissions.filter(function (objectPermission) {
                    return objectPermission.ObjectId == entity.Id;
                }).length > 0;
            });
            if (resultArray.length == 1) {
                return resultArray[0];
            }
            return null;
        }

        function getObjectPermissionForEntity(entity, permissions) {
            var resultArray = permissions.filter(function (permission) {

                return permission.ObjectPermissions.filter(function (objectPermission) {
                    return objectPermission.ObjectId == entity.Id;
                }).length > 0;
            });
            if (resultArray.length == 1) {
                return resultArray[0];
            }
            return null;
        }

        function getEntityForEntityPermission(type, permission) {
            var resultArray = getAllEntities(type).filter(function (entity) {
                return permission.EntityPermissions.filter(function (entityPermission) {
                    return entityPermission.EntityId == entity.Id;
                }).length > 0;
            });
            if (resultArray.length == 1) {
                return resultArray[0];
            }
            return null;
        }

        function getEntityPermissionForEntity(entity, permissions) {
            var resultArray = permissions.filter(function (permission) {

                return permission.EntityPermissions.filter(function (entityPermission) {
                    return entityPermission.EntityId == entity.Id;
                }).length > 0;
            });
            if (resultArray.length == 1) {
                return resultArray[0];
            }
            return null;
        }

        function getObjectPermissionIndex(objectPermissions, object, objectId) {
            for (var i = 0; i < objectPermissions.length; i++) {
                if (objectPermissions[i].Object == object && objectPermissions[i].ObjectId == objectId) {
                    return i;
                }
            }
            return -1;
        }

        function getEntityPermissionIndex(entityPermissions, entity) {
            for (var i = 0; i < entityPermissions.length; i++) {
                if (entityPermissions[i].Entity == entity) {
                    return i;
                }
            }
            return -1;
        }

        function getObjectPermission(objectPermissions, object, objectId) {
            var permissionIndex = getObjectPermissionIndex(objectPermissions, object, objectId);
            if (permissionIndex >= 0) {
                return objectPermissions[permissionIndex];
            }
            return null;
        }

        function getEntityPermission(entityPermissions, entity) {
            var permissionIndex = getEntityPermissionIndex(entityPermissions, entity);
            if (permissionIndex >= 0) {
                return entityPermissions[permissionIndex];
            }
            return null;
        }

        function objectPermissionExists(objectPermissions, object, objectId) {
            return objectPermissions !== undefined &&
                objectPermissions != null &&
                objectPermissions.filter(function (objectPermission) {
                    return objectPermission.Object == object && 
                        objectPermission.ObjectId == objectId;
                }).length;
        }

        function setupPermission(type, entity) {
            var permission = null;
            switch (type) {
                case mdBusinessLogic.dataAccess.entities.entitiesEnum.ProfileType:
                    permission = new mdBusinessLogic.dataAccess.entities.permissions.profileTypePermissions();
                    permission.ProfileId = entity.Id;
                    break;
                case mdBusinessLogic.dataAccess.entities.entitiesEnum.User:
                    permission = new mdBusinessLogic.dataAccess.entities.permissions.userPermissions();
                    permission.UserId = entity.Id;
                    break;
            }
            return permission;
        }

        function removePermission(permissions, index) {
            if (permissions[index] !== undefined) {
                permissions[index].IsDeleted = true;
            }
        }

        function setupObjectPermission(object, objectId) {
            var objectPermission = new mdBusinessLogic.dataAccess.entities.permissions.objectPermission();
            objectPermission.Object = object;
            objectPermission.ObjectId = objectId;
            return objectPermission;
        }

        function setupEntityPermission(entity, entityId) {
            var entityPermission = new mdBusinessLogic.dataAccess.entities.permissions.entityPermission();
            entityPermission.Entity = entity;
            entityPermission.Object = entityId;
            return entityPermission;
        }

        function toggleObjectPermission(permission, object, objectId, accessType) {
            var objectPermission = getObjectPermission(permission.ObjectPermissions, object, objectId);
            if (objectPermission == null) {
                objectPermission = setupObjectPermission(object, objectId);
                permission.ObjectPermissions.push(objectPermission);
            }

            if (objectPermission.AccessTypes.indexOf(accessType) >= 0) {
                objectPermission.AccessTypes.splice(objectPermission.AccessTypes.indexOf(accessType), 1);
            } else {
                objectPermission.AccessTypes.push(accessType);
            }
        }

        function toggleEntityPermission(permission, entity, entityId, accessType) {
            var entityPermission = getEntityPermission(permission.EntityPermissions, entity);
            if (entityPermission == null) {
                entityPermission = setupEntityPermission(entity, entityId);
                permission.EntityPermissions.push(entityPermission);
            }

            if (objectPermission.AccessTypes.indexOf(accessType) >= 0) {
                objectPermission.AccessTypes.splice(objectPermission.AccessTypes.indexOf(accessType), 1);
            } else {
                objectPermission.AccessTypes.push(accessType);
            }
        }

        function getPermissionEntity(type, permission) {
            var result = [];
            switch (type) {
                case enums.entitiesEnum.ProfileType:
                    result = getAllEntities(type).filter(function (profileType) {
                        return permission.ProfileId == profileType.Id;
                    });
                    break;
                case enums.entitiesEnum.User:
                    result = getAllEntities(type).filter(function (user) {
                        return permission.UserId == user.Id;
                    });
                    break;
            }
            if (result.length > 0) {
                return result[0];
            }
            return null;
        }

        function getPermissionEntityName(type, displayProperty, permission) {
            var name = '';
            var result = getPermissionEntity(type, permission);
            if (result != null) {
                name = result[displayProperty];
            }
            return name;
        }

        function getUnusedProfileTypes(permissions, data) {
            if (data === undefined) {
                data = getAllEntities(enums.entitiesEnum.ProfileType);
            }
            return data.filter(function (profileType) {
                return !permissions.filter(function (permission) { return !permission.IsDeleted; }).map(function (permission) { return permission.ProfileId; }).includes(profileType.Id);
            });
        }

        function getUnusedUsers(permissions, data) {
            if (data === undefined) {
                data = getAllEntities(enums.entitiesEnum.User);
            }
            return data.filter(function (user) {
                return !permissions.filter(function (permission) { return !permission.IsDeleted; }).map(function (permission) { return permission.UserId; }).includes(user.Id);
            });
        }

        return {
            permissions: {
                setupPermission: setupPermission,
                getPermission: getPermission,
                getEntityName: getPermissionEntityName,
                removePermission: removePermission
            },
            generic: {
                addToEntities: addToEntities,
                getAllEntities: getAllEntities,
                queryAllEntities: queryAllEntities,
                getUnusedProfileTypes: getUnusedProfileTypes,
                getUnusedUsers: getUnusedUsers
            },
            entity: {
                setupPermission: setupEntityPermission,
                isPermissionSet: isEntityPermissionSet,
                togglePermission: toggleEntityPermission,
                getEntityForPermission: getEntityForEntityPermission,
                getPermissionForEntity: getEntityPermissionForEntity
            },
            object: {
                setupPermission: setupObjectPermission,
                isPermissionSet: isObjectPermissionSet,
                togglePermission: toggleObjectPermission,
                getEntityForPermission: getEntityForObjectPermission,
                getPermissionForEntity: getObjectPermissionForEntity
            }
        };
    }
}());