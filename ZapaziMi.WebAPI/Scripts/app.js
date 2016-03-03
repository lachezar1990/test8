/// <reference path="angular.js" />

var myApp = angular.module('proba', ['ui.router', 'smart-table', 'ui.bootstrap', 'LocalStorageModule', 'ng-currency', 'cService', 'compService', 'ngProgress'
, 'SalonService', 'commonFactories', 'ngThumbModule', 'angularFileUpload', 'resService', 'mwl.calendar', 'ui.select', 'ngSanitize',
'userService', 'ngAnimate']);

var configFunction = function ($stateProvider, $urlRouterProvider, $locationProvider, $httpProvider) {

    $locationProvider.hashPrefix('!').html5Mode(true);
    $urlRouterProvider.otherwise("/Index")
    $stateProvider
        .state('Index', {
            url: '/Index',
            views: {
                "main": {
                    templateUrl: '/Admin/Index',
                    controller: 'IndexController'
                }
            }

        })
      .state('Reservations', {
          url: "/Reservations",
          views: {
              "main": {
                  templateUrl: "/Admin/Reservations",
                  controller: 'ReservationsController'
              }
          }
      })
    .state('Settings', {
        url: "/Settings",
        views: {
            "main": {
                templateUrl: "/Admin/Settings",
                controller: 'SettingsController'
            }
        }
    })
    .state('Schedule', {
        url: "/Schedule",
        views: {
            "main": {
                templateUrl: "/Admin/Schedule",
                controller: 'ScheduleController'
            }
        }
    })
        .state('Info', {
            url: "/Info",
            views: {
                "main": {
                    templateUrl: "/Admin/Info"
                }
            }
        })
    .state('Register', {
        url: "/Register",
        views: {
            "main": {
                templateUrl: "/Acc/Register",
                controller: 'RegisterController'
            }
        }
    })
    .state('Login', {
        url: "/Login",
        views: {
            "main": {
                templateUrl: "/Acc/Login",
                controller: 'LoginController'
            }
        }
    })
    .state('ChangePassword', {
        url: "/ChangePassword",
        views: {
            "main": {
                templateUrl: "/Acc/ChangePassword",
                controller: 'ChangeController'
            }
        }
    })
    .state('Users', {
        url: "/Users",
        views: {
            "main": {
                templateUrl: "/Admin/Users",
                controller: 'UsersController'
            }
        }
    })
    //.state('ReservationModal', {
    //    url: "/Slave/ReservationModal",
    //    views: {
    //        "modal": {

    //        }
    //    }
    //})
    //;

}

configFunction.$inject = ['$stateProvider', '$urlRouterProvider', '$locationProvider', '$httpProvider'];
myApp.config(configFunction);

myApp.run(['authService', function (authService) {
    authService.fillAuthData();
}]);

myApp.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');

    //initialize get if not there
    if (!$httpProvider.defaults.headers.get) {
        $httpProvider.defaults.headers.get = {};
    }

    // Answer edited to include suggestions from comments
    // because previous version of code introduced browser-related errors

    //disable IE ajax request caching
    $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Mon, 26 Jul 1997 05:00:00 GMT';
    // extra
    $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
    $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
});

myApp.controller('TotalController', ['$scope', '$location', 'authService', 'ReservationService', 'ngProgress',
    function ($scope, $location, authService, ReservationService, ngProgress) {

        $scope.isActive = function (viewLocation) {
            return viewLocation === $location.path();
        };

        $scope.logOut = function () {
            authService.logOut();
            $location.path('/Login');
        }

        $scope.counts = {};

        $scope.authentication = authService.authentication;

        if ($scope.authentication.isAuth && $scope.authentication.isInRole.isSalonAdmin) {
            ngProgress.start();
            ReservationService.getReservationCountForAdmin($scope.authentication.userName).then(function (result) {
                console.log(result);
                $scope.counts.resCountForButton = result;

            }).finally(function () {
                ngProgress.complete();
            });
        }
        else if ($scope.authentication.isAuth && $scope.authentication.isInRole.isUser) {
            $location.path('/Login').search('user', '1');
        }
    }]);

myApp.controller('IndexController', ['$scope', function ($scope) {
    $scope.spice = 'very';

    $scope.chiliSpicy = function () {
        $scope.spice = 'chili';
    };

    $scope.jalapenoSpicy = function () {
        $scope.spice = 'jalapeño';
    };
}]);


myApp.directive("checkboxGroup", function () {
    return {
        restrict: "A",
        link: function (scope, elem, attrs) {
            // Determine initial checked boxes
            if (scope.array.indexOf(scope.item.id) !== -1) {
                elem[0].checked = true;
            }

            // Update array on click
            elem.bind('click', function () {
                var index = scope.array.indexOf(scope.item.id);
                // Add if checked
                if (elem[0].checked) {
                    if (index === -1) scope.array.push(scope.item.id);
                }
                    // Remove if unchecked
                else {
                    if (index !== -1) scope.array.splice(index, 1);
                }
                // Sort and update DOM display
                scope.$apply(scope.array.sort(function (a, b) {
                    return a - b
                }));
            });
        }
    }
});



myApp.filter('propsFilter', function () {
    return function (items, props) {
        var out = [];

        if (angular.isArray(items)) {
            items.forEach(function (item) {
                var itemMatches = false;

                var keys = Object.keys(props);
                for (var i = 0; i < keys.length; i++) {
                    var prop = keys[i];
                    var text = props[prop].toLowerCase();
                    if (item[prop].toString().toLowerCase().indexOf(text) !== -1) {
                        itemMatches = true;
                        break;
                    }
                }

                if (itemMatches) {
                    out.push(item);
                }
            });
        } else {
            // Let the output be the input untouched
            out = items;
        }

        return out;
    };
});