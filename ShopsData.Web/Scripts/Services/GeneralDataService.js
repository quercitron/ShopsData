var GeneralDataService = function ($http) {
    return {
        getDataSources: function () {
            return $http({
                method: 'GET',
                url: 'api/dataSource'
            });
        },
        getProductTypes: function () {
            return $http({
                method: 'GET',
                url: 'api/productType'
            });
        },
        getLocations: function () {
            return $http({
                method: 'GET',
                url: 'api/location'
            });
        },
        markProduct: function(productId, isMarked) {
            $http({
                method: 'POST',
                url: 'api/userProduct',
                data: { productId: productId, isMarked: isMarked }
            });
        }
    };
}

GeneralDataService.$inject = ['$http'];