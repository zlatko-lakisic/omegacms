(function () {
    'use strict';

    angular
        .module('app.reporting.report_definitions.designer')
        .directive('reportDesignerEntity', ['$compile', '$http', reportDesignerEntityDirective]);

    /** @ngInject */
    function reportDesignerEntityDirective($compile, $http) {
        return {
            restrict: 'A',
            scope: {
                drag: '&',
                dragend: '&'
            },
            //controller: 'DragAndDropController',
            link: function (scope, element, attr) {
                var parentElement = angular.element(element[0].parentElement);
                element[0].draggable = true;
                var options = scope.$eval(attr.reportDesignerEntity);
                options.IsPreview = parentElement.hasClass('toolbox');

                if (!options.IsPreview) {
                    element.css({ position: 'absolute' });
                    element.css({
                        top: options.Coordinates.y - (element[0].clientHeight / 2),
                        left: options.Coordinates.x - (element[0].clientWidth / 2)
                    });
                }

                attr.reportDesignerEntity = options;

                element.on('dragstart', function (e) {
                    if (options.IsPreview) {
                        localStorage.setItem("dragItem", JSON.stringify(options));
                    }
                });

                element.on('dragend', function (e) {
                    var el = element.position();
                    var parentOffset = parentElement.offset();
                    var top = e.clientY - parentOffset.top - (element[0].clientHeight / 2);
                    var left = e.clientX - parentOffset.left - (element[0].clientWidth / 2);
                    if (!options.IsPreview &&
                        top >= 0 &&
                        left >= 0 &&
                        e.clientX <= (parentOffset.left + parentElement[0].clientWidth) &&
                        e.clientY <= (parentOffset.top + parentElement[0].clientHeight)) {
                        options.Coordinates.y = top;
                        options.Coordinates.x = left;
                        element.css({ top: options.Coordinates.y, left: options.Coordinates.x });
                        attr.$set("report-designer-entity", JSON.stringify(options));
                        var dragEndFunction = scope.dragend();
                        if ('undefined' !== typeof dragEndFunction) {
                            dragEndFunction();
                        }
                    }
                });

                element.on('drag', function (e) {
                    scope.drag({ event: e });
                });


            }
        };
    }
})();