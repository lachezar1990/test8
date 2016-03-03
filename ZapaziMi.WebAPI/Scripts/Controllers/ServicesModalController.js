myApp.controller('ServicesModalController', ['$scope', '$modalInstance', 'salonId', 'serviceId', 'ngProgress', 'SalonsService', 'FileUploader', 'authService',
    function ($scope, $modalInstance, salonId, serviceId, ngProgress, SalonsService, FileUploader, authService) {

        $scope.authentication = authService.authentication;
        $scope.Username = $scope.authentication.userName || 'no auth';

        $scope.ser = {};
        var date = new Date();

        date.setHours(0);
        date.setMinutes(30);

        $scope.ser.LocalTime = date;

        ngProgress.start();
        $scope.isServiceLoading = true;
        if (salonId) {
            $scope.ser.SalonID = salonId;
            SalonsService.getServiceTypes().then(function (result) {
                $scope.types = result;

            }).then(function () {
                if (serviceId) {
                    SalonsService.getServicesByServiceId(serviceId).then(function (result) {
                        $scope.ser = result;
                        console.log($scope.ser);

                        var dateFromDb = new Date();

                        var tempHour = parseInt(result.Time.substring(0, 2));
                        var tempMinute = parseInt(result.Time.substring(3, 5));

                        dateFromDb.setHours(tempHour);
                        dateFromDb.setMinutes(tempMinute);

                        $scope.ser.LocalTime = dateFromDb;

                        $scope.hasId = true;
                        if ($scope.ser.ImageUrl) {
                            $scope.ser.ImageUrl = '/Images/ServiceImages/' + $scope.ser.ImageUrl + '?w=200&h=200&mode=max'
                        }
                    });
                }
            })
                .finally(function () {
                    ngProgress.complete();
                    $scope.isServiceLoading = false;
                });
        }
        else {
            $scope.isServiceLoading = false;
            ngProgress.complete();
            $scope.alertService = { type: 'danger', msg: 'Не сте избрали салон, за да заредите.' };
        }

        if (!serviceId) {
            $scope.hasId = false;
        }
        else {
            $scope.hasId = true
        }

        $scope.serFormSubmit = function (form, ser) {
            if (form.$valid) {
                if (ser.Women || ser.Men || ser.Kids) {
                    $scope.womenMenOrKid = false;
                    ngProgress.start();
                    $scope.isServiceLoading = true;
                    ser.AddedBy = $scope.authentication.userName;
                    var newTime = '';
                    var savedLocalTime = ser.LocalTime;

                    tempHour = ser.LocalTime.getHours();
                    tempMinute = ser.LocalTime.getMinutes();

                    newTime = (tempHour < 10 ? '0' : '') + tempHour + ':'
                     + (tempMinute < 10 ? '0' : '') + tempMinute + ':00';

                    ser.Time = newTime;
                    if (!$scope.hasId) {
                        console.log(ser);
                        console.log(form);

                        SalonsService.saveSer(ser).then(function (data) {
                            console.log(data);
                            $scope.ser = data;
                            $scope.ser.LocalTime = savedLocalTime;
                            $scope.alertService = { type: 'success', msg: 'Данните бяха успешно запазени.' };
                            $scope.insertedSer = data;
                            $scope.hasId = true;
                        }).finally(function () {
                            ngProgress.complete();
                            $scope.isServiceLoading = false;
                        });;

                    }
                    else {
                        SalonsService.updateSer(ser).then(function (data) {
                            console.log(data);
                            $scope.alertService = { type: 'success', msg: 'Данните бяха успешно запазени.' };
                            $scope.insertedSer = data;
                            $scope.hasId = true;
                        }).finally(function () {
                            ngProgress.complete();
                            $scope.isServiceLoading = false;
                        });
                    }
                }
                else {
                    $scope.womenMenOrKid = true;
                }
            }
            else {
                $scope.alertService = { type: 'danger', msg: 'Моля въведете коректни данни!' };
            }
        }

        $scope.deletePic = function () {
            ngProgress.start();
            $scope.isServiceLoading = true;
            SalonsService.delImageSer($scope.ser).then(function (data) {
                console.log(data);
                $scope.alertService = { type: 'success', msg: 'Данните бяха успешно запазени.' };
                $scope.ser.ImageUrl = null;
            }).finally(function () {
                ngProgress.complete();
                $scope.isServiceLoading = false;
            });
        };

        $scope.ok = function () {
            $modalInstance.close();
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

        $scope.closeServiceAlert = function () {
            $scope.alertService = null;
        }

        var uploader = $scope.uploader = new FileUploader({
            url: 'api/SaveImageService/'
        });

        // FILTERS

        uploader.filters.push({
            name: 'imageFilter',
            fn: function (item /*{File|FileLikeObject}*/, options) {
                var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
                return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
            }
        });

        // CALLBACKS

        uploader.onWhenAddingFileFailed = function (item /*{File|FileLikeObject}*/, filter, options) {
            console.info('onWhenAddingFileFailed', item, filter, options);
            $scope.FileTypesService = true;
        };
        uploader.onAfterAddingFile = function (fileItem) {
            console.info('onAfterAddingFile', fileItem);
        };
        uploader.onAfterAddingAll = function (addedFileItems) {
            console.info('onAfterAddingAll', addedFileItems);
        };
        uploader.onBeforeUploadItem = function (item) {
            item.url = 'api/SaveImageService/?id=' + $scope.ser.ServiceID + '&username=' + $scope.Username;
            console.info('onBeforeUploadItem', item);
        };
        uploader.onProgressItem = function (fileItem, progress) {
            console.info('onProgressItem', fileItem, progress);
        };
        uploader.onProgressAll = function (progress) {
            console.info('onProgressAll', progress);
        };
        uploader.onSuccessItem = function (fileItem, response, status, headers) {
            console.info('onSuccessItem', fileItem, response, status, headers);
        };
        uploader.onErrorItem = function (fileItem, response, status, headers) {
            console.info('onErrorItem', fileItem, response, status, headers);
        };
        uploader.onCancelItem = function (fileItem, response, status, headers) {
            console.info('onCancelItem', fileItem, response, status, headers);
        };
        uploader.onCompleteItem = function (fileItem, response, status, headers) {
            console.info('onCompleteItem', fileItem, response, status, headers);
        };
        uploader.onCompleteAll = function () {
            console.info('onCompleteAll');

            SalonsService.getServicesByServiceId($scope.ser.ServiceID).then(function (result) {
                ngProgress.start();
                $scope.isServiceLoading = true;
                $scope.ser = result;
                console.log($scope.ser);

                var dateFromDb = new Date();

                var tempHour = parseInt(result.Time.substring(0, 2));
                var tempMinute = parseInt(result.Time.substring(3, 5));

                dateFromDb.setHours(tempHour);
                dateFromDb.setMinutes(tempMinute);

                $scope.ser.LocalTime = dateFromDb;

                $scope.hasId = true;
                if ($scope.ser.ImageUrl) {
                    $scope.ser.ImageUrl = '/Images/ServiceImages/' + $scope.ser.ImageUrl + '?w=200&h=200&mode=max'
                }
            }).finally(function () {
                ngProgress.complete();
                $scope.isServiceLoading = false;
            });
        };

        console.info('uploader', uploader);

    }]);