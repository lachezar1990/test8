myApp.controller('LoginController', ['$scope', '$location', 'authService', 'ngProgress', 'ReservationService',
    function ($scope, $location, authService, ngProgress, ReservationService) {

        $scope.loginData = {
            userName: '',
            password: ''
        };

        ngProgress.reset();

        $scope.message = '';
        $scope.warningMsg = '';

        var searchObject = $location.search();

        if (searchObject.http) {
            $scope.warningMsg = 'Нямате достъп до този ресурс! Моля, влезте във вашия акаунт или се регистрирайте!';
        }
        
        if (searchObject.user === '1') {
            $scope.warningMsg = 'Вашият акаунт няма достъп до администраторския панел!';
        }
        
        $scope.login = function () {
            ngProgress.start();
            $scope.isLoading = true;

            authService.login($scope.loginData).then(function (response) {
                $scope.authentication = authService.authentication;

                if ($scope.authentication.isAuth && $scope.authentication.isInRole.isSalonAdmin) {
                    ngProgress.start();
                    ReservationService.getReservationCountForAdmin($scope.authentication.userName).then(function (result) {
                        console.log(result);
                        $scope.counts.resCountForButton = result;

                    }).finally(function () {
                        ngProgress.complete();

                    });
                }
                else if ($scope.authentication.isAuth && $scope.authentication.isInRole.isUser) {
                    $scope.warningMsg = 'Вашият акаунт няма достъп до администраторския панел!';
                    $location.path('/Login').search('user', '1');
                }
                $location.path('/Index').search({ 'http': null, 'user': null });

            },
             function (err) {
                 $scope.message = err.error_description;
             }).finally(function () {
                 ngProgress.complete();
                 $scope.isLoading = false;
             });
        };

    }]);