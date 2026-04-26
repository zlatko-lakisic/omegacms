'use strict';

var gulp = require('gulp');
var sass = require('gulp-sass');
var path = require('path');
var conf = require('./gulp/conf');
var gulpDocumentation = require('gulp-documentation');
var concat = require('gulp-concat');
var typedoc = require('gulp-typedoc');
var $ = require('gulp-load-plugins')();
var wiredep = require('wiredep').stream;
var _ = require('lodash');

var buildStyles = function ()
{
    var sassOptions = {
        style: 'expanded'
    };

    var injectFiles = gulp.src([
        path.join(conf.paths.src, '/scripts/app/core/scss/**/*.scss'),
        path.join(conf.paths.src, '/scripts/app/core/**/*.scss'),
        path.join(conf.paths.src, '/scripts/app/core/**/**/*.scss'),
        path.join(conf.paths.src, '/scripts/app/**/*.scss'),
        path.join('!' + conf.paths.src, '/scripts/app/core/directives/md-cms-grid/md-cms-grid-canvas.scss'),
        path.join('!' + conf.paths.src, '/scripts/app/core/directives/md-generictype-designer/form/md-generictype-designer-form-canvas.scss'),
        path.join('!' + conf.paths.src, '/scripts/app/main/components/material-docs/demo-partials/**/*.scss'),
        path.join('!' + conf.paths.src, '/scripts/app/core/scss/partials/**/*.scss'),
        path.join('!' + conf.paths.src, '/scripts/app/index.scss')
    ], {
        read:         false
    });

    var injectOptions = {
        transform   : function (filePath)
        {
            filePath = filePath.replace(conf.paths.src + '/scripts/app/', '');
            return '@import "' + filePath + '";';
        },
        starttag    : '// injector',
        endtag      : '// endinjector',
        addRootSlash: false
    };

    return gulp.src([
            path.join(conf.paths.src, '/scripts/app/index.scss')
        ])
        .pipe($.inject(injectFiles, injectOptions))
        .pipe(wiredep(_.extend({}, conf.wiredep)))
        .pipe($.sourcemaps.init())
        .pipe($.sass(sassOptions).on('error', conf.errorHandler('Sass')))
        .pipe($.autoprefixer()).on('error', conf.errorHandler('Autoprefixer'))
        .pipe($.sourcemaps.write())
        .pipe(gulp.dest('./'));
};

function buildScripts()
{
    return gulp.src(path.join(conf.paths.src, '/app/**/*.js'))
        .pipe($.size())
};

gulp.task('clean', function ()
{
    return $.del([path.join(conf.paths.dist, '/'), path.join(conf.paths.tmp, '/')]);
});

gulp.task('styles', function ()
{
    return buildStyles();
});

gulp.task('scripts', function ()
{
    return buildScripts();
});

gulp.task('inject', gulp.series('scripts', 'styles', function ()
{
    var injectStyles = gulp.src([
        path.join(conf.paths.tmp, '/serve/app/**/*.css'),
        path.join('!' + conf.paths.tmp, '/serve/app/vendor.css')
    ], {read: false});

    var injectScripts = gulp.src([
            path.join(conf.paths.src, '/scripts/app/**/*.module.js'),
            path.join(conf.paths.src, '/scripts/app/**/*.js'),
            path.join('!' + conf.paths.src, '/scripts/app/**/*.spec.js'),
            path.join('!' + conf.paths.src, '/scripts/app/**/*.mock.js'),
        ])
        .pipe($.angularFilesort()).on('error', conf.errorHandler('AngularFilesort'));

    var injectOptions = {
        ignorePath  : [conf.paths.src, path.join(conf.paths.tmp, '/serve')],
        addRootSlash: false
    };

    return gulp.src(path.join(conf.paths.src, '/*.html'))
        .pipe($.inject(injectStyles, injectOptions))
        .pipe($.inject(injectScripts, injectOptions))
        .pipe(wiredep(_.extend({}, conf.wiredep)))
        .pipe(gulp.dest(path.join(conf.paths.tmp, '/serve')));
}));


/**
 *  Default task clean temporaries directories and launch the
 *  main optimization build task
 */
gulp.task('default', gulp.parallel('clean', function () {
    gulp.start('build');
}));

/**
 * Generate the MD.CMS.BusinessLogicTS Documentation
 *
 */
gulp.task("MDCMSBusinessLogicTS-html-documentation", function () {
    return gulp
        .src(["./MD.CMS.Administration.Core.Web/scripts/businessLogic/**/*.ts"])
        .pipe(typedoc({
            // TypeScript options (see typescript docs)
            module: "amd",
            target: "es5",

            // Output options (see typedoc docs)
            json: "./MD.CMS.Administration.Core.Web/scripts/businessLogicMinified/documentation/TSApi.json",

            // TypeDoc options (see typescript docs)
            version: true,
        }));
});

/**
 * Generate the MD.CMS.BusinessLogicTS Documentation Exported
 *
 */
gulp.task("MDCMSBusinessLogicTS-external-html-documentation", function () {
    return gulp
        .src(["./MD.CMS.Administration.Core.Web/scripts/businessLogic/**/*.ts"])
        .pipe(typedoc({
            // TypeScript options (see typescript docs)
            module: "amd",
            target: "es5",

            // Output options (see typedoc docs)
            out: "./dist/doc",
            json: "dist/doc-json/json.json",
            theme: "node_modules/typedoc-default-themes",

            // TypeDoc options (see typescript docs)
            version: true,
        }));
});

gulp.task('MDCMSBusinessLogicJS-compiled', function () {
    return gulp.src(['./MD.CMS.Administration.Core.Web/scripts/businessLogic_ts/jsWrappers/starter.js',
                     './bower_components/lodash/lodash.js',
                     './bower_components/moment-all/moment.js',
                     './MD.CMS.Administration.Core.Web/scripts/businessLogic_ts/compiled/businessLogic.compiled.js',
                     './MD.CMS.Administration.Core.Web/scripts/businessLogic_ts/jsWrappers/ender.js'
        ], {
            allowEmpty:   true
        })
        .pipe(concat('businessLogic.compiled.js'))
        .pipe(gulp.dest('./MD.CMS.Administration.Core.Web/scripts/businessLogicMinified'));
});
