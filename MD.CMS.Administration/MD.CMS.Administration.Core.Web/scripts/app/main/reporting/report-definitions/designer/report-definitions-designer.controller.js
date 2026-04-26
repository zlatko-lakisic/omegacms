(function () {
    'use strict';

    angular
        .module('app.reporting.report_definitions.designer')
        .controller('ReportinDefinitionsDesignerController', ['$mdDialog', '$state', '$scope', '$mdMedia', 'reportDesignerJoinService', 'reportDesignerService', 'allEntities', 'reportToEdit', 'mdFeedbackService', '$q', ReportinDefinitionsDesignerController]);

    /** @ngInject */
    function ReportinDefinitionsDesignerController($mdDialog, $state, $scope, $mdMedia, reportDesignerJoinService, reportDesignerService, allEntities, reportToEdit, $mdFeedbackService, $q) {
        //Private Pttributes
        var reportDefinitionController = new mdBusinessLogic.dataAccess.controllers.reportDefinitionController();
        var vm = this

        //Public Properties
        vm.AllEntities = [];
        vm.ReportToEdit = reportToEdit;
        /*vm.Entities = [];
        vm.handleDrop = handleDrop;
        vm.handleDragEnd = handleDragEnd;
        vm.removeEntity = removeEntity;
        vm.toggleJoinFormDialog = toggleJoinFormDialog;
        vm.reportPreview = reportPreview;
        vm.loadAllColumns = loadAllColumns;
        vm.allColumns = [];
        vm.updateFieldEnabled = updateFieldEnabled;
        vm.toggleFilterFormDialog = toggleFilterFormDialog;
        vm.removeFilter = removeFilter;
        vm.reportDesignerService = reportDesignerService;
        vm.entityFilter = '';
        vm.filteredEntities = [];
        vm.EntitiesLength = 0;
        vm.save = save;*/

        //Public Methods
        vm.onTileEvent = onTileEvent;
        vm.reportPreview = reportPreview;
        vm.toggleFilterFormDialog = toggleFilterFormDialog;
        vm.removeFilter = removeFilter;
        vm.save = save;
        vm.loadDiagramData = loadDiagramData;

        //Private Methods
        function loadDiagramData() {
            return vm.ReportToEdit.Definition.Entities.map(function (en) { return { id: en.UniqueId }; });
        }

        function onTileEvent(event, data) {
            console.log(arguments);
            switch (event) {
                case 'add':
                    return add(event, data);
                case 'remove':
                    return remove(event, data);
                case 'move':
                    return move(event, data);
                case 'render':
                    return render(event, data);
                case 'edit':
                    if (data.type == 'connection') {
                        return connectDialog(event, data);
                    }
                    return customizeDialog(event, data);
                case 'connect':
                    return connectDialog(event, data, true);
                case 'connect-load':
                    return connectLoad(event, data);
            }
            return $q(function (resolve, reject) {
                resolve();
            });
            return promise;
        }

        function add(event, data) {
            return $q(function (resolve, reject) {
                vm.ReportToEdit.Definition.Entities.push(data.data);
                var addeed = vm.ReportToEdit.Definition.Entities[vm.ReportToEdit.Definition.Entities.length - 1];
                //addeed.Id = -Math.floor(mdBusinessLogic.helpers.math.random() * 9999999);
                resolve(addeed);
            });
        }

        function remove(event, data) {
            return $q(function (resolve, reject) {
                if (data.type == 'element') {
                    for (var e = vm.ReportToEdit.Definition.Entities.length - 1; e >= 0; e--) {
                        if (vm.ReportToEdit.Definition.Entities[e].UniqueId == data.data.UniqueId) {
                            var en = vm.ReportToEdit.Definition.Entities[e];

                            //Find Joins to Remove
                            var joinsToRemove = [];
                            for (var i = 0; i < vm.ReportToEdit.Definition.Joins.length; i++) {
                                var join = vm.ReportToEdit.Definition.Joins[i];
                                if (join !== undefined && (join.Left.Entity.UniqueId == en.UniqueId || join.Right.Entity.UniqueId == en.UniqueId)) {
                                    joinsToRemove.push(i);
                                }
                            }

                            //Find Filters to Remove
                            var filtersToRemove = [];
                            for (var i = 0; i < vm.ReportToEdit.Definition.Filters.length; i++) {
                                var filter = vm.ReportToEdit.Definition.Filters[i];
                                if (filter !== undefined && filter.Entity.UniqueId == en.UniqueId) {
                                    filtersToRemove.push(i);
                                }
                            }

                            //Copy Joins to Keep
                            var joinsCopy = [];
                            for (var i = 0; i < vm.ReportToEdit.Definition.Joins.length; i++) {
                                var join = vm.ReportToEdit.Definition.Joins[i];
                                if (join !== undefined && joinsToRemove.indexOf(i) < 0) {
                                    joinsCopy.push(join);
                                }
                            }
                            vm.ReportToEdit.Definition.Joins = joinsCopy;

                            //Copy Filters to Keep
                            var filtersCopy = [];
                            for (var i = 0; i < vm.ReportToEdit.Definition.Filters.length; i++) {
                                var filter = vm.ReportToEdit.Definition.Filters[i];
                                if (filter !== undefined && filtersToRemove.indexOf(i) < 0) {
                                    filtersCopy.push(filter);
                                }
                            }
                            vm.ReportToEdit.Definition.Filters = filtersCopy;

                            vm.ReportToEdit.Definition.Entities.splice(e);
                            break;
                        }
                    }
                } else {
                    for (var i = 0; i < vm.ReportToEdit.Definition.Joins.length; i++) {
                        if (vm.ReportToEdit.Definition.Joins[i].Left.Entity.UniqueId + vm.ReportToEdit.Definition.Joins[i].Right.Entity.UniqueId == data.data.Left.Entity.UniqueId + data.data.Right.Entity.UniqueId) {
                            vm.ReportToEdit.Definition.Joins.splice(i);
                            break;
                        }
                    }
                }
                resolve();
            });
        }

        function move(event, data) {
            return $q(function (resolve, reject) {
                if (data.type == 'element') {
                    for (var i = 0; i < vm.ReportToEdit.Definition.Entities.length; i++) {
                        if (vm.ReportToEdit.Definition.Entities[i].UniqueId == data.data.UniqueId) {
                            vm.ReportToEdit.Definition.Entities[i].Coordinates.x = data.tileData.x;
                            vm.ReportToEdit.Definition.Entities[i].Coordinates.y = data.tileData.y;
                            vm.ReportToEdit.Definition.Entities[i].Coordinates.height = data.tileData.height;
                            vm.ReportToEdit.Definition.Entities[i].Coordinates.width = data.tileData.width;
                            break;
                        }
                    }
                } else {

                }
                resolve();
            });
        }

        function render(event, data) {
            return $q(function (resolve, reject) {
                var clone = $(data.element).clone();
                clone.find('*[data-value]').each(function (index, el) {
                    var key = $(el).attr('data-value');
                    $(el).text(data[key]);
                });
                resolve(clone);
            });
        }

        function connectLoad(event, data) {
            return $q(function (resolve, reject) {
                resolve(vm.ReportToEdit.Definition.Joins.map(function (join) {
                    return {
                        data: join,
                        source: join.Left.Entity.UniqueId,
                        target: join.Right.Entity.UniqueId,
                        id: join.Left.Entity.UniqueId + join.Right.Entity.UniqueId
                    };
                }));
            });
        }

        function connectDialog(event, data, initial) {
            if (initial === undefined) {
                initial = false;
            }
            return $q(function (resolve, reject) {
                if (data.id === undefined) {
                    var useFullScreen = ($mdMedia('sm') || $mdMedia('xs')) && $scope.customFullscreen;
                    $mdDialog.show({
                        controller: "reportDesignerJoinFormController as vm",
                        templateUrl: 'scripts/app/main/reporting/report-definitions/designer/dialogs/report-definitions-designer-joins-form-dialog.html',
                        parent: angular.element(document.body),
                        clickOutsideToClose: false,
                        fullscreen: useFullScreen,
                        resolve: {
                            action: function () {
                                return event;
                            },
                            join: function () {
                                var join = data.data;
                                if (join === undefined) {
                                    join = new mdBusinessLogic.dataAccess.entities.innerReportDefinitionJoin();
                                    join.Left.Entity = data.source.data;
                                    join.Left.Property = '';
                                    join.Right.Entity = data.target.data;
                                    join.Right.Property = '';
                                }
                                return join.clone();
                            },
                            entities: function () {
                                return vm.ReportToEdit.Definition.Entities.slice(1, vm.ReportToEdit.Definition.Entities.length);
                            }
                        }
                    }).then(function (join) {
                        var found = false;
                        for (var i = 0; i < vm.ReportToEdit.Definition.Joins.length; i++) {
                            if (vm.ReportToEdit.Definition.Joins[i].Left.Entity.UniqueId + vm.ReportToEdit.Definition.Joins[i].Right.Entity.UniqueId == join.Left.Entity.UniqueId + join.Right.Entity.UniqueId) {
                                vm.ReportToEdit.Definition.Joins[i] = join;
                                found = true;
                                break;
                            }
                        }
                        if (!found) {
                            vm.ReportToEdit.Definition.Joins.push(join);
                        }
                        resolve({
                            data: join,
                            id: join.Left.Entity.UniqueId + join.Right.Entity.UniqueId
                        });
                    }, function () {
                        resolve();
                    });
                } else {
                    for (var i = 0; i < vm.ReportToEdit.Definition.Joins.length; i++) {
                        if (vm.ReportToEdit.Definition.Joins[i].Left.Entity.UniqueId + vm.ReportToEdit.Definition.Joins[i].Right.Entity.UniqueId == data.id) {
                            resolve({
                                data: vm.ReportToEdit.Definition.Joins[i],
                                id: data.id
                            });
                            break;
                        }
                    }
                }
            });
        }

        function customizeDialog(event, data) {
            return $q(function (resolve, reject) {
                var useFullScreen = ($mdMedia('sm') || $mdMedia('xs')) && $scope.customFullscreen;
                $mdDialog.show({
                    controller: "reportDesignerCustomizeDialogController as vm",
                    templateUrl: 'scripts/app/main/reporting/report-definitions/designer/dialogs/report-definitions-designer-customize-dialog.html',
                    parent: angular.element(document.body),
                    clickOutsideToClose: true,
                    fullscreen: useFullScreen,
                    resolve: {
                        entity: function () {
                            var entity = vm.ReportToEdit.Definition.Entities.filter(function (entity) { return entity.UniqueId == data.data.UniqueId; })[0];
                            return entity.clone();
                        }
                    }
                }).then(function (data) {
                    for (var i = 0; i < vm.ReportToEdit.Definition.Entities.length; i++) {
                        if (vm.ReportToEdit.Definition.Entities[i].UniqueId == data.UniqueId) {
                            vm.ReportToEdit.Definition.Entities[i] = data;
                            break;
                        }
                    }
                    resolve(data);
                }, function () {
                    resolve();
                });
            });
        }

        function reportPreview(event) {
            var useFullScreen = ($mdMedia('sm') || $mdMedia('xs')) && $scope.customFullscreen;
            $mdDialog.show({
                controller: "reportDesignerPreviewController",
                templateUrl: 'scripts/app/main/reporting/report-definitions/designer/dialogs/report-definitions-designer-preview-dialog.html',
                parent: angular.element(document.body),
                targetEvent: event,
                clickOutsideToClose: true,
                fullscreen: useFullScreen,
                resolve: {
                    reportDefinition: function () {
                        return vm.ReportToEdit;
                    }
                }
            });
        }

        function toggleFilterFormDialog(event, action, index) {
            var useFullScreen = ($mdMedia('sm') || $mdMedia('xs')) && $scope.customFullscreen;
            $mdDialog.show({
                controller: "reportDesignerFilterDialogController",
                templateUrl: 'scripts/app/main/reporting/report-definitions/designer/dialogs/report-definitions-designer-filter-dialog.html',
                parent: angular.element(document.body),
                targetEvent: event,
                clickOutsideToClose: true,
                fullscreen: useFullScreen,
                resolve: {
                    reportDefinition: function () {
                        return vm.ReportToEdit;
                    },
                    index: function () {
                        if (action == 'add') {
                            return -1;
                        }
                        return index;
                    }
                }
            })
                .then(function (reportDefinition) {
                    vm.ReportToEdit = reportDefinition;
                }, function () {
                });
        }

        function removeFilter(index) {
            vm.ReportToEdit.Definition.Filters.splice(index, 1);
        }

        function save() {
            vm.ReportToEdit.Json = JSON.stringify(vm.ReportToEdit.Definition)
            reportDefinitionController.save(vm.ReportToEdit, function (data) {
                $state.go('app.report_definitions_list', {}, { reload: true });
            }, function (error) { });
        }

        function init() {
            vm.AllEntities = allEntities;

            $scope.$on('md-cms-diagram-events-element-query-data', function (event, parentData) {
                if (parentData.toolbar) {
                    var entity = vm.AllEntities.filter(function (entity) { return entity.UniqueId == parentData.id; })[0];
                    $scope.$broadcast('md-cms-diagram-events-element-query-data-toolbar-' + parentData.id, {
                        data: entity,
                        tileData: null
                    });
                } else {
                    var entity = vm.ReportToEdit.Definition.Entities.filter(function (entity) { return entity.UniqueId == parentData.id; })[0];
                    $scope.$broadcast('md-cms-diagram-events-element-query-data-' + parentData.id, {
                        data: entity,
                        tileData: {
                            x: entity.Coordinates.x,
                            y: entity.Coordinates.y,
                            height: entity.Coordinates.height,
                            width: entity.Coordinates.width
                        }
                    });
                }
            });
        }

        init();

        /*

        function modifyCanvasEntities(action, entity) {
                var index = -1;
                switch (action) {
                    case 'add':
                        vm.ReportToEdit.Definition.Entities.push(entity);
                        break;
                    case 'edit':
                        for (var i = 0; i < vm.ReportToEdit.Definition.Entities.length; i++) {
                            var en = vm.ReportToEdit.Definition.Entities[i];
                            if (en.UniqueId == entity.UniqueId) {
                                vm.ReportToEdit.Definition.Entities[i] = entity;
                            }
                        }
                        break;
                    default:
                        for (var i = 0; i < vm.ReportToEdit.Definition.Entities.length; i++) {
                            var en = vm.ReportToEdit.Definition.Entities[i];
                            if (en.UniqueId == entity.UniqueId) {
                                index = i;
                                break;
                            }
                        }
                        if (index == 0) {
                            vm.ReportToEdit.Definition.Entities = [];
                            vm.ReportToEdit.Definition.Joins = [];
                            vm.ReportToEdit.Definition.Filters = [];
                        } else {
                            //Find Joins to Remove
                            var joinsToRemove = [];
                            for (var i = 0; i < vm.ReportToEdit.Definition.Joins.length; i++) {
                                var join = vm.ReportToEdit.Definition.Joins[i];
                                if (join !== undefined && (join.Left.Entity.UniqueId == en.UniqueId || join.Right.Entity.UniqueId == en.UniqueId)) {
                                    joinsToRemove.push(i);
                                }
                            }

                            //Find Filters to Remove
                            var filtersToRemove = [];
                            for (var i = 0; i < vm.ReportToEdit.Definition.Filters.length; i++) {
                                var filter = vm.ReportToEdit.Definition.Filters[i];
                                if (filter !== undefined && filter.Entity.UniqueId == en.UniqueId) {
                                    filtersToRemove.push(i);
                                }
                            }

                            //Copy Joins to Keep
                            var joinsCopy = [];
                            for (var i = 0; i < vm.ReportToEdit.Definition.Joins.length; i++) {
                                var join = vm.ReportToEdit.Definition.Joins[i];
                                if (join !== undefined && joinsToRemove.indexOf(i) < 0) {
                                    joinsCopy.push(join);
                                }
                            }
                            vm.ReportToEdit.Definition.Joins = joinsCopy;

                            //Copy Filters to Keep
                            var filtersCopy = [];
                            for (var i = 0; i < vm.ReportToEdit.Definition.Filters.length; i++) {
                                var filter = vm.ReportToEdit.Definition.Filters[i];
                                if (filter !== undefined && filtersToRemove.indexOf(i) < 0) {
                                    filtersCopy.push(filter);
                                }
                            }
                            vm.ReportToEdit.Definition.Filters = filtersCopy;



                            vm.ReportToEdit.Definition.Entities.splice(index, 1);
                        }
                }

                vm.Entities = [];
                for (var i = 0; i < vm.AllEntities.length; i++) {
                    var exists = false;
                    for (var j = 0; j < vm.ReportToEdit.Definition.Entities.length; j++) {
                        if (vm.AllEntities[i].UniqueId == vm.ReportToEdit.Definition.Entities[j].UniqueId) {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists) {
                        vm.Entities.push(vm.AllEntities[i]);
                    }
                }

                vm.EntitiesLength = vm.ReportToEdit.Definition.Entities.length;
                renderJoinContainer();
                vm.ReportToEdit.Json = JSON.stringify(vm.ReportToEdit.Definition)
        }

        function renderJoinContainer() {
            setTimeout(function () {
                var joinContainer = angular.element('#joinContainer');
                var reportCanvas = angular.element('#reportCanvas');
                var svgContainer = angular.element('#svgContainer');
                joinContainer.css({ "z-index": 0, "width": reportCanvas[0].clientWidth + "px", "height": reportCanvas[0].clientHeight + "px" });
                svgContainer.css({ "z-index": 0 });
                joinContainer.attr("height", reportCanvas[0].clientHeight);
                joinContainer.attr("width", reportCanvas[0].clientWidth);
                for (var i = 0; i < vm.ReportToEdit.Definition.Joins.length; i++) {
                    var join = vm.ReportToEdit.Definition.Joins[i];
                    reportDesignerJoinService.connectElements(
                        svgContainer,
                        angular.element('#join' + i),
                        angular.element('#entity' + join.Left.Entity.UniqueId),
                        angular.element('#entity' + join.Right.Entity.UniqueId)
                    );
                }
            }, 20);
        }

        function modifyCanvasJoins(action, join) {
            var index = -1;
            switch (action) {
                case 'add':                   
                    vm.ReportToEdit.Definition.Joins.push(join);
                    break;
                case 'edit':
                    for (var i = 0; i < vm.ReportToEdit.Definition.Joins.length; i++) {
                        var en = vm.ReportToEdit.Definition.Joins[i];
                        if (en.Left.Entity.UniqueId == join.Left.Entity.UniqueId && en.Right.Entity.UniqueId == join.Right.Entity.UniqueId) {
                            vm.ReportToEdit.Definition.Joins[i] = join;
                        }
                    }
                    break;
                default:
                    for (var i = 0; i < vm.ReportToEdit.Definition.Joins.length; i++) {
                        var en = vm.ReportToEdit.Definition.Joins[i];
                        if (en.Left.Entity.UniqueId == join.Left.Entity.UniqueId && en.Right.Entity.UniqueId == join.Right.Entity.UniqueId) {
                            index = i;
                            break;
                        }
                    }
                    vm.ReportToEdit.Definition.Joins.splice(index, 1);
            }
            renderJoinContainer();
            vm.ReportToEdit.Json = JSON.stringify(vm.ReportToEdit.Definition)
        }

        function handleDrop(item) {
            $scope.$apply(function () {
                modifyCanvasEntities('add', item);
            });
        }

        function handleDragEnd() {
            renderJoinContainer();
            for (var i = 0; i < vm.ReportToEdit.Definition.Entities.length; i++) {
                vm.ReportToEdit.Definition.Entities[i] = JSON.parse(angular.element('#entity' + vm.ReportToEdit.Definition.Entities[i].UniqueId).attr('report-designer-entity'));
                vm.ReportToEdit.Definition.Entities[i].Coordinates.x = Math.round(vm.ReportToEdit.Definition.Entities[i].Coordinates.x);
                vm.ReportToEdit.Definition.Entities[i].Coordinates.y = Math.round(vm.ReportToEdit.Definition.Entities[i].Coordinates.y);
            }
        }

        function removeEntity(item) {
            modifyCanvasEntities('remove', item);
        }

        function toggleJoinFormDialog(event, action, item) {
            var useFullScreen = ($mdMedia('sm') || $mdMedia('xs')) && $scope.customFullscreen;
            $mdDialog.show({
                controller: "reportDesignerJoinFormController",
                templateUrl: 'scripts/app/main/reporting/report-definitions/designer/dialogs/report-definitions-designer-joins-form-dialog.html',
                parent: angular.element(document.body),
                targetEvent: event,
                clickOutsideToClose: true,
                fullscreen: useFullScreen,
                resolve: {
                    action: function () {
                        return action;
                    },
                    join: function () {
                        if (action == 'add') {
                            var join = new mdBusinessLogic.dataAccess.entities.innerReportDefinitionJoin();
                            join.Left = {
                                Entity: item,
                                Property: ''
                            }
                            return join;
                        }
                        return item;
                    },
                    entities: function () {
                        return vm.ReportToEdit.Definition.Entities.slice(1, vm.ReportToEdit.Definition.Entities.length);
                    }
                }
            })
            .then(function (item) {
                modifyCanvasJoins(action, item);
            }, function () {
            });
        }
        
        $scope.$watch('vm.entityFilter', filterEntities);

        $scope.$watch('vm.Entities', filterEntities);

        function filterEntities() {
            var lowerCaseText = vm.entityFilter.toLowerCase().trim();
            vm.filteredEntities = [];
            if (lowerCaseText.length > 0) {
                for (var i = 0; i < vm.Entities.length; i++) {
                    var lowerCaseEntityName = vm.Entities[i].Name.toLowerCase().trim();
                    if (lowerCaseEntityName.indexOf(lowerCaseText) >= 0) {
                        vm.filteredEntities.push(vm.Entities[i]);
                    }
                }
            } else {
                vm.filteredEntities = vm.Entities;
                //var maxEntities = vm.Entities.length > 10 ? 10 : vm.Entities;
                //for (var i = 0; i < maxEntities - 1; i++) {
                //    vm.filteredEntities.push(vm.Entities[i]);
                //}
            }
        }

        //Step 2 Methods
        function loadAllColumns() {
            reportDefinitionController.getReportColumns(vm.ReportToEdit, function (data) {
                $scope.$apply(function () {
                    vm.allColumns = data.columns;
                });
            }, function (error) { });
        }

        function updateFieldEnabled(entity, field, value) {
            for (var i = 0; i < vm.ReportToEdit.Definition.Entities.length; i++) {
                if (vm.ReportToEdit.Definition.Entities[i].Name == entity.Name) {
                    var found = false;
                    for (var bf = 0; bf < vm.ReportToEdit.Definition.Entities[i].BaseFields.length; bf++) {
                        if (!found && vm.ReportToEdit.Definition.Entities[i].BaseFields[bf].Name == field.Name) {
                            vm.ReportToEdit.Definition.Entities[i].BaseFields[bf].Enabled = value;
                            found = true;
                        }
                    }

                    for (var f = 0; f < vm.ReportToEdit.Definition.Entities[i].Fields.length; f++) {
                        if (!found && vm.ReportToEdit.Definition.Entities[i].Fields[f].Name == field.Name) {
                            vm.ReportToEdit.Definition.Entities[i].Fields[f].Enabled = value;
                            found = true;
                        }
                    }

                    if (!found) {
                        for (var ef = 0; ef < vm.ReportToEdit.Definition.Entities[i].ExtendedFields.length; ef++) {
                            if (vm.ReportToEdit.Definition.Entities[i].ExtendedFields[ef].Name == field.Name) {
                                vm.ReportToEdit.Definition.Entities[i].ExtendedFields[ef].Enabled = value;
                            }
                        }
                    }

                    //vm.ReportToEdit.Definition.Entities[i].Fields = vm.ReportToEdit.Definition.Entities[i].BaseFields.concat(vm.ReportToEdit.Definition.Entities[i].ExtendedFields);
                    break;
                }
            }
            vm.ReportToEdit.Json = JSON.stringify(vm.ReportToEdit.Definition)
        }

        //Step 3 Methods
        function toggleFilterFormDialog(event, action, index) {
            var useFullScreen = ($mdMedia('sm') || $mdMedia('xs')) && $scope.customFullscreen;
            $mdDialog.show({
                controller: "reportDesignerFilterDialogController",
                templateUrl: 'scripts/app/main/reporting/report-definitions/designer/dialogs/report-definitions-designer-filter-dialog.html',
                parent: angular.element(document.body),
                targetEvent: event,
                clickOutsideToClose: true,
                fullscreen: useFullScreen,
                resolve: {
                    reportDefinition: function () {
                        return vm.ReportToEdit;
                    }, 
                    index: function () {
                        if (action == 'add') {
                            return -1;
                        }
                        return index;
                    }
                }
            })
            .then(function (reportDefinition) {
                vm.ReportToEdit = reportDefinition;
            }, function () {
            });
        }

        function removeFilter(index) {
            vm.ReportToEdit.Definition.Filters.splice(index, 1);
        }

        //Global Methods
        function save() {
            vm.ReportToEdit.Json = JSON.stringify(vm.ReportToEdit.Definition)
            reportDefinitionController.save(vm.ReportToEdit, function (data) {
                $state.go('app.report_definitions_list', {  }, { reload: true });
            }, function (error) { });
        }

        //Init
        vm.AllEntities = allEntities;
        vm.Entities = vm.AllEntities;
        $('.draggableEntities').draggable();
        renderJoinContainer();

        $scope.$on('md-cms-diagram-events-element-query-data', function (event, parentData) {
            if (parentData.toolbar) {
                var entity = vm.filteredEntities.filter(function (entity) { return entity.UniqueId == parentData.id; })[0];
                $scope.$broadcast('md-cms-diagram-events-element-query-data-' + parentData.id, {
                    data: entity,
                    tileData: null
                });
            } else {
                
            }
        });*/
    }
})();
