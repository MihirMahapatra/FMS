$(function () {
    //Default Date 
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;
    $("#AccountingLink").addClass("active");
    $("#ReceiptLink").addClass("active");
    $("#ReceiptLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //-----------------------------------------Insert Button K--------------------------------------------//
    $('#addReceiptRowBtn').on('keydown', function (e) {
        if (e.key === 'Insert' || e.keyCode === 45) {
            $('#addReceiptRowBtn').click();
        }
    });
    $(document).on('keydown', function (e) {
        if (e.key === 'Insert' || e.keyCode === 45) {
            e.preventDefault();
            $('#addReceiptRowBtn').click();
        }
    });
    //----------------------------------------varible declaration-----------------------------------------//
    var ReciptTable = $('#tblReceipt').DataTable({
        "paging": false,
        "lengthChange": false,
        "searching": false,
        "ordering": true,
        "info": false,
        "autoWidth": false,
        "responsive": true,
    });
    var CashBank = $('select[name="ReceiptMode"]');
    var Bank = $('select[name="ddlBankId"]');
    var ChqNo = $('input[name="ChqNo"]');
    var ChqDate = $('input[name="ChqDate"]')
    const VoucherNo = $('input[name="VoucherNo"]');
    const VoucherDate = $('input[name="VoucherDate"]');
    VoucherDate.val(todayDate);
    const Narration = $('textarea[name="Narration"]');
    //-------------------------------------------------------------Recipts --------------------------------------------------------//
    GetReceiptVoucherNo();
    function GetReceiptVoucherNo() {
        selectedOption = CashBank.val();
        $.ajax({
            url: '/Accounting/GetReceiptVoucherNo?CashBank=' + selectedOption + '',
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
    $(document).on('change', 'select[name="ReceiptMode"]', function () {
        selectedOption = CashBank.val();
        if (selectedOption === 'Bank') {
            $.ajax({
                url: '/Accounting/GetBankLedgers',
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    Bank.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    Bank.append(defaultOption);
                    if (result.ResponseCode == 302) {
                        $.each(result.Ledgers, function (key, item) {
                            var option = $('<option></option>').val(item.LedgerId).text(item.LedgerName);
                            Bank.append(option);
                            $('.hdndiv').show();
                            GetReceiptVoucherNo();
                        });
                    }
                },
                error: function (errormessage) {
                    console.log(errormessage)
                }
            })
        }
        else {
            $('.hdndiv').hide();
            GetReceiptVoucherNo();
        }
    });
    $('#addReceiptRowBtn').on('click', function () {
        var uniqueId = 'ddlitem' + new Date().getTime();
        $.ajax({
            url: "/Accounting/GetLedgers",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    var html = '<tr>';
                    html += '<td style = "width:60px">';
                    html += '<div class="form-group row">';
                    html += '<div class="col-sm-9">';
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
                    html += '<input type="text" class="form-control" id = "txtCrAmount" name = "CrBalance">';
                    html += '</div>';
                    html += '</td>';
                    html += '</tr>';
                    var newRow = ReciptTable.row.add($(html)).draw(false).node();
                    $(newRow).find('.select2bs4').select2({
                        theme: 'bootstrap4'
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    });
    var selectedOption = "";
    $(document).on('change', '.ledgerType', function () {
        selectedOption = $(this).val();
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
                        html += '<label name="SubLadgerCurBal" class="col-sm-2 col-form-label">Cur Bal: </label>';
                        html += '<div class="col-sm-5" >';
                        html += '<select class= "select2bs4 SubledgerType"  style = "width: 100%;" name="ddlSubledgerId">';
                        html += '<option>--Select Option--</option>';
                        $.each(result.SubLedgers, function (key, item) {
                            html += '<option value=' + item.SubLedgerId + '>' + item.SubLedgerName + ' </option>';
                        });
                        html += '</select>';
                        html += '</div>';
                        html += '<div class="col-sm-3" >';
                        html += '<input type="text" class="form-control"  name="SubledgerAmunt">';
                        html += '</div>';
                        html += '<div class="col-sm-2">';
                        html += '<button class="btn btn-primary btn-link addSubLedgerBtn" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-plus"></i></button>';
                        html += ' <button class="btn btn-primary btn-link deleteBtn" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                        html += '</div>';
                        html += '</div>';
                        $('.additionalDropdown[data-id="' + dataTarget + '"]').html(html);
                        $('.form-group').find('.select2bs4').select2({
                            theme: 'bootstrap4'
                        });
                    }
                    $(document).on('change', 'input[name="SubledgerAmunt"]', function () {
                        var totalSum = 0;
                        $('input[name="SubledgerAmunt"]').each(function () {
                            var inputValue = parseFloat($(this).val());
                            if (!isNaN(inputValue)) {
                                totalSum += inputValue;
                            }
                        });
                        $('input[name="CrBalance"]').val(totalSum);

                    });
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
                                var bal = "CurBal: " + Math.abs(item.RunningBalance) + " " + item.RunningBalanceType
                                $('label[name="LadgerCurBal"]').text(bal);
                            }
                        });
                    }
                }
            });
        }
    });
    var selectedOptions = "";
    $(document).on('change', '.SubledgerType', function () {
        selectedOptions = $(this).val();
        if (selectedOptions) {
            var dataTarget = $(this).data('target');
            $('.additionalDropdown[data-id="' + dataTarget + '"]').html('');
            var selectedName = $(this).find('option:selected').text();
            $.ajax({
                url: '/Master/GetSubLedgerBalances',
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.ResponseCode == 302) {
                        console.log(result.SubLedgerBalances);
                        console.log(selectedName);
                        $.each(result.SubLedgerBalances, function (index, item) {
                            if (item.SubLedger.SubLedgerName.trim().toLowerCase() === selectedName.trim().toLowerCase()) {
                                var bal = "CurBal: " + Math.abs(item.RunningBalance) + " " + item.RunningBalanceType
                                $('label[name="SubLadgerCurBal"]').text(bal);
                                console.log(bal);
                            }
                        });
                    }
                }
            });
            //
        }
    });
    $(document).on('click', '.deleteBtn', function () {
        $(this).closest('.form-group.row').remove();
    });
    $(document).on('click', '.addSubLedgerBtn', function () {
        var clickedButton = $(this);
        $.ajax({
            url: '/Accounting/GetSubLedgersById?LedgerId=' + selectedOption + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    var html = "";
                    html += '<div class="form-group row">';
                    html += '<label name="SubLadgerCurBal" class="col-sm-2 col-form-label">Cur Bal: </label>';
                    html += '<div class="col-sm-5" >';
                    html += '<select class= "select2bs4"  style = "width: 100%;" name="ddlSubledgerId">';
                    html += '<option>--Select Option--</option>';
                    $.each(result.SubLedgers, function (key, item) {
                        html += '<option value=' + item.SubLedgerId + '>' + item.SubLedgerName + ' </option>';
                    });
                    html += '</select>';
                    html += '</div>';
                    html += '<div class="col-sm-3" >';
                    html += '<input type="text" class="form-control" name="SubledgerAmunt">';
                    html += '</div>';
                    html += '<div class="col-sm-2">';
                    html += '<button class="btn btn-primary btn-link addSubLedgerBtn" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-plus"></i></button>';
                    html += ' <button class="btn btn-primary btn-link deleteBtns" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
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
    $(document).on('click', '.deleteBtns', function () {
        $(this).closest('.form-group.row').remove();
    });
    $('#btnSave').on('click', function () {
        if (CashBank.val() === "Bank") {
            if (!Bank.val() || Bank.val() === '--Select Option--') {
                toastr.error('Bank  Is Required.');
                return;
            } else if (!ChqNo.val()) {
                toastr.error('ChqNo Is Required.');
                return;
            } else if (!ChqDate.val()) {
                toastr.error('ChqDate Is Required.');
                return;
            }
        }
        if (!VoucherDate.val()) {
            toastr.error('VoucherDate Is Required.');
            return;
        } else if (!Narration.val()) {
            toastr.error('Narration Is Required.');
            return;
        } else {
            var requestData = {
                CashBank: CashBank.val(),
                BankLedgerId: Bank.val(),
                ChqNo: ChqNo.val(),
                ChqDate: ChqDate.val(),
                VoucherDate: VoucherDate.val(),
                VoucherNo: VoucherNo.val(),
                Narration: Narration.val(),
                "arr": []
            };
            $('tbody tr').each(function () {
                var row = $(this);
                var rowData = {
                    "ddlLedgerId": row.find("select[name='ddlLedgerId']").val(),
                    "CrBalance": row.find("input[name='CrBalance']").val(),
                    "subledgerData": []
                };
                row.find(".additionalDropdown[data-id^='additionalDropdown_']").each(function () {
                    var subledgerRow = $(this);
                    var subledgerData = {
                        "ddlSubledgerId": [],
                        "SubledgerAmunt": []
                    };
                    subledgerRow.find("select[name='ddlSubledgerId']").each(function () {
                        subledgerData.ddlSubledgerId.push($(this).val());
                    });
                    subledgerRow.find("input[name='SubledgerAmunt']").each(function () {
                        subledgerData.SubledgerAmunt.push($(this).val());
                    });

                    if (subledgerData.ddlSubledgerId.length > 0 || subledgerData.SubledgerAmunt.length > 0) {
                        rowData.subledgerData.push(subledgerData);
                    }
                });
                requestData.arr.push(rowData);
            });
            console.log(requestData);
            $.ajax({
                type: "POST",
                url: '/Accounting/CreateRecipt',
                dataType: 'json',
                data: JSON.stringify(requestData),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    if (Response.ResponseCode == 201) {
                        toastr.success(Response.SuccessMsg);
                        GetReceiptVoucherNo();
                        ReciptTable.clear().draw();
                        Narration.val('');
                        VoucherDate.val('');
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
    });
    //_______________________________________________Receipt List________________________________________________________________________//

    $('a[href="#ReceiptList"]').on('click', function () {
        LoadReceipts();
    });
    function LoadReceipts() {
        $('#loader').show();
        $('.ReceiptList').empty();
        $.ajax({
            url: "/Accounting/GetReceipts",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {

                var html = '';
                html += '<table class="table table-bordered table-hover text-center mt-1 ReceiptList" style="width:100%">';
                html += '<thead>'
                html += '<tr>'
                html += '<th> Voucher No</th>'
                html += '<th>Date</th>'
                html += '<th>Ledger</th>'
                html += '<th>SubLedger</th>'
                html += '<th>narration</th>'
                html += '<th>Amount</th>'
                html += '<th>DrCr</th>'
                html += '<th>Action</th>'
                html += '</tr>'
                html += '</thead>'
                html += '<tbody>';
                if (result.ResponseCode == 302) {
                    $('#loader').hide();
                    $.each(result.GroupedReceipts, function (key, item) {
                        var firstRow = true;
                        $.each(item.Receipts, function (key, Receipt) {
                            html += '<tr>';
                            html += '<td>' + Receipt.VouvherNo + '</td>';
                            let formattedDate = '';
                            const ModifyDate = Receipt.VoucherDate;
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
                            if (item.LedgerDevName !== null) {
                                html += '<td>' + Receipt.LedgerDevName + '</td>';
                            }
                            else {
                                html += '<td>' + Receipt.LedgerName + '</td>';
                            }
                            html += '<td>' + Receipt.SubLedgerName + '</td>';
                            html += '<td>' + Receipt.narration + '</td>';
                            html += '<td>' + Receipt.Amount + '</td>';
                            html += '<td>' + Receipt.DrCr + '</td>';
                            if (firstRow) {
                                html += '<td style="background-color:#ffe6e6;border-bottom: none;">';
                                html += '<button class="btn btn-primary btn-link btn-sm btn-receipt-edit"   id="btnreceiptEdit_' + item.VoucherNo + '" data-id="' + item.VoucherNo + '" data-toggle="modal" data-target="#modal-edit-Product" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                                html += ' <button class="btn btn-primary btn-link btn-sm btn-receipt-delete" id="btnreceiptDelete_' + item.VoucherNo + '"   data-id="' + item.VoucherNo + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
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
                $('.tblReceiptList').html(html);
                if (!$.fn.DataTable.isDataTable('.ReceiptList')) {
                    $('.ReceiptList').DataTable({
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

    //------------------------------Delete Payments---------//
    $(document).on('click', '.btn-receipt-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeleteReciptssDetails(value);
    });
    function DeleteReciptssDetails(Id) {
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
                    url: '/Accounting/DeleteReceipt?id=' + Id + '',
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        if (result.ResponseCode == 200) {
                            toastr.success(result.SuccessMsg);
                            LoadReceipts();
                            GetReceiptVoucherNo();
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