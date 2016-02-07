/// <reference path="angular.js" />

angular.module('compService', [])

    .factory('CompanyService', ['$http', '$q', function ($http, $q) {

        var urlBase = 'http://localhost:20000/api/';
        return {
            saveCompany: function (company) {
                var deferred = $q.defer();
                $http.post(
                        urlBase + 'Companies', company
                    ).
                    success(function (data) {

                        deferred.resolve(data);

                    }).
                    error(function () {
                        deferred.resolve({ success: false });
                    });

                return deferred.promise;
            },
            getCompany: function (username) {
                var deferred = $q.defer();
                promise = $http.get(urlBase + 'Companies/' + username).then(function (response) {
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