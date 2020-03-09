//Header Module
var headerApp = angular.module('headerApp', ['base', 'ngSanitize', 'pascalprecht.translate', 'ngCookies']);
headerApp.controller("NavbarController", function ($scope, $cookieStore, $http, $log, $window,$location, culture) {
    $scope.allCulture = [];
    $scope.currentCulture = {}
    culture.getAllCulture()
        .then(function (data) {
            $scope.allCulture = data;
            $scope.currentCulture = $scope.allCulture.find(function (item) {
                return item.name == culture.getCurrentCultureName()
            });
        });
    $scope.changeLanguage = function (culture) {
        console.log('ssss');
        //console.log($window.location);
        let path = "/Base/SetLanguage?culture=" + culture + "&returnUrl=" + $window.location.pathname + $window.location.hash;
        //console.log(path);
        $window.location.href = path;
        //$location.url(path);
    }

});
headerApp.config(['$translateProvider', 'cultureProvider', function ($translateProvider, cultureProvider) {
    $translateProvider.useStaticFilesLoader({
        prefix: 'app/layout/i18n/dictionary_',
        suffix: '.json'
    });
    var culture = cultureProvider.getCultureName();
    console.log(culture);
    $translateProvider.useSanitizeValueStrategy('sanitizeParameters');
    $translateProvider.preferredLanguage(culture);
    $translateProvider.fallbackLanguage(culture);
}]);
var navbar = document.getElementById("navbar-element");
angular.bootstrap(navbar, ['headerApp']);



//Footer Module
var footerApp = angular.module('footerApp', ['base', 'ngSanitize', 'pascalprecht.translate', 'ngCookies']);
footerApp.controller("FooterController", function ($scope, $cookieStore) {
});
footerApp.config(['$translateProvider', 'cultureProvider', function ($translateProvider, cultureProvider) {
    $translateProvider.useStaticFilesLoader({
        prefix: 'app/layout/i18n/dictionary_',
        suffix: '.json'
    });
    $translateProvider.useSanitizeValueStrategy('sanitizeParameters');
    var culture = cultureProvider.getCultureName();
    $translateProvider.preferredLanguage(culture);
    $translateProvider.fallbackLanguage(culture);
}]);
var footer = document.getElementById("footer-element");
angular.bootstrap(footer, ['footerApp']);