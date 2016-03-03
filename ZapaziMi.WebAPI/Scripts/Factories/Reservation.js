/// <reference path="angular.js" />

angular.module('resService', [])

.factory('ReservationService', ['$q', '$http', function ($q, $http) {

    var urlBase = 'http://localhost:20000/api/';

    return {
        getReservations: function (filter) {
            var deferred = $q.defer();

            var filterForCtrl = {};
            filterForCtrl.From = filter.dtFrom;
            filterForCtrl.To = filter.dtTo;
            filterForCtrl.Status = filter.Status;
            filterForCtrl.SearchText = filter.text;
            filterForCtrl.UserName = filter.Username;
            filterForCtrl.SalonID = filter.SalonID;

            promise = $http.get(urlBase + 'Reserve/GetForAdminTable/', {
                params: filterForCtrl
            }).then(function (response) {
                // The then function here is an opportunity to modify the response
                console.log(response);
                // The return value gets picked up by the then in the controller.
                deferred.resolve(response.data);
            });

            return deferred.promise;
        },
        getReservationById: function (id) {
            var deferred = $q.defer();
            promise = $http.get(urlBase + 'reserve/GetForAdminModal/' + id).then(function (response) {
                // The then function here is an opportunity to modify the response
                console.log(response);
                // The return value gets picked up by the then in the controller.
                deferred.resolve(response.data);
            });

            return deferred.promise;
        },
        getReservationCountForAdmin: function (username) {
            var deferred = $q.defer();
            promise = $http.get(urlBase + 'Reserve/GetReservationCountForAdmin/' + username).then(function (response) {
                // The then function here is an opportunity to modify the response
                console.log(response);
                // The return value gets picked up by the then in the controller.
                deferred.resolve(response.data);
            });

            return deferred.promise;
        },
        getReservationByIdEvents: function (id) {
            var deferred = $q.defer();
            promise = $http.get(urlBase + 'ReserveForSchedule/' + id).then(function (response) {
                // The then function here is an opportunity to modify the response
                console.log(response);
                // The return value gets picked up by the then in the controller.
                deferred.resolve(response.data);
            });

            return deferred.promise;
        },
        ReserveForScheduleById: function (id) {
            var deferred = $q.defer();
            promise = $http.get(urlBase + 'ReserveForScheduleById/' + id).then(function (response) {
                // The then function here is an opportunity to modify the response
                console.log(response);
                // The return value gets picked up by the then in the controller.
                deferred.resolve(response.data);
            });

            return deferred.promise;
        },
        acceptRejectOrder: function (order) {
            var deferred = $q.defer();
            $http.put(
                    urlBase + 'Reserve/' + order.UniqueID, order
                ).
                success(function (data) {
                    deferred.resolve({ success: true });
                    deferred.resolve(data);

                }).
                error(function (response) {
                    if (response.status === 409) {
                        alert('Възникна проблем! Статусът не резервацията не е променен!');
                    }
                    deferred.resolve({ success: false });
                });

            return deferred.promise;
        },
        changeTime: function (order) {
            var deferred = $q.defer();
            $http.put(
                    urlBase + 'Reserve/ChangeTime/' + order.UniqueID, order
                ).
                success(function (data) {
                    deferred.resolve(data);

                }).
                error(function (response) {
                    if (response.status === 409) {
                        alert('Възникна проблем! Статусът не резервацията не е променен!');
                    }
                    deferred.resolve({ success: false });
                });

            return deferred.promise;
        },
        saveEvent: function (event) {
            var deferred = $q.defer();
            $http.post(
                    urlBase + 'Reserve/PostEvent', event
                ).
                success(function (data) {

                    deferred.resolve(data);

                }).
                error(function () {
                    deferred.resolve({ success: false });
                });

            return deferred.promise;
        },
        updateEvent: function (event) {
            var deferred = $q.defer();
            $http.put(
                    urlBase + 'Reserve/PutEvent/' + event.UniqueID, event
                ).
                success(function (data) {
                    deferred.resolve(data);

                }).
                error(function (response) {
                    
                    deferred.resolve({ success: false });
                });

            return deferred.promise;
        },
        deleteEvent: function (id, username) {
            var deferred = $q.defer();
            promise = $http.delete(urlBase + 'reserve/' + id + '/' + username).then(function (response) {
                // The then function here is an opportunity to modify the response
                console.log(response);
                // The return value gets picked up by the then in the controller.
                deferred.resolve(response.data);
            });

            return deferred.promise;
        },
    }
}])