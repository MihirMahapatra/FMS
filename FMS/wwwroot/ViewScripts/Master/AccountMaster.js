$(function () {
    $("#MasterLink").addClass("active");
    $("#AccountMasterLink").addClass("active");
    $("#AccountMasterLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //*------------------------------Variable Declaration------------------------------*//
    const ddlLedger = $('select[name="ddnLedgerId"]');
    const ledgerOpeningBal = $('input[name="ledgerOpeningBal"]');
    const ddlBalanceType = $('select[name="BalanceType"]');
    const ddnSubLedgerLedger = $('select[name="ddnSubLedgerLedgerId"]');
    const SubledgerOpeningBal = $('input[name="SubledgerOpeningBal"]');
    const SubledgerName = $('input[name="SubledgerName"]');
    const SubLedgerBalanceType = $('select[name="SubLedgerBalanceType"]');
    //------------------------------Validation Section-----------------------------//
    ledgerOpeningBal.on("input", function () {
        var inputValue = $(this).val().replace(/[^0-9.]/g, '');
        if (inputValue.length > 10) {
            inputValue = inputValue.substr(0, 10);
        }   
        $(this).val(inputValue);
    });
    SubledgerOpeningBal.on("input", function () {
        var inputValue = $(this).val().replace(/[^0-9.]/g, '');
        if (inputValue.length > 10) {
            inputValue = inputValue.substr(0, 10);
        }
        $(this).val(inputValue);
    });
    SubledgerName.on("input", function () {
        let inputValue = $(this).val();
        inputValue = inputValue.toUpperCase();
        $(this).val(inputValue);
    });

    //-------------------------------Tab------------------------------------------//
    $(document).ready(function () {
        const elements = $('input, select, button').toArray(); // Select all input, select, and button elements
        $(document).on('keydown', 'input, select, button', function (e) {
            if (e.key === 'Tab') {
                e.preventDefault(); // Prevent default tab behavior

                const currentIndex = elements.indexOf(this); // Get index of current element

                // Move focus to the next element in the sequence
                if (currentIndex > -1) {
                    const nextIndex = (currentIndex + 1) % elements.length;
                    elements[nextIndex].focus();
                }
            } else if (e.key === 'Enter') {
                // Simulate Enter behavior for buttons
                if ($(this).is('button')) {
                    e.preventDefault(); // Prevent default Enter behavior
                    $(this).click(); // Trigger the click event for the button
                }
            }
        });
    });
    //*------------------------------Ledger  Balance------------------------------*//
    GetLedgersHasNoSubLedger()
    function GetLedgersHasNoSubLedger() {
        $.ajax({
            url: "/Master/GetLedgersHasNoSubLedger",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddlLedger.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlLedger.append(defaultOption);
                    $.each(result.Ledgers, function (key, item) {
                        var option = $('<option></option>').val(item.LedgerId).text(item.LedgerName);
                        ddlLedger.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    $('#btnLedgerSubmit').on('click', LedgerSubmit);
    function LedgerSubmit() {
        if (!ledgerOpeningBal.val()) {
            toastr.error('OpeningBal  Is Required.');
            return;
        } else if (!ddlLedger.val() || ddlLedger.val() === '--Select Option--') {
            toastr.error('Ledger Type Is Required.');
            return;
        }
        else {
            var requestData = {
                Fk_LedgerId: ddlLedger.val(),
                OpeningBalance: ledgerOpeningBal.val(),
                OpeningBalanceType: ddlBalanceType.val(),
            }
            $.ajax({
                type: "POST",
                url: '/Master/CreateLedgerBalance',
                dataType: 'json',
                data: JSON.stringify(requestData),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    if (Response.ResponseCode == 201) {
                        toastr.success(Response.SuccessMsg);
                        ledgerOpeningBal.val('0')
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
    }
    //*------------------------------SubLedger Balance------------------------------*//
    GetLedgersHasSubLedger();
    function GetLedgersHasSubLedger() {
        $.ajax({
            url: "/Master/GetLedgersHasSubLedger",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddnSubLedgerLedger.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddnSubLedgerLedger.append(defaultOption);
                    $.each(result.Ledgers, function (key, item) {
                        var option = $('<option></option>').val(item.LedgerId).text(item.LedgerName);
                        ddnSubLedgerLedger.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    $('#btnSubLedgerSubmit').on('click', SubLedgerSubmit); 
    function SubLedgerSubmit() {
        var requestData = {
            Fk_LedgerId: ddnSubLedgerLedger.val(),
            SubLedgerName: SubledgerName.val(),
            OpeningBalance: SubledgerOpeningBal.val(),
            BalanceType: SubLedgerBalanceType.val()
        };
        $.ajax({
            type: "POST",
            url: '/Master/CreateSubLedgers',
            dataType: 'json',
            data: JSON.stringify(requestData),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                if (Response.ResponseCode == 201) {
                    toastr.success(Response.SuccessMsg);
                    SubledgerOpeningBal.val('0');
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
    //*------------------------------Ledger Balance List------------------------------*//
    $('a[href="#LedgerBalanceList"]').on('click', function () {
        GetLedgerBalances();
    });
    function GetLedgerBalances() {
        $('.LedgerBalanceListTable').empty();
        $.ajax({
            url: "/Master/GetLedgerBalances",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                console.log(result);
                var html = '';
                html += '<table class="table table-bordered table-hover text-center mt-1 LedgerBalanceListTable" style="width:100%">';
                html += '<thead>'
                html += '<tr>'
                html += '<th hidden>Ledger  Balance Id</th>'
                html += '<th>Group Name</th>'
                html += '<th>Ledger.</th>'
                html += '<th>Opening Bal.</th>'
                html += '<th>Opening Bal.Type</th>'
                html += '<th>Running Balance</th>'
                html += '<th>Running Bal.Type</th>'
                html += '<th>Action</th>'
                html += '</tr>'
                html += '</thead>'
                html += '<tbody>';
                if (result.ResponseCode == 302) {
                    console.log(result);
                    $.each(result.LedgerBalances, function (key, item) {
                        html += '<tr>';
                        html += '<td hidden>' + item.LedgerBalanceId + '</td>';
                        if (item.Ledger !== null) {
                            html += '<td>' + item.Ledger.LedgerGroup.GroupName + '</td>';
                            html += '<td>' + item.Ledger.LedgerName + '</td>';
                        }
                        else {
                            html += '<td>' - '</td>';
                            html += '<td>' - '</td>';
                        }
                        html += '<td>' + Math.abs(item.OpeningBalance) + '</td>';
                        html += '<td>' + item.OpeningBalanceType + '</td>';
                        html += '<td disabled>' + Math.abs(item.RunningBalance) + '</td>';
                        html += '<td>' + item.RunningBalanceType + '</td>';
                        html += '<td style="background-color:#ffe6e6;">';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-ledgerbalance-edit"   id="btnLedgerBalanceEdit_' + item.LedgerBalanceId + '"     data-id="' + item.LedgerBalanceId + '"  style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-ledgerbalance-update" id ="btnLedgerBalanceUpdate_' + item.LedgerBalanceId + '" data-id="' + item.LedgerBalanceId + '" style = "border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;display:none" > <i class="fa-solid fa-floppy-disk"></i></button >';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-ledgerbalance-cancel" id="btnLedgerBalanceCancel_' + item.LedgerBalanceId + '"   data-id="' + item.LedgerBalanceId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px; display:none"> <i class="fa-solid fa-rectangle-xmark"></i></button>';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-ledgerbalance-delete" id="btnLedgerBalanceDelete_' + item.LedgerBalanceId + '"   data-id="' + item.LedgerBalanceId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                        html += '</td>';
                        html += '</tr >';
                    });
                }
                else {
                    html += '<tr>';
                    html += '<td colspan="8">No Record</td>';
                    html += '</tr >';
                }
                html += ' </tbody>';
                html += '</table >';
                $('.tblLedgerBalanceList').html(html);
                if (!$.fn.DataTable.isDataTable('.LedgerBalanceListTable')) {
                    var table = $('.LedgerBalanceListTable').DataTable({
                        "paging": true,
                        "lengthChange": false,
                        "searching": true,
                        "ordering": true,
                        "info": true,
                        "autoWidth": false,
                        "responsive": true,
                        "dom": '<"row"<"col-md-2"f><"col-md-2"l>>rtip',
                    });
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
    $(document).on('click', '.btn-ledgerbalance-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        EditLedgerBalance(value);
    });
    function EditLedgerBalance(Id) {
        var $tr = $('#btnLedgerBalanceEdit_' + Id + '').closest('tr');
        var openingBal = $tr.find('td:eq(3)').text().trim();
        var openingBalType = $tr.find('td:eq(4)').text().trim();
        var runningBal = $tr.find('td:eq(5)').text().trim();
        var runningBalType = $tr.find('td:eq(6)').text().trim();
        $tr.find('td:eq(3)').html('<div class="form-group"><input type="text" class="form-control" value="' + openingBal + '"/></div>');
        if (openingBalType === "Dr") {
            $tr.find('td:eq(4)').html('<div class="form-group"> <div class= "form-group"><select class="form-control select2bs4" style="width: 100%;"><option value="Dr" selected>Dr</option><option value="Cr">Cr</option></select> </div></div> ');
        }
        else {
            $tr.find('td:eq(4)').html('<div class="form-group"> <div class= "form-group"><select class="form-control select2bs4" style="width: 100%;"><option value="Dr">Dr</option><option value="Cr" selected>Cr</option></select> </div></div> ');
        }
        $tr.find('td:eq(4)').html('<div class="form-group"> <div class= "form-group"><select class="form-control select2bs4" style="width: 100%;"><option value="Dr">Dr</option><option value="Cr">Cr</option></select> </div></div> ');
        $tr.find('td:eq(5)').html('<div class="form-group"><input type="text" class="form-control" disabled value="' + runningBal + '"/></div>');
        if (runningBalType === "Dr") {
            $tr.find('td:eq(6)').html('<div class="form-group"> <div class= "form-group"><select disabled class="form-control select2bs4" style="width: 100%;"><option value="Dr" selected>Dr</option><option value="Cr">Cr</option></select> </div></div> ');
        }
        else {
            $tr.find('td:eq(6)').html('<div class="form-group"> <div class= "form-group"><select disabled class="form-control select2bs4" style="width: 100%;"><option value="Dr">Dr</option><option value="Cr" selected>Cr</option></select> </div></div> ');
        }

        $tr.find('.select2bs4').select2({
            theme: 'bootstrap4'
        });
        $tr.find('#btnLedgerBalanceEdit_' + Id + ', #btnLedgerBalanceDelete_' + Id + '').hide();
        $tr.find('#btnLedgerBalanceUpdate_' + Id + ',#btnLedgerBalanceCancel_' + Id + '').show();
    }
    $(document).on('click', '.btn-ledgerbalance-update', (event) => {
        const value = $(event.currentTarget).data('id');
        UpdateLedgerBalance(value);
    });
    function UpdateLedgerBalance(id) {

        var $tr = $('#btnLedgerBalanceUpdate_' + id + '').closest('tr');
        const data = {
            LedgerBalanceId: id,
            OpeningBalance: $tr.find('input[type="text"]').eq(0).val(),
            OpeningBalanceType: $tr.find('Select').eq(0).find('option:selected').val(),
            RunningBalance: $tr.find('input[type="text"]').eq(1).val(),
            RunningBalanceType: $tr.find('Select').eq(1).find('option:selected').val(),
        }
        $.ajax({
            type: "POST",
            url: '/Master/UpdateLedgerBalance',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {

                if (Response.ResponseCode == 200) {
                    GetLedgerBalances();
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
    }
    $(document).on('click', '.btn-ledgerbalance-cancel', (event) => {
        const value = $(event.currentTarget).data('id');
        CanceLedgerBalance(value);
    });
    function CanceLedgerBalance(id) {
        var $tr = $('#btnLedgerBalanceCancel_' + id + '').closest('tr');
        var openingBal = $tr.find('input[type="text"]').eq(0).val();
        var openingBalType = $tr.find('Select').eq(0).find('option:selected').text();
        var runningBal = $tr.find('input[type="text"]').eq(1).val();
        var runningBalType = $tr.find('Select').eq(1).find('option:selected').text();
        $tr.find('td:eq(3)').text(openingBal);
        $tr.find('td:eq(4)').text(openingBalType);
        $tr.find('td:eq(5)').text(runningBal);
        $tr.find('td:eq(6)').text(runningBalType);
        $tr.find('#btnLedgerBalanceEdit_' + id + ', #btnLedgerBalanceDelete_' + id + '').show();
        $tr.find('#btnLedgerBalanceUpdate_' + id + ',#btnLedgerBalanceCancel_' + id + '').hide();
    }
    $(document).on('click', '.btn-ledgerbalance-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeleteLedgerBalance(value);
    });
    function DeleteLedgerBalance(id) {
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
                //Make an AJAX call to the server-side delete action method
                $.ajax({
                    url: '/Master/DeleteLedgerBalance?id=' + id + '',
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        if (result.ResponseCode == 200) {
                            toastr.success(result.SuccessMsg);
                            GetLedgerBalances();
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
    //*------------------------------SubLedger Balance List------------------------------*//
    $('a[href="#SubLedgerBalanceList"]').on('click', function () {
        GetSubLedgerBalances();
    });
    function GetSubLedgerBalances() {
        $('.tblSubLedgerBalanceList').empty();
        $.ajax({
            url: "/Master/GetSubLedgerBalances",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                var html = '';
                html += '<table class="table table-bordered table-hover text-center mt-1 SubLedgerBalanceTable" style="width:100%">';
                html += '<thead>'
                html += '<tr>'
                html += '<th hidden>SubLedger Balance Id</th>'
                html += '<th>Ledger</th>'
                html += '<th>Sub Ledger</th>'
                html += '<th>Opening Bal</th>'
                html += '<th>Opening Bal. Type</th>'
                html += '<th>Running Balance</th>'
                html += '<th>Running Ba. Type</th>'
                html += '<th>Action</th>'
                html += '</tr>'
                html += '</thead>'
                html += '<tbody>';
                if (result.ResponseCode == 302) {
                    $.each(result.SubLedgerBalances, function (key, item) {
                        html += '<tr>';
                        html += '<td hidden>' + item.SubLedgerBalanceId + '</td>';
                        if (item.LedgerBalance !== null) {
                            html += '<td>' + item.LedgerBalance.Ledger.LedgerName + '</td>';
                        }
                        else {
                            html += '<td>' - '</td>';
                        }
                        if (item.SubLedger !== null) {

                            html += '<td>' + item.SubLedger.SubLedgerName + '</td>';
                        }
                        else {
                            html += '<td>' - '</td>';

                        }

                        html += '<td>' + Math.abs(item.OpeningBalance) + '</td>';
                        html += '<td>' + item.OpeningBalanceType + '</td>';
                        html += '<td >' + Math.abs(item.RunningBalance) + '</td>';
                        html += '<td>' + item.RunningBalanceType + '</td>';
                        html += '<td style="background-color:#ffe6e6;">';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-subledgerbalance-edit"   id="btnSubLedgerBalanceEdit_' + item.SubLedgerBalanceId + '"     data-id="' + item.SubLedgerBalanceId + '"  style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-subledgerbalance-update" id ="btnSubLedgerBalanceUpdate_' + item.SubLedgerBalanceId + '" data-id="' + item.SubLedgerBalanceId + '" style = "border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;display:none" > <i class="fa-solid fa-floppy-disk"></i></button >';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-subledgerbalance-cancel" id="btnSubLedgerBalanceCancel_' + item.SubLedgerBalanceId + '"   data-id="' + item.SubLedgerBalanceId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px; display:none"> <i class="fa-solid fa-rectangle-xmark"></i></button>';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-subledgerbalance-delete" id="btnSubLedgerBalanceDelete_' + item.SubLedgerBalanceId + '"   data-id="' + item.SubLedgerBalanceId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                        html += '</td>';
                        html += '</tr >';
                    });
                }
                else {
                    html += '<tr>';
                    html += '<td colspan="8">No Record</td>';
                    html += '</tr >';
                }
                html += ' </tbody>';
                html += '</table >';
                $('.tblSubLedgerBalanceList').html(html);
                if (!$.fn.DataTable.isDataTable('.SubLedgerBalanceTable')) {
                    var table = $('.SubLedgerBalanceTable').DataTable({
                        "paging": true,
                        "lengthChange": false,
                        "searching": true,
                        "ordering": true,
                        "info": true,
                        "autoWidth": false,
                        "responsive": true,
                        "dom": '<"row"<"col-md-2"f><"col-md-2"l>>rtip',
                    });
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
    $(document).on('click', '.btn-subledgerbalance-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        EditSubLedgerBalance(value);
    });
    function EditSubLedgerBalance(Id) {
        var $tr = $('#btnSubLedgerBalanceEdit_' + Id + '').closest('tr');
        var openingBal = $tr.find('td:eq(3)').text().trim();
        var openingBalType = $tr.find('td:eq(4)').text().trim();
        var runningBal = $tr.find('td:eq(5)').text().trim();
        var runningBalType = $tr.find('td:eq(6)').text().trim();
        $tr.find('td:eq(3)').html('<div class="form-group"><input type="text" class="form-control" value="' + openingBal + '"/></div>');
        if (openingBalType === "Cr") {
            $tr.find('td:eq(4)').html('<div class="form-group"> <div class= "form-group"><select class="form-control select2bs4" style="width: 100%;"><option value="Dr" selected>Dr</option><option value="Cr">Cr</option></select> </div></div> ');
        }
        else {
            $tr.find('td:eq(4)').html('<div class="form-group"> <div class= "form-group"><select class="form-control select2bs4" style="width: 100%;"><option value="Dr">Dr</option><option value="Cr" selected>Cr</option></select> </div></div> ');
        }
        $tr.find('td:eq(4)').html('<div class="form-group"> <div class= "form-group"><select class="form-control select2bs4" style="width: 100%;"><option value="Dr">Dr</option><option value="Cr">Cr</option></select> </div></div> ');
        $tr.find('td:eq(5)').html('<div class="form-group"><input type="text" class="form-control" disabled  value="' + runningBal + '"/></div>');
        if (runningBalType === "Cr") {
            $tr.find('td:eq(6)').html('<div class="form-group"> <div class= "form-group"><select disabled class="form-control select2bs4" style="width: 100%;"><option value="Dr" selected>Dr</option><option value="Cr">Cr</option></select> </div></div> ');
        }
        else {
            $tr.find('td:eq(6)').html('<div class="form-group"> <div class= "form-group"><select disabled class="form-control select2bs4" style="width: 100%;"><option value="Dr">Dr</option><option value="Cr" selected>Cr</option></select> </div></div> ');
        }

        $tr.find('.select2bs4').select2({
            theme: 'bootstrap4'
        });
        $tr.find('#btnSubLedgerBalanceEdit_' + Id + ', #btnSubLedgerBalanceDelete_' + Id + '').hide();
        $tr.find('#btnSubLedgerBalanceUpdate_' + Id + ',#btnSubLedgerBalanceCancel_' + Id + '').show();
    }
    $(document).on('click', '.btn-subledgerbalance-update', (event) => {
        const value = $(event.currentTarget).data('id');
        UpdateSubLedgerBalance(value);
    });
    function UpdateSubLedgerBalance(id) {
        var $tr = $('#btnSubLedgerBalanceUpdate_' + id + '').closest('tr');
        const data = {
            SubLedgerBalanceId: id,
            OpeningBalance: $tr.find('input[type="text"]').eq(0).val(),
            OpeningBalanceType: $tr.find('Select').eq(0).find('option:selected').val(),
            RunningBalance: $tr.find('input[type="text"]').eq(1).val(),
            RunningBalanceType: $tr.find('Select').eq(1).find('option:selected').val(),
        }
        $.ajax({
            type: "POST",
            url: '/Master/UpdateSubLedgerBalance',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                if (Response.ResponseCode == 200) {
                    toastr.success(Response.SuccessMsg);
                    GetSubLedgerBalances();
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
    $(document).on('click', '.btn-subledgerbalance-cancel', (event) => {
        const value = $(event.currentTarget).data('id');
        CancelSubLedgerBalance(value);
    });
    function CancelSubLedgerBalance(id) {
        var $tr = $('#btnSubLedgerBalanceCancel_' + id + '').closest('tr');
        var openingBal = $tr.find('input[type="text"]').eq(0).val();
        var openingBalType = $tr.find('Select').eq(0).find('option:selected').text();
        var runningBal = $tr.find('input[type="text"]').eq(1).val();
        var runningBalType = $tr.find('Select').eq(1).find('option:selected').text();
        $tr.find('td:eq(3)').text(openingBal);
        $tr.find('td:eq(4)').text(openingBalType);
        $tr.find('td:eq(5)').text(runningBal);
        $tr.find('td:eq(6)').text(runningBalType);
        $tr.find('#btnSubLedgerBalanceEdit_' + id + ', #btnSubLedgerBalanceDelete_' + id + '').show();
        $tr.find('#btnSubLedgerBalanceUpdate_' + id + ',#btnSubLedgerBalanceCancel_' + id + '').hide();
    }
    $(document).on('click', '.btn-subledgerbalance-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeleteSubLedgerBalance(value);
    });
    function DeleteSubLedgerBalance(id) {
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
                //Make an AJAX call to the server-side delete action method
                $.ajax({
                    url: '/Master/DeleteSubLedgerBalance?id=' + id + '',
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        if (result.ResponseCode == 200) {
                            GetSubLedgerBalances();
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
}); 