myApp.controller('WorkingTimeModalController', ['$scope', '$modalInstance', 'items',
    function ($scope, $modalInstance, items) {

        console.log(items);
        $scope.itemsForUse = angular.copy(items);
        $scope.itemsForReturn = angular.copy(items);
        var date = new Date();
        date.setHours(10);
        date.setMinutes(0);

        var endDate = new Date();
        endDate.setHours(18);
        endDate.setMinutes(0);

        if ($scope.itemsForUse.length == 0) {
            for (var i = 1; i <= 7; i++) {
                $scope.itemsForUse.push({ StartTime: date, EndTime: endDate, Holiday: false, DayOfWeek: i });
                $scope.itemsForReturn.push({ StartTime: date, EndTime: endDate, Holiday: false, DayOfWeek: i });
            }
        }
        else {
            //смяна на времето да е дата, тъй като timepicker-ът не поддържа час само
            angular.forEach($scope.itemsForUse, function (item) {
                if (!item.Holiday) {
                    var newStartDate = new Date();
                    var newEndDate = new Date();

                    tempStartHour = parseInt(item.StartTime.substring(0, 2));
                    tempStartMinute = parseInt(item.StartTime.substring(3, 5));

                    tempEndHour = parseInt(item.EndTime.substring(0, 2));
                    tempEndMinute = parseInt(item.EndTime.substring(3, 5));

                    newStartDate.setHours(tempStartHour);
                    newStartDate.setMinutes(tempStartMinute);

                    newEndDate.setHours(tempEndHour);
                    newEndDate.setMinutes(tempEndMinute);

                    item.StartTime = newStartDate;
                    item.EndTime = newEndDate;
                }
            });
        }

        $scope.changeDay = function (dayOfWeek) {
            $scope.selectedDay = $scope.itemsForUse[dayOfWeek - 1];
            $scope.hideTimes = $scope.selectedDay.Holiday;
            console.log($scope.selectedDay);
        };

        $scope.changeHoliday = function (holiday) {
            if (holiday) {
                $scope.hideTimes = true;
            }
            else {
                var date = new Date();
                date.setHours(10);
                date.setMinutes(0);

                var endDate = new Date();
                endDate.setHours(18);
                endDate.setMinutes(0);

                $scope.selectedDay.StartTime = date;
                $scope.selectedDay.EndTime = endDate;
                $scope.hideTimes = false;
            }
        };

        $scope.ok = function () {
            //смяна на времето, което ще се запази
            var i = 0;
            angular.forEach($scope.itemsForUse, function (item) {
                if (!item.Holiday) {
                    var newStartDate = '';
                    var newEndDate = '';

                    tempStartHour = item.StartTime.getHours();
                    tempStartMinute = item.StartTime.getMinutes();

                    tempEndHour = item.EndTime.getHours();
                    tempEndMinute = item.EndTime.getMinutes();

                    var startTimeString = (tempStartHour < 10 ? '0' : '') + tempStartHour + ':'
                        + (tempStartMinute < 10 ? '0' : '') + tempStartMinute + ':00';

                    var endTimeString = (tempEndHour < 10 ? '0' : '') + tempEndHour + ':'
                        + (tempEndMinute < 10 ? '0' : '') + tempEndMinute + ':00';

                    $scope.itemsForReturn[i].StartTime = startTimeString;
                    $scope.itemsForReturn[i].EndTime = endTimeString;
                    $scope.itemsForReturn[i].Holiday = false;
                }
                else {
                    $scope.itemsForReturn[i].StartTime = null;
                    $scope.itemsForReturn[i].EndTime = null;
                    $scope.itemsForReturn[i].Holiday = true;
                }
                i++;
            });
            console.log($scope.itemsForReturn);
            $modalInstance.close($scope.itemsForReturn);
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
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
    }]);
