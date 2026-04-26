(function () {
    'use strict';

    angular
        .module('app.core')
        .controller('mdCmsGridController', ['$scope', '$q', '$timeout', '$interval', '$element', mdCmsGridController]);
    /** @ngInject */
    function mdCmsGridController($scope, $q, $timeout, $interval, $element) {

        //Private Attributes
        var vm = this;
        var toolbarId = null;
        var toolbarLoaded = false;
        var editor = null;
        var toolbarBlockTiles = [];
        var canvasBlockTiles = [];
        var loadToolbarBlockTiles = false;
        var toolbarBlockTilesLoaded = false;
        var loadCanvasBlockTiles = false;
        var onTileEvent = function (args) { };
        var gridActions = [];
        var device = '';
        var toolbarTileCount = 0;
        var canvasTileCount = 0;
        var loadedCanvasTilePromises = [];

        //Public Attributes
        vm.editMode = false;
        vm.uniqueId = null;
        vm.title = $scope.mdTitle;
        vm.hideToolbar = $scope.mdHideToolbar;
        vm.displayIcon = 'icon-desktop-mac';
        vm.canvasRendered = false;

        //Public Methods
        vm.toggleEditMode = toggleEditMode;
        vm.cancel = cancel;
        vm.save = save;
        vm.gridAction = gridAction;

        //Private Methods
        function gridAction(command) {
            editor.runCommand(command);
            switch (command) {
                case 'device-desktop':
                    vm.displayIcon = 'icon-desktop-mac';
                    break;
                case 'device-tablet':
                    vm.displayIcon = 'icon-tablet-ipad';
                    break;
                case 'device-phone':
                    vm.displayIcon = 'icon-cellphone-iphone';
                    break;
            }
        }

        function cancel(ev) {
            toggleEditMode();
        }

        function save(ev) {
        }

        function toggleEditMode() {
            vm.editMode = !vm.editMode;
            $scope.$broadcast('md-cms-grid-events-toggle-edit-mode', vm.editMode);
        }

        function initGridEvents() {
            editor.on('component:mount', function (element) {
                element.removeClass('flex');
                if (element.attributes.type !== 'wrapper') {
                    var tbBlockTile = toolbarBlockTiles.filter(function (tile) { return element.getAttributes()['data-type'] == tile.id; })[0];
                    var blockTile = canvasBlockTiles.filter(function (tile) { return element.getAttributes()['data-id'] == tile.id; })[0];
                    blockTile.element = $(tbBlockTile.element).clone()[0];
                    onTileEvent({
                        event: 'render',
                        data: {
                            blockTile: blockTile,
                            obj: element.view
                        }
                    }).then(function (html) {
                        if (html) {
                            element.view.$el.html(html);
                        }
                    }, function () { });
                }
            });

            editor.on('component:selected', function () {
                if (gridActions && gridActions.length) {
                    for (var i = 0; i < gridActions.length; i++) {
                        var commandToAdd = 'gridactions' + gridActions[i].id;
                        var commandIcon = gridActions[i].icon;
                        var selectedComponent = editor.getSelected();
                        var defaultToolbar = selectedComponent.get('toolbar');
                        var commandExists = defaultToolbar.filter(function (item) { return item.command == commandToAdd; }).length > 0;
                        if (!commandExists) {
                            selectedComponent.set({
                                toolbar: defaultToolbar.concat([{ 'attributes': { 'class': commandIcon }, 'command': commandToAdd }])
                            });
                        }
                    }
                }
            });

            editor.on('component:add', function (element) {
                element.removeClass('flex');
                if (element.attributes.type !== 'wrapper') {
                    onTileEvent({
                        event: 'add',
                        data: {
                            index: element.index(),
                            parent: element.parent().attributes.attributes,
                            element: element.getAttributes()
                        }
                    }).then(function (data) {
                        if (data !== undefined && data != null) {
                            element.setAttributes(Object.assign({}, element.getAttributes(), data));
                        }
                        element.view.render();
                    }, function () { });
                }
            });

            editor.on('component:remove', function (element) {
                if (element.attributes.type !== 'wrapper') {
                    onTileEvent({
                        event: 'remove',
                        data: {
                            index: element.index(),
                            parent: element.parent().attributes.attributes,
                            element: element.getAttributes()
                        }
                    }).then(function () { }, function () { });
                }
            });

            editor.on('component:drag:end', function (_data) {
                onTileEvent({
                    event: 'moved',
                    data: {
                        index: _data.index,
                        parent: _data.parent.getAttributes(),
                        element: _data.target.getAttributes()
                    }
                }).then(function (data) {
                    if (data !== undefined && data != null) {
                        _data.target.setAttributes(Object.assign({}, _data.target.getAttributes(), data));
                    }
                }, function () { });
            });

            editor.on('device:select', function (selected) {
                switch (selected.id) {
                    case 'tablet':
                        device = 'medium';
                        break;
                    case 'mobilePortrait':
                        device = 'small';
                        break;
                    default:
                        device = 'large';
                        break;
                }
                $(editor.Canvas.getBody()).attr('data-device', device);
            });
        }

        function initGridCommands() {
            if (gridActions && gridActions.length) {
                for (var i = 0; i < gridActions.length; i++) {
                    var gridAction = gridActions[i];
                    editor.Commands.add('gridactions' + gridAction.id, function (editor) {
                        var element = editor.getSelected();
                        onTileEvent({
                            id: gridAction.id,
                            event: gridAction.name,
                            data: element.getAttributes()
                        }).then(function (data) {
                            if (data !== undefined && data != null) {
                                element.setAttributes(Object.assign({}, element.getAttributes(), data));
                            }
                            element.view.render();
                        }, function () { });
                    });
                }
            }

            editor.Commands.add('undo', function (editor, sender) {
                sender.set("active", false);
                editor.UndoManager.undo(1);
            });

            editor.Commands.add('redo', function (editor, sender) {
                sender.set("active", false);
                editor.UndoManager.redo(1);
            });

            editor.getConfig().showDevices = 0;

            editor.Commands.add('device-desktop', function (editor, sender) {
                editor.setDevice("Desktop");
            });

            editor.Commands.add('device-tablet', function (editor, sender) {
                editor.setDevice("Tablet");
            });

            editor.Commands.add('device-phone', function (editor, sender) {
                editor.setDevice("Mobile portrait");
            });
        }

        function loadBlockAndComponent(omegaItem, blockTile) {
            var droppable = blockTile.element[0].attributes['data-droppable'].value == 'true';

            editor.DomComponents.addType('block-' + blockTile.type, {
                isComponent: function (el) {
                    if (el.tagName == 'MD-CMS-GRID-TILE' && el.attributes['data-type'] == blockTile.type) {
                        return { type: 'block-' + blockTile.type };
                    }
                    return false;
                },
                extend: 'OmegaItem',
                model: {
                    defaults: {
                        tagName: 'md-cms-grid-tile',
                        type: 'block-' + blockTile.type,
                        attributes: {
                            'data': blockTile.data,
                            'data-name': blockTile.element[0].attributes['data-name'].value,
                            'data-id': blockTile.id,
                            'data-type': blockTile.type,
                            'data-layout': blockTile.layout.toLowerCase(),
                            'data-parentid': blockTile.parentid,
                            'data-droppable': droppable,
                            'data-tileid': blockTile.tileid,
                            'data-tiledata': new mdBusinessLogic.dataAccess.entities.grid.gridTileData(Object.assign({}, $scope.defailtGridTileData, blockTile.tileData))
                        },
                        classes: blockTile.element[0].attributes['class'].value.split(' ').concat([(droppable ? 'gjs-droppable' : '')]),
                        droppable: droppable,
                        name: blockTile.element[0].attributes['data-name'].value
                    },
                },
                view: {
                    tagName: 'md-cms-grid-tile',
                    onRender: function () {
                        var obj = this;
                        this.model.attributes.name = this.model.attributes.attributes['data-name'];
                        this.model.removeClass(['layout-row', 'layout-column']);
                        if (this.model.getAttributes()['data-droppable']) {
                            this.model.addClass('layout-' + this.model.getAttributes()['data-layout'].toLowerCase());
                        }
                        this.model.removeClass((function () { var classes = []; for (var i = 5; i <= 100; i += 5) { classes = classes.concat(['flex-sm-' + i, 'flex-md-' + i, 'flex-gt-md-' + i]); } return classes; })());
                        this.model.removeClass((function () { var classes = []; for (var i = 100; i <= 1000; i += 50) { classes = classes.concat(['flexheight-' + i]); } return classes; })());
                        this.model.addClass([
                            'flex-sm-' + this.model.getAttributes()['data-tiledata'].getWidth('small'),
                            'flex-md-' + this.model.getAttributes()['data-tiledata'].getWidth('medium'),
                            'flex-gt-md-' + this.model.getAttributes()['data-tiledata'].getWidth(),
                            'flexheight-' + this.model.getAttributes()['data-tiledata'].getHeight()
                        ]);
                        onTileEvent({
                            event: 'render',
                            data: {
                                blockTile: blockTile,
                                obj: obj
                            }
                        }).then(function (html) {
                            if (html && !droppable) {
                                obj.$el.html(html);
                            }
                        }, function () { });
                    }
                }
            });
            console.log(['Component added', blockTile]);

            editor.BlockManager.add('block-' + blockTile.id, {
                id: blockTile.id,
                label: blockTile.element[0].outerHTML.replace('class="ng-hide"', ''),
                content: {
                    type: 'block-' + blockTile.id
                },
                category: {
                    id: blockTile.group ? blockTile.group.name : 'General',
                    label: blockTile.group ? blockTile.group.name : 'General',
                    open: false,
                    attributes: {
                        'omega-category-icon': blockTile.group ? blockTile.group.icon : 'icon-view-dashboard'
                    }
                }
            });
            console.log(['Block added', blockTile]);
        }

        function constructCanvasComponent(tile) {
            var droppable = tile.element[0].attributes['data-droppable'].value == 'true';
            var array = filterCanvasBlockTiles(tile.tileid);
            return {
                type: 'block-' + tile.type,
                attributes: {
                    'data': tile.data,
                    'data-name': tile.element[0].attributes['data-name'].value,
                    'data-id': tile.id,
                    'data-type': tile.type,
                    'data-parentid': tile.parentid,
                    'data-layout': tile.layout.toLowerCase(),
                    'data-tileid': tile.tileid,
                    'data-droppable': droppable,
                    'data-tiledata': new mdBusinessLogic.dataAccess.entities.grid.gridTileData(Object.assign({}, $scope.defailtGridTileData, tile.tileData)),
                },
                classes: tile.element[0].attributes['class'].value.split(' ').concat([(droppable ? 'gjs-droppable' : '')]),
                components: array.map(constructCanvasComponent)
            };
        }

        function loadCanvasComponents(blockTile) {
            editor.addComponents(constructCanvasComponent(blockTile));
            console.log(['Canvas component added', blockTile]);
        }

        function renderCanvasComponents() {
            var getAllComponents = function (model, result) {
                if (result === undefined) {
                    result = [];
                }
                if (model !== undefined) {
                    result.push(model);
                    var components = model.components();
                    for (var i = 0; i < components.length; i++) {
                        getAllComponents(components[i], result);
                    }
                }
                return result;
            }
            var all = getAllComponents(editor.DomComponents.getWrapper());
        }

        function filterCanvasBlockTiles(parentId) {
            return canvasBlockTiles.filter(function (tile) {
                return tile.parentid == parentId;
            });
        }

        function initGridComponents() {
            editor.DomComponents.getWrapper().set({ badgable: false, selectable: false });

            editor.DomComponents.addType('OmegaItem', {
                isComponent: function (el) {
                    if (el.tagName == 'OMEGAITEM') {
                        return { type: 'OmegaItem' };
                    }
                    return false;
                },
                model: {
                    defaults: {
                        attributes: {
                            'flex-width': '30%',
                            'flex-direction': 'row'
                        },
                        resizable: {
                            tl: 0, // Top left
                            tc: 0, // Top center
                            tr: 0, // Top right
                            cl: 0, // Center left
                            cr: 1, // Center right
                            bl: 0, // Bottom left
                            bc: 1, // Bottom center
                            br: 0, // Bottom right
                            updateTarget: function (el, rect, opt) {
                                function updatedValue(selected, type, interval, threshold, defaultValue, min, max) {

                                    var grow = rect[type] > (type === 'w' ? el.offsetWidth : el.offsetHeight) + threshold;
                                    var shrink = rect[type] < (type === 'w' ? el.offsetWidth : el.offsetHeight) - threshold;

                                    function getClassName() {
                                        var className = 'flex';
                                        if (type === 'h') {
                                            className += 'height-';
                                        } else {
                                            className += '-';

                                            switch (device) {
                                                case 'medium':
                                                    className += 'md-';
                                                    break;
                                                case 'small':
                                                    className += 'sm-';
                                                    break;
                                                default:
                                                    className += 'gt-md-';
                                            }
                                        }
                                        return className;
                                    }

                                    if (grow || shrink) {
                                        var oldValue = defaultValue;
                                        var oldClass = null;

                                        for (var i = 0; i < el.classList.length; i++) {
                                            var cl = el.classList[i];
                                            var regex = new RegExp(getClassName() + '\\d+$');
                                            var found = regex.exec(cl);
                                            if (found) {
                                                var val = cl.replace(getClassName(), '');
                                                if (val) {
                                                    oldValue = val;
                                                    oldClass = cl;
                                                    break;
                                                }
                                            }
                                        }

                                        var newValue = Number(oldValue);

                                        if (grow) {
                                            newValue += interval;
                                        } else {
                                            newValue -= interval;
                                        }
                                        if (newValue > max) { newValue = max; }
                                        if (newValue < min) { newValue = min; }

                                        var newClass = getClassName() + newValue;
                                        selected.addClass(newClass);
                                        if (oldClass && oldClass !== newClass) {
                                            selected.removeClass(oldClass);
                                        }

                                        onTileEvent({
                                            event: 'resize',
                                            data: {
                                                data: selected.getAttributes(),
                                                property: (type === 'h' ? 'height' : 'width'),
                                                value: newValue,
                                                device: device
                                            }
                                        }).then(function (data) {
                                        }, function () { });
                                    }
                                }


                                var selected = editor.getSelected();
                                if (!selected) { return; }

                                var type = ['tr', 'cr', 'br'].indexOf(opt.selectedHandler) >= 0 ? 'w' : 'h';

                                var row = el.parentElement;
                                var threshold = 25;
                                if (type === 'w') {
                                    threshold = (row.offsetWidth / 12) * 0.5;
                                }

                                updatedValue(selected, type, (type === 'w' ? 5 : 50), threshold, (type === 'w' ? 30 : 150), (type === 'w' ? 10 : 150), (type === 'w' ? 100 : 1000));
                            },
                        },
                        style: {
                            display: 'flex',
                            'flex-direction': 'row',
                            'flex-wrap': 'wrap'
                        },
                        tagName: 'OmegaItem',
                        copyable: false
                    },
                },
            });
            
            var omegaItem = editor.DomComponents.getType('OmegaItem').model;

            console.log(['toolbarBlockTiles.length', toolbarBlockTiles.length, loadToolbarBlockTiles]);
            for (var i = 0; i < toolbarBlockTiles.length; i++) {
                var blockTile = toolbarBlockTiles[i];
                if (blockTile.data !== undefined && blockTile.data != null) {
                    loadBlockAndComponent(omegaItem, blockTile);
                }
            }

            var array = filterCanvasBlockTiles();

            console.log(['canvasBlockTiles.length', array.length]);
            for (var i = 0; i < array.length; i++) {
                var blockTile = array[i];
                if (blockTile.data !== undefined && blockTile.data != null) {
                    loadCanvasComponents(blockTile);
                }
            }
            vm.canvasRendered = true;
            editor.render();
            renderCanvasComponents();
            $timeout(function () {
                var categoriesProcessed = [];
                var categories = editor.BlockManager.getCategories();
                categories.each(function (category, i) {
                    if (i === 0) {
                        category.set('open', true);
                    }
                    category.on('change:open', function (opened) {
                        opened.get('open') && categories.each(function (category) {
                            category !== opened && category.set('open', false);
                        });
                    });

                    if (categoriesProcessed.indexOf(category.id) < 0) {

                        var element = angular.element('.gjs-block-category[omega-category-icon="' + category.attributes.attributes['omega-category-icon'] + '"] .gjs-title');

                        element.append('<i class="icon s34 ' + category.attributes.attributes['omega-category-icon'] + '"></i>');

                        categoriesProcessed.push(category.id);
                    }
                });
            }, 1000);
        }

        function initGrid() {
            vm.canvasRendered = false;
            $timeout(function () {
                $scope.styleDependencies.unshift('scripts/app/core/directives/md-cms-grid/md-cms-grid-canvas.min.css');
                editor = grapesjs.init({
                    autorender: false,
                    container: angular.element('#gjs')[0],
                    canvas: {
                        notTextable: ['button', 'a', 'input[type=checkbox]', 'input[type=radio]'],
                        styles: $scope.styleDependencies.map(function (style) { return { href: style }; })
                    },
                    fromElement: false,
                    height: '100%',
                    width: 'auto',
                    listenToEl: [
                        $element.closest('md-content#content')[0]
                    ],
                    layerManager: {
                        appendTo: document.createElement('div')
                    },
                    plugins: [],
                    panels: {
                        defaults: []
                    },
                    blockManager: {
                        appendTo: angular.element('#' + toolbarId + ' #blocks')[0],
                        blocks: []
                    },
                    selectorManager: {
                        appendTo: document.createElement('div')
                    },
                    styleManager: {
                        appendTo: document.createElement('div')
                    },
                    traitManager: {
                        appendTo: document.createElement('div')
                    },
                    clearOnRender: false,
                    storageManager: {
                        autoload: false
                    },
                });

                editor.Panels.addPanel({
                    id: 'panel-top',
                    el: '.panel__top',
                });
                editor.Panels.addPanel({
                    id: 'basic-actions',
                    el: '.panel__basic-actions',
                    buttons: [
                        {
                            id: 'visibility',
                            active: true, // active by default
                            className: 'btn-toggle-borders',
                            label: '<u>B</u>',
                            command: 'sw-visibility', // Built-in command
                        }
                    ],
                });

                $scope.$watchGroup([function () {
                    return loadCanvasBlockTiles;
                }, function () {
                    return loadToolbarBlockTiles;
                }], function (loaded) {
                    if (loaded[0] === true && loaded[1] === true) {
                        initGridCommands();
                        initGridComponents();
                        initGridEvents();
                    }
                });
            });
        }

        function init() {
            if ($scope.registerEditEvent !== undefined) {
                $scope.registerEditEvent()(function (reinit) {
                    if (reinit === undefined) {
                        reinit = false;
                    }
                    toggleEditMode(reinit);
                    return vm.editMode;
                });
            }

            $timeout(function () {
                canvasTileCount = $element.find('.grid-wrapper > div[ng-transclude="list"] md-cms-grid-tile').length;
            });

            $scope.$on('md-cms-grid-toolbar-tilecount', function (event, tilecount) {
                console.log(['md-cms-grid-toolbar-tilecount', tilecount]);
                toolbarTileCount = tilecount;
            });

            $scope.$on('md-cms-grid-toolbar-events-loaded', function (event, data) {
                if (data !== undefined && data != null) {
                    toolbarId = data;
                    toolbarLoaded = true;
                }
            });

            $scope.$on('md-cms-grid-tile-html', function (event, data) {
                if (data.toolbar) {
                    toolbarBlockTiles.push(data);
                } else {
                    canvasBlockTiles.push(data);
                }
            });

            var toolbar_block_tile_data_load_timeout = null;
            var canvas_block_tile_data_load_timeout = null;
            var toolbarIntervalId = undefined;
            var canvasIntervalId = undefined;

            function loadTileDataEvent(event, data) {
                console.log(['Tile data loaded', data.toolbar ? 'Toolbar' : 'Canvas', data]);
                if (data.toolbar) {
                    var blockTile = toolbarBlockTiles.filter(function (block) { return block.id == data.id && block.type == data.type && block.toolbar == data.toolbar; })[0];
                    if (blockTile !== undefined) {
                        blockTile.data = data.data;
                    } else {
                        toolbarBlockTiles.push(data);
                    }
                    loadToolbarBlockTiles = toolbarTileCount == toolbarBlockTiles.length && toolbarBlockTiles.length > 0 && toolbarLoaded;
                    console.log(['loadToolbarBlockTiles', loadToolbarBlockTiles, toolbarTileCount]);
                    /*toolbar_block_tile_data_load_timeout = new Date();
                    toolbarIntervalId = $interval(function () {
                        if (((new Date()).getTime() - toolbar_block_tile_data_load_timeout.getTime()) >= 1000) {
                            loadToolbarBlockTiles = true;
                            $interval.cancel(toolbarIntervalId);
                        }
                    }, 1000);*/
                } else {
                    var blockTile = canvasBlockTiles.filter(function (block) { return block.id == data.id && block.type == data.type && block.toolbar == data.toolbar; })[0];
                    if (blockTile !== undefined) {
                        blockTile.data = data.data;
                    } else {
                        canvasBlockTiles.push(data);
                    }
                    loadCanvasBlockTiles = canvasTileCount == canvasBlockTiles.length && canvasBlockTiles.length > 0;
                    /*canvas_block_tile_data_load_timeout = new Date();
                    canvasIntervalId = $interval(function () {
                        if (((new Date()).getTime() - canvas_block_tile_data_load_timeout.getTime()) >= 1000) {
                            loadCanvasBlockTiles = true;
                            $interval.cancel(canvasIntervalId);
                        }
                    }, 1000);*/
                }
            }

            $scope.$on('md-cms-grid-tile-data', loadTileDataEvent);
            $scope.$on('md-cms-grid-toolbar-tile-data', loadTileDataEvent);

            $scope.$watchGroup([
                function () {
                    return toolbarLoaded;
                },
                function () {
                    return vm.editMode;
                }
            ], function (data) {
                if (data[0] && data[1]) {
                    $timeout(function () {
                        if (!loadCanvasBlockTiles) {
                            loadCanvasBlockTiles = true;
                        }
                    }, 2000);

                    $timeout(function () {
                        if (!loadToolbarBlockTiles) {
                            loadToolbarBlockTiles = true;
                        }
                    }, 2000);
                    initGrid();
                }
            });

            $scope.$watch(function () { return $scope.mdOnTileEvent; }, function (mdOnTileEvent) {
                if (mdOnTileEvent !== undefined) {
                    onTileEvent = mdOnTileEvent;
                }
            });

            $scope.$watch(function () { return $scope.gridActions; }, function (_gridActions) {
                if (_gridActions !== undefined) {
                    gridActions = _gridActions;
                }
            });

            $scope.styleDependencies = angular.isDefined($scope.styleDependencies) ? $scope.styleDependencies : [];
        }

        init();
    }
})();
