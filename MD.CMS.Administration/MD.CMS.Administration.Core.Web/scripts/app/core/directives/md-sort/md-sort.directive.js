(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdSort', mdSortDirective);

    function mdSortDirective() {
        return {
            restrict: 'A',
            scope: {
                mdSort: '@',
                name: '@',
                onSort: '&'
            },
            templateUrl: 'scripts/app/core/directives/md-sort/md-sort.template.html',
            link: function (scope, element, attrs) {
                var direction = true; // false - desc, true - asc

                var changeDirection = function () {
                    direction = !direction;
                    scope.directionClass = direction ? "col-sort-asc" : "col-sort-desc";
                    if (scope.onSort) {
                        scope.onSort({ sortingString: direction ? scope.mdSort + " ASC" : scope.mdSort + " DESC" });
                    }
                }

                var setActive = function (init) {
                    direction = true;
                    var prevActive = element.parent().find(".col-active");
                    prevActive.find(".col-sort-desc").removeClass("col-sort-desc").addClass("col-sort-asc");
                    prevActive.removeClass("col-active");
                    element.addClass("col-active");
                    scope.directionClass = direction ? "col-sort-asc" : "col-sort-desc";
                    if (scope.onSort && !init) {
                        scope.onSort({ sortingString: direction ? scope.mdSort + " ASC" : scope.mdSort + " DESC" });
                    }
                }

                if (!scope.name) {
                    scope.name = scope.mdSort;
                }

                if (element.hasClass("col-active")) {
                    setActive(true);
                }

                element.addClass("col-sort");

                element.on('click', function ($event) {
                    if (!element.hasClass("col-active")) {
                        setActive();
                    } else {
                        changeDirection();
                    }
                });

            }
        };
    }
})();