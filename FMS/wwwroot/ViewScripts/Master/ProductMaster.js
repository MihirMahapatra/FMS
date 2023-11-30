$(function () {
    $("#MasterLink").addClass("active");
    $("#ProductMasterLink").addClass("active");
    $("#ProductMasterLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    /***************************************Variable Declaration***********************************************************/
    //products
    const groupId = $('select[name="ProductGroupId"]');
    const subGroupId = $('select[name="ProductSubGroupId"]');
    const productTypeId = $('select[name="ProductTypeId"]');
    $('select[name="ProductTypeId"]').on('keydown', function (e) {
        if (e.keyCode === 9 && !e.shiftKey) {
            e.preventDefault();
            $('select[name="ProductGroupId"]').focus();
        }
    });

    $('select[name="ProductGroupId"]').on('keydown', function (e) {
        if (e.keyCode === 9 && !e.shiftKey) {
            e.preventDefault();
            $('input[name="ProductName"]').focus();
        }
    });

    const unitId = $('select[name="ProductUnitId"]');
    const ProductName = $('input[name = "ProductName"]')
    const Price = $('input[name = "Price"]')
    const GST = $('input[name = "GST"]')
    //mdlproduct
    const mdlProductId = $('input[name="mdlProductId"]');
    const mdlProductName = $('input[name="mdlProductName"]');
    const mdlProductType = $('select[name="mdlProductType"]');
    const mdlUnit = $('select[name="mdlUnit"]');
    const mdlGroup = $('select[name="mdlGroup"]');
    const mdlSubGroup = $('select[name="mdlSubGroup"]');
    const mdlPrice = $('input[name="mdlPrice"]');
    const mdlGst = $('input[name="mdlGst"]');
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
    //-----------------------------------Contorl Foucous Of Element    ProductMaster ProductDetail----------------------------//
    productTypeId.focus();
    ProductName.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    ProductName.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    Price.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    Price.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    GST.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    GST.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    $('.btn-product-create').on('keydown', function (e) {
        if (e.key === 'Enter' || e.keyCode === 13) {
            $('.btn-product-create').click();
        }
    });
    $('.btn-product-create').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('.btn-product-create').on('blur', function () {
        $(this).css('background-color', '');
    });
     //-----------------------------------Contorl Foucous Of Element   ProductMaster StockDetail----------------------------//
    productId.focus();
    openingQty.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    openingQty.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    openingRate.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    openingRate.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    minQty.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    minQty.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    maxQty.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    maxQty.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
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
    Price.on("input", function () {
        var inputValue = $(this).val().replace(/[^0-9.]/g, '');
        $(this).val(inputValue);
    });
    ProductName.on("input", function () {
        let inputValue = $(this).val();
        inputValue = inputValue.toUpperCase();
        $(this).val(inputValue);
    });
    openingRate.on("input", function () {
        var inputValue = $(this).val().replace(/[^0-9.]/g, '');
        $(this).val(inputValue);
    });
    GST.on("input", function () {
        var inputValue = $(this).val().replace(/\D/g, '');
        $(this).val(inputValue);
    });
    //------------------------------------------------jqueryTabEnter -----------------------------------//
  
   
    /*--------------------------------------Unit-------------------------------------------------*/
    //---------get Records----------//
    loadUnits();
    function loadUnits() {

        unitId.empty();
        var defaultOption = $('<option></option>').val('').text('--Select Option--');
        unitId.append(defaultOption);

        $.ajax({
            url: "/Master/GetAllUnits",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    $.each(result.Units, function (key, item) {
                        var option = $('<option></option>').val(item.UnitId).text(item.UnitName);
                        unitId.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    $('#btnUnitAdd').on('click', function () {

        $('#modal-add-unit').modal('show');
    });
    $('#modal-add-unit').on('click', '.unitAdd', (event) => {

        if (!$('input[name="mdlUnitAdd"]').val()) {
            toastr.error('Plz Insert Unit Name');
            return;
        }
        else {
            const data = {
                UnitName: $('input[name="mdlUnitAdd"]').val(),
            }
            $.ajax({
                type: "POST",
                url: '/Master/CreateUnit',
                dataType: 'json',
                data: JSON.stringify(data),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#modal-add-unit').modal('hide');
                    if (Response.ResponseCode == 201) {
                        toastr.success(Response.SuccessMsg);
                        $('input[name="mdlUnitAdd"]').val('');
                    }
                    else {
                        toastr.error(Response.ErrorMsg);
                    }
                    loadUnits();
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }
    });
    $('#btnUnitEdit').on('click', function () {
        if (!unitId.val() || unitId.val() === '--Select Option--') {
            toastr.error('Plz Select a Unit Name To Edit');
            return;
        }
        else {
            $('#modal-edit-unit').modal('show');
            const selectedOption = unitId.find('option:selected');
            var text = selectedOption.text();
            var value = selectedOption.val();
            $("input[name='mdlUnitId']").val(value);
            $("input[name='mdlUnitEdit']").val(text);
        }
    });
    $('#modal-edit-unit').on('click', '.unitEdit', (event) => {
        const data = {
            UnitId: $('input[name="mdlUnitId"]').val(),
            UnitName: $('input[name="mdlUnitEdit"]').val(),
        }
        $.ajax({
            type: "POST",
            url: '/Master/UpdateUnit',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                $('#modal-edit-unit').modal('hide');
                if (Response.ResponseCode == 200) {
                    toastr.success(Response.SuccessMsg);
                    $('input[name="mdlUnitId"]').val('');
                    $('input[name="mdlUnitEdit"]').val('');
                }
                else {
                    toastr.error(Response.ErrorMsg);
                }
                loadUnits();
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
    $('#btnUnitDelete').on('click', function () {

        if (!unitId.val() || unitId.val() === '--Select Option--') {
            toastr.error('Plz Select a Unit Name To Delete');
            return;
        }
        else {
            const Id = unitId.find('option:selected').val();
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
                        url: '/Master/DeleteUnit?id=' + Id + '',
                        type: "POST",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (result) {
                            if (result.ResponseCode = 200) {
                                toastr.success(result.SuccessMsg);
                            }
                            else {
                                toastr.error(result.ErrorMsg);
                            }
                            loadUnits();
                        },
                        error: function (error) {
                            console.log(error);
                        }
                    });
                }
            });
        }

    })
    /*--------------------------------------Group------------------------------------------------*/
    loadGroups();
    function loadGroups() {
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
    $('#btnGrupAdd').on('click', function () {
        $('#modal-add-group').modal('show');
    });
    $('#modal-add-group').on('click', '.groupAdd', (event) => {
        if (!$('input[name="mdlGroupAdd"]').val()) {
            toastr.error('Plz Insert Group Name');
            return;
        }
        else {
            const data = {
                GroupName: $('input[name="mdlGroupAdd"]').val(),
            }
            $.ajax({
                type: "POST",
                url: '/Master/CreateGroup',
                dataType: 'json',
                data: JSON.stringify(data),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#modal-add-group').modal('hide');
                    if (Response.ResponseCode == 201) {
                        toastr.success(Response.SuccessMsg);
                        $('input[name="mdlGroupAdd"]').val('');
                    }
                    else {
                        toastr.error(Response.ErrorMsg);
                    }
                    loadGroups();
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }
    });
    $('#btnGrupEdit').on('click', function () {
        if (!groupId.val() || groupId.val() === '--Select Option--') {
            toastr.error('Plz Select a Group Name To Edit');
            return;
        }
        else {
            $('#modal-edit-group').modal('show');
            const selectedOption = groupId.find('option:selected');
            var text = selectedOption.text();
            var value = selectedOption.val();
            $("input[name='mdlGroupId']").val(value);
            $("input[name='mdlGroupEdit']").val(text);
        }
    });
    $('#modal-edit-group').on('click', '.groupEdit', (event) => {
        const data = {
            GroupId: $('input[name="mdlGroupId"]').val(),
            GroupName: $('input[name="mdlGroupEdit"]').val(),
        }
        $.ajax({
            type: "POST",
            url: '/Master/UpdateGroup',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                $('#modal-edit-group').modal('hide');
                if (Response.ResponseCode == 200) {
                    toastr.success(Response.SuccessMsg);
                    $('input[name="mdlGroupId"]').val('');
                    $('input[name="mdlGroupEdit"]').val('');
                }
                else {
                    toastr.error(Response.ErrorMsg);
                }
                loadGroups();
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
    $('#btnGrupDelete').on('click', function () {
        if (!groupId.val() || groupId.val() === '--Select Option--') {
            toastr.error('Plz Select a Group Name To Delete');
            return;
        }
        else {
            const Id = groupId.find('option:selected').val();
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
                        url: '/Master/DeleteGroup?id=' + Id + '',
                        type: "POST",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (result) {
                            if (result.ResponseCode = 200) {
                                toastr.success(result.SuccessMsg);
                            }
                            else {
                                toastr.error(result.ErrorMsg);
                            }
                            loadGroups();
                        },
                        error: function (error) {
                            console.log(error);
                        }
                    });
                }
            });
        }

    })
    /*--------------------------------------SubGroup------------------------------------------------*/
    groupId.on('change', function () {
        enableSubGroup();
    });
    function enableSubGroup() {
        var GroupIdSelected = groupId.val();
        if (GroupIdSelected) {
            subGroupId.prop("disabled", false);
            loadSubGroups(GroupIdSelected)
        } else {
            subGroupId.prop("disabled", true);
        }
    }
    function loadSubGroups(GroupId) {
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
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    $('#btnSubGroupAdd').on('click', function () {
        $('#modal-add-subGroup').modal('show');
    });
    $('#modal-add-subGroup').on('click', '.subGroupAdd', (event) => {
        if (!$('input[name="mdlSubGroupAdd"]').val()) {
            toastr.error('Plz Insert SubGroup Name');
            return;
        }
        else {
            const data = {
                Fk_GroupId: groupId.val(),
                SubGroupName: $('input[name="mdlSubGroupAdd"]').val(),
            }
            $.ajax({
                type: "POST",
                url: '/Master/CreateSubGroup',
                dataType: 'json',
                data: JSON.stringify(data),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#modal-add-subGroup').modal('hide');
                    if (Response.ResponseCode == 201) {
                        toastr.success(Response.SuccessMsg);
                        $('input[name="mdlSubGroupAdd"]').val('');
                    }
                    else {
                        toastr.error(Response.ErrorMsg);
                    }
                    loadSubGroups(data.Fk_GroupId);
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }
    });
    $('#btnSubGroupEdit').on('click', function () {
        if (!subGroupId.val() || subGroupId.val() === '--Select Option--') {
            toastr.error('Plz Select a SubGroup Name To Edit');
            return;
        }
        else {
            $('#modal-edit-subGroup').modal('show');
            const selectedOption = subGroupId.find('option:selected');
            var text = selectedOption.text();
            var value = selectedOption.val();
            $("input[name='mdlSubGroupId']").val(value);
            $("input[name='mdlSubGroupEdit']").val(text);
        }
    });
    $('#modal-edit-subGroup').on('click', '.subGroupEdit', (event) => {
        const data = {
            SubGroupId: $('input[name="mdlSubGroupId"]').val(),
            SubGroupName: $('input[name="mdlSubGroupEdit"]').val(),
            Fk_GroupId: groupId.find('option:selected').val()
        }
        $.ajax({
            type: "POST",
            url: '/Master/UpdateSubGroup',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                $('#modal-edit-subGroup').modal('hide');
                if (Response.ResponseCode == 200) {
                    toastr.success(Response.SuccessMsg);
                    $('input[name="mdlLSubGroupId"]').val('');
                    $('input[name="mdlSubGroupEdit"]').val('');
                }
                else {
                    toastr.error(Response.ErrorMsg);
                }
                loadSubGroups(data.Fk_GroupId);
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
    $('#btnSubGroupDelete').on('click', function () {
        if (!subGroupId.val() || subGroupId.val() === '--Select Option--') {
            toastr.error('Plz Select a SubGroup Name To Delete');
            return;
        }
        else {
            const Id = subGroupId.find('option:selected').val();
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
                        url: '/Master/DeleteSubGroup?id=' + Id + '',
                        type: "POST",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (result) {
                            if (result.ResponseCode = 200) {
                                toastr.success(result.SuccessMsg);
                            }
                            else {
                                toastr.error(result.ErrorMsg);
                            }
                            const GroupId = groupId.find('option:selected').val();
                            loadSubGroups(GroupId);
                        },
                        error: function (error) {
                            console.log(error);
                        }
                    });
                }
            });
        }

    })
    /*--------------------------------------Product Types------------------------------------------------*/
    loadProductTypes();
    function loadProductTypes() {
        productTypeId.empty();
        var defaultOption = $('<option></option>').val('').text('--Select Option--');
        productTypeId.append(defaultOption);
        $.ajax({
            url: "/Master/GetAllProductTypes",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    $.each(result.ProductTypes, function (key, item) {
                        var option = $('<option></option>').val(item.ProductTypeId).text(item.Product_Type);
                        productTypeId.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    /*------------------------------------------------ Product--------------------------------------------*/
    //---------get Records----------//
    LoadProducts();
    function LoadProducts() {
        $('#loader').show();
        $('.ProductTable').empty();
        $.ajax({
            url: "/Master/GetAllProducts",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#loader').hide();
                var html = '';
                html += '<table class="table table-bordered table-hover text-center mt-1 ProductTable" style="width:100%">';
                html += '<thead>'
                html += '<tr>'
                html += '<th hidden>Product Id</th>'
                html += '<th>Product Name</th>'
                html += '<th>Product Type</th>'
                html += '<th>Unit</th>'
                html += '<th>Group</th>'
                html += '<th>Sub Group</th>'
                html += '<th>Price</th>'
                html += '<th>GST</th>'
                html += '<th>Action</th>'
                html += '</tr>'
                html += '</thead>'
                html += '<tbody>';
                if (result.products !== null) {
                    $.each(result.products, function (key, item) {
                        html += '<tr>';
                        html += '<td hidden>' + item.ProductId + '</td>';
                        html += '<td>' + item.ProductName + '</td>';
                        html += '<td>' + item.ProductType.Product_Type + '</td>';
                        html += '<td>' + item.Unit.UnitName + '</td>';
                        html += '<td>' + item.Group.GroupName + '</td>';
                        if (item.SubGroup !== null) {
                            html += '<td>' + item.SubGroup.SubGroupName + '</td>';
                        }
                        else {
                            html += '<td> - </td>';
                        }
                        html += '<td>' + item.Price + '</td>';
                        html += '<td>' + item.GST + '</td>';
                        html += '<td style="background-color:#ffe6e6;">';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-product-edit"   id="btnProductEdit_' + item.ProductId + '" data-id="' + item.ProductId + '" data-toggle="modal" data-target="#modal-edit-Product" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-product-delete" id="btnProductDelete_' + item.ProductId + '"   data-id="' + item.ProductId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                        html += '</td>';
                        html += '</tr >';
                    });
                }
                else {
                    html += '<tr>';
                    html += '<td colspan="8">No record</td>';
                    html += '</tr>';
                }
                html += ' </tbody>';
                html += '</table >';
                $('.tbProduct').html(html);
                if (!$.fn.DataTable.isDataTable('.ProductTable')) {
                    $('.ProductTable').DataTable({
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
    $(document).on('click', '.btn-product-create', CreateProduct);
    function CreateProduct() {
        if (!productTypeId.val() || productTypeId.val() === '--Select Option--') {
            toastr.error('Product Type Is Required.');
            productTypeId.focus();
            return;
        }
        else if (!groupId.val() || groupId.val() === '--Select Option--') {
            toastr.error('Group Name Is Required.');
            groupId.focus();
            return;
        }
        else if (!ProductName.val()) {
            toastr.error('Product Name Is Required.');
            ProductName.focus();
            return;
        }
        else if (!unitId.val() || groupId.val() === '--Select Option--') {
            toastr.error('Unit Name Is Required.');
            unitId.focus();
            return;
        }     
        else if (!Price.val()) {
            toastr.error('Price Is Required.');
            Price.focus();
            return;
        } else if (!GST.val()) {
            toastr.error('Gst Is Required.');
            GST.focus();
            return;
        }
        else {
            const data = {
                ProductName: ProductName.val(),
                Fk_ProductTypeId: productTypeId.val(),
                Fk_UnitId: unitId.val(),
                Fk_GroupId: groupId.val(),
                Fk_SubGroupId: subGroupId.val(),
                Price: Price.val(),
                GST: GST.val()
            }

            $.ajax({
                type: "POST",
                url: '/Master/CreateProduct',
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
                    LoadProducts();
                    ProductName.val('');
                    Price.val('');
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }
    }
    $(document).on('click', '.btn-product-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        EditProduct(value);
    });
    function EditProduct(Id) {
        var $tr = $('#btnProductEdit_' + Id + '').closest('tr');
        var ProductId = Id;
        var ProductName = $tr.find('td:eq(1)').text().trim();
        var ProductType = $tr.find('td:eq(2)').text().trim();
        var Unit = $tr.find('td:eq(3)').text().trim();
        var Group = $tr.find('td:eq(4)').text().trim();
        var SubGroup = $tr.find('td:eq(5)').text().trim();
        var Price = $tr.find('td:eq(6)').text().trim();
        var Gst = $tr.find('td:eq(7)').text().trim();

        $('input[name="mdlProductId"]').val(Id);
        $('input[name="mdlProductName"]').val(ProductName);
        $('input[name="mdlPrice"]').val(Price);
        $('input[name="mdlGst"]').val(Gst);

        $.ajax({
            url: "/Master/GetAllUnits",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                var selectElement = $('select[name="mdlUnit"]');
                if (result.ResponseCode == 302) {
                    selectElement.empty();
                    $.each(result.Units, function (key, item) {
                        var option = $('<option></option>').val(item.UnitId).text(item.UnitName);
                        if (item.UnitName === Unit) {
                            option.attr('selected', 'selected');
                        }
                        selectElement.append(option);
                    });
                }
                else {
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    selectElement.append(defaultOption);
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
        $.ajax({
            url: "/Master/GetAllProductTypes",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                var selectElement = $('select[name="mdlProductType"]');
                if (result.ResponseCode == 302) {
                    selectElement.empty();
                    $.each(result.ProductTypes, function (key, item) {
                        var option = $('<option></option>').val(item.ProductTypeId).text(item.Product_Type);
                        if (item.Product_Type === ProductType) {
                            option.attr('selected', 'selected'); // Set selected attribute
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
            url: "/Master/GetAllGroups",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                var selectElement = $('select[name="mdlGroup"]');
                if (result.ResponseCode == 302) {
                    selectElement.empty();
                    $.each(result.Groups, function (key, item) {
                        var option = $('<option></option>').val(item.GroupId).text(item.GroupName);
                        if (item.GroupName === Group) {
                            option.attr('selected', 'selected');
                            getSubGroup(item.GroupId, SubGroup)
                        }
                        selectElement.append(option);
                    });
                }
                else {
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    selectElement.append(defaultOption);
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    function getSubGroup(GroupId, SubGroupName) {
        var defaultOption = $('<option></option>').val('').text('--Select Option--');
        var selectSubGroupElement = $('select[name="mdlSubGroup"]');
        selectSubGroupElement.empty();
        selectSubGroupElement.append(defaultOption);
        $.ajax({
            url: '/Master/GetSubGroups?Groupid=' + GroupId + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    $.each(result.SubGroups, function (key, item) {
                        var option = $('<option></option>').val(item.SubGroupId).text(item.SubGroupName);
                        if (item.SubGroupName === SubGroupName) {
                            option.attr('selected', 'selected');
                        }
                        selectSubGroupElement.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    $('select[name="mdlGroup"]').on('change', function () {
        const GroupId = $('select[name="mdlGroup"]').val();
        getSubGroup(GroupId);
    });
    $('#modal-edit-Product').on('click', '.product', (event) => {

        if (!mdlProductId.val()){
            $('input[name="mdlProductId"]').css('border-color', 'red');
            return;
        } else if (!mdlProductName.val()) {
            $('input[name="mdlProductName"]').css('border-color', 'red');
            return;
        } else if (!mdlProductType.val() || mdlProductType.val() === '--Select Option--') {
            $('input[name="mdlProductType"]').css('border-color', 'red');
            return;
        } else if (!mdlUnit.val() || mdlUnit.val() === '--Select Option--') {
            $('input[name="mdlUnit"]').css('border-color', 'red');
            return;
        } else if (!mdlGroup.val() || mdlGroup.val() === '--Select Option--') {
            $('input[name="mdlGroup"]').css('border-color', 'red');
            return;
        } else if (!mdlSubGroup.val() || mdlSubGroup.val() === '--Select Option--') {
            $('input[name="mdlSubGroup"]').css('border-color', 'red');
            return;
        } else if (!mdlPrice.val()) {
            $('input[name="mdlPrice"]').css('border-color', 'red');
            return;
        } else if (!mdlGst.val()) {
            $('input[name="mdlGst"]').css('border-color', 'red');
            return;
        }
        else {
            const data = {
                ProductId: $('input[name="mdlProductId"]').val(),
                ProductName: $('input[name="mdlProductName"]').val(),
                Fk_ProductTypeId: $('select[name="mdlProductType"]').val(),
                Fk_UnitId: $('select[name="mdlUnit"]').val(),
                Fk_GroupId: $('select[name="mdlGroup"]').val(),
                Fk_SubGroupId: $('select[name="mdlSubGroup"]').val(),
                Price: $('input[name="mdlPrice"]').val(),
                GST: $('input[name="mdlGst"]').val()
            }
            $.ajax({
                type: "POST",
                url: '/Master/UpdateProduct',
                dataType: 'json',
                data: JSON.stringify(data),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#modal-edit-Product').modal('hide');
                    if (Response.ResponseCode = 200) {
                        toastr.success(Response.SuccessMsg);
                    }
                    else {
                        toastr.error(Response.ErrorMsg);
                    }
                    LoadProducts();
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }


    });
    $(document).on('click', '.btn-product-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeleteProduct(value);
    });
    function DeleteProduct(Id) {
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
                    url: '/Master/DeleteProduct?id=' + Id + '',
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
                        LoadProducts();
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
            }
        });
    }
    /*--------------------------------------------------------------- Stock Details--------------------------------------------*/
    //---------get Records----------//
    $('a[href="#StockDetail"]').on('click', function () {
        LoadProductForStock();
        LoadStocks();
    });
    function LoadProductForStock() {
        productId.empty();
        var defaultOption = $('<option></option>').val('').text('--Select Option--');
        productId.append(defaultOption);
        $.ajax({
            url: "/Master/GetProductsWhichNotInStock",
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
                            option.attr('selected', 'selected'); // Set selected attribute
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
    $('#modal-edit-Stock').on('click', '.stock', (event) => {
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
       
    });

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











