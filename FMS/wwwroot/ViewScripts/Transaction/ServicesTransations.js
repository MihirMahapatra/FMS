$(function () {
    $("#TransactionLink").addClass("active");
    $("#ServiceLink").addClass("active");
    $("#ServiceLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //----------------------------------------varible declaration-----------------------------------------//
    //default date
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const todayDate = `${day}/${month}/${year}`;
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
    
    var ServiceEntryTable = $('#tblServiceEntry').DataTable({
        "paging": false,
        "lengthChange": false,
        "searching": false,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true,
        lengthMenu: [5, 10, 25, 50], // Set the available page lengths
        pageLength: 5 // Set the default page length to 5
    });
    const TransactionNo = $('input[name="TransactionNo"]');
    const ServiceDate = $('input[name="ServiceDate"]');
    ServiceDate.val(todayDate);
    const ddlServicesGood = $('select[name="ddlServiceGoodId"]');
    const ddlLabour = $('select[name="ddlLabourId"]');

    //----------------------------------------Production Screen-----------------------------------------//
    GetLastServiceNo();
    function GetLastServiceNo() {
        $.ajax({
            url: "/Transaction/GetLastServiceNo",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                TransactionNo.val(result.Data);
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    GetServicesGoods();
    function GetServicesGoods() {
        $.ajax({
            url: "/Transaction/GetServiceGoods",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddlServicesGood.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlServicesGood.append(defaultOption);
                    $.each(result.products, function (key, item) {
                        var option = $('<option></option>').val(item.ProductId).text(item.ProductName);
                        ddlServicesGood.append(option);
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
            url: "/Transaction/GetServiceLabours",
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
        html += '<td><div class="form-group"><select required class="form-control form-control-sm select2bs4 ServiceGood ServiceGood_' + uniqueId + '" style="width: 100%;" id="' + uniqueId + '"></select></div></td>';
        html += '<td> <div class="form-group"><select required class=" form-control select2bs4 labour_' + uniqueId + ' labour" style="width: 100%;"></select></div></td>';
        html += ' <td><div class="form-group"><input type="text" class=" form-control" disabled></div></td>';
        html += '<td>' +
            '<div class="form-group">' +
            '<div class="input-group">' +
            '<input type="text" class="form-control" value="0">' +
            ' <div class="input-group-append">' +
            ' <span class="input-group-text" id="Unit">N/A</span>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</td>';
        html += '<td><div class="form-group"><input type="text" class="form-control" disabled></div></td>';
        html += '<td><div class="form-group"><input type="text" class="form-control"></div></td>';
        html += '<td><div class="form-group"><input type="text" class="form-control" disabled></div></td>';
        html += '<td style="background-color:#ffe6e6;">';
        html += '<button class="btn btn-primary btn-link addBtn" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-plus"></i></button>';
        html += ' <button class="btn btn-primary btn-link deleteBtn" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
        html += '</td>';
        html += '</tr>';
        var newRow = ServiceEntryTable.row.add($(html)).draw(false).node();
        $.ajax({
            url: "/Transaction/GetServiceGoods",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    var selectElement = $('.ServiceGood_' + uniqueId + '');
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
            url: "/Transaction/GetServiceLabours",
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
        $('#tblServiceEntry tbody').find('.select2bs4').select2({
            theme: 'bootstrap4'
        });
    });
    $(document).on('click', '.deleteBtn', function () {
        $(this).closest('tr').remove();
    });
    $(document).on('change', '.ServiceGood', function () {
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
                            var span = selectElement.closest('tr').find('span#Unit');
                            var Texthas = result.LabourRate.Product.Unit.UnitName;
                            if (Texthas != null) {
                                span.text(result.LabourRate.Product.Unit.UnitName);
                            } else {
                                span.text("N/A");
                            }


                            //console.log();
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
    $('#tblServiceEntry tbody').on('change', 'input[type="text"]', function () {
        var row = $(this).closest('tr');
        var quantity = parseFloat(row.find('input:eq(1)').val());
        var rate = parseFloat(row.find('input:eq(2)').val());
        var otamount = parseFloat(row.find('input:eq(3)').val());
        var amount = (quantity * rate) + otamount;
        row.find('input:eq(4)').val(amount.toFixed(2));
    });
    $('#btnSave').on('click', function () {
        if (!ServiceDate.val()) {
            toastr.error('Services Date  Is Required.');
            return;
        } else {
            $('#loader').show();
            var rowData = [];
            $('#tblServiceEntry tbody tr').each(function () {
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
                ProductionNo: TransactionNo.val(),
                ProductionDate: ServiceDate.val(),
                rowData: rowData
            };
            console.log(requestData)
            $.ajax({
                type: "POST",
                url: '/Transaction/CreateServiceEntry',
                dataType: 'json',
                data: JSON.stringify(requestData),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#loader').hide();
                    if (Response.ResponseCode == 201) {
                        toastr.success(Response.SuccessMsg);
                        ServiceDate.val(todayDate)
                        ServiceEntryTable.row('tbody tr:not(:first-child)').remove().draw();
                        $('.qty').val('');
                        $('.rate').val('');
                        $('.amount').val('');
                        $('.otamount').val('');
                    }
                    else {
                        toastr.error(Response.ErrorMsg);
                    }
                    GetLastProductionNo();
                },
                error: function (error) {
                    console.log(error);
                    $('#loader').hide();
                }
            });
        }

    });

    //----------------------------------------Production List-----------------------------------------//
    //Edit
    $('a[href="#ServiceEntryList"]').on('click', function () {
        GetServiceEntryList();
    });
   
    function GetServiceEntryList() {
        $('#loader').show();
        $('.tblServiceEntryListTable').empty();
        $.ajax({
            url: "/Transaction/GetServiceEntry",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#loader').hide();
                var html = '';
                html += '<table class="table table-bordered table-hover text-center mt-1 tblServiceEntryListTable" style="width:100%">';
                html += '<thead>'
                html += '<tr>'
                html += '<th hidden>ProductionEntryId</th>'
                html += '<th>Service No</th>'
                html += '<th>Service Date</th>'
                html += '<th>Product Name</th>'
                html += '<th>Labour Name</th>'
                html += '<th>Labour Type</th>'
                html += '<th>Quantity</th>'
                html += '<th>Rate</th>'
                html += '<th>OTAmount</th>'
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
                        html += '<td>' + item.OTAmount + '</td>';
                        html += '<td>' + item.Amount + '</td>';
                        html += '<td style="background-color:#ffe6e6;">';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-serviceEntry-edit"   id="btnServicesEntryEdit_' + item.ProductionEntryId + '"     data-id="' + item.ProductionEntryId + '" data-toggle="modal" data-target="#modal-edit-service-entry" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-serviceEntry-delete" id="btnServicesEntryDelete_' + item.ProductionEntryId + '"   data-id="' + item.ProductionEntryId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
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
                $('.tblServiceEntryList').html(html);
                if (!$.fn.DataTable.isDataTable('.tblServiceEntryListTable')) {
                    var table = $('.tblServiceEntryListTable').DataTable({
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
    $(document).on('click', '.btn-serviceEntry-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        EditServiceEntry(value);
    });
    function EditServiceEntry(Id) {
        var $tr = $('#btnServicesEntryEdit_' + Id + '').closest('tr');
        var TransactionNo = $tr.find('td:eq(1)').text().trim();
        var ServiceDate = $tr.find('td:eq(2)').text().trim();
        var productId = $tr.find('td:eq(3)').text().trim();
        var labourId = $tr.find('td:eq(4)').text().trim();
        var labourType = $tr.find('td:eq(5)').text().trim();
        var quantity = $tr.find('td:eq(6)').text().trim();
        var rate = $tr.find('td:eq(7)').text().trim();
        var Otamount = $tr.find('td:eq(8)').text().trim();
        var amount = $tr.find('td:eq(9)').text().trim();
        //fill Modal data
        $('input[name="mdlServiceEntryId"]').val(Id);
        $('input[name="mdlTransactionNo"]').val(TransactionNo);
        $('input[name="mdlTransactionDate"]').val(ServiceDate);
        $('input[name="mdlLabourType"]').val(labourType);
        $('input[name="mdlQuantity"]').val(quantity);
        $('input[name="mdlRate"]').val(rate);
        $('input[name="mdlOtAmount"]').val(Otamount);
        $('input[name="mdlAmount"]').val(amount);
       
        $.ajax({
            url: "/Transaction/GetServiceGoods",
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
            url: "/Transaction/GetServiceLabours",
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
    $('input[name="mdlOtAmount"]').change(function () {
        var quantity = $('input[name="mdlQuantity"]').val();
        var rate = $('input[name="mdlRate"]').val();
        var otamount = $('input[name="mdlOtAmount"]').val();
        var amount = (quantity * rate) + otamount;
        $('input[name="mdlAmount"]').val(amount)
    });
    //Update
    $('#modal-edit-service-entry').on('click', '.btnUpdate', (event) => {
        const data = {
            ProductionEntryId: $('input[name="mdlServiceEntryId"]').val(),
            ProductionNo: $('input[name="mdlTransactionNo"]').val(),
            Date: $('input[name="mdlTransactionDate"]').val(),
            Fk_ProductId: $('select[name="mdlProductId"]').val(),
            Fk_LabourId: $('select[name="mdlLabourId"]').val(),
            LabourType: $('input[name="mdlLabourType"]').val(),
            Quantity: $('input[name="mdlQuantity"]').val(),
            Rate: $('input[name="mdlRate"]').val(),
            OTAmount: $('input[name="mdlOtAmount"]').val(),
            Amount: $('input[name="mdlAmount"]').val(),
            LedgerId: $('input[name="mdlLedgerId"]').val(),
        }

        $.ajax({
            type: "POST",
            url: '/Transaction/UpdateServiceEntry',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                $('#modal-edit-service-entry').modal('hide');
                GetServiceEntryList();
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
    $(document).on('click', '.btn-serviceEntry-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeleteserviceEntry(value);
    });
    function DeleteserviceEntry(Id) {
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
                    url: '/Transaction/DeleteServiceEntry?id=' + Id + '',
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        if (result.ResponseCode == 200) {
                            toastr.success(result.SuccessMsg);
                            GetServiceEntryList();
                            GetLastServiceNo();
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