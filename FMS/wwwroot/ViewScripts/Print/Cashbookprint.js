$(function () {
    $('#btnView').on('click', function () {
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
                ToDate: toDate.val(),
            };
            $.ajax({
                url: "/Reports/CashBookReport",
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
                    html += '<th>Particulurs</th>'
                    html += '<th>Voucher Type</th>'
                    html += '<th>Voucher Number</th>'
                    html += '<th>Debit</th>'
                    html += '<th>Credit</th>'
                    html += '<th>Balance</th>'
                    html += '<th>Type</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    if (result.ResponseCode == 302) {
                        $.each(result.Payments, function (key, item) {
                            html += '<tr>';
                            html += '<td>' + item.LabourName + '</td>';
                            html += '<td>' + item.OpeningBal + '</td>';
                            html += '<td>' + item.OpeningBalType + '</td>';
                            html += '<td>' + item.BillingAmt + '</td>';
                            html += '<td>' + item.DamageAmt + '</td>';
                            html += '<td>' + item.PaymentAmt + '</td>';
                            var Balance = item.BillingAmt - item.DamageAmt - item.PaymentAmt;
                            html += '<td>' + Balance + '</td>';
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
});