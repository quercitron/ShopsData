var CurrentDataController = function ($scope, $routeParams, $location, CurrentDataService, GeneralDataService, $window) {
    $scope.locationId = $routeParams.locationId;
    $scope.productTypeId = $routeParams.productTypeId;

    $scope.updatePath = function () {
        //$window.alert($scope.locationId);
        //$window.alert($scope.productTypeId);
        $location.path('CurrentData/' + $scope.locationId + '/' + $scope.productTypeId);
    };

    CurrentDataService
        .getDataSummaryGrouped($routeParams.locationId, $routeParams.productTypeId)
        .success(function (data) {
            $scope.productGroups = data;
            //$window.alert(data.length);
        });

    GeneralDataService
        .getDataSources()
        .success(function (data) {
            $scope.dataSources = data;
        });

    GeneralDataService
        .getProductTypes()
        .success(function (data) {
            $scope.productTypes = data;
        });

    GeneralDataService
        .getLocations()
        .success(function (data) {
            $scope.locations = data;
        });
}

CurrentDataController.$inject = ['$scope', '$routeParams', '$location', 'CurrentDataService', 'GeneralDataService', '$window'];