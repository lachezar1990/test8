myApp.controller('ChangeController', ['$scope', '$location', '$timeout', 'authService', 'ngProgress',
    function ($scope, $location, $timeout, authService, ngProgress) {

        $scope.savedSuccessfully = false;
        $scope.message = "";

        $scope.model = {
            UserName: $scope.authentication.userName,
            OldPassword: "",
            NewPassword: "",
            ConfirmPassword: ""
        };

        ngProgress.reset();

        $scope.change = function () {
            ngProgress.start();
            $scope.isLoading = true;

            authService.changePassword($scope.model).then(function (response) {

                $scope.savedSuccessfully = true;
                $scope.message = "Вашата парола беше успешно сменена! Ще бъдете прехвърлени към страницата за вход.";
                startTimer();

            },
             function (response) {
                 var errors = [];
                 for (var key in response.data.ModelState) {
                     for (var i = 0; i < response.data.ModelState[key].length; i++) {
                         errors.push(response.data.ModelState[key][i]);
                     }
                 }
                 $scope.message = "Паролата не беше сменена:" + errors.join(' ');
             }).finally(function () {
                 ngProgress.complete();
                 $scope.isLoading = false;
             });
        };

        var startTimer = function () {
            var timer = $timeout(function () {
                $timeout.cancel(timer);
                $location.path('/Login');
            }, 2000);
        }
    }]);