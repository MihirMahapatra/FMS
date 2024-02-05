$(function () {
    $("#MasterLink").addClass("active");
    $("#PartyMasterLink").addClass("active");
    $("#PartyMasterLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    /***************************************Variable Declaration********************************************/
    const ddlpartyType = $('select[name="ddnPartyTypeId"]');
    const partyName = $('input[name="PartyName"]');
    const ddlState = $('select[name="ddnStateId"]');
    const ddlCity = $('select[name="ddnCityId"]');
    const phoneNo = $('input[name="PhoneNo"]');
    const email = $('input[name="Mail"]');
    const address = $('input[name="Address"]');
    const gstNo = $('input[name="GstNo"]');
    //-----------------------------------Contorl Foucous Of Element    PartyMaster---------------------------//
    ddlpartyType.focus();
    partyName.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    partyName.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    phoneNo.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    phoneNo.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    email.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    email.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    address.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    address.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    gstNo.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    gstNo.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    $('.btn-primary').on('keydown', function (e) {
        if (e.key === 'Enter' || e.keyCode === 12) {
            $('.btn-primary').click();
        }
    });
    $('.btn-primary').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('.btn-primary').on('blur', function () {
        $(this).css('background-color', '');
    });
    $('.btn-party-create').on('keydown', function (e) {
        if (e.key === 'Enter' || e.keyCode === 12) {
            $('.btn - party - create').click();
        }
    });
    $('.btn-party-create').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('.btn-party-create').on('blur', function () {
        $(this).css('background-color', '');
    });

    //***************************************Validation Section****************************************//
    partyName.on("input", function () {
        let inputValue = $(this).val();
        inputValue = inputValue.toUpperCase();
        $(this).val(inputValue);
    });
    phoneNo.on("input", function () {
        var inputValue = $(this).val().replace(/\D/g, '');
        if (inputValue.length > 10) {
            inputValue = inputValue.substr(0, 10);
        }
        $(this).val(inputValue);
    });
    email.on('blur', function (event) {
        const Regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
        const isValid = Regex.test(emailInput.val());
        if (!isValid) {
            toastr.error("Invalid Email");
        }
    });
    address.on("input", function () {
        let inputValue = $(this).val();
        inputValue = inputValue.toUpperCase();
        $(this).val(inputValue);
    });
    gstNo.on("input", function () {
        let inputValue = $(this).val();
        inputValue = inputValue.toUpperCase();
        $(this).val(inputValue);
    });
    /*******************************************Party***********************************************/
    LoadPartyType();
    function LoadPartyType() {
        $.ajax({
            url: "/Master/GetPartyTypes",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                ddlpartyType.empty();
                var defaultOption = $('<option></option>').val('').text('--Select Option--');
                ddlpartyType.append(defaultOption);
                $.each(result, function (key, item) {
                    var option = $('<option></option>').val(item.LedgerId).text(item.LedgerName);
                    ddlpartyType.append(option);
                });
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    GetParties();
    function GetParties() {
        $('.PartyTable').empty();
        $.ajax({
            url: "/Master/GetParties",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#loader').hide();
                var html = '';
                html += '<table class="table table-bordered table-hover mt-1 PartyTable" style="width:100%">';
                html += '<thead>'
                html += '<tr>'
                html += '<th hidden>Party Id</th>'
                html += '<th>Party Name</th>'
                html += '<th>Party Type</th>'
                html += '<th>State</th>'
                html += '<th>City</th>'
                html += '<th>Phone No</th>'
                html += '<th>Email</th>'
                html += '<th hidden>Address</th>'
                html += '<th>GST No</th>'
                html += '<th>Action</th>'
                html += '</tr>'
                html += '</thead>'
                html += '<tbody>';
                if (result.ResponseCode == 302) {
                    $.each(result.Parties, function (key, item) {
                        html += '<tr>';
                        html += '<td hidden>' + item.PartyId + '</td>';
                        html += '<td>' + item.PartyName + '</td>';
                        html += '<td>' + item.Ledger.LedgerName + '</td>';
                        html += '<td>' + item.State.StateName + '</td>';
                        html += '<td>' + item.City.CityName + '</td>';
                        html += '<td >' + item.Phone + '</td>';
                        html += '<td>' + item.Email + '</td>';
                        html += '<td hidden>' + item.Address + '</td>';
                        html += '<td>' + item.GstNo + '</td>';
                        html += '<td style="background-color:#ffe6e6;">';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-party-edit"   id="btnPartyEdit_' + item.PartyId + '"     data-id="' + item.PartyId + '" data-toggle="modal" data-target="#modal-party-edit" style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-party-delete" id="btnPartyDelete_' + item.PartyId + '"   data-id="' + item.PartyId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                        html += '</td>';
                        html += '</tr >';
                    });
                }

                else {
                    html += '<tr>';
                    html += '<td colspan="15">No record</td>';
                    html += '</tr>';
                }
                html += ' </tbody>';
                html += '</table >';
                $('.tblPartyList').html(html);
                if (!$.fn.DataTable.isDataTable('.PartyTable')) {
                    var table = $('.PartyTable').DataTable({
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
    $(document).on('click', '#btnRefresh', GetParties);
    $('.btn-party-create').on('click', CreateParty);
    function CreateParty() {
        if (!ddlpartyType.val() || ddlpartyType.val() === '--Select Option--') {
            toastr.error('Plz Select a Party Type');
            ddlpartyType.focus();
            return;
        }
        else if (!partyName.val()) {
            toastr.error("Party Name is required.");
            partyName.focus();
            return;
        }
        else if (!ddlState.val() || ddlState.val() === '--Select Option--') {
            toastr.error('Plz Select State');
            ddlState.focus();
            return;
        }
        else if (!ddlCity.val() || ddlCity.val() === '--Select Option--') {
            toastr.error('Plz Select City');
            ddlCity.focus();
            return;
        }
        else if (!phoneNo.val()) {
            toastr.error("Phone Number is required.");
            phoneNo.focus();
            return;
        }
        else if (!email.val()) {
            toastr.error("email is required.");
            email.focus();
            return;
        }
        else if (!address.val()) {
            toastr.error("Adress is required.");
            address.focus();
            return;
        }
        else if (!gstNo.val()) {
            toastr.error("Gst No is required.");
            gstNo.focus();
            return;
        }
        else {
            $('#loader').show();
            const data = {
                Fk_PartyType: ddlpartyType.val(),
                Fk_StateId: ddlState.val(),
                Fk_CityId: ddlCity.val(),
                PartyName: partyName.val(),
                Address: address.val(),
                Phone: phoneNo.val(),
                Email: email.val(),
                GstNo: gstNo.val(),
            }
            $.ajax({
                type: "POST",
                url: '/Master/CreateParty',
                dataType: 'json',
                data: JSON.stringify(data),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#loader').hide();
                    if (Response.ResponseCode == 201) {
                        toastr.success(Response.SuccessMsg);
                    }
                    else {
                        toastr.error(Response.ErrorMsg);
                    }
                    partyName.val('');
                    address.val('');
                    phoneNo.val('');
                    email.val('');
                    gstNo.val('');
                },
                error: function (error) {
                    console.log(error);
                    $('#loader').hide();
                }
            });
        }

    }
    $(document).on('click', '.btn-party-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        EditParty(value);
    });
    function EditParty(Id) {
        var $tr = $('#btnPartyEdit_' + Id + '').closest('tr');
        var PartyName = $tr.find('td:eq(1)').text().trim();
        var PartyType = $tr.find('td:eq(2)').text().trim();
        var State = $tr.find('td:eq(3)').text().trim();
        var City = $tr.find('td:eq(4)').text().trim();
        var PhoneNo = $tr.find('td:eq(5)').text().trim();
        var Email = $tr.find('td:eq(6)').text().trim();
        var Address = $tr.find('td:eq(7)').text().trim();
        var GSTNo = $tr.find('td:eq(8)').text().trim();
        //fill Modal
        $('input[name="mdlPartyId"]').val(Id);
        $('input[name="mdlPartyName"]').val(PartyName);
        $('input[name="mdlPhoneNo"]').val(PhoneNo);
        $('input[name="mdlMail"]').val(Email);
        $('input[name="mdlAddress"]').val(Address);
        $('input[name="mdlGstNo"]').val(GSTNo);
        $.ajax({
            url: "/Master/GetPartyTypes",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                var ddlPartyType = $('select[name="mdlddnPartyTypeId"]');
                ddlPartyType.empty();
                var defaultOption = $('<option></option>').val('').text('--Select Option--');
                ddlPartyType.append(defaultOption);
                $.each(result, function (key, item) {
                    var option = $('<option></option>').val(item.LedgerId).text(item.LedgerName);
                    if (item.LedgerName === PartyType) {
                        option.attr('selected', 'selected');
                    }
                    ddlPartyType.append(option);
                });
                let selectedState = '';
                $.ajax({
                    url: "/Master/GetStates",
                    type: "GET",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        var ddlState = $('select[name="mdlddnStateId"]');
                        ddlState.empty();
                        var defaultOption = $('<option></option>').val('').text('--Select Option--');
                        ddlState.append(defaultOption);
                        if (result.ResponseCode == 302) {
                            $.each(result.States, function (key, item) {
                                var option = $('<option></option>').val(item.StateId).text(item.StateName);
                                if (item.StateName === State) {
                                    option.attr('selected', 'selected');
                                    selectedState = item.StateId;
                                }
                                ddlState.append(option);
                            });
                            loadCities(selectedState);
                        }
                    },
                    error: function (errormessage) {
                        console.log(errormessage)
                    }
                });
                $.ajax({
                    url: '/Master/GetCities?id=' + selectedState + '',
                    type: "GET",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        var ddlCity = $('select[name="mdlddnCityId"]');
                        ddlCity.empty();
                        var defaultOption = $('<option></option>').val('').text('--Select Option--');
                        ddlCity.append(defaultOption);
                        if (result.ResponseCode == 302) {
                            $.each(result.Cities, function (key, item) {
                                var option = $('<option></option>').val(item.CityId).text(item.CityName);
                                if (item.CityName === City) {
                                    option.attr('selected', 'selected');
                                }
                                ddlCity.append(option);
                            });
                        }
                    },
                    error: function (errormessage) {
                        console.log(errormessage)
                    }
                });
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    $('.btn-party-update').on('click', UpdateParty);
    function UpdateParty() {
        const data = {
            PartyId: $('input[name="mdlPartyId"]').val(),
            Fk_PartyType: $('select[name="mdlddnPartyTypeId"]').val(),
            Fk_StateId: $('select[name="mdlddnStateId"]').val(),
            Fk_CityId: $('select[name="mdlddnCityId"]').val(),
            PartyName: $('input[name="mdlPartyName"]').val(),
            Address: $('input[name="mdlAddress"]').val(),
            Phone: $('input[name="mdlPhoneNo"]').val(),
            Email: $('input[name="mdlMail"]').val(),
            GstNo: $('input[name="mdlGstNo"]').val(),
        }
        $.ajax({
            type: "POST",
            url: '/Master/UpdateParty',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                $('#modal-party-edit').modal('hide');
                if (Response.ResponseCode == 200) {
                    toastr.success(Response.SuccessMsg);
                }
                else {
                    toastr.error(Response.ErrorMsg);
                }
                GetParties();
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
    $(document).on('click', '.btn-party-delete', (event) => {
        const value = $(event.currentTarget).data('id');
        DeleteParty(value);
    });
    function DeleteParty(Id) {
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
                    url: '/Master/DeleteParty?id=' + Id + '',
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
                        GetParties();
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
            }
        });
    }
    /**************************************State*************************************/
    LoadState();
    function LoadState() {
        ddlState.empty();
        var defaultOption = $('<option></option>').val('').text('--Select Option--');
        ddlState.append(defaultOption);
        $.ajax({
            url: "/Master/GetStates",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    $.each(result.States, function (key, item) {
                        var option = $('<option></option>').val(item.StateId).text(item.StateName);
                        ddlState.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    $('#btnStateAdd').on('click', function () {
        $('#modal-add-state').modal('show');
    });
    $('.StateAdd').on('click', StateAdd);
    function StateAdd() {
        const data = {
            StateName: $('input[name="mdlStateAdd"]').val(),
        }
        $.ajax({
            type: "POST",
            url: '/Master/CreateState',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                $('#modal-add-state').modal('hide');
                if (Response.ResponseCode == 201) {
                    toastr.success(Response.SuccessMsg);
                    $('input[name="mdlStateAdd"]').val('');
                }
                else {
                    toastr.error(Response.ErrorMsg);
                }
                LoadState();
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
    $("#btnStateEdit").on('click', StateEdit);
    function StateEdit() {
        if (!ddlState.val() || ddlState.val() === '--Select Option--') {
            toastr.error('Plz Select a State To Edit');
            return;
        }
        else {
            $('#modal-edit-state').modal('show');
            const selectedOption = ddlState.find('option:selected').not(':contains("--Select Option--")');
            var text = selectedOption.text();
            var value = selectedOption.val();
            $("input[name='mdlStateEdit']").val(text);
            $("input[name='mdlStateId']").val(value);
        }
    }
    $('.StateUpdate').on('click', StateUpdate);
    function StateUpdate() {
        const data = {
            StateId: $('input[name="mdlStateId"]').val(),
            StateName: $('input[name="mdlStateEdit"]').val(),
        }
        $.ajax({
            type: "POST",
            url: '/Master/UpdateState',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                $('#modal-edit-state').modal('hide');
                if (Response.ResponseCode == 200) {
                    toastr.success(Response.SuccessMsg);
                    $('input[name="mdlStateId"]').val('');
                    $('input[name="mdlStateEdit"]').val('');
                    LoadState();
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
    $('#btnStateDelete').on('click', StateDelete);
    function StateDelete() {
        if (!ddlState.val() || ddlState.val() === '--Select Option--') {
            toastr.error('Plz Select a State To Delete');
            return;
        }
        const Id = ddlState.find('option:selected').val();
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
                    url: '/Master/DeleteState?id=' + Id + '',
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        if (result.ResponseCode == 200) {
                            toastr.success(result.SuccessMsg);
                            LoadState();
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
    /**************************************City*************************************/
    ddlState.on("change", function () {
        ddlCity.prop("disabled", false);
        ddlCity.empty();
        var selectedState = $(this).val();
        LoadCity(selectedState);
    });
    function LoadCity(id) {
        ddlCity.empty();
        var defaultOption = $('<option></option>').val('').text('--Select Option--');
        ddlCity.append(defaultOption);
        $.ajax({
            url: '/Master/GetCities?id=' + id + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    $.each(result.Cities, function (key, item) {
                        var option = $('<option></option>').val(item.CityId).text(item.CityName);
                        ddlCity.append(option);
                    });
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    $("#btnCityAdd").on('click', function () {
        if (!ddlState.val() || ddlState.val() === '--Select Option--') {
            toastr.error('Plz Select a State For Which You Add A City');
            return;
        }
        else {
            $('#modal-add-city').modal('show'); 
        }
    });
    $('.CityeAdd').on('click', CreateCity);
    function CreateCity() {
        if (!ddlState.val() || ddlState.val() === '--Select Option--') {
            toastr.error('Plz Select a State');
            return;
        }
        else {
            const data = {
                Fk_StateId: ddlState.val(),
                CityName: $('input[name="mdlCityAdd"]').val(),
            }
            $.ajax({
                type: "POST",
                url: '/Master/CreateCity',
                dataType: 'json',
                data: JSON.stringify(data),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    $('#modal-add-city').modal('hide');
                    if (Response.ResponseCode == 201) {
                        toastr.success(Response.SuccessMsg);
                        $('input[name="mdlCityAdd"]').val('');
                    }
                    else {
                        toastr.error(Response.ErrorMsg);
                    }
                    LoadCity(data.Fk_StateId);
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }

    }
    $("#btnCityEdit").on('click', CityEdit);
    function CityEdit() {
        if (!ddlState.val() || ddlState.val() === '--Select Option--') {
            toastr.error('Plz Select a State For Which You Update A City');
            return;
        }
        else if (!ddlCity.val() || ddlCity.val() === '--Select Option--') {
            toastr.error('Plz Select a City To Edit');
            return;
        }
        else {
            $('#modal-edit-city').modal('show');
            const selectedStateOption = ddlState.find('option:selected').not(':contains("--Select Option--")');
            const selectedCityOption = ddlCity.find('option:selected').not(':contains("--Select Option--")');
            var text = selectedCityOption.text();
            var cityvalue = selectedCityOption.val();
            $("input[name='mdlCityEdit']").val(text);
            $("input[name='mdlCityId']").val(value);
            $("input[name='mdlStateId']").val(selectedStateOption.val());
        }
    }
    $('.CityUpdate').on('click', UpdateCity);
    function UpdateCity() {
        const data = {
            CityId: $('input[name="mdlCityId"]').val(),
            Fk_StateId: $('input[name="mdlStateId"]').val(),
            CityName: $('input[name="mdlCityEdit"]').val(),
        }
        $.ajax({
            type: "POST",
            url: '/Master/UpdateCity',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (Response) {
                $('#modal-edit-city').modal('hide');
                if (Response.ResponseCode == 200) {
                    toastr.success(Response.SuccessMsg);
                    $('input[name="mdlCityId"]').val('');
                    $('input[name="mdlStateId"]').val('');
                    $('input[name="mdlCityEdit"]').val('');
                }
                else {
                    toastr.error(Response.ErrorMsg);
                }
                LoadCity(data.Fk_StateId);
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
    $('#btnCityDelete').on('click', CityDelete);
    function CityDelete() {
        if (!ddlState.val() || ddlState.val() === '--Select Option--') {
            toastr.error('Plz Select a State For City Which You Want To Delete');
            return;
        }
        else if (!ddlCity.val() || ddlCity.val() === '--Select Option--') {
            toastr.error('Plz Select a City To Delete');
            return;
        }
        else {
            const StateId = ddlState.find('option:selected').val()
            const Id = ddlCity.find('option:selected').val();
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
                        url: '/Master/DeleteCity?id=' + Id + '',
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
                            const StateId = ddlState.find('option:selected').val();
                            LoadCity(StateId);
                        },
                        error: function (error) {
                            console.log(error);
                        }

                    });
                }
            });
        }
    }

});