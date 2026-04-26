(function () {
    'use strict';

    angular
        .module('app.core')
        .factory('mdSortService', [mdSortService]);

    /** @ngInject */
    function mdSortService() {
        var direction, type;

        return {
            setActive: function (newType, element) {
                if (newType != type) {
                    type = newType;
                    if (element) {
                        $(".col-active").removeClass("col-active")
                        $(element).addClass("col-active");
                    }
                    direction = "";
                }
            },
            reset: function () {
                direction = "";
                type = "";
            },
            changeDirection: function () {
                if (direction == "asc") {
                    direction = "desc";
                } else if (direction == "desc") {
                    direction = "asc";
                }else{
                    direction = "asc";
                }
            },
            getDirectionClass: function () {
                return direction == "asc" ? "col-sort-asc" : "col-sort-desc";
            },
            getSortString: function () {
                return type + " " + direction.toUpperCase();
            }
        };
    }
}());