var basePath = '/app/login';
//module
var app = angular.module("app", ['base', 'ngSanitize', 'ui.select', 'ngMessages', 'ngMask', 'pascalprecht.translate', 'ui.router']);

//controller
app.controller("loginPage",
    function ($scope, $cookies, $log, $http, $window, $state) {
        $scope.errors = [];
        $scope.model = {};
        $scope.model.LinkLogin = $window.LinkLogin;
        $scope.model.LinkOrig = $window.LinkOrig;
        $scope.model.IP = $window.IP;
        $scope.model.ChapId = $window.ChapId;
        $scope.model.ChapChallenge = $window.ChapChallenge;
        $scope.model.Error = $window.Error;
        $scope.doLogin = function () {
            var obj = {};
            obj.username = $scope.model.username;
            obj.password = hexMD5($window.ChapId + $scope.model.password + $window.ChapChallenge);
            obj.linkOrig = $window.LinkOrig;
            obj.linkLogin = $window.LinkLogin;

            $http({
                method: "POST",
                url: "/Login/LoginPost",
                params: obj
            }).then(function success(response) {
                $state.transitionTo('loginSuccess');
            }, function error(error) {
                console.log(error.data);
                if (error.data.hasOwnProperty('InternalServerError'))
                    $scope.errors.push("ServerError");
                if (error.data.hasOwnProperty('captcha'))
                    $scope.errors.push("CaptchaError");

                refreshCaptcha();
            });
        }
        $scope.refreshCaptcha = function () {
            refreshCaptcha();
        }
        function refreshCaptcha() {
            $("#captchaImg").css("background-image", "url('/Login/GetCaptchaImage?random=" + new Date().getTime());
        }
    });
app.controller("loginSuccess",
    function ($scope, $cookies, $log, $http, $window) {

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
        .state('login', {
            url: '',
            controller: "loginPage",
            templateUrl: basePath + '/views/login-page.html'
        })
        .state('loginSuccess', {
            url: 'success',
            controller: "loginSuccess",
            templateUrl: basePath + '/views/login-success-page.html'
        });
    $urlRouterProvider.otherwise('/');
}]);
