$(function () {
    $("#ReportsLink").addClass("active");
    $("#StockReportLink").addClass("active");
    $("#StockReportLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //----------------------------------------varible declaration-----------------------------------------//
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;
    const ddlProductType = $('select[name="ddlProductTypeId"]');
    const fromDate = $('input[name="FromDate"]');
    fromDate.val(todayDate);
    const toDate = $('input[name="ToDate"]');
    toDate.val(todayDate);
    const ddlZeroValued = $('select[name="ddlZerovalued"]');
    const ddlProductTypeDetailed = $('select[name="ddlDetailedProductTypeId"]');
    const ddlProduct = $('select[name="ddlProductId"]');
    const fromDateDetailed = $('input[name="DetailedFromDate"]');
    fromDateDetailed.val(todayDate);
    const toDateDetailed = $('input[name="DetaledToDate"]');
    toDateDetailed.val(todayDate);
    const ddlZeroValuedDetailed = $('select[name="ddlDetailedZerovalued"]');
    var PrintData = {};
    //var PrintDataDetailed = {};
    //-----------------------------------stock Report Summerized---------------------------------------//
    GetAllProductTypes();
    function GetAllProductTypes() {
        $.ajax({
            url: "/Reports/GetAllProductTypes",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddlProductType.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlProductType.append(defaultOption);
                    $.each(result.ProductTypes, function (key, item) {
                        var option = $('<option></option>').val(item.ProductTypeId).text(item.Product_Type);
                        ddlProductType.append(option);
                    });
                }
                else {
                    ddlProductType.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlProductType.append(defaultOption);
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    var Data = {};
    $('#btnViewSummerized').on('click', function () {
        $('#loader').show();
        $('.SummerizedStockReportTable').empty();
        if (!ddlProductType.val() || ddlProductType.val() === '--Select Option--') {
            toastr.error('ProductType  Is Required.');
            return;
        }
        else if (!fromDate.val()) {
            toastr.error('FromDate Is Required.');
            return;
        } else if (!toDate.val()) {
            toastr.error('ToDate Type Is Required.');
            return;
        }
        else {
            var requestData = {
                FromDate: fromDate.val(),
                ToDate: toDate.val(),
                ZeroValued: ddlZeroValued.val(),
                ProductTypeId: ddlProductType.val(),
            };
            Data = {
                FromDate: fromDate.val(),
                ToDate: toDate.val(),
                ZeroValued: ddlZeroValued.val(),
                ProductTypeId: ddlProductType.val(),
            }
            $.ajax({
                url: "/Reports/GetSummerizedStockReports",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                data: JSON.stringify(requestData),
                success: function (result) {
                    $('#loader').hide();
                    var html = '';
                    html += '<table id="Summarizedtbl" class="table table-bordered table-hover text-center mt-2 SummerizedStockReportTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th></th>'
                    html += '<th>Product</th>'
                    html += '<th>Opening(+)</th>'
                    html += '<th>Purchase(+)</th>'
                    html += '<th>Purchase Ret.(-)</th>'
                    html += '<th>Production(+)</th>'
                    html += '<th>Production(-)</th>'
                    html += '<th>Sales(-)</th>'
                    html += '<th>Sales Ret(+)</th>'
                    html += '<th>Damage(-)</th>'
                    html += '<th>STO(-)</th>'
                    html += '<th>STI(+)</th>'
                    html += '<th>Closing</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    if (result.ResponseCode == 302) {
                        $.each(result.StockReports, function (key, item) {
                            if (item.unit !== 'SET') {
                                html += '<tr>';
                                html += '<td><button  class="btn btn-primary btn-sm toggleColumnsBtn" id="btn-info-' + item.ProductId + '"  data-id="' + item.ProductId + '" style=" border-radius: 50%;" ><i class="fa-solid fa-circle-info"></i></button></td>'
                                html += '<td>' + item.ProductName + '</td>';
                                html += '<td>' + item.OpeningQty.toFixed(2) + '  ' + item.UnitName + '</td>';
                                html += '<td>' + item.PurchaseQty.toFixed(2) + '</td>';
                                html += '<td>' + item.PurchaseReturnQty.toFixed(2) + '</td>';
                                html += '<td>' + item.ProductionQty.toFixed(2) + '</td>';
                                html += '<td>' + item.ProductionEntryQty.toFixed(2) + '</td>';
                                html += '<td>' + item.SalesQty.toFixed(2) + '</td>';
                                html += '<td>' + item.SalesReturnQty.toFixed(2) + '</td>';
                                html += '<td>' + item.DamageQty.toFixed(2) + '</td>';
                                html += '<td>' + item.OutwardQty.toFixed(2) + '</td>';
                                html += '<td>' + item.InwardQty.toFixed(2) + '</td>';
                                var closing = item.OpeningQty + item.PurchaseQty + item.ProductionQty + item.SalesReturnQty + item.InwardQty - item.PurchaseReturnQty - item.SalesQty - item.DamageQty - item.OutwardQty - item.ProductionEntryQty;
                                if (0 > closing) {
                                    html += '<td class="bg-danger text-white">' + closing.toFixed(2) + ' ' + item.UnitName + '</td>';
                                }
                                else {
                                    html += '<td>' + closing.toFixed(2) + ' ' + item.UnitName + '</td>';
                                }
                                html += '</tr >';
                            }
                        });
                        $('#BtnPrintSummarized').show();
                    }
                    else {
                        html += '<tr>';
                        html += '<td colspan="13">No Record</td>';
                        html += '</tr >';
                    }
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblSummerizedStockList').html(html);
                    if (!$.fn.DataTable.isDataTable('.SummerizedStockReportTable')) {
                        var table = $('.SummerizedStockReportTable').DataTable({
                            "responsive": true, "lengthChange": false, "autoWidth": false, "searching": true,
                           
                        });
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
        }

    })
    $('#BtnPrintSummarized').on('click', function () {
        var queryString = $.param(Data); // Serialize object to query string
        var url = '/Print/StockSumrizedReportPrint?' + queryString; // Append query string to URL
        window.open(url, '_blank');
    });
    $(document).on('click', '.toggleColumnsBtn', (event) => {
        const value = $(event.currentTarget).data('id');
        var requestData = {
            FromDate: fromDate.val(),
            ToDate: toDate.val(),
            ZeroValued: ddlZeroValued.val(),
            ProductTypeId: ddlProductType.val(),
            ProductId: value
        };
        $.ajax({
            url: "/Reports/GetBranchWiseStockInfo",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            data: JSON.stringify(requestData),
            success: function (result) {
                var html = '';
                html += '<table class="table table-bordered table-hover text-center mt-2 SummerizedStockInfoTable" style="width:100%">';
                html += '<thead>'
                html += '<tr>'
                html += '<th>Branch</th>'
                html += '<th>Opening Qty</th>'
                html += '<th>Closing</th>'
                html += '</tr>'
                html += '</thead>'
                html += '<tbody>';
                if (result.ResponseCode == 302) {
                    var totalClosing = 0;
                    $.each(result.StockInfos, function (key, item) {
                        html += '<tr>';
                        html += '<td>' + item.BranchName + '</td>';
                        html += '<td>' + item.OpeningStock.toFixed(2) + '</td>';
                        let closingStock = item.OpeningStock + item.RunningStock;
                        totalClosing += closingStock;
                        html += '<td>' + closingStock.toFixed(2) + '</td>';
                        html += '</tr >';
                    });
                    html += '<tr>';
                    html += '<td colspan=2> Total Closing</td>';
                    html += '<td >' + totalClosing.toFixed(2) +' </td>';
                    html +='</tr> ';
                }
                else {
                    html += '<tr>';
                    html += '<td colspan="3">No Record</td>';
                    html += '</tr >';
                }
                html += ' </tbody>';
                html += '</table >';
                $('.tblSummerizedInfo').html(html);
            },
            error: function (errormessage) {
                Swal.fire(
                    'Error!',
                    'An error occurred',
                    'error'
                );
            }
        });
        $('#modal-stock-info').modal('show');
    });
    //-----------------------------------stock Report Detailed-------------------------------------------//
    GetAllProductTypesForDetailed()
    function GetAllProductTypesForDetailed() {
        $.ajax({
            url: "/Reports/GetAllProductTypes",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddlProductTypeDetailed.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlProductTypeDetailed.append(defaultOption);
                    $.each(result.ProductTypes, function (key, item) {
                        var option = $('<option></option>').val(item.ProductTypeId).text(item.Product_Type);
                        ddlProductTypeDetailed.append(option);
                    });
                }
                else {
                    ddlProductTypeDetailed.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlProductTypeDetailed.append(defaultOption);
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    $("#ProductType").on("change", function () {
        $("#ProductId").prop("disabled", false);
        $("#ProductId").empty();
        var ProductTypeId = $(this).val();
        GetProductByTypeId(ProductTypeId);
    });
    function GetProductByTypeId(id) {
        $.ajax({
            url: '/Reports/GetProductByTypeId?ProductTypeId=' + id + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddlProduct.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlProduct.append(defaultOption);
                    $.each(result.Products, function (key, item) {
                        var option = $('<option></option>').val(item.ProductId).text(item.ProductName);
                        ddlProduct.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    var requestData = {};
    $('#btnViewDetailed').on('click', function () {
        $('#loader').show();
        $('.DetailedStockReportTable').empty();
        if (!ddlProductTypeDetailed.val() || ddlProductTypeDetailed.val() === '--Select Option--') {
            toastr.error('ProductType  Is Required.');
            return;
        } else if (!ddlProduct.val() || ddlProduct.val() === '--Select Option--') {
            toastr.error('Product Name  Is Required.');
            return;
        }
        else if (!fromDateDetailed.val()) {
            toastr.error('FromDate Is Required.');
            return;
        } else if (!toDateDetailed.val()) {
            toastr.error('ToDate Type Is Required.');
            return;
        }
        else {
             requestData = {
                FromDate: fromDateDetailed.val(),
                ToDate: toDateDetailed.val(),
                ZeroValued: ddlZeroValuedDetailed.val(),
                ProductTypeId: ddlProductTypeDetailed.val(),
                ProductId: ddlProduct.val()
            };
            $.ajax({
                url: "/Reports/GetDetailedStockReport",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                data: JSON.stringify(requestData),
                success: function (result) {
                    $('#loader').hide();
                    var html = '';
                    html += '<table class="table table-bordered table-hover text-center mt-2 DetailedStockReportTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th>Date</th>'
                    html += '<th>Trxn No</th>'
                    html += '<th>Particular</th>'
                    html += '<th>Inward(+)</th>'
                    html += '<th>Outward(-)</th>'
                    html += '<th>Stock</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    if (result.ResponseCode == 302) {
                        $.each(result.DetailedStock, function (key, item) {
                            var Stock = 0;
                            html += '<tr>';
                            html += '<td colspan="6"  class="bg-primary">' + item.BranchName + '</td>';
                            html += '</tr >';
                            html += '<tr>';
                            html += '<td colspan="5">Opening Qty</td>';
                            if (0 > item.OpeningQty) {
                                html += '<td class="bg-danger text-white">' + item.OpeningQty + '</td>';
                            }
                            else {
                                html += '<td>' + item.OpeningQty + '</td>';
                            }
                            html += '</tr >';
                            Stock = item.OpeningQty;
                            var totalinward = 0;
                            var totaloutward = 0;
                            $.each(item.Stocks, function (key, item2) {
                                const ModifyDate = item2.TransactionDate;
                                var formattedDate = '';
                                if (ModifyDate) {
                                    const dateObject = new Date(ModifyDate);
                                    if (!isNaN(dateObject)) {
                                        const day = String(dateObject.getDate()).padStart(2, '0');
                                        const month = String(dateObject.getMonth() + 1).padStart(2, '0');
                                        const year = dateObject.getFullYear();
                                        formattedDate = `${day}/${month}/${year}`;
                                    }
                                }
                                html += '<tr>';
                                html += '<td>' + formattedDate + '</td>';
                                html += '<td>' + item2.TransactionNo + '</td>';
                                html += '<td>' + item2.Particular + '</td>';
                                if (item2.IncrementStock === true) {
                                    totalinward += item2.Quantity;
                                    html += '<td>' + item2.Quantity.toFixed(2) + '</td>';
                                    html += '<td >-</td>';
                                } else {
                                    totaloutward += item2.Quantity;
                                    html += '<td >-</td>';
                                    html += '<td>' + item2.Quantity.toFixed(2) + '</td>';
                                    
                                }
                                //html += item2.IncrementStock === true ? '<td>' + item2.Quantity.toFixed(2) + '</td>' : '<td>-</td>';
                                //html += item2.IncrementStock === false ? '<td>' + item2.Quantity.toFixed(2) + '</td>' : '<td>-</td>';
                                Stock += item2.IncrementStock === true ? item2.Quantity : -item2.Quantity;
                                if (0 > Stock) {
                                    html += '<td class="bg-danger text-white">' + Stock.toFixed(2) + '</td>';
                                }
                                else {
                                    html += '<td>' + Stock.toFixed(2) + '</td>';
                                }
                                html += '</tr >';
                            });
                            html += '<tr>';
                            html += '<td colspan="3">Totals</td>';
                            html += '<td>' + totalinward.toFixed(2) + '</td>';
                            html += '<td>' + totaloutward.toFixed(2) + '</td>';
                            html += '<td >-</td>';
                            html += '</tr >';
                        });
                        $('#BtnPrintDetailed').show();
                        PrintDataDetailed = {
                            FromDate: fromDateDetailed.val(),
                            ToDate: toDateDetailed.val(),
                            Stocks: result.DetailedStock
                        }
                    }
                    else {
                        html += '<tr>';
                        html += '<td colspan="6">No Record</td>';
                        html += '</tr >';
                    }
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblDetailedStockList').html(html);
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
        }
    })
    $('#BtnPrintDetailed').on('click', function () {
        console.log(requestData);
        var queryString = $.param(requestData); // Serialize object to query string
        var url = '/Print/StockDetailedReportPrint?' + queryString; // Append query string to URL
        window.open(url, '_blank');
    });
});