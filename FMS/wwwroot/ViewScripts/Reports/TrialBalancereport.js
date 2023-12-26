$(function () {
    //----------------------------------------varible declaration-----------------------------------------//
    //default date
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;
    const fromDate = $('input[name="FromDate"]');
    fromDate.val(todayDate);
    const toDate = $('input[name="ToDate"]');
    toDate.val(todayDate);
    const ClosingBal = $('input[name="ClosingBal"]');
    //-----------------------------------------------------Stock Report Scren --------------------------------------------------//
    $('#btnView').on('click', function () {
        $('#loader').show();
        $('.LadgerBookReportTable').empty();

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
            };
            $.ajax({
                url: "/Reports/TrialBalanceReport",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                data: JSON.stringify(requestData),
                success: function (result) {
                    $('#loader').hide();
                    var html = '';
                    html += '<table class="table table-bordered table-hover text-center mt-2 LadgerBookReportTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th>Particulurs</th>'
                    html += '<th>Opening Balance</th>'
                    html += '<th>Type</th>'
                    html += '<th>Debit Total</th>'
                    html += '<th>Credit Total</th>'
                    html += '<th>Closing Balance</th>'
                    html += '<th>Type</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    if (result.ResponseCode == 302) {
                        $(".hddiv").show();
                        $.each(result.TrialBalances, function (key, item) {
                            html += '<tr>';
                            html += '<td>' + item.LedgerName + '</td>';
                            html += '<td>' + item.OpeningBal + '</td>';
                            if (item.OpeningBal > 0) {
                                html += '<td>' + "Cr" + '</td>';
                            } else if (item.OpeningBal < 0) {
                                html += '<td>' + "Dr" + '</td>';
                            } else {
                                html += '<td>' + "-" + '</td>';
                            }
                            html += '<td>' + item.DebitTotal + '</td>';
                            html += '<td>' + item.CreditTotal + '</td>'; 
                            html += '<td>' + item.ClosingBal + '</td>';
                            if (item.ClosingBal > 0) {
                                html += '<td>' + "Cr" + '</td>';
                            } else if (item.ClosingBal < 0) {
                                html += '<td>' + "Dr" + '</td>';
                            } else {
                                html += '<td>' + "-" + '</td>';
                            }
                            html += '</tr >';
                        });
                    }
                    else {
                        html += '<tr>';
                        html += '<td colspan="6">No Record</td>';
                        html += '</tr >';
                    }
                    html += ' </tbody>';
                    html += '</table >';
                    $('.LadgerBookTransationsDetails').html(html);
                    if (!$.fn.DataTable.isDataTable('.LadgerBookReportTable')) {
                        var table = $('.LadgerBookReportTable').DataTable({
                            "responsive": true, "lengthChange": false, "autoWidth": false,
                            "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"]
                        }).buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');
                    }
                    PrintData = {
                        
                        FromDate: fromDate.val(),
                        Todate: toDate.val(),
                        TrialBalances: result.TrialBalances
                    };
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
    $("#btnPrint").click(function () {
        $.ajax({
            type: "POST",
            url: '/Print/TrialBalancePrintData',
            dataType: 'json',
            data: JSON.stringify(PrintData),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                window.open(Response.redirectTo, '_blank');
            },
        });
    });
});