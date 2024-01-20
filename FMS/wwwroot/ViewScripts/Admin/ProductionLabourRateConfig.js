$(function () {
    $("#AdminLink").addClass("active");
    $("#ProductionLabourRateConfigLink").addClass("active");
    $("#ProductionLabourRateConfigLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    /***************************************Variable Declaration***********************************************************/
    //Default Date 
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;
    const date = $('input[name="date"]');
    date.val(todayDate);
    const productTypeId = $('select[name="ProductTypeId"]');
    const itemId = $('select[name="ItemId"]');
    const rate = $('input[name="Rate"]');
    /***************************************Validation Section***********************************************************/
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
    //*******************************************Labour Rate*******************************************//
    loadLabourRates();
    function loadLabourRates() {
        $('#loader').show();
        $('.tblLabourRate').empty();
        $.ajax({
            url: "/Admin/GetProductionLabourRates",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                var html = '';
                var sl = 1;

                $('#loader').hide();
                html += '<table class="table table-bordered table-hover text-center mt-1 LabourRateTable" style="width:100%">';
                html += '<thead>'
                html += '<tr>'
                html += '<th hidden>Labour Rate Id</th>'
                html += '<th style="width:20%">Date</th>'
                html += '<th style="width:20%">Product Type</th>'
                html += '<th style="width:30%">Product</th>'
                html += '<th style="width:10%">Rate</th>'
                html += '<th style="width:20%">Action</th>'
                html += '</tr>'
                html += '</thead>'
                html += '<tbody>';
                if (result.ResponseCode == 302) {
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
                        if (item.ProductType !== null) {
                            html += '<td>' + item.ProductType.Product_Type + '</td>';
                        }
                        else {
                            html += '<td>-</td>';
                        }
                        if (item.Product !== null) {
                            html += '<td>' + item.Product.ProductName + '</td>';
                        }
                        else {
                            html += '<td>-</td>';
                        }
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

                }
                else {
                    html += '<tr>';
                    html += '<td colspan="6">No record</td>';
                    html += '</tr>';
                }
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
    LoadProducts();
    function LoadProducts() {
        itemId.empty();
        var defaultOption = $('<option></option>').val('').text('--Select Option--');
        itemId.append(defaultOption);
        $.ajax({
            url: '/Admin/GetProductFinishedGoods',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    $.each(result.Products, function (key, item) {
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
    $('.btn-labourrate-create').on('click', CreateLabourRate);
    function CreateLabourRate() {
        if (!date.val()) {
            toastr.error("Date is required.");
            date.focus();
            return;
        }
        else if (!itemId.val() || itemId.val() === '--Select Option--') {
            toastr.error("Item Field required.");
            itemId.focus();
            return;
        }
        else if (!rate.val()) {
            toastr.error("Rate Field is required.");
            rate.focus();
            return;
        }
        else {
            $('#loader').show();
            const data = {
                FormtedDate: date.val(),
                Fk_ProductId: itemId.val(),
                Rate: rate.val(),
            }
            $.ajax({
                type: "POST",
                url: '/Admin/CreateLabourRate',
                dataType: 'json',
                data: JSON.stringify(data),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#loader').hide();
                    if (Response.ResponseCode = 201) {
                        toastr.success(Response.SuccessMsg);
                        date.val(todayDate);
                        rate.val('0');
                    }
                    else {
                        toastr.error(Response.ErrorMsg);
                    }
                },
                error: function (error) {
                    console.log(error);
                    $('#loaderBtn').hide();
                }
            });
        }
    }
    $(document).on('click', '#btnRefresh', loadLabourRates);  
    $(document).on('click', '.btn-labourrate-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        EditLabourRate(value);
    });
    function EditLabourRate(id) {
        var $tr = $('#btnLabourRateEdit_' + id + '').closest('tr');
        var date = moment($tr.find('td:eq(1)').text().trim(), 'DD MMM YYYY').format('DD/MM/YYYY');
        var ProductType = $tr.find('td:eq(2)').text().trim();
        var ProductName = $tr.find('td:eq(3)').text().trim();
        var rate = $tr.find('td:eq(4)').text().trim();
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
        $tr.find('td:eq(4)').html('<div class="form-group"><input type="text" class="form-control" value="' + rate + '"></div>');
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
            //Fk_ProductTypeId: $tr.find('Select').eq(0).find('option:selected').val(),
            Fk_ProductId: $tr.find('Select').eq(1).find('option:selected').val(),
            Rate: $tr.find('input[type="text"]').eq(1).val(),
        }
        console.log(data);
        $.ajax({
            type: "POST",
            url: '/Admin/UpdateLabourRate',
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
        //var ProductType = $tr.find('Select').eq(0).find('option:selected').text();
        //var productName = $tr.find('Select').eq(1).find('option:selected').text();
        var rate = $tr.find('input[type="text"]').eq(1).val();
        $tr.find('td:eq(1)').text(date);
        //$tr.find('td:eq(2)').text(ProductType);
        //$tr.find('td:eq(3)').text(productName);
        $tr.find('td:eq(4)').text(rate);
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
                    url: '/Admin/DeleteLabourRate?id=' + id + '',
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
});