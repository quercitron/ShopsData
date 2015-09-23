var CurrentDataController = function ($scope, $routeParams, CurrentDataService, GeneralDataService) {
    $scope.locationId = $routeParams.locationId;
    $scope.productTypeId = $routeParams.productTypeId;

    CurrentDataService
        .getDataSummary($routeParams.locationId, $routeParams.productTypeId)
        .success(function (data) {
            $scope.products = data;
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
}

CurrentDataController.$inject = ['$scope', '$routeParams', 'CurrentDataService', 'GeneralDataService'];