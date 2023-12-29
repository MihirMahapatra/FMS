$(function () {
    $("#AdminLink").addClass("active");
    $("#ProductSetupLink").addClass("active");
    $("#ProductSetupLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    /***************************************Variable Declaration***********************************************************/
    /*---products---*/
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
    const WholeSalePrice = $('input[name = "WholesalePrice"]')
    const GST = $('input[name = "GST"]')
    /*---mdlproduct---*/
    const mdlProductId = $('input[name="mdlProductId"]');
    const mdlProductName = $('input[name="mdlProductName"]');
    const mdlProductType = $('select[name="mdlProductType"]');
    const mdlUnit = $('select[name="mdlUnit"]');
    const mdlGroup = $('select[name="mdlGroup"]');
    const mdlSubGroup = $('select[name="mdlSubGroup"]');
    const mdlPrice = $('input[name="mdlPrice"]');
    const MdlWholeSalePrice = $('input[name="mdlWholeSalePrice"]');
    const mdlGst = $('input[name="mdlGst"]');
    /***********************************************************Contorl Foucous Of Element***********************************************************/
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
    /***************************************Validation Section***********************************************************/
    Price.on("input", function () {
        var inputValue = $(this).val().replace(/[^0-9.]/g, '');
        $(this).val(inputValue);
    });
    ProductName.on("input", function () {
        let inputValue = $(this).val();
        inputValue = inputValue.toUpperCase();
        $(this).val(inputValue);
    });
    GST.on("input", function () {
        var inputValue = $(this).val().replace(/\D/g, '');
        $(this).val(inputValue);
    }); 
    /***************************************  Unit ***********************************************************/
    loadUnits();
    function loadUnits() {

        unitId.empty();
        var defaultOption = $('<option></option>').val('').text('--Select Option--');
        unitId.append(defaultOption);
        $.ajax({
            url: "/Admin/GetAllUnits",
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
    $('.unitAdd').on('click', UnitAdd);
    function UnitAdd() {
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
                url: '/Admin/CreateUnit',
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
    }
    $('#btnUnitEdit').on('click', UnitEdit);
    function UnitEdit() {
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
            $("input[name='mdlUnitName']").val(text);
        }
    }
    $('.mdlUnitUpdate').on('click', UnitUpdate);
    function UnitUpdate() {
        const data = {
            UnitId: $('input[name="mdlUnitId"]').val(),
            UnitName: $('input[name="mdlUnitName"]').val(),
        }
        $.ajax({
            type: "POST",
            url: '/Admin/UpdateUnit',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                $('#modal-edit-unit').modal('hide');
                if (Response.ResponseCode == 200) {
                    toastr.success(Response.SuccessMsg);
                    $('input[name="mdlUnitId"]').val('');
                    $('input[name="mdlUnitName"]').val('');
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
    $('#btnUnitDelete').on('click', UnitDelete);
    function UnitDelete() {

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
                        url: '/Admin/DeleteUnit?id=' + Id + '',
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

    }
    /*************************************** Group ***********************************************************/
    productTypeId.on('change', function () {
        enablGroup();
    });
    function enablGroup() {
        var productTypIdSelected = productTypeId.val();
        if (productTypIdSelected) {
            groupId.prop("disabled", false);
            loadGroups(productTypIdSelected);
        } else {
            groupId.prop("disabled", true);
        }
    }
    function loadGroups(productTypIdSelected) {
        groupId.empty();
        var defaultOption = $('<option></option>').val('').text('--Select Option--');
        groupId.append(defaultOption);

        $.ajax({
            url: '/Admin/GetAllGroups?ProdutTypeId=' + productTypIdSelected + '',
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
    $('.groupAdd').on('click', GroupAdd);
    function GroupAdd () {
        if (!$('input[name="mdlGroupAdd"]').val()) {
            toastr.error('Plz Insert Group Name');
            return;
        }
        else {
            const data = {
                GroupName: $('input[name="mdlGroupAdd"]').val(),
                Fk_ProductTypeId: productTypeId.val()
            }
            $.ajax({
                type: "POST",
                url: '/Admin/CreateGroup',
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
                        $('#modal-add-group').modal('hide');
                        toastr.error(Response.ErrorMsg);
                    }
                    loadGroups(data.Fk_ProductTypeId);
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }
    }
    $('#btnGrupEdit').on('click', GrupEdit);
    function GrupEdit() {
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
    }
    $('.groupUpdate').on('click', GroupUpdate);
    function GroupUpdate() {
        const data = {
            GroupId: $('input[name="mdlGroupId"]').val(),
            GroupName: $('input[name="mdlGroupEdit"]').val(),
            Fk_ProductTypeId: productTypeId.val()
        }
        $.ajax({
            type: "POST",
            url: '/Admin/UpdateGroup',
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
                loadGroups(data.Fk_ProductTypeId);
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
    $('#btnGrupDelete').on('click', GrupDelete)
    function GrupDelete() {
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
                        url: '/Admin/DeleteGroup?id=' + Id + '',
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
                            loadGroups(productTypeId.val());
                        },
                        error: function (error) {
                            console.log(error);
                        }
                    });
                }
            });
        }
    }
    /*************************************** SubGroup ***********************************************************/
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
            url: '/Admin/GetSubGroups?GroupId=' + GroupId + '',
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
    $('.subGroupAdd').on('click', SubGroupAdd);
    function SubGroupAdd() {
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
                url: '/Admin/CreateSubGroup',
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
    }
    $('#btnSubGroupEdit').on('click', SubGroupEdit);
    function SubGroupEdit () {
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
    }
    $('.subGroupUpdate').on('click', UpdateSubGroup);
    function UpdateSubGroup () {
        const data = {
            SubGroupId: $('input[name="mdlSubGroupId"]').val(),
            SubGroupName: $('input[name="mdlSubGroupEdit"]').val(),
            Fk_GroupId: groupId.find('option:selected').val()
        }
        $.ajax({
            type: "POST",
            url: '/Admin/UpdateSubGroup',
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
    }
    $('#btnSubGroupDelete').on('click', SubGroupDelete);
    function SubGroupDelete () {
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
                        url: '/Admin/DeleteSubGroup?id=' + Id + '',
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

    }
    /***************************************Product Types***********************************************************/
    loadProductTypes();
    function loadProductTypes() {
        productTypeId.empty();
        var defaultOption = $('<option></option>').val('').text('--Select Option--');
        productTypeId.append(defaultOption);
        $.ajax({
            url: "/Admin/GetAllProductTypes",
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
   /***************************************Product***********************************************************/
    LoadProducts();
    function LoadProducts() {
        $('#loader').show();
        $('.ProductTable').empty();
        $.ajax({
            url: "/Admin/GetAllProducts",
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
                html += '<th>Price(W)</th>'
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
                        html += '<td>' + item.WholeSalePrice + '</td>';
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
                WholeSalePrice: WholeSalePrice.val(),
                GST: GST.val()
            }

            $.ajax({
                type: "POST",
                url: '/Admin/CreateProduct',
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
        var WholeSalePrice = $tr.find('td:eq(7)').text().trim();
        var Gst = $tr.find('td:eq(8)').text().trim();
        $('input[name="mdlProductId"]').val(ProductId);
        $('input[name="mdlProductName"]').val(ProductName);
        $('input[name="mdlPrice"]').val(Price);
        $('input[name="mdlWholeSalePrice"]').val(WholeSalePrice);
        $('input[name="mdlGst"]').val(Gst);
        $.ajax({
            url: "/Admin/GetAllUnits",
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
            url: "/Admin/GetAllProductTypes",
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
                            getGroups(item.ProductTypeId, Group, SubGroup)
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
    $('select[name="mdlProductType"]').on('change', function () {
        const ProductTypeId = $('select[name="mdlProductType"]').val();
        getGroups(ProductTypeId);
    });
    function getGroups(ProductTypeId, groupName, SubGroupName) {
        var defaultOption = $('<option></option>').val('').text('--Select Option--');
        var selectGroupElement = $('select[name="mdlGroup"]');
        selectGroupElement.empty();
        selectGroupElement.append(defaultOption);
        $.ajax({
            url: '/Admin/GetAllGroups?ProdutTypeId=' + ProductTypeId + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    $.each(result.Groups, function (key, item) {
                        var option = $('<option></option>').val(item.GroupId).text(item.GroupName);
                        if (item.GroupName === groupName) {
                            option.attr('selected', 'selected');
                            getSubGroup(item.GroupId, SubGroupName)
                        }
                        selectGroupElement.append(option);
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
    function getSubGroup(GroupId, SubGroupName) {
        var defaultOption = $('<option></option>').val('').text('--Select Option--');
        var selectSubGroupElement = $('select[name="mdlSubGroup"]');
        selectSubGroupElement.empty();
        selectSubGroupElement.append(defaultOption);
        $.ajax({
            url: '/Admin/GetSubGroups?Groupid=' + GroupId + '',
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
    $('.btn-product-update').on('click', UpdateProduct);
    function UpdateProduct () {
        if (!mdlProductId.val()) {
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
        }  else if (!mdlPrice.val()) {
            $('input[name="mdlPrice"]').css('border-color', 'red');
            return;
        } else if (!mdlGst.val()) {
            $('input[name="mdlGst"]').css('border-color', 'red');
            return;
        }
        else {
            const data = {
                ProductId: mdlProductId.val(),
                ProductName: mdlProductName.val(),
                Fk_ProductTypeId: mdlProductType.val(),
                Fk_UnitId: mdlUnit.val(),
                Fk_GroupId: mdlGroup.val(),
                Fk_SubGroupId: mdlSubGroup.val(),
                Price: mdlPrice.val(),
                WholeSalePrice: MdlWholeSalePrice.val(),
                GST: mdlGst.val()
            }
            $.ajax({
                type: "POST",
                url: '/Admin/UpdateProduct',
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
    }
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
                    url: '/Admin/DeleteProduct?id=' + Id + '',
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
})