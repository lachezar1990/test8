/// <reference path="angular.js" />

angular.module('commonFactories', [])

    .factory('CommonFactory', ['$filter', function ($filter) {

        var myFactory = {
            getWorkingTimeString: function (scheduleFromScope) {

                var result = '';
                var i = 0;
                var weekdayNames = [
              "\u043f\u043d",
                    "\u0432\u0442",
                    "\u0441\u0440",
                    "\u0447\u0442",
                    "\u043f\u0442",
                    "\u0441\u0431",
                    "\u043d\u0434"
                ];

                var schedule = angular.copy(scheduleFromScope);

                var groupedTimes = [];
                var tempTimes = [];
                var remove = [];
                var holidays = [];
                angular.forEach(schedule, function (item) {
                    console.log(weekdayNames[item.DayOfWeek - 1]);
                    console.log(item);
                    for (j = 0; schedule.length > j; j++) {
                        if (!schedule[j].Holiday) {
                            if ((item.StartTime === schedule[j].StartTime && item.EndTime === schedule[j].EndTime) && !schedule[j].Changed) {
                                tempTimes.push(schedule[j]);
                                remove.push(j); //от тук
                                console.log(schedule[j]);
                                console.log(schedule);
                                console.log(tempTimes);
                            }
                        }
                        else {
                            holidays.push(schedule[j]);
                            remove.push(j); //от тук
                        }

                    }

                    if (!item.Changed) {
                        if (!item.Holiday) {
                            groupedTimes.push(tempTimes);
                        }
                        angular.forEach(remove, function (index) {
                            schedule[index] = { 'Changed': 1 };
                        });
                    }
                    tempTimes = [];
                    remove = [];
                    i++;
                });
                console.log('nakraq');
                console.log(schedule);
                console.log(groupedTimes);

                var tempText = '';
                var daysInArray = [];
                var textResult = [];

                angular.forEach(groupedTimes, function (array) {
                    var currLength = array.length;
                    var oneElement = currLength === 1;
                    var hasDiffDays = false;
                    for (var i = 0; i < currLength; i++) {
                        if (oneElement) {
                            tempText = weekdayNames[array[i].DayOfWeek - 1] + ' : ' + $filter('limitTo')(array[i].StartTime, 5)
                                + ' - ' + $filter('limitTo')(array[i].EndTime, 5);
                        }
                        else {
                            daysInArray.push(weekdayNames[array[i].DayOfWeek - 1]);
                            if (i !== currLength - 1) {
                                if ((array[i].DayOfWeek + 1) !== array[i + 1].DayOfWeek) {
                                    hasDiffDays = true;
                                }
                            }
                        }
                    }

                    if (oneElement) {
                        textResult.push(tempText);
                    }
                    else {
                        if (hasDiffDays) {
                            textResult.push((daysInArray.join(", ") + ' : ' + $filter('limitTo')(array[0].StartTime, 5) + ' - '
                                + $filter('limitTo')(array[0].EndTime, 5)));
                        }
                        else {
                            textResult.push((daysInArray[0] + ' - ' + daysInArray[currLength - 1] + ' : ' + $filter('limitTo')(array[0].StartTime, 5) + ' - '
                                + $filter('limitTo')(array[0].EndTime, 5)))
                        }
                    }

                    daysInArray = [];
                });

                angular.forEach(holidays, function (array) {
                    daysInArray.push(weekdayNames[array.DayOfWeek - 1]);
                });
                if (daysInArray.length > 0) {
                    textResult.push(('Почивни дни: ' + daysInArray.join(", ")));
                }
                else {
                    textResult.push(('Почивни дни: няма'));
                }
                daysInArray = [];

                console.log(textResult);
                return textResult;
            }

        };
        return myFactory;

    }]);