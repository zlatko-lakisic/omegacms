(function ($, mdBusinessLogic) {
    function cmsGirdstackDdPlugin(grid) {
        this.grid = grid;
    }

    cmsGirdstackDdPlugin.prototype.resizable = function (el, opts) {
        el = $(el);
        if (opts === 'disable' || opts === 'enable' || opts === 'destroy') {
            el.resizable(opts);
        } else if (opts === 'option') {
            var key = arguments[2];
            var value = arguments[3];
            el.resizable(opts, key, value);
        } else {
            var handles = el.data('gs-resize-handles') ? el.data('gs-resize-handles') :
                this.grid.opts.resizable.handles;
            el.resizable($.extend({}, this.grid.opts.resizable, {
                handles: handles
            }, {
                start: this.grid.opts.mdCmsGridOptions.onDragStart(this.grid, opts.start || function () { }), //opts.start || function () { },
                stop: this.grid.opts.mdCmsGridOptions.onDragEnd(this.grid, opts.stop || function () { }), //opts.stop || function () { },
                resize: this.grid.opts.mdCmsGridOptions.onResize(this.grid, opts.resize || function () { }) //opts.resize || function () { }
            }));
        }
        return this;
    };

    cmsGirdstackDdPlugin.prototype.draggable = function (el, opts) {
        el = $(el);
        if (opts === 'disable' || opts === 'enable' || opts === 'destroy') {
            el.draggable(opts);
        } else {
            el.draggable($.extend({}, this.grid.opts.draggable, {
                containment: (this.grid.opts._isNested && !this.grid.opts.dragOut) ?
                    angular.element(this.grid.el).parent() :
                    (this.grid.opts.draggable.containment || null),
                start: this.grid.opts.mdCmsGridOptions.onDragStart(this.grid, opts.start || function () { }), //opts.start || function () { },
                stop: this.grid.opts.mdCmsGridOptions.onDragEnd(this.grid, opts.stop || function () { }), //opts.stop || function () { },
                drag: this.grid.opts.mdCmsGridOptions.onDragOrResize(this.grid, opts.drag || function () { }) //opts.resize || function () { }
            }));
        }
        return this;
    };

    cmsGirdstackDdPlugin.prototype.dragIn = function (el, opts) {
        return this;
    };

    cmsGirdstackDdPlugin.prototype.isDraggable = function (el) {
        return true;
    };

    cmsGirdstackDdPlugin.prototype.droppable = function (el, opts) {
        el = $(el);
        el.droppable($.extend({}, opts, {
            activeClass: "ui-state-default",
            hoverClass: "ui-state-hover"
        }));
        return this;
    };

    cmsGirdstackDdPlugin.prototype.isDroppable = function (el, opts) {
        el = $(el);
        return Boolean(el.data('droppable'));
    };

    cmsGirdstackDdPlugin.prototype.on = function (el, eventName, callback) {
        $(el).on(eventName, callback);
        return this;
    };

    cmsGirdstackDdPlugin.prototype.on = function (el, eventName) {
        return this;
    };

    mdBusinessLogic.cmsGirdstackDdPlugin = cmsGirdstackDdPlugin;
})(jQuery, mdBusinessLogic);