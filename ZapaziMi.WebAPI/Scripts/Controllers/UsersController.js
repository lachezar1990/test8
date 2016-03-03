myApp.controller('UsersController', ['$scope', 'ngProgress', 'SalonsService', 'authService', 'UsersService', '$modal',
    function ($scope, ngProgress, SalonsService, authService, UsersService, $modal) {

        $scope.authentication = authService.authentication;
        $scope.Username = $scope.authentication.userName || 'no auth';

        $scope.itemsByPage = 10;

        $scope.displayedCollection = [].concat($scope.users);

        $scope.loadUsers = function (roleId) {
            if (!$scope.AlertForSalons) {
                ngProgress.start();
                $scope.isTableLoading = true;
                UsersService.getUsers(roleId).then(function (result) {
                    console.log(result);
                    $scope.users = result;
                    $scope.hasUsers = $scope.users.length > 0;
                    console.info($scope.displayedCollection);
                }).finally(function () {
                    ngProgress.complete();
                    $scope.isTableLoading = false;
                });
            }
        };

        $scope.filter = {};

        ngProgress.start();
        UsersService.getRoles().then(function (result) {
            console.log(result);
            $scope.roles = result;
            $scope.filter.RoleId = $scope.roles[0].Id;
            $scope.loadUsers($scope.roles[0].Id);
        }).finally(function () {
            ngProgress.complete();
        });

        $scope.openModal = function (username) {

            var modalInstance = $modal.open({
                templateUrl: 'SalonsModal.html',
                controller: 'SalonsModalController',
                size: 'lg',
                resolve: {
                    username: function () {
                        return username;
                    }
                }
            });
        };

    }]);