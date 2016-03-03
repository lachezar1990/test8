myApp.controller('ReservationsController', ['$scope', '$modal', 'ReservationService', 'ngProgress', 'SalonsService', '$timeout', 'authService',
    function ($scope, $modal, ReservationService, ngProgress, SalonsService, $timeout, authService) {

        $scope.filter = {};

        $scope.itemsByPage = 10;
        $scope.authentication = authService.authentication;

        $scope.Username = $scope.authentication.userName || 'no auth';

        var newDate = new Date();

        $scope.filter.dtFrom = newDate.toISOString();
        $scope.filter.Username = $scope.Username;

        var dateTo = new Date();
        dateTo.setMonth(dateTo.getMonth() + 1);

        $scope.filter.dtTo = dateTo.toISOString();

        $scope.maxDateFrom = dateTo.toISOString();
        $scope.maxDateTo = dateTo.toISOString();

        $scope.minDateFrom = new Date(2015, 4, 5).toISOString();


        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };

        $scope.loadingSalonsDDL = true;

        SalonsService.getSalonsForDdl($scope.Username).then(function (result) {
            $scope.salonsForDdl = result;
            $scope.AlertForSalons = $scope.salonsForDdl.length === 0;
            if (!$scope.AlertForSalons) {
                $scope.filter.SalonID = $scope.salonsForDdl[0].SalonID;
            }
            $scope.loadReservations();
        }).finally(function () {
            $scope.loadingSalonsDDL = false;
        });

        $scope.displayedCollection = [].concat($scope.reservations);

        $scope.loadReservations = function () {
            if (!$scope.AlertForSalons) {
                ngProgress.start();
                $scope.isTableLoading = true;
                ReservationService.getReservations($scope.filter).then(function (result) {
                    console.log(result);
                    $scope.reservations = result;
                    $scope.hasReservations = $scope.reservations.length > 0;
                }).finally(function () {
                    ngProgress.complete();
                    $scope.isTableLoading = false;
                });
            }
        };

        $scope.openFrom = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.openedFrom = true;
        };
        $scope.openTo = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.openedTo = true;
        };

        $scope.openModal = function (orderId) {

            var modalInstance = $modal.open({
                templateUrl: 'ReservationModal.html',
                controller: 'ReservationModalController',
                size: 'lg',
                resolve: {
                    orderId: function () {
                        return orderId;
                    }
                }
            });

            modalInstance.result.then(function () {
                $timeout(function () {
                    $scope.loadReservations();
                }, 100);
            }, function () {
                $timeout(function () {
                    $scope.loadReservations();
                }, 100);
                console.log('Modal dismissed at: ' + new Date());
            });
        };


    }
]);
