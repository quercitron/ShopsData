var ProductDetailsService = function ($http) {
    return {
        getDetails: function (locationId, productId) {
            return $http({
                method: 'GET',
                url: 'api/ProductDetails/' + locationId + '/' + productId
            });
        }
    };
}

ProductDetailsService.$inject = ['$http'];