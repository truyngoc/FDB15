// module
var FdbApp = angular.module('FdbApp', []);

// controller
FdbApp.controller('TemplateModelController', function ($scope, TemplateModelService) {

    getTemplateModels();

    function getTemplateModels() {
        TemplateModelService.getTemplateModels()
            .success(function (m) {
                $scope.models = m;
                //console.log($scope.models);
            })
            .error(function (err) {
                $scope.status = 'Unable to load TemplateModel data:' + err.message;
                console.log($scope.status);
            });
    }
});

// another to write controller (khong can tao AngularJS service)
FdbApp.controller('FuckController', function ($scope, $http) {

    // get list
    $scope._load = function () {
        $http.get('/TemplateModel/GetTemplateModels')
            .success(function (data, status, headers, config) {
                $scope.models = data;
            })
            .error(function (data, status, headers, config) {
                // log error here
            });
    }


    // goi ham nay khi load trang lan dau
    $scope._load();
    

    // delete (dung POST)
    $scope._delete = function (data) {
        var state = confirm('Do you want to delete this record');

        if (state == true) {
            $http.post('/TemplateModel/DeleteTemplateModel',data)                
                .success(function (status) {                    
                    console.log(status);
                    $scope._load();
                })
                .error(function (status) {
                    // log error here
                });
        }
    };

    
});


// factory service (call action method of controller)
FdbApp.factory('TemplateModelService', ['$http', function ($http) {
    var TemplateModelService = {};

    TemplateModelService.getTemplateModels = function () {
        return $http.get('/TemplateModel/GetTemplateModels');
    };

    return TemplateModelService
}]);