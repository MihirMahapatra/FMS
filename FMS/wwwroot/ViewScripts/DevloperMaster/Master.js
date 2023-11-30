$(function () {
    $("#developerLink").addClass("active");
    $("#Setuplink").addClass("active");
    $("#Setuplink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //******************************************** Variable Declaration Section****************************//
    const branchName = $('#txtBranchName');
    const contactNumber = $('#txtContactNumber');
    const branchAddress = $('#txtBranchAddress');
    const branchCode = $('#txtBranchCode');
    const financialYear = $('#txtFinancialYear');
    const startDate = $('input[name="StartDate"]');
    const endDate = $('input[name="EndDate"]');
    const FinancialYearBranchId = $('select[name="ddlFinancialYearBranchId"]');
    const LedgerBranchId = $('select[name="ddlLedgerBranchId"]');
    const ddlLedgerGroup = $('select[name="LedgerGroupId"]');
    const ddlLedgerSubGroup = $('select[name="LedgerSubGroupId"]');
    //***************************************Validation Section****************************************//
    branchName.on("input", function () {
        let inputValue = $(this).val();
        inputValue = inputValue.replace(/[^A-Za-z\s]/g, '');
        inputValue = inputValue.toUpperCase();
        $(this).val(inputValue);
    });
    contactNumber.on("input", function () {
        var inputValue = $(this).val().replace(/\D/g, '');
        if (inputValue.length > 10) {
            inputValue = inputValue.substr(0, 10);
        }
        $(this).val(inputValue);
    });
    branchAddress.on("input", function () {
        $(this).val($(this).val().toUpperCase());
    });
    financialYear.on("blur", function () {
        const enteredText = financialYear.val();
        const regex = /^\d{4}-\d{4}$/;
        if (!regex.test(enteredText)) {
            toastr.error("Financial Year Format Should Be 2008-2009");
        }
    })
    startDate.on("blur", function () {
        let inputValue = $(this).val();
        var dateRegex = /^(\d{2})\/(\d{2})\/(\d{4})$/;
        if (!dateRegex.test(inputValue)) {
            toastr.error("Invalid date format. Please use DD/MM/YYYY.");
        }
    });
    endDate.on("blur", function () {
        let inputValue = $(this).val();
        var dateRegex = /^(\d{2})\/(\d{2})\/(\d{4})$/;
        if (!dateRegex.test(inputValue)) {
            toastr.error("Invalid date format. Please use DD/MM/YYYY.");
        }
    });
    //***************************************Branch****************************************//
    //function Declration  
    function loadBranch() {
        $('.tblBranch').empty();
        $.ajax({
            url: "/Devloper/GetAllBranch",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {

                var html = '';
                if (result.ResponseCode == 404) {
                    html += '<table class="table table-bordered table-hover text-center mt-1 BranchTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th hidden>Branch Id</th>'
                    html += '<th>Branch Name</th>'
                    html += '<th>Branch Address</th>'
                    html += '<th>Contact Number</th>'
                    html += '<th>Branch Code</th>'
                    html += '<th>Action</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    html += '<tr>';
                    html += '<td colspan="5">No record</td>';
                    html += '</tr>';
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblBranch').html(html);
                }
                if (result.ResponseCode == 302) {
                    html += '<table class="table table-bordered table-hover text-center mt-1 BranchTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th hidden>Branch Id</th>'
                    html += '<th>Branch Name</th>'
                    html += '<th>Branch Address</th>'
                    html += '<th>Contact Number</th>'
                    html += '<th>Branch Code</th>'
                    html += '<th>Action</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    $.each(result.Branches, function (key, item) {
                        html += '<tr>';
                        html += '<td hidden>' + item.BranchId + '</td>';
                        html += '<td>' + item.BranchName + '</td>';
                        html += '<td>' + item.BranchAddress + '</td>';
                        html += '<td>' + item.ContactNumber + '</td>';
                        html += '<td>' + item.BranchCode + '</td>';
                        html += '<td style="background-color:#ffe6e6;">';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-branch-edit"   id="btnBranchEdit_' + item.BranchId + '"     data-id="' + item.BranchId + '"  style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-branch-update" id ="btnBranchUpdate_' + item.BranchId + '" data-id="' + item.BranchId + '" style = "border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;display:none" > <i class="fa-solid fa-floppy-disk"></i></button >';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-branch-cancel" id="btnBranchCancel_' + item.BranchId + '"   data-id="' + item.BranchId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px; display:none"> <i class="fa-solid fa-rectangle-xmark"></i></button>';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-branch-delete" id="btnBranchDelete_' + item.BranchId + '"   data-id="' + item.BranchId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                        html += '</td>';
                        html += '</tr >';
                    });
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblBranch').html(html);
                    if (!$.fn.DataTable.isDataTable('.BranchTable')) {
                        $('.BranchTable').DataTable({
                            "paging": true,
                            "lengthChange": false,
                            "searching": true,
                            "ordering": true,
                            "info": true,
                            "autoWidth": true,
                            "responsive": true,
                            "dom": '<"row"<"col-md-2"f><"col-md-2"l>>rtip'
                        });
                    }
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
    function CreateBranch() {
        if (contactNumber.val().length !== 10) {
            toastr.error("Contact number must be exactly 10 digits.");
            return;
        }
        if (!branchName.val()) {
            toastr.error("Branch Name is required.");
            return;
        }
        if (!branchAddress.val()) {
            toastr.error("Address is required.");
            return;
        }
        if (!branchCode.val()) {
            toastr.error("Branch Code is required.");
            return;
        }

        const data = {
            BranchName: branchName.val(),
            ContactNumber: contactNumber.val(),
            BranchAddress: branchAddress.val(),
            BranchCode: branchCode.val(),
        }
        $.ajax({
            type: "POST",
            url: '/Devloper/CreateBranch',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                loadBranch();
                loadBranchForFinancialYear()
                if (Response.ResponseCode = 201) {
                    toastr.success(Response.SuccessMsg);
                    branchName.val('');
                    contactNumber.val('');
                    branchAddress.val('');
                    branchCode.val('');
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
    function BranchEdit(Id) {

        var $tr = $('#btnBranchEdit_' + Id + '').closest('tr');
        var BranchName = $tr.find('td:eq(1)').text().trim();
        var BranchAddress = $tr.find('td:eq(2)').text().trim();
        var ContactNumber = $tr.find('td:eq(3)').text().trim();
        var BranchCode = $tr.find('td:eq(4)').text().trim();

        $tr.find('td:eq(1)').html('<div class="form-group"><input type="text" class="form-control" value="' + BranchName + '"></div>');
        $tr.find('td:eq(2)').html('<div class="form-group"><input type="text" class="form-control" value="' + BranchAddress + '"></div>');
        $tr.find('td:eq(3)').html('<div class="form-group"><input type="text" class="form-control" value="' + ContactNumber + '"></div>');
        $tr.find('td:eq(4)').html('<div class="form-group"><input type="text" class="form-control" value="' + BranchCode + '"></div>');

        $tr.find('#btnBranchEdit_' + Id + ', #btnBranchDelete_' + Id + '').hide();
        $tr.find('#btnBranchUpdate_' + Id + ',#btnBranchCancel_' + Id + '').show();
    }
    function BranchUpdate(id) {
        var $tr = $('#btnBranchUpdate_' + id + '').closest('tr');
        const data = {
            BranchId: id,
            BranchName: $tr.find('input[type="text"]').eq(0).val(),
            BranchAddress: $tr.find('input[type="text"]').eq(1).val(),
            ContactNumber: $tr.find('input[type="text"]').eq(2).val(),
            BranchCode: $tr.find('input[type="text"]').eq(3).val(),
        }
        $.ajax({
            type: "POST",
            url: '/Devloper/UpdateBranch',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                loadBranch();
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
    function BranchCancel(id) {
        var $tr = $('#btnBranchCancel_' + id + '').closest('tr');
        var BranchName = $tr.find('input[type="text"]').eq(0).val();
        var BranchAddress = $tr.find('input[type="text"]').eq(1).val();
        var ContactNumber = $tr.find('input[type="text"]').eq(2).val();
        var BranchCode = $tr.find('input[type="text"]').eq(3).val();

        $tr.find('td:eq(1)').text(BranchName);
        $tr.find('td:eq(2)').text(BranchAddress);
        $tr.find('td:eq(3)').text(ContactNumber);
        $tr.find('td:eq(4)').text(BranchCode);

        $tr.find('#btnBranchEdit_' + id + ', #btnBranchDelete_' + id + '').show();
        $tr.find('#btnBranchUpdate_' + id + ',#btnBranchCancel_' + id + '').hide();
    }
    function BranchDelete(id) {
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
                    url: '/Devloper/DeleteBranch?id=' + id + '',
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {

                        loadBranch();

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
    //function Call
    loadBranch();
    $(document).on('click', '.btn-Branch-create', CreateBranch);
    $(document).on('click', '.btn-branch-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        BranchEdit(value);
    });
    $(document).on('click', '.btn-branch-update', (event) => {
        const value = $(event.currentTarget).data('id');
        BranchUpdate(value);
    });
    $(document).on('click', '.btn-branch-cancel', (event) => {
        const value = $(event.currentTarget).data('id');
        BranchCancel(value);
    });
    $(document).on('click', '.btn-branch-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        BranchDelete(value);
    });
    //**********************************Financial Year**************************************//
    //function Declration
    function loadBranchForFinancialYear() {
        FinancialYearBranchId.empty();
        var defaultOption = $('<option></option>').val('').text('--Select Option--');
        FinancialYearBranchId.append(defaultOption);
        $.ajax({
            url: "/Devloper/GetAllBranch",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    $.each(result.Branches, function (key, item) {
                        var option = $('<option></option>').val(item.BranchId).text(item.BranchName);
                        FinancialYearBranchId.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    function loadFinancialYear() {
        $('.tblFinancialYear').empty();
        $.ajax({
            url: "/Devloper/GetFinancialYears",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                var html = '';
                if (result.ResponseCode == 404) {
                    html += '<table class="table table-bordered table-hover text-center mt-1 FinancialYearTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th hidden>Financia Year Id</th>'
                    html += '<th>Branch</th>'
                    html += '<th>Financial Year</th>'
                    html += '<th>Start Date</th>'
                    html += '<th>End Date</th>'
                    html += '<th>Action</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    html += '<tr>';
                    html += '<td colspan="6">No record</td>';
                    html += '</tr>';
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblFinancialYear').html(html);
                }
                if (result.ResponseCode == 302) {
                    html += '<table class="table table-bordered table-hover text-center mt-1 FinancialYearTable" style="width:100%">';
                    html += '<thead>'
                    html += '<tr>'
                    html += '<th hidden>Financia Year Id</th>'
                    html += '<th>Branch</th>'
                    html += '<th>Financial Year</th>'
                    html += '<th>Start Date</th>'
                    html += '<th>End Date</th>'
                    html += '<th>Action</th>'
                    html += '</tr>'
                    html += '</thead>'
                    html += '<tbody>';
                    $.each(result.FinancialYears, function (key, item) {
                        html += '<tr>';
                        html += '<td hidden>' + item.FinancialYearId + '</td>';
                        html += '<td>' + item.Branch.BranchName + '</td>';
                        html += '<td>' + item.Financial_Year + '</td>';
                        html += '<td>' + item.StartDate + '</td>';
                        html += '<td>' + item.EndDate + '</td>';
                        html += '<td style="background-color:#ffe6e6;">';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-financialyear-edit"   id="btnFinancialYearEdit_' + item.FinancialYearId + '"     data-id="' + item.FinancialYearId + '"  style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-financialyear-update" id ="btnFinancialYearUpdate_' + item.FinancialYearId + '" data-id="' + item.FinancialYearId + '" style = "border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;display:none" > <i class="fa-solid fa-floppy-disk"></i></button >';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-financialyear-cancel" id="btnFinancialYearCancel_' + item.FinancialYearId + '"   data-id="' + item.FinancialYearId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px; display:none"> <i class="fa-solid fa-rectangle-xmark"></i></button>';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-financialyear-delete" id="btnFinancialYearDelete_' + item.FinancialYearId + '"   data-id="' + item.FinancialYearId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                        html += '</td>';
                        html += '</tr >';
                    });
                    html += ' </tbody>';
                    html += '</table >';
                    $('.tblFinancialYear').html(html);
                    if (!$.fn.DataTable.isDataTable('.FinancialYearTable')) {
                        $('.FinancialYearTable').DataTable({
                            "paging": true,
                            "lengthChange": false,
                            "searching": true,
                            "ordering": true,
                            "info": true,
                            "autoWidth": true,
                            "responsive": true,
                            "dom": '<"row"<"col-md-2"f><"col-md-2"l>>rtip'
                        });
                    }
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
    function CreateFinancialYear() {
        if (!financialYear.val()) {
            toastr.error("Financial Year Field is Requird.");
            return;
        }
        if (!startDate.val()) {
            toastr.error("Start Date is required.");
            return;
        }
        if (!endDate.val()) {
            toastr.error("End Date is required.");
            return;
        }
        if (!FinancialYearBranchId.val() || FinancialYearBranchId.val() === '--Select Option--') {
            toastr.error("Branch is required.");
            return;
        }
        const data = {
            
            Financial_Year: financialYear.val(),
            FK_BranchId: FinancialYearBranchId.val(),
            StartDate: startDate.val(),
            EndDate: endDate.val(),
        }
        console.log(data);
        $.ajax({
            type: "POST",
            url: '/Devloper/CreateFinancialYear',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                loadFinancialYear();
                if (Response.ResponseCode == 201) {
                    toastr.success(Response.SuccessMsg);
                    financialYear.val('');
                    startDate.val('');
                    endDate.val('');
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
    function EditFinancialYear(Id) {
        var $tr = $('#btnFinancialYearEdit_' + Id + '').closest('tr');
        var Branch = $tr.find('td:eq(1)').text().trim();
        var FinancialYear = $tr.find('td:eq(2)').text().trim();
        var StartDate = moment($tr.find('td:eq(3)').text().trim(), 'MMM DD YYYY').format('DD/MM/YYYY');
        var EndDate = moment($tr.find('td:eq(4)').text().trim(), 'MMM DD YYYY').format('DD/MM/YYYY');
        $tr.find('td:eq(1)').html('<div class="form-group"><input type="text" class="form-control" value="' + Branch + '"></div>');
        $tr.find('td:eq(2)').html('<div class="form-group"><input type="text" class="form-control" value="' + FinancialYear + '"></div>');
        //****************Start Date Input***********************/
        var startDateInputHtml = `
        <div class="input-group date" id="datepicker1_${Id}" data-target-input="nearest">
            <input type="text" class="form-control datetimepicker-input" data-target="#datepicker1_${Id}" name="startDate" value="${StartDate}" />
            <div class="input-group-append" data-target="#datepicker1_${Id}" data-toggle="datetimepicker">
                <div class="input-group-text"><i class="fa fa-calendar"></i></div>
            </div>
        </div>`;
        $tr.find('td:eq(3)').html(startDateInputHtml);
        $('#datepicker1_' + Id).datetimepicker({
            format: 'DD/MM/YYYY',
        });
        //****************End Date Input***********************//
        var endDateInputHtml = `
        <div class="input-group date" id="datepicker2_${Id}" data-target-input="nearest">
            <input type="text" class="form-control datetimepicker-input" data-target="#datepicker2_${Id}" name="endDate" value="${EndDate}" />
            <div class="input-group-append" data-target="#datepicker2_${Id}" data-toggle="datetimepicker">
                <div class="input-group-text"><i class="fa fa-calendar"></i></div>
            </div>
        </div>`;
        $tr.find('td:eq(4)').html(endDateInputHtml);
        $('#datepicker2_' + Id).datetimepicker({
            format: 'DD/MM/YYYY',
        });
        //****************************************************//

        $tr.find('#btnFinancialYearEdit_' + Id + ', #btnFinancialYearDelete_' + Id + '').hide();
        $tr.find('#btnFinancialYearUpdate_' + Id + ',#btnFinancialYearCancel_' + Id + '').show();
    }
    function UpdateFinancialYear(id) {
        var $tr = $('#btnFinancialYearUpdate_' + id + '').closest('tr');
        const data = {
            FinancialYearId: id,
            Financial_Year: $tr.find('input[type="text"]').eq(0).val(),
            StartDate: $tr.find($('#datepicker1_' + id + ' input.datetimepicker-input')).val(),
            EndDate: $tr.find($('#datepicker2_' + id + ' input.datetimepicker-input')).val(),
        }
        $.ajax({
            type: "POST",
            url: '/Devloper/UpdateFinancialYear',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                loadFinancialYear();
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

    }
    function CancelFinancialYear(id) {
        var $tr = $('#btnFinancialYearCancel_' + id + '').closest('tr');
        var Branch = $tr.find('input[type="text"]').eq(0).val();
        var Financial_Year = $tr.find('input[type="text"]').eq(1).val();
        var StartDate = moment($tr.find($('#datepicker1_' + id + ' input.datetimepicker-input')).val(), 'DD/MM/YYYY').format('MMM DD YYYY');
        var EndDate = moment($tr.find($('#datepicker2_' + id + ' input.datetimepicker-input')).val(), 'DD/MM/YYYY').format('MMM DD YYYY');
        $tr.find('td:eq(1)').text(Branch);
        $tr.find('td:eq(2)').text(Financial_Year);
        $tr.find('td:eq(3)').text(StartDate);
        $tr.find('td:eq(4)').text(EndDate);
        $tr.find('#btnFinancialYearEdit_' + id + ', #btnFinancialYearDelete_' + id + '').show();
        $tr.find('#btnFinancialYearUpdate_' + id + ',#btnFinancialYearCancel_' + id + '').hide();
    }
    function DeleteFinancialYear(id) {
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
                    url: '/Devloper/DeleteFinancialYear?id=' + id + '',
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        loadFinancialYear();
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

    //function Call
    $('a[href="#FinancialYearMaster"]').on('click', function () {
        loadBranchForFinancialYear();
        loadFinancialYear();
    });
    $(document).on('click', '.btn-financialyear-create', CreateFinancialYear);
    $(document).on('click', '.btn-financialyear-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        EditFinancialYear(value);
    });
    $(document).on('click', '.btn-financialyear-update', (event) => {
        const value = $(event.currentTarget).data('id');
        UpdateFinancialYear(value);
    });
    $(document).on('click', '.btn-financialyear-cancel', (event) => {
        const value = $(event.currentTarget).data('id');
        CancelFinancialYear(value);
    });
    $(document).on('click', '.btn-financialyear-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeleteFinancialYear(value);
    });
    

    //**********************************Accounting**************************************//
    //function Declration
    function LoadLedgerGroup() {
        ddlLedgerGroup.empty();
        var defaultOption = $('<option></option>').val('').text('--Select Option--');
        ddlLedgerGroup.append(defaultOption);
        $.ajax({
            url: "/Devloper/GetLedgerGroups",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    $.each(result.LedgerGroups, function (key, item) {
                        var option = $('<option></option>').val(item.LedgerGroupId).text(item.GroupName);
                        ddlLedgerGroup.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    function loadBranchForLedger() {
        LedgerBranchId.empty();
        var defaultOption = $('<option></option>').val('').text('--Select Option--');
        LedgerBranchId.append(defaultOption);
        $.ajax({
            url: "/Devloper/GetAllBranch",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {

                if (result.ResponseCode == 302) {
                    $.each(result.Branches, function (key, item) {
                        var option = $('<option></option>').val(item.BranchId).text(item.BranchName);
                        LedgerBranchId.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    function enableLedgerSubGroup() {
        var ddlLedgerBranchIdSelected = LedgerBranchId.val();
        var LedgerGroupIdSelected = ddlLedgerGroup.val();

        if (ddlLedgerBranchIdSelected && LedgerGroupIdSelected) {
            ddlLedgerSubGroup.prop("disabled", false);
            LoadLedgerSubGroup(ddlLedgerBranchIdSelected, LedgerGroupIdSelected)

        } else {
            ddlLedgerSubGroup.prop("disabled", true);
        }
    }
    function LoadLedgerSubGroup(BranchId, GroupId) {
        ddlLedgerSubGroup.empty();
        var defaultOption = $('<option></option>').val('').text('--Select Option--');
        ddlLedgerSubGroup.append(defaultOption);
        $.ajax({
            url: '/Devloper/GetLedgerSubGroups?BranchId=' + BranchId + '&GroupId=' + GroupId + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    $.each(result.LedgerSubGroups, function (key, item) {
                        var option = $('<option></option>').val(item.LedgerSubGroupId).text(item.SubGroupName);
                        ddlLedgerSubGroup.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }

     //function Call
    $('a[href="#AccountMaster"]').on('click', function () {
        LoadLedgerGroup()
        loadBranchForLedger()
    });
    
    //--Group--// 
    $('#btnLedgerGrupAdd').on('click', function () {

        $('#modal-add-group').modal('show');
    });
    $('#modal-add-group').on('click', '.ledgerGroupAdd', (event) => {

        if (!$('input[name="mdlLedgerGroupAdd"]').val()) {
            toastr.error('Plz Insert Group Name');
            return;
        }

        const data = {
            GroupName: $('input[name="mdlLedgerGroupAdd"]').val(),
        }
        $.ajax({
            type: "POST",
            url: '/Devloper/CreateLedgerGroup',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                $('#modal-add-group').modal('hide');
                if (Response.ResponseCode == 201) {
                    toastr.success(Response.SuccessMsg);
                    $('input[name="mdlLedgerGroupAdd"]').val('');
                }
                else {
                    toastr.error(Response.ErrorMsg);
                }
                LoadLedgerGroup();
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
    $("#btnLedgerGrupEdit").on('click', function () {
        if (!ddlLedgerGroup.val() || ddlLedgerGroup.val() === '--Select Option--') {
            toastr.error('Plz Select a Group Name To Edit');
            return;
        }
        else {
            $('#modal-edit-group').modal('show');
            const selectedOption = ddlLedgerGroup.find('option:selected');
            var text = selectedOption.text();
            var value = selectedOption.val();
            $("input[name='mdlLedgerGroupEdit']").val(text);
            $("input[name='mdlLedgerGroupId']").val(value);
        }
    });
    $('#modal-edit-group').on('click', '.ledgerGroupEdit', (event) => {
        const data = {
            LedgerGroupId: $('input[name="mdlLedgerGroupId"]').val(),
            GroupName: $('input[name="mdlLedgerGroupEdit"]').val(),
        }
        $.ajax({
            type: "POST",
            url: '/Devloper/UpdateLedgerGroup',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                $('#modal-edit-group').modal('hide');
                LoadLedgerGroup();
                if (Response.ResponseCode == 200) {
                    toastr.success(Response.SuccessMsg);
                    $('input[name="mdlLedgerGroupId"]').val('');
                    $('input[name="mdlLedgerGroupEdit"]').val('');
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
    $('#btnLedgerGrupDelete').on('click', function () {

        if (!ddlLedgerGroup.val() || ddlLedgerGroup.val() === '--Select Option--') {
            toastr.error('Plz Select a Group Name To Edit');
            return;
        }
        const Id = ddlLedgerGroup.find('option:selected').val();
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
                    url: '/Devloper/DeleteLedgerGroup?id=' + Id + '',
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        LoadLedgerGroup();
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
    })
    //--SubGroup--// 
    ddlLedgerGroup.on('change',function () {
        if (LedgerBranchId.val()) {
            enableLedgerSubGroup();
        }
    });
    LedgerBranchId.on('change',function () {
        if (ddlLedgerGroup.val()) {
            enableLedgerSubGroup();
        }
    });
    $('#btnLedgerSubGrupAdd').on('click', function () {
        if (!ddlLedgerGroup.val() || ddlLedgerGroup.val() === '--Select Option--') {
            toastr.error('Plz Select a Group Name For Create a Subgroup');
            return;
        }
        if (!LedgerBranchId.val() || LedgerBranchId.val() === '--Select Option--') {
            toastr.error('Plz Select branch Name For SubGruop');
            return;
        }
        $('#modal-add-subgroup').modal('show');
    });
    $('#modal-add-subgroup').on('click', '.ledgerSubGroupAdd', (event) => {
        const data = {
            Fk_LedgerGroupId: ddlLedgerGroup.val(),
            Fk_BranchId: LedgerBranchId.val(),
            SubGroupName: $('input[name="mdlLedgerSubGroupAdd"]').val(),
        }
        $.ajax({
            type: "POST",
            url: '/Devloper/CreateLedgerSubGroup',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                $('#modal-add-subgroup').modal('hide');
                if (Response.ResponseCode == 201) {
                    toastr.success(Response.SuccessMsg);
                    $('input[name="mdlLedgerSubGroupAdd"]').val('');
                }
                else {
                    toastr.error(Response.ErrorMsg);
                }
                const BranchId = LedgerBranchId.find('option:selected').val();
                const LederGroupId = ddlLedgerGroup.find('option:selected').val();
                LoadLedgerSubGroup(BranchId, LederGroupId);
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
    $('#btnLedgerSubGrupEdit').on('click', function () {
        if (!ddlLedgerGroup.val() || ddlLedgerGroup.val() === '--Select Option--') {
            toastr.error('Plz Select a Group Name');
            return;
        }
        else if (!LedgerBranchId.val() || LedgerBranchId.val() === '--Select Option--') {
            toastr.error('Plz Select branch');
            return;
        }
        else if (!ddlLedgerSubGroup.val() || ddlLedgerSubGroup.val() === '--Select Option--') {
            toastr.error('Plz Select SubGroup Name');
            return;
        }
        else {
            const selectedOption = ddlLedgerSubGroup.find('option:selected');
            var text = selectedOption.text();
            var value = selectedOption.val();
            $("input[name='mdlLedgerSubGroupEdit']").val(text);
            $("input[name='mdlLedgerSubGroupId']").val(value);
            $('#modal-edit-subgroup').modal('show');
        }
    });
    $('#modal-edit-subgroup').on('click', '.ledgerSubGroupEdit', (event) => {
        const data = {
            LedgerSubGroupId: $('input[name="mdlLedgerSubGroupId"]').val(),
            SubGroupName: $('input[name="mdlLedgerSubGroupEdit"]').val(),
            Fk_BranchId: LedgerBranchId.find('option:selected').val(),
            Fk_LedgerGroupId: ddlLedgerGroup.find('option:selected').val()
        }
        $.ajax({
            type: "POST",
            url: '/Devloper/UpdateLedgerSubGroup',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                $('#modal-edit-subgroup').modal('hide');
            
                LoadLedgerSubGroup(data.Fk_BranchId, data.Fk_LedgerGroupId);
                if (Response.ResponseCode == 200) {
                    toastr.success(Response.SuccessMsg);
                    $('input[name="mdlLedgerGroupId"]').val('');
                    $('input[name="mdlLedgerGroupEdit"]').val('');
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
    $('#btnLedgerSubGrupDelete').on('click', function () {

        if (!ddlLedgerSubGroup.val() || ddlLedgerSubGroup.val() === '--Select Option--') {
            toastr.error('Plz Select a Group Name To Edit');
            return;
        }
        if (!LedgerBranchId.val() || LedgerBranchId.val() === '--Select Option--') {
            toastr.error('Plz Select a Branch From Which You Want To Delete SubLedger');
            return;
        }
        const Id = ddlLedgerSubGroup.find('option:selected').val();
        const BranchId = LedgerBranchId.find('option:selected').val();
        const LederGroupId = ddlLedgerGroup.find('option:selected').val();
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
                    url: '/Devloper/DeleteLedgerSubGroup?id=' + Id + '&BranchId=' + BranchId + '',
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        LoadLedgerSubGroup(BranchId, LederGroupId);
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
    })
    //--Ledgers--//
    var ledgerTable = $('#tblLedger').DataTable({
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
    $('#addLedgerRowBtn').on('click',function () {
        var newRowData = [
            '<div class="form-group"><select class="form-control select2bs4" style="width: 100%;"><option selected="selected" value="None">None</option><option value="Cash">Cash</option><option value="Bank">Bank</option></select></div>',
            '<div class="form-group"><input type="text" class="form-control" id=""></div>',
            '<div class="form-group"><input type="text" class="form-control" id=""></div>',
            '<div class="form-group"><select class="form-control select2bs4" style="width: 100%;"><option value="CR" selected="selected">CR</option><option value="DR">DR</option></select></div>'
        ];
        ledgerTable.row.add(newRowData).draw();
        $('#tblLedger tbody').find('.select2bs4').select2({
            theme: 'bootstrap4'
        });
    });
    $('#btnLedger').on('click',function () {
        if (!LedgerBranchId.val() || LedgerBranchId.val() === '--Select Option--') {
            toastr.error('Plz Select a branch Name For Ledger');
            return;
        }
        if (!ddlLedgerGroup.val() || ddlLedgerGroup.val() === '--Select Option--') {
            toastr.error('Plz Select a Group Name For Ledger');
            return;
        }
        var rowData = [];
        $('#tblLedger tbody tr').each(function () {
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
        var selectedLedgerGroupId = ddlLedgerGroup.val();
        var selectedLedgerBranch = LedgerBranchId.val();
        var selectedLedgerSubGroupId = ddlLedgerSubGroup.val();
        var requestData = {
            BranchId: selectedLedgerBranch,
            LedgerGroupId: selectedLedgerGroupId,
            LedgerSubGroupId: selectedLedgerSubGroupId,
            rowData: rowData
        };
        $.ajax({
            type: "POST",
            url: '/Devloper/CreateLedgers',
            dataType: 'json',
            data: JSON.stringify(requestData),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                if (Response.ResponseCode == 201) {
                    toastr.success(Response.SuccessMsg);
                    ledgerTable.clear().draw();
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
})



