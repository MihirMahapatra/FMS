$(function () {
    $("#AdminLink").addClass("active");
    $("#ProductConfigLink").addClass("active");
    $("#ProductConfigLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    var ProductConfigTable = $('#tblProuctConfig').DataTable({
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
    //*********************************Variable Declaration**********************************************************************//
    const ddlFinishedGood = $('select[name="ddlFinishedGoodId"]');
    const ddlRawMateria = $('select[name="ddlRawMaterialId"]');
    GetFinishedGoods();
    function GetFinishedGoods() {
        $.ajax({
            url: "/Admin/GetFinishedGoods",
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
    $(document).on('click', '.addBtn', function () {
        var uniqueId = 'ddlitem' + new Date().getTime();
        var html = '<tr>';
        html += '<td><div class="form-group"><select class="form-control form-control-sm select2bs4 rawMaterial" style="width: 100%;" id="' + uniqueId + '"></select></div></td>';
        html += '<td><div class="form-group"><input type="text" class="form-control" value="0"></div></td>';
        html += '<td><div class="form-group"><input type="text" class="form-control" disabled></div></td>';
        html += '<td style="background-color:#ffe6e6;">';
        html += '<button class="btn btn-primary btn-link addBtn" id="addLedgerRowBtn" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-plus"></i></button>';
        html += ' <button class="btn btn-primary btn-link deleteBtn" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
        html += '</td>';
        html += '</tr>';
        var newRow = ProductConfigTable.row.add($(html)).draw(false).node();
        $.ajax({
            url: "/Admin/GetRawMaterials",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    var selectElement = $('#' + uniqueId);
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
        $('#tblProuctConfig tbody').find('.select2bs4').select2({
            theme: 'bootstrap4'
        });
    });
    $(document).on('click', '.deleteBtn', function () {
        $(this).closest('tr').remove();
    });
    GetRawMaterials()
    function GetRawMaterials() {
        $.ajax({
            url: "/Admin/GetRawMaterials",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {

                    ddlRawMateria.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlRawMateria.append(defaultOption);
                    $.each(result.products, function (key, item) {
                        var option = $('<option></option>').val(item.ProductId).text(item.ProductName);
                        ddlRawMateria.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    $(document).on('change', '.rawMaterial', function () {
        var selectElement = $(this);
        var selectedProductId = selectElement.val();
        if (selectedProductId) {
            $.ajax({
                url: '/Admin/GetProductUnit?ProductId=' + selectedProductId,
                type: 'GET',
                contentType: 'application/json;charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    if (result.ResponseCode == 302) {
                        var inputField = selectElement.closest('tr').find('input[type="text"]').eq(1);
                        inputField.val(result.product.Unit.UnitName);
                    }
                },
                error: function (errormessage) {
                    console.log(errormessage);
                }
            });
        }
    });
    $('#btnSave').on('click', function () {
        var rowData = [];
        $('#tblProuctConfig tbody tr').each(function () {
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
            FinishedGoodId: ddlFinishedGood.val(),
            RowData: rowData
        };
        $.ajax({
            type: "POST",
            url: '/Admin/CreateProductConfig',
            dataType: 'json',
            data: JSON.stringify(requestData),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                if (Response.ResponseCode == 201) {
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
    //*********************************Production List**********************************************************************//
    $('a[href="#ProductionConfigList"]').on('click', function () {
        GetProductionConfig();
    });
    function GetProductionConfig() {
        $('.ProductionConfigListTable').empty();
        $.ajax({
            url: "/Admin/GetProductionConfig",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                var html = '';
                html += '<table class="table table-bordered table-hover text-center mt-1 ProductionConfigListTable" style="width:100%">';
                html += '<thead>'
                html += '<tr>'
                html += '<th hidden>ProductionId</th>'
                html += '<th>FinishedGood</th>'
                html += '<th>RawMaterial</th>'
                html += '<th>Quantity</th>'
                html += '<th>Unit</th>'
                html += '<th>Action</th>'
                html += '</tr>'
                html += '</thead>'
                html += '<tbody>';
                if (result.ResponseCode == 302) {
                    $.each(result.Productions, function (key, item) {
                        html += '<tr>';
                        html += '<td hidden>' + item.ProductionId + '</td>';
                        html += '<td>' + item.FinishedGoodName + '</td>';
                        html += '<td>' + item.RawMaterialName + '</td>';
                        html += '<td>' + item.Quantity + '</td>';
                        html += '<td>' + item.Unit + '</td>';
                        html += '<td style="background-color:#ffe6e6;">';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-production-config-edit"   id="btnProductionConfigEdit_' + item.ProductionId + '"     data-id="' + item.ProductionId + '" data-toggle="modal" data-target="#modal-edit-production-config" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-production-config-update" id ="btnProductionConfigUpdate_' + item.ProductionId + '" data-id="' + item.ProductionId + '" style = "border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;display:none" > <i class="fa-solid fa-floppy-disk"></i></button >';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-production-config-cancel" id="btnProductionConfigCancel_' + item.ProductionId + '"   data-id="' + item.ProductionId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px; display:none"> <i class="fa-solid fa-rectangle-xmark"></i></button>';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-production-config-delete" id="btnProductionConfigDelete_' + item.ProductionId + '"   data-id="' + item.ProductionId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                        html += '</td>';
                        html += '</tr >';
                    });
                }
                else {
                    html += '<tr>';
                    html += '<td colspan="6">No Record</td>';
                    html += '</tr >';
                }
                html += ' </tbody>';
                html += '</table >';
                $('.tblProductionConfigList').html(html);
                if (!$.fn.DataTable.isDataTable('.ProductionConfigListTable')) {
                    var table = $('.ProductionConfigListTable').DataTable({
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
    $(document).on('click', '.btn-production-config-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        EditProductConfig(value);
    });
    function EditProductConfig(Id) {
        var $tr = $('#btnProductionConfigEdit_' + Id + '').closest('tr');
        var finishedGood = $tr.find('td:eq(1)').text().trim();
        var RawMaterial = $tr.find('td:eq(2)').text().trim();
        var quantity = $tr.find('td:eq(3)').text().trim();
        var unit = $tr.find('td:eq(4)').text().trim();

        $tr.find('td:eq(3)').html('<div class="form-group"><input type="text" class="form-control" value="' + quantity + '"></div>');
        var html = '';
        $.ajax({
            url: "/Admin/GetFinishedGoods",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    var html = '';
                    html += '<div class="form-group">';
                    html += '<select class="form-control select2bs4" style="width: 100%;" name="FinishedGoodId">';
                    $.each(result.products, function (key, item) {
                        if (item.ProductName === finishedGood) {
                            html += '<option value="' + item.ProductId + '" selected>' + item.ProductName + '</option>';
                        } else {
                            html += '<option value="' + item.ProductId + '">' + item.ProductName + '</option>';
                        }
                    });
                    html += '</select>';
                    html += '</div>';
                    $tr.find('td:eq(1)').html(html);
                    $tr.find('.select2bs4').select2({
                        theme: 'bootstrap4'
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
        $.ajax({
            url: "/Admin/GetRawMaterials",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    var html = '';
                    html += '<div class="form-group">';
                    html += '<select class="form-control select2bs4" style="width: 100%;" name="RawMaterialId">';
                    $.each(result.products, function (key, item) {
                        if (item.ProductName === RawMaterial) {
                            html += '<option value="' + item.ProductId + '" selected>' + item.ProductName + '</option>';
                        } else {
                            html += '<option value="' + item.ProductId + '">' + item.ProductName + '</option>';
                        }
                    });
                    html += '</select>';
                    html += '</div>';
                    $tr.find('td:eq(2)').html(html); // Update HTML content first
                    $tr.find('.select2bs4').select2({
                        theme: 'bootstrap4'
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
        $tr.find('#btnProductionConfigEdit_' + Id + ', #btnProductionConfigDelete_' + Id + '').hide();
        $tr.find('#btnProductionConfigUpdate_' + Id + ',#btnProductionConfigCancel_' + Id + '').show();
    }
    $(document).on('click', '.btn-production-config-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeleteProductConfig(value);
    });
    function DeleteProductConfig(id) {
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
                    url: '/Admin/DeleteProductConfig?id=' + id + '',
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
                        GetProductionConfig();
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
            }
        });
    }
    $(document).on('click', '.btn-production-config-update', (event) => {
        const value = $(event.currentTarget).data('id');
        UpdateProductConfig(value);
    });
    function UpdateProductConfig(Id) {
        var $tr = $('#btnProductionConfigUpdate_' + Id + '').closest('tr');
        const data = {
            ProductionId: Id,
            FinishedGoodId: $tr.find('Select').eq(0).find('option:selected').val(),
            RawMaterialId: $tr.find('Select').eq(1).find('option:selected').val(),
            Quantity: $tr.find('input[type="text"]').eq(0).val(),
        }
        $.ajax({
            type: "POST",
            url: '/Admin/UpdateProductConfig',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                GetProductionConfig();
                if (Response.ResponseCode = 200) {
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
    $(document).on('click', '.btn-production-config-cancel', (event) => {
        const value = $(event.currentTarget).data('id');
        CancelProductConfig(value);
    });
    function CancelProductConfig(Id){
        var $tr = $('#btnProductionConfigCancel_' + Id + '').closest('tr');
        var RawMaterial = $tr.find('Select').eq(0).find('option:selected').text();
        var FinishedGood = $tr.find('Select').eq(1).find('option:selected').text();
        var Quantity = $tr.find('input[type="text"]').eq(0).val();

        $tr.find('td:eq(1)').text(RawMaterial);
        $tr.find('td:eq(2)').text(FinishedGood);
        $tr.find('td:eq(3)').text(Quantity);
        $tr.find('#btnProductionConfigEdit_' + Id + ', #btnProductionConfigDelete_' + Id + '').show();
        $tr.find('#btnProductionConfigUpdate_' + Id + ',#btnProductionConfigCancel_' + Id + '').hide();
    }
});