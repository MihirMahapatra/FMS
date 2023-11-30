$(function () {
    $("#MasterLink").addClass("active");
    $("#PartyMasterLink").addClass("active");
    $("#PartyMasterLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    //***************************************Variable Declaration********************************************//
    const partyTypeId = $('select[name="ddnPartyTypeId"]');
    const partyName = $('input[name="PartyName"]');
    const stateId = $('select[name="ddnStateId"]');
    const cityId = $('select[name="ddnCityId"]');
    const phoneNo = $('input[name="PhoneNo"]');
    const email = $('input[name="Mail"]');
    const address = $('input[name="Address"]');
    const gstNo = $('input[name="GstNo"]');
    const creditLimit = $('input[name="CreditLimit"]');
    const openingBalance = $('input[name="OpeningBalance"]');
    const balanceType = $('select[name="ddnBalanceType"]');
    const ddlPartyType = $('select[name="ddnPartyTypeId"]');
    const ddlState = $('select[name="ddnStateId"]');
    const ddlCity = $('select[name="ddnCityId"]');
     //-----------------------------------Contorl Foucous Of Element    PartyMaster---------------------------//
    partyTypeId.focus();
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
    creditLimit.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    creditLimit.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
    openingBalance.on('focus', function () {
        $(this).css('border-color', 'red');
    });
    openingBalance.on('blur', function () {
        $(this).css('border-color', ''); // Reset background color on blur
    });
   
    $('.btn-primary').on('keydown', function (e) {
        if (e.key === 'Enter' || e.keyCode === 13) {
            $('.btn-primary').click();
        }
    });
    $('.btn-primary').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('.btn-primary').on('blur', function () {
        $(this).css('background-color', '');
    });
    $('.btn - party - create').on('keydown', function (e) {
        if (e.key === 'Enter' || e.keyCode === 13) {
            $('.btn - party - create').click();
        }
    });
    $('.btn - party - create').on('focus', function () {
        $(this).css('background-color', 'black');
    });
    $('.btn - party - create').on('blur', function () {
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
    creditLimit.on("input", function () {
        var inputValue = $(this).val().replace(/[^0-9.]/g, '');
        $(this).val(inputValue);
    });
    openingBalance.on("input", function () {
        var inputValue = $(this).val().replace(/[^0-9.]/g, '');
        $(this).val(inputValue);
    });
    //**************************************Party Type*************************************//
   
    LoadPartyType()
    function LoadPartyType() {
        $.ajax({
            url: "/Master/GetPartyTypes",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",

            success: function (result) {
                ddlPartyType.empty();
                var defaultOption = $('<option></option>').val('').text('--Select Option--');
                ddlPartyType.append(defaultOption);
                $.each(result, function (key, item) {
                    var option = $('<option></option>').val(item.LedgerId).text(item.LedgerName);
                    ddlPartyType.append(option);
                });
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    //**************************************State*************************************//
   
    LoadState()
    function LoadState() {
        $.ajax({
            url: "/Master/GetStates",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddlState.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlState.append(defaultOption);
                    $.each(result.States, function (key, item) {
                        var option = $('<option></option>').val(item.StateId).text(item.StateName);
                        ddlState.append(option);
                    });
                }
                else {
                    ddlState.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlState.append(defaultOption);
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    $('#modal-add-state').on('click', '.StateAdd', (event) => {
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
    });
    $("#btnStateEdit").on('click',function () {
        if (!ddlState.val() || ddlState.val() === '--Select Option--') {
            toastr.error('Plz Select a State To Edit');
            return;
        }
        else {
            const selectedOption = ddlState.find('option:selected');
            var text = selectedOption.text();
            var value = selectedOption.val();
            $("input[name='mdlStateEdit']").val(text);
            $("input[name='mdlStateId']").val(value);
        }
    });
    $('#modal-edit-state').on('click', '.StateEdit', (event) => {
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
    });
    $('#btnStateDelete').on('click', function () {

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
                        }
                        else {
                            toastr.error(result.ErrorMsg);
                        }
                        LoadState();
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
            }
        });
    })
    //**************************************City*************************************//
  
    $("#stateId").on("change", function () {
        $("#cityId").prop("disabled", false);
        $("#cityId").empty();
        var selectedState = $(this).val();
        LoadCity(selectedState);
    });
    function LoadCity(id) {
        $.ajax({
            url: '/Master/GetCities?id=' + id + '',
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    ddlCity.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlCity.append(defaultOption);
                    $.each(result.Cities, function (key, item) {
                        var option = $('<option></option>').val(item.CityId).text(item.CityName);
                        ddlCity.append(option);
                    });
                }
                else {
                    ddlCity.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlCity.append(defaultOption);
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    $("#btnCityAdd").on('click',function () {
        if (!ddlState.val() || ddlState.val() === '--Select Option--') {
            toastr.error('Plz Select a State For Which You Add A City');
            return;
        }
    });
    $('#modal-add-city').on('click', '.CityeAdd', (event) => {
        const selectedOption = ddlState.find('option:selected');
        const data = {
            Fk_StateId: selectedOption.val(),
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
    });
    $("#btnCityEdit").on('click',function () {

        if (!ddlState.val() || ddlState.val() === '--Select Option--') {
            toastr.error('Plz Select a State For Which You Update A City');
            return;
        }
        else if (!ddlCity.val() || ddlCity.val() === '--Select Option--') {
            toastr.error('Plz Select a City To Edit');
            return;
        }
        else {
            const selectedStateOption = ddlState.find('option:selected');
            const selectedCityOption = ddlCity.find('option:selected');
            var text = selectedCityOption.text();
            var value = selectedCityOption.val();
            $("input[name='mdlCityEdit']").val(text);
            $("input[name='mdlCityId']").val(value);
            $("input[name='mdlStateId']").val(selectedStateOption.val());
        }
    });
    $('#modal-edit-city').on('click', '.CityEdit', (event) => {
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
    });
    $('#btnCityDelete').on('click', function () {

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
    })
    //**************************************Party*************************************//

    
    $(document).on('click', '.btn-party-create', CreateParty);
    function CreateParty() {
        if (!partyTypeId.val() || partyTypeId.val() === '--Select Option--') {
            toastr.error('Plz Select a Party Type');
            partyTypeId.focus();
            return;
        }
        else if (!partyName.val()) {
            toastr.error("Party Name is required.");
            partyName.focus();
            return;
        }
        else if (!stateId.val() || stateId.val() === '--Select Option--') {
            toastr.error('Plz Select State');
            stateId.focus();
            return;
        }
        else if (!cityId.val() || cityId.val() === '--Select Option--') {
            toastr.error('Plz Select City');
            //cityId.focus();
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
        else if (!openingBalance.val()) {
            toastr.error("openingBalance is required.");
            openingBalance.focus();
            return;
        }
        else if (!creditLimit.val()) {
            toastr.error("creditLimit is required.");
            creditLimit.focus();
            return;
        }
        else {
            const data = {
                Fk_PartyType: partyTypeId.val(),
                Fk_StateId: stateId.val(),
                Fk_CityId: cityId.val(),
                PartyName: partyName.val(),
                Address: address.val(),
                Phone: phoneNo.val(),
                Email: email.val(),
                GstNo: gstNo.val(),
                CreditLimit: creditLimit.val(),
                OpeningBalance: openingBalance.val(),
                BalanceType: balanceType.val()
            }
        
            $.ajax({
                type: "POST",
                url: '/Master/CreateParty',
                dataType: 'json',
                data: JSON.stringify(data),
                contentType: "application/json;charset=utf-8",
                success: function (Response) {
                    console.log(Response)
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
                }
            });
        }

    }
});