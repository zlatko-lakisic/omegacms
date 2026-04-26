(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdPager', ['$rootScope', mdPagerDirective]);
    /** @ngInject */
    function mdPagerDirective($rootScope) {
        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-pager/md-pager.template.html',
            scope: {
                page: "=",
                total: "=",
                onPagerChange: "&",
                noTabs: "@",
                noSize: "@"
            },
            link: function (scope, element, attrs) {
                scope.uniqueId = mdBusinessLogic.helpers.Guid.create().value;
                scope.pageSizes = [10, 20, 50, 100];
                scope.pageSize = 10;
                scope.disableNext = true;
                scope.disablePrevious = true;
                scope.pagingInfo = "";
                scope.hidePager = true;
                scope.noTabs = scope.noTabs || false;
                scope.noSize = scope.noSize || false;

                var getTotalItems = function() {
                    return ((scope.page + 1) * scope.pageSize) > scope.total ? scope.total : ((scope.page + 1) * scope.pageSize);
                }

                //0 - shown left
                //1 - show both
                //2 - show right
                //3 - show none
                var showArrows = function () {
                    if (scope.total <= scope.pageSize) {
                        return 3;
                    }
                    if(scope.page==0){
                        return 2;
                    }
                    if (scope.page < (Math.ceil(scope.total / scope.pageSize) - 1) && parseInt(scope.total / scope.pageSize) > 1) {
                        return 1;
                    }
                    return 0
                }

                var updateValues = function () {
                    scope.hidePager = scope.total <= 10 || scope.total == 0 || scope.total == undefined;
                    scope.pagingInfo = (scope.page * scope.pageSize + 1) + " - " + getTotalItems() + " of " + (scope.total || 0);
                    scope.disableNext = showArrows() == 0 || showArrows() == 3;
                    scope.disablePrevious = showArrows() == 2 || showArrows() == 3;
                }

                scope.updatePager = function () {
                    updateValues();
                    scope.onPagerChange({ currentPage: scope.page, pageSize: scope.pageSize, pagesBorder: showArrows() });
                }


                scope.nextPage = function () {
                    scope.page++;
                    scope.updatePager();
                }

                scope.previousPage = function () {
                    scope.page--;
                    scope.updatePager();
                }

                scope.$watch('total', function () {
                    updateValues();
                });

                scope.toggleOpen = function () {
                    $('#' + scope.uniqueId).toggleClass('open');
                }
            }
        };
    }
})();
