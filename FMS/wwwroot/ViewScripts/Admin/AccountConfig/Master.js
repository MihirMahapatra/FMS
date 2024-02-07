$(function () {
    $("#AdminLink").addClass("active");
    $("#AccountConfigLink").addClass("active");
    $("#AccountConfigLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    var ledgerTable = $('#tblLedger').DataTable({
        "paging": true,
        "lengthChange": false,
        "searching": false,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true,
        lengthMenu: [5, 10, 25, 50], // Set the available page lengths
        pageLength: 5 // Set the default page length to 5
    });
    var subLedgerTable = $('#tblSubLedger').DataTable({
        "paging": true,
        "lengthChange": false,
        "searching": false,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true,
        lengthMenu: [5, 10, 25, 50], // Set the available page lengths
        pageLength: 5 // Set the default page length to 5
    });
    //*********************************Variable Declaration**********************************************************************//
    const ddlLedgerGroup = $('select[name="LedgerGroupId"]');
    const ddlLedgerSubGroup = $('select[name="LedgerSubGroupId"]');
    //**********************************************Ledger Group***********************************************//
    LoadLedgerGroup()
    function LoadLedgerGroup()   {
        $.ajax({
            url: "/Admin/GetLedgerGroups",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddlLedgerGroup.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlLedgerGroup.append(defaultOption);
                    $.each(result.LedgerGroups, function (key, item) {
                        var option = $('<option></option>').val(item.LedgerGroupId).text(item.GroupName);
                        ddlLedgerGroup.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    //************************************************Ledger SubGroup*********************************************************//
    function enableLedgerSubGroup() {
        var LedgerGroupIdSelected = ddlLedgerGroup.val();
        if (LedgerGroupIdSelected) {
            ddlLedgerSubGroup.prop("disabled", false);
            LoadLedgerSubGroup(LedgerGroupIdSelected)

        } else {
            ddlLedgerSubGroup.prop("disabled", true);
        }
    }
    function LoadLedgerSubGroup(GroupId) {
        ddlLedgerSubGroup.empty();
        var defaultOption = $('<option></option>').val('').text('--Select Option--');
        ddlLedgerSubGroup.append(defaultOption);
        $.ajax({
            url: '/Admin/GetLedgerSubGroups?GroupId=' + GroupId + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    $.each(result.LedgerSubGroups, function (key, item) {
                        var option = $('<option></option>').val(item.LedgerSubGroupId).text(item.SubGroupName);
                        ddlLedgerSubGroup.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    ddlLedgerGroup.change(function () {
        enableLedgerSubGroup();
    });
    $('#btnLedgerSubGrupAdd').on('click', function () {
        if (!ddlLedgerGroup.val() || ddlLedgerGroup.val() === '--Select Option--') {
            toastr.error('Plz Select a Group Name For Create a Subgroup');
            return;
        }
        $('#modal-add-subgroup').modal('show');
    });
    $('.ledgerSubGroupAdd').on('click', LedgerSubGroupAdd);
    function LedgerSubGroupAdd () {
        const data = {
            Fk_LedgerGroupId: ddlLedgerGroup.val(),
            SubGroupName: $('input[name="mdlLedgerSubGroupAdd"]').val(),
        }
        $.ajax({
            type: "POST",
            url: '/Admin/CreateLedgerSubGroup',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                $('#modal-add-subgroup').modal('hide');
                if (Response.ResponseCode == 201) {
                    toastr.success(Response.SuccessMsg);
                    $('input[name="mdlLedgerSubGroupAdd"]').val('');
                }
                else {
                    toastr.error(Response.ErrorMsg);
                }
                const LederGroupId = ddlLedgerGroup.find('option:selected').val();
                LoadLedgerSubGroup(LederGroupId);
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
    $('#btnLedgerSubGrupEdit').on('click', LedgerSubGrupEdit);
    function LedgerSubGrupEdit() {
        if (!ddlLedgerGroup.val() || ddlLedgerGroup.val() === '--Select Option--') {
            toastr.error('Plz Select a Group Name');
            return;
        }
        else if (!ddlLedgerSubGroup.val() || ddlLedgerSubGroup.val() === '--Select Option--') {
            toastr.error('Plz Select SubGroup Name');
            return;
        }
        else {
            const selectedOption = ddlLedgerSubGroup.find('option:selected');
            var text = selectedOption.text();
            var value = selectedOption.val();
            $("input[name='mdlLedgerSubGroupEdit']").val(text);
            $("input[name='mdlLedgerSubGroupId']").val(value);
            $('#modal-edit-subgroup').modal('show');
        }
    }
    $('.ledgerSubGroupUpdate').on('click', LedgerSubGroupUpdate);
    function LedgerSubGroupUpdate() {
        const data = {
            LedgerSubGroupId: $('input[name="mdlLedgerSubGroupId"]').val(),
            SubGroupName: $('input[name="mdlLedgerSubGroupEdit"]').val(),
            Fk_LedgerGroupId: ddlLedgerGroup.find('option:selected').val()
        }
        $.ajax({
            type: "POST",
            url: '/Admin/UpdateLedgerSubGroup',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                $('#modal-edit-subgroup').modal('hide');
                LoadLedgerSubGroup(data.Fk_LedgerGroupId);
                if (Response.ResponseCode == 200) {
                    toastr.success(Response.SuccessMsg);
                    $('input[name="mdlLedgerGroupId"]').val('');
                    $('input[name="mdlLedgerGroupEdit"]').val('');
                }
                else {
                    toastr.error(Response.ErrorMsg);
                }
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
    $('#btnLedgerSubGrupDelete').on('click', LedgerSubGrupDelete)
    function LedgerSubGrupDelete() {
        if (!ddlLedgerSubGroup.val() || ddlLedgerSubGroup.val() === '--Select Option--') {
            toastr.error('Plz Select a SubGroup Name To Edit');
            return;
        }
        const Id = ddlLedgerSubGroup.find('option:selected').val();
        const LederGroupId = ddlLedgerGroup.find('option:selected').val();
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
                    url: '/Admin/DeleteLedgerSubGroup?id=' + Id + '',
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        LoadLedgerSubGroup(LederGroupId);
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
    //******************************************Ledger***************************************************//
    $('#addLedgerRowBtn').click(function () {
        var newRowData = [
            '<div class="form-group"><select class="form-control select2bs4" style="width: 100%;"><option selected="selected" value="None">None</option><option value="Cash">Cash</option><option value="Bank">Bank</option></select></div>',
            '<div class="form-group"><input type="text" class="form-control" id=""></div>',
            '<div class="form-group"><select class="form-control select2bs4" style="width: 100%;"><option selected="selected" value="No">No</option><option value="Yes">Yes</option></select></div>',
        ];
        ledgerTable.row.add(newRowData).draw();
        $('#tblLedger tbody').find('.select2bs4').select2({
            theme: 'bootstrap4'
        });
    });
    $('#btnLedger').click(function () {
        $('#loader').show();
        if (!ddlLedgerGroup.val() || ddlLedgerGroup.val() === '--Select Option--') {
            toastr.error('Plz Select a Group Name For Ledger');
            return;
        }
        var rowData = [];
        $('#tblLedger tbody tr').each(function () {
            var row = $(this);
            var cellData = [];
            row.find('td').each(function () {
                var cell = $(this);
                var input = cell.find('input, select');
                var value = input.val();
                cellData.push(value);
            });
            rowData.push(cellData);
        });
        var selectedLedgerGroupId = ddlLedgerGroup.val();
        var selectedLedgerSubGroupId = ddlLedgerSubGroup.val();
        var requestData = {
            LedgerGroupId: selectedLedgerGroupId,
            LedgerSubGroupId: selectedLedgerSubGroupId,
            rowData: rowData
        };
        $.ajax({
            type: "POST",
            url: '/Admin/CreateLedgers',
            dataType: 'json',
            data: JSON.stringify(requestData),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                $('#loader').hide();
                if (Response.ResponseCode == 201) {
                    toastr.success(Response.SuccessMsg);
                    ledgerTable.clear().draw();
                }
                else {
                    toastr.error(Response.ErrorMsg);
                }
            },
            error: function (error) {
                console.log(error);
                $('#loader').hide();
            }
        });
    });
});