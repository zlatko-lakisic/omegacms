(function ()
{
    'use strict';

    angular
        .module('app.quick-panel', [])
        .config(['$translatePartialLoaderProvider', 'msApiProvider', config]);

    /** @ngInject */
    function config($translatePartialLoaderProvider, msApiProvider)
    {
        // Translation
        $translatePartialLoaderProvider.addPart('scripts/app/quick-panel');

        // Api
        msApiProvider.register('quickPanel.activities', ['scripts/app/data/quick-panel/activities.json']);
        msApiProvider.register('quickPanel.contacts', ['scripts/app/data/quick-panel/contacts.json']);
        msApiProvider.register('quickPanel.events', ['scripts/app/data/quick-panel/events.json']);
        msApiProvider.register('quickPanel.notes', ['scripts/app/data/quick-panel/notes.json']);
    }
})();
