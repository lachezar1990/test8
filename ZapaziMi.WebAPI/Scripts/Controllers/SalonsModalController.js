myApp.controller('SalonsModalController', ['$scope', '$modalInstance', 'username', 'SalonsService', 'ngProgress',
    function ($scope, $modalInstance, username, SalonsService, ngProgress) {

        ngProgress.start();
        $scope.loadingSalonsForm = true;
        SalonsService.getSalonsForAdmin(username).then(function (result) {
            $scope.salons = result;
            
        }).finally(function () {
            $scope.loadingSalonsForm = false;
            ngProgress.complete();
        });

        $scope.cancel = function () {
            $modalInstance.dismiss();
        };

        $scope.hideShowSalon = function (salon, visible) {
            ngProgress.start();
            $scope.loadingSalonsForm = true;
            salon.VisibleForUsers = visible;
            SalonsService.changeSalonVisibility(salon.SalonID, salon).then(function (result) {
                //$scope.salons = result;

            }).finally(function () {
                $scope.loadingSalonsForm = false;
                ngProgress.complete();
            });
        };

        $scope.addMessage = function (salon) {
            ngProgress.start();
            $scope.loadingSalonsForm = true;
            SalonsService.addMessage(salon.SalonID, salon).then(function (result) {
                //$scope.salons = result;
                salon.Add = false;
            }).finally(function () {
                $scope.loadingSalonsForm = false;
                ngProgress.complete();
            });
        };

    }]);