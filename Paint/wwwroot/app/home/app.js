var basePath = '/app/home';
//module
var app = angular.module("app", ['base', 'ngSanitize', 'ui.select', 'ngMessages', 'ngMask', 'pascalprecht.translate', 'ui.router']);

//controller
app.controller("indexPage",
    function ($scope, $state) {
        var _this = this;
        var canvas = null;
        this.$scope = $scope;
        _this.gotoStep2 = function () {
            $state.go('step2', { canvas: canvas });
        }
        _this.initCanvas = function () {
            canvas = new fabric.Canvas('c');
            canvas.setDimensions({
                width: 800,
                height: 400
            });

            canvas.setBackgroundColor('#565656', canvas.renderAll.bind(canvas));
            canvas.on('object:selected', function () {
                _this.$scope.$evalAsync(function () {
                    _this.activeObject = canvas.getActiveObject();
                });
            });
            canvas.on('selection:cleared', function () {
                _this.$scope.$evalAsync(function () {
                    _this.activeObject = null;
                });
            });
            canvas.on('selection:updated', function () {
                _this.$scope.$evalAsync(function () {
                    _this.activeObject = canvas.getActiveObject();
                });
            });
        };
        _this.onWindowResize = function () {
            canvas.setDimensions({
                width: 800,
                height: 400
            });
        };
        _this.addText = function () {
            var text = new fabric.IText('Sample Text', {
                left: canvas.width / 2,
                top: canvas.height / 2,
                fill: '#e0f7fa',
                fontFamily: 'sans-serif',
                hasRotatingPoint: false,
                centerTransform: true,
                originX: 'center',
                originY: 'center'
            });
            canvas.add(text);
            //text.on('selected', function () {
            //    _this.color = _this.mdPickerColors.getColor(text.get('fill'));
            //});
            text.on('deselected', function () {
                _this.color = null;
            });
        };
        _this.addTextbox = function () {
            var textbox = new fabric.Textbox('Textbox', {
                left: canvas.width / 2,
                top: canvas.height / 2,
                fill: '#e0f7fa',
                fontFamily: 'sans-serif',
                hasRotatingPoint: false,
                centerTransform: true,
                originX: 'center',
                originY: 'center'
            });
            textbox.set('width', textbox.text.length * textbox.fontSize / 2);
            canvas.add(textbox);
        };
        _this.addRect = function () {
            canvas.add(new fabric.Rect({
                left: canvas.width / 2,
                top: canvas.height / 2,
                fill: '#ffa726',
                width: 100,
                height: 100,
                originX: 'center',
                originY: 'center'
            }));
        };
        _this.addCircle = function () {
            canvas.add(new fabric.Circle({
                left: canvas.width / 2,
                top: canvas.height / 2,
                fill: '#26a69a',
                radius: 50,
                originX: 'center',
                originY: 'center'
            }));
        };
        _this.addTriangle = function () {
            canvas.add(new fabric.Triangle({
                left: canvas.width / 2,
                top: canvas.height / 2,
                fill: '#78909c',
                width: 100,
                height: 100,
                originX: 'center',
                originY: 'center'
            }));
        };
        _this.remove = function () {
            var activeObjects = canvas.getActiveObjects();
            canvas.discardActiveObject();
            if (activeObjects.length) {
                canvas.remove.apply(canvas, activeObjects);
            }
        };
        _this.getStyle = function () {
            if (_this.activeObject != null) {
                if (_this.color != null) {
                    if (_this.color.hex !== _this.activeObject.fill.toLowerCase()) {
                        _this.activeObject.set('fill', _this.color.hex);
                        canvas.requestRenderAll();
                    }
                    return _this.color.style;
                }
                else {
                    return {
                        'background-color': _this.activeObject.fill,
                        'color': _this.activeObject.fill
                    };
                }
            }
        };
        _this.initCanvas();
        window.addEventListener('resize', _this.onWindowResize);
    });
app.controller("step2Page",
    function ($scope, $stateParams, $state) {
        if (!$stateParams.canvas) {
            $state.go('index');
            return;
        }
        //console.log($stateParams.canvas.toJSON());
        var _this = this;

        var canvas = null;
        this.$scope = $scope;
        _this.initCanvas = function () {
            canvas = new fabric.Canvas('c');
            canvas.loadFromJSON($stateParams.canvas.toJSON());
            canvas.setDimensions({
                width: 800,
                height: 400
            });
            canvas._objects.forEach(function (item) {

                item.hasControls = true;
                item.lockMovementX = true;
                item.lockMovementY = true;
                item.lockScalingX = true;
                item.lockScalingY = true;
                item.lockRotation = true;
            });
            canvas.setBackgroundColor('#565656', canvas.renderAll.bind(canvas));
            canvas.on('object:selected', function () {
                _this.$scope.$evalAsync(function () {
                    _this.activeObject = canvas.getActiveObject();
                });
            });
            canvas.on('selection:cleared', function () {
                _this.$scope.$evalAsync(function () {
                    _this.activeObject = null;
                });
            });
            canvas.on('selection:updated', function () {
                _this.$scope.$evalAsync(function () {
                    _this.activeObject = canvas.getActiveObject();
                });
            });
        };

        _this.onWindowResize = function () {
            canvas.setDimensions({
                width: 800,
                height: 400
            });

        };
        _this.getStyle = function () {
            if (_this.activeObject != null) {
                if (_this.color != null) {
                    if (_this.color.hex !== _this.activeObject.fill.toLowerCase()) {
                        _this.activeObject.set('fill', _this.color.hex);
                        canvas.requestRenderAll();
                    }
                    return _this.color.style;
                }
                else {
                    return {
                        'background-color': _this.activeObject.fill,
                        'color': _this.activeObject.fill
                    };
                }
            }
        };
        _this.send = function () {
            alert(JSON.stringify(canvas));
        };
        _this.render = function () {
            canvas.loadFromJSON(canvas.toJSON());
        };
        _this.initCanvas();
        window.addEventListener('resize', this.onWindowResize);
    });
app.config(['$translateProvider', '$stateProvider', '$urlRouterProvider', 'cultureProvider', function ($translateProvider, $stateProvider, $urlRouterProvider, cultureProvider) {

    $translateProvider.useStaticFilesLoader({
        prefix: basePath + '/i18n/dictionary_',
        suffix: '.json'
    });
    var culture = cultureProvider.getCultureName();
    $translateProvider.useSanitizeValueStrategy('sanitizeParameters');
    $translateProvider.preferredLanguage(culture);
    $translateProvider.fallbackLanguage(culture);

    $stateProvider
        .state('index', {
            url: '/',
            controller: "indexPage",
            controllerAs: 'index',
            templateUrl: basePath + '/views/index-page.html'
        })
        .state('step2', {
            url: '/step2',
            controller: "step2Page",
            controllerAs: 'step2',
            params: { canvas: null },
            templateUrl: basePath + '/views/step2-page.html'
        });
    $urlRouterProvider.otherwise('/');
}]);
app.directive('ngLoading', function ($compile) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var loading = angular.element('<div class="test"></div>');
            element.append(loading);
            $compile(loading)(scope);
        }
    };
});