(function () {
    'use strict';

    angular
        .module('app.core')
        .factory('mdAutocompleteService', ['$q', mdAutocompleteService]);

    /** @ngInject */
    function mdAutocompleteService($q) {

        function setup(scope) {
            var profileTypeController = new mdBusinessLogic.dataAccess.controllers.profileTypeController();
            var taxonomyController = new mdBusinessLogic.dataAccess.controllers.taxonomyController();
            var userController = new mdBusinessLogic.dataAccess.controllers.userController();
            var contentController = new mdBusinessLogic.dataAccess.controllers.contentController();
            var mediaContentController = new mdBusinessLogic.dataAccess.controllers.mediaContentController();
            var contentTypeDefinitionController = new mdBusinessLogic.dataAccess.controllers.contentTypeDefinitionController();
            var allEntities = [];

            function deduplicate(items) {
                if (scope.items !== undefined && scope.items.length > 0) {
                    for (var i = items.length - 1; i >= 0; i--) {
                        if (scope.items.filter(function (item) { return item.Id == items[i].Id; }).length > 0) {
                            items.splice(i, 1);
                        }
                    }
                }
                return items;
            }

            function querySearch(query) {
                var deferred = $q.defer();

                if (query && query.length > 0) {
                    switch (scope.mdType) {
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.ProfileType:
                            profileTypeController.search({ searchTerm: query }, function (results) {
                                fillAllEntities(results);
                                deferred.resolve(deduplicate(allEntities));
                            }, function (error) {
                                deferred.resolve(function () { return []; });
                            });
                            break;
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.User:
                            userController.search({ searchTerm: query }, function (results) {
                                fillAllEntities(results);
                                deferred.resolve(deduplicate(allEntities));
                            }, function (error) {
                                deferred.resolve(function () { return []; });
                            });
                            break;
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.Taxonomy:
                            taxonomyController.GetByParentIdWithPagination({
                                parentId: 1,
                                pageIndex: 0,
                                pageSize: 10,
                                searchTerm: encodeURI(query)
                            }, function (results) {
                                fillAllEntities(results.Items);
                                deferred.resolve(deduplicate(allEntities));
                            }, function (error) {
                                deferred.resolve(function () { return []; });
                            });
                            break;
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.Content:
                            contentController.search({ searchTerm: query }, function (data) {
                                fillAllEntities(data);
                                deferred.resolve(deduplicate(allEntities));
                            }, function (error) {
                            });
                            break;
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.MediaContent:
                            mediaContentController.search({ searchTerm: query }, function (data) {
                                fillAllEntities(data);
                                deferred.resolve(deduplicate(allEntities));
                            }, function (error) {
                                deferred.resolve([]);
                            });
                            break;
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.ContentTypeDefinition:
                            contentTypeDefinitionController.getAll(function (data) {
                                data = data.filter(function (ct) { return ct.Name.indexOf(query) >= 0; });
                                fillAllEntities(data);
                                deferred.resolve(deduplicate(allEntities));
                            }, function (error) {
                            });
                            break;
                        default:
                            fillAllEntities(scope.mdSelectedItem.value.split(scope.mdSelectedItem.delimiter));
                            deferred.resolve([query]);
                    }
                } else {
                    deferred.resolve(allEntities);
                }

                return deferred.promise;
            }

            function selectedItemChange(entity) {
                if (entity != undefined && entity != null) {
                    scope.mdSelectedEntity = entity; 
                    switch (scope.mdType) {
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.ProfileType:
                            scope.mdSelectedItem.value = entity.Id;
                            break;
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.User:
                            scope.mdSelectedItem.value = entity.Id;
                            break;
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.Taxonomy:
                            scope.mdSelectedItem.value = entity.Id;
                            break;
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.Content:
                            scope.mdSelectedItem.value = entity.UniqueId;
                            break;
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.MediaContent:
                            scope.mdSelectedItem.value = entity.Id + scope.mdSelectedItem.delimiter + entity.PreviewUrl;
                            break;
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.ContentTypeDefinition:
                            scope.mdSelectedItem.value = entity.Id;
                            break;
                        default:
                            scope.mdSelectedItem.value = entity;
                    }
                }
                return scope.mdSelectedItem.value;
            }

            function fillAllEntities(_allEntities) {
                for (var i = 0; i < _allEntities.length; i++) {
                    switch (scope.mdType) {
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.ProfileType:
                            _allEntities[i]._displayText = _allEntities[i].Name;
                            break;
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.User:
                            _allEntities[i]._displayText = _allEntities[i].Username;
                            break;
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.Taxonomy:
                            _allEntities[i]._displayText = _allEntities[i].Name;
                            break;
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.Content:
                            _allEntities[i]._displayText = _allEntities[i].Title;
                            break;
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.MediaContent:
                            _allEntities[i]._displayText = _allEntities[i].Name;
                            break;
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.ContentTypeDefinition:
                            _allEntities[i]._displayText = _allEntities[i].Name;
                            break;
                    }
                }
                allEntities = _allEntities;
            }

            function transformItemToChip($chip) {
                switch (scope.mdType) {
                    case mdBusinessLogic.dataAccess.entities.entitiesEnum.ProfileType:
                        return $chip.Name;
                    case mdBusinessLogic.dataAccess.entities.entitiesEnum.User:
                        return $chip.Username;
                    case mdBusinessLogic.dataAccess.entities.entitiesEnum.Taxonomy:
                        return $chip.Name;
                    case mdBusinessLogic.dataAccess.entities.entitiesEnum.Content:
                        return $chip.Title;
                    case mdBusinessLogic.dataAccess.entities.entitiesEnum.MediaContent:
                        return $chip.Title;
                    case mdBusinessLogic.dataAccess.entities.entitiesEnum.ContentTypeDefinition:
                        return $chip.Name;
                    default:
                        return $chip;
                }
            }

            function addItemToList($chip, $index) {
                scope.mdSelectedItem.value = scope.items.map(function (item) {
                    switch (scope.mdType) {
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.ProfileType:
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.User:
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.Taxonomy:
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.Content:
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.MediaContent:
                        case mdBusinessLogic.dataAccess.entities.entitiesEnum.ContentTypeDefinition:
                            return item.Id;
                        default:
                            return item;
                    }
                }).join(scope.mdSelectedItem.delimiter);
            }

            function removeItemFromList($chip, $index) {
                scope.mdSelectedItem.value = scope.items.map(function (item) { return item.Id; }).join(scope.mdSelectedItem.delimiter);
            }

            function init(isMultiple) {
                if (isMultiple === undefined) {
                    isMultiple = false;
                }
                scope.isMultiple = isMultiple;
                scope.typeString = mdBusinessLogic.dataAccess.entities.entitiesEnum[scope.mdType];
                return updateEntity();
            }

            function updateEntity() {
                var promise = $q(function (resolve, reject) {
                    if (scope.mdSelectedItem !== undefined && scope.mdSelectedItem.value != null) {
                        switch (scope.mdType) {
                            case mdBusinessLogic.dataAccess.entities.entitiesEnum.ProfileType:
                                scope.displayProperty = 'Name';

                                var id = 0;
                                if (scope.mdSelectedItem.value.split(scope.mdSelectedItem.delimiter).length) {
                                    id = scope.mdSelectedItem.value.split(scope.mdSelectedItem.delimiter)[0];
                                }
                                if (id > 0) {
                                    profileTypeController.getById(id, function (data) {
                                        scope.$apply(function () {
                                            scope.entity = data;
                                            scope.entity._displayText = data.Name;
                                        });
                                        resolve(data);
                                    }, function (error) {
                                        reject(error);
                                    });
                                }
                                break;
                            case mdBusinessLogic.dataAccess.entities.entitiesEnum.User:
                                scope.displayProperty = 'Username';

                                var id = 0;
                                if (scope.mdSelectedItem.value.split(scope.mdSelectedItem.delimiter).length) {
                                    id = scope.mdSelectedItem.value.split(scope.mdSelectedItem.delimiter)[0];
                                }
                                if (id > 0) {
                                    userController.getById(id, function (data) {
                                        scope.$apply(function () {
                                            scope.entity = data;
                                            scope.entity._displayText = data.Username;
                                        });
                                        resolve(data);
                                    }, function (error) {
                                        reject(error);
                                    });
                                }
                                break;
                            case mdBusinessLogic.dataAccess.entities.entitiesEnum.Taxonomy:
                                scope.displayProperty = 'Title';

                                function processTaxonomy(id, callback) {
                                    if (!isNaN(id) && id > 0) {
                                        taxonomyController.getById(id, function (data) {
                                            scope.$apply(function () {
                                                data._displayText = data.TaxonomyPath;
                                                callback(data);
                                            });
                                            resolve(data);
                                        }, function (error) {
                                            reject(error);
                                        });
                                    }
                                }
                                if (scope.isMultiple) {
                                    var ids = scope.mdSelectedItem.value.split(scope.mdSelectedItem.delimiter);
                                    for (var i in ids) {
                                        var id = ids[i];
                                        processTaxonomy(id, function (data) {
                                            scope.items.push(data);
                                        });
                                    }
                                } else {
                                    processTaxonomy(scope.mdSelectedItem.value, function (data) {
                                        scope.entity = data;
                                    });
                                }
                                break;
                            case mdBusinessLogic.dataAccess.entities.entitiesEnum.Content:
                                scope.displayProperty = 'Title';
                                var id = 0;
                                if (scope.mdSelectedItem.value.split(scope.mdSelectedItem.delimiter).length) {
                                    id = scope.mdSelectedItem.value.split(scope.mdSelectedItem.delimiter)[0];
                                }
                                if (id == scope.mdSelectedItem.value && scope.mdSelectedItem.value.split('-').length) {
                                    id = scope.mdSelectedItem.value.split('-')[0];
                                }
                                if (id > 0) {
                                    contentController.getById(id, true, mdBusinessLogic.settings.lcid, false, scope.mdSelectedItem.dataBound, scope.mdSelectedItem.jsonField.getRelevantConstraint().contentTypeId, function (data) {
                                        scope.$apply(function () {
                                            scope.entity = data;
                                            scope.entity._displayText = data.Title;
                                        });
                                        resolve(data);
                                    }, function (error) {
                                        reject(error);
                                    });
                                }
                                break;
                            case mdBusinessLogic.dataAccess.entities.entitiesEnum.MediaContent:
                                scope.displayProperty = 'Name';
                                var id = 0;
                                if (scope.mdSelectedItem.value.split(scope.mdSelectedItem.delimiter).length) {
                                    id = scope.mdSelectedItem.value.split(scope.mdSelectedItem.delimiter)[0];
                                }
                                if (id == scope.mdSelectedItem.value && scope.mdSelectedItem.value.split('-').length) {
                                    id = scope.mdSelectedItem.value.split('-')[0];
                                }
                                if (id > 0) {
                                    mediaContentController.getById(id, mdBusinessLogic.settings.lcid, function (data) {
                                        scope.$apply(function () {
                                            scope.entity = data;
                                            scope.entity._displayText = data.Name;
                                        });
                                        resolve(data);
                                    }, function (error) {
                                        reject(error);
                                    });
                                }
                                break;
                            case mdBusinessLogic.dataAccess.entities.entitiesEnum.ContentTypeDefinition:
                                scope.displayProperty = 'Name';

                                var id = 0;
                                if (scope.mdSelectedItem.value.split(scope.mdSelectedItem.delimiter).length) {
                                    id = scope.mdSelectedItem.value.split(scope.mdSelectedItem.delimiter)[0];
                                }
                                if (id > 0) {
                                    contentTypeDefinitionController.getById(id, function (data) {
                                        scope.$apply(function () {
                                            scope.entity = data;
                                            scope.entity._displayText = data.Name;
                                        });
                                        resolve(data);
                                    }, function (error) {
                                        reject(error);
                                    });
                                }
                                break;
                            default:
                                if (scope.isMultiple) {
                                    scope.items = scope.mdSelectedItem.value.split(scope.mdSelectedItem.delimiter);
                                } else {
                                    scope.entity = {};
                                    scope.entity._displayText = '';
                                }
                        }
                    } else {
                        resolve();
                    }
                });
                promise.then();
                return promise;
            }

            return {
                querySearch: querySearch,
                selectedItemChange: selectedItemChange,
                transformItemToChip: transformItemToChip,
                addItemToList: addItemToList,
                removeItemFromList: removeItemFromList,
                updateEntity: updateEntity,
                init: init
            }
        }

        return {
            setup: setup
        };

    }
}());
