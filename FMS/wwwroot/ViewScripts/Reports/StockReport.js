$(function () {
    //default date
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;

    $("#ReportsLink").addClass("active");
    $("#StockReportLink").addClass("active");
    $("#StockReportLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //----------------------------------------varible declaration-----------------------------------------//
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
    //-----------------------------------stock Report Summerized---------------------------------------//
    GetAllProductTypes();
    function GetAllProductTypes() {
        $.ajax({
            url: "/Transaction/GetAllProductTypes",
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
            $.ajax({
                url: "/Reports/GetSummerizedStockReports",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                data: JSON.stringify(requestData),
                success: function (result) {
                    $('#loader').hide();
                    var html = '';
                    html += '<table class="table table-bordered table-hover text-center mt-2 SummerizedStockReportTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
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
                            html += '<tr>';
                            html += '<td>' + item.ProductName + '</td>';
                            html += '<td>' + item.OpeningQty + '</td>';
                            html += '<td>' + item.PurchaseQty + '</td>';
                            html += '<td>' + item.PurchaseReturnQty + '</td>';
                            html += '<td>' + item.ProductionQty + '</td>';
                            html += '<td>' + item.ProductionEntryQty + '</td>';
                            html += '<td>' + item.SalesQty + '</td>';
                            html += '<td>' + item.SalesReturnQty + '</td>';
                            html += '<td>' + item.DamageQty + '</td>';
                            html += '<td>' + item.OutwardQty + '</td>';
                            html += '<td>' + item.InwardQty + '</td>';
                            var closing = item.OpeningQty + item.PurchaseQty + item.ProductionQty + item.SalesReturnQty + item.InwardQty - item.PurchaseReturnQty - item.SalesQty - item.DamageQty - item.OutwardQty - item.ProductionEntryQty;
                            html += '<td>' + closing + '</td>';
                            html += '</tr >';
                        });
                    }
                    else {
                        html += '<tr>';
                        html += '<td colspan="11">No Record</td>';
                        html += '</tr >';
                    }
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblSummerizedStockList').html(html);
                    if (!$.fn.DataTable.isDataTable('.SummerizedStockReportTable')) {
                        var table = $('.SummerizedStockReportTable').DataTable({
                            "responsive": true, "lengthChange": false, "autoWidth": false,
                            "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"]
                        }).buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');
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
    //-----------------------------------stock Report Detailed-------------------------------------------//
    GetAllProductTypesForDetailed()
    function GetAllProductTypesForDetailed() {
        $.ajax({
            url: "/Transaction/GetAllProductTypes",
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
                    $.each(result.products, function (key, item) {
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
            var requestData = {
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
                    html += '<th>Product Name</th>'
                    html += '<th>Trxn N0</th>'
                    html += '<th>Particular</th>'
                    html += '<th>Inward(+)</th>'
                    html += '<th>Outward(-)</th>'
                    html += '<th>Closing Stock</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    if (result.ResponseCode == 302) {
                        let item = result.product
                        var Stock = 0;
                            html += '<tr>';
                            html += '<td></td>';
                            html += '<td></td>';
                            html += '<td></td>';
                            html += '<td></td>';
                            html += '<td></td>';
                            html += '<td></td>';
                            html += '<td>' + item.OpeningQty + '</td>';
                            html += '</tr >';
                            Stock = item.OpeningQty;
                            if (item.ProductionEntries !== null) {
                                $.each(item.ProductionEntries, function (key, Production) {
                                    html += '<tr>';
                                    html += '<td>' + Production.ProductionDate + '</td>';
                                    html += '<td>' + item.ProductName + '</td>';
                                    html += '<td>' + Production.ProductionNo + '</td>';
                                    html += '<td>Production</td>';
                                    html += '<td>' + Production.Quantity + '</td>';
                                    html += '<td></td>';
                                    Stock += Production.Quantity
                                    html += '<td>' + Stock + '</td>';
                                    html += '</tr >';
                                });
                            }
                            if (item.ProductionEntryTransactions != null) {
                                $.each(item.ProductionEntryTransactions, function (key, Pet) {
                                    html += '<tr>';
                                    html += '<td>' + Pet.TransactionDate + '</td>';
                                    html += '<td>' + item.ProductName + '</td>';
                                    html += '<td>' + Pet.TransactionNo + '</td>';
                                    html += '<td>Raw Material Used For Production</td>';
                                    html += '<td></td>';
                                    html += '<td>' + Pet.Quantity + '</td>';
                                    Stock -= Pet.Quantity
                                    html += '<td>' + Stock + '</td>';
                                    html += '</tr >';
                                });
                            }
                            if (item.DamageTransactions != null) {
                                $.each(item.DamageTransactions, function (key, Damage) {
                                    html += '<tr>';
                                    html += '<td>' + Damage.TransactionDate + '</td>';
                                    html += '<td>' + item.ProductName + '</td>';
                                    html += '<td>' + Damage.TransactionNo + '</td>';
                                    html += '<td>Damage</td>';
                                    html += '<td></td>';
                                    html += '<td>' + Damage.Quantity + '</td>';
                                    Stock -= Damage.Quantity
                                    html += '<td>' + Stock + '</td>';
                                    html += '</tr >';
                                });
                            }
                            if (item.SalesTransactions != null) {
                                $.each(item.SalesTransactions, function (key, Sales) {
                                    html += '<tr>';
                                    html += '<td>' + Sales.TransactionDate + '</td>';
                                    html += '<td>' + item.ProductName + '</td>';
                                    html += '<td>' + Sales.TransactionNo + '</td>';
                                    html += '<td>Sales</td>';
                                    html += '<td></td>';
                                    html += '<td>' + Sales.Quantity + '</td>';
                                    Stock -= Sales.Quantity
                                    html += '<td>' + Stock + '</td>';
                                    html += '</tr >';
                                });
                            }
                            if (item.SalesReturnTransactions != null) {
                                $.each(item.SalesReturnTransactions, function (key, SalesReturn) {
                                    html += '<tr>';
                                    html += '<td>' + SalesReturn.TransactionDate + '</td>';
                                    html += '<td>' + item.ProductName + '</td>';
                                    html += '<td>' + SalesReturn.TransactionNo + '</td>';
                                    html += '<td>Sales Return</td>';
                                    html += '<td>' + SalesReturn.Quantity + '</td>';
                                    html += '<td></td>';
                                    Stock += SalesReturn.Quantity
                                    html += '<td>' + Stock + '</td>';
                                    html += '</tr >';
                                });
                            }
                            if (item.PurchaseTransactions != null) {
                                $.each(item.PurchaseTransactions, function (key, Purchase) {
                                    html += '<tr>';
                                    html += '<td>' + Purchase.TransactionDate + '</td>';
                                    html += '<td>' + item.ProductName + '</td>';
                                    html += '<td>' + Purchase.TransactionNo + '</td>';
                                    html += '<td>Purchase</td>';
                                    html += '<td>' + Purchase.Quantity + '</td>';
                                    html += '<td></td>';
                                    Stock += Purchase.Quantity
                                    html += '<td>' + Stock + '</td>';
                                    html += '</tr >';
                                });
                            }
                            if (item.PurchaseReturnTransactions != null) {
                                $.each(item.PurchaseReturnTransactions, function (key, PurchaseReturn) {
                                    html += '<tr>';
                                    html += '<td>' + PurchaseReturn.TransactionDate + '</td>';
                                    html += '<td>' + item.ProductName + '</td>';
                                    html += '<td>' + PurchaseReturn.TransactionNo + '</td>';
                                    html += '<td>Purchase Return</td>';
                                    html += '<td></td>';
                                    html += '<td>' + PurchaseReturn.Quantity + '</td>';
                                    Stock -= PurchaseReturn.Quantity
                                    html += '<td>' + Stock + '</td>';
                                    html += '</tr >';
                                });
                            }
                            if (item.InwardSupplyTransactions != null) {
                                $.each(item.InwardSupplyTransactions, function (key, Inward) {
                                    html += '<tr>';
                                    html += '<td>' + Inward.TransactionDate + '</td>';
                                    html += '<td>' + item.ProductName + '</td>';
                                    html += '<td>' + Inward.TransactionNo + '</td>';
                                    html += '<td>Inward Supply</td>';
                                    html += '<td>' + Inward.Quantity + '</td>';
                                    html += '<td></td>';
                                    Stock += Inward.Quantity
                                    html += '<td>' + Stock + '</td>';
                                    html += '</tr >';
                                });
                            }
                            if (item.OutwardSupplyTransactions != null) {
                                $.each(item.OutwardSupplyTransactions, function (key, Outward) {
                                    html += '<tr>';
                                    html += '<td>' + Outward.TransactionDate + '</td>';
                                    html += '<td>' + item.ProductName + '</td>';
                                    html += '<td>' + Outward.TransactionNo + '</td>';
                                    html += '<td>Outward Supply</td>';
                                    html += '<td></td>';
                                    html += '<td>' + Outward.Quantity + '</td>';
                                    Stock -= Outward.Quantity
                                    html += '<td>' + Stock + '</td>';
                                    html += '</tr >';
                                });
                            }
                    }
                    else {
                        html += '<tr>';
                        html += '<td colspan="7">No Record</td>';
                        html += '</tr >';
                    }
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblDetailedStockList').html(html);
                    if (!$.fn.DataTable.isDataTable('.DetailedStockReportTable')) {
                        var table = $('.DetailedStockReportTable').DataTable({
                            "responsive": true, "lengthChange": false, "autoWidth": false,
                            "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"]
                        }).buttons().container().appendTo('#example2_wrapper .col-md-6:eq(0)');
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
});