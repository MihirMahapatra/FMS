$(function () {
    //Default Date 
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;
    $("#TransactionLink").addClass("active");
    $("#DamageTransactionLink").addClass("active");
    $("#DamageTransactionLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //----------------------------------------varible declaration-----------------------------------------//
    const damageOrderId = $('input[name="hdnDamageOrderId"]');
    const ddlProductType = $('select[name="ddlProductTypeId"]');
    const ddlLabour = $('select[name="ddlLabourId"]');
    const transactionNo = $('input[name="TransactionNo"]');
    const transactionDate = $('input[name="TransactionDate"]');
    transactionDate.val(todayDate);
    const totalAmout = $('input[name="GrandTotal"]');
    const reason = $('textarea[name="Reason"]');
    var DamageTable = $('#tblDamage').DataTable({
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
    //-----------------------------------Contorl Foucous Of Element Damage----------------------------//
    ddlProductType.focus();
    reason.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    reason.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    ddlLabour.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    ddlLabour.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    ddlProductType.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    ddlProductType.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    $('#addDamageRowBtn').on('keydown', function (e) {
        if (e.key === 'Insert' || e.keyCode === 45) {

        }
    });
    $(document).on('keydown', function (e) {
        if (e.key === 'Insert' || e.keyCode === 45) {
            e.preventDefault();
            $('#addDamageRowBtn').click();
        }
    });
    $('#addDamageRowBtn').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('#addDamageRowBtn').on('blur', function () {
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
    //----------------------------------------------------- Damage Scren --------------------------------------------------//
    GetAllProductTypes();
    function GetAllProductTypes() {
        $.ajax({
            url: "/Transaction/GetAllProductTypes",
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
    GetAllLabours();
    function GetAllLabours() {
        $.ajax({
            url: "/Transaction/GetAllLabours",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddlLabour.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlLabour.append(defaultOption);
                    $.each(result.Labours, function (key, item) {
                        var option = $('<option></option>').val(item.LabourId).text(item.LabourName);
                        ddlLabour.append(option);
                    });
                }
                else {
                    ddlLabour.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlLabour.append(defaultOption);
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    GetLastDamageEntry();
    function GetLastDamageEntry() {
        $.ajax({
            url: "/Transaction/GetLastDamageEntry",
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
    $('#addDamageRowBtn').on('click', addDamageRowBtn);
    function addDamageRowBtn() {
        var uniqueId = 'ddlitem' + new Date().getTime();
        var html = '';
        if (!ddlProductType.val() || ddlProductType.val() === '--Select Option--') {
            toastr.error('Pls select Product Type First');
            ddlProductType.focus();
            return;
        }
        else {

            html = '<tr>';
            html += '<td hidden><input type="hidden" class="form-control" value="0"></td>';
            html += '<td><div class="form-group"><select class="form-control form-control-sm select2bs4 SelectedProduct" style="width: 100%;" id="' + uniqueId + '"></select></div></td>';
            html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
            html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
            html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
            html += '<td><button class="btn btn-primary btn-link deleteBtn" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button></td>';
            html += '</tr>';
            var newRow = DamageTable.row.add($(html)).draw(false).node();
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
                    }
                },
                error: function (errormessage) {
                    console.log(errormessage)
                }
            });
        }

        $('#tblDamage tbody').find('.select2bs4').select2({
            theme: 'bootstrap4'
        });
    }
    $(document).on('change', '.SelectedProduct', function () {
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
                        Textbox.eq(1).val(result.product.Price);
                    }
                },
                error: function (errormessage) {
                    console.log(errormessage);
                }
            });
        }
    });
    $(document).on('click', '.deleteBtn', function () {
        $(this).closest('tr').remove();
    });
    $('#tblDamage tbody').on('change', 'input[type="text"]', function () {
        var row = $(this).closest('tr');
        var quantity = parseFloat(row.find('input:eq(1)').val());
        var rate = parseFloat(row.find('input:eq(2)').val());
        var amount = quantity * rate;
        row.find('input:eq(3)').val(amount.toFixed(2));
        var totalAmount = 0;
        $('#tblDamage tbody tr').each(function () {
            var amount = parseFloat($(this).find('input:eq(3)').val());
            if (!isNaN(amount)) {
                totalAmount += amount;
            }
        });
        $('input[name="GrandTotal"]').val(totalAmount.toFixed(2));
    });
    $('#btnSave').on('click', CreateDamage);
    function CreateDamage() {
        if (!transactionDate.val()) {
            toastr.error('TransactionDate Is Required.');
            transactionDate.focus();
            return;
        }
        else if (!ddlProductType.val() || ddlProductType.val() === '--Select Option--') {
            toastr.error('Product Type Is Required.');
            ddlProductType.focus();
            return;
        }
        else if (!reason.val()) {
            toastr.error('Reason Is Required.');
            reason.focus();
            return;
        }
        else {
            $('#loader').show();
            var rowData = [];
            $('#tblDamage tbody tr').each(function () {
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
                TransactionDate: transactionDate.val(),
                TransactionNo: transactionNo.val(),
                Fk_LabourId: ddlLabour.val(),
                Fk_ProductTypeId: ddlProductType.val(),
                Reason: reason.val(),
                TotalAmount: totalAmout.val(),
                rowData: rowData
            };
            $.ajax({
                type: "POST",
                url: '/Transaction/CreateDamage',
                dataType: 'json',
                data: JSON.stringify(requestData),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#loader').hide();
                    if (Response.ResponseCode == 201) {
                        toastr.success(Response.SuccessMsg);
                        DamageTable.clear().draw();
                        transactionDate.val('');
                        totalAmout.val('');
                        reason.val('');
                        GetLastDamageEntry();
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
    //-----------------------------------------------------Damage List Scren --------------------------------------------------//
    $('a[href="#DamageList"]').on('click', function () {
        GetDamages();
    });
    function GetDamages() {
        $('#loader').show();
        $('.DamageListTable').empty();
        $.ajax({
            url: "/Transaction/GetDamages",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#loader').hide();
                var html = `
                  <table class="table table-bordered table-hover text-center mt-1 DamageListTable" style="width:100%">
                    <thead>
                      <tr>
                        <th hidden>Damage Order Id</th>
                        <th>Trxn No</th>
                        <th>Trxn Dt.</th>
                        <th>Labour</th>
                        <th>Product Type</th>
                        <th>Reason</th>
                        <th>Total Amount</th>
                        <th>Action</th>
                      </tr>
                    </thead>
                    <tbody>
                `;
                if (result.ResponseCode == 302) {
                    $.each(result.DamageOrders, function (key, item) {
                        let formattedDate = '';
                        const ModifyDate = item.TransactionDate;
                        if (ModifyDate) {
                            const dateObject = new Date(ModifyDate);
                            if (!isNaN(dateObject)) {
                                const day = String(dateObject.getDate()).padStart(2, '0');
                                const month = String(dateObject.getMonth() + 1).padStart(2, '0');
                                const year = dateObject.getFullYear();
                                formattedDate = `${day}/${month}/${year}`;
                            }
                        }
                        html += `
                      <tr>
                        <td hidden>${item.DamageOrderId}</td>
                        <td>${item.TransactionNo}</td>
                        <td>${formattedDate}</td>
                        <td>${item.Labour !== null ? item.Labour.LabourName : '-'}</td>
                        <td>${item.ProductType !== null ? item.ProductType.Product_Type : '-'}</td>
                        <td>${item.Reason}</td>
                        <td>${item.TotalAmount}</td>
                        <td style="background-color:#ffe6e6;">
                          <button class="btn btn-primary btn-link btn-sm btn-damage-edit" id="btnDamageEdit_${item.DamageOrderId}" data-id="${item.DamageOrderId}" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>
                          <button class="btn btn-primary btn-link btn-sm btn-damage-delete" id="btnDamageDelete_${item.DamageOrderId}" data-id="${item.DamageOrderId}" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>
                        </td>
                      </tr>
                    `;
                    });
                } else {
                    html += `
                    <tr>
                      <td colspan="8">No Record</td>
                    </tr>
                  `;
                }
                html += '</tbody></table>';

                $('.tblDamageList').html(html);
                if (!$.fn.DataTable.isDataTable('.DamageListTable')) {
                    var table = $('.DamageListTable').DataTable({
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
    //Update Operation
    $(document).on('click', '.btn-damage-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        GetDamageById(value);
        $('#tab-Damage').trigger('click');
        $('#btnSave').hide();
        $('#btnUpdate').show();
    });
    function GetDamageById(Id) {
        $.ajax({
            url: '/Transaction/GetDamageById?Id=' + Id + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                damageOrderId.val(result.DamageOrder.DamageOrderId)
                const ModifytransactionDate = result.DamageOrder.TransactionDate;
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
                transactionNo.val(result.DamageOrder.TransactionNo)
                $.ajax({
                    url: "/Transaction/GetAllLabours",
                    type: "GET",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result1) {
                        if (result1.ResponseCode == 302) {
                            ddlLabour.empty();
                            var defaultOption = $('<option></option>').val('').text('--Select Option--');
                            ddlLabour.append(defaultOption);
                            $.each(result1.Labours, function (key, item) {
                                var option = $('<option></option>').val(item.LabourId).text(item.LabourName);
                                if (item.LabourId === result.DamageOrder.Fk_LabourId) {
                                    option.attr('selected', 'selected');
                                }
                                ddlLabour.append(option);
                            });
                        }
                        else {
                            ddlLabour.empty();
                            var defaultOption = $('<option></option>').val('').text('--Select Option--');
                            ddlLabour.append(defaultOption);
                        }
                    },
                    error: function (errormessage) {
                        console.log(errormessage)
                    }
                });
                $.ajax({
                    url: "/Transaction/GetAllProductTypes",
                    type: "GET",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result2) {
                        if (result2.ResponseCode == 302) {
                            ddlProductType.empty();
                            var defaultOption = $('<option></option>').val('').text('--Select Option--');
                            ddlProductType.append(defaultOption);
                            $.each(result2.ProductTypes, function (key, item) {
                                var option = $('<option></option>').val(item.ProductTypeId).text(item.Product_Type);
                                if (item.ProductTypeId === result.DamageOrder.Fk_ProductTypeId) {
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
                totalAmout.val(result.DamageOrder.TotalAmount);
                reason.val(result.DamageOrder.Reason);
                //Fill Table Data
                DamageTable.clear().draw();
                $.each(result.DamageOrder.DamageTransactions, function (key, item) {
                    var html = '<tr>';
                    html += '<td  hidden><input type="hidden" class="form-control"  value=' + item.DamageTransactionId + '></td>';
                    html += '<td><div class="form-group"><select class="form-control form-control-sm select2bs4" style="width: 100%;" id="ddn_' + item.DamageTransactionId + '"> </select></div></td>';
                    html += '<td><div class="form-group"><input type="text" class="form-control" id="" value=' + item.Quantity + '></div></td>';
                    html += '<td><div class="form-group"><input type="text" class="form-control" id=""  value=' + item.Rate + '></div></td>';
                    html += '<td><div class="form-group"><input type="text" class="form-control" id=""  value=' + item.Amount + '></div></td>';
                    html += '<td><button class="btn btn-primary btn-link deleteBtn" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button></td>';
                    html += '</tr>';
                    var newRow = DamageTable.row.add($(html)).draw(false).node();
                    var selectElement = $('#ddn_' + item.DamageTransactionId);
                    selectElement.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    selectElement.append(defaultOption);
                    $.ajax({
                        url: '/Transaction/GetProductByType?ProductTypeId=' + result.DamageOrder.Fk_ProductTypeId + '',
                        type: "GET",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (result4) {
                            if (result4.ResponseCode == 302) {
                                $.each(result4.Products, function (key, item2) {
                                    var option = $('<option></option>').val(item2.ProductId).text(item2.ProductName);
                                    if (item.Fk_ProductId === item2.ProductId) {
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
                })
                $('#tblDamage tbody').find('.select2bs4').select2({
                    theme: 'bootstrap4'
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
    $(document).on('click', '.deleteBtn', function () {
        $(this).closest('tr').remove();
    });
    $(document).on('click', '#btnUpdate', function () {

        if (!transactionDate.val()) {
            toastr.error('TransactionDate Is Required.');
            transactionDate.focus();
            return;
        }
        else if (!ddlProductType.val() || ddlProductType.val() === '--Select Option--') {
            toastr.error('Product Type Is Required.');
            ddlProductType.focus();
            return;
        }
        else if (!reason.val()) {
            toastr.error('Reason Is Required.');
            reason.focus();
            return;
        } else {
            $('#loader').show();
            var rowData = [];
            $('#tblDamage tbody tr').each(function () {
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
                DamageId: damageOrderId.val(),
                TransactionDate: transactionDate.val(),
                TransactionNo: transactionNo.val(),
                Fk_LabourId: ddlLabour.val(),
                Fk_ProductTypeId: ddlProductType.val(),
                TotalAmount: totalAmout.val(),
                Reason: reason.val(),
                rowData: rowData
            };
            $.ajax({
                type: "POST",
                url: '/Transaction/UpdateDamage',
                dataType: 'json',
                data: JSON.stringify(requestData),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#loader').hide();
                    if (Response.ResponseCode == 200) {
                        toastr.success(Response.SuccessMsg);
                        DamageTable.clear().draw();
                        transactionDate.val('');
                        totalAmout.val('');
                        reason.val('');
                        GetLastDamageEntry();
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
    });
    //Delete Operation
    $(document).on('click', '.btn-damage-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeleteDamage(value);
    });
    function DeleteDamage(Id) {
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
                    url: '/Transaction/DeleteDamage?id=' + Id + '',
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        if (result.ResponseCode == 200) {
                            toastr.success(result.SuccessMsg);
                            GetDamages();
                            GetLastDamageEntry();
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