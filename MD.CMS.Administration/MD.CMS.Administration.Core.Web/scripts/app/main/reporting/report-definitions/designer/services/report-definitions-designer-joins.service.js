(function ()
{
    'use strict';

    angular
        .module('app.reporting.report_definitions.designer')
        .factory('reportDesignerJoinService', [reportDesignerJoinService]);

    /** @ngInject */
    function reportDesignerJoinService()
    {
        //Helper Methods
        function signum(x) {
            return (x < 0) ? -1 : 1;
        }
        function absolute(x) {
            return (x < 0) ? -x : x;
        }
        function drawPath(path, startX, startY, endX, endY) {

            var deltaX = (endX - startX) * 0.15;
            var deltaY = (endY - startY) * 0.15;
            // for further calculations which ever is the shortest distance
            var delta = deltaY < absolute(deltaX) ? deltaY : absolute(deltaX);

            // set sweep-flag (counter/clock-wise)
            // if start element is closer to the left edge,
            // draw the first arc counter-clockwise, and the second one clock-wise
            var arc1 = 0; var arc2 = 1;
            if (startX > endX) {
                arc1 = 1;
                arc2 = 0;
            }
            // draw tha pipe-like path
            // 1. move a bit down, 2. arch,  3. move a bit to the right, 4.arch, 5. move down to the end 
            var newAttr = "M" + startX + " " + startY +
                            " V" + (startY + delta) +
                            " A" + delta + " " + delta + " 0 0 " + arc1 + " " + (startX + delta * signum(deltaX)) + " " + (startY + 2 * delta) +
                            " H" + (endX - delta * signum(deltaX)) +
                            " A" + delta + " " + delta + " 0 0 " + arc2 + " " + endX + " " + (startY + 3 * delta) +
                            " V" + endY;
            path.attr("d", newAttr);
        }
        function connectElements(svgContainer, path, startElem, endElem) {

            var startElemOffset = startElem.offset();
            var endElemOffset = endElem.offset();

            // if first element is lower than the second, swap!
            if (startElemOffset.top > endElemOffset.top) {
                var temp = startElem;
                startElem = endElem;
                endElem = temp;
            }

            // get (top, left) corner coordinates of the svg container   
            var svgTop = svgContainer.offset().top;
            var svgLeft = svgContainer.offset().left;

            // get (top, left) coordinates for the two elements
            var startCoord = startElem.offset();
            var endCoord = endElem.offset();

            // calculate path's start (x,y)  coords
            // we want the x coordinate to visually result in the element's mid point
            var startX = startCoord.left + 0.5 * startElem.outerWidth() - svgLeft;    // x = left offset + 0.5*width - svg's left offset
            var startY = startCoord.top + startElem.outerHeight() - svgTop;        // y = top offset + height - svg's top offset

            // calculate path's end (x,y) coords
            var endX = endCoord.left + 0.5 * endElem.outerWidth() - svgLeft;
            var endY = endCoord.top - svgTop;
            if (startElemOffset.top > endElemOffset.top) {
                endY = endCoord.top - svgTop + 0.5 * startElem.outerHeight();
            }

            // call function for drawing the path
            drawPath(path, startX, startY, endX, endY);

        }

        var service = {
            connectElements: connectElements
        };

        return service;
    }
}());