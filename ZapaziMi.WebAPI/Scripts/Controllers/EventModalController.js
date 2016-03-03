myApp.controller('EventModalController', ['$scope', '$modalInstance', 'SalonsService', '$filter', 'ngProgress', 'ReservationService',
    'orderId', 'salonId', 'authService',
    function ($scope, $modalInstance, SalonsService, $filter, ngProgress, ReservationService, orderId, salonId, authService) {

        $scope.event = {};
        if (!salonId) {
            $modalInstance.dismiss('Няма салон');
        }
        $scope.event.SalonID = salonId;
        $scope.event.Services = [];
        $scope.event.UniqueID = orderId;

        $scope.authentication = authService.authentication;

        $scope.Username = $scope.authentication.userName;

        $scope.event.UserName = $scope.Username;

        $scope.loadEvent = function () {
            if (orderId) {
                ngProgress.start();
                $scope.isLoading = true;
                ReservationService.ReserveForScheduleById(orderId).then(function (result) {
                    $scope.event = result;
                    console.log($scope.events);

                    var notUtc = moment($scope.event.StartDateTime);
                    notUtc = notUtc.local();
                    $scope.event.StartDateTime = notUtc.utc().toISOString();
                    $scope.selectedService();
                }).finally(function () {
                    ngProgress.complete();
                    $scope.isLoading = false;
                });
            }
        };


        var newDate = new Date();
        $scope.minDateFrom = newDate.toISOString();

        $scope.event.StartDateTime = new Date(newDate.getFullYear(), newDate.getMonth(), newDate.getDate(), 12, 0);

        var dateTo = new Date();
        dateTo.setMonth(dateTo.getMonth() + 1);

        $scope.maxDateFrom = dateTo.toISOString();

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };

        $scope.openFrom = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.openedFrom = true;
        };

        $scope.ok = function () {
            $modalInstance.close();
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

        $scope.salonServices = [];
        $scope.totalPrice = 0;
        $scope.totalTime = '00:00';

        SalonsService.getServicesBySalonIdSche(salonId).then(function (result) {
            ngProgress.start();
            $scope.isLoading = true;

            $scope.salonServices = result;
            console.log($scope.salonServices);

            SalonsService.getEventEmplBySalonId(salonId).then(function (result) {
                $scope.emplForDdl = result;
                console.log($scope.emplForDdl)

                if (orderId) {
                    $scope.loadEvent();
                }

            });
        }).finally(function () {
            ngProgress.complete();
            $scope.isLoading = false;
        });

        $scope.selectedService = function (item, model) {
            console.log($scope.event.Services);

            var totalPrice = 0;
            var totalTime = 0;
            var tempHour = 0;
            var tempMinute = 0;
            var hourMinutes = 60;
            $scope.selectedServices = [];

            angular.forEach($scope.event.Services, function (item) {
                var service = $filter('filter')($scope.salonServices, { ServiceID: item })[0];

                totalPrice += service.Price;
                tempHour = parseInt(service.Time.substring(0, 2));
                tempMinute = parseInt(service.Time.substring(3, 5));

                totalTime += (tempHour * hourMinutes) + tempMinute;

                $scope.selectedServices.push(service);
            });

            $scope.totalPrice = totalPrice;

            var totalHours = parseInt((totalTime / hourMinutes));
            var totalMinutes = (totalTime % hourMinutes);

            $scope.totalTime = (totalHours < 10 ? '0' : '') + totalHours + ':' + (totalMinutes < 10 ? '0' : '') + totalMinutes;
        };

        $scope.eventFormSubmit = function (form, event) {
            if (form.$valid) {
                $scope.noServ = false;
                if ($scope.event.Services.length > 0) {
                    ngProgress.start();
                    $scope.isLoading = true;
                    if (!$scope.event.UniqueID) {
                        ReservationService.saveEvent(event).then(function (data) {
                            $scope.event.UniqueID = data;
                            console.log($scope.event);
                        }).finally(function () {
                            ngProgress.complete();
                            $scope.isLoading = false;
                        });
                    }
                    else {
                        ReservationService.updateEvent(event).then(function (data) {
                            console.log($scope.event);
                        }).finally(function () {
                            ngProgress.complete();
                            $scope.isLoading = false;
                        });
                    }
                }
                else {
                    $scope.noServ = true;
                }
            }
        };

    }]);