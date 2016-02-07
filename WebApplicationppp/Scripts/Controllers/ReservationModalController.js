myApp.controller('ReservationModalController', ['$scope', '$modalInstance', 'orderId', 'ngProgress', 'ReservationService',
    function ($scope, $modalInstance, orderId, ngProgress, ReservationService) {

        $scope.loadReservation = function () {
            if (orderId) {
                ngProgress.start();
                $scope.isTableLoading = true;
                ReservationService.getReservationById(orderId).then(function (result) {
                    $scope.order = result;
                    console.log($scope.order);
                    if ($scope.order.OrderDetails.length > 0) {
                        var totalTime = 0;
                        var totalPrice = 0;
                        var hourMinutes = 60;

                        angular.forEach($scope.order.OrderDetails, function (item) {

                            var tempHour = parseInt(item.Time.substring(0, 2));
                            var tempMinute = parseInt(item.Time.substring(3, 5));

                            totalTime += (tempHour * tempMinute) + tempMinute;
                            totalPrice += item.Price;
                        });

                        var totalHours = parseInt((totalTime / hourMinutes));
                        var totalMinutes = (totalTime % hourMinutes);

                        $scope.totalDetailsPrice = totalPrice;

                        $scope.totalDetailsTime = (totalHours < 10 ? '0' : '') + totalHours + ':'
                        + (totalMinutes < 10 ? '0' : '') + totalMinutes;
                    }
                }).finally(function () {
                    ngProgress.complete();
                    $scope.isTableLoading = false;
                });
            }
        };

        $scope.loadReservation();

        var newDate = new Date();
        $scope.minDateFrom = newDate.toISOString();

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

        $scope.editingTime = function () {
            $scope.order.newDate = $scope.order.Date;
            var notUtc = moment($scope.order.StartDateTime);
            notUtc = notUtc.local();
            $scope.order.newStartDateTime = notUtc.utc();
        }

        $scope.saveTimeChanges = function () {
            ngProgress.start();
            $scope.isTableLoading = true;
            var resForSend = $scope.order;

            resForSend.Date = $scope.order.newDate;
            resForSend.StartDateTime = $scope.order.newStartDateTime;

            ReservationService.changeTime(resForSend).then(function (data) {
                $scope.order.Date = data.Date;
                $scope.order.StartDateTime = data.StartDateTime;
                $scope.order.EndDateTime = data.EndDateTime;
                $scope.order.StartTime = data.StartTime;
                $scope.order.EndTime = data.EndTime;
                console.log($scope.order);
            }).finally(function () {
                ngProgress.complete();
                $scope.isTableLoading = false;
            });
        };

        $scope.ok = function () {
            var result = confirm('Сигурен ли сте, че искате да потвърдите резервацията?');
            if (result) {
                var resForSend = $scope.order;
                resForSend.Accepted = true;

                ReservationService.acceptRejectOrder(resForSend).then(function (data) {
                    if (data.success) {
                        console.log(data);
                        $modalInstance.close();
                    }
                    else {
                        resForSend.Accepted = false;

                        alert('Възникна проблем! Статусът не резервацията не е променен!');
                    }
                });

            }
        };

        $scope.reject = function () {
            var result = confirm('Сигурен ли сте, че искате да откажете резервацията?');
            if (result) {
                var resForSend = $scope.order;
                resForSend.Rejected = true;

                ReservationService.acceptRejectOrder(resForSend).then(function (data) {
                    if (data.success) {
                        console.log(data);
                        $modalInstance.close();
                    }
                    else {
                        resForSend.Rejected = false;

                        alert('Възникна проблем! Статусът не резервацията не е променен!');
                    }
                });

            }
        };

        $scope.finished = function () {
            var result = confirm('Сигурен ли сте, че искате да приключите резервацията?');
            if (result) {
                var resForSend = $scope.order;
                resForSend.Finished = true;

                ReservationService.acceptRejectOrder(resForSend).then(function (data) {
                    if (data.success) {
                        console.log(data);
                        $modalInstance.close();
                    }
                    else {
                        resForSend.Finished = false;

                        alert('Възникна проблем! Статусът не резервацията не е променен!');
                    }
                });

            }
        };

        $scope.didntCome = function () {
            var result = confirm('Сигурен ли сте, че клиентът не се е явил резервацията?');
            if (result) {
                var resForSend = $scope.order;
                resForSend.DidntCome = true;

                ReservationService.acceptRejectOrder(resForSend).then(function (data) {
                    if (data.success) {
                        console.log(data);
                        $modalInstance.close();
                    }
                    else {
                        resForSend.DidntCome = false;

                        alert('Възникна проблем! Статусът не резервацията не е променен!');
                    }
                });

            }
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    }]);
