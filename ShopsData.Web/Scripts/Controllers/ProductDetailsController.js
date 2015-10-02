var ProductDetailsController = function ($scope, $routeParams, $location, ProductDetailsService) {
    $scope.locationId = $routeParams.locationId;
    $scope.productId = $routeParams.productId;

    ProductDetailsService
        .getDetails($routeParams.locationId, $routeParams.productId)
        .success(function (data) {
            $scope.details = data;
        });

    var plotData = [[new Date('2008-05-07'), null, 60], [new Date('2008-05-08'), 70, 65], [new Date('2008-05-10'), 80, 70], [new Date('2008-05-11'), 60, 65], [new Date('2008-05-11'), null, 65], [new Date('2008-05-14'), null, 70]];

    var g = new Dygraph(

        // containing div
        document.getElementById("graphdiv"),

        // CSV or path to a CSV file.
        plotData,
		{
		    labels: ["Date", "Source1", "Source2"],
		    xlabel: 'Date',
		    ylabel: 'Price',
		    rollPeriod: 0,
		    showRoller: false,
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
}

ProductDetailsController.$inject = ['$scope', '$routeParams', '$location', 'ProductDetailsService'];