var ProductDetailsController = function ($scope, $routeParams, $location, ProductDetailsService) {
    $scope.locationId = $routeParams.locationId;
    $scope.productId = $routeParams.productId;

    ProductDetailsService
        .getDetails($routeParams.locationId, $routeParams.productId)
        .success(function (data) {
            $scope.details = data;

            var plotData = [[new Date('2008-05-07'), null, 60], [new Date('2008-05-08'), 70, 65], [new Date('2008-05-10'), 80, 70], [new Date('2008-05-11'), 60, 65], [new Date('2008-05-11'), null, 65], [new Date('2008-05-14'), null, 70]];

            for (var i = 0; i < data.Prices.PlotData.length; i++) {
                data.Prices.PlotData[i][0] = new Date(data.Prices.PlotData[i][0]);
            }

            var g = new Dygraph(

                // containing div
                document.getElementById("graphdiv"),

                // CSV or path to a CSV file.
                data.Prices.PlotData,
                {
                    labels: data.Prices.Labels,
                    xlabel: 'Date',
                    ylabel: 'Price',
                    rollPeriod: 0,
                    showRoller: false,
                    legend: 'always',
                    //labelsSeparateLines: true,
                    drawPoints: true,
                    //drawAxesAtZero: true,
                    axes:
                    {
                        x:
                        {
                            drawAxis: true,
                            drawGrid: true
                        },
                        y:
                        {
                            drawGrid: true,
                            independentTicks: true
                        }
                    },
                    connectSeparatedPoints: true
                }
            );
        });
}

ProductDetailsController.$inject = ['$scope', '$routeParams', '$location', 'ProductDetailsService'];