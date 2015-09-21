var CurrentDataController = function ($scope, $routeParams, CurrentDataService) {
    $scope.locationId = $routeParams.locationId;
    $scope.productTypeId = $routeParams.productTypeId;
    CurrentDataService
        .getDataSummary($routeParams.locationId, $routeParams.productTypeId)
        .success(function(data) {
            $scope.products = data;
        });
}

CurrentDataController.$inject = ['$scope', '$routeParams', 'CurrentDataService'];