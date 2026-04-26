/** Maps to legacy Fuse `ELEMENTS_NAVIGATION` + `LAYOUT_NAVIGATION` (AngularJS Material 1.x docs). */
export const AmElementGroups: { group: string; label: string; items: { slug: string; name: string; state: string }[] }[] =
  [
    {
      group: 'inputs',
      label: 'Inputs',
      items: [
        { slug: 'autocomplete', name: 'Autocomplete', state: 'material_components_autocomplete' },
        { slug: 'checkbox', name: 'Checkbox', state: 'material_components_checkbox' },
        { slug: 'chips', name: 'Chips', state: 'material_components_chips' },
        { slug: 'date-picker', name: 'Date Picker', state: 'material_components_datepicker' },
        { slug: 'input', name: 'Input', state: 'material_components_input' },
        { slug: 'radio-button', name: 'Radio Button', state: 'material_components_radioButton' },
        { slug: 'select', name: 'Select', state: 'material_components_select' },
        { slug: 'slider', name: 'Slider', state: 'material_components_slider' },
        { slug: 'switch', name: 'Switch', state: 'material_components_switch' },
      ],
    },
    {
      group: 'buttons',
      label: 'Buttons',
      items: [
        { slug: 'button', name: 'Button', state: 'material_components_button' },
        { slug: 'fab-actions', name: 'Fab Actions', state: 'material_components_fabActions' },
        { slug: 'fab-speed-dial', name: 'Fab Speed Dial', state: 'material_components_fabSpeedDial' },
        { slug: 'fab-toolbar', name: 'Fab Toolbar', state: 'material_components_fabToolbar' },
      ],
    },
    {
      group: 'content-elements',
      label: 'Content elements',
      items: [
        { slug: 'bottom-sheet', name: 'Bottom Sheet', state: 'material_components_bottomSheet' },
        { slug: 'card', name: 'Card', state: 'material_components_card' },
        { slug: 'content', name: 'Content', state: 'material_components_content' },
        { slug: 'dialog', name: 'Dialog', state: 'material_components_dialog' },
        { slug: 'icon', name: 'Icon', state: 'material_components_icon' },
        { slug: 'sidenav', name: 'Sidenav', state: 'material_components_sidenav' },
        { slug: 'subheader', name: 'Subheader', state: 'material_components_subheader' },
        { slug: 'tabs', name: 'Tabs', state: 'material_components_tabs' },
        { slug: 'toast', name: 'Toast', state: 'material_components_toast' },
        { slug: 'toolbar', name: 'Toolbar', state: 'material_components_toolbar' },
        { slug: 'tooltip', name: 'Tooltip', state: 'material_components_tooltip' },
        { slug: 'whiteframe', name: 'Whiteframe', state: 'material_components_whiteframe' },
      ],
    },
    {
      group: 'lists',
      label: 'Lists',
      items: [
        { slug: 'grid-list', name: 'Grid List', state: 'material_components_gridList' },
        { slug: 'list', name: 'List', state: 'material_components_list' },
      ],
    },
    {
      group: 'menus',
      label: 'Menus',
      items: [
        { slug: 'menu', name: 'Menu', state: 'material_components_menu' },
        { slug: 'menu-bar', name: 'Menu Bar', state: 'material_components_menu-bar' },
      ],
    },
    {
      group: 'progress',
      label: 'Progress',
      items: [
        { slug: 'progress-circular', name: 'Progress Circular', state: 'material_components_progressCircular' },
        { slug: 'progress-linear', name: 'Progress Linear', state: 'material_components_progressLinear' },
      ],
    },
    {
      group: 'others',
      label: 'Others',
      items: [
        { slug: 'divider', name: 'Divider', state: 'material_components_divider' },
        { slug: 'ripple', name: 'Ripple', state: 'material_core_ripple' },
        { slug: 'sticky', name: 'Sticky', state: 'material_components_sticky' },
        { slug: 'swipe', name: 'Swipe', state: 'material_components_swipe' },
        { slug: 'util', name: 'Util', state: 'material_core_util' },
        { slug: 'virtual-repeat', name: 'Virtual Repeat', state: 'material_components_virtualRepeat' },
      ],
    },
  ];

export const AmLayoutItems: { slug: string; name: string; state: string }[] = [
  { slug: 'introduction', name: 'Introduction', state: 'material_components_layout_introduction' },
  { slug: 'container', name: 'Layout Containers', state: 'material_components_layout_containers' },
  { slug: 'children', name: 'Layout Children', state: 'material_components_layout_grid' },
  { slug: 'alignment', name: 'Child Alignment', state: 'material_components_layout_align' },
  { slug: 'options', name: 'Extra Options', state: 'material_components_layout_options' },
  { slug: 'tips', name: 'Troubleshooting', state: 'material_components_layout_tips' },
];
