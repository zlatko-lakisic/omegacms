(function () {
    'use strict';

    angular
        .module('app.taxonomy.forms')
        .controller('TaxonomyFormController', ['$mdDialog', '$rootScope', '$state', '$scope', 'taxonomy', /*'contents', */'mdFeedbackService', TaxonomyFormController]);

    /** @ngInject */
    function TaxonomyFormController($mdDialog, $rootScope, $state, $scope, taxonomy, /*contents, */$mdFeedbackService) {
        var vm = this;
        var taxonomyController = new mdBusinessLogic.dataAccess.controllers.taxonomyController();
        var contentController = new mdBusinessLogic.dataAccess.controllers.contentController();
        var taxonomyContentController = new mdBusinessLogic.dataAccess.controllers.taxonomyContentController();
        vm.currentTaxonomyPath = $state.params.path;
        var parentTaxonomy = {};

        // Data
        vm.isNew = $state.params.action != 'edit';
        var addOrEdit = $state.params.action;
        vm.formTitle = addOrEdit === 'edit' ? $rootScope.globals.resources.Titles.EditTaxonomy : $rootScope.globals.resources.Titles.AddTaxonomy;
        vm.currentView = $state.params.currentView;
        vm.taxonomy = taxonomy;
        vm.selectedContentItemToAdd = {};
        vm.selectedContentItemToPreview = {};

        // Methods
        function findContentIndex(content) {
            for (var i = 0; i < vm.taxonomy.Contents.length; i++) {
                if (vm.taxonomy.Contents[i].Id == content.Id) {
                    return i;
                }
            }
            return -1;
        }

        vm.addContent = function ($event) {
            if (findContentIndex(vm.selectedContentItemToAdd) < 0) {
                vm.taxonomy.Contents.push(vm.selectedContentItemToAdd);
            }
            vm.selectedContentItemToAdd = {};
        }

        vm.removeContent = function ($event, content) {
            var indexToRemove = findContentIndex(content);
            if (indexToRemove >= 0) {
                vm.taxonomy.Contents.splice(indexToRemove, 1);
            }
        }

        vm.previewContent = function ($event, content) {
            vm.selectedContentItemToPreview = content;
            $mdDialog.show({
                contentElement: '#taxonomyContent_moreInfoDialog',
                parent: angular.element(document.body),
                targetEvent: $event,
                clickOutsideToClose: true
            });
        }

        vm.save = function ($event) {
            if (mdBusinessLogic.settings.lcid != 0) {
                vm.taxonomy.lcid = mdBusinessLogic.settings.lcid;
            }
            if (vm.taxonomy.ParentId == 0) {
                vm.taxonomy.ParentId = 1;
            }
            if (vm.isNew) {
                vm.taxonomy.IsNew = vm.isNew;
            }

            taxonomyController.save(vm.taxonomy, function (data) {
                $scope.$emit('LoadNav', {
                    action: 'save',
                    type: mdBusinessLogic.dataAccess.entities.entitiesEnum.Taxonomy,
                    value: angular.copy(data)
                });
                $mdFeedbackService.reportInfo('save');
                vm.redirect(data);

            }, function (error) {
                $mdFeedbackService.reportError('save', error);
            })
        }

        vm.redirect = function (data) {
            var taxonomy = vm.taxonomy;
            if (data !== undefined && data != null) {
                taxonomy = data;
            }
            $state.go('app.taxonomy_list', { taxonomyPath: taxonomy.TaxonomyPath, currentView: vm.currentView });
        }

















        /*vm.basicForm = {};
        vm.formWizard = {};
        vm.isNew = $state.params.action != 'edit';
        var addOrEdit = $state.params.action;
        vm.id = 0;
        vm.formTitle = addOrEdit === 'edit' ? $rootScope.globals.resources.Titles.EditTaxonomy : $rootScope.globals.resources.Titles.AddTaxonomy;
        var dialogInfoText = addOrEdit === 'edit' ? $rootScope.globals.resources.Labels.EditedText : $rootScope.globals.resources.Labels.AddedText;
        vm.currentView = $state.params.currentView;
        vm.redirect = redirect;
        //contents
        vm.removedContent = [];
        vm.content = [];
        vm.contentSearchText;
        vm.AddContent = [];
        vm.selectedcontentItem;
        vm.queryAllContent = queryAllContent;
        vm.addContent = addContent;
        vm.RemoveContent = removeContent;
        vm.sendForm = sendForm;

        if (!vm.isNew) {
            for (var i in vm.taxonomy.Contents) {
                vm.content[i] = vm.taxonomy.Contents[i];
            }
        }

        vm.contents = contents.map(function (content) {
            content._lowertitle = content.Title.toLowerCase();
            return content;
        });

        function redirect() {          
            $state.go('app.taxonomy_list', { taxonomyPath: vm.taxonomy.TaxonomyPath, currentView: vm.currentView }, { reload: true });
        }

        function queryAllContent(query) {
            var lowercaseQuery = angular.lowercase(query);
            var results = query ? vm.contents.filter(function (query) {
                return function filterFn(content) {
                    return (content._lowertitle.indexOf(lowercaseQuery) === 0);
                };
            }) : [];
            var i = results.length;
            return results;
        }

        function addContent(content) {
            var index = contentController.doesContentExist(vm.taxonomy.Contents, content);
            if (index == -1) {
                vm.taxonomy.Contents.push(content);
            } else {
                vm.content.splice(vm.content.length-1, 1);
            }
            vm.selectedcontentItem = null;
            vm.contentSearchText = '';
        }


        function removeContent(content) {
            var index = contentController.doesContentExist(vm.taxonomy.Contents, content)
            if (index != -1) {
                vm.taxonomy.Contents.splice(index, 1);
            }
        }

        function sendForm(ev) {
            if (mdBusinessLogic.settings.lcid != 0) {
                vm.taxonomy.lcid = mdBusinessLogic.settings.lcid;
            }
            if (vm.taxonomy.ParentId == 0) {
                vm.taxonomy.ParentId = 1;
            }
            if (vm.isNew) {
                vm.taxonomy.IsNew = vm.isNew;
            }

            taxonomyController.save(vm.taxonomy, function (data) {
                $mdFeedbackService.reportInfo('save');
                redirect();

            }, function (error) {
                $mdFeedbackService.reportError('save', error);
            })
        }*/
    }
})();
