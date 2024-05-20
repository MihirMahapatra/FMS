$(function () {
    //default date
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;
    $("#ReportsLink").addClass("active");
    $("#InwardOutwardLink").addClass("active");
    $("#InwardOutwardLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //----------------------------------------varible declaration-----------------------------------------//
    const fromDate = $('input[name="FromDateSummerized"]');
    fromDate.val(todayDate);
    const toDate = $('input[name="ToDateSummerized"]');
    toDate.val(todayDate);
    //const ddlLabour = $('select[name="ddlLabour"]');
    //const ddlSummerizedLabourTypes = $('select[name="ddlSummerizedLabourTypes"]');
    //const ddlDetailedLabourTypes = $('select[name="ddlDetailedLabourTypes"]');
    //var PrintDataSummarized = {};
    //var PrintData = {};
    //-----------------------------------------------------InwardOutward Report Screen --------------------------------------------------//
    $('#btnViewSummerized').on('click', function () {
        $('#loader').show();
        $('.SummerizedLabourReportTable').empty();
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
                ToDate: toDate.val()
            };
            $.ajax({
                url: "/Reports/GetInwardOutwardReport",
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
                    html += '<th>Date</th>'
                    html += '<th>Branch</th>'
                    html += '<th>Transaction No</th>'
                    html += '<th>VoucherType</th>'
                    html += '<th>Item</th>'
                    html += '<th>Issue</th>' 
                    html += '<th>Recived</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    if (result.ResponseCode == 302) {
                        $('#BtnPrintSummarized').show();
                        $.each(result.InwardOutwardTransation.Orders, function (index, item) {
                            console.log(result.InwardOutwardTransation)
                            html += '<tr>';
                            html += '<td>' + item.TransactionDate + '</td>';
                            html += '<td>' + item.BranchName + '</td>';
                            html += '<td>' + item.TransactionNo + '</td>';
                            html += '<td>' + item.VoucherType + '</td>';
                            html += '<td>' + item.Product + '</td>';
                            if (item.VoucherType == 'OutWardTransation' ) {
                                html += '<td>' + item.Quantity + '</td>';
                                html += '<td>' + '-' + '</td>';
                            }
                            else {
                                html += '<td>' + '-' + '</td>';
                                html += '<td>' + item.Quantity + '</td>';
                            }
                            html += '</tr >';
                        });
                    }
                    else {
                        html += '<tr>';
                        html += '<td colspan="7">No Record</td>';
                        html += '</tr >';
                    }
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblSummerizedList').html(html);
                    if (!$.fn.DataTable.isDataTable('.SummerizedLabourReportTable')) {
                        var table = $('.SummerizedLabourReportTable').DataTable({
                            "responsive": true, "lengthChange": false, "autoWidth": false,
                        })
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
});