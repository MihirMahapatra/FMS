﻿$(function () {
    $("#AdminLink").addClass("active");
    $("#AlternateUnitLink").addClass("active");
    $("#AlternateUnitLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    /***************************************Variable Declaration***********************************************************/
    const ProductId = $('select[name="ProductId"]');
    const UnitName = $('#Unit');
    const HdnUnitId = $('#HdnUnitId');
    const AlternateQty = $('input[name = "AlternateQty"]');
    const AlternateUnit = $('input[name = "AlternateUnit"]');
    LoadProducts();
    function LoadProducts() {
        ProductId.empty();
        var defaultOption = $('<option></option>').val('').text('--Select Product--');
        ProductId.append(defaultOption);
        $.ajax({
            url: "/Admin/GetAllProducts",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    $.each(result.products, function (key, item) {
                        var option = $('<option></option>').val(item.ProductId).text(item.ProductName);
                        ProductId.append(option);
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
    LoadAlternateUnit();
    function LoadAlternateUnit() {
        $('#loader').show();
        $('.AlternateUnitTable').empty();
        $.ajax({
            url: "/Admin/GetAlternateUnits",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#loader').hide();
                var html = '';
                html += '<table class="table table-bordered table-hover text-center mt-1 AlternateUnitTable" style="width:100%">';
                html += '<thead>'
                html += '<tr>'
                html += '<th hidden>AlternateUnit Id</th>'
                html += '<th>Product Name</th>'
                html += '<th colspan=2>Unit</th>'
                html += '<th colspan=2>Alternate Unit</th>'
                html += '<th>Action</th>'
                html += '</tr>'
                html += '</thead>'
                html += '<tbody>';
                if (result.AlternateUnits !== null) {
                    $.each(result.AlternateUnits, function (key, item) {
                        html += '<tr>';
                        html += '<td hidden>' + item.AlternateUnitId + '</td>';
                        if (item.Product!==null) {
                            html += '<td>' + item.Product.ProductName + '</td>';
                        }
                        else {
                            html += '<td>-</td>';
                        }
                        if (item.Unit !== null) {
                            html += '<td>1</td>';
                            html += '<td>' + item.Unit.UnitName + '</td>';
                        }
                        else {
                            html += '<td>-</td>';
                            html += '<td>-</td>';
                        }
                        html += '<td>' + item.AlternateQuantity + '</td>';
                        html += '<td>' + item.AlternateUnitName + '</td>';
                        html += '<td style="background-color:#ffe6e6;">';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-alternateunit-edit"   id="btnAlternateUnitEdit_' + item.AlternateUnitId + '" data-id="' + item.AlternateUnitId + '" data-toggle="modal" data-target="#modal-edit-Product" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-alternateunit-delete" id="btnAlternateUnitDelete_' + item.AlternateUnitId + '"   data-id="' + item.AlternateUnitId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                        html += '</td>';
                        html += '</tr >';
                    });
                }
                else {
                    html += '<tr>';
                    html += '<td colspan="7">No record</td>';
                    html += '</tr>';
                }
                html += ' </tbody>';
                html += '</table >';
                $('.tbAlternateUnit').html(html);
                if (!$.fn.DataTable.isDataTable('.AlternateUnitTable')) {
                    $('.AlternateUnitTable').DataTable({
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
    ProductId.on('change', function () {
        var productId = ProductId.val();
        $.ajax({
            url: '/Admin/GetProductById?ProductId=' + productId + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                console.log(result);
                if (result.ResponseCode == 302) {
                    UnitName.text(result.product.Unit.UnitName);
                    HdnUnitId.val(result.product.Unit.UnitId);
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    });
    $(document).on('click', '.btn-alternateunit-create', CreateAlternateUnit);
    function CreateAlternateUnit() {
        if (!ProductId.val() || ProductId.val() === '--Select Option--') {
            toastr.error('Plz Select Prosuct.');
            ProductId.focus();
            return;
        }
        else if (!AlternateQty.val()) {
            toastr.error('Quantity Is Required.');
            ProductName.focus();
            return;
        }
        else if (!AlternateUnit.val()) {
            toastr.error('Alternate Unit Is Required.');
            ProductName.focus();
            return;
        }
        else {
            const data = {
                FK_ProductId: ProductId.val(),
                Fk_UnitId: HdnUnitId.val(),
                AlternateUnitName: AlternateUnit.val(),
                AlternateQuantity: AlternateQty.val()
            }
            $.ajax({
                type: "POST",
                url: '/Admin/CreateAlternateUnit',
                dataType: 'json',
                data: JSON.stringify(data),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    if (Response.ResponseCode == 201) {
                        toastr.success(Response.SuccessMsg);
                    }
                    else {
                        toastr.error(Response.ErrorMsg);
                    }
                    LoadAlternateUnit();
                    AlternateUnitName.val('');
                    AlternateQuantity.val('');
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }
    }

    $(document).on('click', '.btn-alternateunit-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeleteAlternateUnit(value);
    });
    function DeleteAlternateUnit(Id) {
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
                    url: '/Admin/DeleteAlternateUnit?id=' + Id + '',
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
                        LoadAlternateUnit();
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
            }
        });
    }
});