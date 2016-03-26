/// <reference path="angular.js" />

angular.module('saveService', [])

.factory('SaveDataService', function ($http, $q) {
    var deferred = $q.defer();
    var urlBase = 'http://diplomna.somee.com/api/';
    return {
        saveRating: function (rating) {
            $http.post(
                    urlBase + 'Ratings', rating
                ).
                success(function (data) {

                    deferred.resolve(data);

                }).
                error(function () {
                    deferred.resolve({ success: false });
                });

            return deferred.promise;
        }
    }
})