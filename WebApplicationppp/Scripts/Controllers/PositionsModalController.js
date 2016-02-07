myApp.controller('PositionsModalController', ['$scope', '$modalInstance', 'salonId', 'SalonsService', 'ngProgress',
    function ($scope, $modalInstance, salonId, SalonsService, ngProgress) {

        $scope.positionToAdd = {};
        $scope.positionToAdd.positionName = null;
        $scope.positionCountForClose = 0;

        ngProgress.start();
        $scope.isPositionLoading = true;
        if (salonId) {
            SalonsService.getPositions(salonId).then(function (result) {
                $scope.salonPositions = result;

            }).finally(function () {
                ngProgress.complete();
                $scope.isPositionLoading = false;
            });
        }
        else {
            $scope.isPositionLoading = false;
            ngProgress.complete();
            $scope.alertPos = { type: 'danger', msg: 'Не сте избрали салон, за да заредите.' };
        }

        $scope.addPosition = function (positionName) {
            ngProgress.start();
            $scope.isPositionLoading = true;
            var position = {};
            position.Username = $scope.Username;
            position.PositionName = positionName;
            position.SalonID = salonId;

            SalonsService.savePosition(position).then(function (data) {
                console.log(data);
                $scope.positionToAdd.positionName = null;
                $scope.positionCountForClose++;
                $scope.salonPositions.push({ Deleted: false, PositionName: data.PositionName, SalonID: data.SalonID, AddedOn: data.AddedOn, PositionID: data.PositionID });
            }).finally(function () {
                ngProgress.complete();
                $scope.isPositionLoading = false;
            });
        };

        $scope.deletePos = function (pos, index) {
            ngProgress.start();
            $scope.isPositionLoading = true;
            var position = { PositionID: pos.PositionID, Username: 'da se opravi!', IsForDelete: true };

            SalonsService.deletePosition(position).then(function (data) {
                $scope.salonPositions.splice(index, 1);
                $scope.positionCountForClose--;

                //$scope.alertSalon = { type: 'success', msg: 'Данните бяха успешно запазени.' };
            }).finally(function () {
                ngProgress.complete();
                $scope.isPositionLoading = false;
            });
        };
        $scope.updatePos = function (pos, index) {
            ngProgress.start();
            $scope.isPositionLoading = true;
            var position = { PositionID: pos.PositionID, Username: 'da se opravi!', IsForDelete: false, PositionName: pos.PositionNameNew };

            SalonsService.deletePosition(position).then(function (data) {
                pos.PositionName = pos.PositionNameNew;
                pos.Editing = false;
                //$scope.alertSalon = { type: 'success', msg: 'Данните бяха успешно запазени.' };
            }).finally(function () {
                ngProgress.complete();
                $scope.isPositionLoading = false;
            });
        };

        $scope.closePosAlert = function () {
            $scope.alertPos = null;
        }

        $scope.ok = function () {
            $modalInstance.close($scope.positionCountForClose);
        };

        $scope.cancel = function () {
            $modalInstance.dismiss($scope.positionCountForClose);
        };
    }]);
