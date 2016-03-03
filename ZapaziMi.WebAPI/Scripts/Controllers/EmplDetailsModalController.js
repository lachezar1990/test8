myApp.controller('EmplDetailsModalController', ['$scope', '$modalInstance', 'salonId', 'insert', 'emplId', 'SalonsService', 'ngProgress', 'FileUploader', 'authService',
    function ($scope, $modalInstance, salonId, insert, emplId, SalonsService, ngProgress, FileUploader, authService) {

        $scope.authentication = authService.authentication;
        $scope.Username = $scope.authentication.userName || 'no auth';

        $scope.insert = insert;

        ngProgress.start();
        $scope.isPositionLoading = true;
        if (salonId) {
            SalonsService.getPositions(salonId).then(function (result) {
                $scope.positions = result;

            }).then(function () {
                if (emplId) {
                    SalonsService.getSalonEmplById(emplId).then(function (result) {
                        $scope.empl = result;
                        console.log($scope.empl);
                        $scope.hasId = true;
                        if ($scope.empl.ImageUrl) {
                            $scope.empl.ImageUrl = '/Images/EmplImages/' + $scope.empl.ImageUrl + '?w=200&h=200&mode=max'
                        }
                    });
                }
            })
                .finally(function () {
                    ngProgress.complete();
                    $scope.isPositionLoading = false;
                });
        }
        else {
            $scope.isPositionLoading = false;
            ngProgress.complete();
            $scope.alertPos = { type: 'danger', msg: 'Не сте избрали салон, за да заредите.' };
        }

        if ($scope.insert) {
            $scope.hasId = false;
        }
        else {

        }

        $scope.emplFormSubmit = function (form, empl) {
            if (form.$valid) {
                ngProgress.start();
                $scope.isPositionLoading = true;
                empl.AddedBy = $scope.Username;
                if ($scope.insert) {
                    console.log(empl);
                    console.log(form);

                    SalonsService.saveEmpl(empl).then(function (data) {
                        console.log(data);
                        $scope.empl = data;
                        $scope.alertEmpl = { type: 'success', msg: 'Данните бяха успешно запазени.' };
                        $scope.insertedEmpl = data;
                        $scope.hasId = true;
                        $scope.insert = false;
                    }).finally(function () {
                        ngProgress.complete();
                        $scope.isPositionLoading = false;
                    });;

                }
                else {
                    SalonsService.updateEmpl(empl).then(function (data) {
                        console.log(data);
                        $scope.alertEmpl = { type: 'success', msg: 'Данните бяха успешно запазени.' };
                        $scope.insertedEmpl = data;
                        $scope.hasId = true;
                    }).finally(function () {
                        ngProgress.complete();
                        $scope.isPositionLoading = false;
                    });
                }
            }
            else {
                $scope.alertEmpl = { type: 'danger', msg: 'Моля въведете коректни данни!' };
            }
        }

        $scope.deletePic = function () {
            ngProgress.start();
            $scope.isPositionLoading = true;
            SalonsService.delImageEmpl($scope.empl).then(function (data) {
                console.log(data);
                $scope.alertEmpl = { type: 'success', msg: 'Данните бяха успешно запазени.' };
                $scope.empl.ImageUrl = null;
            }).finally(function () {
                ngProgress.complete();
                $scope.isPositionLoading = false;
            });
        };

        $scope.ok = function () {
            $modalInstance.close();
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

        $scope.closeEmplAlert = function () {
            $scope.alertEmpl = null;
        }

        var uploader = $scope.uploader = new FileUploader({
            url: 'api/SaveImageEmpl/'
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
            $scope.FileTypesEmpl = true;
        };
        uploader.onAfterAddingFile = function (fileItem) {
            console.info('onAfterAddingFile', fileItem);
        };
        uploader.onAfterAddingAll = function (addedFileItems) {
            console.info('onAfterAddingAll', addedFileItems);
        };
        uploader.onBeforeUploadItem = function (item) {
            item.url = 'api/SaveImageEmpl/?id=' + $scope.empl.EmployeeID + '&username=' + $scope.Username;
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

            ngProgress.start();
            $scope.isPositionLoading = true;

            SalonsService.getSalonEmplById($scope.empl.EmployeeID).then(function (result) {

                $scope.empl = result;
                console.log($scope.empl);
                $scope.hasId = true;
                if ($scope.empl.ImageUrl) {
                    $scope.empl.ImageUrl = '/Images/EmplImages/' + $scope.empl.ImageUrl + '?w=200&h=200&mode=max'
                }
            }).finally(function () {
                ngProgress.complete();
                $scope.isPositionLoading = false;
            });;
        };

        console.info('uploader', uploader);

    }]);
