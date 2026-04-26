(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdCmsAliases', ['$q', '$mdDialog', 'mdFeedbackService', mdCmsAliases]);
    /** @ngInject */
    function mdCmsAliases($q, $mdDialog, $mdFeedbackService) {
        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-cms-aliases/md-cms-aliases.template.html',
            transclude: true,
            scope: {
                mdContent: "=",
                mdInputName: "@",
                mdFloatingLabel: "@",
                placeholder: "@",
                ngDisabled: "=",
                ngPattern: "=?",
                onSave: "&",
                mdForm: "="
            },
            link: function (scope, element, attrs) {

                //Directive variables
                var contentAliasController = new mdBusinessLogic.dataAccess.controllers.contentAliasController();

                scope.uniqueId = mdBusinessLogic.helpers.Guid.create().value;
                scope.defaultAliases = [];
                scope.customAliases = scope.mdContent.ContentAliases;
                scope.alias = '';

                //Directive methods
                scope.getInnerHtml = getInnerHtml;
                scope.addAlias = addAlias;
                scope.deleteAlias = deleteAlias;

                //Autocomplete query search
                function getInnerHtml() {
                    return element.find('md-inner-html').html() || '';
                }

                function addAlias() {
                    if (scope.alias !== undefined && scope.alias.length > 0) {
                        checkAlias(scope.alias).then(function (aliasExists) {
                            if (!aliasExists) {
                                var alias = new mdBusinessLogic.dataAccess.entities.contentAlias();
                                alias.Alias = scope.alias;
                                alias.ContentId = scope.mdContent.Id;
                                alias.DateCreated = scope.mdContent.DateCreated;
                                alias.LCID = scope.mdContent.LCID;
                                scope.customAliases.push(alias);
                                scope.alias = '';
                            } else {
                                $mdFeedbackService.reportCustomInfo('Alias already exists!');
                            }
                        }, function (error) {
                            $mdFeedbackService.reportError('save', error);
                        });
                    } else {
                        $mdFeedbackService.reportCustomInfo('You cannot add an empty alias!');
                    }
                }

                function checkAlias(aliasToAdd) {
                    var defer = $q.defer();
                    if (scope.customAliases.filter(function (alias) { return alias.Alias == aliasToAdd; }).length > 0) {
                        defer.resolve(true);
                    } else {
                        contentAliasController.getAllByContent(scope.mdContent, function (data) {
                            defer.resolve(data.filter(function (alias) { return alias.Value == aliasToAdd; }).length > 0);
                        }, function (error) {
                            dever.reject(error);
                        });
                    }

                    return defer.promise;
                }

                function deleteAlias($index) {
                    scope.customAliases.splice($index, 1);
                }

                function save() {
                    scope.mdContent.ContentAliases = scope.customAliases;
                }

                function init() {
                    scope.onSave()(save);
                }

                init();
            }
        };
    }
})();
