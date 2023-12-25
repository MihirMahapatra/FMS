$(function () {
    //default date
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;

    $("#TransactionLink").addClass("active");
    $("#ProductionLink").addClass("active");
    $("#ProductionLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //----------------------------------------varible declaration-----------------------------------------//
    var RawMaterialDetailTable = $('#tblRawMaterialDetails').DataTable({
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
    var ProuctionEntryTable = $('#tblProuctionEntry').DataTable({
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
    const ProductionNo = $('input[name="ProductionNo"]');
    const ProductionDate = $('input[name="ProductionDate"]');
    ProductionDate.val(todayDate);
    const ddlFinishedGood = $('select[name="ddlFinishedGoodId"]');
    const ddlLabour = $('select[name="ddlLabourId"]');

    //----------------------------------------Production Screen-----------------------------------------//
    GetLastProductionNo();
    function GetLastProductionNo() {
        $.ajax({
            url: "/Transaction/GetLastProductionNo",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                ProductionNo.val(result.Data);
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    GetProductFinishedGood();
    function GetProductFinishedGood() {
        $.ajax({
            url: "/Transaction/GetProductFinishedGood",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddlFinishedGood.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlFinishedGood.append(defaultOption);
                    $.each(result.products, function (key, item) {
                        var option = $('<option></option>').val(item.ProductId).text(item.ProductName);
                        ddlFinishedGood.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    GetLabours()
    function GetLabours() {
        $.ajax({
            url: "/Transaction/GetProductionLabours",
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
            },
            error: function (errormessage) {
                console.log(errormessage);
            }
        });
    }
    $(document).on('click', '.addBtn', function () {
        var uniqueId = 'ddlitem' + new Date().getTime();

        var html = '<tr>';
        html += '<td><div class="form-group"><select required class="form-control form-control-sm select2bs4 finishedGood finishedGood_' + uniqueId + '" style="width: 100%;" id="' + uniqueId + '"></select></div></td>';
        html += '<td> <div class="form-group"><select required class=" form-control select2bs4 labour_' + uniqueId + ' labour" style="width: 100%;"></select></div></td>';
        html += ' <td><div class="form-group"><input type="text" class=" form-control" disabled></div></td>';
        html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
        html += '<td><div class="form-group"><input type="text" class="form-control" disabled></div></td>';
        html += '<td><div class="form-group"><input type="text" class="form-control" disabled></div></td>';
        html += '<td style="background-color:#ffe6e6;">';
        html += '<button class="btn btn-primary btn-link addBtn" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-plus"></i></button>';
        html += ' <button class="btn btn-primary btn-link deleteBtn" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
        html += '</td>';
        html += '</tr>';
        var newRow = ProuctionEntryTable.row.add($(html)).draw(false).node();

        $.ajax({
            url: "/Transaction/GetProductFinishedGood",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    var selectElement = $('.finishedGood_' + uniqueId + '');
                    selectElement.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    selectElement.append(defaultOption);
                    $.each(result.products, function (key, item) {
                        var option = $('<option></option>').val(item.ProductId).text(item.ProductName);
                        selectElement.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
        $.ajax({
            url: "/Transaction/GetProductionLabours",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    var selectElement = $('.labour_' + uniqueId + '');
                    selectElement.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    selectElement.append(defaultOption);
                    $.each(result.Labours, function (key, item) {
                        var option = $('<option></option>').val(item.LabourId).text(item.LabourName);
                        selectElement.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage);
            }
        });
        $('#tblProuctionEntry tbody').find('.select2bs4').select2({
            theme: 'bootstrap4'
        });
    });
    $(document).on('click', '.deleteBtn', function () {
        $(this).closest('tr').remove();
    });
    $(document).on('change', '.finishedGood', function () {
        var selectElement = $(this);
        var selectedProductId = selectElement.val();
        var tableBody = $('#tblRawMaterialDetails tbody');
        tableBody.empty();
        if (selectedProductId) {
            $.ajax({
                url: '/Transaction/GetProductionConfig?ProductId=' + selectedProductId,
                type: 'GET',
                contentType: 'application/json;charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.ResponseCode == 302) {
                        console.log(result);
                        var html = '';
                        $.each(result.Productions, function (key, item) {
                            html += '<tr>';
                            html += '<td>' + item.ProductName + '</td>';
                            html += '<td>' + item.Quantity + ' ' + item.Unit + '</td>';
                            html += '</tr >';
                        });
                    }
                    else {
                        html += '<tr>';
                        html += '<td colspan="2">No Record</td>';
                        html += '</tr >';
                    }
                    tableBody.append(html);
                },
                error: function (errormessage) {
                    console.log(errormessage);
                }
            });
            $.ajax({
                url: '/Transaction/GetProductLabourRate?ProductId=' + selectedProductId,
                type: 'GET',
                contentType: 'application/json;charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.ResponseCode == 302) {
                        if (result.LabourRate !== null) { 
                        var inputField = selectElement.closest('tr').find('input[type="text"]').eq(2);
                        inputField.val(result.LabourRate.Rate);
                        //console.log(result.LabourRate.Product.Unit.UnitName);
                    }
                }
            },
                error: function (errormessage) {
                    console.log(errormessage);
                }
            });
}
    });
$(document).on('change', '.labour', function () {
    var selectElement = $(this);
    var selectedLabourId = selectElement.val();
    if (selectedLabourId) {
        $.ajax({
            url: '/Transaction/GetLabourDetailById?LabourId=' + selectedLabourId,
            type: 'GET',
            contentType: 'application/json;charset=utf-8',
            dataType: 'json',
            success: function (result) {
                console.log(result)
                if (result.ResponseCode == 302) {
                    var inputField = selectElement.closest('tr').find('input[type="text"]').eq(0);
                    inputField.val(result.Labour.LabourType.Labour_Type);
                }
            },
            error: function (errormessage) {
                console.log(errormessage);
            }
        });
    }
});
$('#tblProuctionEntry tbody').on('change', 'input[type="text"]', function () {
    var row = $(this).closest('tr');
    var quantity = parseFloat(row.find('input:eq(1)').val());
    var rate = parseFloat(row.find('input:eq(2)').val());
    var amount = quantity * rate;
    row.find('input:eq(3)').val(amount.toFixed(2));
});
$('#btnSave').on('click', function () {
    if (!ProductionDate.val()) {
        toastr.error('Production Date  Is Required.');
        return;
    } else {
        var rowData = [];
        $('#tblProuctionEntry tbody tr').each(function () {
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
            ProductionNo: ProductionNo.val(),
            ProductionDate: ProductionDate.val(),
            rowData: rowData
        };
        console.log(requestData)
        $.ajax({
            type: "POST",
            url: '/Transaction/CreateProductionEntry',
            dataType: 'json',
            data: JSON.stringify(requestData),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                if (Response.ResponseCode == 201) {
                    toastr.success(Response.SuccessMsg);
                    ProductionDate.val('');
                    ProuctionEntryTable.clear().draw();
                }
                else {
                    toastr.error(Response.ErrorMsg);
                }
                GetLastProductionNo();
            },
            error: function (error) {
                console.log(error);
            }
        });
    }

});

//----------------------------------------Production List-----------------------------------------//
//Edit
$('a[href="#ProductionEntryList"]').on('click', function () {
    GetProductionEntry();
});
function GetProductionEntry() {
    $('#loader').show();
    $('.ProductionEntryListTable').empty();
    $.ajax({
        url: "/Transaction/GetProductionEntry",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#loader').hide();
            var html = '';
            html += '<table class="table table-bordered table-hover text-center mt-1 ProductionEntryListTable" style="width:100%">';
            html += '<thead>'
            html += '<tr>'
            html += '<th hidden>ProductionEntryId</th>'
            html += '<th>Production No</th>'
            html += '<th>Production Date</th>'
            html += '<th>Product Name</th>'
            html += '<th>Labour Name</th>'
            html += '<th>Labour Type</th>'
            html += '<th>Quantity</th>'
            html += '<th>Rate</th>'
            html += '<th>Amount</th>'
            html += '<th>Action</th>'
            html += '</tr>'
            html += '</thead>'
            html += '<tbody>';
            if (result.ResponseCode == 302) {

                $.each(result.ProductionEntries, function (key, item) {
                    html += '<tr>';
                    html += '<td hidden>' + item.ProductionEntryId + '</td>';
                    html += '<td>' + item.ProductionNo + '</td>';
                    const ModifyDate = item.ProductionDate;
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
                    if (item.Product !== null) {
                        html += '<td>' + item.Product.ProductName + '</td>';
                    }
                    else {
                        html += '<td>' - '</td>';
                    }
                    if (item.Labour !== null) {
                        html += '<td>' + item.Labour.LabourName + '</td>';
                    }
                    else {
                        html += '<td>' - '</td>';
                    }

                    html += '<td>' + item.LabourType + '</td>';
                    html += '<td>' + item.Quantity + '</td>';
                    html += '<td>' + item.Rate + '</td>';
                    html += '<td>' + item.Amount + '</td>';
                    html += '<td style="background-color:#ffe6e6;">';
                    html += '<button class="btn btn-primary btn-link btn-sm btn-productionentry-edit"   id="btnProductionEntryEdit_' + item.ProductionEntryId + '"     data-id="' + item.ProductionEntryId + '" data-toggle="modal" data-target="#modal-edit-production-entry" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                    html += ' <button class="btn btn-primary btn-link btn-sm btn-productionentry-delete" id="btnProductionEntryDelete_' + item.ProductionEntryId + '"   data-id="' + item.ProductionEntryId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                    html += '</td>';
                    html += '</tr >';
                });
            }
            else {
                html += '<tr>';
                html += '<td colspan="10">No Record</td>';
                html += '</tr >';
            }
            html += ' </tbody>';
            html += '</table >';
            $('.tblProductionEntryList').html(html);
            if (!$.fn.DataTable.isDataTable('.ProductionEntryListTable')) {
                var table = $('.ProductionEntryListTable').DataTable({
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
$(document).on('click', '.btn-productionentry-edit', (event) => {
    const value = $(event.currentTarget).data('id');
    EditProductionEntry(value);
});
function EditProductionEntry(Id) {
    var $tr = $('#btnProductionEntryEdit_' + Id + '').closest('tr');
    var productionNo = $tr.find('td:eq(1)').text().trim();
    var productionDate = $tr.find('td:eq(2)').text().trim();
    var productId = $tr.find('td:eq(3)').text().trim();
    var labourId = $tr.find('td:eq(4)').text().trim();
    var labourType = $tr.find('td:eq(5)').text().trim();
    var quantity = $tr.find('td:eq(6)').text().trim();
    var rate = $tr.find('td:eq(7)').text().trim();
    var amount = $tr.find('td:eq(8)').text().trim();
    //fill Modal data
    $('input[name="mdlProductionEntryId"]').val(Id);
    $('input[name="mdlProductionNo"]').val(productionNo);
    $('input[name="mdlProductionDate"]').val(productionDate);
    $('input[name="mdlLabourType"]').val(labourType);
    $('input[name="mdlQuantity"]').val(quantity);
    $('input[name="mdlRate"]').val(rate);
    $('input[name="mdlAmount"]').val(amount);
    $.ajax({
        url: "/Transaction/GetProductFinishedGood",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var selectElement = $('select[name="mdlProductId"]');
            if (result.ResponseCode == 302) {
                selectElement.empty();
                var defaultOption = $('<option></option>').val('').text('--Select Option--');
                selectElement.append(defaultOption);
                $.each(result.products, function (key, item) {
                    var option = $('<option></option>').val(item.ProductId).text(item.ProductName);
                    if (item.ProductName === productId) {
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
    $.ajax({
        url: "/Transaction/GetProductionLabours",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var selectElement = $('select[name="mdlLabourId"]')
            if (result.ResponseCode == 302) {
                selectElement.empty();
                var defaultOption = $('<option></option>').val('').text('--Select Option--');
                selectElement.append(defaultOption);
                $.each(result.Labours, function (key, item) {
                    var option = $('<option></option>').val(item.LabourId).text(item.LabourName);
                    if (item.LabourName === labourId) {
                        option.attr('selected', 'selected');
                    }
                    selectElement.append(option);
                });
            }
        },
        error: function (errormessage) {
            console.log(errormessage);
        }
    });
}
$('select[name="mdlProductId"]').change(function () {
    const ProductId = $(this).val();
    $.ajax({
        url: '/Transaction/GetProductLabourRate?ProductId=' + ProductId,
        type: 'GET',
        contentType: 'application/json;charset=utf-8',
        dataType: 'json',
        success: function (result) {
            console.log(result)
            if (result.ResponseCode == 302) {
                $('input[name="mdlRate"]').val(result.Data);
                if ($('input[name="mdlQuantity"]') !== '') {
                    var quantity = $('input[name="mdlQuantity"]').val();
                    var rate = $('input[name="mdlRate"]').val();
                    var amount = quantity * rate;
                    $('input[name="mdlAmount"]').val(amount)
                }
            }
        },
        error: function (errormessage) {
            console.log(errormessage);
        }
    });
});
$('select[name="mdlLabourId"]').change(function () {
    const LabourId = $(this).val();
    $.ajax({
        url: '/Transaction/GetLabourDetailById?LabourId=' + LabourId,
        type: 'GET',
        contentType: 'application/json;charset=utf-8',
        dataType: 'json',
        success: function (result) {
            console.log(result)
            if (result.ResponseCode == 302) {
                $('input[name="mdlLabourType"]').val(result.Labour.LabourType.Labour_Type);
            }
        },
        error: function (errormessage) {
            console.log(errormessage);
        }
    });
});
$('input[name="mdlQuantity"]').change(function () {
    var quantity = $('input[name="mdlQuantity"]').val();
    var rate = $('input[name="mdlRate"]').val();
    var amount = quantity * rate;
    $('input[name="mdlAmount"]').val(amount)
});
//Update
$('#modal-edit-production-entry').on('click', '.btnUpdate', (event) => {
    const data = {
        ProductionEntryId: $('input[name="mdlProductionEntryId"]').val(),
        ProductionNo: $('input[name="mdlProductionNo"]').val(),
        Date: $('input[name="mdlProductionDate"]').val(),
        Fk_ProductId: $('select[name="mdlProductId"]').val(),
        Fk_LabourId: $('select[name="mdlLabourId"]').val(),
        LabourType: $('input[name="mdlLabourType"]').val(),
        Quantity: $('input[name="mdlQuantity"]').val(),
        Rate: $('input[name="mdlRate"]').val(),
        Amount: $('input[name="mdlAmount"]').val(),
        LedgerId: $('input[name="mdlLedgerId"]').val(),
    }

    $.ajax({
        type: "POST",
        url: '/Transaction/UpdateProductionEntry',
        dataType: 'json',
        data: JSON.stringify(data),
        contentType: "application/json;charset=utf-8",
        success: function (Response) {
            $('#modal-edit-production-entry').modal('hide');
            GetProductionEntry();
            if (Response.ResponseCode == 200) {
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
});
//Delete Operation
$(document).on('click', '.btn-productionentry-delete', (event) => {
    const value = $(event.currentTarget).data('id');
    DeleteProductionEntry(value);
});
function DeleteProductionEntry(Id) {
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
                url: '/Transaction/DeleteProductionEntry?id=' + Id + '',
                type: "POST",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.ResponseCode == 200) {
                        toastr.success(result.SuccessMsg);
                        GetProductionEntry();
                        GetLastProductionNo();
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