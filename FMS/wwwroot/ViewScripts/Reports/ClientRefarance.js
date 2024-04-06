$(function () {
    //Default Date 
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;
    $("#ReportsLink").addClass("active");
    $("#ClientRefaranceLink").addClass("active");
    $("#ClientRefaranceLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //----------------------------------------varible declaration-----------------------------------------//
    const fromDateSummerized = $('input[name="FromDateSummerized"]');
    fromDateSummerized.val(todayDate);
    const toDateSummerized = $('input[name="ToDateSummerized"]');
    toDateSummerized.val(todayDate);
    //-------------------------------------ClientRefarance Report------------------------------------------------//
    var requestData = {};
    $('#btnViewSummerized').on('click', function () {
        $('#loader').show();
        $('.SummerizedReportTable').empty();
        if (!fromDateSummerized.val()) {
            toastr.error('FromDate Is Required.');
            return;
        } else if (!toDateSummerized.val()) {
            toastr.error('ToDate Is Required.');
            return;
        } else {

            requestData = {
                FromDate: fromDateSummerized.val(),
                ToDate: toDateSummerized.val(),
            };
            $.ajax({
                url: "/Reports/GetCustomerRefranceReport",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                data: JSON.stringify(requestData),
                success: function (result) {
                    $('#loader').hide();
                    var html = '';
                    html += '<table class="table table-bordered table-hover text-center mt-2 SummerizedReportTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th class="text-left">Client Name</th>'
                    html += '<th>Refarance Name</th>'
                    html += '<th class="text-right">Balance</th>'
                    html += '<th>Type</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    if (result.ResponseCode == 302) {
                        $('#BtnPrintSummarized').show();
                        $.each(result.PartySummerized, function (key, item) {
                            html += '<tr>';
                            html += '<td class="text-left">' + item.PartyName + '</td>';
                            if (item.RefarenceName != null) {
                                html += '<td>' + item.RefarenceName + '</td>';
                            } else {
                                html += '<td>' + "-" + '</td>';
                            }
                            var balance = item.Balance + item.OpeningBal;
                            var balanceType = balance >= 0 ? "Dr" : "Cr";
                            //html += '<td>' + Math.abs(balance) + '</td>';
                            if (0 > balance) {
                                html += '<td class="bg-danger text-white text-right">' + balance.toFixed(2) + '</td>';
                            }
                            else {
                                html += '<td class=" text-right">' + balance.toFixed(2) + '</td>';
                            }
                            html += '<td>' + balanceType + '</td>';
                            html += '</tr>';
                        })
                    }
                    else {
                        html += '<tr>';
                        html += '<td colspan="6">No Record</td>';
                        html += '</tr >';
                    }
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblSummerizedList').html(html);
                    if (!$.fn.DataTable.isDataTable('.SummerizedReportTable')) {
                        var table = $('.SummerizedReportTable').DataTable({
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
    });
    $('#BtnPrintSummarized').on('click', function () {
        var queryString = $.param(requestData);
        var url = '/Print/ClientRefarencePrint?' + queryString;
        window.open(url, '_blank');
    });
})