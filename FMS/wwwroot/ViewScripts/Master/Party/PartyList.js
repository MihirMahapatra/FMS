$(function () {
    GetParties();
    function GetParties() {
        $('#loader').show();
        $('.PartyTable').empty();
        $.ajax({
            url: "/Master/GetParties",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#loader').hide();
                var html = '';
                html += '<table class="table table-bordered table-hover text-center mt-1 PartyTable" style="width:100%">';
                html += '<thead>'
                html += '<tr>'
                html += '<th></th>'
                html += '<th hidden>Party Id</th>'
                html += '<th>Party Name</th>'
                html += '<th>Party Type</th>'
                html += '<th hidden>State</th>'
                html += '<th hidden>City</th>'
                html += '<th hidden>Phone No</th>'
                html += '<th hidden>Email</th>'
                html += '<th hidden>Address</th>'
                html += '<th>GST No</th>'
                html += '<th>Credit Limit</th>'
                html += '<th>Op Bal</th>'
                html += '<th>Op Bal.Type</th>'
                html += '<th>Running Bal</th>'
                html += '<th>Running Bal.Type</th>'
                html += '<th>Action</th>'
                html += '</tr>'
                html += '</thead>'
                html += '<tbody>';
                if (result.ResponseCode == 302) {
                    $.each(result.Parties, function (key, item) {
                        html += '<tr>';
                        html += '<td><button  class="btn btn-primary btn-sm toggleColumnsBtn" id="btn-info-' + item.PartyId + '"  data-id="' + item.PartyId + '" style=" border-radius: 50%;" ><i class="fa-solid fa-circle-info"></i></button></td>'
                        html += '<td hidden>' + item.PartyId + '</td>';
                        html += '<td>' + item.PartyName + '</td>';
                        if (item.Ledger !== null) {
                            html += '<td>' + item.Ledger.LedgerName + '</td>';
                        }
                        else {
                            html += '<td> - </td>';
                        }
                        html += '<td hidden>' + item.State.StateName + '</td>';
                        html += '<td hidden>' + item.City.CityName + '</td>';
                        html += '<td hidden>' + item.Phone + '</td>';
                        html += '<td hidden>' + item.Email + '</td>';
                        html += '<td hidden>' + item.Address + '</td>';
                        html += '<td>' + item.GstNo + '</td>';
                        html += '<td>' + item.CreditLimit + '</td>';
                        if (item.SubLedgerBalance !== null) {
                            html += '<td>' + Math.abs(item.SubLedgerBalance.OpeningBalance) + '</td>';
                            html += '<td>' + item.SubLedgerBalance.OpeningBalanceType + '</td>';
                            if (item.SubLedgerBalance.RunningBalance > item.CreditLimit) {
                                html += '<td class="bg-danger text-white">' + Math.abs(item.SubLedgerBalance.RunningBalance) + '</td>';
                            }
                            else {
                                html += '<td>' + Math.abs(item.SubLedgerBalance.RunningBalance) + '</td>';
                            }
                            html += '<td>' + item.SubLedgerBalance.RunningBalanceType + '</td>';
                        }
                        else {
                            html += '<td> - </td>';
                            html += '<td> - </td>';
                            html += '<td> - </td>';
                            html += '<td> - </td>';
                        }
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
    $(document).on('mouseenter', '.toggleColumnsBtn', (event) => {
        const value = $(event.currentTarget).data('id');
        var $tr = $('#btn-info-' + value + '').closest('tr');
        var StateName = $tr.find('td:eq(4)').text().trim();
        var CityName = $tr.find('td:eq(5)').text().trim();
        var Phone = $tr.find('td:eq(6)').text().trim();
        var Email = $tr.find('td:eq(7)').text().trim();
        var Address = $tr.find('td:eq(8)').text().trim();
        $('#lblStateName').text(StateName);
        $('#lblCityName').text(CityName);
        $('#lblPhone').text(Phone);
        $('#lblEmail').text(Email);
        $('#lblAddress').text(Address);
        $('#modal-party-info').modal('show');
    });
    $(document).on('mouseleave', '.toggleColumnsBtn', (event) => {
        $('#modal-party-info').modal('hide');
    });
    $(document).on('click', '.btn-party-edit', (event) => {
        const value = $(event.currentTarget).data('id');
        EditParty(value);
    });
    function EditParty(Id) {
        var $tr = $('#btnPartyEdit_' + Id + '').closest('tr');
        var PartyName = $tr.find('td:eq(2)').text().trim();
        var PartyType = $tr.find('td:eq(3)').text().trim();
        var State = $tr.find('td:eq(4)').text().trim();
        var City = $tr.find('td:eq(5)').text().trim();
        var PhoneNo = $tr.find('td:eq(6)').text().trim();
        var Email = $tr.find('td:eq(7)').text().trim();
        var Address = $tr.find('td:eq(8)').text().trim();
        var GSTNo = $tr.find('td:eq(9)').text().trim();
        var CreditLimit = $tr.find('td:eq(10)').text().trim();
        //fill Modal
        $('input[name="mdlPartyId"]').val(Id);
        $('select[name="ddnPartyTypeId"]').val(PartyType);
        $('input[name="PartyName"]').val(PartyName);
        $('select[name="ddnStateId"]').val(State);
        $('select[name="ddnCityId"]').val(City);
        $('input[name="PhoneNo"]').val(PhoneNo);
        $('input[name="Mail"]').val(Email);
        $('input[name="Address"]').val(Address);
        $('input[name="GstNo"]').val(GSTNo);
        $('input[name="CreditLimit"]').val(CreditLimit);

        $.ajax({
            url: "/Master/GetPartyTypes",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                console.log(result);
                var ddlPartyType = $('select[name="ddnPartyTypeId"]');
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
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
        let selectedState = '';
        $.ajax({
            url: "/Master/GetStates",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                var ddlState = $('select[name="ddnStateId"]');
                if (result.ResponseCode == 302) {
                    ddlState.empty();
                    var defaultOption = $('<option></option>').val('').text('--Select Option--');
                    ddlState.append(defaultOption);
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
        function loadCities(selectedState) {
            $.ajax({
                url: '/Master/GetCities?id=' + selectedState + '',
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var ddlCity = $('select[name="ddnCityId"]');
                    if (result.ResponseCode == 302) {
                        ddlCity.empty();
                        var defaultOption = $('<option></option>').val('').text('--Select Option--');
                        ddlCity.append(defaultOption);
                        $.each(result.Cities, function (key, item) {
                            var option = $('<option></option>').val(item.CityId).text(item.CityName);
                            if (item.CityName === City) {
                                option.attr('selected', 'selected');
                            }
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
    }
    $('.btn-party-update').on('click', UpdateParty);
    function UpdateParty() {
        const data = {
            PartyId: $('input[name="mdlPartyId"]').val(),
            Fk_PartyType: $('select[name="ddnPartyTypeId"]').val(),
            Fk_StateId: $('select[name="ddnStateId"]').val(),
            Fk_CityId: $('select[name="ddnCityId"]').val(),
            PartyName: $('input[name="PartyName"]').val(),
            Address: $('input[name="Address"]').val(),
            Phone: $('input[name="PhoneNo"]').val(),
            Email: $('input[name="Mail"]').val(),
            GstNo: $('input[name="GstNo"]').val(),
            CreditLimit: $('input[name="CreditLimit"]').val(),
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
})