$(function () {
        $.ajax({
            url: "/Reports/GetGraphData",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    //Starting 
                    var areaChartData = {
                        labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'Octber', 'November', 'December'],
                        datasets: [
                            {
                                label: 'Sales',
                                backgroundColor: '#fc030f',
                                borderColor: '#09daed',
                                pointRadius: false,
                                pointColor: '#3b8bba',
                                pointStrokeColor: 'rgba(60,141,188,1)',
                                pointHighlightFill: '#fff',
                                pointHighlightStroke: 'rgba(60,141,188,1)',
                                data: result.GraphData.SalesAmount,
                                labelFontColor: 'white'
                            },
                            {
                                label: 'Purchases',
                                backgroundColor: '#18cf0e',
                                borderColor: 'rgba(210, 214, 222, 1)',
                                pointRadius: false,
                                pointColor: 'rgba(210, 214, 222, 1)',
                                pointStrokeColor: '#c1c7d1',
                                pointHighlightFill: '#fff',
                                pointHighlightStroke: 'rgba(220,220,220,1)',
                                data: result.GraphData.PurchaseAmount,
                                labelFontColor: 'white'
                            },
                            {
                                label: 'Productions',
                                backgroundColor: '#f5d94e',
                                borderColor: 'rgba(210, 214, 222, 1)',
                                pointRadius: false,
                                pointColor: 'rgba(210, 214, 222, 1)',
                                pointStrokeColor: '#c1c7d1',
                                pointHighlightFill: '#fff',
                                pointHighlightStroke: 'rgba(220,220,220,1)',
                                data: result.GraphData.ProductionAmount,
                                labelFontColor: 'white'
                            },
                            
                        ]
                    }
                    //-------------
                    //- BAR CHART -
                    //-------------
                    var barChartCanvas = $('#barChart').get(0).getContext('2d')
                    var barChartData = $.extend(true, {}, areaChartData)
                    var temp0 = areaChartData.datasets[0]
                    var temp1 = areaChartData.datasets[1]
                    barChartData.datasets[0] = temp1
                    barChartData.datasets[1] = temp0
                    var barChartOptions = {
                        responsive: true,
                        maintainAspectRatio: false,
                        datasetFill: false,
                        scales: {
                            xAxes: [{
                                ticks: {
                                    fontColor: 'white' 
                                }
                            }],
                            yAxes: [{
                                ticks: {
                                    fontColor: 'white' 
                                }
                            }]
                        }
                    }
                    new Chart(barChartCanvas, {
                        type: 'bar',
                        data: barChartData,
                        options: barChartOptions
                    })
                    //Ending
                    //secod chart
                    var stackedBarChartCanvas = $('#stackedBarChart').get(0).getContext('2d');
                    var stackedBarChartData = {
                        labels: areaChartData.labels,
                        datasets: [
                            {
                                label: 'Sales',
                                backgroundColor: '#fc030f',
                                borderColor: '#09daed',
                                pointRadius: false,
                                pointColor: '#3b8bba',
                                pointStrokeColor: 'rgba(60,141,188,1)',
                                pointHighlightFill: '#fff',
                                pointHighlightStroke: 'rgba(60,141,188,1)',
                                data: result.GraphData.SalesAmount,
                                labelFontColor: 'white'
                            },
                            {
                                label: 'Received Amount',
                                backgroundColor: '#18cf0e',
                                borderColor: 'rgba(210, 214, 222, 1)',
                                pointRadius: false,
                                pointColor: 'rgba(210, 214, 222, 1)',
                                pointStrokeColor: '#c1c7d1',
                                pointHighlightFill: '#fff',
                                pointHighlightStroke: 'rgba(220,220,220,1)',
                                data: result.GraphData.ReceivedAmount,
                                labelFontColor: 'white'
                            }
                        ]
                    };
                    var stackedBarChartOptions = {
                        responsive: true,
                        maintainAspectRatio: false,
                        datasetFill: false,
                        scales: {
                            xAxes: [{
                                ticks: {
                                    fontColor: 'white'
                                }
                            }],
                            yAxes: [{
                                ticks: {
                                    fontColor: 'white'
                                }
                            }]
                        }
                    };

                    new Chart(stackedBarChartCanvas, {
                        type: 'bar',
                        data: stackedBarChartData,
                        options: stackedBarChartOptions
                    });
                
                    //Ending 
                }
            },
            error: function (errormessage) {
                $('#loader').hide();
                Swal.fire(
                    'Error!',
                    'An error occurred',
                    'error'
                );
            }
        });
})