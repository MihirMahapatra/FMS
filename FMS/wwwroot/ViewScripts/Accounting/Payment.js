$(function () {
    //Default Date 
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;
    $("#AccountingLink").addClass("active");
    $("#PaymentLink").addClass("active");
    $("#PaymentLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //-----------------------------------------Insert Button K--------------------------------------------//
    $('#addPaymentRowBtn').on('keydown', function (e) {
        if (e.key === 'Insert' || e.keyCode === 45) {
            $('#addPaymentRowBtn').click();
        }
    });
    $(document).on('keydown', function (e) {
        if (e.key === 'Insert' || e.keyCode === 45) {
            e.preventDefault();
            $('#addPaymentRowBtn').click();
        }
    });
    //----------------------------------------varible declaration-----------------------------------------//
    var PaymentTable = $('#tblPayment').DataTable({
        "paging": false,
        "lengthChange": false,
        "searching": false,
        "ordering": true,
        "info": false,
        "autoWidth": false,
        "responsive": true,
    });
    var CashBank = $('select[name="PaymentMode"]');
    var Bank = $('select[name="ddlBankId"]');
    var ChqNo = $('input[name="ChqNo"]');
    var ChqDate = $('input[name="ChqDate"]')
    const VoucherNo = $('input[name="VoucherNo"]');
    const VoucherDate = $('input[name="VoucherDate"]');
    VoucherDate.val(todayDate);
    const Narration = $('textarea[name="Narration"]');
    //-------------------------------------------------------------Payment --------------------------------------------------------//
    GetPaymentVoucherNo();
    function GetPaymentVoucherNo() {
        selectedOption = CashBank.val();
        $.ajax({
            url: '/Accounting/GetPaymentVoucherNo?CashBank=' + selectedOption + '',
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
    $(document).on('change', 'select[name="PaymentMode"]', function () {
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
                            GetPaymentVoucherNo();
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
            GetPaymentVoucherNo();
        }
    });
    $('#addPaymentRowBtn').on('click', addPaymentRowBtn);
    function addPaymentRowBtn() {
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
                    html += '<input type="text" class="form-control" id = "txtDrAmount" name = "DrBalance">';
                    html += '</div>';
                    html += '</td>';
                    html += '</tr>';
                    var newRow = PaymentTable.row.add($(html)).draw(false).node();
                    //var newRow = $('#tblPayment tbody').append(html);
                    
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
    var selectedOption = "";
    $(document).on('change', '.ledgerType', function () {
        selectedOption = $(this).val();
        if (selectedOption) {
            var dataTarget = $(this).data('target');
            $('.additionalDropdown[data-id="' + dataTarget + '"]').html('');
            var selectedName = $(this).find('option:selected').text();
            var uniqueId = new Date().getTime();
            var html = "";
            $.ajax({
                url: '/Accounting/GetSubLedgersById?LedgerId=' + selectedOption + '',
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.ResponseCode == 302) {
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
                        html += '<input type="text" class="form-control"  name="SubledgerAmunt">';
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
                                var bal = "CurBal: " + Math.abs(item.RunningBalance) + " " + item.RunningBalanceType
                                $('label[name="LadgerCurBal"]').text(bal);
                            }
                        });
                    }
                }
            });
        }
    });
    $(document).on('change', 'input[name="SubledgerAmunt"]', function () {
        var totalSum = 0;
        $('input[name="SubledgerAmunt"]').each(function () {
            var inputValue = parseFloat($(this).val());
            if (!isNaN(inputValue)) {
                totalSum += inputValue;
            }
        });
        $('input[name="DrBalance"]').val(totalSum);

    });
    $(document).on('change', '.SubledgerType', function () {
        var select = $(this);
        var tr = select.closest('div.form-group.row'); 
        selectedOptions = select.val();

        if (selectedOptions) {
            var uniqueId = select.closest('.form-group.row').find('label[for^="SubLadgerCurBal_"]').attr('for').split('_')[1];
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
        $(this).closest().remove();
        var row = PaymentTable.row($(this).closest('.form-group.row'));
        row.remove().draw();
    });
    $(document).on('click', '.addSubLedgerBtn', function () {
        var clickedButton = $(this);
        var uniqueId = new Date().getTime();
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
                    html += '<input type="text" class="form-control" name="SubledgerAmunt">';
                    html += '</div>';
                    html += '<div class="col-sm-2">';
                    html += '<button class="btn btn-primary btn-link addSubLedgerBtn" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-plus"></i></button>';
                    html += ' <button class="btn btn-primary btn-link deleteBtn" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
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
    $(document).on('click', '.deleteBtn', function () {
        $(this).closest('.form-group.row').remove();
    });
    $('#btnSave').on('click', CreatePayment);
    function CreatePayment() {
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
            $('#loader').show();
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
                    "DrBalance": row.find("input[name='DrBalance']").val(),
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
                url: '/Accounting/CreatePayment',
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

    //_______________________________________________Payment List________________________________________________________________________//

    $('a[href="#PaymentList"]').on('click', function () {
        LoadPayments();
    });
    function LoadPayments() {
        $('#loader').show();
        $('.PaymentList').empty();
        $.ajax({
            url: "/Accounting/GetPayments",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#loader').hide();
                var html = '';
                html += '<table class="table table-bordered table-hover text-center mt-1 PaymentList" style="width:100%">';
                html += '<thead>'
                html += '<tr>'
                html += '<th> VoucherNo</th>'
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
                    $.each(result.GroupedPayments, function (key, item) {
                        var firstRow = true;
                        $.each(item.Payments, function (key, Payment) {
                            html += '<tr>';
                            html += '<td>' + Payment.VouvherNo + '</td>';
                            let formattedDate = '';
                            const ModifyDate = Payment.VoucherDate;
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
                            if (Payment.LedgerDevName !== null) {
                                html += '<td>' + Payment.LedgerDevName + '</td>';
                            }
                            else {
                                html += '<td>' + Payment.LedgerName + '</td>';
                            }
                            html += '<td>' + Payment.SubLedgerName + '</td>';
                            html += '<td>' + Payment.Narration + '</td>';
                            html += '<td>' + Payment.Amount + '</td>';
                            html += '<td>' + Payment.DrCr + '</td>';
                            if (firstRow) {
                                html += '<td style="background-color:#ffe6e6;border-bottom: none;">';
                                html += '<button class="btn btn-primary btn-link btn-sm btn-payment-edit"   id="btnPaymentEdit_' + item.VoucherNo + '" data-id="' + item.VoucherNo + '" data-toggle="modal" data-target="#modal-edit-Product" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                                html += ' <button class="btn btn-primary btn-link btn-sm btn-payment-delete" id="btnPaymentDelete_' + item.VoucherNo + '"   data-id="' + item.VoucherNo + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
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
                $('.tblPaymentList').html(html);

                if (!$.fn.DataTable.isDataTable('.PaymentList')) {
                    $('.PaymentList').DataTable({
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

//-----------------------EditPayments-----------------------------------//
    $(document).on('click', '.btn-payment-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        GetPaymemtById(value);
        $('#tab-Payment').trigger('click');
        $('#btnSave').hide();
        $('#btnUpdate').show();
    });
    function GetPaymemtById(Id) {
        $.ajax({
            url: '/Accounting/GetPaymentById?Id=' + Id + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                console.log(result);
                const ModifytransactionDate = result.GroupedLederwisePayments[0].LederwisePayments[0].Payments[0].VoucherDate;
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
                VoucherNo.val(result.GroupedLederwisePayments[0].VoucherNo);
                Narration.val(result.GroupedLederwisePayments[0].LederwisePayments[0].Payments[0].Narration);
                if (result.GroupedLederwisePayments[0].LederwisePayments[0].Payments[0].CashBank == 'Bank') {
                    CashBank.val('Bank');
                    CashBank.trigger('change.select2');
                    $('.hdndiv').show();
                    $.ajax({
                        url: '/Accounting/GetBankLedgers',
                        type: "GET",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (result1) {
                            Bank.empty();
                            var defaultOption = $('<option></option>').val('').text('--Select Option--');
                            Bank.append(defaultOption);
                            if (result1.ResponseCode == 302) {
                                $.each(result1.Ledgers, function (key, item) {
                                    var option = $('<option></option>').val(item.LedgerId).text(item.LedgerName);
                                    if (item.LedgerId == result.GroupedLederwisePayments[0].LederwisePayments[0].Payments[0].CashBankLedgerId) {
                                        option.attr('selected', 'selected');
                                    }
                                    Bank.append(option);
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
                    CashBank.val('Cash');
                }
                // Clear the table
                PaymentTable.clear().draw();
                var uniqueId = 0;
                result.GroupedLederwisePayments.forEach(function (group) {
                    group.LederwisePayments.forEach(function (ledger) {
                        var html = ''; // Build HTML for ledger dropdowns and associated subledgers
                        var ledgerDropdownAdded = false; // Flag to track ledger dropdown addition
                        ledger.Payments.forEach(function (payment) {
                            if (!ledgerDropdownAdded) {
                                // Add ledger dropdown if not added yet
                                html += '<tr>';
                                html += '<td style="width:60px">';
                                html += '<div class="form-group row">';
                                html += '<div class="col-sm-7">';
                                html += '<select class="select2bs4 form-control ledgerType" style="width: 100%;" data-target="additionalDropdown_' + uniqueId + '" name="ddlLedgerId">';
                                html += '<option>--Select Option--</option>';

                                $.ajax({
                                    url: "/Accounting/GetLedgers",
                                    type: "GET",
                                    contentType: "application/json;charset=utf-8",
                                    dataType: "json",
                                    success: function (result1) {
                                        if (result1.ResponseCode == 302) {
                                            $.each(result1.Ledgers, function (key, item1) {
                                                var option = $('<option></option>').val(item1.LedgerId).text(item1.LedgerName);
                                                if (item1.LedgerId == ledger.Fk_LedgerId) {
                                                    option.attr('selected', 'selected');
                                                }
                                                $('.ledgerType').append(option);
                                            });
                                        }
                                    },
                                });

                                // Close ledger dropdown
                                html += '</select>';
                                html += '</div>';
                                html += '<label name="LadgerCurBal" class="col-sm-3 col-form-label">Cur Bal:</label>';
                                html += '</div>';
                                // Adding container for additional dropdowns
                                html += '<div class="additionalDropdowns" data-id="additionalDropdown_' + uniqueId + '"></div>';
                                // Increment uniqueId for the next set of additionalDropdowns
                                uniqueId++;
                                // Set the flag to true since ledger dropdown has been added
                                ledgerDropdownAdded = true;
                            }

                            // Add subledger dropdowns for each payment
                            html += '<div class="form-group row">';
                            html += '<div class="col-sm-2">';
                            html += '<label name="SubLadgerCurBal" class="col-form-label">Cur Bal: </label>';
                            html += '</div>';
                            html += '<div class="col-sm-5">';
                            html += '<select class="select2bs4 form-control SubledgerType" style="width: 100%;" name="ddlSubledgerId">';
                            html += '<option>--Select Option--</option>';

                            // Populate subledger dropdown
                            $.ajax({
                                url: '/Accounting/GetSubLedgersById?LedgerId=' + ledger.Fk_LedgerId,
                                type: "GET",
                                contentType: "application/json;charset=utf-8",
                                dataType: "json",
                                success: function (result2) {
                                    if (result2.ResponseCode == 302) {
                                        var subledgerDropdown = $('.SubledgerType'); // Select the appropriate subledger dropdown
                                        subledgerDropdown.empty(); // Clear existing options before appending new ones
                                        $.each(result2.SubLedgers, function (key, item2) {
                                            var option = $('<option></option>').val(item2.SubLedgerId).text(item2.SubLedgerName);
                                            if (item2.SubLedgerId == payment.Fk_SubLedgerId) {
                                                option.attr('selected', 'selected');
                                            }
                                            subledgerDropdown.append(option); // Append the option to the subledger dropdown
                                        });
                                    } else {
                                        console.error('Failed to fetch subledgers');
                                    }
                                },
                                error: function (xhr, status, error) {
                                    console.error('AJAX error:', error);
                                }
                            });

                            html += '</select>';
                            html += '</div>';
                            html += '<div class="col-sm-3">';
                            html += '<input type="text" class="form-control" name="SubledgerAmount" value="' + payment.Amount + '">';
                            html += '</div>';
                            html += '<div class="col-sm-2">';
                            html += '<button class="btn btn-primary btn-link addSubLedgerBtn" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-plus"></i></button>';
                            html += ' <button class="btn btn-primary btn-link deleteBtns" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                            html += '</div>';
                            html += '</div>';
                        });

                        // Close the table row
                        html += '</td>';
                        html += '<td style="width:15px">';
                        html += '<div class="form-group">';
                        html += '<input type="text" class="form-control" id="txtDrAmount" name="DrBalance">';
                        html += '</div>';
                        html += '</td>';
                        html += '</tr>';

                        // Append generated HTML to PaymentTable
                        var newRow = PaymentTable.row.add($(html)).draw(false).node();
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
    //------------------------------Delete Payments---------//
    $(document).on('click', '.btn-payment-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeletePaymentsDetails(value);
    });
    function DeletePaymentsDetails(Id) {
        
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
                    url: '/Accounting/DeletePayment?id=' + Id + '',
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        if (result.ResponseCode == 200) {
                            toastr.success(result.SuccessMsg);
                            LoadPayments();
                            GetPaymentVoucherNo();
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