(function ()
{
    'use strict';

    angular
        .module('app.reporting.report_definitions.designer')
        .factory('reportDesignerService', [reportDesignerService]);

    /** @ngInject */
    function reportDesignerService()
    {
        var comparerTypes = [{
            title: "Equals",
            value: 1,
            icon: "icon-code-equal"
        },{
            title: "Does not Equal",
            value: 2,
            icon: "icon-code-not-equal"
        },{
            title: "Like",
            value: 3,
            icon: "icon-percent"
        },{
            title: "Greater Than",
            value: 4,
            icon: "icon-code-greater-than"
        },{
            title: "Greater Than or Equal To",
            value: 5,
            icon: "icon-code-greater-than-or-equal"
        },{
            title: "Less Than",
            value: 6,
            icon: "icon-code-less-than"
        },{
            title: "Less Than or Equal To",
            value: 7,
            icon: "icon-code-less-than-or-equal"
        }];

        var service = {
            getAllComparerTypes: function () {
                return comparerTypes;
            },
            getComparerType: function (value) {
                for (var i = 0; i < comparerTypes.length; i++) {
                    if (comparerTypes[i].value == value) {
                        return comparerTypes[i];
                    }
                }
            }
        };

        return service;
    }
}());