myApp.controller('RegisterController', ['$scope', '$location', '$timeout', 'authService', 'ngProgress',
    function ($scope, $location, $timeout, authService, ngProgress) {

        $scope.savedSuccessfully = false;
        $scope.message = "";

        $scope.registration = {
            userName: "",
            password: "",
            confirmPassword: ""
        };

        ngProgress.reset();

        $scope.signUp = function () {
            ngProgress.start();
            $scope.isLoading = true;

            authService.saveRegistration($scope.registration).then(function (response) {

                $scope.savedSuccessfully = true;
                $scope.message = "Потребителят беше регистриран успешно! Ще бъдете прехвърлени след 2 секунди към страницата за вход.";
                startTimer();

            },
             function (response) {
                 var errors = [];
                 for (var key in response.data.ModelState) {
                     for (var i = 0; i < response.data.ModelState[key].length; i++) {
                         errors.push(response.data.ModelState[key][i]);
                     }
                 }
                 $scope.message = "Потребителят не беше регистриран:" + errors.join(' ');
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