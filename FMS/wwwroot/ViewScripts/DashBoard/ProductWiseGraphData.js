$(function () {
    $("#ReportsLink").addClass("active");
    $("#GrapdataProductWiseLink").addClass("active");
    $("#GrapdataProductWiseLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    const ddlFinishedGood = $('select[name="ddlFinishedGoodId"]');
    GetFinishedGoods();
    
    function GetFinishedGoods() {
        $.ajax({
            url: "/Admin/GetFinishedGoods",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddlFinishedGood.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlFinishedGood.append(defaultOption);
                    $.each(result.Products, function (key, item) {
                        var option = $('<option></option>').val(item.ProductId).text(item.ProductName);
                        ddlFinishedGood.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    var barChartInstance;

    ddlFinishedGood.on('change', function () {
        var selectElement = $(this);
        requestData = {
            ProductId: selectElement.val()
        };

        if (barChartInstance) {
            barChartInstance.destroy();
        }

        $.ajax({
            url: "/Reports/GetGrapDataProductWiseReport",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            data: JSON.stringify(requestData),
            success: function (result) {
                if (result.ResponseCode == 302) {
                    // Starting Chart Rendering
                    var areaChartData = {
                        labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
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
                            }
                        ]
                    };

                    //-------------
                    //- BAR CHART -
                    //-------------
                    var barChartCanvas = $('#barChart').get(0).getContext('2d');
                    var barChartData = $.extend(true, {}, areaChartData);
                    var temp0 = areaChartData.datasets[0];
                    var temp1 = areaChartData.datasets[1];
                    barChartData.datasets[0] = temp1;
                    barChartData.datasets[1] = temp0;
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
                    };

                    // Create new chart instance
                    barChartInstance = new Chart(barChartCanvas, {
                        type: 'bar',
                        data: barChartData,
                        options: barChartOptions
                    });
                    // Ending Chart Rendering
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
    });
    

    

});