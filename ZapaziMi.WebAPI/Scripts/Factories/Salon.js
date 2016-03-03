/// <reference path="angular.js" />

angular.module('SalonService', [])

    .factory('SalonsService', ['$http', '$q', function ($http, $q) {

        var urlBase = 'http://localhost:20000/api/';
        return {
            saveSalon: function (salon) {
                var deferred = $q.defer();
                $http.post(
                        urlBase + 'Salons', salon
                    ).
                    success(function (data) {

                        deferred.resolve(data);

                    }).
                    error(function () {
                        deferred.resolve({ success: false });
                    });

                return deferred.promise;
            },
            savePosition: function (position) {
                var deferred = $q.defer();
                $http.post(
                        urlBase + 'Positions', position
                    ).
                    success(function (data) {

                        deferred.resolve(data);

                    }).
                    error(function () {
                        deferred.resolve({ success: false });
                    });

                return deferred.promise;
            },
            saveEmpl: function (empl) {
                var deferred = $q.defer();
                $http.post(
                        urlBase + 'Employees', empl
                    ).
                    success(function (data) {

                        deferred.resolve(data);

                    }).
                    error(function () {
                        deferred.resolve({ success: false });
                    });

                return deferred.promise;
            },
            saveSer: function (ser) {
                var deferred = $q.defer();
                $http.post(
                        urlBase + 'Services', ser
                    ).
                    success(function (data) {

                        deferred.resolve(data);

                    }).
                    error(function () {
                        deferred.resolve({ success: false });
                    });

                return deferred.promise;
            },
            getSalonsForDdl: function (username) {
                var deferred = $q.defer();
                promise = $http.get(urlBase + 'Salons/' + username).then(function (response) {
                    // The then function here is an opportunity to modify the response
                    console.log(response);
                    // The return value gets picked up by the then in the controller.
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            getSalon: function (id) {
                var deferred = $q.defer();
                promise = $http.get(urlBase + 'GetSalon/' + id).then(function (response) {
                    // The then function here is an opportunity to modify the response
                    console.log(response);
                    // The return value gets picked up by the then in the controller.
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            getSalonImages: function (id) {
                var deferred = $q.defer();
                promise = $http.get(urlBase + 'SalonImages/' + id).then(function (response) {
                    // The then function here is an opportunity to modify the response
                    console.log(response);
                    // The return value gets picked up by the then in the controller.
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            getSalonEmpl: function (id) {
                var deferred = $q.defer();
                promise = $http.get(urlBase + 'Employees/' + id).then(function (response) {
                    // The then function here is an opportunity to modify the response
                    console.log(response);
                    // The return value gets picked up by the then in the controller.
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            getSalonEmplById: function (id) {
                var deferred = $q.defer();
                promise = $http.get(urlBase + 'Employees/GetById/' + id).then(function (response) {
                    // The then function here is an opportunity to modify the response
                    console.log(response);
                    // The return value gets picked up by the then in the controller.
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            getEventEmplBySalonId: function (id) {
                var deferred = $q.defer();
                promise = $http.get(urlBase + 'Employees/GetForEvent/' + id).then(function (response) {
                    // The then function here is an opportunity to modify the response
                    console.log(response);
                    // The return value gets picked up by the then in the controller.
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            getPositions: function (id) {
                var deferred = $q.defer();
                promise = $http.get(urlBase + 'Positions/' + id).then(function (response) {
                    // The then function here is an opportunity to modify the response
                    console.log(response);
                    // The return value gets picked up by the then in the controller.
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            getServicesBySalonId: function (id) {
                var deferred = $q.defer();
                promise = $http.get(urlBase + 'Services/ForAdmin/' + id).then(function (response) {
                    // The then function here is an opportunity to modify the response
                    console.log(response);
                    // The return value gets picked up by the then in the controller.
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            getServicesBySalonIdSche: function (id) {
                var deferred = $q.defer();
                promise = $http.get(urlBase + 'Services/ForAdminSch/' + id).then(function (response) {
                    // The then function here is an opportunity to modify the response
                    console.log(response);
                    // The return value gets picked up by the then in the controller.
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            getServicesByServiceId: function (id) {
                var deferred = $q.defer();
                promise = $http.get(urlBase + 'Services/ForAdminModal/' + id).then(function (response) {
                    // The then function here is an opportunity to modify the response
                    console.log(response);
                    // The return value gets picked up by the then in the controller.
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            getServiceTypes: function () {
                var deferred = $q.defer();
                promise = $http.get(urlBase + 'ServiceTypes').then(function (response) {
                    // The then function here is an opportunity to modify the response
                    console.log(response);
                    // The return value gets picked up by the then in the controller.
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            updateSalon: function (salon) {
                var deferred = $q.defer();
                $http.put(
                        urlBase + 'Salons/' + salon.SalonID, salon
                    ).
                    success(function (data) {

                        deferred.resolve(data);

                    }).
                    error(function (response) {
                        console.log(response.status);
                        deferred.resolve({ success: false });
                    });

                return deferred.promise;
            },
            updateEmpl: function (empl) {
                var deferred = $q.defer();
                $http.put(
                        urlBase + 'Employees/' + empl.EmployeeID, empl
                    ).
                    success(function (data) {

                        deferred.resolve(data);

                    }).
                    error(function (response) {
                        console.log(response.status);
                        deferred.resolve({ success: false });
                    });

                return deferred.promise;
            },
            updateSer: function (ser) {
                var deferred = $q.defer();
                $http.put(
                        urlBase + 'Services/' + ser.ServiceID, ser
                    ).
                    success(function (data) {

                        deferred.resolve(data);

                    }).
                    error(function (response) {
                        console.log(response.status);
                        deferred.resolve({ success: false });
                    });

                return deferred.promise;
            },
            updateEmplWorkTime: function (emplId, schedule) {
                var deferred = $q.defer();
                $http.put(
                        urlBase + 'Employees/ChangeWorkingTime/' + emplId, schedule
                    ).
                    success(function (data) {

                        deferred.resolve(data);

                    }).
                    error(function (response) {
                        console.log(response.status);
                        deferred.resolve({ success: false });
                    });

                return deferred.promise;
            },
            delImageEmpl: function (empl) {
                var deferred = $q.defer();
                $http.put(
                        urlBase + 'Employees/DeleteImage/' + empl.EmployeeID, empl
                    ).
                    success(function (data) {

                        deferred.resolve(data);

                    }).
                    error(function (response) {
                        console.log(response.status);
                        deferred.resolve({ success: false });
                    });

                return deferred.promise;
            },
            delEmpl: function (id, username) {
                var deferred = $q.defer();
                promise = $http.delete(urlBase + 'Employees/' + id + '/' + username).success(function (response) {
                    // The then function here is an opportunity to modify the response
                    console.log(response);
                    // The return value gets picked up by the then in the controller.
                    deferred.resolve(response.data);
                    }).
                    error(function (response) {
                        console.log(response);
                        deferred.resolve({ success: false });
                    });;

                return deferred.promise;
            },
            delSer: function (id, username) {
                var deferred = $q.defer();
                promise = $http.delete(urlBase + 'Services/' + id + '/' + username).success(function (response) {
                    // The then function here is an opportunity to modify the response
                    console.log(response);
                    // The return value gets picked up by the then in the controller.
                    deferred.resolve(response.data);
                }).
                    error(function (response) {
                        console.log(response);
                        deferred.resolve({ success: false });
                    });;

                return deferred.promise;
            },
            delImageSer: function (ser) {
                var deferred = $q.defer();
                $http.put(
                        urlBase + 'Services/DeleteImage/' + ser.ServiceID, ser
                    ).
                    success(function (data) {

                        deferred.resolve(data);

                    }).
                    error(function (response) {
                        console.log(response.status);
                        deferred.resolve({ success: false });
                    });

                return deferred.promise;
            },
            deleteAnImage: function (image) {
                var deferred = $q.defer();
                $http.put(
                        urlBase + 'SalonImages/' + image.ImageID, image
                    ).
                    success(function (data) {

                        deferred.resolve(data);

                    }).
                    error(function (response) {
                        console.log(response.status);
                        deferred.resolve({ success: false });
                    });

                return deferred.promise;
            },
            deletePosition: function (pos) {
                var deferred = $q.defer();
                $http.put(
                        urlBase + 'Positions/' + pos.PositionID, pos
                    ).
                    success(function (data) {

                        deferred.resolve(data);

                    }).
                    error(function (response) {
                        console.log(response.status);
                        deferred.resolve({ success: false });
                    });

                return deferred.promise;
            },
            // за админите

            getSalonsForAdmin: function (username) {
                var deferred = $q.defer();
                promise = $http.get(urlBase + 'SalonsForAdmin/' + username).then(function (response) {
                    // The then function here is an opportunity to modify the response
                    console.log(response);
                    // The return value gets picked up by the then in the controller.
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            changeSalonVisibility: function (id, salon) {
                var deferred = $q.defer();
                $http.put(
                        urlBase + 'SalonsForAdmin/ChangeVisibility/' + id, salon
                    ).
                    success(function (data) {

                        deferred.resolve(data);

                    }).
                    error(function (response) {
                        console.log(response.status);
                        deferred.resolve({ success: false });
                    });

                return deferred.promise;
            },
            addMessage: function (id, salon) {
                var deferred = $q.defer();
                $http.put(
                        urlBase + 'SalonsForAdmin/AddMessage/' + id, salon
                    ).
                    success(function (data) {

                        deferred.resolve(data);

                    }).
                    error(function (response) {
                        console.log(response.status);
                        deferred.resolve({ success: false });
                    });

                return deferred.promise;
            },
        }
    }]);