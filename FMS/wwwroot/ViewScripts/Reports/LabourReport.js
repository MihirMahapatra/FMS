$(function () {
    //default date
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;

    $("#ReportsLink").addClass("active");
    $("#LabourReportLink").addClass("active");
    $("#LabourReportLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //----------------------------------------varible declaration-----------------------------------------//
    const fromDate = $('input[name="FromDate"]');
    fromDate.val(todayDate);
    const toDate = $('input[name="ToDate"]');
    toDate.val(todayDate);
    const fromDateDetailed = $('input[name="FromDateDetailed"]');
    fromDateDetailed.val(todayDate);
    const toDateDetailed = $('input[name="ToDateDetailed"]');
    toDateDetailed.val(todayDate);
    const ddlLabour = $('select[name="ddlLabour"]');
    const ddlSummerizedLabourTypes = $('select[name="ddlSummerizedLabourTypes"]');
    const ddlDetailedLabourTypes = $('select[name="ddlDetailedLabourTypes"]');
    var PrintDataSummarized = {};
    var PrintData = {};
    //-----------------------------------------------------Stock Report Scren --------------------------------------------------//
    function getPrintHeaderFooter() {
        var headerFooter =
            '<div style="font-size: 12px; color: red; font-weight: bold;">Bhuasuni Precast</div>' +
            '<div style="font-size: 10px; color: blue; text-align: center;">Address | Phone: 123-456-7890 | Email: info@example.com</div>' +
            '<div style="font-size: 10px; text-align: right;">Printed on: ' + new Date().toLocaleString() + '</div>';
        return headerFooter;
    }
    GetLabourTypes();
    function GetLabourTypes() {
        $.ajax({
            url: "/Reports/GetLabourTypes",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddlSummerizedLabourTypes.empty();
                    ddlDetailedLabourTypes.empty();
                    var defaultSummerizedOption = $('<option></option>').val('').text('--Select Option--');
                    var defaultDetailedOption = $('<option></option>').val('').text('--Select Option--');
                    ddlSummerizedLabourTypes.append(defaultSummerizedOption);
                    ddlDetailedLabourTypes.append(defaultDetailedOption);
                    $.each(result.LabourTypes, function (key, item) {
                        var optionSummerized = $('<option></option>').val(item.LabourTypeId).text(item.Labour_Type);
                        var optionDetailed = $('<option></option>').val(item.LabourTypeId).text(item.Labour_Type);
                        ddlSummerizedLabourTypes.append(optionSummerized);
                        ddlDetailedLabourTypes.append(optionDetailed);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    $('#btnViewSummerized').on('click', function () {
        $('#loader').show();
        $('.SummerizedLabourReportTable').empty();
        if (!ddlSummerizedLabourTypes.val() || ddlSummerizedLabourTypes.val() === '--Select Option--') {
            toastr.error('Labour Type Is Required.');
            productTypeId.focus();
            return;
        }
        if (!fromDate.val()) {
            toastr.error('FromDate Is Required.');
            return;
        } else if (!toDate.val()) {
            toastr.error('ToDate Is Required.');
            return;
        }
        else {
            var requestData = {
                FromDate: fromDate.val(),
                ToDate: toDate.val(),
                LabourTypeId: ddlSummerizedLabourTypes.val()
            };
            $.ajax({
                url: "/Reports/GetSummerizedLabourReport",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                data: JSON.stringify(requestData),
                success: function (result) {
                    $('#loader').hide();
                    var html = '';
                    html += '<table class="table table-bordered table-hover text-center mt-2 SummerizedLabourReportTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th>Labour Name</th>'
                    html += '<th>Opening Bal</th>'
                    html += '<th>Balance Type</th>'
                    html += '<th>Billing Amt</th>'
                    html += '<th>Damage Amt(-)</th>'
                    html += '<th>Payment Amt(-)</th>'
                    html += '<th>Balance</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    if (result.ResponseCode == 302) {
                        $('#BtnPrintSummarized').show();
                        $.each(result.LaborReports, function (key, item) {
                            html += '<tr>';
                            html += '<td>' + item.LabourName + '</td>';
                            html += '<td>' + item.OpeningBal + '</td>';
                            html += '<td>' + item.OpeningBalType + '</td>';
                            html += '<td>' + item.BillingAmt + '</td>';
                            html += '<td>' + item.DamageAmt + '</td>';
                            html += '<td>' + item.PaymentAmt + '</td>';
                            var Balance = item.OpeningBal + (item.BillingAmt - item.DamageAmt - item.PaymentAmt);
                            html += '<td>' + Balance + '</td>';
                            html += '</tr >';
                        });
                        PrintDataSummarized = {
                            FromDate: fromDate.val(),
                            ToDate: toDate.val(),
                            LaborReports: result.LaborReports
                        }
                    }
                    else {
                        html += '<tr>';
                        html += '<td colspan="7">No Record</td>';
                        html += '</tr >';
                    }
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblSummerizedLabourList').html(html);
                    if (!$.fn.DataTable.isDataTable('.SummerizedLabourReportTable')) {
                        var table = $('.SummerizedLabourReportTable').DataTable({
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
    $('#BtnPrintSummarized').on('click', function () {
        console.log(PrintDataSummarized);
        $.ajax({
            type: "POST",
            url: '/Print/LabourSummarizedPrintData',
            dataType: 'json',
            data: JSON.stringify(PrintDataSummarized),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                window.open(Response.redirectTo, '_blank');

            },
        });
    });
    ddlDetailedLabourTypes.on('change', function () {
        var LabourTypIdSelected = ddlDetailedLabourTypes.val();
        if (LabourTypIdSelected) {
            GetLaboursByLabourTypeId(LabourTypIdSelected);
            ddlLabour.prop("disabled", false);
        }
        else {
            ddlLabour.prop("disabled", true);
        }
    });
    function GetLaboursByLabourTypeId(Id) {
        $.ajax({
            url: '/Reports/GetLaboursByLabourTypeId?LabourTypeId= ' + Id + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddlLabour.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlLabour.append(defaultOption);
                    $.each(result.Labours, function (key, item) {
                        var option = $('<option></option>').val(item.LabourId).text(item.LabourName);
                        ddlLabour.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage);
            }
        });
    }
    $('#btnViewDetailed').on('click', function () {
        $('#loader').show();
        $('.DetailedLabourReportTable').empty();
        if (!fromDateDetailed.val()) {
            toastr.error('FromDate Is Required.');
            return;
        } else if (!toDateDetailed.val()) {
            toastr.error('ToDate Is Required.');
            return;
        } else if (!ddlDetailedLabourTypes.val() || ddlDetailedLabourTypes.val() === '--Select Option--') {
            toastr.error('Labour Type Is Required.');
            return;
        } else if (!ddlLabour.val() || ddlLabour.val() === '--Select Option--') {
            toastr.error('Labour Name Is Required.');
            return;
        }
        else {
            var requestData = {
                FromDate: fromDateDetailed.val(),
                ToDate: toDateDetailed.val(),
                LabourId: ddlLabour.val(),
                LabourTypeId: ddlDetailedLabourTypes.val()
            };
            var LabourName = "";
            $.ajax({
                url: "/Reports/GetDetailedLabourReport",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                data: JSON.stringify(requestData),
                success: function (result) {
                    $('#loader').hide();
                    var html = '';
                    html += '<table class="table table-bordered table-hover text-center mt-2 DetailedLabourReportTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th>Date</th>'
                    html += '<th>Trxn No</th>'
                    html += '<th>Particular</th>'
                    html += '<th>Qty</th>'
                    html += '<th>Rate</th>'
                    html += '<th>OT Amount</th>'
                    html += '<th>Billing</th>'
                    html += '<th>Damage</th>'
                    html += '<th>Payment</th>'
                    html += '<th>Balance</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    if (result.ResponseCode == 302) {
                        $.each(result.Labours, function (key, item) {
                            var Balance = item.OpeningBalance;
                            html += '<tr>';
                            html += '<td colspan=9>Opening Bal.</td>';
                            html += '<td>' + item.OpeningBalance + '</td>';
                            html += '</tr >';
                            if (item.ProductionEntries !== null) {
                                $.each(item.ProductionEntries, function (key, Production) {
                                    const ModifyDate = Production.ProductionDate;
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
                                    html += '<td>' + Production.ProductionNo + '</td>';
                                    html += '<td>' + Production.Product.ProductName + '</td>';
                                    html += '<td>' + Production.Quantity + '</td>';
                                    html += '<td>' + Production.Rate + '</td>';
                                    html += '<td>' + Production.OTAmount + '</td>';
                                    html += '<td>' + Production.Amount + '</td>';
                                    html += '<td>-</td>';
                                    html += '<td>-</td>';
                                    Balance += Production.Amount
                                    html += '<td>' + Balance + '</td>';
                                    html += '</tr >';

                                });
                            }
                            if (item.Payment !== null) {
                                $.each(item.Payment, function (key, Payment) {
                                    const ModifyDate = Payment.VoucherDate;
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
                                    html += '<td>' + Payment.VouvherNo + '</td>';
                                    html += '<td>Payment</td>';
                                    html += '<td>-</td>';
                                    html += '<td>-</td>';
                                    html += '<td>-</td>';
                                    html += '<td>-</td>';
                                    html += '<td>-</td>';
                                    html += '<td>' + Payment.Amount + '</td>';
                                    Balance -= Payment.Amount
                                    html += '<td>' + Balance + '</td>';
                                    html += '</tr >';
                                });
                            }
                            if (item.DamageOrders !== null) {
                                $.each(item.DamageOrders, function (key, Damage) {
                                    const ModifyDate = Damage.TransactionDate;
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
                                    html += '<td>' + Damage.TransactionNo + '</td>';
                                    html += '<td>Damage</td>';
                                    html += '<td>-</td>';
                                    html += '<td>-</td>';
                                    html += '<td>-</td>';
                                    html += '<td>-</td>';
                                    html += '<td>' + Damage.TotalAmount + '</td>';
                                    html += '<td>-</td>';
                                    Balance -= Damage.TotalAmount
                                    html += '<td>' + Balance + '</td>';
                                    html += '</tr >';
                                });
                            }
                        });

                        $('#BtnPrint').show();
                        LabourName = result.Labours[0].LabourName
                        PrintData = {
                            LaborReports: result.Labours,
                            FromDate: fromDateDetailed.val(),
                            LabourName: LabourName,
                            ToDate: toDateDetailed.val(),
                            OpeningBal: result.Labours[0].OpeningBalance
                        };
                    }
                    else {
                        html += '<tr>';
                        html += '<td colspan="9">No Record</td>';
                        html += '</tr >';
                    }
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblDetailedLabourList').html(html);
                    if (!$.fn.DataTable.isDataTable('.DetailedLabourReportTable')) {
                        var table = $('.DetailedLabourReportTable').DataTable({
                            "responsive": true,
                            "lengthChange": false,
                            "autoWidth": false,
                            "buttons": [
                                {
                                    extend: 'copy',
                                    footer: true
                                },
                                {
                                    extend: 'csv',
                                    footer: true
                                },
                                {
                                    extend: 'excel',
                                    footer: true
                                },
                                {
                                    extend: 'pdf',
                                    footer: true,
                                    title: function () {
                                        return getPrintHeaderFooter();
                                    }
                                },
                                {
                                    extend: 'print',
                                    footer: true,
                                    title: function () {
                                        return getPrintHeaderFooter();
                                    },

                                },
                                {
                                    extend: 'colvis'
                                }
                            ]
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
    });
    $('#BtnPrint').on('click', function () {
        console.log(PrintData);
        $.ajax({
            type: "POST",
            url: '/Print/LabourDetailedPrintData',
            dataType: 'json',
            data: JSON.stringify(PrintData),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                window.open(Response.redirectTo, '_blank');

            },
        });
    });

});