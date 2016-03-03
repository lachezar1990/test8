/// <reference path="angular.js" />

angular.module('userService', [])

    .factory('UsersService', ['$http', '$q', function ($http, $q) {

        var urlBase = 'http://localhost:20000/api/';
        return {
            
            getUsers: function (role) {
                var deferred = $q.defer();
                promise = $http.get(urlBase + 'Account/GetAllUsers?role=' + role).then(function (response) {
                    // The then function here is an opportunity to modify the response
                    console.log(response);
                    // The return value gets picked up by the then in the controller.
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            getRoles: function () {
                var deferred = $q.defer();
                promise = $http.get(urlBase + 'Account/GetAllRoles').then(function (response) {
                    // The then function here is an opportunity to modify the response
                    console.log(response);
                    // The return value gets picked up by the then in the controller.
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            updateCompany: function (company) {
                var deferred = $q.defer();
                $http.put(
                        urlBase + 'Companies/' + company.CompanyID, company
                    ).
                    success(function (data) {

                        deferred.resolve(data);

                    }).
                    error(function (response) {
                        console.log(response.status);
                        deferred.resolve({ success: false });
                    });

                return deferred.promise;
            }
        }
    }])