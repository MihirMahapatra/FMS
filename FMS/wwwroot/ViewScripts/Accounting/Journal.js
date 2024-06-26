$(function () {
    //Default Date 
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;
    $("#AccountingLink").addClass("active");
    $("#JournalLink").addClass("active");
    $("#JournalLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //-----------------------------------------Insert Button K--------------------------------------------//
    $('#addJournalRowBtn').on('keydown', function (e) {
        if (e.key === 'Insert' || e.keyCode === 45) {
            $('#addJournalRowBtn').click();
        }
    });
    $(document).on('keydown', function (e) {
        if (e.key === 'Insert' || e.keyCode === 45) {
            e.preventDefault();
            $('#addJournalRowBtn').click();
        }
    });
    //----------------------------------------varible declaration-----------------------------------------//
    var JournalTable = $('#tblJournal').DataTable({
        "paging": false,
        "lengthChange": false,
        "searching": false,
        "ordering": true,
        "info": false,
        "autoWidth": false,
        "responsive": true,
    });
    const VoucherNo = $('input[name="VoucherNo"]');
    const VoucherDate = $('input[name="VoucherDate"]');
    VoucherDate.val(todayDate);
    const Narration = $('textarea[name="Narration"]');
    //-------------------------------------------------------------Journal --------------------------------------------------------//
    GetJournalVoucherNo();
    function GetJournalVoucherNo() {
        $.ajax({
            url: "/Accounting/GetJournalVoucherNo",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                VoucherNo.val(result.Data);
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    $('#addJournalRowBtn').on('click', addJournalRowBtn);
    function addJournalRowBtn() {
        var uniqueId = 'ddlitem' + new Date().getTime();
        $.ajax({
            url: "/Accounting/GetLedgers",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    var html = '<tr class="tr">';
                    html += '<td style="width:10px">';
                    html += '<div class="form-group">';
                    html += '<select class="form-control select2bs4 mySelection" style = "width: 100%"  name = "BalanceType">';
                    html += '<option value="Dr" selected = "selected" > DR </option>';
                    html += '<option value ="Cr"> CR </option>';
                    html += '</select>';
                    html += '</div>';
                    html += '</td>';
                    html += '<td style = "width:45px">';
                    html += '<div class="form-group row">';
                    html += '<div class="col-sm-6">';
                    html += '<select class="select2bs4 ledgerType" style = "width: 100%;" data-target="additionalDropdown_' + uniqueId + '" name="ddlLedgerId">';
                    html += '<option>--Select Option--</option>';
                    $.each(result.Ledgers, function (key, item) {
                        var option = $('<option></option>').val(item.LedgerId).text(item.LedgerName);
                        html += option.prop('outerHTML');
                    });
                    html += '</select>';
                    html += '</div>';
                    html += '<label name="LadgerCurBal" class="col-sm-3 col-form-label" > Cur Bal: </label>';
                    html += '</div>';
                    html += '<div class="additionalDropdown" data-id="additionalDropdown_' + uniqueId + '"> </div>';
                    html += '</td>';
                    html += '<td style = "width:15px">';
                    html += '<div class="form-group">';
                    html += '<input type="text" class="form-control" id = "txtDrAmount" name = "DrBalance">';
                    html += '</div>';
                    html += '</td>';
                    html += '<td style = "width:15px">';
                    html += '<div class="form-group">';
                    html += '<input type="text" class="form-control" id = "txtCrAmount" name = "CrBalance" disabled>';
                    html += '</div>';
                    html += '</td>';
                    html += '<td style = "width:15px">';
                    html += ' <button class="btn btn-primary btn-link journalRemoveBtn" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                    html += '</td>';
                    html += '</tr>';
                    //var newRow = JournalTable.row.add($(html)).draw(false).node();
                    var newRow = $('#tblJournal tbody').append(html);
                    $(newRow).find('.select2bs4').select2({
                        theme: 'bootstrap4'
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    $(document).on('change', '.mySelection', function () {
        var select = $(this);
        var tr = select.closest('tr');
        var Option = select.val();
        if (Option === 'Dr') {
            tr.find('input[name="DrBalance"]').prop('disabled', false);
            tr.find('input[name="CrBalance"]').prop('disabled', true);
            tr.find('input[name="CrBalance"]').val('');
        } else if (Option === 'Cr') {
            tr.find('input[name="DrBalance"]').prop('disabled', true);
            tr.find('input[name="CrBalance"]').prop('disabled', false);
            tr.find('input[name="DrBalance"]').val('');
        } else {
            tr.find('input[name="DrBalance"]').prop('disabled', false);
            tr.find('input[name="CrBalance"]').prop('disabled', false);
        }
    });
    var selectedOption = "";
    $(document).on('change', '.ledgerType', function () {
        var select = $(this);
        var uniqueId = new Date().getTime();
        var tr = select.closest('tr');
        selectedOption = select.val();
        if (selectedOption) {
            var dataTarget = $(this).data('target');
            $('.additionalDropdown[data-id="' + dataTarget + '"]').html('');
            var selectedName = $(this).find('option:selected').text();
            var html = "";
            $.ajax({
                url: '/Accounting/GetSubLedgersById?LedgerId=' + selectedOption + '',
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.ResponseCode == 302) {
                        html += '<div class="form-group row">';
                        html += '<label name="SubLadgerCurBal" for="SubLadgerCurBal_' + uniqueId +'" class="col-sm-2 col-form-label">Cur Bal: </label>';
                        html += '<div class="col-sm-5" >';
                        html += '<select class= "select2bs4 SubledgerType"  style = "width: 100%;" name="ddlSubledgerId">';
                        html += '<option>--Select Option--</option>';
                        $.each(result.SubLedgers, function (key, item) {
                            html += '<option value=' + item.SubLedgerId + '>' + item.SubLedgerName + ' </option>';
                        });
                        html += '</select>';
                        html += '</div>';
                        html += '<div class="col-sm-3" >';
                        html += '<input type="text" class="form-control"  name="SubledgerAmount">';
                        html += '</div>';
                        html += '<div class="col-sm-2">';
                        html += '<button class="btn btn-primary btn-link addSubLedgerBtn" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-plus"></i></button>';
                        html += ' <button class="btn btn-primary btn-link deleteBtns" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                        html += '</div>';
                        html += '</div>';
                        $('.additionalDropdown[data-id="' + dataTarget + '"]').html(html);
                        $('.form-group').find('.select2bs4').select2({
                            theme: 'bootstrap4'
                        });
                    }
                },
                error: function (errormessage) {
                    console.log(errormessage)
                }
            });
            $.ajax({
                url: '/Master/GetLedgerBalances',
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {

                    if (result.ResponseCode == 302) {
                        $.each(result.LedgerBalances, function (index, item) {
                            if (item.Ledger.LedgerName === selectedName) {
                                var bal = "Cur Bal: " + Math.abs(item.RunningBalance) + " " + item.RunningBalanceType
                                tr.find('label[name="LadgerCurBal"]').text(bal);
                            }
                        });
                    }
                }
            });
        }
    });
    $(document).on('input', 'input[name="SubledgerAmount"]', function () {
        var row = $(this).closest('.tr');
        var totalSum = 0;
        row.find('input[name="SubledgerAmount"]').each(function () {
            var value = parseFloat($(this).val()) || 0;
            totalSum += value;
        });
        row.find('input[name="DrBalance"]').val(totalSum.toFixed(2));
    });
    var selectedOptions = "";
    $(document).on('change', '.SubledgerType', function () {
        var select = $(this);
        var tr = select.closest('div.form-group.row'); 
        selectedOptions = select.val();
        if (selectedOptions) {
            var uniqueId = select.closest('.form-group.row').find('label[for^="SubLadgerCurBal_"]').attr('for').split('_')[1];
            var selectedName = select.find('option:selected').text();
            $.ajax({
                url: '/Master/GetSubLedgerBalances',
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.ResponseCode == 302) {
                        $.each(result.SubLedgerBalances, function (index, item) {
                            if (item.SubLedger.SubLedgerName.trim().toLowerCase() === selectedName.trim().toLowerCase()) {
                                var bal = "CurBal: " + Math.abs(item.RunningBalance) + " " + item.RunningBalanceType
                                tr.find('label[for="SubLadgerCurBal_' + uniqueId + '"]').text(bal);
                            }
                        });
                    }
                }
            });
            //
        }
    });
    $(document).on('click', '.deleteBtns', function () {
        $(this).closest('.form-group.row').remove();
    });
    $(document).on('click', '.addSubLedgerBtn', function () {
        var clickedButton = $(this);
        var uniqueId =new Date().getTime();
        $.ajax({
            url: '/Accounting/GetSubLedgersById?LedgerId=' + selectedOption + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    var html = "";
                    html += '<div class="form-group row">';
                    html += '<label name="SubLadgerCurBal" for="SubLadgerCurBal_' + uniqueId + '" class="col-sm-2 col-form-label">Cur Bal: </label>';
                    html += '<div class="col-sm-5" >';
                    html += '<select class= "select2bs4 SubledgerType"  style = "width: 100%;" name="ddlSubledgerId">';
                    html += '<option>--Select Option--</option>';
                    $.each(result.SubLedgers, function (key, item) {
                        html += '<option value=' + item.SubLedgerId + '>' + item.SubLedgerName + ' </option>';
                    });
                    html += '</select>';
                    html += '</div>';
                    html += '<div class="col-sm-3" >';
                    html += '<input type="text" class="form-control" name="SubledgerAmount">';
                    html += '</div>';
                    html += '<div class="col-sm-2">';
                    html += '<button class="btn btn-primary btn-link addSubLedgerBtn" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-plus"></i></button>';
                    html += ' <button class="btn btn-primary btn-link removeSubladgerBtn" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                    html += '</div>';
                    html += '</div>';
                    clickedButton.closest('.form-group.row').after(html);
                    $('.form-group').find('.select2bs4').select2({
                        theme: 'bootstrap4'
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    });
    $(document).on('click', '.removeSubladgerBtn', function () {
        $(this).closest('.form-group.row').remove();
    });
    $(document).on('click', '.journalRemoveBtn', function () {
        $(this).closest('.tr').remove();
    });
    $('#btnSave').on('click', CreateJournal);
    function CreateJournal() {
        if (!VoucherDate.val()) {
            toastr.error('VoucherDate Is Required.');
            return;
        } else if (!Narration.val()) {
            toastr.error('Narration Is Required.');
            return;
        } else {
            $('#loader').show();
            var requestData = {
                VoucherDate: VoucherDate.val(),
                VoucherNo: VoucherNo.val(),
                Narration: Narration.val(),
                "arr": []
            };
            
            $('tbody tr').each(function () {
                var row = $(this);
                var rowData = {
                    "BalanceType": row.find("select[name='BalanceType']").val(),
                    "ddlLedgerId": row.find("select[name='ddlLedgerId']").val(),
                    "DrBalance": row.find("input[name='DrBalance']").val(),
                    "CrBalance": row.find("input[name='CrBalance']").val(),
                    "subledgerData": []
                };
                if (rowData.ddlLedgerId) {
                    row.find(".additionalDropdown[data-id^='additionalDropdown_']").each(function () {
                        var subledgerRow = $(this);
                        var subledgerData = {
                            "ddlSubledgerId": [],
                            "SubledgerAmunt": []
                        };
                        subledgerRow.find("select[name='ddlSubledgerId']").each(function () {
                            subledgerData.ddlSubledgerId.push($(this).val());
                        });
                        subledgerRow.find("input[name='SubledgerAmount']").each(function () {
                            subledgerData.SubledgerAmunt.push($(this).val());
                        });
                        if (subledgerData.ddlSubledgerId.length > 0 || subledgerData.SubledgerAmunt.length > 0) {
                            rowData.subledgerData.push(subledgerData);
                        }
                    });
                    requestData.arr.push(rowData);
                }
               
            });
            $.ajax({
                type: "POST",
                url: '/Accounting/CreateJournal',
                dataType: 'json',
                data: JSON.stringify(requestData),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#loader').hide();
                    if (Response.ResponseCode == 201) {
                        toastr.success(Response.SuccessMsg);
                        GetJournalVoucherNo();
                        JournalTable.clear().draw();
                        Narration.val('');
                        VoucherDate.val('');
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
        }
    }
    //_______________________________________________Journal List ________________________________________________________________________//
    $('a[href="#JournalList"]').on('click', function () {
        LoadJournals();
    });
    function LoadJournals() {
        $('#loader').show();
        $('.JournalList').empty();
        $.ajax({
            url: "/Accounting/GetJournals",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#loader').hide();
                var html = '';
                html += '<table class="table table-bordered table-hover text-center mt-1 JournalList" style="width:100%">';
                html += '<thead>'
                html += '<tr>'
                html += '<th> Vouvher No</th>'
                html += '<th>Date</th>'
                html += '<th>Ledger</th>'
                html += '<th>SubLedger</th>'
                html += '<th>Narration</th>'
                html += '<th>Amount</th>'
                html += '<th>DrCr</th>'
                html += '<th>Action</th>'
                html += '</tr>'
                html += '</thead>'
                html += '<tbody>';
                if (result.ResponseCode == 302) {
                    $.each(result.GroupedJournals, function (key, item) {
                        var firstRow = true;
                        $.each(item.Journals, function (key, journal) {
                            html += '<tr>';
                            html += '<td>' + item.VoucherNo + '</td>';
                            let formattedDate = '';
                            const ModifyDate = journal.VoucherDate;
                            if (ModifyDate) {
                                const dateObject = new Date(ModifyDate);
                                if (!isNaN(dateObject)) {
                                    const day = String(dateObject.getDate()).padStart(2, '0');
                                    const month = String(dateObject.getMonth() + 1).padStart(2, '0');
                                    const year = dateObject.getFullYear();
                                    formattedDate = `${day}/${month}/${year}`;
                                }
                            }
                            html += '<td>' + formattedDate + '</td>';
                            if (journal.LedgerDevName !== null) {
                                html += '<td>' + journal.LedgerDevName + '</td>';
                            }
                            else {
                                html += '<td>' + journal.LedgerName + '</td>';
                            }
                            html += '<td>' + journal.SubLedgerName + '</td>';
                            html += '<td>' + journal.Narration + '</td>';
                            html += '<td>' + journal.Amount + '</td>';
                            html += '<td>' + journal.DrCr + '</td>';
                            if (firstRow) {
                                html += '<td style="background-color:#ffe6e6;border-bottom: none;">';
                                html += '<button class="btn btn-primary btn-link btn-sm btn-Journal-edit"   id="btnJournalEdit_' + item.VoucherNo + '" data-id="' + item.VoucherNo + '" data-toggle="modal" data-target="#modal-edit-Product" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                                html += ' <button class="btn btn-primary btn-link btn-sm btn-Journal-delete" id="btnJournalDelete_' + item.VoucherNo + '"   data-id="' + item.VoucherNo + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                                html += '</td>';
                                html += '</tr >';
                                firstRow = false;
                            } else {
                                html += '<td style:"border-bottom: none;"></td>';
                            }
                            html += '</tr>';
                        })


                    });
                }
                else {
                    $('#loader').hide();
                    html += '<tr>';
                    html += '<td colspan="9">No record</td>';
                    html += '</tr>';
                }
                html += ' </tbody>';
                html += '</table >';
                $('.tblJournalList').html(html);
                if (!$.fn.DataTable.isDataTable('.JournalList')) {
                    $('.JournalList').DataTable({
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
    //---------------------JouranalEdit--------------------------//
    $(document).on('click', '.btn-Journal-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        GetJournalById(value);
        $('#tab-Journal').trigger('click');
        $('#btnSave').hide();
        $('#btnUpdate').show();
    });
    function GetJournalById(Id) {
        $.ajax({
            url: '/Accounting/GetJournalById?Id=' + Id + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                console.log(result);
                const ModifytransactionDate = result.GroupedLederwiseJournals[0].LederwiseJournals[0].Journals[0].VoucherDate;
                if (ModifytransactionDate) {
                    const dateObject = new Date(ModifytransactionDate);
                    if (!isNaN(dateObject)) {
                        const day = String(dateObject.getDate()).padStart(2, '0');
                        const month = String(dateObject.getMonth() + 1).padStart(2, '0');
                        const year = dateObject.getFullYear();
                        const formattedDate = `${day}/${month}/${year}`;
                        VoucherDate.val(formattedDate);
                    }
                }
                VoucherNo.val(result.GroupedLederwiseJournals[0].VoucherNo);
                Narration.val(result.GroupedLederwiseJournals[0].LederwiseJournals[0].Journals[0].Narration);
                // Journal Table  Clear
                JournalTable.clear().draw();
                $.each(result.GroupedLederwiseJournals, function (key, item) {
                    $.each(item.LederwiseJournals, function (key, item1) {
                        var uniqueId = 'ddlitem' + new Date().getTime();
                        $.ajax({
                            url: "/Accounting/GetLedgers",
                            type: "GET",
                            contentType: "application/json;charset=utf-8",
                            dataType: "json",
                            success: function (result) {
                                if (result.ResponseCode == 302) {
                                    var selectedLederName = "";
                                    var html = '<tr id="' + uniqueId + '" class="tr">';
                                    html += '<td style="width:10px">';
                                    html += '<div class="form-group">';
                                    html += '<select class="form-control select2bs4 mySelection" style = "width: 100%"  name = "BalanceType">';
                                   
                                    if (item1.Journals[0].DrCr.toLowerCase() == "cr") {
                                        html += '<option value="Dr" > DR </option>';
                                        html += '<option value ="Cr" selected = "selected"> CR </option>';
                                    } else {
                                        html += '<option value="Dr" selected = "selected" > DR </option>';
                                        html += '<option value ="Cr"> CR </option>';
                                    }
                                    
                                    html += '</select>';
                                    html += '</div>';
                                    html += '</td>';
                                    html += '<td style = "width:45px">';
                                    html += '<div class="form-group row">';
                                    html += '<div class="col-sm-6">';
                                    html += '<select class="select2bs4 ledgerType" style = "width: 100%;" data-target="additionalDropdown_' + uniqueId + '" name="ddlLedgerId">';
                                    html += '<option>--Select Option--</option>';
                                    $.each(result.Ledgers, function (key, Ledger) {
                                        var option = $('<option></option>').val(Ledger.LedgerId).text(Ledger.LedgerName);
                                        if (Ledger.LedgerId == item1.Fk_LedgerId) {
                                            option.attr('selected', 'selected');
                                            selectedLederName = Ledger.LedgerName;
                                        }
                                        html += option.prop('outerHTML');
                                    });
                                    html += '</select>';
                                    html += '</div>';
                                    html += '<label id= "LadgerCurBal_' + uniqueId + '" name="LadgerCurBal" class="col-sm-3 col-form-label" > Cur Bal: </label>';
                                    html += '</div>';
                                    html += '<div class="additionalDropdown" data-id="additionalDropdown_' + uniqueId + '"> </div>';
                                    html += '</td>';
                                    html += '<td style = "width:15px">';
                                    html += '<div class="form-group">';
                                    if (item1.Journals[0].DrCr.toLowerCase() == "cr") {
                                        html += '<input type="text" class="form-control"  id= "DrBalance_' + uniqueId + '" disabled name = "DrBalance">';
                                    } else {
                                        html += '<input type="text" class="form-control"  id= "DrBalance_' + uniqueId + '" name = "DrBalance">';
                                    }
                                    html += '</div>';
                                    html += '</td>';
                                    html += '<td style = "width:15px">';
                                    html += '<div class="form-group">';
                                    if (item1.Journals[0].DrCr.toLowerCase() == "cr") {
                                        html += '<input type="text" class="form-control" for="CrBalance_' + uniqueId + '"  id= "CrBalance_' + uniqueId + '" name = "CrBalance">';
                                    } else {
                                        html += '<input type="text" class="form-control" for="CrBalance_' + uniqueId + '"  id= "CrBalance_' + uniqueId + '" name = "CrBalance" disabled>';
                                    }
                                    html += '</div>';
                                    html += '</td>';
                                    html += '<td style = "width:15px">';
                                    html += ' <button class="btn btn-primary btn-link journalRemoveBtn" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                                    html += '</td>';
                                    html += '</tr>';
                                    var newRow = JournalTable.row.add($(html)).draw(false).node();
                                    //var newRow = $('#tblJournal tbody').append(html);
                                    $(newRow).find('.select2bs4').select2({
                                        theme: 'bootstrap4'
                                    });
                                    $.ajax({
                                        url: '/Master/GetLedgerBalances',
                                        type: "GET",
                                        contentType: "application/json;charset=utf-8",
                                        dataType: "json",
                                        success: function (result) {

                                            if (result.ResponseCode == 302) {
                                                $.each(result.LedgerBalances, function (index, bal) {
                                                    if (bal.Ledger.LedgerName === selectedLederName) {
                                                        var balance = "CurBal: " + Math.abs(bal.RunningBalance) + " " + bal.RunningBalanceType
                                                        $('#LadgerCurBal_' + uniqueId + '').text(balance);
                                                    }
                                                });
                                            }
                                        }
                                    });
                                    // Initialize sub-ledger dropdown
                                    if (item1.Journals && item1.Journals.length > 0) {
                                        $.each(item1.Journals, function (index, journal) {
                                            var subUniqueId = new Date().getTime();
                                            selectedOption = item1.Fk_LedgerId;
                                            if (journal.Fk_SubLedgerId != null) {
                                                $.ajax({
                                                    url: '/Accounting/GetSubLedgersById?LedgerId=' + item1.Fk_LedgerId,
                                                    type: "GET",
                                                    contentType: "application/json;charset=utf-8",
                                                    dataType: "json",
                                                    success: function (subResult) {
                                                        if (subResult.ResponseCode == 302) {
                                                            var SubladgerName = "";
                                                            var subHtml = '';
                                                            subHtml += '<div class="form-group row">';
                                                            subHtml += '<label name="SubLadgerCurBal" for="SubLadgerCurBal_' + subUniqueId + '" class="col-sm-2 col-form-label">Cur Bal: </label>';
                                                            subHtml += '<div class="col-sm-5">';
                                                            subHtml += '<select class="select2bs4 SubledgerType" style="width: 100%;" name="ddlSubledgerId">';
                                                            subHtml += '<option>--Select Option--</option>';
                                                            $.each(subResult.SubLedgers, function (key, subLedger) {
                                                                var option = $('<option></option>').val(subLedger.SubLedgerId).text(subLedger.SubLedgerName);
                                                                if (subLedger.SubLedgerId == journal.Fk_SubLedgerId) {
                                                                    option.attr('selected', 'selected');
                                                                    SubladgerName = subLedger.SubLedgerName;
                                                                }
                                                                subHtml += option.prop('outerHTML');
                                                            });
                                                            subHtml += '</select>';
                                                            subHtml += '</div>';
                                                            subHtml += '<div class="col-sm-3">';
                                                            subHtml += '<input type="text" class="form-control" name="SubledgerAmount" value="' + journal.Amount + '">';
                                                            subHtml += '</div>';
                                                            subHtml += '<div class="col-sm-2">';
                                                            subHtml += '<button class="btn btn-primary btn-link addSubLedgerBtn" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-plus"></i></button>';
                                                            subHtml += ' <button class="btn btn-primary btn-link deleteBtns" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                                                            subHtml += '</div>';
                                                            subHtml += '</div>';
                                                            $.ajax({
                                                                url: '/Master/GetSubLedgerBalances',
                                                                type: "GET",
                                                                contentType: "application/json;charset=utf-8",
                                                                dataType: "json",
                                                                success: function (result) {
                                                                    if (result.ResponseCode == 302) {
                                                                        $.each(result.SubLedgerBalances, function (index, subBal) {
                                                                            if (subBal.SubLedger.SubLedgerName.trim().toLowerCase() === SubladgerName.trim().toLowerCase()) {
                                                                                var bal = "CurBal: " + Math.abs(subBal.RunningBalance) + " " + subBal.RunningBalanceType
                                                                                $('label[for="SubLadgerCurBal_' + subUniqueId + '"]').text(bal);
                                                                            }
                                                                        });
                                                                    }
                                                                }
                                                            });
                                                            $('.additionalDropdown[data-id="additionalDropdown_' + uniqueId + '"]').append(subHtml);
                                                            $('.form-group').find('.select2bs4').select2({
                                                                theme: 'bootstrap4'
                                                            });
                                                            var totalAmount = 0;
                                                            $('.form-group').find('input[name="SubledgerAmount"]').each(function () {
                                                                var amount = parseFloat($(this).val()) || 0;
                                                                totalAmount += amount;
                                                            });
                                                           
                                                            if (journal.DrCr.toLowerCase() === "cr") {
                                                                $('#CrBalance_' + uniqueId + '').val(totalAmount);
                                                            } else {
                                                                $('#DrBalance_' + uniqueId + '').val(totalAmount);
                                                            }
                                                        }
                                                    },
                                                    error: function (errormessage) {
                                                        console.log(errormessage)
                                                    }
                                                });
                                            }
                                            else {
                                                if (journal.DrCr.toLowerCase() === "cr") {
                                                    $('#CrBalance_' + uniqueId + '').val(journal.Amount);
                                                } else {
                                                    $('#DrBalance_' + uniqueId + '').val(journal.Amount);
                                                }
                                            }
                                        });

                                    }
                                }
                            },
                            error: function (errormessage) {
                                console.log(errormessage)
                            }
                        });
                    });

                });
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
    //---------------------Update Jouranal--------------------------//
    $('#btnUpdate').on('click', Updatejournals);
    function Updatejournals() {
        if (!VoucherDate.val()) {
            toastr.error('VoucherDate Is Required.');
            return;
        } else if (!Narration.val()) {
            toastr.error('Narration Is Required.');
            return;
        } else {
            $('#loader').show();
            var requestData = {
                VoucherDate: VoucherDate.val(),
                VoucherNo: VoucherNo.val(),
                Narration: Narration.val(),
                "arr": []
            };
            $('tbody tr').each(function () {
                var row = $(this);
                var rowData = {
                    "BalanceType": row.find("select[name='BalanceType']").val(),
                    "ddlLedgerId": row.find("select[name='ddlLedgerId']").val(),
                    "DrBalance": row.find("input[name='DrBalance']").val(),
                    "CrBalance": row.find("input[name='CrBalance']").val(),
                    "subledgerData": []
                };
                if (rowData.ddlLedgerId) {
                    row.find(".additionalDropdown[data-id^='additionalDropdown_']").each(function () {
                        var subledgerRow = $(this);
                        var subledgerData = {
                            "ddlSubledgerId": [],
                            "SubledgerAmunt": []
                        };
                        subledgerRow.find("select[name='ddlSubledgerId']").each(function () {
                            subledgerData.ddlSubledgerId.push($(this).val());
                        });
                        subledgerRow.find("input[name='SubledgerAmount']").each(function () {
                            subledgerData.SubledgerAmunt.push($(this).val());
                        });
                        if (subledgerData.ddlSubledgerId.length > 0 || subledgerData.SubledgerAmunt.length > 0) {
                            rowData.subledgerData.push(subledgerData);
                        }
                    });
                    requestData.arr.push(rowData);
                }
               
                
            });
            $.ajax({
                type: "POST",
                url: '/Accounting/UpdateJournal',
                dataType: 'json',
                data: JSON.stringify(requestData),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#loader').hide();
                    if (Response.ResponseCode == 201) {
                        toastr.success(Response.SuccessMsg);
                        GetPaymentVoucherNo();
                        PaymentTable.clear().draw();
                        Narration.val('');
                        VoucherDate.val('');
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
        }
    }
    //------------------------------Delete Journals---------//
    $(document).on('click', '.btn-Journal-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeleteJournalDetails(value);
    });
    function DeleteJournalDetails(Id) {
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
                    url: '/Accounting/DeleteJournal?id=' + Id + '',
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        if (result.ResponseCode == 200) {
                            toastr.success(result.SuccessMsg);
                            LoadJournals();
                            GetJournalVoucherNo();
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