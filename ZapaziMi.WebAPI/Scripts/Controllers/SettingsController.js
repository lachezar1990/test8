myApp.controller('SettingsController', ['$scope', '$modal', 'CityService', 'CompanyService', 'ngProgress', 'SalonsService', 'CommonFactory',
    'FileUploader', '$timeout', 'authService',
    function ($scope, $modal, CityService, CompanyService, ngProgress, SalonsService, CommonFactory, FileUploader, $timeout, authService) {

        $scope.authentication = authService.authentication;
        $scope.Username = $scope.authentication.userName || 'no auth';
        $scope.loadingCompanyForm = true;
        $scope.loadingSalonsForm = true;
        $scope.showAlertForSalons = true;
        $scope.salon = {};
        $scope.salon.SalonID = null;
        $scope.company = {};

        ngProgress.start();

        CityService.getCities().then(function (result) {
            $scope.cities = result;
        }).then(function () {
            CompanyService.getCompany($scope.Username).then(function (result) {
                $scope.company = result;
                console.log($scope.company);
            }).finally(function () {
                $scope.loadingCompanyForm = false;
            });
        }).then(function () {
            SalonsService.getSalonsForDdl($scope.Username).then(function (result) {
                $scope.salonsForDdl = result;
                if (result.length > 0) {
                    $scope.showAlertForSalons = false;
                }
            }).finally(function () {
                $scope.loadingSalonsForm = false;
            });
        })
            .finally(function () { ngProgress.complete(); });

        console.log($scope.cities);

        $scope.companyFormSubmit = function (form, company) {
            if (form.$valid) {
                console.log(form);
                console.log(company);
                ngProgress.start();
                $scope.success = false;

                var companyForSave = {};

                companyForSave.CompanyName = company.CompanyName;
                companyForSave.Email = company.Email;
                companyForSave.CityID = company.CityID;
                companyForSave.AddressText = company.CompanyAddress;
                companyForSave.Phones = company.CompanyPhones;
                companyForSave.CreateBy = $scope.Username;

                if (!company.CompanyID) {
                    CompanyService.saveCompany(companyForSave).then(function (data) {
                        console.log(data);
                        $scope.alertComp = { type: 'success', msg: 'Данните бяха успешно запазени.' };
                    }).finally(function () { ngProgress.complete(); });;
                }
                else {
                    companyForSave.CompanyID = company.CompanyID
                    CompanyService.updateCompany(companyForSave).then(function (data) {
                        console.log(data);
                        $scope.alertComp = { type: 'success', msg: 'Данните бяха успешно запазени.' };
                    }).finally(function () { ngProgress.complete(); });;
                }
            }
            else {
                $scope.alertComp = { type: 'danger', msg: 'Моля въведете коректни данни!' };
            }
        }

        $scope.closeAlert = function (index) {
            $scope.alertComp = null;
        };

        $scope.loadSalon = function (id) {
            ngProgress.start();
            $scope.loadingSalonsForm = true;
            $scope.alertSalon = null;
            if (id) {
                SalonsService.getSalon(id).then(function (result) {
                    $scope.salonForForm = result;
                    console.log($scope.salonForForm);
                    console.log($scope.salonForForm.SalonImages);
                    if (result.length == 0) {
                        $scope.showSalonForm = false;
                    }
                    else {
                        $scope.showSalonForm = true;
                        $scope.neighbourhoods = $scope.salonForForm.Neighbourhoods;
                        $scope.hasNeighbourhoods = $scope.neighbourhoods.length > 0;

                        $scope.salonForForm.SalonPhones = $scope.salonForForm.SalonPhones.join();
                        if ($scope.salonForForm.SalonSchedule.length == 0) {
                            $scope.workTimeText = 'Не сте въвели работно време';
                            $scope.redTextWorkingTime = true;
                        }
                        else {
                            $scope.workTimeText = 'Въвели сте';
                            $scope.redTextWorkingTime = false;
                        }

                        $scope.workTimeText = CommonFactory.getWorkingTimeString($scope.salonForForm.SalonSchedule).join(' ,');
                        $scope.redTextWorkingTime = false;

                        uploader.queue = [];
                        $scope.MainImagePath = null;
                        $scope.MainImage = null;

                        angular.forEach($scope.salonForForm.SalonImages, function (item) {
                            var dummy = new FileUploader.FileItem(uploader, {
                                lastModifiedDate: item.AddedOn,
                                size: 1e6,
                                type: 'image/jpeg',
                                name: item.ImageName
                            });

                            dummy.progress = 100;
                            dummy.isUploaded = true;
                            dummy.isSuccess = true;
                            dummy.IsMain = item.IsMain;
                            dummy.ImageID = item.ImageID;
                            dummy.ImagePath = '/Images/SalonImages/' + item.ImagePath + '?w=80&h=80&mode=max';
                            dummy.PathFromDb = item.ImagePath;

                            uploader.queue.push(dummy);

                            if (item.IsMain) {
                                $scope.MainImagePath = '/Images/SalonImages/' + item.ImagePath + '?w=300&h=250&mode=max';
                                $scope.MainImage = item;
                            }
                        });
                    }
                }).finally(function () {
                    $scope.loadingSalonsForm = false;
                    ngProgress.complete();
                });
            }
            else {
                $scope.showSalonForm = false;
                $scope.loadingSalonsForm = false;
                ngProgress.complete();
                $scope.alertSalon = { type: 'warning', msg: 'Не сте избрали салон, за да заредите.' };
            }
        }

        $scope.changeDdlSCity = function (id) {
            if (id) {
                ngProgress.start();
                $scope.salonForForm.NeighbourhoodID = null;
                CityService.getNeighbourhoodsByCityId(id).then(function (result) {
                    $scope.neighbourhoods = result;
                    $scope.hasNeighbourhoods = $scope.neighbourhoods.length > 0;
                }).finally(function () { ngProgress.complete(); });
            }
        };

        $scope.salonFormSubmit = function (form, salon) {
            if (form.$valid) {
                ngProgress.start();
                $scope.salonForForm.Username = $scope.Username;
                if (salon.SalonID) {
                    console.log(salon);
                    console.log(form);

                    SalonsService.updateSalon(salon).then(function (data) {
                        console.log(data);
                        $scope.alertSalon = { type: 'success', msg: 'Данните бяха успешно запазени.' };
                    }).finally(function () { ngProgress.complete(); });;
                }
                else {
                    SalonsService.saveSalon(salon).then(function (data) {
                        console.log(data);
                        $scope.alertComp = { type: 'success', msg: 'Данните бяха успешно запазени.' };
                        $scope.salonsForDdl.push({ SalonID: data.SalonID, SalonName: data.SalonName });
                        $scope.salon.SalonID = data.SalonID;
                        $scope.salonForForm.SalonID = data.SalonID;

                        uploader.queue = [];
                        $scope.MainImagePath = null;
                        $scope.MainImage = null;
                    }).finally(function () { ngProgress.complete(); });;
                }
            }
            else {
                $scope.alertSalon = { type: 'danger', msg: 'Моля въведете коректни данни!' };
            }
        }

        $scope.closeSalonAlert = function () {
            $scope.alertSalon = null;
        }

        $scope.openModal = function () {

            var modalInstance = $modal.open({
                templateUrl: 'WorkingTimeModal.html',
                controller: 'WorkingTimeModalController',
                resolve: {
                    items: function () {
                        return $scope.salonForForm.SalonSchedule;
                    }
                }
            });

            modalInstance.result.then(function (schedule) {
                $scope.salonForForm.SalonSchedule = schedule;
                $scope.workTimeText = CommonFactory.getWorkingTimeString($scope.salonForForm.SalonSchedule).join(' ,');
                $scope.redTextWorkingTime = false;
            }, function () {
                console.log('Modal dismissed at: ' + new Date());
            });

        }

        $scope.openEmplTimeModal = function (index) {

            var modalInstance = $modal.open({
                templateUrl: 'EmplWorkingTimeModal.html',
                controller: 'EmplWorkingTimeModalController',
                resolve: {
                    items: function () {
                        return $scope.salonEmpl[index];
                    }
                }
            });

            modalInstance.result.then(function (schedule) {
                $timeout(function () {
                    document.getElementById('btnLoadEmployees').click();
                }, 100);
            }, function () {
                console.log('Modal dismissed at: ' + new Date());
            });

        }


        //салон

        $scope.showSalonForm = false;

        $scope.addNewSalon = function () {
            $scope.showSalonForm = true;
            $scope.salonForForm = {};
            $scope.salonForForm.SalonSchedule = [];
            $scope.salon.SalonID = null;
            $scope.workTimeText = 'Не сте въвели работно време';
            $scope.redTextWorkingTime = true;
        }

        $scope.openEmplModal = function (salonId, insertEmpl, emplId) {

            var modalInstance = $modal.open({
                templateUrl: 'EmplDetailsModal.html',
                controller: 'EmplDetailsModalController',
                size: 'lg',
                resolve: {
                    salonId: function () {
                        return salonId;
                    },
                    insert: function () {
                        return insertEmpl;
                    },
                    emplId: function () {
                        return emplId;
                    }
                }
            });

            modalInstance.result.then(function () {
                $timeout(function () {
                    document.getElementById('btnLoadEmployees').click();
                }, 100);
            }, function () {
                $timeout(function () {
                    document.getElementById('btnLoadEmployees').click();
                }, 100);
                console.log('Modal dismissed at: ' + new Date());
            });
        };

        $scope.openPosModal = function (salonId) {

            var modalInstance = $modal.open({
                templateUrl: 'PositionsModal.html',
                controller: 'PositionsModalController',
                resolve: {
                    salonId: function () {
                        return salonId;
                    }
                }
            });

            modalInstance.result.then(function (count) {
                $scope.positionsCount = $scope.positionsCount + count;
            }, function (count) {
                $scope.positionsCount = $scope.positionsCount + count;
                console.log('Modal dismissed at: ' + new Date());
            });
        };

        $scope.openServicesModal = function (salonId, serviceId) {

            var modalInstance = $modal.open({
                templateUrl: 'ServicesModal.html',
                controller: 'ServicesModalController',
                size: 'lg',
                resolve: {
                    salonId: function () {
                        return salonId;
                    },
                    serviceId: function () {
                        return serviceId;
                    }
                }
            });

            modalInstance.result.then(function () {
                $timeout(function () {
                    document.getElementById('btnLoadSer').click();
                }, 100);
            }, function () {
                $timeout(function () {
                    document.getElementById('btnLoadSer').click();
                }, 100);
                console.log('Modal dismissed at: ' + new Date());
            });
        };

        //$scope.openTypesModal = function () {

        //    var modalInstance = $modal.open({
        //        templateUrl: 'TypesModal.html',
        //        controller: 'TypesModalController'
        //    });
        //};

        //за прикачането на файловете

        $scope.removeFromDb = function (item, index) {
            ngProgress.start();
            uploader.queue.splice(index, 1);

            if (item.IsMain) {
                $scope.MainImage = null;
                $scope.MainImagePath = null;
            }

            var image = { ImageID: item.ImageID, Username: 'da se opravi!', ForDelete: true };

            SalonsService.deleteAnImage(image).then(function (data) {
                $scope.alertSalon = { type: 'success', msg: 'Данните бяха успешно запазени.' };
            }).finally(function () { ngProgress.complete(); });
        };

        $scope.makeMain = function (item, index) {
            ngProgress.start();



            var image = { ImageID: item.ImageID, Username: 'da se opravi!', ForDelete: false };

            SalonsService.deleteAnImage(image).then(function (data) {
                //$scope.MainImagePath = '/Images/SalonImages/' + item.PathFromDb + '?w=300&h=250&mode=max';
                //$scope.MainImage = item;

                //angular.forEach($scope.salonForForm.SalonImages, function (image) {
                //    if (image.IsMain) {
                //        image.IsMain = false;
                //    }
                //});

                //item.IsMain = true;

                SalonsService.getSalonImages($scope.salon.SalonID).then(function (result) {
                    $scope.salonForForm.SalonImages = result;
                    console.log($scope.salonForForm.SalonImages);
                    uploader.queue = [];
                    $scope.MainImagePath = null;
                    $scope.MainImage = null;

                    angular.forEach($scope.salonForForm.SalonImages, function (item) {
                        var dummy = new FileUploader.FileItem(uploader, {
                            lastModifiedDate: item.AddedOn,
                            size: 1e6,
                            type: 'image/jpeg',
                            name: item.ImageName
                        });

                        dummy.progress = 100;
                        dummy.isUploaded = true;
                        dummy.isSuccess = true;
                        dummy.IsMain = item.IsMain;
                        dummy.ImageID = item.ImageID;
                        dummy.ImagePath = '/Images/SalonImages/' + item.ImagePath + '?w=80&h=80&mode=max';
                        dummy.PathFromDb = item.ImagePath;

                        uploader.queue.push(dummy);

                        if (item.IsMain) {
                            $scope.MainImagePath = '/Images/SalonImages/' + item.ImagePath + '?w=300&h=250&mode=max';
                            $scope.MainImage = item;
                        }
                    });
                });

                $scope.alertSalon = { type: 'success', msg: 'Данните бяха успешно запазени.' };
            }).finally(function () { ngProgress.complete(); });
        };

        var uploader = $scope.uploader = new FileUploader({
            url: 'api/SaveImages/' + $scope.salon.SalonID
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
            $scope.FileTypes = true;
        };
        uploader.onAfterAddingFile = function (fileItem) {
            console.info('onAfterAddingFile', fileItem);
        };
        uploader.onAfterAddingAll = function (addedFileItems) {
            console.info('onAfterAddingAll', addedFileItems);
        };
        uploader.onBeforeUploadItem = function (item) {
            item.url = 'api/SaveImages/?id=' + $scope.salon.SalonID + '&main=false&username=' + $scope.Username;
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
            $scope.FileTypes = false;

            SalonsService.getSalonImages($scope.salon.SalonID).then(function (result) {
                $scope.salonForForm.SalonImages = result;
                console.log($scope.salonForForm.SalonImages);
                uploader.queue = [];
                $scope.MainImagePath = null;
                $scope.MainImage = null;

                angular.forEach($scope.salonForForm.SalonImages, function (item) {
                    var dummy = new FileUploader.FileItem(uploader, {
                        lastModifiedDate: item.AddedOn,
                        size: 1e6,
                        type: 'image/jpeg',
                        name: item.ImageName
                    });

                    dummy.progress = 100;
                    dummy.isUploaded = true;
                    dummy.isSuccess = true;
                    dummy.IsMain = item.IsMain;
                    dummy.ImageID = item.ImageID;
                    dummy.ImagePath = '/Images/SalonImages/' + item.ImagePath + '?w=80&h=80&mode=max';
                    dummy.PathFromDb = item.ImagePath;

                    uploader.queue.push(dummy);

                    if (item.IsMain) {
                        $scope.MainImagePath = '/Images/SalonImages/' + item.ImagePath + '?w=300&h=250&mode=max';
                        $scope.MainImage = item;
                    }
                });
            });
        };

        console.info('uploader', uploader);

        //Служители

        $scope.positionsCount = -1;

        $scope.loadSalonEmployees = function (salonId) {
            ngProgress.start();
            $scope.loadingEmplForm = true;
            $scope.alertEmpl = null;
            if (salonId) {
                SalonsService.getSalonEmpl(salonId).then(function (result) {
                    $scope.salonEmpl = result.Employees;
                    $scope.positionsCount = result.PositionsCount;
                    console.log($scope.salonEmpl);
                    if ($scope.salonEmpl.length == 0) {
                        $scope.showEmplForm = false;
                        $scope.noEmplForSalon = true;
                    }
                    else {
                        $scope.showEmplForm = true;
                        $scope.noEmplForSalon = false;

                        angular.forEach($scope.salonEmpl, function (item) {
                            if (item.EmplSchedule.length == 0) {
                                item.workTimeText = 'Не сте въвели работно време';
                                item.redTextWorkingTime = true;
                            }
                            else {
                                item.workTimeText = 'Въвели сте';
                                item.redTextWorkingTime = false;


                                item.workTimeText = CommonFactory.getWorkingTimeString(item.EmplSchedule).join(' ,');
                                item.redTextWorkingTime = false;
                            }
                        });
                    }
                }).finally(function () {
                    $scope.loadingEmplForm = false;
                    ngProgress.complete();
                });
            }
            else {
                $scope.showEmplForm = false;
                $scope.loadingEmplForm = false;
                ngProgress.complete();
                $scope.alertEmpl = { type: 'warning', msg: 'Не сте избрали салон, за да заредите.' };
                $scope.positionsCount = -1;
            }
        }

        $scope.closeEmplAlert = function () {
            $scope.alertEmpl = null;
        }

        $scope.delEmpl = function (id, index) {
            var result = confirm('Сигурни ли сте, че искате да изтриете служителя?');

            if (result) {

                $scope.loadingEmplForm = true;
                ngProgress.start();

                SalonsService.delEmpl(id, $scope.Username).then(function (result) {
                    $scope.salonEmpl.splice(index, 1);
                }).finally(function () {
                    $scope.loadingEmplForm = false;
                    ngProgress.complete();
                });;
            }
        }

        //Услуги

        $scope.typesCount = -1;

        $scope.loadSalonSer = function (salonId) {
            ngProgress.start();
            $scope.loadingSerForm = true;
            $scope.alertSer = null;
            if (salonId) {
                SalonsService.getServicesBySalonId(salonId).then(function (result) {
                    $scope.salonServices = result;
                    console.log($scope.salonServices);
                    if ($scope.salonServices.length == 0) {
                        $scope.showSerForm = false;
                        $scope.noSerForSalon = true;
                    }
                    else {
                        $scope.showSerForm = true;
                        $scope.noSerForSalon = false;

                    }
                }).finally(function () {
                    $scope.loadingSerForm = false;
                    ngProgress.complete();
                });
            }
            else {
                $scope.showSerForm = false;
                $scope.loadingSerForm = false;
                ngProgress.complete();
                $scope.alertSer = { type: 'warning', msg: 'Не сте избрали салон, за да заредите.' };
                $scope.positionsCount = -1;
            }
        }

        $scope.closeSerAlert = function () {
            $scope.alertSer = null;
        }

        $scope.delSer = function (id, index) {
            var result = confirm('Сигурни ли сте, че искате да изтриете услугата?');

            if (result) {

                $scope.loadingSerForm = true;
                ngProgress.start();

                SalonsService.delSer(id, $scope.Username).then(function (result) {
                    $scope.salonServices.splice(index, 1);
                }).finally(function () {
                    $scope.loadingSerForm = false;
                    ngProgress.complete();
                });;
            }
        }
    }
]);
