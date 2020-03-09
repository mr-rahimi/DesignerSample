//module
var app = angular.module("base", ['ngCookies']);

//provider
app.provider("culture", function () {
    return {
        getCultureName: function () {
            var $cookies;
            angular.injector(['ngCookies']).invoke(['$cookies', function (_$cookies_) {
                $cookies = _$cookies_;
            }]);

            var x = $cookies.get(".AspNetCore.Culture");
            if (x) {
                var y = x.split('|')[1];
                var z = y.split('=')[1]
            }
            //else
            //    z = 'fa-IR';
            return z;
        },
        $get: function ($http, $q, $cookies) {
            return {
                getAllCulture: function () {
                    //return 'hh';
                    var defer = $q.defer();
                    $http({
                        method: "GET",
                        url: "/Base/GetAllCulture"
                    }).then(function success(response) {
                        defer.resolve(response.data);
                    }, function error(response) {
                        $log.error(response);
                    });
                    return defer.promise;
                },
                getCurrentCultureName: function () {
                    var x = $cookies.get(".AspNetCore.Culture");
                    if (x) {
                        var y = x.split('|')[1];
                        var z = y.split('=')[1]
                    }
                    return z;
                },
                getCurrentCulture: function () {
                    if (x) {
                        var y = x.split('|')[1];
                        var z = y.split('=')[1]
                    }
                    
                    return z;
                },
            };
        }
    };
});
