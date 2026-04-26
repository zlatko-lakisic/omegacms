(function ()
{
    'use strict';

    angular
        .module('app.core')
        .provider('omegaTheming', ['fuseThemingProvider', '$mdThemingProvider', 'fuseThemes', 'fusePalettes', omegaThemingProvider]);

    /** @ngInject */
    function omegaThemingProvider(fuseThemingProvider, $mdThemingProvider, fuseThemes, fusePalettes)
    {
        this.apply = apply;

        this.$get = function () {
            return {
                apply: apply
            }
        }

        function apply(customThemes) {
            if (customThemes !== undefined) {
                for (var key in customThemes) {
                    fuseThemes[key] = customThemes[key];
                }
            }

            $mdThemingProvider.alwaysWatchTheme(true);

            // Define custom palettes
            angular.forEach(fusePalettes, function (palette) {
                $mdThemingProvider.definePalette(palette.name, palette.options);
            });

            // Register custom themes
            angular.forEach(fuseThemes, function (theme, themeName) {
                $mdThemingProvider.theme(themeName)
                    .primaryPalette(theme.primary.name, theme.primary.hues)
                    .accentPalette(theme.accent.name, theme.accent.hues)
                    .warnPalette(theme.warn.name, theme.warn.hues)
                    .backgroundPalette(theme.background.name, theme.background.hues);
            });

            // Store generated PALETTES and THEMES objects from $mdThemingProvider
            // in our custom provider, so we can inject them into other areas
            fuseThemingProvider.setRegisteredPalettes($mdThemingProvider._PALETTES);
            fuseThemingProvider.setRegisteredThemes($mdThemingProvider._THEMES);
        }
    }
}());