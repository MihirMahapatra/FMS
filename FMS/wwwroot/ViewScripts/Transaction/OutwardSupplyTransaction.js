
$(function () {
    //Default Date 
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;
    $("#TransactionLink").addClass("active");
    $("#OutwardSupplyTransactionLink").addClass("active");
    $("#OutwardSupplyTransactionLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //----------------------------------------varible declaration-----------------------------------------//
    const outwardSupplyOrderId = $('input[name="hdnOutwardSupplyOrderId"]');
    const transactionDate = $('input[name="TransactionDate"]');
    transactionDate.val(todayDate);
    const transactionNo = $('input[name="TransactionNo"]');
    const challanno = $('input[name="ChallanNo"]');
    const vehicleno = $('input[name="VehicleNo"]');
    const ddlBranch = $('select[name="ddlBranchId"]');
    const ddlProductType = $('select[name="ddlProductTypeId"]');
    const totalAmout = $('input[name="GrandTotal"]');
    var OutwardSupplyTable = $('#tblOutwardSupply').DataTable({
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
    //-----------------------------------------Contorl Foucous Of Element----------------------------------//
    ddlBranch.focus();
    $('#addOutwardSupplyRowBtn').on('keydown', function (e) {
        if (e.key === 'Insert' || e.keyCode === 45) {

        }
    });
    $(document).on('keydown', function (e) {
        if (e.key === 'Insert' || e.keyCode === 45) {
            e.preventDefault();
            $('#addOutwardSupplyRowBtn').click();
        }
    });
    $('#addOutwardSupplyRowBtn').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('#addOutwardSupplyRowBtn').on('blur', function () {
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
    //-----------------------------------------------------OutWard Supply Scren --------------------------------------------------//
    GetAllBranch();
    function GetAllBranch() {
        $.ajax({
            url: "/Transaction/GetAllBranch",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddlBranch.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlBranch.append(defaultOption);
                    $.each(result.Branches, function (key, item) {
                        var option = $('<option></option>').val(item.BranchId).text(item.BranchName);
                        ddlBranch.append(option);
                    });
                }
                else {
                    ddlBranch.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlBranch.append(defaultOption);
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
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
    GetLastOutwardSupply();
    function GetLastOutwardSupply() {
        $.ajax({
            url: "/Transaction/GetLastOutwardSupply",
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
    $('#addOutwardSupplyRowBtn').on('click', addOutwardSupplyRowBtn);
    function addOutwardSupplyRowBtn() {
        var uniqueId = 'ddlitem' + new Date().getTime();
        var selectedOptionText = $("select[name='ddlProductTypeId'] option:selected").text();
        var html = '';
        if (!ddlProductType.val() || ddlProductType.val() === '--Select Option--') {
            toastr.error('Pls select Product Type First');
            ddlProductType.focus();
            return;
        }
        else {
            html = '<tr>';
            html += '<td hidden><input type="hidden" class="form-control" value="0"></td>';
            html += '<td><div class="form-group"><select class="form-control form-control-sm select2bs4 Products" style="width: 100%;" id="' + uniqueId + '"></select></div></td>';
            html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
            html += '<td><div class="form-group"><input type="text" class="form-control" disabled value="N/A"></div></td>';
            html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
            html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
            html += '<td><button class="btn btn-primary btn-link deleteBtn" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button></td>';
            html += '</tr>';
            var newRow = OutwardSupplyTable.row.add($(html)).draw(false).node();
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
        $('#tblOutwardSupply tbody').find('.select2bs4').select2({
            theme: 'bootstrap4'
        });
    }
    $(document).on('click', '.deleteBtn', function () {
        var row = OutwardSupplyTable.row($(this).closest('tr'));
        row.remove().draw();
    });
    $(document).on('change', '.Products', function () {
        var selectElement = $(this);
        var selectedProductId = selectElement.val();
        if (selectedProductId) {
            $.ajax({
                url: '/Admin/GetProductById?ProductId=' + selectedProductId + '',
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.ResponseCode == 302) {
                        var Textbox = selectElement.closest('tr').find('input[type="text"]');
                        for (var i = 0; i < 7; i++) {
                            Textbox.eq(i).val('0');
                        }
                       
                        Textbox.eq(1).val(result.Product.Unit.UnitName);
                    }
                },
                error: function (errormessage) {
                    console.log(errormessage);
                }
            });
           
        }
    });
    $('#tblOutwardSupply tbody').on('change', 'input[type="text"]', function () {
        var row = $(this).closest('tr');
        var quantity = parseFloat(row.find('input:eq(1)').val());
        var rate = parseFloat(row.find('input:eq(3)').val());
        var amount = quantity * rate;
        row.find('input:eq(4)').val(amount.toFixed(2));
        var totalAmount = 0;
        $('#tblOutwardSupply tbody tr').each(function () {
            var amount = parseFloat($(this).find('input:eq(4)').val());
            if (!isNaN(amount)) {
                totalAmount += amount;
            }
        });
        $('input[name="GrandTotal"]').val(totalAmount.toFixed(2));
    });
    $('#btnSave').on('click', CreateOutwardSupply);
    function CreateOutwardSupply() {

        if (!transactionDate.val()) {
            toastr.error('TransactionDate Is Required.');
            transactionDate.focus();
            return;
        }
        else if (!ddlBranch.val() || ddlBranch.val() === '--Select Option--') {
            toastr.error('Branch Is Required.');
            ddlBranch.focus();
            return;
        }
        else if (!ddlProductType.val() || ddlProductType.val() === '--Select Option--') {
            toastr.error('Product Type Is Required.');
            ddlProductType.focus();
            return;
        }
        else {
            $('#loader').show();
            var rowData = [];
            $('#tblOutwardSupply tbody tr').each(function () {
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
                ChallanNo: challanno.val(),
                VehicleNo: vehicleno.val(),
                Branch: ddlBranch.val(),
                Fk_ProductTypeId: ddlProductType.val(),
                TotalAmount: totalAmout.val(),
                rowData: rowData
            };
            $.ajax({
                type: "POST",
                url: '/Transaction/CreateOutwardSupply',
                dataType: 'json',
                data: JSON.stringify(requestData),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#loader').hide();
                    if (Response.ResponseCode == 201) {
                        toastr.success(Response.SuccessMsg);
                        OutwardSupplyTable.clear().draw();
                        transactionDate.val('');
                        totalAmout.val('');
                        challanno.val('');
                        vehicleno.val('');
                        GetLastOutwardSupply();
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
    //-----------------------------------------------------OutwardSupply List Scren --------------------------------------------------//
    //Update Operation
    $('a[href="#OutwardSupplyList"]').on('click', function () {
        GetOutwardSupply();
    });
    function GetOutwardSupply() {
        $('#loader').show();
        $('.OutwardSupplyListTable').empty();
        $.ajax({
            url: "/Transaction/GetOutwardSupply",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#loader').hide();
                var html = '';
                html += '<table class="table table-bordered table-hover text-center mt-1 OutwardSupplyListTable" style="width:100%">';
                html += '<thead>'
                html += '<tr>'
                html += '<th hidden>Outward Supply Order Id</th>'
                html += '<th>Trxn No</th>'
                html += '<th>Trxn Dt.</th>'
                html += '<th>To Branch</th>'
                html += '<th>Product Type</th>'
                html += '<th>Total Amount</th>'
                html += '<th>Action</th>'
                html += '</tr>'
                html += '</thead>'
                html += '<tbody>';
                if (result.ResponseCode == 302) {
                    $.each(result.OutwardSupplies, function (key, item) {
                        html += '<tr>';
                        html += '<td hidden>' + item.OutwardSupplyOrderId + '</td>';
                        html += '<td>' + item.TransactionNo + '</td>';
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
                        html += '<td>' + formattedDate + '</td>';
                        html += '<td>' + item.BranchName + '</td>';
                        if (item.ProductType !== null) {
                            html += '<td>' + item.ProductType.Product_Type + '</td>';
                        }
                        else {
                            html += '<td>' - '</td>';
                        }
                        html += '<td>' + item.TotalAmount + '</td>';
                        html += '<td style="background-color:#ffe6e6;">';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-outwardsupply-edit"   id="btnOutwardSupplyEdit_' + item.OutwardSupplyOrderId + '"     data-id="' + item.OutwardSupplyOrderId + '" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-outwardsupply-delete" id="btnOutwardSupplyDelete_' + item.OutwardSupplyOrderId + '"   data-id="' + item.OutwardSupplyOrderId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                        html += '</td>';
                        html += '</tr >';
                    });
                }
                else {
                    html += '<tr>';
                    html += '<td colspan="7">No Record</td>';
                    html += '</tr >';
                }
                html += ' </tbody>';
                html += '</table >';
                $('.tblOutwardSupplyList').html(html);
                if (!$.fn.DataTable.isDataTable('.OutwardSupplyListTable')) {
                    var table = $('.OutwardSupplyListTable').DataTable({
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
    $(document).on('click', '.btn-outwardsupply-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        GetOutwardSupplyById(value);
        $('#tab-OutwardSupply').trigger('click');
        $('#btnSave').hide();
        $('#btnUpdate').show();
    });
    function GetOutwardSupplyById(Id) {
        $.ajax({
            url: '/Transaction/GetOutwardSupplyById?Id=' + Id + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                outwardSupplyOrderId.val(result.OutwardSupply.OutwardSupplyOrderId)
                const ModifytransactionDate = result.OutwardSupply.TransactionDate;
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
                transactionNo.val(result.OutwardSupply.TransactionNo)
                $.ajax({
                    url: "/Transaction/GetAllBranch",
                    type: "GET",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result1) {
                        if (result1.ResponseCode == 302) {
                            ddlBranch.empty();
                            var defaultOption = $('<option></option>').val('').text('--Select Option--');
                            ddlBranch.append(defaultOption);
                            $.each(result1.Branches, function (key, item) {
                                var option = $('<option></option>').val(item.BranchId).text(item.BranchName);
                                if (item.BranchId === result.OutwardSupply.ToBranch) {
                                    option.attr('selected', 'selected');
                                }
                                ddlBranch.append(option);
                            });
                        }
                        else {
                            ddlBranch.empty();
                            var defaultOption = $('<option></option>').val('').text('--Select Option--');
                            ddlBranch.append(defaultOption);
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
                                if (item.ProductTypeId === result.OutwardSupply.Fk_ProductTypeId) {
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
                totalAmout.val(result.OutwardSupply.TotalAmount);
                challanno.val(result.OutwardSupply.ChallanNo);
                vehicleno.val(result.OutwardSupply.VehicleNo);
                //Fill Table Data
                OutwardSupplyTable.clear().draw();
                $.each(result.OutwardSupply.OutwardSupplyTransactions, function (key, item) {
                    var html = '<tr>';
                    html += '<td  hidden><input type="hidden" class="form-control"  value=' + item.OutwardSupplyTransactionId + '></td>';
                    html += '<td><div class="form-group"><select class="form-control form-control-sm select2bs4" style="width: 100%;" id="ddn_' + item.OutwardSupplyTransactionId + '"> </select></div></td>';
                    html += '<td><div class="form-group"><input type="text" class="form-control" id="" value=' + item.Quantity + '></div></td>';
                    html += '<td><div class="form-group"><input type="text" class="form-control" disabled value=' + item.Product.Unit.UnitName + '></div></td>';
                    html += '<td><div class="form-group"><input type="text" class="form-control" id=""  value=' + item.Rate + '></div></td>';
                    html += '<td><div class="form-group"><input type="text" class="form-control" id=""  value=' + item.Amount + '></div></td>';
                    html += '<td><button class="btn btn-primary btn-link deleteBtn" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button></td>';
                    html += '</tr>';
                    var newRow = OutwardSupplyTable.row.add($(html)).draw(false).node();
                    var selectElement = $('#ddn_' + item.OutwardSupplyTransactionId);
                    selectElement.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    selectElement.append(defaultOption);
                    $.ajax({
                        url: '/Transaction/GetProductByType?ProductTypeId=' + result.OutwardSupply.Fk_ProductTypeId + '',
                        type: "GET",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (result3) {
                            if (result3.ResponseCode == 302) {
                                $.each(result3.Products, function (key, item1) {
                                    var option = $('<option></option>').val(item1.ProductId).text(item1.ProductName);
                                    if (item.Fk_ProductId === item1.ProductId) {
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
                $('#tblOutwardSupply tbody').find('.select2bs4').select2({
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
    $('#btnUpdate').on('click', UpdateOutwardSupply);
    function UpdateOutwardSupply() {

        if (!transactionDate.val()) {
            toastr.error('TransactionDate Is Required.');
            transactionDate.focus();
            return;
        }
        else if (!ddlBranch.val() || ddlBranch.val() === '--Select Option--') {
            toastr.error('Branch Is Required.');
            ddlBranch.focus();
            return;
        }
        else if (!ddlProductType.val() || ddlProductType.val() === '--Select Option--') {
            toastr.error('Product Type Is Required.');
            ddlProductType();
            return;
        } else {
            $('#loader').show();
            var rowData = [];
            $('#tblOutwardSupply tbody tr').each(function () {
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
                SupplyId: outwardSupplyOrderId.val(),
                TransactionDate: transactionDate.val(),
                ChallanNo: challanno.val(),
                VehicleNo: vehicleno.val(),
                TransactionNo: transactionNo.val(),
                Branch: ddlBranch.val(),
                Fk_ProductTypeId: ddlProductType.val(),
                TotalAmount: totalAmout.val(),
                rowData: rowData
            };
            $.ajax({
                type: "POST",
                url: '/Transaction/UpdateOutwardSupply',
                dataType: 'json',
                data: JSON.stringify(requestData),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#loader').hide();
                    if (Response.ResponseCode == 200) {
                        toastr.success(Response.SuccessMsg);
                        OutwardSupplyTable.clear().draw();
                        transactionDate.val('');
                        totalAmout.val('');
                        GetLastOutwardSupply();
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
    $(document).on('click', '.btn-outwardsupply-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeleteOutwardSupply(value);
    });
    function DeleteOutwardSupply(Id) {
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
                    url: '/Transaction/DeleteOutwardSupply?id=' + Id + '',
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
                        GetOutwardSupply();
                        GetLastOutwardSupply();
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
            }
        });
    }
});