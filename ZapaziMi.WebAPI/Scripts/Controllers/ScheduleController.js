myApp.controller('ScheduleController', ['$scope', '$modal', '$timeout', 'ngProgress', 'ReservationService', 'SalonsService', 'authService', '$location',
    function ($scope, $modal, $timeout, ngProgress, ReservationService, SalonsService, authService, $location) {
        moment.locale('bg');

        $scope.calendarView = 'month';

        $scope.editHTML = '<i class=\'glyphicon glyphicon-pencil\'></i>';
        $scope.deleteHTML = '<i class=\'glyphicon glyphicon-remove\'></i>';

        $scope.calendarDay = new Date();

        $scope.authentication = authService.authentication;

        $scope.Username = $scope.authentication.userName || 'no auth';
        $scope.isLoading = true;

        if (!$scope.authentication.isAuth) {
            $location.path('/Login').search('http', '401');
        }

        SalonsService.getSalonsForDdl($scope.Username).then(function (result) {
            $scope.salonsForDdl = result;
            $scope.AlertForSalons = $scope.salonsForDdl.length === 0;
            if (!$scope.AlertForSalons) {
                $scope.SalonID = $scope.salonsForDdl[0].SalonID;
                $scope.loadEvents();
            }

        }).finally(function () {
            $scope.isLoading = false;
        });

        $scope.events = [];

        $scope.loadEvents = function () {
            ngProgress.start();
            $scope.isLoading = true;
            ReservationService.getReservationByIdEvents($scope.SalonID).then(function (result) {
                $scope.events = result;
                console.log($scope.events);
                ReservationService.getReservationCountForAdmin($scope.authentication.userName).then(function (result) {
                    console.log(result);
                    $scope.counts.resCountForButton = result;
                });
            }).finally(function () {
                ngProgress.complete();
                $scope.isLoading = false;
            });
        };


        $scope.deleteEvent = function (id) {
            ngProgress.start();
            $scope.isLoading = true;
            ReservationService.deleteEvent(id, $scope.Username).then(function (result) {
                alert('Събитието беше изтрито!');
                $scope.loadEvents();
            }).finally(function () {
                ngProgress.complete();
                $scope.isLoading = false;
            });
        };

        //    id: 1,
        //    title: 'My event title', // The title of the event
        //    type: 'info', // The type of the event (determines its color). Can be important, warning, info, inverse, success or special
        //    startsAt: new Date(2015, 4, 13, 8), // A javascript date object for when the event starts
        //    endsAt: new Date(2015, 4, 13, 15), // A javascript date object for when the event ends
        //    //editable: false, // If edit-event-html is set and this field is explicitly set to false then dont make it editable
        //    //deletable: false, // If delete-event-html is set and this field is explicitly set to false then dont make it deleteable
        //    incrementsBadgeTotal: true //If set to false then will not count towards the badge total amount on the month and year view
        //},

        $scope.eventEdited = function (event) {
            event = event || {};
            $scope.openModal(event.UniqueID, $scope.SalonID);
        };

        $scope.eventDeleted = function (event) {
            var result = confirm('Сигурен ли сте, че искате да изтриете резервацията?');
            if (result) {
                $scope.deleteEvent(event.UniqueID);
            }
        };


        $scope.openModal = function (eventId, salonId) {

            var modalInstance = $modal.open({
                templateUrl: 'EventModal.html',
                controller: 'EventModalController',
                size: 'lg',
                resolve: {
                    orderId: function () {
                        return eventId;
                    },
                    salonId: function () {
                        return salonId;
                    }
                }
            });

            modalInstance.result.then(function () {
                $timeout(function () {
                    $scope.loadEvents();
                }, 100);
            }, function () {
                $timeout(function () {
                    $scope.loadEvents();
                }, 100);
            });
        };

    }]);