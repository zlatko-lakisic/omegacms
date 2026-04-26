(function () {
    'use strict';

    angular
        .module('app.reporting.report_definitions.designer')
        .directive('reportDesignerCanvas', ['$compile', '$document', reportDesignerCanvasDirective]);

    /** @ngInject */
    function reportDesignerCanvasDirective($compile, $document) {
        return {
            restrict: 'A',
            scope: {
                drop: '&',
                dragover: '&'
            },
            link: function (scope, element, attr) {

                element.on('dragover', function (e) {
                    if (e.stopPropagation) e.stopPropagation();
                    if (e.preventDefault) e.preventDefault();
                    scope.dragover({ event: e });
                });

                element.on('drop', function (e) {
                    var element = document.querySelectorAll('#reportCanvas');

                    var height = element[0].clientHeight;
                    var width = element[0].clientWidth;

                    if (e.stopPropagation) e.stopPropagation();
                    if (localStorage.getItem("dragItem") != undefined) {
                        var options = JSON.parse(localStorage.getItem("dragItem"));
                        options.Coordinates.x = e.offsetX;
                        options.Coordinates.y = e.offsetY;
                        if (options.Coordinates.x < 90) {
                            options.Coordinates.x = -1;
                        }
                        if (options.Coordinates.y < 30) {
                            options.Coordinates.y = -1;
                        }
                        if (options.Coordinates.x >= 0 && options.Coordinates.y >= 0) {
                            var dropFunction = scope.drop();
                            if ('undefined' !== typeof dropFunction) {
                                dropFunction(options);
                            }
                            localStorage.removeItem("dragItem");
                        }
                    }
                });

            }
        };
    }

    function offset(elm) {
        try { return elm.offset(); } catch (e) { }
        var rawDom = elm[0];
        var _x = 0;
        var _y = 0;
        var body = document.documentElement || document.body;
        var scrollX = window.pageXOffset || body.scrollLeft;
        var scrollY = window.pageYOffset || body.scrollTop;
        _x = rawDom.getBoundingClientRect().left + scrollX;
        _y = rawDom.getBoundingClientRect().top + scrollY;
        return { left: _x, top: _y };
    }
})();