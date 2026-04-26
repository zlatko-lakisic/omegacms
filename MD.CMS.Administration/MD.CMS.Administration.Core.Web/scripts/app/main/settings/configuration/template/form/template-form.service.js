(function () {
    'use strict';

    angular
        .module('app.settings.configuration.template-form')
        .service('TemplateService', [TemplateService]);

    function TemplateService() {
        var service = this;
        service.template = new mdBusinessLogic.dataAccess.entities.template();
        service.templateUrl = '';

        service.notifyObservers = notifyObservers;
        service.observerCallbacks = [];

        //register an observer
        service.registerObserverCallback = function (callback) {
            service.observerCallbacks.push(callback);
        };

        //call this when you know 'foo' has been changed
         function notifyObservers() {
             angular.forEach(service.observerCallbacks, function (callback) {
                callback();
            });
        };


        service.setTemplate = function (template) {
            service.template = template;
            notifyObservers();
        }

        service.getTemplate = function () {
            return service.template;
        }

        service.setTemplateUrl = function (templateUrl) {
            service.templateUrl = templateUrl;
        }

        service.getTemplateUrl = function () {
            return service.templateUrl
        }

    }
})();