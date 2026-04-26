(function () {
    'use strict';

    angular
        .module('app.core')
		.controller('mdCmsDiagramController', ['$scope', '$q', '$timeout', '$element', mdCmsDiagramController]);
    /** @ngInject */
	function mdCmsDiagramController($scope, $q, $timeout, $element) {

        //Private Attributes
		var vm = this;
		var toolbarId = null;
		var toolbarLoaded = false;
		var toolbarBlockTiles = [];
		var canvasBlockTiles = [];
		var loadToolbarBlockTiles = false;
		var loadCanvasBlockTiles = false;
		var toolbarTileCount = 0;
		var canvasTileCount = 0;
		var editor = null;
		var graph = null;
		var model = null;
		var onTileEvent = function (args) { return $q(function (resolve, reject) { resolve(); }); };
		var mxGraphGetLabel = mxGraph.prototype.getLabel;
		var mxCellRendererRedrawLabel = mxCellRenderer.prototype.redrawLabel;
		var diagramActions = [];

        //Public Attributes
		vm.diagramLoaded = false;

		//Public Methods
		vm.editMode = false;
		vm.toggleEditMode = toggleEditMode;

		//MxGraphSubClasses

		// Defines a subclass for mxVertexHandler that adds a set of clickable
		// icons to every selected vertex.
		function mxVertexToolHandler(state) {
			mxVertexHandler.apply(this, arguments);
		};

		mxVertexToolHandler.prototype = new mxVertexHandler();
		mxVertexToolHandler.prototype.constructor = mxVertexToolHandler;

		mxVertexToolHandler.prototype.domNode = null;

		mxVertexToolHandler.prototype.init = function () {
			mxVertexHandler.prototype.init.apply(this, arguments);

			// In this example we force the use of DIVs for images in IE. This
			// handles transparency in PNG images properly in IE and fixes the
			// problem that IE routes all mouse events for a gesture via the
			// initial IMG node, which means the target vertices 
			var obj = this;
			var domNode = $('<div></div>')
			domNode.addClass('diagram-element-toolbar');

			function addButton(domNode, iconClass, action, callback) {
				if (callback === undefined) {
					callback = function () { };
                }

				var icon = $('<i>');
				icon.addClass('icon s10');
				icon.addClass(iconClass);

				mxEvent.addGestureListeners(icon[0],
					mxUtils.bind(this, function (evt) {
						// Disables dragging the image
						mxEvent.consume(evt);
					})
				);

				mxEvent.addListener(icon[0], 'click',
					mxUtils.bind(this, function (evt) {
						var data = {};
						if (obj.state.cell.isConnectable()) {
							data = {
								source: obj.state.cell.source.parent.value.data,
								target: obj.state.cell.target.parent.value.data,
								data: obj.state.cell.data,
								type: 'connection'
							};
						} else {
							data = obj.state.cell.value.data;
						}
						onTileEvent({
							event: action,
							data: data
						}).then(function (data) {
							callback(obj, data);
						}, function () { });
						mxEvent.consume(evt);
					})
				);

				domNode.append(icon);
			}

			if (diagramActions && diagramActions.length) {
				for (var i = 0; i < diagramActions.length; i++) {
					addButton(domNode, diagramActions[i].icon, diagramActions[i].action, diagramActions[i].callback);
				}
			}


			this.domNode = domNode[0];

			this.graph.container.appendChild(this.domNode);
			this.redrawTools();
		};

		mxVertexToolHandler.prototype.redraw = function () {
			mxVertexHandler.prototype.redraw.apply(this);
			this.redrawTools();
		};

		mxVertexToolHandler.prototype.redrawTools = function () {
			if (this.state != null && this.domNode != null) {
				var dy = (mxClient.IS_VML && document.compatMode == 'CSS1Compat') ? 20 : 4;
				var left = (this.state.x + this.state.width - ((diagramActions.length * 16) + 11));
				var top = (this.state.y + this.state.height + dy);

				if (this.state.cell.isConnectable()) {
					left = (this.state.x + (this.state.width / 2) - (diagramActions.length * 8));
					top = (this.state.y + (this.state.height / 2) - 12);
                }

				this.domNode.style.left = left + 'px';
				this.domNode.style.top = top + 'px';
			}
		};

		mxVertexToolHandler.prototype.destroy = function (sender, me) {
			mxVertexHandler.prototype.destroy.apply(this, arguments);

			if (this.domNode != null) {
				this.domNode.parentNode.removeChild(this.domNode);
				this.domNode = null;
			}
		};

		//Private Methods
		function toggleEditMode() {
			vm.editMode = !vm.editMode;
			$scope.$broadcast('md-cms-diagram-events-toggle-edit-mode', vm.editMode);
		}

		function getByType(id) {
			return toolbarBlockTiles.filter(function (tile) { return tile.id == id; })[0];
		}

		function redrawLabel(state, forced) {
			function render(labelValue, obj) {
				var graph = state.view.graph;
				var value = (labelValue != null) ? labelValue : obj.getLabelValue(state);
				var wrapping = graph.isWrapping(state.cell);
				var clipping = graph.isLabelClipped(state.cell);
				var isForceHtml = (state.view.graph.isHtmlLabel(state.cell) || (value != null && mxUtils.isNode(value)));
				var dialect = (isForceHtml) ? mxConstants.DIALECT_STRICTHTML : state.view.graph.dialect;
				var overflow = state.style[mxConstants.STYLE_OVERFLOW] || 'visible';

				if (state.text != null && (state.text.wrap != wrapping || state.text.clipped != clipping ||
					state.text.overflow != overflow || state.text.dialect != dialect)) {
					state.text.destroy();
					state.text = null;
				}

				if (state.text == null && value != null && (mxUtils.isNode(value) || value.length > 0)) {
					obj.createLabel(state, value);
				}
				else if (state.text != null && (value == null || value.length == 0)) {
					state.text.destroy();
					state.text = null;
				}

				if (state.text != null) {
					// Forced is true if the style has changed, so to get the updated
					// result in getLabelBounds we apply the new style to the shape
					if (forced) {
						// Checks if a full repaint is needed
						if (state.text.lastValue != null && obj.isTextShapeInvalid(state, state.text)) {
							// Forces a full repaint
							state.text.lastValue = null;
						}

						state.text.resetStyles();
						state.text.apply(state);

						// Special case where value is obtained via hook in graph
						state.text.valign = graph.getVerticalAlign(state);
					}

					var bounds = obj.getLabelBounds(state);
					var nextScale = obj.getTextScale(state);
					obj.resolveColor(state, 'color', mxConstants.STYLE_FONTCOLOR);

					if (forced || state.text.value != value || state.text.isWrapping != wrapping ||
						state.text.overflow != overflow || state.text.isClipping != clipping ||
						state.text.scale != nextScale || state.text.dialect != dialect ||
						state.text.bounds == null || !state.text.bounds.equals(bounds)) {
						state.text.dialect = dialect;
						state.text.value = value;
						state.text.bounds = bounds;
						state.text.scale = nextScale;
						state.text.wrap = wrapping;
						state.text.clipped = clipping;
						state.text.overflow = overflow;

						// Preserves visible state
						var vis = state.text.node.style.visibility;
						obj.redrawLabelShape(state.text);
						state.text.node.style.visibility = vis;
					}
				}
			}

			var obj = this;
			if (mxUtils.isNode(state.cell.value) && state.cell.value.nodeName.toLowerCase() == 'cmselement') {
				var blockTile = state.cell.value.data;
				var element = blockTile.element.clone();
				element.height(state.cell.geometry.height);
				element.width(state.cell.geometry.width);

				blockTile.tileData.width = state.cell.geometry.width;
				blockTile.tileData.height = state.cell.geometry.height;
				blockTile.tileData.y = state.cell.geometry.y;
				blockTile.tileData.x = state.cell.geometry.x;

				onTileEvent({
					event: 'move',
					data: blockTile
				}).then(function (data) {
					if (data !== undefined && data != null) {
						blockTile = Object.assign({}, blockTile, data);
					}
					onTileEvent({
						event: 'render',
						data: {
							blockTile: blockTile,
							element: element
						}
					}).then(function (element) {
						if (element !== undefined && element != null) {
							element.removeClass('toolbar');
							element.removeClass('ng-hide');
							var html = element[0].outerHTML;
							render(html, obj);
						} else {
							render(null, obj);
						}
					}, function () {
						render(null, obj);
					});
				}, function () { });
			} else {
				render(null, obj);
            }
		}

		function addCmsElement(blockTile, graph, x, y, width, height) {
			var parent = graph.getDefaultParent();
			var model = graph.getModel();

			var v1 = null;

			model.beginUpdate();
			try {
				// NOTE: For non-HTML labels the image must be displayed via the style
				// rather than the label markup, so use 'image=' + image for the style.
				// as follows: v1 = graph.insertVertex(parent, null, label,
				// pt.x, pt.y, 120, 120, 'image=' + image);

				var doc = mxUtils.createXmlDocument();
				var obj = doc.createElement('CmsElement');
				obj.setAttribute('label', blockTile.label);
				obj.setAttribute('type', blockTile.id);
				obj.data = blockTile;

				v1 = graph.insertVertex(parent, blockTile.id, obj, x, y, width, height);
				v1.setConnectable(false);

				// Presets the collapsed size
				v1.geometry.alternateBounds = new mxRectangle(0, 0, 120, 40);

				// Adds the ports at various relative locations
				/*var port = graph.insertVertex(v1, null, 'Top Link', 0.5, 0, 16, 16,
					'port', true);
				port.geometry.offset = new mxPoint(-4, -4);*/

				var port = graph.insertVertex(v1, null, 'Right Link', 1, 0.5, 16, 16,
					'port;image=' + mdBusinessLogic.settings.appBase + 'scripts/app/core/directives/md-cms-diagram/img/link.png;spacingLeft=18', true);
				port.geometry.offset = new mxPoint(-4, -4);

				/*var port = graph.insertVertex(v1, null, 'Bottom Link', 0.5, 1, 16, 16,
					'port', true);
				port.geometry.offset = new mxPoint(-4, -4);*/

				var port = graph.insertVertex(v1, null, 'Left Link', 0, 0.5, 16, 16,
					'port;image=' + mdBusinessLogic.settings.appBase + 'scripts/app/core/directives/md-cms-diagram/img/link.png;align=right;imageAlign=right;spacingRight=18', true);
				port.geometry.offset = new mxPoint(-6, -4);
			}
			finally {
				model.endUpdate();
			}

			graph.setSelectionCell(v1);
        }

		function dropEvent(blockTile, graph, evt, cell, x, y) {
			addCmsElement(blockTile, graph, x, y, blockTile.element.width(), blockTile.element.height());
		}

		function registerToolbarElement(bt) {
			var blockTile = bt;
			// Function that is executed when the image is dropped on
			// the graph. The cell argument points to the cell under
			// the mousepointer if there is one.
			

			var dragElt = blockTile.element.clone()[0];
			dragElt.style.border = 'dashed black 1px';

			// Creates the image which is used as the drag icon (preview)
			var ds = mxUtils.makeDraggable(blockTile.element[0], graph, function (graph, evt, cell, x, y) {
				onTileEvent({
					event: 'add',
					data: blockTile
				}).then(function (data) {
					if (data !== undefined && data != null) {
						blockTile.data = Object.assign({}, blockTile.data, data);
					}
					dropEvent(blockTile, graph, evt, cell, x, y);
				}, function () { });
			}, dragElt, 0, 0, true, true);
			ds.setGuidesEnabled(true);
        }

		function configureStylesheet(graph) {
			var style = new Object();
			style[mxConstants.STYLE_SHAPE] = mxConstants.SHAPE_RECTANGLE;
			style[mxConstants.STYLE_PERIMETER] = 0;
			style[mxConstants.STYLE_ALIGN] = mxConstants.ALIGN_LEFT;
			style[mxConstants.STYLE_VERTICAL_ALIGN] = mxConstants.ALIGN_TOP;
			style[mxConstants.STYLE_GRADIENTCOLOR] = 'none';
			style[mxConstants.STYLE_FILLCOLOR] = 'none';
			style[mxConstants.STYLE_STROKECOLOR] = 'none';
			style[mxConstants.STYLE_FONTCOLOR] = '#000000';
			style[mxConstants.STYLE_ROUNDED] = false;
			//style[mxConstants.STYLE_OPACITY] = '80';
			//style[mxConstants.STYLE_FONTSIZE] = '12';
			style[mxConstants.STYLE_FONTSTYLE] = 0;
			//style[mxConstants.STYLE_IMAGE_WIDTH] = '48';
			//style[mxConstants.STYLE_IMAGE_HEIGHT] = '48';
			graph.getStylesheet().putDefaultVertexStyle(style);

			// NOTE: Alternative vertex style for non-HTML labels should be as
			// follows. This repaces the above style for HTML labels.
			/*var style = new Object();
			style[mxConstants.STYLE_SHAPE] = mxConstants.SHAPE_LABEL;
			style[mxConstants.STYLE_PERIMETER] = mxPerimeter.RectanglePerimeter;
			style[mxConstants.STYLE_VERTICAL_ALIGN] = mxConstants.ALIGN_TOP;
			style[mxConstants.STYLE_ALIGN] = mxConstants.ALIGN_CENTER;
			style[mxConstants.STYLE_IMAGE_ALIGN] = mxConstants.ALIGN_CENTER;
			style[mxConstants.STYLE_IMAGE_VERTICAL_ALIGN] = mxConstants.ALIGN_TOP;
			style[mxConstants.STYLE_SPACING_TOP] = '56';
			style[mxConstants.STYLE_GRADIENTCOLOR] = '#7d85df';
			style[mxConstants.STYLE_STROKECOLOR] = '#5d65df';
			style[mxConstants.STYLE_FILLCOLOR] = '#adc5ff';
			style[mxConstants.STYLE_FONTCOLOR] = '#1d258f';
			style[mxConstants.STYLE_FONTFAMILY] = 'Verdana';
			style[mxConstants.STYLE_FONTSIZE] = '12';
			style[mxConstants.STYLE_FONTSTYLE] = '1';
			style[mxConstants.STYLE_ROUNDED] = '1';
			style[mxConstants.STYLE_IMAGE_WIDTH] = '48';
			style[mxConstants.STYLE_IMAGE_HEIGHT] = '48';
			style[mxConstants.STYLE_OPACITY] = '80';
			graph.getStylesheet().putDefaultVertexStyle(style);*/

			/*style = new Object();
			style[mxConstants.STYLE_SHAPE] = mxConstants.SHAPE_SWIMLANE;
			style[mxConstants.STYLE_PERIMETER] = mxPerimeter.RectanglePerimeter;
			style[mxConstants.STYLE_ALIGN] = mxConstants.ALIGN_CENTER;
			style[mxConstants.STYLE_VERTICAL_ALIGN] = mxConstants.ALIGN_TOP;
			style[mxConstants.STYLE_FILLCOLOR] = '#3b97e3';
			style[mxConstants.STYLE_GRADIENTCOLOR] = '#3b97e3';
			style[mxConstants.STYLE_STROKECOLOR] = '#3b97e3';
			style[mxConstants.STYLE_FONTCOLOR] = '#000000';
			style[mxConstants.STYLE_ROUNDED] = true;
			style[mxConstants.STYLE_OPACITY] = '80';
			style[mxConstants.STYLE_STARTSIZE] = '30';
			style[mxConstants.STYLE_FONTSIZE] = '16';
			style[mxConstants.STYLE_FONTSTYLE] = 1;
			graph.getStylesheet().putCellStyle('group', style);*/

			style = new Object();
			style[mxConstants.STYLE_SHAPE] = mxConstants.SHAPE_IMAGE;
			style[mxConstants.STYLE_FONTCOLOR] = '#774400';
			style[mxConstants.STYLE_PERIMETER] = mxPerimeter.RectanglePerimeter;
			style[mxConstants.STYLE_PERIMETER_SPACING] = '6';
			style[mxConstants.STYLE_ALIGN] = mxConstants.ALIGN_LEFT;
			style[mxConstants.STYLE_VERTICAL_ALIGN] = mxConstants.ALIGN_MIDDLE;
			style[mxConstants.STYLE_FONTSIZE] = '10';
			style[mxConstants.STYLE_FONTSTYLE] = 2;
			style[mxConstants.STYLE_IMAGE_WIDTH] = '16';
			style[mxConstants.STYLE_IMAGE_HEIGHT] = '16';
			graph.getStylesheet().putCellStyle('port', style);

			style = graph.getStylesheet().getDefaultEdgeStyle();
			style[mxConstants.STYLE_LABEL_BACKGROUNDCOLOR] = '#FFFFFF';
			style[mxConstants.STYLE_STROKECOLOR] = '#3b97e3';
			style[mxConstants.STYLE_STROKEWIDTH] = '2';
			style[mxConstants.STYLE_ROUNDED] = true;
			style[mxConstants.STYLE_EDGE] = mxEdgeStyle.EntityRelation;
		}

		function diagramInit() {
			if (!mxClient.isBrowserSupported()) {
				mxUtils.error('Browser is not supported!', 200, false);
			}
			else {
				diagramActions.push({
					icon: 'icon-pencil',
					action: 'edit'
				});
				diagramActions.push({
					icon: 'icon-trash',
					action: 'remove',
					callback: function (obj) {
						obj.graph.removeCells([obj.state.cell]);
					}
				});


				// Assigns some global constants for general behaviour, eg. minimum
				// size (in pixels) of the active region for triggering creation of
				// new connections, the portion (100%) of the cell area to be used
				// for triggering new connections, as well as some fading options for
				// windows and the rubberband selection.
				mxConstants.MIN_HOTSPOT_SIZE = 16;
				mxConstants.DEFAULT_HOTSPOT = 1;

				mxCellRenderer.prototype.redrawLabel = redrawLabel;

				// Enables guides
				mxGraphHandler.prototype.guidesEnabled = true;

				// Alt disables guides
				mxGraphHandler.prototype.useGuidesForEvent = function (me) {
					return !mxEvent.isAltDown(me.getEvent());
				};

				// Defines the guides to be red (default)
				mxConstants.GUIDE_COLOR = '#FF0000';

				// Defines the guides to be 1 pixel (default)
				mxConstants.GUIDE_STROKEWIDTH = 1;

				// Enables snapping waypoints to terminals
				mxEdgeHandler.prototype.snapToTerminals = true;

				// Workaround for Internet Explorer ignoring certain CSS directives
				if (mxClient.IS_QUIRKS) {
					document.body.style.overflow = 'hidden';
					new mxDivResizer(container);
					new mxDivResizer(outline);
					new mxDivResizer(toolbar);
					new mxDivResizer(sidebar);
					new mxDivResizer(status);
				}

				// Creates a wrapper editor with a graph inside the given container.
				// The editor is used to create certain functionality for the
				// graph, such as the rubberband selection, but most parts
				// of the UI are custom in this example.
				editor = new mxEditor();
				graph = editor.graph;
				model = graph.getModel();

				// Disable highlight of cells when dragging from toolbar
				graph.setDropEnabled(false);

				// Uses the port icon while connections are previewed
				graph.connectionHandler.getConnectImage = function (state) {
					return new mxImage(state.style[mxConstants.STYLE_IMAGE], 16, 16);
				};


				graph.connectionHandler.addListener(mxEvent.CONNECT, function (sender, evt) {
					var edge = evt.getProperty('cell');
					var source = graph.getModel().getTerminal(edge, true);
					var target = graph.getModel().getTerminal(edge, false);

					onTileEvent({
						event: 'connect',
						data: {
							source: source.parent.value.data,
							target: target.parent.value.data,
							data: edge.data,
							type: 'connection',
							id: evt.properties.event.id
						}
					}).then(function (data) {
						edge.data = data.data;
						edge.id = data.id;
					}, function () { });
				});

				// Centers the port icon on the target port
				graph.connectionHandler.targetConnectImage = true;

				// Does not allow dangling edges
				graph.setAllowDanglingEdges(false);

				// Sets the graph container and configures the editor
				editor.setGraphContainer($element.find('#diagram-canvas')[0]);

				// Defines the default group to be used for grouping. The
				// default group is a field in the mxEditor instance that
				// is supposed to be a cell which is cloned for new cells.
				// The groupBorderSize is used to define the spacing between
				// the children of a group and the group bounds.
				var group = new mxCell('Group', new mxGeometry(), 'group');
				group.setVertex(true);
				group.setConnectable(false);
				editor.defaultGroup = group;
				editor.groupBorderSize = 20;

				// Disables drag-and-drop into non-swimlanes.
				graph.isValidDropTarget = function (cell, cells, evt) {
					return this.isSwimlane(cell);
				};

				// Disables drilling into non-swimlanes.
				graph.isValidRoot = function (cell) {
					return this.isValidDropTarget(cell);
				}

				// Does not allow selection of locked cells
				graph.isCellSelectable = function (cell) {
					return !this.isCellLocked(cell);
				};

				// Returns a shorter label if the cell is collapsed and no
				// label for expanded groups
				graph.getLabel = function (cell) {
					var tmp = mxGraph.prototype.getLabel.apply(this, arguments); // "supercall"

					if (this.isCellLocked(cell)) {
						// Returns an empty label but makes sure an HTML
						// element is created for the label (for event
						// processing wrt the parent label)
						return '';
					}
					else if (this.isCellCollapsed(cell)) {
						var index = tmp.indexOf('</h1>');

						if (index > 0) {
							tmp = tmp.substring(0, index + 5);
						}
					}

					return tmp;
				}

				// Disables HTML labels for swimlanes to avoid conflict
				// for the event processing on the child cells. HTML
				// labels consume events before underlying cells get the
				// chance to process those events.
				//
				// NOTE: Use of HTML labels is only recommended if the specific
				// features of such labels are required, such as special label
				// styles or interactive form fields. Otherwise non-HTML labels
				// should be used by not overidding the following function.
				// See also: configureStylesheet.
				graph.isHtmlLabel = function (cell) {
					return !this.isSwimlane(cell);
				}

				// To disable the folding icon, use the following code:
				/*graph.isCellFoldable = function(cell)
				{
					return false;
				}*/

				// Shows a "modal" window when double clicking a vertex.
				graph.dblClick = function (evt, cell) {

					// Do not fire a DOUBLE_CLICK event here as mxEditor will
					// consume the event and start the in-place editor.
					if (this.isEnabled() &&
						!mxEvent.isConsumed(evt) &&
						cell != null &&
						this.isCellEditable(cell)) {
						if (this.model.isEdge(cell) ||
							!this.isHtmlLabel(cell)) {
							this.startEditingAtCell(cell);
						}
						else {
							graph.setEnabled(false);
							var blockTile = cell.value.data;
							onTileEvent({
								event: 'edit',
								data: blockTile
							}).then(function (data) {
								if (data !== undefined && data != null) {
									blockTile = Object.assign({}, blockTile, data);
								}
								graph.setEnabled(true);
							}, function () { graph.setEnabled(true); });
						}
					}

					// Disables any default behaviour for the double click
					mxEvent.consume(evt);
				};

				graph.addListener(mxEvent.MOVE_END, function (sender, evt) {

					var cell = evt.getProperty("cell"); // cell may be null
					if (cell != null) {
						var blockTile = cell.value.data;
						onTileEvent({
							event: 'move',
							data: blockTile
						}).then(function (data) {
							if (data !== undefined && data != null) {
								blockTile = Object.assign({}, blockTile, data);
							}
							graph.setEnabled(true);
						}, function () { graph.setEnabled(true); });
					}
					evt.consume();
				});

				// Enables new connections
				graph.setConnectable(true);

				graph.createHandler = function (state) {
					if (state != null &&
						state.cell != null &&
						(state.cell.isVertex() || state.cell.isConnectable())) {
						$element.find('#diagram-canvas').toggleClass('connectable-selected', state.cell.isConnectable());
						return new mxVertexToolHandler(state);
					}

					return mxGraph.prototype.createHandler.apply(this, arguments);
				};

				// Adds all required styles to the graph (see below)
				configureStylesheet(graph);
				
				$scope.$watchGroup([function () {
					return loadCanvasBlockTiles;
				}, function () {
					return loadToolbarBlockTiles;
				}], function (loaded) {
					if (loaded[0] === true && loaded[1] === true) {
						for (var i = 0; i < toolbarBlockTiles.length; i++) {
							registerToolbarElement(toolbarBlockTiles[i]);
						}
						for (var i = 0; i < canvasBlockTiles.length; i++) {
							addCmsElement(canvasBlockTiles[i], graph, canvasBlockTiles[i].tileData.x, canvasBlockTiles[i].tileData.y, canvasBlockTiles[i].tileData.width, canvasBlockTiles[i].tileData.height);
						}
						onTileEvent({
							event: 'connect-load'
						}).then(function (data) {
							for (var i = 0; i < data.length; i++) {
								var source = graph.getModel().getCell(data[i].source).children.filter(function (cell) { return cell.value == 'Right Link'; })[0];
								var target = graph.getModel().getCell(data[i].target).children.filter(function (cell) { return cell.value == 'Left Link'; })[0];
								graph.connectionHandler.connect(source, target, { id: data[i].id });
							}
							vm.diagramLoaded = true;
						}, function () { });
					}
				});
			}
		}

		function loadTileDataEvent(event, data) {
			if (data.toolbar) {
				var blockTile = toolbarBlockTiles.filter(function (block) { return block.id == data.id && block.toolbar == data.toolbar; })[0];
				if (blockTile !== undefined) {
					blockTile.data = data.data;
				} else {
					toolbarBlockTiles.push(data);
				}
				loadToolbarBlockTiles = toolbarTileCount == toolbarBlockTiles.length && toolbarBlockTiles.length > 0 && toolbarLoaded;
			} else {
				console.log(arguments);
				var blockTile = canvasBlockTiles.filter(function (block) { return block.id == data.id && block.toolbar == data.toolbar; })[0];
				if (blockTile !== undefined) {
					blockTile.data = data.data;
				} else {
					canvasBlockTiles.push(data);
				}
				loadCanvasBlockTiles = canvasTileCount == canvasBlockTiles.length && canvasBlockTiles.length > 0;
			}
		}

		function init() {
			vm.diagramLoaded = false;
			$timeout(function () {
				toggleEditMode();
			}, 1000);

			$scope.$watch(function () { return $scope.mdOnTileEvent; }, function (mdOnTileEvent) {
				if (mdOnTileEvent !== undefined) {
					onTileEvent = mdOnTileEvent;
				}
			});

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
					diagramInit();
				}
			});

			$scope.$on('md-cms-diagram-toolbar-tilecount', function (event, tilecount) {
				toolbarTileCount = tilecount;
			});

			$scope.$on('md-cms-diagram-toolbar-events-loaded', function (event, data) {
				if (data !== undefined && data != null) {
					toolbarId = data;
					toolbarLoaded = true;
				}
			});

			$scope.$watch(function () { return $scope.mdOnTileEvent; }, function (mdOnTileEvent) {
				if (mdOnTileEvent !== undefined) {
					onTileEvent = mdOnTileEvent;
				}
			});

			$scope.$on('md-cms-diagram-element-data', loadTileDataEvent);
			$scope.$on('md-cms-diagram-toolbar-element-data', loadTileDataEvent);
        }

        init();
    }
})();
