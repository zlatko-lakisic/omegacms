(function ()
{
    'use strict';

    angular
        .module('omega')
        .controller('IndexController', ['fuseTheming', '$scope', '$timeout', 'msNavigationService', 'mdPermissionAuthenticateService', IndexController]);

    /** @ngInject */
    function IndexController(fuseTheming, $scope, $timeout, msNavigationService, mdPermissionAuthenticateService)
    {
        var vm = this;
        var userLoggedIn = false;
        var permissionControllerProfileType = new mdBusinessLogic.dataAccess.controllers.permissionControllerProfileType();
        var permissionControllerUser = new mdBusinessLogic.dataAccess.controllers.permissionControllerUser();

        // Data
        vm.themes = fuseTheming.themes;

        function loadContentNav(action, value) {
            var folderController = new mdBusinessLogic.dataAccess.controllers.folderController();
            switch (action) {
                case 'save':
                    msNavigationService.deleteItem('main.content-list.' + value.FolderPath.replace('Root/').split('/').join('.'));
                    msNavigationService.saveItem('main.content-list.' + value.FolderPath.replace('Root/').split('/').join('.'), {
                        title: value.Name,
                        icon: value.Children.length > 0 ? 'icon-folder-multiple' : 'icon-folder',
                        state: 'app.content_list',
                        weight: 99,
                        noTransform: true,
                        linkAndSub: true,
                        stateParams: {
                            folderPath: value.FolderPath,
                            currentView: 'grid'
                        },
                        collapseDisabled: true
                    }, true);
                    break;
                case 'remove':
                    msNavigationService.deleteItem('main.content-list.' + value.FolderPath.replace('Root/').split('/').join('.'));
                    break;
                case 'load':
                    msNavigationService.saveItem('main.content-list', {
                        title: 'Menus.MainContentContent',
                        icon: 'icon-folder-multiple',
                        state: 'app.content_list',
                        weight: 1,
                        collapseDisabled: true,
                        callback: function ($scope, $q) {
                            return $q(function (resolve, reject) {
                                folderController.getHierarchyByParentId(0, false, function (data) {
                                    function addChildren(folders) {
                                        for (var i = 0; i < folders.length; i++) {
                                            var folder = folders[i];
                                            $scope.$apply(function () {
                                                msNavigationService.saveItem('main.content-list.' + folder.FolderPath.replace('Root/').split('/').join('.'), {
                                                    title: folder.Name,
                                                    icon: folder.Children.length > 0 ? 'icon-folder-multiple' : 'icon-folder',
                                                    state: 'app.content_list',
                                                    weight: i,
                                                    noTransform: true,
                                                    linkAndSub: true,
                                                    stateParams: {
                                                        folderPath: folder.FolderPath,
                                                        currentView: 'grid'
                                                    },
                                                    collapseDisabled: true
                                                }, true);
                                            });
                                            addChildren(folder.Children);
                                        }
                                    }
                                    data = Array.isArray(data) ? data[0] : data;
                                    if (data != undefined && data != null) {
                                        addChildren(data.Children);
                                    }
                                    resolve();
                                    //$scope.$broadcast('LoadMenuFromDirective');
                                }, function (error) {
                                    resolve();
                                });
                            });
                        }
                    });
                    break;
            }
        }

        function loadMediaContentNav(action, value) {

            var folderController = new mdBusinessLogic.dataAccess.controllers.folderController();
            switch (action) {
                case 'save':
                    msNavigationService.deleteItem('main.mediacontent-list.' + value.FolderPath.replace('Root/').split('/').join('.'));
                    msNavigationService.saveItem('main.mediacontent-list.' + value.FolderPath.replace('Root/').split('/').join('.'), {
                        title: value.Name,
                        icon: value.Children.length > 0 ? 'icon-folder-multiple' : 'icon-folder',
                        state: 'app.content_list',
                        weight: 99,
                        noTransform: true,
                        linkAndSub: true,
                        stateParams: {
                            folderPath: value.FolderPath,
                            currentView: 'grid'
                        },
                        collapseDisabled: true
                    }, true);
                    break;
                case 'remove':
                    msNavigationService.deleteItem('main.mediacontent-list.' + value.FolderPath.replace('Root/').split('/').join('.'));
                    break;
                case 'load':
                    msNavigationService.saveItem('main.mediacontent-list', {
                        title: 'Menus.MainContentMediaContent',
                        icon: 'icon-folder-multiple-image',
                        state: 'app.mediacontent_list',
                        weight: 4,
                        collapseDisabled: true,
                        callback: function ($scope, $q) {
                            return $q(function (resolve, reject) {
                                folderController.getHierarchyByParentId(0, false, function (data) {
                                    function addChildren(folders) {
                                        for (var i = 0; i < folders.length; i++) {
                                            var folder = folders[i];
                                            $scope.$apply(function () {
                                                msNavigationService.saveItem('main.mediacontent-list.' + folder.FolderPath.replace('Root/').split('/').join('.'), {
                                                    title: folder.Name,
                                                    icon: folder.Children.length > 0 ? 'icon-folder-multiple-image' : 'icon-folder-image',
                                                    state: 'app.mediacontent_list',
                                                    weight: i,
                                                    noTransform: true,
                                                    linkAndSub: true,
                                                    stateParams: {
                                                        folderPath: folder.FolderPath,
                                                        currentView: 'grid'
                                                    },
                                                    collapseDisabled: true
                                                }, true);
                                            });
                                            addChildren(folder.Children);
                                        }
                                    }
                                    data = Array.isArray(data) ? data[0] : data;
                                    if (data != undefined && data != null) {
                                        addChildren(data.Children);
                                    }
                                    resolve();
                                    //$scope.$broadcast('LoadMenuFromDirective');
                                }, function (error) {
                                    resolve();
                                });
                            });
                        }
                    });
                    break;
            }
        }

        function loadMenuContentNav(action, value) {
            var menuController = new mdBusinessLogic.dataAccess.controllers.menuController();
            switch (action) {
                case 'save':
                    msNavigationService.deleteItem('main.menu-list.' + value.MenuPath.replace('Root/').split('/').join('.'));
                    msNavigationService.saveItem('main.menu-list.' + value.MenuPath.replace('Root/').split('/').join('.'), {
                        title: value.Name,
                        icon: value.Children.length > 0 ? 'icon-menu' : 'icon-menu',
                        state: 'app.menu_list',
                        weight: 99,
                        noTransform: true,
                        linkAndSub: true,
                        stateParams: {
                            menuPath: value.MenuPath,
                            currentView: 'grid'
                        },
                        collapseDisabled: true
                    }, true);
                    break;
                case 'remove':
                    msNavigationService.deleteItem('main.menu-list.' + value.MenuPath.replace('Root/').split('/').join('.'));
                    break;
                case 'load':
                    msNavigationService.saveItem('main.menu-list', {
                        title: 'Menus.MainContentMenu',
                        icon: 'icon-menu',
                        state: 'app.menu_list',
                        weight: 4,
                        collapseDisabled: true,
                        callback: function ($scope, $q) {
                            return $q(function (resolve, reject) {
                                menuController.getAll(function (data) {
                                    var menus = data.filter(function (menu) { return menu.Name != 'Root' });
                                    for (var i = 0; i < menus.length; i++) {
                                        var menu = menus[i];
                                        $scope.$apply(function () {
                                            msNavigationService.saveItem('main.menu-list.' + menu.MenuPath.replace('Root/').split('/').join('.'), {
                                                title: menu.Name,
                                                icon: 'icon-menu',
                                                state: 'app.menu_list',
                                                weight: i,
                                                noTransform: true,
                                                linkAndSub: true,
                                                stateParams: {
                                                    menuPath: menu.MenuPath,
                                                    currentView: 'grid'
                                                },
                                                collapseDisabled: true
                                            }, true);
                                        });
                                    }
                                    resolve();
                                    //$scope.$broadcast('LoadMenuFromDirective');
                                }, function (error) {
                                    resolve();
                                });
                            });
                        }
                    });
                    break;
            }
        }

        function loadTaxonomyContentNav(action, value) {

            var taxonomyController = new mdBusinessLogic.dataAccess.controllers.taxonomyController();
            switch (action) {
                case 'save':
                    msNavigationService.deleteItem('main.taxonomy-list.' + value.TaxonomyPath.replace('Root/').split('/').join('.'));
                    msNavigationService.saveItem('main.taxonomy-list.' + value.TaxonomyPath.replace('Root/').split('/').join('.'), {
                        title: value.Name,
                        icon: value.Children.length > 0 ? 'icon-checkbox-multiple-marked' : 'icon-checkbox-multiple',
                        state: 'app.taxonomy_list',
                        weight: 99,
                        noTransform: true,
                        linkAndSub: true,
                        stateParams: {
                            taxonomyPath: value.TaxonomyPath,
                            currentView: 'grid'
                        },
                        collapseDisabled: true
                    }, true);
                    break;
                case 'remove':
                    msNavigationService.deleteItem('main.taxonomy-list.' + value.TaxonomyPath.replace('Root/').split('/').join('.'));
                    break;
                case 'load':
                    msNavigationService.saveItem('main.taxonomy-list', {
                        title: 'Menus.MainContentTaxonomy',
                        icon: 'icon-checkbox-multiple-marked',
                        state: 'app.taxonomy_list',
                        weight: 3,
                        collapseDisabled: true,
                        callback: function ($scope, $q) {
                            return $q(function (resolve, reject) {
                                taxonomyController.getHierarchyByParentId(0, false, function (data) {
                                    function addChildren(taxonomies) {
                                        for (var i = 0; i < taxonomies.length; i++) {
                                            var taxonomy = taxonomies[i];
                                            $scope.$apply(function () {
                                                msNavigationService.saveItem('main.taxonomy-list.' + taxonomy.TaxonomyPath.replace('Root/').split('/').join('.'), {
                                                    title: taxonomy.Name,
                                                    icon: taxonomy.Children.length > 0 ? 'icon-checkbox-multiple-marked' : 'icon-checkbox-multiple',
                                                    state: 'app.taxonomy_list',
                                                    weight: i,
                                                    noTransform: true,
                                                    linkAndSub: true,
                                                    stateParams: {
                                                        taxonomyPath: taxonomy.TaxonomyPath,
                                                        currentView: 'grid'
                                                    },
                                                    collapseDisabled: true
                                                }, true);
                                            });
                                            addChildren(taxonomy.Children);
                                        }
                                    }
                                    data = Array.isArray(data) ? data[0] : data;
                                    if (data != undefined && data != null) {
                                        addChildren(data.Children);
                                    }
                                    resolve();
                                    //$scope.$broadcast('LoadMenuFromDirective');
                                }, function (error) {
                                    resolve();
                                });
                            });
                        }
                    });
                    break;
            }
        }

        function loadNav(evt, data) {
            if (evt === undefined) {
                evt = {};
            }

            if (data === undefined) {
                data = {
                    type: 'all',
                    action: 'load',
                    value: {}
                };
            }

            switch (data.type) {
                case mdBusinessLogic.dataAccess.entities.entitiesEnum.Content:
                case mdBusinessLogic.dataAccess.entities.entitiesEnum.MediaContent:
                    loadContentNav(data.action, data.value);
                    loadMediaContentNav(data.action, data.value);
                    break;
                case mdBusinessLogic.dataAccess.entities.entitiesEnum.Menu:
                    loadMenuContentNav(data.action, data.value);
                    break;
                case mdBusinessLogic.dataAccess.entities.entitiesEnum.Taxonomy:
                    loadTaxonomyContentNav(data.action, data.value);
                    break;
                case 'all':
                    loadContentNav(data.action, data.value);
                    loadMediaContentNav(data.action, data.value);
                    loadMenuContentNav(data.action, data.value);
                    loadTaxonomyContentNav(data.action, data.value);
                    break;
            }

            $timeout(function () {
                $scope.$broadcast('LoadMenuFromDirective');
            });
        }
        
        $scope.$on('LoadNav', loadNav);

        $scope.$on('NavigationLoaded', function () {
            if (userLoggedIn) {
                loadNav();
            }
        });

        $scope.$watch(function () {
            return mdBusinessLogic.globals.loggedOnUser;
        }, function (loggedOnUserNewValue, loggedOnUserOldValue) {
            userLoggedIn = loggedOnUserNewValue !== undefined && loggedOnUserNewValue != null;
            if (
                (loggedOnUserNewValue !== undefined && loggedOnUserNewValue != null) &&
                (loggedOnUserOldValue === undefined || loggedOnUserOldValue == null)
            ) {
                if (mdPermissionAuthenticateService.getLoggedOnProfileTypePermissions() == null ||
                    mdPermissionAuthenticateService.getLoggedOnProfileTypePermissions().length == 0) {
                    permissionControllerProfileType.getLoggedOnProfileTypePermissions(function (data) {
                        mdPermissionAuthenticateService.setLoggedOnProfileTypePermissions(data);
                    }, function (error) {
                    });
                }
                if (mdPermissionAuthenticateService.getLoggedOnUserPermissions() == null ||
                    mdPermissionAuthenticateService.getLoggedOnUserPermissions().length == 0) {
                    permissionControllerUser.getLoggedOnUserPermissions(function (data) {
                        mdPermissionAuthenticateService.setLoggedOnUserPermissions(data);
                    }, function (error) {
                    });
                }
            }
        });

        mdPermissionAuthenticateService.onPermissionsInitialLoadedPromise().then(function () {
            mdBusinessLogic.settings.admin.onEvent(mdBusinessLogic.settings.adminEventTypes.onLogedInAndPermissionsLoaded, $scope);
        });
    }
})();
