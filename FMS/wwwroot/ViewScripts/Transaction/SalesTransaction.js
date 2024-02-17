$(function () {
    $("#TransactionLink").addClass("active");
    $("#SalesTransactionLink").addClass("active");
    $("#SalesTransactionLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //----------------------------------------varible declaration-----------------------------------------//
    //default date
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;
    var GstTable = $('#tblGstdifference');
    var SalesTable = $('#tblSales').DataTable({
        "paging": false,
        "lengthChange": false,
        "searching": false,
        "ordering": true,
        "info": false,
        "autoWidth": false,
        "responsive": true,
        lengthMenu: [5, 10, 25, 50], // Set the available page lengths
        pageLength: 10 // Set the default page length to 5
    });
    const ddlPayment = $('select[name="ddlPayment"]');
    const ddlRate = $('select[name="ddlRate"]');
    const CustomerName = $('input[name="CustomerName"]');
    const transactionNo = $('input[name="TransactionNo"]');
    const SalesOrderId = $('input[name="hdnSaleOrderId"]');
    const transactionDate = $('input[name="TransactionDate"]');
    transactionDate.val(todayDate);
    const ddlCustomer = $('select[name="ddlCustomerId"]');
    const orderNo = $('input[name="OrderNo"]');
    const orderDate = $('input[name="OrderDate"]');
    orderDate.val(todayDate);
    const subTotal = $('input[name="SubTotal"]');
    const discount = $('input[name="TotalDiscountAmount"]');
    const gst = $('input[name="GstAmount"]');
    const vehicleNo = $('input[name="VehicleNo"]');
    const transpoterName = $('input[name="TranspoterName"]');
    const grandTotal = $('input[name="GrandTotal"]');
    const Narration = $('textarea[name="NarrationSales"]')
    //---------------------------------Contorl Foucous Of Element Sale-------------------------------//
    ddlPayment.focus();
    CustomerName.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    CustomerName.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    orderNo.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    orderNo.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    orderDate.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    orderDate.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    vehicleNo.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    vehicleNo.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    transpoterName.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    transpoterName.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });

    var chkPage = false;
    $('a[href="#CreateSale"]').on('click', function () {
        chkPage = false;
    });
    $(document).on('keydown', function (e) {
        if (e.key === 'Insert' || e.keyCode === 45) {
            if (!chkPage) {
                e.preventDefault();
                $('#addSalesRowBtn').click();
            }
        }
    });
    $('#addSalesRowBtn').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('#addSalesRowBtn').on('blur', function () {
        $(this).css('background-color', '');
    });
    $('#btnSave').on('keydown', function (e) {
        if (e.key === 'Enter' || e.keyCode === 13) {
            $('#btnSave').click();
        }
    });
    $('#btnSave').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('#btnSave').on('blur', function () {
        $(this).css('background-color', '');
    });
    $('#btnUpdate').on('keydown', function (e) {
        if (e.key === 'Enter' || e.keyCode === 13) {
            $('#btnUpdate').click();
        }
    });
    $('#btnUpdate').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('#btnUpdate').on('blur', function () {
        $(this).css('background-color', '');
    });
    //--------------------------------------- sales validation-------------------------------------//
    CustomerName.on("input", function () {
        let inputValue = $(this).val();
        inputValue = inputValue.toUpperCase();
        $(this).val(inputValue);
    });
    CustomerName.on('keydown', function (event) {
        const keyCode = event.keyCode || event.which;
        if (keyCode >= 48 && keyCode <= 57 || (keyCode >= 96 && keyCode <= 111)) {
            event.preventDefault();
        }
    });
    transpoterName.on('keydown', function (event) {
        const keyCode = event.keyCode || event.which;
        if (keyCode >= 48 && keyCode <= 57 || (keyCode >= 96 && keyCode <= 111)) {
            event.preventDefault();
        }
        else {
            let inputValue = $(this).val();
            inputValue = inputValue.toUpperCase();
            $(this).val(inputValue);
        }
    });

    orderNo.on("input", function () {
        let inputValue = $(this).val();
        inputValue = inputValue.toUpperCase();
        $(this).val(inputValue);
    });
    vehicleNo.on("input", function () {
        let inputValue = $(this).val();
        inputValue = inputValue.toUpperCase();
        $(this).val(inputValue);
    });
    //----------------------------------------Sale Return---------------------------------------------- //
    const Sr_ddlPayment = $('select[name="Sr_ddlPayment"]');
    const Sr_ddlRate = $('select[name="Sr_ddlRate"]');
    const Sr_CustomerName = $('input[name="Sr_CustomerName"]');
    const Sr_ddlCustomer = $('select[name="Sr_ddlCustomerId"]');
    const Sr_OrderId = $('input[name="hdnSaleReturnOrderId"]');
    const Sr_transactionDate = $('input[name="Sr_TransactionDate"]');
    Sr_transactionDate.val(todayDate);
    const Sr_transactionNo = $('input[name="Sr_TransactionNo"]');
    const Sr_orderNo = $('input[name="Sr_OrderNo"]');
    const Sr_orderDate = $('input[name="Sr_OrderDate"]');
    Sr_orderDate.val(todayDate);
    const Sr_subTotal = $('input[name="Sr_SubTotal"]');
    const Sr_discount = $('input[name="Sr_DiscountAmount"]');
    const Sr_grandTotal = $('input[name="Sr_GrandTotal"]');
    const Sr_vehicleNo = $('input[name="Sr_VehicleNo"]');
    const Sr_transpoterName = $('input[name="Sr_TranspoterName"]');
    const Sr_receivingPerson = $('input[name="Sr_ReceivingPerson"]');
    const Sr_gst = $('input[name="Sr_GstAmount"]');
    const Sr_Narration = $('textarea[name="NarrationSalesReturn"]');
    var SalesReturnTable = $('#tblSalesReturn').DataTable({
        "paging": false,
        "lengthChange": false,
        "searching": false,
        "ordering": true,
        "info": false,
        "autoWidth": false,
        "responsive": true,
        lengthMenu: [5, 10, 25, 50], // Set the available page lengths
        pageLength: 10,// Set the default page length to 5
    });
    //-------------------------------Contorl Foucous Of Element Sale Return--------------------------------//
    Sr_ddlPayment.focus();
    Sr_CustomerName.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    Sr_CustomerName.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    Sr_orderNo.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    Sr_orderNo.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    Sr_orderDate.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    Sr_orderDate.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    Sr_vehicleNo.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    Sr_vehicleNo.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    Sr_transpoterName.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    Sr_transpoterName.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    Sr_receivingPerson.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    Sr_receivingPerson.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    $('a[href="#CreateSalesReturn"]').on('click', function () {
        chkPage = true;
    });
    $(document).on('keydown', function (e) {
        if (e.key === 'Insert' || e.keyCode === 45) {
            if (chkPage) {
                e.preventDefault();
                $('#addSalesReturnRowBtn').click();
            }
        }
    });
    $('#addSalesReturnRowBtn').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('#addSalesReturnRowBtn').on('blur', function () {
        $(this).css('background-color', '');
    });
    $('#Sr_btnSave').on('keydown', function (e) {
        if (e.key === 'Enter' || e.keyCode === 13) {
            $('#Sr_btnSave').click();
        }
    });
    $('#Sr_btnSave').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('#Sr_btnSave').on('blur', function () {
        $(this).css('background-color', '');
    });
    $('#Sr_btnUpdate').on('keydown', function (e) {
        if (e.key === 'Enter' || e.keyCode === 13) {
            $('#Sr_btnUpdate').click();
        }
    });
    $('#Sr_btnUpdate').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('#Sr_btnUpdate').on('blur', function () {
        $(this).css('background-color', '');
    });

    //--------------------------------------- sales Return validation-------------------------------------//
    Sr_CustomerName.on('keydown', function (event) {
        const keyCode = event.keyCode || event.which;
        if (keyCode >= 48 && keyCode <= 57 || (keyCode >= 96 && keyCode <= 111)) {
            event.preventDefault();
        }
        else {
            let inputValue = $(this).val();
            inputValue = inputValue.toUpperCase();
            $(this).val(inputValue);
        }
    });
    Sr_orderNo.on("input", function () {
        let inputValue = $(this).val();
        inputValue = inputValue.toUpperCase();
        $(this).val(inputValue);
    });
    Sr_vehicleNo.on("input", function () {
        let inputValue = $(this).val();
        inputValue = inputValue.toUpperCase();
        $(this).val(inputValue);
    });
    Sr_transpoterName.on('keydown', function (event) {
        const keyCode = event.keyCode || event.which;
        if (keyCode >= 48 && keyCode <= 57 || (keyCode >= 96 && keyCode <= 111)) {
            event.preventDefault();
        }
        else {
            let inputValue = $(this).val();
            inputValue = inputValue.toUpperCase();
            $(this).val(inputValue);
        }
    });
    Sr_receivingPerson.on('keydown', function (event) {
        const keyCode = event.keyCode || event.which;
        if (keyCode >= 48 && keyCode <= 57 || (keyCode >= 96 && keyCode <= 111)) {
            event.preventDefault();
        }
        else {
            let inputValue = $(this).val();
            inputValue = inputValue.toUpperCase();
            $(this).val(inputValue);
        }
    });
    //------------------------------------------Sales Screen-----------------------------------------//
    GetLastSalesTransaction();
    GetSalesType()
    function GetSalesType() {
        $.ajax({
            url: "/Transaction/GetSalesType",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $.each(result, function (key, item) {
                    var option = $('<option></option>').val(key).text(item);
                    ddlPayment.append(option);
                });
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    GetRateType();
    function GetRateType() {
        $.ajax({
            url: "/Transaction/GetRateType",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $.each(result, function (key, item) {
                    var option = $('<option></option>').val(key).text(item);
                    ddlRate.append(option);
                });
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    function GetLastSalesTransaction() {
        $.ajax({
            url: "/Transaction/GetLastSalesTransaction",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                transactionNo.val(result.Data);
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    function GetSundryDebtors() {
        $.ajax({
            url: "/Transaction/GetSundryDebtors",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddlCustomer.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlCustomer.append(defaultOption);
                    $.each(result.SubLedgers, function (key, item) {
                        var option = $('<option></option>').val(item.SubLedgerId).text(item.SubLedgerName);
                        ddlCustomer.append(option);
                    });
                }
                else {
                    ddlCustomer.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlCustomer.append(defaultOption);
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    $(document).on('change', 'select[name="ddlPayment"]', function () {
        selectedOption = ddlPayment.val();
        if (selectedOption === 'credit') {
            $('.hdnddn').show();
            $('.hdntxt').hide();
            GetSundryDebtors();
        } else {
            $('.hdnddn').hide();
            $('.hdntxt').show();
        }
    });
    $(document).on('change', 'select[name = "ddlRate"]', function () {
        selectedOption = ddlRate.val();
        if (selectedOption === 'wholesalerate') {
            $('.rate').prop('disabled', true);
        } else {
            $('.rate').prop('disabled', false);
        }
    });
    $('#addSalesRowBtn').click(function () {
        var uniqueId = 'ddlitem' + new Date().getTime();
        var html = '<tr>';
        html += '<td hidden><input type="hidden" class="form-control" value="0"></td>';
        html += '<td><div class="form-group"><select class="form-control form-control-sm select2bs4 FinishedGood" style="width: 100%;" id="' + uniqueId + '"></select></div></td>';
        html += '<td style="width:8%"><div class="form-group"><input type="text" id="txtAlternateQty" class="form-control" value="0"></div></td>';
        html += '<td style="width:12%">' +
            '<div class="form-group">' +
            '<select class="form-control form-control select2bs4 selectedUnit" style="width:100%" disabled></select>' +
            '</div>' +
            '</td>';
        html += '<td>' +
            '<div class="form-group">' +
            '<div class="input-group">' +
            '<input type="text" class="form-control" value="0">' +
            ' <div class="input-group-append">' +
            ' <span class="input-group-text" id="Unit">N/A</span>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</td>';
        if (ddlRate.val() === 'wholesalerate') {
            html += '<td><div class="form-group"><input type="text" class="form-control rate" value="0" disabled></div></td>';
        }
        else {
            html += '<td><div class="form-group"><input type="text" class="form-control rate" value="0"></div></td>';
        }
        html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
        html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
        html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
        html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
        html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
        html += '<td><button class="btn btn-primary btn-link deleteBtn" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button></td>';
        html += '</tr>';
        SalesTable.row.add($(html)).draw(false).node();
        $.ajax({
            url: "/Transaction/GetProductFinishedGood",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    var selectElement = $('#' + uniqueId);
                    selectElement.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    selectElement.append(defaultOption);
                    $.each(result.Products, function (key, item) {
                        var option = $('<option></option>').val(item.ProductId).text(item.ProductName);
                        selectElement.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
        $('#tblSales tbody').find('.select2bs4').select2({
            theme: 'bootstrap4'
        });
    });
    $(document).on('click', '.deleteBtn', function () {
        $(this).closest('tr').remove();
    });
    $(document).on('change', '.FinishedGood', function () {
        var selectElement = $(this);
        var selectedProductId = selectElement.val();
        var rateType = ddlRate.val();
        if (selectedProductId) {
            $.ajax({
                url: '/Transaction/GetProductGstWithRate?id=' + selectedProductId + '&&RateType=' + rateType + ' ',
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.ResponseCode == 302) {
                        var Textbox = selectElement.closest('tr').find('input[type="text"]');
                        for (var i = 0; i < 7; i++) {
                            Textbox.eq(i).val('0');
                        }
                        Textbox.eq(4).val(result.Product.GST);
                        Textbox.eq(1).val(result.Product.Price);
                        var span = selectElement.closest('tr').find('span#Unit');
                        span.text(result.Product.Unit.UnitName);
                    }
                },
                error: function (errormessage) {
                    console.log(errormessage);
                }
            });
            $.ajax({
                url: '/Transaction/GetProductAlternateUnit?ProductId=' + selectedProductId,
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var selectBox = selectElement.closest('tr').find('select').eq(1);
                    if (result.ResponseCode == 302) {
                        selectBox.empty();
                        var defaultOption = $('<option></option>').val('').text('--Select Option--');
                        selectBox.append(defaultOption);
                        $.each(result.AlternateUnits, function (key, item) {
                            var option = $('<option></option>').val(item.AlternateUnitId).text(item.AlternateUnitName);
                            selectBox.append(option);
                        });
                    }
                    else {
                        selectBox.empty();
                        var defaultOption = $('<option></option>').val('').text('--Select Option--');
                        selectBox.append(defaultOption);
                    }
                },
                error: function (errormessage) {
                    console.log(errormessage)
                }
            });
        }
       
    });
    $(document).on('change', '#txtAlternateQty', function () {
        var row = $(this).closest('tr');
        var alternateQty = $(this).val();
        var alternateUnitDropDown = row.find('select:eq(1)');
        parseFloat(alternateQty) !== 0 ? alternateUnitDropDown.prop('disabled', false) : alternateUnitDropDown.prop('disabled', true);
    });
    $(document).on('change', '.selectedUnit', function () {
        var row = $(this).closest('tr');
        var SelectedAlternateUnitId = $(this).val();
        if (SelectedAlternateUnitId) {
            $.ajax({
                url: '/Transaction/GetAlternateUnitByAlternateUnitId?AlternateUnitId=' + SelectedAlternateUnitId,
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.ResponseCode == 302) {
                        var alternateUnitQty = row.find('input:eq(1)').val();
                        var TotalUnitQty = parseFloat(alternateUnitQty) !== 0 ? alternateUnitQty * result.AlternateUnit.UnitQuantity : result.AlternateUnit.UnitQuantity;
                        row.find('input:eq(2)').val(TotalUnitQty);
                        row.find('span#Unit').text(result.AlternateUnit.Unit.UnitName);
                    }
                },
                error: function (errormessage) {
                    console.log(errormessage);
                }
            });
        }
    });
    $('#tblSales tbody').on('change', 'input[type="text"]', function () {
        var row = $(this).closest('tr');
        var quantity = parseFloat(row.find('input:eq(1)').val());
        var rate = parseFloat(row.find('input:eq(3)').val());
        var discountPercentage = parseFloat(row.find('input:eq(4)').val());
        var GstPercentage = parseFloat(row.find('input:eq(6)').val());
        var amount = quantity * rate * (1 - discountPercentage / 100);
        var GstAmounts = amount * GstPercentage / 100;
        var AcctualAmount = amount + GstAmounts;
        var discountAmount = (discountPercentage > 0) ? (rate * quantity - amount) : 0;
        row.find('input:eq(8)').val(AcctualAmount.toFixed(2));
        row.find('input:eq(7)').val(GstAmounts.toFixed(2));
        row.find('input:eq(5)').val(discountAmount.toFixed(2));

        var totalAmount = 0;
        var toalSubTotalAmount = 0;
        var totalGstAmount = 0;
        var totalDiscountAmount = 0;
        $('#tblSales tbody tr').each(function () {
            var qty = parseFloat($(this).find('input:eq(1)').val());
            var rate = parseFloat($(this).find('input:eq(3)').val());
            var amount = parseFloat($(this).find('input:eq(8)').val());
            var GstAmount = parseFloat($(this).find('input:eq(7)').val());
            var DiscountAmount = parseFloat($(this).find('input:eq(5)').val());
            if (!isNaN(qty) && !isNaN(rate)) {
                toalSubTotalAmount += (qty * rate)
            }
            if (!isNaN(amount)) {
                totalAmount += amount;
            }
            if (!isNaN(GstAmount)) {
                totalGstAmount += GstAmount;
            }
            if (!isNaN(DiscountAmount)) {
                totalDiscountAmount += DiscountAmount;
            }
        });
        $('input[name="SubTotal"]').val(toalSubTotalAmount.toFixed(2));
        $('input[name="TotalDiscountAmount"]').val(totalDiscountAmount.toFixed(2));
        $('input[name="GstAmount"]').val(totalGstAmount.toFixed(2));
        $('input[name="GrandTotal"]').val(totalAmount.toFixed(2));
        const gstDifferences = {};
        $('#tblSales tbody tr').each(function () {
            const gstRate = parseFloat($(this).find('input:eq(6)').val());
            const amount = parseFloat($(this).find('input:eq(7)').val());
            if (!isNaN(gstRate) && !isNaN(amount)) {
                if (gstRate in gstDifferences) {
                    gstDifferences[gstRate] += amount;
                } else {
                    gstDifferences[gstRate] = amount;
                }
            }
        });
        const gstDifferenceBody = $('#tblGstdifference tbody');
        gstDifferenceBody.empty(); // Clear the table before adding the rows.
        for (const rate in gstDifferences) {
            const row = $('<tr><td>GST ' + rate + ' % Amount: ' + gstDifferences[rate].toFixed(2) + '</td></tr>');
            gstDifferenceBody.append(row);
        }
        // end //
    });
    $('#btnSave').on('click', CreateSale);
    function CreateSale() {
        if (!transactionDate.val()) {
            toastr.error('TransactionDate Is Required.');
            return;
        }
        if (ddlPayment.val() == 'cash' && (!CustomerName.val())) {
            toastr.error('Customer Name Is Required.');
            CustomerName.focus();
            return;
        }
        else if (ddlPayment.val() == 'credit' && !ddlCustomer.val() || ddlCustomer.val() === '--Select Option--') {
            toastr.error('Customer Name Is Required.');
            CustomerName.focus();
            return;
        }
        else if (!orderNo.val()) {
            toastr.error('orderNo Is Required.');
            orderNo.focus();
            return;
        } else if (!orderDate.val()) {
            toastr.error('orderDate Is Required.');
            orderDate.focus();
            return;
        } else if (!vehicleNo.val()) {
            toastr.error('VehicleNo Is Required.');
            vehicleNo.focus();
            return;
        }
        else if (!transpoterName.val()) {
            toastr.error('TranspoterName Is Required.');
            transpoterName.focus();
            return;
        }
        else {
            $('#loader').show();
            var rowData = [];
            $('#tblSales tbody tr').each(function () {
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
            var PrintData = [];
            $('#tblSales tbody tr').each(function () {
                var row = $(this);
                var PrintCelles = [];
                row.find('td').each(function () {
                    var cell = $(this);
                    var input = cell.find('input,select');
                    if (input.is('select')) {
                        value = input.find('option:selected').text();
                    } else {
                        value = input.val();
                    }
                    PrintCelles.push(value);
                });
                PrintData.push(PrintCelles);
            });
            var requestData = {
                TransactionType: ddlPayment.val(),
                RateType: ddlRate.val(),
                Fk_SubLedgerId: ddlCustomer.val(),
                CustomerName: CustomerName.val(),
                TransactionDate: transactionDate.val(),
                TransactionNo: transactionNo.val(),
                OrderNo: orderNo.val(),
                OrderDate: orderDate.val(),
                TranspoterName: transpoterName.val(),
                VehicleNo: vehicleNo.val(),
                SubTotal: subTotal.val(),
                Discount: discount.val(),
                Gst: gst.val(),
                GrandTotal: grandTotal.val(),
                Naration: Narration.val(),
                rowData: rowData,
                PrintData: PrintData
            };
            $.ajax({
                type: "POST",
                url: '/Transaction/CreateSale',
                dataType: 'json',
                data: JSON.stringify(requestData),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#loader').hide();
                    if (Response.ResponseCode == 201) {
                        toastr.success(Response.SuccessMsg);
                        SalesTable.clear().draw();
                        CustomerName.val('');
                        orderNo.val('');
                        orderDate.val('');
                        transpoterName.val('');
                        vehicleNo.val('');
                        subTotal.val('0');
                        discount.val('0');
                        gst.val('0');
                        grandTotal.val('0');
                        Narration.val('');
                        GetLastSalesTransaction();
                        GstTable.clear().draw();
                        $.ajax({
                            type: "POST",
                            url: '/Print/SalesPrintData',
                            dataType: 'json',
                            data: JSON.stringify(requestData),
                            contentType: "application/json;charset=utf-8",
                            success: function (Response) {
                                window.open(Response.redirectTo, '_blank');
                            },
                        });
                    }
                    else {
                        toastr.error(Response.ErrorMsg);
                        $('#loader').hide();
                    }
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }
    }
    //-----------------------------------------------------Sales List Scren --------------------------------------------------//
    $('a[href="#SaleList"]').on('click', function () {
        GetSalesList();
    });
    function GetSalesList() {
        $('#loader').show();
        $('.SalesListTable').empty();
        $.ajax({
            url: "/Transaction/GetSales",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#loader').hide();
                var html = '';
                html += '<table class="table table-bordered table-hover text-center mt-1 SalesListTable" style="width:100%">';
                html += '<thead>'
                html += '<tr>'
                html += '<th hidden>Sales OrderId</th>'
                html += '<th>Trxn No</th>'
                html += '<th>Trxn Dt.</th>'
                html += '<th>Party</th>'
                html += '<th>Trxn Type</th>'
                html += '<th>Challan No</th>'
                html += '<th>Grand Total</th>'
                html += '<th>Action</th>'
                html += '</tr>'
                html += '</thead>'
                html += '<tbody>';
                if (result.ResponseCode == 302) {
                    $.each(result.salesOrders, function (key, item) {
                        html += '<tr>';
                        html += '<td hidden>' + item.SalesOrderId + '</td>';
                        html += '<td>' + item.TransactionNo + '</td>';
                        const ModifyDate = item.TransactionDate;
                        var formattedDate = '';
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
                        if (item.SubLedger !== null) {
                            html += '<td>' + item.SubLedger.SubLedgerName + '</td>';
                        }
                        else {
                            html += '<td>' + item.CustomerName + '</td>';
                        }
                        html += '<td>' + item.TransactionType + '</td>';

                        html += '<td>' + item.OrderNo + '</td>';
                        html += '<td>' + item.GrandTotal + '</td>';
                        html += '<td style="background-color:#ffe6e6;">';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-sales-edit"   id="btnsalesEdit_' + item.SalesOrderId + '"     data-id="' + item.SalesOrderId + '" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-sales-delete" id="btnsalesDelete_' + item.SalesOrderId + '"   data-id="' + item.SalesOrderId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                        html += '</td>';
                        html += '</tr >';
                    });
                }
                else {
                    html += '<tr>';
                    html += '<td colspan="9">No Record</td>';
                    html += '</tr >';
                }
                html += ' </tbody>';
                html += '</table >';
                $('.tblSalesList').html(html);
                if (!$.fn.DataTable.isDataTable('.SalesListTable')) {
                    var table = $('.SalesListTable').DataTable({
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
                $('#loader').hide();
                Swal.fire(
                    'Error!',
                    'An error occurred',
                    'error'
                );
            }
        });
    }
    $(document).on('click', '.btn-sales-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        GetSalesById(value);
        $('#tab-CreateSale').trigger('click');
        $('#btnSave').hide();
        $('#btnUpdate').show();
    });
    function GetSalesById(Id) {
        $.ajax({
            url: '/Transaction/GetSalesById?Id=' + Id + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                SalesOrderId.val(result.salesOrder.SalesOrderId)
                const ModifytransactionDate = result.salesOrder.TransactionDate;
                if (ModifytransactionDate) {
                    const dateObject = new Date(ModifytransactionDate);
                    if (!isNaN(dateObject)) {
                        const day = String(dateObject.getDate()).padStart(2, '0');
                        const month = String(dateObject.getMonth() + 1).padStart(2, '0');
                        const year = dateObject.getFullYear();
                        const formattedDate = `${day}/${month}/${year}`;
                        transactionDate.val(formattedDate);
                    }
                }
                transactionNo.val(result.salesOrder.TransactionNo)
                $.ajax({
                    url: "/Transaction/GetSalesType",
                    type: "GET",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result2) {
                        ddlPayment.empty();
                        $.each(result2, function (key, item2) {
                            var option = $('<option></option>').val(key).text(item2);
                            if (key === result.salesOrder.TransactionType) {
                                option.attr('selected', 'selected');
                            }
                            ddlPayment.append(option);
                        });
                    },
                    error: function (errormessage) {
                        console.log(errormessage)
                    }
                });
                if (result.salesOrder.TransactionType === 'cash') {
                    $('.hdnddn').hide();
                    $('.hdntxt').show();
                    CustomerName.val(result.salesOrder.CustomerName);
                } else {
                    $('.hdnddn').show();
                    $('.hdntxt').hide();
                    $.ajax({
                        url: "/Transaction/GetSundryDebtors",
                        type: "GET",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (result1) {
                            if (result.ResponseCode == 302) {
                                ddlCustomer.empty();
                                var defaultOption = $('<option></option>').val('').text('--Select Option--');
                                ddlCustomer.append(defaultOption);
                                $.each(result1.SubLedgers, function (key, item1) {
                                    var option = $('<option></option>').val(item1.SubLedgerId).text(item1.SubLedgerName);
                                    if (result.salesOrder.Fk_SubLedgerId !== null) {
                                        if (item1.SubLedgerId === result.salesOrder.Fk_SubLedgerId) {
                                            option.attr('selected', 'selected');
                                        }
                                    }
                                    ddlCustomer.append(option);
                                });
                            }
                            else {
                                ddlCustomer.empty();
                                var defaultOption = $('<option></option>').val('').text('--Select Option--');
                                ddlCustomer.append(defaultOption);
                            }
                        },
                        error: function (errormessage) {
                            console.log(errormessage)
                        }
                    });
                }
                transpoterName.val(result.salesOrder.TranspoterName);
                Narration.val(result.salesOrder.Naration)
                vehicleNo.val(result.salesOrder.VehicleNo);
                const ModifyinvoiceDate = result.salesOrder.InvoiceDate;
                if (ModifyinvoiceDate) {
                    const dateObject = new Date(ModifyinvoiceDate);
                    if (!isNaN(dateObject)) {
                        const day = String(dateObject.getDate()).padStart(2, '0');
                        const month = String(dateObject.getMonth() + 1).padStart(2, '0');
                        const year = dateObject.getFullYear();
                        const formattedDate = `${day}/${month}/${year}`;
                    }
                }
                orderNo.val(result.salesOrder.OrderNo);
                ModifyorderDate = result.salesOrder.OrderDate;
                if (ModifyorderDate) {
                    const dateObject = new Date(ModifyorderDate);
                    if (!isNaN(dateObject)) {
                        const day = String(dateObject.getDate()).padStart(2, '0');
                        const month = String(dateObject.getMonth() + 1).padStart(2, '0');
                        const year = dateObject.getFullYear();
                        const formattedDate = `${day}/${month}/${year}`;
                        orderDate.val(formattedDate);
                    }
                }
                subTotal.val(result.salesOrder.SubTotal);
                discount.val(result.salesOrder.Discount);
                grandTotal.val(result.salesOrder.GrandTotal);
                gst.val(result.salesOrder.Gst);
                //Fill Table Data
                SalesTable.clear().draw();
                $.each(result.salesOrder.SalesTransactions, function (key, item) {
                    var html = '<tr>';
                    html += '<td  hidden><input type="hidden" class="form-control"  value=' + item.SalesId + '></td>';
                    html += '<td><div class="form-group"><select class="form-control form-control-sm select2bs4 FinishedGood" style="width: 100%;" id="ddn_' + item.SalesId + '"> </select></div></td>';
                    html += '<td style="width:8%"><div class="form-group"><input type="text" id="txtAlternateQty" class="form-control" value=' + item.AlternateQuantity + '></div></td>';
                    html += '<td style="width:12%">' +
                        '<div class="form-group">' +
                        '<select class="form-control form-control select2bs4 selectedUnit" style="width:100%" id="ddnUnit_' + item.SalesId + '" disabled></select>' +
                        '</div>' +
                        '</td>';
                    html += '<td>' +
                        '<div class="form-group">' +
                        '<div class="input-group">' +
                        '<input type="text" class="form-control" id="" value=' + item.UnitQuantity + '>' +
                        ' <div class="input-group-append">' +
                        ' <span class="input-group-text" id="Unit">' + item.UnitName + '</span>' +
                        '</div>' +
                        '</div>' +
                        '</div>' +
                        '</td>';
                    html += '<td><div class="form-group"><input type="text" class="form-control" id=""  value=' + item.Rate + '></div></td>';
                    html += '<td><div class="form-group"><input type="text" class="form-control" id=""  value=' + item.Discount + '></div></td>';
                    html += '<td><div class="form-group"><input type="text" class="form-control" id=""  value=' + item.DiscountAmount + '></div></td>';
                    html += '<td><div class="form-group"><input type="text" class="form-control" id=""  value=' + item.Gst + '></div></td>';
                    html += '<td><div class="form-group"><input type="text" class="form-control" id=""  value=' + item.GstAmount + '></div></td>';
                    html += '<td><div class="form-group"><input type="text" class="form-control" id=""  value=' + item.Amount + '></div></td>';
                    html += '<td><button class="btn btn-primary btn-link deleteBtn" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button></td>';
                    html += '</tr>';
                    var newRow = SalesTable.row.add($(html)).draw(false).node();
                    var selectElement = $('#ddn_' + item.SalesId);
                    selectElement.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    selectElement.append(defaultOption);
                    $.ajax({
                        url: "/Transaction/GetProductFinishedGood",
                        type: "GET",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (result3) {
                            if (result3.ResponseCode == 302) {
                                $.each(result3.Products, function (key, item3) {
                                    var option = $('<option></option>').val(item3.ProductId).text(item3.ProductName);
                                    if (item.Fk_ProductId === item3.ProductId) {
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
                    var selectUnitElement = $('#ddnUnit_' + item.SalesId);
                    selectUnitElement.empty();
                    var defaultUnitOption = $('<option></option>').val('').text('--Select Option--');
                    selectUnitElement.append(defaultUnitOption);
                    $.ajax({
                        url: '/Transaction/GetProductAlternateUnit?ProductId=' + item.Fk_ProductId,
                        type: "GET",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (result) {
                            if (result.ResponseCode == 302) {
                                $.each(result.AlternateUnits, function (key, item1) {
                                    var unitoption = $('<option></option>').val(item1.AlternateUnitId).text(item1.AlternateUnitName);
                                    if (item.Fk_AlternateUnitId === item1.AlternateUnitId) {
                                        unitoption.attr('selected', 'selected');
                                    }
                                    selectUnitElement.append(unitoption);
                                });
                            }
                        },
                        error: function (errormessage) {
                            console.log(errormessage)
                        }
                    });
                })
                $('#tblSales tbody').find('.select2bs4').select2({
                    theme: 'bootstrap4'
                });
                const gstDifferences = {};
                $('#tblSales tbody tr').each(function () {
                    const gstRate = parseFloat($(this).find('input:eq(5)').val());
                    const amount = parseFloat($(this).find('input:eq(7)').val());
                    if (!isNaN(gstRate) && !isNaN(amount)) {
                        if (gstRate in gstDifferences) {
                            gstDifferences[gstRate] += amount;
                        } else {
                            gstDifferences[gstRate] = amount;
                        }
                    }
                });
                const gstDifferenceBody = $('#tblGstdifference tbody');
                gstDifferenceBody.empty();
                for (const rate in gstDifferences) {
                    const row = $('<tr><td>GST ' + rate + ' % Amount: ' + gstDifferences[rate].toFixed(2) + '</td></tr>');
                    gstDifferenceBody.append(row);
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
    $('#btnUpdate').on('click', UpdateSales);
    function UpdateSales() {
        if (!transactionDate.val()) {
            toastr.error('TransactionDate Is Required.');
            transactionDate.focus();
            return;
        }
        if (ddlPayment.val() == 'cash' && (!CustomerName.val())) {
            toastr.error('Customer Name Is Required.');
            CustomerName.focus();
            return;
        }
        else if (ddlPayment.val() == 'credit' && !ddlCustomer.val() || ddlCustomer.val() === '--Select Option--') {
            toastr.error('Customer Name Is Required.');
            CustomerName.focus();
            return;
        } else if (!orderNo.val()) {
            toastr.error('orderNo Is Required.');
            orderNo.focus();
            return;
        } else if (!orderDate.val()) {
            toastr.error('orderDate Is Required.');
            orderDate.focus();
            return;
        } else if (!vehicleNo.val()) {
            toastr.error('VehicleNo Is Required.');
            vehicleNo.focus();
            return;
        }
        else if (!transpoterName.val()) {
            toastr.error('TranspoterName Is Required.');
            transpoterName.focus();
            return;
        } else {
            $('#loader').show();
            var rowData = [];
            $('#tblSales tbody tr').each(function () {
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
            var requestData = {
                SalesOrderId: SalesOrderId.val(),
                TransactionType: ddlPayment.val(),
                RateType: ddlRate.val(),
                TransactionDate: transactionDate.val(),
                TransactionNo: transactionNo.val(),
                Fk_SubLedgerId: ddlCustomer.val(),
                CustomerName: CustomerName.val(),
                OrderNo: orderNo.val(),
                OrderDate: orderDate.val(),
                TranspoterName: transpoterName.val(),
                VehicleNo: vehicleNo.val(),
                SubTotal: subTotal.val(),
                Discount: discount.val(),
                Gst: gst.val(),
                Naration: Narration.val(),
                GrandTotal: grandTotal.val(),
                rowData: rowData
            };
            $.ajax({
                type: "POST",
                url: '/Transaction/UpdateSales',
                dataType: 'json',
                data: JSON.stringify(requestData),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#loader').hide();
                    if (Response.ResponseCode == 200) {
                        toastr.success(Response.SuccessMsg);
                        SalesTable.clear().draw();
                        SalesOrderId.val('');
                        CustomerName.val('');
                        transactionNo.val('');
                        transactionDate.val('');
                        orderNo.val('');
                        orderDate.val('');
                        transpoterName.val('');
                        vehicleNo.val('');
                        subTotal.val('0');
                        discount.val('0');
                        Narration.val('');
                        gst.val('0');
                        grandTotal.val('0');
                        GetLastSalesTransaction();
                        GstTable.clear().draw();
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
    $(document).on('click', '.btn-sales-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeleteSalesRecord(value);
    });
    function DeleteSalesRecord(Id) {
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
                    url: '/Transaction/DeleteSales?id=' + Id + '',
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        if (result.ResponseCode == 200) {
                            toastr.success(result.SuccessMsg);
                        }
                        else {
                            toastr.error(result.ErrorMsg);
                        }
                        GetLastSalesTransaction();
                        GetSalesList();
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
            }
        });
    }
    //-------------------------------------------------------------Sales Return Screen --------------------------------------------------------//
    Sr_GetSundryDebtors();
    function Sr_GetSundryDebtors() {
        $.ajax({
            url: "/Transaction/GetSundryDebtors",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    Sr_ddlCustomer.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    Sr_ddlCustomer.append(defaultOption);
                    $.each(result.SubLedgers, function (key, item) {
                        var option = $('<option></option>').val(item.SubLedgerId).text(item.SubLedgerName);
                        Sr_ddlCustomer.append(option);
                    });
                }
                else {
                    Sr_ddlCustomer.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    Sr_ddlCustomer.append(defaultOption);
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    GetLastSalesReturnTransaction();
    function GetLastSalesReturnTransaction() {
        $.ajax({
            url: "/Transaction/GetLastSalesReturnTransaction",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                Sr_transactionNo.val(result.Data);
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    GetSalesReturnType();
    function GetSalesReturnType() {
        $.ajax({
            url: "/Transaction/GetSalesType",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $.each(result, function (key, item) {
                    var option = $('<option></option>').val(key).text(item);
                    Sr_ddlPayment.append(option);
                });

            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    GetSalesReturnRateType()
    function GetSalesReturnRateType() {
        $.ajax({
            url: "/Transaction/GetRateType",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $.each(result, function (key, item) {
                    var option = $('<option></option>').val(key).text(item);
                    Sr_ddlRate.append(option);
                });
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    $('#addSalesReturnRowBtn').on('click', SalesReturnRowBtn);
    function SalesReturnRowBtn() {
        var uniqueId = 'ddlitem' + new Date().getTime();
        var html = '<tr>';
        html += '<td hidden><input type="hidden" class="form-control" value="0"></td>';
        html += '<td><div class="form-group"><select class="form-control form-control-sm select2bs4 GoodsFinishedReturn" style="width: 100%;" id="' + uniqueId + '"></select></div></td>';
        html += '<td style="width:8%"><div class="form-group"><input type="text" class="form-control" id="Qtyrt" value="0"></div></td>';
        html += '<td style="width:12%">' +
            '<div class="form-group">' +
            '<select class="form-control form-control select2bs4 selectedUnitRtn" style="width:100%" disabled></select>' +
            '</div>' +
            '</td>';
        html += '<td>' +
            '<div class="form-group">' +
            '<div class="input-group">' +
            '<input type="text" class="form-control" value="0">' +
            ' <div class="input-group-append">' +
            ' <span class="input-group-text" id="Unitrtn">N/A</span>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</td>';
        if (Sr_ddlRate.val() === 'wholesalerate') {
            html += '<td><div class="form-group"><input type="text" class="form-control sr_rate" value="0" disabled></div></td>';
        }
        else {
            html += '<td><div class="form-group"><input type="text" class="form-control sr_rate" value="0"></div></td>';
        }
        html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
        html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
        html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
        html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
        html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
        html += '<td><button class="btn btn-primary btn-link deleteBtnReturn" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button></td>';
        html += '</tr>';
        var newRow = SalesReturnTable.row.add($(html)).draw(false).node();
        $.ajax({
            url: "/Transaction/GetProductFinishedGood",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    var selectElement = $('#' + uniqueId);
                    selectElement.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    selectElement.append(defaultOption);
                    $.each(result.Products, function (key, item) {
                        var option = $('<option></option>').val(item.ProductId).text(item.ProductName);
                        selectElement.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
        $('#tblSalesReturn tbody').find('.select2bs4').select2({
            theme: 'bootstrap4'
        });
    }
    $(document).on('change', '.GoodsFinishedReturn', function () {
        var selectElement = $(this);
        var selectedProductId = selectElement.val();
        var rateType = Sr_ddlRate.val();
        if (selectedProductId) {
            $.ajax({
                url: '/Transaction/GetProductGstWithRate?id=' + selectedProductId + '&&RateType=' + rateType + '',
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.ResponseCode == 302) {
                        var Textbox = selectElement.closest('tr').find('input[type="text"]');
                        for (var i = 0; i < 7; i++) {
                            Textbox.eq(i).val('0');
                        }
                        Textbox.eq(4).val(result.Product.GST);
                        Textbox.eq(1).val(result.Product.Price);
                        var span = selectElement.closest('tr').find('span#Unitrtn');
                        span.text(result.Product.Unit.UnitName);
                    }
                },
                error: function (errormessage) {
                    console.log(errormessage);
                }
            });
            $.ajax({
                url: '/Transaction/GetProductAlternateUnit?ProductId=' + selectedProductId,
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var selectBox = selectElement.closest('tr').find('select').eq(1);
                    if (result.ResponseCode == 302) {
                        selectBox.empty();
                        var defaultOption = $('<option></option>').val('').text('--Select Option--');
                        selectBox.append(defaultOption);
                        $.each(result.AlternateUnits, function (key, item) {
                            var option = $('<option></option>').val(item.AlternateUnitId).text(item.AlternateUnitName);
                            selectBox.append(option);
                        });
                    }
                    else {
                        selectBox.empty();
                        var defaultOption = $('<option></option>').val('').text('--Select Option--');
                        selectBox.append(defaultOption);
                    }
                },
                error: function (errormessage) {
                    console.log(errormessage)
                }
            });
        }
    });
    $(document).on('change', '#Qtyrt', function () {
        var row = $(this).closest('tr');
        var alternateQty = $(this).val();
        var alternateUnitDropDown = row.find('select:eq(1)');
        parseFloat(alternateQty) !== 0 ? alternateUnitDropDown.prop('disabled', false) : alternateUnitDropDown.prop('disabled', true);
    });
    $(document).on('change', '.selectedUnitRtn', function () {
        var selectElement = $(this);
        var SelectedAlternateUnitId = selectElement.val();
        if (SelectedAlternateUnitId) {
            $.ajax({
                url: '/Transaction/GetAlternateUnitByAlternateUnitId?AlternateUnitId=' + SelectedAlternateUnitId,
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.ResponseCode == 302) {
                        var Textbox = selectElement.closest('tr').find('input[type="text"]');
                        var qtyprElement = $('#Qtyrt');
                        var qty = qtyprElement.val();
                        TotalUnits = qty * result.AlternateUnit.UnitQuantity
                        Textbox.eq(1).val(TotalUnits);
                        var span = selectElement.closest('tr').find('span#Unit');
                        span.text(result.AlternateUnit.Unit.UnitName)
                    }
                },
                error: function (errormessage) {
                    console.log(errormessage);
                }
            });
        }
    });
    $(document).on('click', '.deleteBtnReturn', function () {
        $(this).closest('tr').remove();
    });
    $(document).on('change', 'select[name="Sr_ddlPayment"]', function () {
        selectedOption = Sr_ddlPayment.val();
        if (selectedOption === 'credit') {
            $('.Sr_hdnddn').show();
            $('.Sr_hdntxt').hide();
        } else {
            $('.Sr_hdnddn').hide();
            $('.Sr_hdntxt').show();
        }
    });
  
    $('#tblSalesReturn tbody').on('change', 'input[type="text"]', function () {
        var row = $(this).closest('tr');
        var quantity = parseFloat(row.find('input:eq(1)').val());
        var rate = parseFloat(row.find('input:eq(3)').val());
        var discountPercentage = parseFloat(row.find('input:eq(4)').val());
        var GstPercentage = parseFloat(row.find('input:eq(6)').val());
        var amount = quantity * rate * (1 - discountPercentage / 100);
        //var GstAmounts = amount * GstPercentage / (100 + GstPercentage);
        var GstAmounts = amount * GstPercentage / 100;
        var acctualAmount = amount + GstAmounts;
        var discountAmount = (discountPercentage > 0) ? (rate * quantity - amount) : 0;
        row.find('input:eq(8)').val(acctualAmount.toFixed(2));
        row.find('input:eq(7)').val(GstAmounts.toFixed(2));
        row.find('input:eq(5)').val(discountAmount);
        var totalAmount = 0;
        var toalSubTotalAmount = 0;
        var totalGstAmount = 0;
        var totalDiscountAmount = 0;
        $('#tblSalesReturn tbody tr').each(function () {
            var qty = parseFloat($(this).find('input:eq(1)').val());
            var rate = parseFloat($(this).find('input:eq(3)').val());
            var amount = parseFloat($(this).find('input:eq(8)').val());
            var GstAmount = parseFloat($(this).find('input:eq(7)').val());
            var DiscountAmount = parseFloat($(this).find('input:eq(5)').val());
            if (!isNaN(qty) && !isNaN(rate)) {
                toalSubTotalAmount += (qty * rate)
            }
            if (!isNaN(amount)) {
                totalAmount += amount;
            }
            if (!isNaN(GstAmount)) {
                totalGstAmount += GstAmount;
            }
            if (!isNaN(DiscountAmount)) {
                totalDiscountAmount += DiscountAmount;
            }
        });
        $('input[name="Sr_SubTotal"]').val(toalSubTotalAmount.toFixed(2));
        $('input[name="Sr_GrandTotal"]').val(totalAmount.toFixed(2));
        $('input[name="Sr_GstAmount"]').val(totalGstAmount.toFixed(2));
        $('input[name="Sr_DiscountAmount"]').val(totalDiscountAmount.toFixed(2));
        const gstDifferences = {};
        $('#tblSalesReturn tbody tr').each(function () {
            const gstRate = parseFloat($(this).find('input:eq(6)').val()); // Assuming GST rate is in the 6th input field (0-based index).
            const amount = parseFloat($(this).find('input:eq(7)').val());   // Assuming the amount is in the 8th input field (0-based index).

            if (!isNaN(gstRate) && !isNaN(amount)) {
                if (gstRate in gstDifferences) {
                    gstDifferences[gstRate] += amount;
                } else {
                    gstDifferences[gstRate] = amount;
                }
            }
        });
        const gstDifferenceBody = $('#tblGstdifferencert tbody');
        gstDifferenceBody.empty(); // Clear the table before adding the rows.
        for (const rate in gstDifferences) {
            const row = $('<tr><td>GST ' + rate + ' % Amount: ' + gstDifferences[rate].toFixed(2) + '</td></tr>');
            gstDifferenceBody.append(row);
        }
        // end //
    });
    $('#Sr_btnSave').on('click', CreateSalesReturn);
    function CreateSalesReturn() {
        if (!Sr_transactionDate.val()) {
            toastr.error('TransactionDate Is Required.');
            Sr_transactionDate.focus();
            return;
        }
        if (Sr_ddlPayment.val() == 'cash' && (!Sr_CustomerName.val())) {
            toastr.error('Customer Name Is Required.');
            Sr_CustomerName.focus();
            return;
        }
        else if (Sr_ddlPayment.val() == 'credit' && !Sr_ddlCustomer.val() || Sr_ddlCustomer.val() === '--Select Option--') {
            toastr.error('Customer Name Is Required.');
            Sr_CustomerName.focus();
            return;
        }
        else if (!Sr_orderNo.val()) {
            toastr.error('orderNo Is Required.');
            Sr_orderNo.focus();
            return;
        }
        else if (!Sr_orderDate.val()) {
            toastr.error('orderDate Is Required.');
            Sr_orderDate.focus();
            return;
        } else if (!Sr_vehicleNo.val()) {
            toastr.error('VehicleNo Is Required.');
            Sr_vehicleNo.focus();
            return;
        }
        else if (!Sr_transpoterName.val()) {
            toastr.error('TranspoterName Is Required.');
            Sr_transpoterName.focus();
            return;
        } else if (!Sr_receivingPerson.val()) {
            toastr.error('ReceivingPerson Is Required.');
            Sr_receivingPerson.focus();
            return;
        }
        else {
            $('#loader').show();
            var rowData = [];
            $('#tblSalesReturn tbody tr').each(function () {
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

            var requestData = {
                TransactionType: Sr_ddlPayment.val(),
                RateType: Sr_ddlRate.val(),
                Fk_SubLedgerId: Sr_ddlCustomer.val(),
                CustomerName: Sr_CustomerName.val(),
                TransactionDate: Sr_transactionDate.val(),
                TransactionNo: Sr_transactionNo.val(),
                OrderNo: Sr_orderNo.val(),
                OrderDate: Sr_orderDate.val(),
                SubTotal: Sr_subTotal.val(),
                Discount: Sr_discount.val(),
                GrandTotal: Sr_grandTotal.val(),
                Gst: Sr_gst.val(),
                TranspoterName: Sr_transpoterName.val(),
                VehicleNo: Sr_vehicleNo.val(),
                Naration: Sr_Narration.val(),
                ReceivingPerson: Sr_receivingPerson.val(),
                rowData: rowData
            };
            $.ajax({
                type: "POST",
                url: '/Transaction/CreateSalesReturn',
                dataType: 'json',
                data: JSON.stringify(requestData),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#loader').hide();
                    if (Response.ResponseCode == 201) {
                        toastr.success(Response.SuccessMsg);
                        SalesReturnTable.clear().draw();
                        Sr_CustomerName.val('');
                        Sr_orderNo.val('');
                        Sr_orderDate.val('');
                        Sr_subTotal.val('0');
                        Sr_discount.val('0');
                        Sr_gst.val('0');
                        Sr_grandTotal.val('0');
                        Sr_transpoterName.val('');
                        Sr_vehicleNo.val('');
                        Sr_receivingPerson.val('');
                        Sr_Narration.val('');
                        GetLastSalesReturnTransaction();
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
    //_________________________________________Sales Return  List Screen ____________________________//
    $('a[href="#SalesReturnList"]').on('click', function () {
        GetSalesReturnList();
    });
    function GetSalesReturnList() {
        $('#loader').show();
        $('.SalesReturnListTable').empty();
        $.ajax({
            url: "/Transaction/GetSalesReturns",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {

                $('#loader').hide();
                var html = '';
                html += '<table class="table table-bordered table-hover text-center mt-1 SalesReturnListTable" style="width:100%">';
                html += '<thead>'
                html += '<tr>'
                html += '<th hidden>Sales OrderId</th>'
                html += '<th>Trxn No</th>'
                html += '<th>Trxn Dt.</th>'
                html += '<th>Party</th>'
                html += '<th>Mat.ReceiptNo</th>'
                html += '<th>Grand Total</th>'
                html += '<th>Action</th>'
                html += '</tr>'
                html += '</thead>'
                html += '<tbody>';
                if (result.ResponseCode == 302) {
                    $.each(result.SalesReturnOrders, function (key, item) {
                        html += '<tr>';
                        html += '<td hidden>' + item.SalesReturnOrderId + '</td>';
                        html += '<td>' + item.TransactionNo + '</td>';
                        const ModifyDate = item.TransactionDate;
                        var formattedDate = '';
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
                        if (item.SubLedger !== null) {
                            html += '<td>' + item.SubLedger.SubLedgerName + '</td>';
                        }
                        else {
                            html += '<td>' + item.CustomerName + '</td>';
                        }
                        html += '<td>' + item.OrderNo + '</td>';
                        html += '<td>' + item.GrandTotal + '</td>';
                        html += '<td style="background-color:#ffe6e6;">';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-salesReturn-edit"   id="btnsalesReturnEdit_' + item.SalesReturnOrderId + '"     data-id="' + item.SalesReturnOrderId + '" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-salesReturn-delete" id="btnsalesReturnDelete_' + item.SalesReturnOrderId + '"   data-id="' + item.SalesReturnOrderId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
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
                $('.tblSalesReturnList').html(html);
                if (!$.fn.DataTable.isDataTable('.SalesReturnListTable')) {
                    var table = $('.SalesReturnListTable').DataTable({
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
                $('#loader').hide();
                Swal.fire(
                    'Error!',
                    'An error occurred',
                    'error'
                );
            }
        });
    }
    $(document).on('click', '.btn-salesReturn-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        GetSalesReturnById(value);
        $('#tab-CreateSalesReturn').trigger('click');
        $('#Sr_btnSave').hide();
        $('#Sr_btnUpdate').show();
    });
    function GetSalesReturnById(Id) {
        $.ajax({
            url: '/Transaction/GetSalesReturnById?Id=' + Id + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                console.log(result);
                Sr_OrderId.val(result.SalesReturnOrder.SalesReturnOrderId)
                const ModifytransactionDates = result.SalesReturnOrder.TransactionDate;
                if (ModifytransactionDates) {
                    const dateObject = new Date(ModifytransactionDates);
                    if (!isNaN(dateObject)) {
                        const day = String(dateObject.getDate()).padStart(2, '0');
                        const month = String(dateObject.getMonth() + 1).padStart(2, '0');
                        const year = dateObject.getFullYear();
                        const formattedDate = `${day}/${month}/${year}`;
                        Sr_transactionDate.val(formattedDate);
                    }
                }
                Sr_transactionNo.val(result.SalesReturnOrder.TransactionNo)
                $.ajax({
                    url: "/Transaction/GetSalesType",
                    type: "GET",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result2) {
                        ddlPayment.empty();
                        $.each(result2, function (key, item2) {
                            var option = $('<option></option>').val(key).text(item2);
                            if (key === result.SalesReturnOrder.TransactionType) {
                                option.attr('selected', 'selected');
                            }
                            Sr_ddlPayment.append(option);
                        });
                    },
                    error: function (errormessage) {
                        console.log(errormessage)
                    }
                });
                if (result.SalesReturnOrder.TransactionType === 'cash') {
                    $('.Sr_hdnddn').hide();
                    $('.Sr_hdntxt').show();
                    Sr_CustomerName.val(result.SalesReturnOrder.CustomerName);
                } else {
                    $('.Sr_hdnddn').show();
                    $('.Sr_hdntxt').hide();
                    $.ajax({
                        url: "/Transaction/GetSundryDebtors",
                        type: "GET",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (result1) {
                            if (result.ResponseCode == 302) {
                                Sr_ddlCustomer.empty();
                                var defaultOption = $('<option></option>').val('').text('--Select Option--');
                                Sr_ddlCustomer.append(defaultOption);
                                $.each(result1.SubLedgers, function (key, item1) {
                                    var option = $('<option></option>').val(item1.SubLedgerId).text(item1.SubLedgerName);
                                    if (item1.SubLedgerId === result.SalesReturnOrder.Fk_SubLedgerId) {
                                        option.attr('selected', 'selected');
                                    }
                                    Sr_ddlCustomer.append(option);
                                });
                            }
                            else {
                                Sr_ddlCustomer.empty();
                                var defaultOption = $('<option></option>').val('').text('--Select Option--');
                                Sr_ddlCustomer.append(defaultOption);
                            }
                        },
                        error: function (errormessage) {
                            console.log(errormessage)
                        }
                    });
                }
                $.ajax({
                    url: "/Transaction/GetSalesType",
                    type: "GET",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result2) {
                        Sr_ddlPayment.empty();
                        $.each(result2, function (key, item2) {
                            var option = $('<option></option>').val(key).text(item2);
                            if (key === result.SalesReturnOrder.TransactionType) {
                                option.attr('selected', 'selected');
                            }
                            Sr_ddlPayment.append(option);
                        });
                    },
                    error: function (errormessage) {
                        console.log(errormessage)
                    }
                });
                Sr_transpoterName.val(result.SalesReturnOrder.TranspoterName);
                Sr_Narration.val(result.SalesReturnOrder.Naration);
                Sr_vehicleNo.val(result.SalesReturnOrder.VehicleNo);
                Sr_orderNo.val(result.SalesReturnOrder.OrderNo);
                Sr_receivingPerson.val(result.SalesReturnOrder.ReceivingPerson);
                ModifyorderDate = result.SalesReturnOrder.OrderDate;
                if (ModifyorderDate) {
                    const dateObject = new Date(ModifyorderDate);
                    if (!isNaN(dateObject)) {
                        const day = String(dateObject.getDate()).padStart(2, '0');
                        const month = String(dateObject.getMonth() + 1).padStart(2, '0');
                        const year = dateObject.getFullYear();
                        const formattedDate = `${day}/${month}/${year}`;
                        Sr_orderDate.val(formattedDate);
                    }
                }
                Sr_subTotal.val(result.SalesReturnOrder.SubTotal);
                Sr_discount.val(result.SalesReturnOrder.Discount);
                Sr_gst.val(result.SalesReturnOrder.Discount)
                Sr_grandTotal.val(result.SalesReturnOrder.GrandTotal);
                Sr_gst.val(result.SalesReturnOrder.Gst);
                SalesReturnTable.clear().draw();
                $.each(result.SalesReturnOrder.SalesReturnTransactions, function (key, item) {
                    var html = '<tr>';
                    html += '<td  hidden><input type="hidden" class="form-control"  value=' + item.SalesReturnId + '></td>';
                    html += '<td><div class="form-group"><select class="form-control form-control-sm select2bs4" style="width: 100%;" id="ddn_' + item.SalesReturnId + '"> </select></div></td>';
                    html += '<td style="width:8%"><div class="form-group"><input type="text" class="form-control" id="Qtyrt" value=' + item.AlternateQuantity + '></div></td>';
                    html += '<td style="width:12%">' +
                        '<div class="form-group">' +
                        '<select class="form-control form-control select2bs4 selectedUnitRtn" style="width:100%" id="ddnUnit_' + item.SalesReturnId + '" disabled></select>' +
                        '</div>' +
                        '</td>';
                    html += '<td>' +
                        '<div class="form-group">' +
                        '<div class="input-group">' +
                        '<input type="text" class="form-control" value=' + item.UnitQuantity + '>' +
                        ' <div class="input-group-append">' +
                        ' <span class="input-group-text" id="Unitrtn" value="N/A">' + item.UnitName + '</span>' +
                        '</div>' +
                        '</div>' +
                        '</div>' +
                        '</td>';
                    html += '<td><div class="form-group"><input type="text" class="form-control" id=""  value=' + item.Rate + '></div></td>';
                    html += '<td><div class="form-group"><input type="text" class="form-control" id=""  value=' + item.Discount + '></div></td>';
                    html += '<td><div class="form-group"><input type="text" class="form-control" id=""  value=' + item.DiscountAmount + '></div></td>';
                    html += '<td><div class="form-group"><input type="text" class="form-control" id=""  value=' + item.Gst + '></div></td>';
                    html += '<td><div class="form-group"><input type="text" class="form-control" id=""  value=' + item.GstAmount + '></div></td>';
                    html += '<td><div class="form-group"><input type="text" class="form-control" id=""  value=' + item.Amount + '></div></td>';
                    html += '<td><button class="btn btn-primary btn-link deleteBtnrt" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button></td>';
                    html += '</tr>';
                    var newRow = SalesReturnTable.row.add($(html)).draw(false).node();
                    var selectElement = $('#ddn_' + item.SalesReturnId);
                    selectElement.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    selectElement.append(defaultOption);
                    $.ajax({
                        url: "/Transaction/GetProductFinishedGood",
                        type: "GET",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (result3) {
                            if (result3.ResponseCode == 302) {
                                $.each(result3.Products, function (key, item3) {
                                    var option = $('<option></option>').val(item3.ProductId).text(item3.ProductName);
                                    if (item.Fk_ProductId === item3.ProductId) {
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
                    var selectUnitElement = $('#ddnUnit_' + item.SalesReturnId);
                    selectUnitElement.empty();
                    var defaultUnitOption = $('<option></option>').val('').text('--Select Option--');
                    selectUnitElement.append(defaultUnitOption);
                    $.ajax({
                        url: '/Transaction/GetProductAlternateUnit?ProductId=' + item.Fk_ProductId,
                        type: "GET",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (result) {
                            if (result.ResponseCode == 302) {
                                $.each(result.AlternateUnits, function (key, item1) {
                                    var unitoption = $('<option></option>').val(item1.AlternateUnitId).text(item1.AlternateUnitName);
                                    if (item.Fk_AlternateUnitId === item1.AlternateUnitId) {
                                        unitoption.attr('selected', 'selected');
                                    }
                                    selectUnitElement.append(unitoption);
                                });
                            }
                        },
                        error: function (errormessage) {
                            console.log(errormessage)
                        }
                    });
                })
                $('#tblSalesReturn tbody').find('.select2bs4').select2({
                    theme: 'bootstrap4'
                });
                const gstDifferences = {};
                $('#tblSalesReturn tbody tr').each(function () {
                    const gstRate = parseFloat($(this).find('input:eq(5)').val());
                    const amount = parseFloat($(this).find('input:eq(7)').val());
                    if (!isNaN(gstRate) && !isNaN(amount)) {
                        if (gstRate in gstDifferences) {
                            gstDifferences[gstRate] += amount;
                        } else {
                            gstDifferences[gstRate] = amount;
                        }
                    }
                });
                const gstDifferenceBody = $('#tblGstdifferencert tbody');
                gstDifferenceBody.empty();
                for (const rate in gstDifferences) {
                    const row = $('<tr><td>GST ' + rate + ' % Amount: ' + gstDifferences[rate].toFixed(2) + '</td></tr>');
                    gstDifferenceBody.append(row);
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
    $('#Sr_btnUpdate').on('click', Sr_UpdateSales);
    function Sr_UpdateSales() {
        if (!Sr_transactionDate.val()) {
            toastr.error('TransactionDate Is Required.');
            Sr_transactionDate.focus();
            return;
        }
        if (Sr_ddlPayment.val() == 'cash' && (!Sr_CustomerName.val())) {
            toastr.error('Customer Name Is Required.');
            Sr_CustomerName.focus();
            return;
        }
        else if (Sr_ddlPayment.val() == 'credit' && !Sr_ddlCustomer.val() || Sr_ddlCustomer.val() === '--Select Option--') {
            toastr.error('Customer Name Is Required.');
            CustomerName.focus();
            return;
        }
        else if (!Sr_orderNo.val()) {
            toastr.error('orderNo Is Required.');
            Sr_orderNo.focus();
            return;
        } else if (!Sr_orderDate.val()) {
            toastr.error('orderDate Is Required.');
            Sr_orderDate.focus();
            return;
        } else if (!Sr_vehicleNo.val()) {
            toastr.error('VehicleNo Is Required.');
            Sr_vehicleNo.focus();
            return;
        }
        else if (!Sr_transpoterName.val()) {
            toastr.error('TranspoterName Is Required.');
            Sr_transpoterName.focus();
            return;
        } else if (!Sr_receivingPerson.val()) {
            toastr.error('ReceivingPerson Is Required.');
            Sr_receivingPerson();
            return;
        }
        else {
            $('#loader').show();
            var rowData = [];
            $('#tblSalesReturn tbody tr').each(function () {
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
            var requestData = {
                SalesRetunOrderId: Sr_OrderId.val(),
                TransactionType: Sr_ddlPayment.val(),
                RateType: Sr_ddlRate.val(),
                TransactionDate: Sr_transactionDate.val(),
                TransactionNo: Sr_transactionNo.val(),
                CustomerName: Sr_CustomerName.val(),
                Fk_SubLedgerId: Sr_ddlCustomer.val(),
                OrderNo: Sr_orderNo.val(),
                OrderDate: Sr_orderDate.val(),
                SubTotal: Sr_subTotal.val(),
                Discount: Sr_discount.val(),
                GrandTotal: Sr_grandTotal.val(),
                Gst: Sr_gst.val(),
                TranspoterName: Sr_transpoterName.val(),
                VehicleNo: Sr_vehicleNo.val(),
                Naration: Sr_Narration.val(),
                ReceivingPerson: Sr_receivingPerson.val(),
                rowData: rowData
            };
            $.ajax({
                type: "POST",
                url: '/Transaction/UpdateSalesReturn',
                dataType: 'json',
                data: JSON.stringify(requestData),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#loader').hide();
                    if (Response.ResponseCode == 200) {
                        toastr.success(Response.SuccessMsg);
                        SalesReturnTable.clear().draw();
                        Sr_OrderId.val('');
                        Sr_transactionNo.val('');
                        Sr_transactionDate.val('');
                        Sr_receivingPerson.val('');
                        Sr_transpoterName.val('');
                        Sr_vehicleNo.val('');
                        Sr_orderNo.val('');
                        Sr_orderDate.val('');
                        Sr_Narration.val('');
                        Sr_subTotal.val('0');
                        Sr_discount.val('0');
                        Sr_grandTotal.val('0');
                        GetLastSalesReturnTransaction();
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
    $(document).on('click', '.btn-salesReturn-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeleteSalesReturnRecord(value);
    });
    function DeleteSalesReturnRecord(Id) {
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
                    url: '/Transaction/DeleteSalesReturn?id=' + Id + '',
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        if (result.ResponseCode == 200) {
                            toastr.success(result.SuccessMsg);
                            GetSalesReturnList();
                            GetLastSalesReturnTransaction();
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