$(function () {
    //Default Date 
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;
    $("#MasterLink").addClass("active");
    $("#LabourMasterLink").addClass("active");
    $("#LabourMasterLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    /***************************************Variable Declaration***********************************************************/
    const labourType = $('#txtLabourType');
    const labourTypeId = $('select[name="LabourTypeId"]');
    const labourName = $('#txtLabourName');
    const address = $('#txtAddress');
    const Phone = $('#txtPhone');
    const Reference = $('#txtReference');
    const openingBalance = $('input[name="OpeningBalance"]');
    const balanceType = $('select[name="ddnBalanceType"]');
    const date = $('input[name="date"]');
    date.val(todayDate);
    const itemId = $('select[name="ItemId"]');
    const rate = $('input[name="Rate"]');
    /***************************************Validation Section***********************************************************/
    labourType.on("input", function () {
        let inputValue = $(this).val();
        inputValue = inputValue.toUpperCase();
        $(this).val(inputValue);
    });
    labourName.on("input", function () {
        let inputValue = $(this).val();
        inputValue = inputValue.toUpperCase();
        $(this).val(inputValue);
    });
    address.on("input", function () {
        let inputValue = $(this).val();
        inputValue = inputValue.toUpperCase();
        $(this).val(inputValue);
    });
    Phone.on("input", function () {
        var inputValue = $(this).val().replace(/\D/g, '');
        if (inputValue.length > 10) {
            inputValue = inputValue.substr(0, 10);
        }
        $(this).val(inputValue);
    });
    Reference.on("input", function () {
        let inputValue = $(this).val();
        inputValue = inputValue.toUpperCase();
        $(this).val(inputValue);
    });
    date.on("blur", function () {
        let inputValue = $(this).val();
        var dateRegex = /^(\d{2})\/(\d{2})\/(\d{4})$/;
        if (!dateRegex.test(inputValue)) {
            toastr.error("Invalid date format. Please use DD/MM/YYYY.");
        }
    });
    rate.on("input", function () {
        var inputValue = $(this).val().replace(/[^0-9.]/g, '');
        $(this).val(inputValue);
    });
    openingBalance.on("input", function () {
        var inputValue = $(this).val().replace(/[^0-9.]/g, '');
        if (inputValue.length > 10) {
            inputValue = inputValue.substr(0, 10);
        }
        $(this).val(inputValue);
    });
    //----------------------------------Contorl Foucous Of Element Labour Type----------------------------------//
    labourType.focus();
    $('.btn-labourtype-create').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('.btn-labourtype-create').on('blur', function () {
        $(this).css('background-color', '');
    });
    $('#txtLabourType').on('focus', function () {
        $(this).css('border-color', 'red');
    });
    $('#txtLabourType').on('blur', function () {
        $(this).css('border-color', '');
    });
   //----------------------------------Contorl Foucous Of Element Labour Details-------------------------------//
    labourTypeId.focus();
    labourTypeId.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    labourTypeId.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    labourName.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    labourName.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    address.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    address.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    Phone.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    Phone.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    Reference.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    Reference.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    openingBalance.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    openingBalance.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    balanceType.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    balanceType.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    $('.btn-labourdetail-create').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('.btn-labourdetail-create').on('blur', function () {
        $(this).css('background-color', '');
    });
    //----------------------------------Contorl Foucous Of Element Labour Rate-------------------------------//
    itemId.focus();
    itemId.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    itemId.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    rate.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    rate.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    $('.btn-labourrate-create').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('.btn-labourrate-create').on('blur', function () {
        $(this).css('background-color', '');
    });
    //******************************************Labour Type******************************************//
    loadLabourTypes();
    function loadLabourTypes() {
        $('#loader').show();
        $('.tblLabourType').empty();
        $.ajax({
            url: "/Master/GetAllLabourTypes",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#loader').hide();
                var html = '';
                var sl = 1;
                if (result.ResponseCode == 404) {
                    html += '<table class="table table-bordered table-hover text-center mt-1 LabourTypeTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th hidden>LabourType Id</th>'
                    html += '<th>Sl No</th>'
                    html += '<th>Labour Type</th>'
                    html += '<th>Action</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    html += '<tr>';
                    html += '<td colspan="4">No record</td>';
                    html += '</tr>';
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblLabourType').html(html);
                }
                if (result.ResponseCode == 302) {
                    html += '<table class="table table-bordered table-hover text-center mt-1 LabourTypeTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th hidden>LabourType Id</th>'
                    html += '<th>Sl No</th>'
                    html += '<th>Labour Type</th>'
                    html += '<th>Action</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    $.each(result.LabourTypes, function (key, item) {
                        html += '<tr>';
                        html += '<td hidden>' + item.LabourTypeId + '</td>';
                        html += '<td>' + sl + '</td>';
                        html += '<td>' + item.Labour_Type + '</td>';
                        html += '<td style="background-color:#ffe6e6;">';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-labourtype-edit"   id="btnLabourTypeEdit_' + item.LabourTypeId + '"     data-id="' + item.LabourTypeId + '"  style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-labourtype-update" id ="btnLabourTypeUpdate_' + item.LabourTypeId + '" data-id="' + item.LabourTypeId + '" style = "border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;display:none" > <i class="fa-solid fa-floppy-disk"></i></button >';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-labourtype-cancel" id="btnLabourTypeCancel_' + item.LabourTypeId + '"   data-id="' + item.LabourTypeId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px; display:none"> <i class="fa-solid fa-rectangle-xmark"></i></button>';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-labourtype-delete" id="btnLabourTypeDelete_' + item.LabourTypeId + '"   data-id="' + item.LabourTypeId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                        html += '</td>';
                        html += '</tr >';
                        sl++;
                    });
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblLabourType').html(html);
                    if (!$.fn.DataTable.isDataTable('.LabourTypeTable')) {
                        $('.LabourTypeTable').DataTable({
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
    $(document).on('click', '.btn-labourtype-create', CreateLabourType);
    function CreateLabourType() {

        if (!labourType.val()) {
            toastr.error("Labour Type is required.");
            labourType.focus();
            return;
        }
        const data = {
            Labour_Type: labourType.val(),
        }
        $.ajax({
            type: "POST",
            url: '/Master/CreateLabourType',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                loadLabourTypes();
                LoadLabourTypeForLabourDetails();
                console.log(Response)
                if (Response.ResponseCode == 201) {
                    toastr.success(Response.SuccessMsg);
                    labourType.val('');
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
    $(document).on('click', '.btn-labourtype-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        EditLabourType(value);
    });
    function EditLabourType(Id) {

        var $tr = $('#btnLabourTypeEdit_' + Id + '').closest('tr');
        var LabourType = $tr.find('td:eq(2)').text().trim();
        $tr.find('td:eq(2)').html('<div class="form-group"><input type="text" class="form-control" value="' + LabourType + '"></div>');
        $tr.find('#btnLabourTypeEdit_' + Id + ', #btnLabourTypeDelete_' + Id + '').hide();
        $tr.find('#btnLabourTypeUpdate_' + Id + ',#btnLabourTypeCancel_' + Id + '').show();
    }
    $(document).on('click', '.btn-labourtype-update', (event) => {
        const value = $(event.currentTarget).data('id');
        UpdateLabourType(value);
    });
    function UpdateLabourType(id) {
        var $tr = $('#btnLabourTypeUpdate_' + id + '').closest('tr');
        const data = {
            LabourTypeId: id,
            Labour_Type: $tr.find('input[type="text"]').eq(0).val(),
        }
        $.ajax({
            type: "POST",
            url: '/Master/UpdateLabourType',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                loadLabourTypes();
                if (Response.ResponseCode = 200) {
                    toastr.success(Response.SuccessMsg);
                    labourType.val('');
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
    $(document).on('click', '.btn-labourtype-cancel', (event) => {
        const value = $(event.currentTarget).data('id');
        CancelLabourType(value);
    });
    function CancelLabourType(id) {
        var $tr = $('#btnLabourTypeCancel_' + id + '').closest('tr');
        var LabourType = $tr.find('input[type="text"]').eq(0).val();
        $tr.find('td:eq(2)').text(LabourType);
        $tr.find('#btnLabourTypeEdit_' + id + ', #btnLabourTypeDelete_' + id + '').show();
        $tr.find('#btnLabourTypeUpdate_' + id + ',#btnLabourTypeCancel_' + id + '').hide();
    }
    $(document).on('click', '.btn-labourtype-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeleteLabourType(value);
    });
    function DeleteLabourType(id) {
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
                // Make an AJAX call to the server-side delete action method
                $.ajax({
                    url: '/Master/DeleteLabourType?id=' + id + '',
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        LoadLabourTypeForLabourDetails();
                        loadLabourTypes();

                        if (result.ResponseCode = 200) {
                            toastr.success(result.SuccessMsg);
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
    /**********************************************Labour Detail************************************************/
    /*-- Bind To DropDown--*/
    LoadLabourTypeForLabourDetails();
    function LoadLabourTypeForLabourDetails() {
        $.ajax({
            url: "/Master/GetAllLabourTypes",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                // var selectElement = $('select[name="LabourTypeId"]');
                if (result.ResponseCode == 404) {

                }
                if (result.ResponseCode == 302) {
                    labourTypeId.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    labourTypeId.append(defaultOption);
                    $.each(result.LabourTypes, function (key, item) {
                        var option = $('<option></option>').val(item.LabourTypeId).text(item.Labour_Type);
                        labourTypeId.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    loadLabourDetails();
    function loadLabourDetails() {
        $('#loader').show();
        $('.tblLabourDetail').empty();
        $.ajax({
            url: "/Master/GetAllLabourDetails",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#loader').hide();
                var html = '';
                if (result.ResponseCode == 404) {
                    html += '<table class="table table-bordered table-hover text-center mt-1 LabourDetailTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th hidden>Labour Id</th>'
                    html += '<th>Labour Name</th>'
                    html += '<th>Labour Type</th>'
                    html += '<th>Address</th>'
                    html += '<th>Phone No</th>'
                    html += '<th>Reference</th>'
                    html += '<th>Action</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    html += '<tr>';
                    html += '<td colspan="7">No record</td>';
                    html += '</tr>';
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblLabourDetail').html(html);
                }
                if (result.ResponseCode == 302) {
                    html += '<table class="table table-bordered table-hover text-center mt-1 LabourDetailTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th hidden>Labour Id</th>'
                    html += '<th>Labour Name</th>'
                    html += '<th>Labour Type</th>'
                    html += '<th>Address</th>'
                    html += '<th>Phone No</th>'
                    html += '<th>Reference</th>'
                    html += '<th>Action</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    $.each(result.Labours, function (key, item) {
                        html += '<tr>';
                        html += '<td hidden>' + item.LabourId + '</td>';
                        html += '<td>' + item.LabourName + '</td>';
                        html += '<td>' + item.LabourType.Labour_Type + '</td>';
                        html += '<td>' + item.Address + '</td>';
                        html += '<td>' + item.Phone + '</td>';
                        html += '<td>' + item.Reference + '</td>';
                        html += '<td style="background-color:#ffe6e6;">';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-labourdetail-edit"   id="btnLabourDetailEdit_' + item.LabourId + '"     data-id="' + item.LabourId + '"  style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-labourdetail-update" id ="btnLabourDetailUpdate_' + item.LabourId + '" data-id="' + item.LabourId + '" style = "border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;display:none" > <i class="fa-solid fa-floppy-disk"></i></button >';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-labourdetail-cancel" id="btnLabourDetailCancel_' + item.LabourId + '"   data-id="' + item.LabourId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px; display:none"> <i class="fa-solid fa-rectangle-xmark"></i></button>';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-labourdetail-delete" id="btnLabourDetailDelete_' + item.LabourId + '"   data-id="' + item.LabourId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                        html += '</td>';
                        html += '</tr >';
                    });
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblLabourDetail').html(html);
                    if (!$.fn.DataTable.isDataTable('.LabourDetailTable')) {
                        $('.LabourDetailTable').DataTable({
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
    $(document).on('click', '.btn-labourdetail-create', CreateLabourDetail);
    function CreateLabourDetail() {
        if (!labourTypeId.val()) {
            toastr.error('Please select Labour Type.');
            labourTypeId.focus();
            return;
        }
        if (!labourName.val()) {
            toastr.error('Labour Name Is Required');
            labourName.focus();
            return;
        }
        if (!address.val()) {
            toastr.error('Labour Address Is Required');
            address.focus();
            return;
        }
        if (Phone.val().length !== 10) {
            toastr.error("Contact number must be exactly 10 digits.");
            Phone.focus();
            return;
        }
        if (!Reference.val()) {
            toastr.error('Labour Reference Is Required');
            Reference.focus();
            return;
        }
        const data = {
            Fk_Labour_TypeId: labourTypeId.val(),
            LabourName: labourName.val(),
            Address: address.val(),
            Phone: Phone.val(),
            Reference: Reference.val(),
            OpeningBalance: openingBalance.val(),
            BalanceType: balanceType.val()
        }
        $.ajax({
            type: "POST",
            url: '/Master/CreateLabourDetail',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                loadLabourDetails();
                if (Response.ResponseCode = 201) {
                    toastr.success(Response.SuccessMsg);
                    labourName.val('');
                    address.val('');
                    Phone.val('');
                    Reference.val('');
                    openingBalance.val('');
                    balanceType.val('');
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
    $(document).on('click', '.btn-labourdetail-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        EditLabourDetail(value);
    });
    function EditLabourDetail(Id) {
        var $tr = $('#btnLabourDetailEdit_' + Id + '').closest('tr');
        var LabourName = $tr.find('td:eq(1)').text().trim();
        var LabourType = $tr.find('td:eq(2)').text().trim();
        var Address = $tr.find('td:eq(3)').text().trim();
        var PhoneNo = $tr.find('td:eq(4)').text().trim();
        var Reference = $tr.find('td:eq(5)').text().trim();
        //**********For Dropdown In Table*********//
        var html = '';
        $.ajax({
            url: "/Master/GetAllLabourTypes",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                console.log(result);
                if (result.ResponseCode == 404) {

                }
                if (result.ResponseCode == 302) {
                    /*******************select Input***************************/
                    html += '<div class="form-group">';
                    html += '<select class="form-control select2bs4" style="width: 100%;" name="LabourTypeId">';
                    $.each(result.LabourTypes, function (key, item) {
                        if (item.Labour_Type === LabourType) {
                            html += '<option value="' + item.LabourTypeId + '" selected>' + item.Labour_Type + '</option>';
                        } else {
                            html += '<option value="' + item.LabourTypeId + '">' + item.Labour_Type + '</option>';
                        }
                    });
                    html += '</select>';
                    html += '</div>';
                    $tr.find('td:eq(2)').html(html); // Update HTML content first
                    $tr.find('.select2bs4').select2({
                        theme: 'bootstrap4'
                    });
                    /*******************************************/
                    $tr.find('td:eq(1)').html('<div class="form-group"><input type="text" class="form-control" value="' + LabourName + '"></div>');
                    $tr.find('td:eq(3)').html('<div class="form-group"><input type="text" class="form-control" value="' + Address + '"></div>');
                    $tr.find('td:eq(4)').html('<div class="form-group"><input type="text" class="form-control" value="' + PhoneNo + '"></div>');
                    $tr.find('td:eq(5)').html('<div class="form-group"><input type="text" class="form-control" value="' + Reference + '"></div>');
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
        /***************************************/
        $tr.find('#btnLabourDetailEdit_' + Id + ', #btnLabourDetailDelete_' + Id + '').hide();
        $tr.find('#btnLabourDetailUpdate_' + Id + ',#btnLabourDetailCancel_' + Id + '').show();
    }
    $(document).on('click', '.btn-labourdetail-update', (event) => {
        const value = $(event.currentTarget).data('id');
        UpdatLabourDetail(value);
    });
    function UpdatLabourDetail(id) {
        var $tr = $('#btnLabourDetailUpdate_' + id + '').closest('tr');
        const data = {
            LabourId: id,
            LabourName: $tr.find('input[type="text"]').eq(0).val(),
            Fk_Labour_TypeId: $tr.find('Select').eq(0).find('option:selected').val(),
            Address: $tr.find('input[type="text"]').eq(1).val(),
            Phone: $tr.find('input[type="text"]').eq(2).val(),
            Reference: $tr.find('input[type="text"]').eq(3).val()
        }
        $.ajax({
            type: "POST",
            url: '/Master/UpdateLabourDetail',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                loadLabourDetails();
                if (Response.ResponseCode = 200) {
                    toastr.success(Response.SuccessMsg);
                    labourName.val('');
                    address.val('');
                    Phone.val('');
                    Reference.val('');
                    openingBalance.val('0');
                    balanceType.val('');
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
    $(document).on('click', '.btn-labourdetail-cancel', (event) => {
        const value = $(event.currentTarget).data('id');
        CancelLabourDetail(value);
    });
    function CancelLabourDetail(id) {
        var $tr = $('#btnLabourDetailCancel_' + id + '').closest('tr');
        var LabourName = $tr.find('input[type="text"]').eq(0).val();
        var LabourType = $tr.find('Select').eq(0).find('option:selected').text();
        var Address = $tr.find('input[type="text"]').eq(1).val();
        var PhoneNo = $tr.find('input[type="text"]').eq(2).val();
        var Reference = $tr.find('input[type="text"]').eq(3).val();
        $tr.find('td:eq(1)').text(LabourName);
        $tr.find('td:eq(2)').text(LabourType);
        $tr.find('td:eq(3)').text(Address);
        $tr.find('td:eq(4)').text(PhoneNo);
        $tr.find('td:eq(5)').text(Reference);
        $tr.find('#btnLabourDetailEdit_' + id + ', #btnLabourDetailDelete_' + id + '').show();
        $tr.find('#btnLabourDetailUpdate_' + id + ',#btnLabourDetailCancel_' + id + '').hide();
    }
    $(document).on('click', '.btn-labourdetail-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeleteLabourDetail(value);
    });
    function DeleteLabourDetail(id) {
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
                    url: '/Master/DeleteLabourDetail?id=' + id + '',
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        loadLabourDetails();
                        if (result.ResponseCode == 200) {
                            toastr.success(result.SuccessMsg);
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
    //*******************************************Labour Rate*******************************************//
    loadLabourRates();
    function loadLabourRates() {
        $('#loader').show();
        $('.tblLabourRate').empty();
        $.ajax({
            url: "/Master/GetAllLabourRates",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                var html = '';
                var sl = 1;
                if (result.ResponseCode == 404) {
                    $('#loader').hide();
                    html += '<table class="table table-bordered table-hover text-center mt-1 LabourRateTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th hidden>Labour Rate Id</th>'
                    html += '<th>Date</th>'
                    html += '<th>Item</th>'
                    html += '<th>Rate</th>'
                    html += '<th>Action</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    html += '<tr>';
                    html += '<td colspan="5">No record</td>';
                    html += '</tr>';
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblLabourRate').html(html);
                }
                if (result.ResponseCode == 302) {
                    html += '<table class="table table-bordered table-hover text-center mt-1 LabourRateTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th hidden>Labour Rate Id</th>'
                    html += '<th>Date</th>'
                    html += '<th>Item</th>'
                    html += '<th>Rate</th>'
                    html += '<th>Action</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    $.each(result.LabourRates, function (key, item) {

                        html += '<tr>';
                        html += '<td hidden>' + item.LabourRateId + '</td>';
                        const ModifyDate = item.Date;
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
                        html += '<td>' + item.Product.ProductName + '</td>';
                        html += '<td>' + item.Rate + '</td>';
                        html += '<td style="background-color:#ffe6e6;">';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-labourrate-edit"   id="btnLabourRateEdit_' + item.LabourRateId + '"     data-id="' + item.LabourRateId + '"  style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-labourrate-update" id ="btnLabourRateUpdate_' + item.LabourRateId + '" data-id="' + item.LabourRateId + '" style = "border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;display:none" > <i class="fa-solid fa-floppy-disk"></i></button >';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-labourrate-cancel" id="btnLabourRateCancel_' + item.LabourRateId + '"   data-id="' + item.LabourRateId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px; display:none"> <i class="fa-solid fa-rectangle-xmark"></i></button>';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-labourrate-delete" id="btnLabourRateDelete_' + item.LabourRateId + '"   data-id="' + item.LabourRateId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                        html += '</td>';
                        html += '</tr >';
                        sl++;
                    });
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblLabourRate').html(html);
                    if (!$.fn.DataTable.isDataTable('.LabourRateTable')) {
                        $('.LabourRateTable').DataTable({
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
    loadItems();
    function loadItems() {
        $.ajax({
            url: "/Master/GetAllFinishedGood",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    itemId.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    itemId.append(defaultOption);
                    $.each(result.products, function (key, item) {
                        var option = $('<option></option>').val(item.ProductId).text(item.ProductName);
                        itemId.append(option);
                    });

                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    $(document).on('click', '.btn-labourrate-create', CreateLabourRate);
    function CreateLabourRate() {
        if (!date.val()) {
            toastr.error("Date is required.");
            date.focus();
            return;
        }
        if (!itemId.val()) {
            toastr.error("Item Field required.");
            itemId.focus();
            return;

        }
        if (!rate.val()) {
            toastr.error("Rate Field is required.");
            rate.focus();
            return;
        }
        const data = {
            FormtedDate: date.val(),
            Fk_ProductId: itemId.val(),
            Rate: rate.val(),
        }
        console.log(data);
        $.ajax({
            type: "POST",
            url: '/Master/CreateLabourRate',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                loadLabourRates();
                if (Response.ResponseCode = 201) {
                    toastr.success(Response.SuccessMsg);
                    date.val('');
                    rate.val('0');
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
    $(document).on('click', '.btn-labourrate-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        EditLabourRate(value);
    });
    function EditLabourRate(id) {

        var $tr = $('#btnLabourRateEdit_' + id + '').closest('tr');
        var date = moment($tr.find('td:eq(1)').text().trim(), 'DD MMM YYYY').format('DD/MM/YYYY');
        var ProductName = $tr.find('td:eq(2)').text().trim();
        var rate = $tr.find('td:eq(3)').text().trim();
        //****************Date  Input***********************/
        var dateInputHtml = `
        <div class="input-group date" id="datepicker_${id}" data-target-input="nearest">
            <input type="text" class="form-control datetimepicker-input" data-target="#datepicker_${id}" name="date" value="${date}" />
            <div class="input-group-append" data-target="#datepicker_${id}" data-toggle="datetimepicker">
                <div class="input-group-text"><i class="fa fa-calendar"></i></div>
            </div>
        </div>`;
        $tr.find('td:eq(1)').html(dateInputHtml);
        $('#datepicker_' + id).datetimepicker({
            format: 'DD/MM/YYYY',
        });

        //****************Select Input***********************/
        var html = '';
        $.ajax({
            url: "/Master/GetAllFinishedGood",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {

                if (result.ResponseCode == 302) {
                    html += '<div class="form-group">';
                    html += '<select class="form-control select2bs4" style="width: 100%;" name="ItemId">';
                    $.each(result.products, function (key, item) {
                        if (item.ProductName === ProductName) {
                            html += '<option value="' + item.ProductId + '" selected>' + item.ProductName + '</option>';
                        } else {
                            html += '<option value="' + item.ProductId + '">' + item.ProductName + '</option>';
                        }
                    });
                    html += '</select>';
                    html += '</div>';
                    $tr.find('td:eq(2)').html(html);
                    $tr.find('.select2bs4').select2({
                        theme: 'bootstrap4'
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
        //***************************************************/
        $tr.find('td:eq(3)').html('<div class="form-group"><input type="text" class="form-control" value="' + rate + '"></div>');
        $tr.find('#btnLabourRateEdit_' + id + ', #btnLabourRateDelete_' + id + '').hide();
        $tr.find('#btnLabourRateUpdate_' + id + ',#btnLabourRateCancel_' + id + '').show();
    }
    $(document).on('click', '.btn-labourrate-update', (event) => {
        const value = $(event.currentTarget).data('id');
        UpdatLabourRate(value);
    });
    function UpdatLabourRate(id) {
        var $tr = $('#btnLabourRateUpdate_' + id + '').closest('tr');
        const data = {
            LabourRateId: id,
            FormtedDate: $tr.find($('#datepicker_' + id + ' input.datetimepicker-input')).val(),
            Fk_ProductId: $tr.find('Select').eq(0).find('option:selected').val(),
            Rate: $tr.find('input[type="text"]').eq(1).val(),
        }
        console.log(data);
        $.ajax({
            type: "POST",
            url: '/Master/UpdateLabourRate',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                loadLabourRates();
                if (Response.ResponseCode = 200) {
                    toastr.success(Response.SuccessMsg);
                    date.val('');
                    rate.val('0');
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
    $(document).on('click', '.btn-labourrate-cancel', (event) => {
        const value = $(event.currentTarget).data('id');
        CancelLabourRate(value);
    });
    function CancelLabourRate(id) {
        var $tr = $('#btnLabourRateCancel_' + id + '').closest('tr');
        var date = moment($tr.find($('#datepicker_' + id + ' input.datetimepicker-input')).val(), 'DD/MM/YYYY').format('DD/MM/YYYY');
        var productName = $tr.find('Select').eq(0).find('option:selected').text();
        var rate = $tr.find('input[type="text"]').eq(1).val();
        $tr.find('td:eq(1)').text(date);
        $tr.find('td:eq(2)').text(productName);
        $tr.find('td:eq(3)').text(rate);
        $tr.find('#btnLabourRateEdit_' + id + ', #btnLabourRateDelete_' + id + '').show();
        $tr.find('#btnLabourRateUpdate_' + id + ',#btnLabourRateCancel_' + id + '').hide();
    }
    $(document).on('click', '.btn-labourrate-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeleteLabourRate(value);
    });
    function DeleteLabourRate(id) {
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
                // Make an AJAX call to the server-side delete action method
                $.ajax({
                    url: '/Master/DeleteLabourRate?id=' + id + '',
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        loadLabourRates();
                        if (result.ResponseCode = 200) {
                            toastr.success(result.SuccessMsg);
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
})



