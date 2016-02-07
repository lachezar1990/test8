/// <reference path="angular.js" />

angular.module('cService', [])

.factory('CityService', ['$q', '$http', function ($q, $http) {

    var urlBase = 'http://localhost:20000/api/';

    return {
        getCities: function () {
            var deferred = $q.defer();
            promise = $http.get(urlBase + 'Cities').then(function (response) {
                // The then function here is an opportunity to modify the response
                console.log(response);
                // The return value gets picked up by the then in the controller.
                deferred.resolve(response.data);
            });

            return deferred.promise;
        },
        getNeighbourhoodsByCityId: function (id) {
            var deferred = $q.defer();
            promise = $http.get(urlBase + 'Neighbourhoods/' + id).then(function (response) {
                // The then function here is an opportunity to modify the response
                console.log(response);
                // The return value gets picked up by the then in the controller.
                deferred.resolve(response.data);
            });

            return deferred.promise;
        },
    }
}])