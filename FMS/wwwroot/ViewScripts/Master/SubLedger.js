$(function () {
    GetSubLedgers();
    function GetSubLedgers() {
        $('.SubLedgerTable').empty();
        $.ajax({
            url: "/Admin/GetSubLedgers",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                var html = '';
                if (result.ResponseCode == 404) {
                    html += '<table class="table table-bordered table-hover text-center mt-1 SubLedgerTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th hidden>SubLedger Id</th>'
                    html += '<th>Ledger Name</th>'
                    html += '<th>SubLedger Name</th>'
                    html += '<th>Action</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    html += '<tr>';
                    html += '<td colspan="4">No record</td>';
                    html += '</tr>';
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblSubLedger').html(html);
                }
                if (result.ResponseCode == 302) {
                    html += '<table class="table table-bordered table-hover text-center mt-1 SubLedgerTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th hidden>SubLedger Id</th>'
                    html += '<th>Ledger Name</th>'
                    html += '<th>SubLedger Name</th>'
                    html += '<th>Action</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    $.each(result.SubLedgers, function (key, item) {
                        html += '<tr>';
                        html += '<td hidden>' + item.SubLedgerId + '</td>';
                        html += '<td>' + item.LedgerName + '</td>';
                        html += '<td>' + item.SubLedgerName + '</td>';
                        html += '<td style="background-color:#ffe6e6;">';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-subledger-edit"   id="btnSubLedgerEdit_' + item.SubLedgerId + '"     data-id="' + item.SubLedgerId + '" data-toggle="modal" data-target="#modal-edit-subledger" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-subledger-delete" id="btnSubLedgerDelete_' + item.SubLedgerId + '"   data-id="' + item.SubLedgerId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                        html += '</td>';
                        html += '</tr >';
                    });
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblSubLedger').html(html);
                    if (!$.fn.DataTable.isDataTable('.SubLedgerTable')) {
                        $('.SubLedgerTable').DataTable({
                            "paging": true,
                            "lengthChange": false,
                            "searching": true,
                            "ordering": true,
                            "info": true,
                            "autoWidth": false,
                            "responsive": true,
                            "dom": '<"row"<"col-md-2"f><"col-md-2"l>>rtip'
                        });
                    }
                }
            },
            error: function (errormessage) {
                Swal.fire(
                    'Error!',
                    'An error occurred',
                    'error'
                );
            }
        });
    }
    $(document).on('click', '.btn-subledger-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        EditSubLedger(value);
    });
    function EditSubLedger(Id) {
        var $tr = $('#btnSubLedgerEdit_' + Id + '').closest('tr');
        var ledgerName = $tr.find('td:eq(1)').text().trim();
        var SubledgerName = $tr.find('td:eq(2)').text().trim();


        //fill Modal data
        $('input[name="mdlSubLedgerId"]').val(Id);
        $('input[name="mdlSubLedgerName"]').val(SubledgerName);

        $.ajax({
            url: "/Admin/GetLedgers",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                var selectElement = $('select[name="mdlLedgerid"]');
                if (result.ResponseCode == 404) {

                }
                if (result.ResponseCode == 302) {
                    selectElement.empty();
                    $.each(result.Ledgers, function (key, item) {
                        var option = $('<option></option>').val(item.LedgerId).text(item.LedgerName);
                        if (item.LedgerName === ledgerName) {
                            option.attr('selected', 'selected');
                        }
                        selectElement.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    $('#modal-edit-subledger').on('click', '.btnSubLedgerModal', (event) => {
        const data = {
            SubLedgerId: $('input[name="mdlSubLedgerId"]').val(),
            Fk_LedgerId: $('select[name="mdlLedgerid"]').val(),
            SubLedgerName: $('input[name="mdlSubLedgerName"]').val(),
        }
        $.ajax({
            type: "POST",
            url: '/Admin/UpdateSubLedger',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                $('#modal-edit-subledger').modal('hide');
                GetSubLedgers();
                if (Response.ResponseCode == 200) {
                    toastr.success(Response.SuccessMsg);
                }
                else {
                    toastr.error(Response.ErrorMsg);
                }
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
    $(document).on('click', '.btn-subledger-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeleteSubLedger(value);
    });
    function DeleteSubLedger(Id) {
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/Admin/DeleteSubLedger?id=' + Id + '',
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        GetSubLedgers();
                        if (result.ResponseCode == 200) {
                            toastr.success(result.SuccessMsg);
                        }
                        else {
                            toastr.error(result.ErrorMsg);
                        }
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
            }
        });
    }
})