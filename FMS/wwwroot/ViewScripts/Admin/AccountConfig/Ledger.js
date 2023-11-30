$(function () {
    GetLedgers();
    function GetLedgers() {
        $('.LedgerTable').empty();
        $.ajax({
            url: "/Admin/GetLedgers",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                var html = '';
                if (result.ResponseCode == 404) {
                    html += '<table class="table table-bordered table-hover text-center mt-1 LedgerTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th hidden>Ledger Id</th>'
                    html += '<th>Group</th>'
                    html += '<th>SubGroup</th>'
                    html += '<th>Ledger</th>'
                    html += '<th>Ledger Type</th>'
                    html += '<th>Action</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    html += '<tr>';
                    html += '<td colspan="6">No record</td>';
                    html += '</tr>';
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblLedger').html(html);
                }
                if (result.ResponseCode == 302) {
                    html += '<table class="table table-bordered table-hover text-center mt-1 LedgerTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th hidden>Ledger Id</th>'
                    html += '<th>Group</th>'
                    html += '<th>SubGroup</th>'
                    html += '<th>Ledger</th>'
                    html += '<th>Ledger Type</th>'
                    html += '<th>Action</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    $.each(result.Ledgers, function (key, item) {
                        html += '<tr>';
                        html += '<td hidden>' + item.LedgerId + '</td>';
                        html += '<td>' + item.LedgerGroup.GroupName + '</td>';
                        if (item.LedgerSubGroup !== null) {
                            html += '<td>' + item.LedgerSubGroup.SubGroupName + '</td>';
                        } else {
                            html += '<td> - </td>';
                        }
                        html += '<td>' + item.LedgerName + '</td>';
                        html += '<td>' + item.LedgerType + '</td>';
                        html += '<td style="background-color:#ffe6e6;">';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-ledger-edit"   id="btnLedgerEdit_' + item.LedgerId + '"     data-id="' + item.LedgerId + '" data-toggle="modal" data-target="#modal-edit-ledger" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-ledger-delete" id="btnLedgerDelete_' + item.LedgerId + '"   data-id="' + item.LedgerId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                        html += '</td>';
                        html += '</tr >';
                    });
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblLedger').html(html);
                    if (!$.fn.DataTable.isDataTable('.LedgerTable')) {
                        $('.LedgerTable').DataTable({
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
    $(document).on('click', '.btn-ledger-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        EditLedger(value);
    });
    function EditLedger(Id) {
        var $tr = $('#btnLedgerEdit_' + Id + '').closest('tr');
        var ledgerGroupName = $tr.find('td:eq(1)').text().trim();
        var ledgerSubGroupName = $tr.find('td:eq(2)').text().trim();
        var ledgerName = $tr.find('td:eq(3)').text().trim();
        var ledgerType = $tr.find('td:eq(4)').text().trim();
     
        //fill Modal data
        $('input[name="mdlLedgerId"]').val(Id);
        $('input[name="mdlLedgerName"]').val(ledgerName);
        $('select[name="mdlLedgerType"]').val(ledgerType);
        
        $.ajax({
            url: "/Admin/GetLedgerGroups",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                var selectElement = $('select[name="mdlLedgerGroupId"]');
                if (result.ResponseCode == 404) {

                }
                if (result.ResponseCode == 302) {
                    selectElement.empty();
                    $.each(result.LedgerGroups, function (key, item) {
                        var option = $('<option></option>').val(item.LedgerGroupId).text(item.GroupName);
                        if (item.GroupName === ledgerGroupName) {
                            option.attr('selected', 'selected');
                            getSubGroup(item.LedgerGroupId, ledgerSubGroupName);
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
    function getSubGroup(GroupId, ledgerSubGroupName) {
        var defaultOption = $('<option></option>').val('').text('--Select Option--');
        var selectSubGroupElement = $('select[name="mdlLedgerSubGroupId"]');
        selectSubGroupElement.empty();
        selectSubGroupElement.append(defaultOption);
        $.ajax({
            url: '/Admin/GetLedgerSubGroups?GroupId=' + GroupId + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    $.each(result.LedgerSubGroups, function (key, item) {
                        var option = $('<option></option>').val(item.LedgerSubGroupId).text(item.SubGroupName);
                        if (item.SubGroupName === ledgerSubGroupName) {
                            option.attr('selected', 'selected');
                        }
                        selectSubGroupElement.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    $('select[name="mdlLedgerGroupId"]').change(function () {
        const GroupId = $('select[name="mdlLedgerGroupId"]').val();
        getSubGroup(GroupId);
    });
    $('#modal-edit-ledger').on('click', '.btnLedgerModal', (event) => {
        const data = {
            LedgerId: $('input[name="mdlLedgerId"]').val(),
            Fk_LedgerGroupId: $('select[name="mdlLedgerGroupId"]').val(),
            Fk_LedgerSubGroupId: $('select[name="mdlLedgerSubGroupId"]').val(),
            LedgerType: $('select[name="mdlLedgerType"]').val(),
            LedgerName: $('input[name="mdlLedgerName"]').val(),
        }
   
        $.ajax({
            type: "POST",
            url: '/Admin/UpdateLedger',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                $('#modal-edit-ledger').modal('hide');
                GetLedgers();
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
    $(document).on('click', '.btn-ledger-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeleteLedger(value);
    });
    function DeleteLedger(Id) {
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
                    url: '/Admin/DeleteLedger?id=' + Id + '',
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        GetLedgers();
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