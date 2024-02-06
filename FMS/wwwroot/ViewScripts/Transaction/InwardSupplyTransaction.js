$(function () {
    //Default Date 
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;
    $("#TransactionLink").addClass("active");
    $("#InwardSupplyTransactionLink").addClass("active");
    $("#InwardSupplyTransactionLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //----------------------------------------varible declaration-----------------------------------------//
    const inwardSupplyOrderId = $('input[name="hdnInwardSupplyOrderId"]');
    const transactionDate = $('input[name="TransactionDate"]');
    transactionDate.val(todayDate);
    const transactionNo = $('input[name="TransactionNo"]');
    const ddlBranch = $('select[name="ddlBranchId"]');
    const ddlProductType = $('select[name="ddlProductTypeId"]');
    const totalAmout = $('input[name="GrandTotal"]');
    //var InwardSupplyTable = $('#tblInwardSupply').DataTable({
    //    "paging": false,
    //    "lengthChange": false,
    //    "searching": false,
    //    "ordering": true,
    //    "info": false,
    //    "autoWidth": false,
    //    "responsive": true,
    //    lengthMenu: [5, 10, 25, 50], // Set the available page lengths
    //    pageLength: 10,// Set the default page length to 5
    //});
    //-----------------------------------------Contorl Foucous Of Element----------------------------------//
    ddlBranch.focus();
    $('#addInwardSupplyRowBtn').on('keydown', function (e) {
        if (e.key === 'Insert' || e.keyCode === 45) {
           
        }
    });
    $(document).on('keydown', function (e) {
        if (e.key === 'Insert' || e.keyCode === 45) {
            e.preventDefault();
            $('#addInwardSupplyRowBtn').click();
        }
    });
    $('#addInwardSupplyRowBtn').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('#addInwardSupplyRowBtn').on('blur', function () {
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
    //-----------------------------------------------------Inward Supply Scren --------------------------------------------------//
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
    GetLastInwardSupply();
    function GetLastInwardSupply() {
        $.ajax({
            url: "/Transaction/GetLastInwardSupply",
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
    $('#addInwardSupplyRowBtn').on('click', addInwardSupplyRowBtn);
    function addInwardSupplyRowBtn() {
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
            html += '<td><div class="form-group"><select class="form-control form-control-sm select2bs4" style="width: 100%;" id="' + uniqueId + '"></select></div></td>';
            html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
            html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
            html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
            html += '<td><button class="btn btn-primary btn-link deleteBtn" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button></td>';
            html += '</tr>';
            var newRow = $('#tblInwardSupply tbody').append(html);
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
        $('#tblInwardSupply tbody').find('.select2bs4').select2({
            theme: 'bootstrap4'
        });
    }
    $(document).on('click', '.deleteBtn', function () {
        $(this).closest('tr').remove();
    });
    $('#tblInwardSupply tbody').on('change', 'input[type="text"]', function () {
        var row = $(this).closest('tr');
        var quantity = parseFloat(row.find('input:eq(1)').val());
        var rate = parseFloat(row.find('input:eq(2)').val());
        var amount = quantity * rate;
        row.find('input:eq(3)').val(amount.toFixed(2));
        var totalAmount = 0;
        $('#tblInwardSupply tbody tr').each(function () {
            var amount = parseFloat($(this).find('input:eq(3)').val());
            if (!isNaN(amount)) {
                totalAmount += amount;
            }
        });
        $('input[name="GrandTotal"]').val(totalAmount.toFixed(2));
    });
    $('#btnSave').on('click', CreateInwardSupply);
    function CreateInwardSupply() {
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
            $('#tblInwardSupply tbody tr').each(function () {
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
                Branch: ddlBranch.val(),
                Fk_ProductTypeId: ddlProductType.val(),
                TotalAmount: totalAmout.val(),
                rowData: rowData
            };
            $.ajax({
                type: "POST",
                url: '/Transaction/CreateInwardSupply',
                dataType: 'json',
                data: JSON.stringify(requestData),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#loader').hide();
                    if (Response.ResponseCode == 201) {
                        toastr.success(Response.SuccessMsg);
                        InwardSupplyTable.clear().draw();
                        transactionDate.val('');
                        totalAmout.val('');
                        GetLastInwardSupply();
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
    //-----------------------------------------------------InwardSupply List Scren --------------------------------------------------//
    //Update Operation
    $('a[href="#InwardSupplyList"]').on('click', function () {
        GetInwardSupply();
    });
    function GetInwardSupply() {
        $('#loader').show();
        $('.InwardSupplyListTable').empty();
        $.ajax({
            url: "/Transaction/GetInwardSupply",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#loader').hide();
                var html = '';
                html += '<table class="table table-bordered table-hover text-center mt-1 InwardSupplyListTable" style="width:100%">';
                html += '<thead>'
                html += '<tr>'
                html += '<th hidden>Inward Supply Order Id</th>'
                html += '<th>Trxn No</th>'
                html += '<th>Trxn Dt.</th>'
                html += '<th>From Branch</th>'
                html += '<th>Product Type</th>'
                html += '<th>Total Amount</th>'
                html += '<th>Action</th>'
                html += '</tr>'
                html += '</thead>'
                html += '<tbody>';
                if (result.ResponseCode == 302) {
                    $.each(result.InwardSupplies, function (key, item) {
                        html += '<tr>';
                        html += '<td hidden>' + item.InwardSupplyOrderId + '</td>';
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
                        html += '<button class="btn btn-primary btn-link btn-sm btn-inwardsupply-edit"   id="btnInwardSupplyEdit_' + item.InwardSupplyOrderId + '"     data-id="' + item.InwardSupplyOrderId + '" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-inwardsupply-delete" id="btnInwardSupplyDelete_' + item.InwardSupplyOrderId + '"   data-id="' + item.InwardSupplyOrderId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
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
                $('.tblInwardSupplyList').html(html);
                if (!$.fn.DataTable.isDataTable('.InwardSupplyListTable')) {
                    var table = $('.InwardSupplyListTable').DataTable({
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
    $(document).on('click', '.btn-inwardsupply-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        GetInwardSupplyById(value);
        $('#tab-InwardSupply').trigger('click');
        $('#btnSave').hide();
        $('#btnUpdate').show();
    });
    function GetInwardSupplyById(Id) {
        $.ajax({
            url: '/Transaction/GetInwardSupplyById?Id=' + Id + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                inwardSupplyOrderId.val(result.InwardSupply.InwardSupplyOrderId)
                const ModifytransactionDate = result.InwardSupply.TransactionDate;
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
                transactionNo.val(result.InwardSupply.TransactionNo)
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
                                if (item.BranchId === result.InwardSupply.FromBranch) {
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
                                if (item.ProductTypeId === result.InwardSupply.Fk_ProductTypeId) {
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
                totalAmout.val(result.InwardSupply.TotalAmount);
                //Fill Table Data
                InwardSupplyTable.clear().draw();
                $.each(result.InwardSupply.InwardSupplyTransactions, function (key, item) {
                    var html = '<tr>';
                    html += '<td  hidden><input type="hidden" class="form-control"  value=' + item.InwardSupplyTransactionId + '></td>';
                    html += '<td><div class="form-group"><select class="form-control form-control-sm select2bs4" style="width: 100%;" id="ddn_' + item.InwardSupplyTransactionId + '"> </select></div></td>';
                    html += '<td><div class="form-group"><input type="text" class="form-control" id="" value=' + item.Quantity + '></div></td>';
                    html += '<td><div class="form-group"><input type="text" class="form-control" id=""  value=' + item.Rate + '></div></td>';
                    html += '<td><div class="form-group"><input type="text" class="form-control" id=""  value=' + item.Amount + '></div></td>';
                    html += '<td><button class="btn btn-primary btn-link deleteBtn" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button></td>';
                    html += '</tr>';
                    var newRow = InwardSupplyTable.row.add($(html)).draw(false).node();
                    var selectElement = $('#ddn_' + item.InwardSupplyTransactionId);
                    selectElement.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    selectElement.append(defaultOption);
                    $.ajax({
                        url: '/Transaction/GetProductByType?ProductTypeId=' + result.InwardSupply.Fk_ProductTypeId + '',
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
                $('#tblInwardSupply tbody').find('.select2bs4').select2({
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
    $(document).on('click', '#btnUpdate', function () {
        
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
            $('#tblInwardSupply tbody tr').each(function () {
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
                SupplyId: inwardSupplyOrderId.val(),
                TransactionDate: transactionDate.val(),
                TransactionNo: transactionNo.val(),
                Branch: ddlBranch.val(),
                Fk_ProductTypeId: ddlProductType.val(),
                TotalAmount: totalAmout.val(),
                rowData: rowData
            };
            $.ajax({
                type: "POST",
                url: '/Transaction/UpdateInwardSupply',
                dataType: 'json',
                data: JSON.stringify(requestData),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#loader').hide();
                    if (Response.ResponseCode == 200) {
                        toastr.success(Response.SuccessMsg);
                        InwardSupplyTable.clear().draw();
                        transactionDate.val('');
                        totalAmout.val('');
                        GetLastInwardSupply();
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
    $(document).on('click', '.btn-inwardsupply-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeleteInwardSupply(value);
    });
    function DeleteInwardSupply(Id) {
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
                    url: '/Transaction/DeleteInwardSupply?id=' + Id + '',
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
                        GetInwardSupply();
                        GetLastInwardSupply();
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
            }
        });
    }
});