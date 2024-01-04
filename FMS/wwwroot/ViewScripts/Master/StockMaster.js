$(function () {
    StockMasterLink
    $("#MasterLink").addClass("active");
    $("#StockMasterLink").addClass("active");
    $("#StockMasterLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    /***************************************Variable Declaration***********************************************************/
    //mdl stoctedit
    const mdlStockId = $('input[name="mdlStockId"]');
    const mdlOpeningQty = $('input[name="mdlOpeningQty"]');
    const mdlOpeningRate = $('input[name="mdlOpeningRate"]');
    const mdlAvilableQty = $('input[name="mdlAvilableQty"]');
    const mdlMinQty = $('input[name="mdlMinQty"]');
    const mdlMaxQty = $('input[name="mdlMaxQty"]');
    //stock
    const openingQty = $('input[name = "OpeningQty"]')
    const openingRate = $('input[name = "OpeningRate"]')
    const minQty = $('input[name = "MinQty"]');
    const maxQty = $('input[name = "MaxQty"]')
    const productId = $('select[name="ProductId"]');
    const groupId = $('select[name="GroupId"]');
    const subGroupId = $('select[name="SubGroupId"]');

    //-----------------------------------Contorl Foucous Of Element   ProductMaster StockDetail----------------------------//
    groupId.focus();
    openingQty.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    openingQty.on('blur', function () {
        $(this).css('border-color', '');
    });
    openingRate.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    openingRate.on('blur', function () {
        $(this).css('border-color', '');
    });
    minQty.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    minQty.on('blur', function () {
        $(this).css('border-color', '');
    });
    maxQty.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    maxQty.on('blur', function () {
        $(this).css('border-color', '');
    });
    $('.btn-stock-create').on('keydown', function (e) {
        if (e.key === 'Enter' || e.keyCode === 13) {
            $('.btn-stock-create').click();
        }
    });
    $('.btn-stock-create').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('.btn-stock-create').on('blur', function () {
        $(this).css('background-color', '');
    });
    /***************************************Validation Section***********************************************************/
    openingQty.on("input", function () {
        var inputValue = $(this).val().replace(/[^0-9.]/g, '');
        $(this).val(inputValue);
    });
    minQty.on("input", function () {
        var inputValue = $(this).val().replace(/[^0-9.]/g, '');
        $(this).val(inputValue);
    });
    maxQty.on("input", function () {
        var inputValue = $(this).val().replace(/[^0-9.]/g, '');
        $(this).val(inputValue);
    });
    openingRate.on("input", function () {
        var inputValue = $(this).val().replace(/[^0-9.]/g, '');
        $(this).val(inputValue);
    });
    /*--------------------------------------------------------------- Stock Details--------------------------------------------*/
    LoadGroups();
    function LoadGroups() {
        groupId.empty();
        var defaultOption = $('<option></option>').val('').text('--Select Option--');
        groupId.append(defaultOption);
        $.ajax({
            url: "/Master/GetAllGroups",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    $.each(result.Groups, function (key, item) {
                        var option = $('<option></option>').val(item.GroupId).text(item.GroupName);
                        groupId.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    groupId.on('change', function () {
        var SelectedGroupId = groupId.val();
        if (SelectedGroupId) {
            subGroupId.prop("disabled", false);
            LoadSubGroup(SelectedGroupId);
        } else {
            subGroupId.prop("disabled", true);
            productId.prop("disabled", false);
        }
    });
    function LoadSubGroup(GroupId) {
        subGroupId.empty();
        var defaultOption = $('<option></option>').val('').text('--Select Option--');
        subGroupId.append(defaultOption);
        $.ajax({
            url: '/Master/GetSubGroups?GroupId=' + GroupId + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    $.each(result.SubGroups, function (key, item) {
                        var option = $('<option></option>').val(item.SubGroupId).text(item.SubGroupName);
                        subGroupId.append(option);
                    });
                }
                else {
                    LoadProducts(GroupId);
                    productId.prop("disabled", false);
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    subGroupId.on('change', function () {
        var SelectedGroupId = groupId.val();
        var SelectedSubGroupId = subGroupId.val();
        if (SelectedGroupId && SelectedSubGroupId) {
            productId.prop("disabled", false);
            LoadProducts(SelectedGroupId, SelectedSubGroupId)
        } else {
            productId.prop("disabled", true);
        }
    });
    function LoadProducts(GroupId, SubGroupId) {
        productId.empty();
        var defaultOption = $('<option></option>').val('').text('--Select Option--');
        productId.append(defaultOption);
        $.ajax({
            url: '/Master/GetProductsWhichNotInStock?GroupId=' + GroupId + '&&SubGroupId=' + SubGroupId + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $.each(result.products, function (key, item) {
                    var option = $('<option></option>').val(item.ProductId).text(item.ProductName);
                    productId.append(option);
                });
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    LoadStocks();
    function LoadStocks() {
        $('#loader').show();
        $('.tblStock').empty();

        $.ajax({
            url: "/Master/GetStocks",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#loader').hide();
                var html = '';
                html += '<table class="table table-bordered table-hover text-center mt-1 StockTable" style="width:100%">';
                html += '<thead>'
                html += '<tr>'
                html += '<th hidden>Stock Id</th>'
                html += '<th>Product Name</th>'
                html += '<th>Opening Qty</th>'
                html += '<th>Rate</th>'
                html += '<th>Amount</th>'
                html += '<th>Avilable Qty</th>'
                html += '<th>Min Qty</th>'
                html += '<th>Max Qty</th>'
                html += '<th>Action</th>'
                html += '</tr>'
                html += '</thead>'
                html += '<tbody>';
                if (result.Stocks !== null) {
                    $.each(result.Stocks, function (key, item) {
                        html += '<tr>';
                        html += '<td hidden>' + item.StockId + '</td>';
                        html += '<td>' + item.Product.ProductName + '</td>';
                        html += '<td>' + item.OpeningStock + '</td>';
                        html += '<td>' + item.Rate + '</td>';
                        html += '<td>' + item.Amount + '</td>';
                        if (item.AvilableStock < item.MinQty) {
                            html += '<td class="bg-danger text-white">' + item.AvilableStock + '</td>';
                        }
                        else {
                            html += '<td>' + item.AvilableStock + '</td>';
                        }
                        html += '<td>' + item.MinQty + '</td>';
                        html += '<td>' + item.MaxQty + '</td>';
                        html += '<td style="background-color:#ffe6e6;">';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-stock-edit"   id="btnStockEdit_' + item.StockId + '" data-id="' + item.StockId + '" data-toggle="modal" data-target="#modal-edit-Stock" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-stock-delete" id="btnStockDelete_' + item.StockId + '"   data-id="' + item.StockId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
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
                $('.tblStock').html(html);
                if (!$.fn.DataTable.isDataTable('.StockTable')) {
                    $('.StockTable').DataTable({
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
    //----------Insert Records----------//
    $(document).on('click', '.btn-stock-create', CreateStock);
    function CreateStock() {
        $('#loader').show();
        if (!productId.val() || productId.val() === '--Select Option--') {
            toastr.error('Product Name Is Required.');
            productId.focus();
            return;
        }
        else if (!openingQty.val()) {
            toastr.error('Opening Quantity Is Required.');
            openingQty.focus();
            return;
        } else if (!openingRate.val()) {
            toastr.error('Opening Rate Is Required.');
            openingRate.focus();
            return;
        }
        else if (!minQty.val()) {
            toastr.error('Min Quantity Is Required.');
            minQty.focus();
            return;
        }
        else if (!maxQty.val()) {
            toastr.error('Max Quantity Is Required.');
            maxQty.focus();
            return;
        }
        else {
            const data = {
                Fk_ProductId: productId.val(),
                OpeningStock: openingQty.val(),
                Rate: openingRate.val(),
                MinQty: minQty.val(),
                MaxQty: maxQty.val()
            };
            $.ajax({
                type: "POST",
                url: '/Master/CreateStock',
                dataType: 'json',
                data: JSON.stringify(data),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#loader').hide();
                    if (Response.ResponseCode == 201) {
                        toastr.success(Response.SuccessMsg);
                        LoadStocks()
                        LoadProductForStock();
                        openingQty.val('');
                        openingRate.val('');
                        minQty.val('');
                        maxQty.val('');
                        GST.val('');
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
    $(document).on('click', '#btnRefresh', LoadStocks);
    //-------Update Records---------//

    $(document).on('click', '.btn-stock-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        EditStock(value);
    });
    function EditStock(Id) {
        var $tr = $('#btnStockEdit_' + Id + '').closest('tr');
        var productName = $tr.find('td:eq(1)').text().trim();
        var openingQty = $tr.find('td:eq(2)').text().trim();
        var openingRate = $tr.find('td:eq(3)').text().trim();
        var avilableQty = $tr.find('td:eq(5)').text().trim();
        var minimumQty = $tr.find('td:eq(6)').text().trim();
        var maximumQty = $tr.find('td:eq(7)').text().trim();
        //fill Modal data
        $('input[name="mdlStockId"]').val(Id);
        $('input[name="mdlOpeningQty"]').val(openingQty);
        $('input[name="mdlOpeningRate"]').val(openingRate);
        $('input[name="mdlAvilableQty"]').val(avilableQty);
        $('input[name="mdlMinQty"]').val(minimumQty);
        $('input[name="mdlMaxQty"]').val(maximumQty);

        $.ajax({
            url: "/Master/GetAllProducts",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                var selectElement = $('select[name="mdlProductId"]');
                if (result.ResponseCode == 302) {
                    selectElement.empty();
                    $.each(result.products, function (key, item) {
                        var option = $('<option></option>').val(item.ProductId).text(item.ProductName);
                        if (item.ProductName === productName) {
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
    }
    $('.stock').on('click', UpdateStock);
    function UpdateStock() {
        if (!mdlStockId.val()) {
            $('input[name="mdlStockId"]').css('border-color', 'red');
            return;
        } else if (!mdlOpeningQty.val()) {
            $('input[name="mdlOpeningQty"]').css('border-color', 'red');
            return;
        } else if (!mdlOpeningRate.val()) {
            $('input[name="mdlOpeningRate"]').css('border-color', 'red');
            return;
        } else if (!mdlAvilableQty.val()) {
            $('input[name="mdlAvilableQty"]').css('border-color', 'red');
            return;
        } else if (!mdlMinQty.val()) {
            $('input[name="mdlMinQty"]').css('border-color', 'red');
            return;
        } else if (!mdlMaxQty.val()) {
            $('input[name="mdlMaxQty"]').css('border-color', 'red');
            return;
        } else {
            const data = {
                StockId: $('input[name="mdlStockId"]').val(),
                Fk_ProductId: $('select[name="mdlProductId"]').val(),
                OpeningStock: $('input[name="mdlOpeningQty"]').val(),
                Rate: $('input[name="mdlOpeningRate"]').val(),
                AvilableStock: $('input[name="mdlAvilableQty"]').val(),
                MinQty: $('input[name="mdlMinQty"]').val(),
                MaxQty: $('input[name="mdlMaxQty"]').val(),
            }
            $.ajax({
                type: "POST",
                url: '/Master/UpdateStock',
                dataType: 'json',
                data: JSON.stringify(data),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#modal-edit-Stock').modal('hide');

                    if (Response.ResponseCode == 200) {
                        toastr.success(Response.SuccessMsg);
                    }
                    else {
                        toastr.error(Response.ErrorMsg);
                    }
                    LoadStocks();
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }

    }
    //-----------Delete Records-------//
    $(document).on('click', '.btn-stock-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeleteStock(value);
    });
    function DeleteStock(Id) {
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
                    url: '/Master/DeleteStock?id=' + Id + '',
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
                        LoadStocks();
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
            }
        });
    }
})