var basePath = '/app/register';
//module
var app = angular.module("app", ['base', 'ngSanitize', 'ui.select', 'ngMessages', 'ngMask', 'pascalprecht.translate', 'ui.router']);

//controller
app.controller("registerForm",
    function ($scope, $http, $log, $translate, $state) {
        $scope.model = {};
        $scope.selectedService = {};
        $scope.services = [];
        $http({
            method: "Get",
            url: "/Register/GetAllServices"
        }).then(function success(response) {
            $scope.services = response.data.map(function (item) {
                return {
                    value: item.id,
                    speed: item.speed,
                    price: (item.price == 0 ? $translate.instant('Free') : item.price + ' ' + $translate.instant('Rial')),
                    duration: $translate.instant('Days', { day: item.duration })
                }
            });
        }, function error(error) {
            $log.error(error);
        });
        $scope.$watch('selectedService.model', function (value) {
            if (value)
                $scope.model.serviceId = value.value;
        });
        $scope.postForm = function () {
            $http({
                method: "POST",
                url: "/Register/ReceiveForm",
                params: $scope.model
            }).then(function success(response) {
                if (response.data == "success")
                    $state.transitionTo('registerSuccess');
                console.log(response);
                }, function error(error) {

                $log.error(error);
            });
        }
    });
app.controller("registerSuccess",
    function ($scope, $http, $log, $translate, $state) {
        
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
        .state('registerForm', {
            url: '/',
            controller: "registerForm",
            templateUrl: basePath + '/views/register-form.html'
        })
        .state('registerSuccess', {
            url: '/success',
            controller: "registerSuccess",
            templateUrl: basePath + '/views/register-success.html'
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