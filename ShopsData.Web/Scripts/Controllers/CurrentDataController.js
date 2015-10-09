var CurrentDataController = function ($scope, $routeParams, $location, CurrentDataService, GeneralDataService, $window) {
    $scope.locationId = $routeParams.locationId;
    $scope.productTypeId = $routeParams.productTypeId;

    $scope.updatePath = function () {
        //$window.alert($scope.locationId);
        //$window.alert($scope.productTypeId);
        $location.path('CurrentData/' + $scope.locationId + '/' + $scope.productTypeId);
    };

    $scope.mark = function(productGroup) {
        GeneralDataService.markProduct(productGroup.ProductId, productGroup.IsMarked);
    };

    $scope.predicate = '';
    $scope.reverse = false;
    $scope.order = function (predicate) {
        if ($scope.predicate === predicate && $scope.reverse) {
            $scope.predicate = '';
            $scope.reverse = false;
            return;
        }
        $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
        $scope.predicate = predicate;
    };

    $scope.minPrice = null;
    $scope.maxPrice = null;
    $scope.priceFilter = function (product) {
        if ($scope.minPrice && product.MinPrice < $scope.minPrice) {
            return false;
        }
        if ($scope.maxPrice && product.MinPrice > $scope.maxPrice) {
            return false;
        }
        return true;
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