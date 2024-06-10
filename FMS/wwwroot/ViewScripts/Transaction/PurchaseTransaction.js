$(function () {
    //default date
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;

    $("#TransactionLink").addClass("active");
    $("#PurchaseTransactionLink").addClass("active");
    $("#PurchaseTransactionLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //----------------------------------------varible declaration-----------------------------------------//
    //purchase
    const purchaseOrderId = $('input[name="hdnPurchaseOrderId"]');
    const transactionDate = $('input[name="TransactionDate"]');
    const ddlProductType = $('select[name="ddlProductType"]');
    const transactionNo = $('input[name="TransactionNo"]');
    const ddlSupplyer = $('select[name="ddlSupplyerId"]');
    const invoiceNo = $('input[name="InvoiceNo"]');
    const invoiceDate = $('input[name="InvoiceDate"]');
    const subTotal = $('input[name="SubTotal"]');
    const discount = $('input[name="TotalDiscountAmount"]');
    const transportationCharges = $('input[name="TransportationCharges"]');
    const grandTotal = $('input[name="GrandTotal"]');
    const vehicleNo = $('input[name="VehicleNo"]');
    const transpoterName = $('input[name="TranspoterName"]');
    const receivingPerson = $('input[name="ReceivingPerson"]');
    const gst = $('input[name="GstAmount"]');
    const Naration = $('textarea[name="NarrationPurchase"]');
    var PurchaseTable = $('#tblPurchase').DataTable({
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
    transactionDate.val(todayDate);
    invoiceDate.val(todayDate);
    //-----------------------------------Contorl Foucous Of Element Purchase----------------------------//
    transactionDate.focus();
    invoiceNo.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    invoiceNo.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    invoiceDate.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    invoiceDate.on('blur', function () {
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
    receivingPerson.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    receivingPerson.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    var chkPage = false;
    $('a[href="#CreatePurchase"]').on('click', function () {
        chkPage = false;
    });
    $(document).on('keydown', function (e) {
        if (e.key === 'Insert' || e.keyCode === 45) {
            if (!chkPage) {
                e.preventDefault();
                $('#addPurchaseRowBtn').click();
            }
        }
    });
    $('#addPurchaseRowBtn').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('#addPurchaseRowBtn').on('blur', function () {
        $(this).css('background-color', '');
    });
   
    $('#btnSave').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('#btnSave').on('blur', function () {
        $(this).css('background-color', '');
    });
   
    $('#btnUpdate').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('#btnUpdate').on('blur', function () {
        $(this).css('background-color', '');
    });
    //-------------------------------------Purchase Validation--------------------------------//
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
    receivingPerson.on('keydown', function (event) {
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
    invoiceNo.on("input", function () {
        let inputValue = $(this).val();
        inputValue = inputValue.toUpperCase();
        $(this).val(inputValue);
    });
    vehicleNo.on("input", function () {
        let inputValue = $(this).val();
        inputValue = inputValue.toUpperCase();
        $(this).val(inputValue);
    });
    //------------------------------------purchase return-----------------------------------//
    const purchaseReturnOrderId = $('input[name="hdnPurchaseReturnOrderId"]');
    const Pr_transactionDate = $('input[name="Pr_TransactionDate"]');
    const Pr_transactionNo = $('input[name="Pr_TransactionNo"]');
    const Pr_ddlProductType = $('select[name="Pr_ddlProductType"]');
    const Pr_ddlSupplyer = $('select[name="Pr_ddlSupplyerId"]');
    const Pr_invoiceNo = $('input[name="Pr_InvoiceNo"]');
    const Pr_invoiceDate = $('input[name="Pr_InvoiceDate"]');
    const Pr_subTotal = $('input[name="Pr_SubTotal"]');
    const Pr_discount = $('input[name="Pr_DiscountAmount"]');
    const Pr_transportationCharges = $('input[name="Pr_TransportationCharges"]');
    const Pr_grandTotal = $('input[name="Pr_GrandTotal"]');
    const Pr_vehicleNo = $('input[name="Pr_VehicleNo"]');
    const Pr_transpoterName = $('input[name="Pr_TranspoterName"]');
    //const Pr_receivingPerson = $('input[name="Pr_ReceivingPerson"]');
    const Pr_gst = $('input[name="Pr_GstAmount"]');
    const Pr_Narration = $('textarea[name="NarrationPurchaseReturn"]');
    var PurchaseReturnTable = $('#tblPurchaseReturn').DataTable({
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
    Pr_transactionDate.val(todayDate)
    Pr_invoiceDate.val(todayDate);
    /*-----------------------------------Contorl Foucous Of Element Purchase Return----------------------------*/
    Pr_invoiceNo.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    Pr_invoiceNo.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    Pr_invoiceDate.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    Pr_invoiceDate.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    Pr_vehicleNo.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    Pr_vehicleNo.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    Pr_transpoterName.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    Pr_transpoterName.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    //Pr_receivingPerson.on('focus', function () {
    //    $(this).css('border-color', 'red');
    //});
    //Pr_receivingPerson.on('blur', function () {
    //    $(this).css('border-color', ''); // Reset background color on blur
    //});
    $('a[href="#CreatePurchaseReturn"]').on('click', function () {
        chkPage = true;
    });
    $(document).on('keydown', function (e) {
        if (e.key === 'Insert' || e.keyCode === 45) {
            if (chkPage) {
                e.preventDefault();
                $('#addPurchaseReturnRowBtn').click();
            }
        }
    });
    $('#addPurchaseReturnRowBtn').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('#addPurchaseReturnRowBtn').on('blur', function () {
        $(this).css('background-color', '');
    });
    //$('#Pr_btnSave').on('keydown', function (e) {
    //    if (e.key === 'Enter' || e.keyCode === 13) {
    //        $('#Pr_btnSave').click();
    //    }
    //});
    $('#Pr_btnSave').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('#Pr_btnSave').on('blur', function () {
        $(this).css('background-color', '');
    });
    $('#Pr_btnUpdate').on('keydown', function (e) {
        if (e.key === 'Enter' || e.keyCode === 13) {
            $('#Pr_btnUpdate').click();
        }
    });
    $('#Pr_btnUpdate').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('#Pr_btnUpdate').on('blur', function () {
        $(this).css('background-color', '');
    });
    /*-------------------------------------Purchase Return Validation------------------------------------------*/
    Pr_invoiceNo.on("input", function () {
        let inputValue = $(this).val();
        inputValue = inputValue.toUpperCase();
        $(this).val(inputValue);
    });
    Pr_vehicleNo.on("input", function () {
        let inputValue = $(this).val();
        inputValue = inputValue.toUpperCase();
        $(this).val(inputValue);
    });
    Pr_transpoterName.on('keydown', function (event) {
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
  //Pr_receivingPerson.on('keydown', function (event) {
  //      const keyCode = event.keyCode || event.which;
  //      if (keyCode >= 48 && keyCode <= 57 || (keyCode >= 96 && keyCode <= 111)) {
  //          event.preventDefault();
  //      }
  //      else {
  //          let inputValue = $(this).val();
  //          inputValue = inputValue.toUpperCase();
  //          $(this).val(inputValue);
  //      }
  //  });
    /*-------------------------------------------------------------Purchase Screen --------------------------------------------------------*/
    //Insert Operation  
    GetProductTypes();
    function GetProductTypes() {
        $.ajax({
            url: "/Transaction/GetProductTypes",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddlProductType.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlProductType.append(defaultOption);
                    $.each(result.ProductTypes, function (key, item) {
                        var option = $('<option></option>').val(item.ProductTypeId).text(item.Product_Type);
                        ddlProductType.append(option);
                    });
                }
                else {
                    ddlProductType.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlProductType.append(defaultOption);
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    GetSundryCreditors();
    function GetSundryCreditors() {
        $.ajax({
            url: "/Transaction/GetSundryCreditors",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddlSupplyer.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlSupplyer.append(defaultOption);
                    $.each(result.SubLedgers, function (key, item) {
                        var option = $('<option></option>').val(item.SubLedgerId).text(item.SubLedgerName + "( " +item.Adress+ ")");
                        ddlSupplyer.append(option);
                    });
                }
                else {
                    ddlSupplyer.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlSupplyer.append(defaultOption);
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    GetLastPurchaseTransaction();
    function GetLastPurchaseTransaction() {
        $.ajax({
            url: "/Transaction/GetLastPurchaseTransaction",
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
    function addPurchaseRow() {
        if (!ddlProductType.val() || ddlProductType.val() === '--Select Option--') {
            toastr.error('Plz Select Product Type First');
            ddlProductType.focus();
            return;
        }
        else {
            var uniqueId = 'ddlitem' + new Date().getTime();
            var html = '<tr>';
            html += '<td hidden><input type="hidden" class="form-control" value="0"></td>';
            html += '<td>' +
                '<div class="form-group">' +
                '    <select class="form-control form-control-sm select2bs4 Rawmaterial" style="width: 100%;" id="' + uniqueId + '"></select>' +
                '</div>' +
                '</td>';
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
            html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
            html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
            html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
            html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
            html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
            html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
            html += '<td style="background-color:#ffe6e6; text-align:center">';
            html += '<button class="btn btn-primary btn-link addBtn" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-plus"></i></button>';
            html += '<button class="btn btn-primary btn-link deleteBtn" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
            html += '</td>';
            html += '</tr>';
            var newRow = PurchaseTable.row.add($(html)).draw();
            $.ajax({
                url: '/Transaction/GetProductByType?ProductTypeId=' + ddlProductType.val() + '',
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
                        selectElement.focus();
                    }
                },
                error: function (errormessage) {
                    console.log(errormessage)
                }
            });
            $('#tblPurchase tbody').find('.select2bs4').select2({
                theme: 'bootstrap4'
            });
        }
    }
    $('#addPurchaseRowBtn').click(function () {
        addPurchaseRow();
    });
    $('#tblPurchase').on('click', '.addBtn', function () {
        addPurchaseRow();
    });
    $(document).on('change', '.Rawmaterial', function () {
        var selectElement = $(this);
        var selectedProductId = selectElement.val();
        if (selectedProductId) {
            $.ajax({
                url: '/Transaction/GetProductGstWithRate?id=' + selectedProductId,
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.ResponseCode == 302) {
                        var Textbox = selectElement.closest('tr').find('input[type="text"]');
                        for (var i = 0; i < 7; i++) {
                            Textbox.eq(i).val('0');
                        }
                        /*  Textbox.eq(2).val(result.product.Price);*/
                        Textbox.eq(5).val(result.Product.GST);
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
    $(document).on('click', '.deleteBtn', function () {
        var row = PurchaseTable.row($(this).closest('tr'));
        row.remove().draw();
    });
    $('#tblPurchase tbody').on('change', 'input[type="text"]', function () {
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
        $('#tblPurchase tbody tr').each(function () {
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
        $('#tblPurchase tbody tr').each(function () {
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
        gstDifferenceBody.empty();
        for (const rate in gstDifferences) {
            const row = $('<tr><td>GST ' + rate + ' % Amount: ' + gstDifferences[rate].toFixed(2) + '</td></tr>');
            gstDifferenceBody.append(row);
        }
    });
    transportationCharges.on('change', function () {
        var tranportChgAmount = $(this).val();
        subTotalAmount = subTotal.val();
        discountAmount = discount.val();
        gstAmount = gst.val();
        updateGrandTotal = parseFloat(tranportChgAmount) + parseFloat(subTotalAmount) + parseFloat(gstAmount) - parseFloat(discountAmount);
        grandTotal.val(updateGrandTotal.toFixed(2));
    });
    $('#btnSave').on('click', CreatePurchase);
    function CreatePurchase() {
        if (!transactionDate.val()) {
            toastr.error('TransactionDate Is Required.');
            return;
        }
        else if (!ddlProductType.val() || ddlProductType.val() === '--Select Option--') {
            toastr.error('Product Type Is Required.');
            Pr_ddlProductType.focus();
            return;
        }
        else if (!ddlSupplyer.val() || ddlSupplyer.val() === '--Select Option--') {
            toastr.error('Supplyer Name Is Required.');
            ddlSupplyer.focus();
            return;
        } else if (!invoiceNo.val()) {
            toastr.error('InvoiceNo Is Required.');
            invoiceNo.focus();
            return;
        } else if (!invoiceDate.val()) {
            toastr.error('InvoiceDate Is Required.');
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
        } else if (!receivingPerson.val()) {
            toastr.error('ReceivingPerson Is Required.');
            receivingPerson.focus();
            return;
        }
        else {
            $('#loader').show();
            var rowData = [];
            $('#tblPurchase tbody tr').each(function () {
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
                Fk_ProductTypeId: ddlProductType.val(),
                Fk_SubLedgerId: ddlSupplyer.val(),
                TransactionDate: transactionDate.val(),
                TransactionNo: transactionNo.val(),
                InvoiceNo: invoiceNo.val(),
                InvoiceDate: invoiceDate.val(),
                SubTotal: subTotal.val(),
                DiscountAmount: discount.val(),
                GrandTotal: grandTotal.val(),
                TransportationCharges: transportationCharges.val(),
                GstAmount: gst.val(),
                TranspoterName: transpoterName.val(),
                VehicleNo: vehicleNo.val(),
                ReceivingPerson: receivingPerson.val(),
                Naration: Naration.val(),
                rowData: rowData
            };
            $.ajax({
                type: "POST",
                url: '/Transaction/CreatePurchase',
                dataType: 'json',
                data: JSON.stringify(requestData),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#loader').hide();
                    if (Response.ResponseCode == 201) {
                        toastr.success(Response.SuccessMsg);
                        PurchaseTable.clear().draw();
                        transactionDate.val('');
                        invoiceNo.val('');
                        invoiceDate.val('');
                        transpoterName.val('');
                        vehicleNo.val('');
                        receivingPerson.val('');
                        subTotal.val('0');
                        discount.val('0');
                        gst.val('0');
                        grandTotal.val('0');
                        Naration.val('');
                        transportationCharges.val('0');
                        GetLastPurchaseTransaction();
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
    //-----------------------------------------------------Purchase List Scren --------------------------------------------------//
    //Update Operation
    $('a[href="#PurchaseList"]').on('click', function () {
        GetPurchaseList();
    });
    function GetPurchaseList() {
        $('#loader').show();
        $('.PurchaseListTable').empty();
        $.ajax({
            url: "/Transaction/GetPurchases",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#loader').hide();
                var html = '';
                html += '<table class="table table-bordered table-hover text-center mt-1 PurchaseListTable" style="width:100%">';
                html += '<thead>'
                html += '<tr>'
                html += '<th hidden>Purchase OrderId</th>'
                html += '<th>Trxn No</th>'
                html += '<th>Trxn Dt.</th>'
                html += '<th>Party</th>'
                html += '<th>Mat Receipt No</th>'
                html += '<th>Grand Total</th>'
                html += '<th>Action</th>'
                html += '</tr>'
                html += '</thead>'
                html += '<tbody>';
                if (result.ResponseCode == 302) {
                    $.each(result.purchaseOrders, function (key, item) {
                        html += '<tr>';
                        html += '<td hidden>' + item.PurchaseOrderId + '</td>';
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
                            html += '<td>' - '</td>';
                        }

                        html += '<td>' + item.InvoiceNo + '</td>';

                        html += '<td>' + item.GrandTotal + '</td>';
                        html += '<td style="background-color:#ffe6e6;">';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-purchase-edit"   id="btnPurchaseEdit_' + item.PurchaseOrderId + '"     data-id="' + item.PurchaseOrderId + '" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-purchase-delete" id="btnPurchaseDelete_' + item.PurchaseOrderId + '"   data-id="' + item.PurchaseOrderId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
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
                $('.tblPurchaseList').html(html);
                if (!$.fn.DataTable.isDataTable('.PurchaseListTable')) {
                    var table = $('.PurchaseListTable').DataTable({
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
    $(document).on('click', '.btn-purchase-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        GetPurchaseById(value);
        $('#tab-CreatePurchase').trigger('click');
        $('#btnSave').hide();
        $('#btnUpdate').show();
    });
    function GetPurchaseById(Id) {
        $.ajax({
            url: '/Transaction/GetPurchaseById?Id=' + Id + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                purchaseOrderId.val(result.purchaseOrder.PurchaseOrderId)
                const ModifytransactionDate = result.purchaseOrder.TransactionDate;
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
                transactionNo.val(result.purchaseOrder.TransactionNo)
                $.ajax({
                    url: "/Transaction/GetProductTypes",
                    type: "GET",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result0) {
                        if (result0.ResponseCode == 302) {
                            ddlProductType.empty();
                            var defaultOption = $('<option></option>').val('').text('--Select Option--');
                            ddlProductType.append(defaultOption);
                            $.each(result0.ProductTypes, function (key, item) {
                                var option = $('<option></option>').val(item.ProductTypeId).text(item.Product_Type);
                                if (item.ProductTypeId === result.purchaseOrder.Fk_ProductTypeId) {
                                    option.attr('selected', 'selected');
                                }
                                ddlProductType.append(option);
                            });
                        }
                        else {
                            ddlProductType.empty();
                            var defaultOption = $('<option></option>').val('').text('--Select Option--');
                            ddlProductType.append(defaultOption);
                        }
                    },
                    error: function (errormessage) {
                        console.log(errormessage)
                    }
                });
                $.ajax({
                    url: "/Transaction/GetSundryCreditors",
                    type: "GET",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result1) {

                        if (result1.ResponseCode == 302) {
                            ddlSupplyer.empty();
                            var defaultOption = $('<option></option>').val('').text('--Select Option--');
                            ddlSupplyer.append(defaultOption);
                            $.each(result1.SubLedgers, function (key, item) {
                                var option = $('<option></option>').val(item.SubLedgerId).text(item.SubLedgerName);
                                if (item.SubLedgerId === result.purchaseOrder.Fk_SubLedgerId) {
                                    option.attr('selected', 'selected');
                                }
                                ddlSupplyer.append(option);
                            });
                        }
                        else {
                            ddlSupplyer.empty();
                            var defaultOption = $('<option></option>').val('').text('--Select Option--');
                            ddlSupplyer.append(defaultOption);
                        }
                    },
                    error: function (errormessage) {
                        console.log(errormessage)
                    }
                });
                invoiceNo.val(result.purchaseOrder.InvoiceNo);
                transpoterName.val(result.purchaseOrder.TranspoterName);
                receivingPerson.val(result.purchaseOrder.ReceivingPerson);
                vehicleNo.val(result.purchaseOrder.VehicleNo);
                Naration.val(result.purchaseOrder.Naration);
                const ModifyinvoiceDate = result.purchaseOrder.InvoiceDate;
                if (ModifyinvoiceDate) {
                    const dateObject = new Date(ModifyinvoiceDate);
                    if (!isNaN(dateObject)) {
                        const day = String(dateObject.getDate()).padStart(2, '0');
                        const month = String(dateObject.getMonth() + 1).padStart(2, '0');
                        const year = dateObject.getFullYear();
                        const formattedDate = `${day}/${month}/${year}`;
                        invoiceDate.val(formattedDate);
                    }
                }

                ModifyorderDate = result.purchaseOrder.OrderDate;
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
                subTotal.val(result.purchaseOrder.SubTotal);
                discount.val(result.purchaseOrder.Discount);
                transportationCharges.val(result.purchaseOrder.TransportationCharges)
                grandTotal.val(result.purchaseOrder.GrandTotal);
                gst.val(result.purchaseOrder.Gst);
                PurchaseTable.clear().draw();//Fill Table Data
                $.each(result.purchaseOrder.PurchaseTransactions, function (key, item) {
                    var html = '<tr>';
                    html += '<td  hidden><input type="hidden" class="form-control"  value=' + item.PurchaseId + '></td>';
                    html += '<td>' +
                        '<div class="form-group">' +
                        '    <select class="form-control form-control-sm select2bs4 Rawmaterial" style="width: 100%;" id="ddnProduct_' + item.PurchaseId + '"></select>' +
                        '</div>' +
                        '</td>';

                    html += '<td style="width:8%"><div class="form-group"><input type="text" id="txtAlternateQty" class="form-control" value=' + item.AlternateQuantity + '></div></td>';
                    html += '<td style="width:12%">' +
                        '<div class="form-group">' +
                        '<select class="form-control form-control select2bs4 selectedUnit" style="width:100%" id="ddnUnit_' + item.PurchaseId + '" disabled></select>' +
                        '</div>' +
                        '</td>';
                    html += '<td>' +
                        '<div class="form-group">' +
                        '<div class="input-group">' +
                        '<input type="text" class="form-control" value=' + item.UnitQuantity + '>' +
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
                    html += '<td style="background-color:#ffe6e6; text-align:center">';
                    html += '<button class="btn btn-primary btn-link addBtn" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-plus"></i></button>';
                    html += '<button class="btn btn-primary btn-link deleteBtn" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                    html += '</td>';
                    html += '</tr>';
                    PurchaseTable.row.add($(html)).draw(false).node();
                    var selectProductElement = $('#ddnProduct_' + item.PurchaseId);
                    selectProductElement.empty();
                    var defaultProductOption = $('<option></option>').val('').text('--Select Option--');
                    selectProductElement.append(defaultProductOption);
                    $.ajax({
                        url: '/Transaction/GetProductByType?ProductTypeId=' + result.purchaseOrder.Fk_ProductTypeId + '',
                        type: "GET",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (result) {
                            if (result.ResponseCode == 302) {
                                $.each(result.Products, function (key, item1) {
                                    var productoption = $('<option></option>').val(item1.ProductId).text(item1.ProductName);
                                    if (item.Fk_ProductId === item1.ProductId) {
                                        productoption.attr('selected', 'selected');
                                    }
                                    selectProductElement.append(productoption);
                                });
                            }
                        },
                        error: function (errormessage) {
                            console.log(errormessage)
                        }
                    });
                    var selectUnitElement = $('#ddnUnit_' + item.PurchaseId);
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
                $('#tblPurchase tbody').find('.select2bs4').select2({
                    theme: 'bootstrap4'
                });
                const gstDifferences = {};
                $('#tblPurchase tbody tr').each(function () {
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
    $('#btnUpdate').on('click', UpdatePurchase);
    function UpdatePurchase() {

        if (!transactionDate.val()) {
            toastr.error('TransactionDate Is Required.');
            return;
        }
        else if (!ddlProductType.val() || ddlProductType.val() === '--Select Option--') {
            toastr.error('Product Type Is Required.');
            Pr_ddlProductType.focus();
            return;
        }
        else if (!ddlSupplyer.val() || ddlSupplyer.val() === '--Select Option--') {
            toastr.error('Supplyer Name Is Required.');
            ddlSupplyer.focus();
            return;
        } else if (!invoiceNo.val()) {
            toastr.error('InvoiceNo Is Required.');
            invoiceNo.focus();
            return;
        } else if (!invoiceDate.val()) {
            toastr.error('InvoiceDate Is Required.');
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
        } else if (!receivingPerson.val()) {
            toastr.error('ReceivingPerson Is Required.');
            receivingPerson.focus();
            return;
        } else {
            $('#loader').show();
            var rowData = [];
            $('#tblPurchase tbody tr').each(function () {
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
                PurchaseOrderId: purchaseOrderId.val(),
                Fk_ProductTypeId: ddlProductType.val(),
                TransactionDate: transactionDate.val(),
                TransactionNo: transactionNo.val(),
                Fk_SubLedgerId: ddlSupplyer.val(),
                InvoiceNo: invoiceNo.val(),
                InvoiceDate: invoiceDate.val(),
                TransportationCharges: transportationCharges.val(),
                TranspoterName: transpoterName.val(),
                VehicleNo: vehicleNo.val(),
                ReceivingPerson: receivingPerson.val(),
                SubTotal: subTotal.val(),
                DiscountAmount: discount.val(),
                GstAmount: gst.val(),
                GrandTotal: grandTotal.val(),
                Naration: Naration.val(),
                rowData: rowData
            };
            $.ajax({
                type: "POST",
                url: '/Transaction/UpdatePurchase',
                dataType: 'json',
                data: JSON.stringify(requestData),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#loader').hide();
                    if (Response.ResponseCode == 200) {
                        toastr.success(Response.SuccessMsg);
                        PurchaseTable.clear().draw();
                        purchaseOrderId.val('');
                        transactionNo.val('');
                        transactionDate.val('');
                        invoiceNo.val('');
                        invoiceDate.val('');
                        vehicleNo.val('');
                        transpoterName.val('');
                        receivingPerson.val('');
                        subTotal.val('0');
                        transportationCharges.val('0');
                        gst.val('0');
                        discount.val('0');
                        grandTotal.val('0');
                        Naration.val('');
                        GetLastPurchaseTransaction();
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
    //Delete Operation
    $(document).on('click', '.btn-purchase-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeletePurchaseRecord(value);
    });
    function DeletePurchaseRecord(Id) {
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
                    url: '/Transaction/DeletePurchase?id=' + Id + '',
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
                        GetPurchaseList();
                        GetLastPurchaseTransaction();
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
            }
        });
    }
    //-------------------------------------------------------------Purchase Return Screen --------------------------------------------------------//
    $('a[href="#CreatePurchaseReturn"]').on('click', function () {
        GetSundryCreditorsForPurchaseReturn();
        GetLastPurchaseReturnTransaction();
        GetProductTypeForPurchaseReturn();
        Pr_transactionDate.focus();
    });
    function GetProductTypeForPurchaseReturn() {
        $.ajax({
            url: "/Transaction/GetProductTypes",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    Pr_ddlProductType.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    Pr_ddlProductType.append(defaultOption);
                    $.each(result.ProductTypes, function (key, item) {
                        var option = $('<option></option>').val(item.ProductTypeId).text(item.Product_Type);
                        Pr_ddlProductType.append(option);
                    });
                }
                else {
                    Pr_ddlProductType.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    Pr_ddlProductType.append(defaultOption);
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    function GetSundryCreditorsForPurchaseReturn() {
        $.ajax({
            url: "/Transaction/GetSundryCreditors",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    Pr_ddlSupplyer.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    Pr_ddlSupplyer.append(defaultOption);
                    $.each(result.SubLedgers, function (key, item) {
                        var option = $('<option></option>').val(item.SubLedgerId).text(item.SubLedgerName + " (" + item.Adress + ")");
                        Pr_ddlSupplyer.append(option);
                    });
                }
                else {
                    Pr_ddlSupplyer.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    Pr_ddlSupplyer.append(defaultOption);
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    function GetLastPurchaseReturnTransaction() {
        $.ajax({
            url: "/Transaction/GetLastPurchaseReturnTransaction",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                Pr_transactionNo.val(result.Data);
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    function PurchaseReturnRowBtn() {
        if (!Pr_ddlProductType.val() || Pr_ddlProductType.val() === '--Select Option--') {
            toastr.error('Plz Select Product Type First');
            Pr_ddlProductType.focus();
            return;
        }
        else {
            var uniqueId = 'ddlitem' + new Date().getTime();
            var html = '<tr>';
            html += '<td hidden><input type="hidden" class="form-control" value="0"></td>';
            html += '<td><div class="form-group"><select class="form-control form-control-sm select2bs4 Rawmaterial" style="width: 100%;" id="' + uniqueId + '"></select></div></td>';
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
                ' <span class="input-group-text" id="Unit">N/A</span>' +
                '</div>' +
                '</div>' +
                '</div>' +
                '</td>';
            html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
            html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
            html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
            html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
            html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
            html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
            html += '<td style="background-color:#ffe6e6; text-align:center">';
            html += '<button class="btn btn-primary btn-link addBtnReturn" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-plus"></i></button>';
            html += '<button class="btn btn-primary btn-link deleteBtnReturn" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
            html += '</td>';
            html += '</tr>';
            var newRow = PurchaseReturnTable.row.add($(html)).draw(false).node();
            $.ajax({
                url: '/Transaction/GetProductByType?ProductTypeId=' + Pr_ddlProductType.val() + '',
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
            $('#tblPurchaseReturn tbody').find('.select2bs4').select2({
                theme: 'bootstrap4'
            });
        }
    }
    $('#addPurchaseReturnRowBtn').click(function () {
        PurchaseReturnRowBtn();
    });
    $('#tblPurchaseReturn').on('click', '.addBtnReturn', function () {
        PurchaseReturnRowBtn();
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
        var row = PurchaseReturnTable.row($(this).closest('tr'));
        row.remove().draw();
    });
    $(document).on('keydown', function (e) {
        if (e.which == 40) {
            addPurchaseReturnRow();
            e.preventDefault();
        }
    });
    $('#tblPurchaseReturn tbody').on('change', 'input[type="text"]', function () {
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
        $('#tblPurchaseReturn tbody tr').each(function () {
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
        $('input[name="Pr_SubTotal"]').val(toalSubTotalAmount.toFixed(2));
        $('input[name="Pr_GrandTotal"]').val(totalAmount.toFixed(2));
        $('input[name="Pr_GstAmount"]').val(totalGstAmount.toFixed(2));
        $('input[name="Pr_DiscountAmount"]').val(totalDiscountAmount.toFixed(2));
        const gstDifferences = {};
        $('#tblPurchaseReturn tbody tr').each(function () {
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

        const gstDifferenceBody = $('#PrtblGstdifference tbody');
        gstDifferenceBody.empty();
        for (const rate in gstDifferences) {
            const row = $('<tr><td>GST ' + rate + ' % Amount: ' + gstDifferences[rate].toFixed(2) + '</td></tr>');
            gstDifferenceBody.append(row);
        }
    });
    Pr_transportationCharges.on('change', function () {
        var tranportChgAmount = $(this).val();
        subTotalAmount = Pr_subTotal.val();
        discountAmount = Pr_discount.val();
        gstAmount = Pr_gst.val();
        updateGrandTotal = parseFloat(tranportChgAmount) + parseFloat(subTotalAmount) + parseFloat(gstAmount) - parseFloat(discountAmount);
        Pr_grandTotal.val(updateGrandTotal.toFixed(2));
    });
    $('#Pr_btnSave').on('click', CreatetPurchaseReturn);
    function CreatetPurchaseReturn() {

        if (!Pr_transactionDate.val()) {
            toastr.error('TransactionDate Is Required.');
            return;
        }
        else if (!Pr_ddlProductType.val() || Pr_ddlProductType.val() === '--Select Option--') {
            toastr.error('Product Type Is Required.');
            Pr_ddlProductType.focus();
            return;
        }
        else if (!Pr_ddlSupplyer.val() || Pr_ddlSupplyer.val() === '--Select Option--') {
            toastr.error('Supplyer Name Is Required.');
            Pr_ddlSupplyer.focus();
            return;
        } else if (!Pr_invoiceNo.val()) {
            toastr.error('InvoiceNo Is Required.');
            Pr_invoiceNo.focus();
            return;
        } else if (!Pr_invoiceDate.val()) {
            toastr.error('InvoiceDate Is Required.');
            return;
        } else if (!Pr_vehicleNo.val()) {
            toastr.error('VehicleNo Is Required.');
            Pr_vehicleNo.focus();
            return;
        }
        else if (!Pr_transpoterName.val()) {
            toastr.error('TranspoterName Is Required.');
            Pr_transpoterName.focus();
            return;
        } else {
            $('#loader').show();
            var rowData = [];
            $('#tblPurchaseReturn tbody tr').each(function () {
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
            if (rowData !== null) {
                var requestData = {
                    Fk_ProductTypeId: Pr_ddlProductType.val(),
                    Fk_SubLedgerId: Pr_ddlSupplyer.val(),
                    TransactionDate: Pr_transactionDate.val(),
                    TransactionNo: Pr_transactionNo.val(),
                    InvoiceNo: Pr_invoiceNo.val(),
                    InvoiceDate: Pr_invoiceDate.val(),
                    SubTotal: Pr_subTotal.val(),
                    DiscountAmount: Pr_discount.val(),
                    GrandTotal: Pr_grandTotal.val(),
                    GstAmount: Pr_gst.val(),
                    TransportationCharges: Pr_transportationCharges.val(),
                    TranspoterName: Pr_transpoterName.val(),
                    VehicleNo: Pr_vehicleNo.val(),
                    //ReceivingPerson: Pr_receivingPerson.val(),
                    Naration: Pr_Narration.val(),
                    rowData: rowData
                };
                $.ajax({
                    type: "POST",
                    url: '/Transaction/CreatetPurchaseReturn',
                    dataType: 'json',
                    data: JSON.stringify(requestData),
                    contentType: "application/json;charset=utf-8",
                    success: function (Response) {
                        $('#loader').hide();
                        if (Response.ResponseCode == 201) {
                            toastr.success(Response.SuccessMsg);
                            PurchaseReturnTable.clear().draw();
                            Pr_transactionDate.val('');
                            Pr_invoiceNo.val('');
                            Pr_invoiceDate.val('');
                            Pr_transpoterName.val('');
                            Pr_vehicleNo.val('');
                            Pr_receivingPerson.val('');
                            Pr_subTotal.val('0');
                            Pr_gst.val('0');
                            Pr_discount.val('0');
                            Pr_grandTotal.val('0');
                            Pr_Narration.val('');
                            Pr_transportationCharges.val('0');
                            GetLastPurchaseReturnTransaction();
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
            else {
                toastr.error('Invalid Transaction');
            }

        }
    }
    /*-----------------------------------------------------Purchase Return List Scren --------------------------------------------------*/
    $('a[href="#PurchaseReturnList"]').on('click', function () {
        GetPurchaseReturns();
    });
    function GetPurchaseReturns() {
        $('#loader').show();
        $('.PurchaseReturnListTable').empty();
        $.ajax({
            url: "/Transaction/GetPurchaseReturns",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#loader').hide();
                var html = '';
                html += '<table class="table table-bordered table-hover text-center mt-1 PurchaseReturnListTable" style="width:100%">';
                html += '<thead>'
                html += '<tr>'
                html += '<th hidden>Purchase Return OrderId</th>'
                html += '<th>Trxn No</th>'
                html += '<th>Trxn Dt.</th>'
                html += '<th>Party</th>'
                html += '<th>Challan No</th>'
                html += '<th>Grand Total</th>'
                html += '<th>Action</th>'
                html += '</tr>'
                html += '</thead>'
                html += '<tbody>';
                if (result.ResponseCode == 302) {
                    $.each(result.purchaseReturnOrders, function (key, item) {
                        html += '<tr>';
                        html += '<td hidden>' + item.PurchaseReturnOrderId + '</td>';
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
                            html += '<td>' - '</td>';
                        }

                        html += '<td>' + item.InvoiceNo + '</td>';
                        html += '<td>' + item.GrandTotal + '</td>';
                        html += '<td style="background-color:#ffe6e6;">';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-purchase-return-edit"   id="btnPurchaseReturnEdit_' + item.PurchaseReturnOrderId + '"     data-id="' + item.PurchaseReturnOrderId + '" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-purchase-return-delete" id="btnPurchaseReturnDelete_' + item.PurchaseReturnOrderId + '"   data-id="' + item.PurchaseReturnOrderId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
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
                $('.tblPurchaseReturnList').html(html);
                if (!$.fn.DataTable.isDataTable('.PurchaseReturnListTable')) {
                    var table = $('.PurchaseReturnListTable').DataTable({
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
    $(document).on('click', '.btn-purchase-return-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        GetPurchaseReturnById(value);
        $('#tab-CreatePurchaseReturn').trigger('click');
        $('#Pr_btnSave').hide();
        $('#Pr_btnUpdate').show();
    });
    function GetPurchaseReturnById(Id) {
        $.ajax({
            url: '/Transaction/GetPurchaseReturnById?Id=' + Id + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                purchaseReturnOrderId.val(result.purchaseReturnOrder.PurchaseReturnOrderId)
                const ModifytransactionDate = result.purchaseReturnOrder.TransactionDate;
                if (ModifytransactionDate) {
                    const dateObject = new Date(ModifytransactionDate);
                    if (!isNaN(dateObject)) {
                        const day = String(dateObject.getDate()).padStart(2, '0');
                        const month = String(dateObject.getMonth() + 1).padStart(2, '0');
                        const year = dateObject.getFullYear();
                        const formattedDate = `${day}/${month}/${year}`;
                        Pr_transactionDate.val(formattedDate);
                    }
                }
                Pr_transactionNo.val(result.purchaseReturnOrder.TransactionNo);
                $.ajax({
                    url: "/Transaction/GetProductTypes",
                    type: "GET",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result0) {
                        if (result0.ResponseCode == 302) {
                            Pr_ddlProductType.empty();
                            var defaultOption = $('<option></option>').val('').text('--Select Option--');
                            Pr_ddlProductType.append(defaultOption);
                            $.each(result0.ProductTypes, function (key, item) {
                                var option = $('<option></option>').val(item.ProductTypeId).text(item.Product_Type);
                                if (item.ProductTypeId === result.purchaseReturnOrder.Fk_ProductTypeId) {
                                    option.attr('selected', 'selected');
                                }
                                Pr_ddlProductType.append(option);
                            });
                        }
                        else {
                            Pr_ddlProductType.empty();
                            var defaultOption = $('<option></option>').val('').text('--Select Option--');
                            Pr_ddlProductType.append(defaultOption);
                        }
                    },
                    error: function (errormessage) {
                        console.log(errormessage)
                    }
                });
                $.ajax({
                    url: "/Transaction/GetSundryCreditors",
                    type: "GET",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result1) {
                        if (result1.ResponseCode == 302) {
                            Pr_ddlSupplyer.empty();
                            var defaultOption = $('<option></option>').val('').text('--Select Option--');
                            Pr_ddlSupplyer.append(defaultOption);
                            $.each(result1.SubLedgers, function (key, item) {
                                var option = $('<option></option>').val(item.SubLedgerId).text(item.SubLedgerName);
                                if (item.SubLedgerId === result.purchaseReturnOrder.Fk_SubLedgerId) {
                                    option.attr('selected', 'selected');
                                }
                                Pr_ddlSupplyer.append(option);
                            });
                        }
                        else {
                            Pr_ddlSupplyer.empty();
                            var defaultOption = $('<option></option>').val('').text('--Select Option--');
                            Pr_ddlSupplyer.append(defaultOption);
                        }
                    },
                    error: function (errormessage) {
                        console.log(errormessage)
                    }
                });
                Pr_invoiceNo.val(result.purchaseReturnOrder.InvoiceNo);
                const ModifyinvoiceDate = result.purchaseReturnOrder.InvoiceDate;
                if (ModifyinvoiceDate) {
                    const dateObject = new Date(ModifyinvoiceDate);
                    if (!isNaN(dateObject)) {
                        const day = String(dateObject.getDate()).padStart(2, '0');
                        const month = String(dateObject.getMonth() + 1).padStart(2, '0');
                        const year = dateObject.getFullYear();
                        const formattedDate = `${day}/${month}/${year}`;
                        Pr_invoiceDate.val(formattedDate);
                    }
                }
                Pr_vehicleNo.val(result.purchaseReturnOrder.VehicleNo);
               // Pr_receivingPerson.val(result.purchaseReturnOrder.ReceivingPerson);
                Pr_transpoterName.val(result.purchaseReturnOrder.TranspoterName);
                Pr_Narration.val(result.purchaseReturnOrder.Naration);
                ModifyorderDate = result.purchaseReturnOrder.OrderDate;
                if (ModifyorderDate) {
                    const dateObject = new Date(ModifyorderDate);
                    if (!isNaN(dateObject)) {
                        const day = String(dateObject.getDate()).padStart(2, '0');
                        const month = String(dateObject.getMonth() + 1).padStart(2, '0');
                        const year = dateObject.getFullYear();
                        const formattedDate = `${day}/${month}/${year}`;
                        Pr_orderDate.val(formattedDate);
                    }
                }
                Pr_subTotal.val(result.purchaseReturnOrder.SubTotal);
                Pr_discount.val(result.purchaseReturnOrder.Discount);
                Pr_transportationCharges.val(result.purchaseReturnOrder.TransportationCharges)
                Pr_gst.val(result.purchaseReturnOrder.Gst);
                Pr_grandTotal.val(result.purchaseReturnOrder.GrandTotal);
                PurchaseReturnTable.clear().draw();
                $.each(result.purchaseReturnOrder.PurchaseReturnTransactions, function (key, item) {
                    var html = '<tr>';
                    html += '<td  hidden><input type="hidden" class="form-control"  value=' + item.PurchaseReturnId + '></td>';
                    html += '<td><div class="form-group"><select class="form-control form-control-sm select2bs4 Rawmaterial" style="width: 100%;" id="ddn_' + item.PurchaseReturnId + '"> </select></div></td>';
                    html += '<td style="width:8%"><div class="form-group"><input type="text" class="form-control" id="Qtyrt" value=' + item.AlternateQuantity + '></div></td>';
                    html += '<td style="width:12%">' +
                        '<div class="form-group">' +
                        '<select class="form-control form-control select2bs4 selectedUnitRtn" style="width:100%" id="ddnUnit_' + item.PurchaseReturnId + '" disabled></select>' +
                        '</div>' +
                        '</td>';
                    html += '<td>' +
                        '<div class="form-group">' +
                        '<div class="input-group">' +
                        '<input type="text" class="form-control" value=' + item.UnitQuantity + '>' +
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
                    html += '<td style="background-color:#ffe6e6; text-align:center">';
                    html += '<button class="btn btn-primary btn-link addBtnReturn" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-plus"></i></button>';
                    html += '<button class="btn btn-primary btn-link deleteBtnReturn" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                    html += '</td>';
                    html += '</tr>';
                    PurchaseReturnTable.row.add($(html)).draw(false).node();
                    var selectProductElement = $('#ddn_' + item.PurchaseReturnId);
                    selectProductElement.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    selectProductElement.append(defaultOption);
                    $.ajax({
                        url: '/Transaction/GetProductByType?ProductTypeId=' + result.purchaseReturnOrder.Fk_ProductTypeId + '',
                        type: "GET",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (result) {
                            if (result.ResponseCode == 302) {
                                $.each(result.Products, function (key, item1) {
                                    var option = $('<option></option>').val(item1.ProductId).text(item1.ProductName);
                                    if (item.Fk_ProductId === item1.ProductId) {
                                        option.attr('selected', 'selected');
                                    }
                                    selectProductElement.append(option);
                                });
                            }
                        },
                        error: function (errormessage) {
                            console.log(errormessage)
                        }
                    });
                    var selectUnitElement = $('#ddnUnit_' + item.PurchaseReturnId);
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
                $('#tblPurchaseReturn tbody').find('.select2bs4').select2({
                    theme: 'bootstrap4'
                });
                const gstDifferences = {};
                $('#tblPurchaseReturn tbody tr').each(function () {
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
                const gstDifferenceBody = $('#tblPrGstdifference tbody');
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
    $('#Pr_btnUpdate').on('click', UpdatetPurchaseReturn);
    function UpdatetPurchaseReturn() {
        if (!Pr_transactionDate.val()) {
            toastr.error('TransactionDate Is Required.');
            return;
        }
        else if (!Pr_ddlProductType.val() || Pr_ddlProductType.val() === '--Select Option--') {
            toastr.error('Product Type Is Required.');
            Pr_ddlProductType.focus();
            return;
        }
        else if (!Pr_ddlSupplyer.val() || Pr_ddlSupplyer.val() === '--Select Option--') {
            toastr.error('Supplyer Name Is Required.');
            Pr_ddlSupplyer.focus();
            return;
        } else if (!Pr_invoiceNo.val()) {
            toastr.error('InvoiceNo Is Required.');
            Pr_invoiceNo.focus();
            return;
        } else if (!Pr_invoiceDate.val()) {
            toastr.error('InvoiceDate Is Required.');
            return;
        } else if (!Pr_vehicleNo.val()) {
            toastr.error('VehicleNo Is Required.');
            Pr_vehicleNo.focus();
            return;
        }
        else if (!Pr_transpoterName.val()) {
            toastr.error('TranspoterName Is Required.');
            Pr_transpoterName.focus();
            return;
        }
         else {
            $('#loader').show();
            var rowData = [];
            $('#tblPurchaseReturn tbody tr').each(function () {
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
                PurchaseOrderId: purchaseReturnOrderId.val(),
                Fk_ProductTypeId: Pr_ddlProductType.val(),
                TransactionDate: Pr_transactionDate.val(),
                TransactionNo: Pr_transactionNo.val(),
                Fk_SubLedgerId: Pr_ddlSupplyer.val(),
                InvoiceNo: Pr_invoiceNo.val(),
                InvoiceDate: Pr_invoiceDate.val(),
                TranspoterName: Pr_transpoterName.val(),
                VehicleNo: Pr_vehicleNo.val(),
                //ReceivingPerson: Pr_receivingPerson.val(),
                SubTotal: Pr_subTotal.val(),
                DiscountAmount: Pr_discount.val(),
                TransportationCharges: Pr_transportationCharges.val(),
                GstAmount: Pr_gst.val(),
                GrandTotal: Pr_grandTotal.val(),
                Naration: Pr_Narration.val(),
                rowData: rowData
            };
            $.ajax({
                type: "POST",
                url: '/Transaction/UpdatetPurchaseReturn',
                dataType: 'json',
                data: JSON.stringify(requestData),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#loader').hide();
                    if (Response.ResponseCode == 200) {
                        toastr.success(Response.SuccessMsg);
                        PurchaseReturnTable.clear().draw();
                        purchaseReturnOrderId.val('');
                        Pr_transactionNo.val('');
                        Pr_transactionDate.val('');
                        Pr_invoiceNo.val('');
                        Pr_invoiceDate.val('');
                        Pr_transpoterName.val('');
                        Pr_vehicleNo.val('');
                       // Pr_receivingPerson.val('');
                        Pr_subTotal.val('0');
                        Pr_discount.val('0');
                        Pr_gst.val('0');
                        Pr_grandTotal.val('0');
                        Pr_Narration.val('');
                        Pr_transportationCharges.val('0');
                        GetLastPurchaseReturnTransaction();
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
    $(document).on('click', '.btn-purchase-return-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeletePurchaseReturnRecord(value);
    });
    function DeletePurchaseReturnRecord(Id) {
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
                    url: '/Transaction/DeletetPurchaseReturn?id=' + Id + '',
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        if (result.ResponseCode == 200) {
                            GetPurchaseReturns();
                            toastr.success(result.SuccessMsg);
                        }
                        else {
                            toastr.error(result.ErrorMsg);
                        }

                    },
                    error: function (error) {
                        console.log(error);
                        $('#loader').hide();
                    }
                });
            }
        });
    }
});