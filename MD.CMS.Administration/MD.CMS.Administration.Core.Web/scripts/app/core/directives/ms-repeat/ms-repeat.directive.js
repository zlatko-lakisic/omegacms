(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('msRepeat', function ($compile) {
            return {
                link: function (scope, element, attributes) {
                    var position = 0;
                    var name = $(element).attr('name');
                    $(element).parent().wrap('<div></div>');
                    for (var i = 1; i < parseInt(attributes.msRepeat) ; i++) {
                        position++;
                        element.parent().parent().append($compile("<md-input-container class='md-block'><input" + " id='" + name + "Repeat" + "' ng-model='vm.fields." + name + ".value[" + i + "]' ng-change='vm.content.setField(" + name + "~ vm.fields." + name + ".value[" + i + "])'></md-input-container>")(scope));
                    }
                    element.parent().css('padding-right', '33px');
                    element.parent().append($('<i class="icon icon-plus-box s26 repeat-icon" style="float:right;position:absolute;top:0;right:0px;"></i>').on('click', function () {
                    position++;
                    var id = "[" + position + "]";
                    element.parent().parent().append($compile("<md-input-container class='md-block'><input" + " id='" + name + "Repeat" + "' ng-model='vm.fields." + name + ".value" + id + "' ng-change='vm.content.setField(" + name + ", vm.fields." + name + ".value" + id + ")'></md-input-container>")(scope));
                    }));  
                }
            };
        });

    /** @ngInject */
    function msRepeat($compile) {
        
    }
})();