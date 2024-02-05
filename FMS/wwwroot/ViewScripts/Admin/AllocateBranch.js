$(function () {
    $("#AdminLink").addClass("active");
    $("#AllocateBranchLink").addClass("active");
    $("#AllocateBranchLink i.far.fa-circle").removeClass("far fa-circle").addClass("far fa-dot-circle");
    /********************************************Declaration Section****************************/
    const userId = $('select[name="UserId"]');
    const branchId = $('select[name="BranchId"]');
    GetAllUserAndBranch();
    function GetAllUserAndBranch() {
        $.ajax({
            url: "/Admin/GetAllUserAndBranch",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.ResponseCode == 302) {
                    userId.empty();
                    branchId.empty();
                    var defaultOptionUser = $('<option></option>').val('').text('--Select Option--');
                    var defaultOptionBranch = $('<option></option>').val('').text('--Select Option--');
                    userId.append(defaultOptionUser);
                    branchId.append(defaultOptionBranch);
                    $.each(result.Branches, function (key, item) {
                        var option = $('<option></option>').val(item.BranchId).text(item.BranchName);
                        branchId.append(option);
                    });
                    $.each(result.Users, function (key, item) {
                        var option = $('<option></option>').val(item.id).text(item.UserName);
                        userId.append(option);
                    });
                    var html = '';
                    $.each(result.UserBranch, function (key, item) {
                        html += '<tr>';
                        html += '<td hidden>' + item.Id + '</td>';
                        html += '<td>' + item.Branch.BranchName + '</td>';
                        html += '<td>' + item.User.UserName + '</td>';
                        html += '<td style="background-color:#ffe6e6;">';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-labourtype-edit"   id="btnLabourTypeEdit_' + item.LabourTypeId + '"     data-id="' + item.LabourTypeId + '"  style="border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-edit"></i></button>';
                        html += '<button class="btn btn-primary btn-link btn-sm btn-labourtype-update" id ="btnLabourTypeUpdate_' + item.LabourTypeId + '" data-id="' + item.LabourTypeId + '" style = "border: 0px;color: #fff; background-color:#337AB7; border-color: #3C8DBC; border-radius: 4px;display:none" > <i class="fa-solid fa-floppy-disk"></i></button >';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-labourtype-cancel" id="btnLabourTypeCancel_' + item.LabourTypeId + '"   data-id="' + item.LabourTypeId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px; display:none"> <i class="fa-solid fa-rectangle-xmark"></i></button>';
                        html += ' <button class="btn btn-primary btn-link btn-sm btn-labourtype-delete" id="btnLabourTypeDelete_' + item.LabourTypeId + '"   data-id="' + item.LabourTypeId + '" style="border: 0px;color: #fff; background-color:#FF0000; border-color: #3C8DBC; border-radius: 4px;"> <i class="fa-solid fa-trash-can"></i></button>';
                        html += '</td>';
                        html += '</tr >';
                    });
                    $('#example1 tbody').html(html);
                    $("#example1").DataTable({
                        "responsive": true, "lengthChange": false, "autoWidth": false,
                    });  
                }
            },
            error: function (errormessage) {
                console.log(errormessage)
            }
        });
    }
    $('#btnBranch').on('click', function (event) {
        if (!userId.val() || userId.val() === '-- Select User --') {
            toastr.error('User is Required.');
            return;
        }
        else if (!branchId.val() || branchId.val() === '-- Select Branch --') {
            toastr.error('Branch Is Required.');
            return;
        }
        else {
            var data = {
                UserId: userId.val(),
                BranchId: branchId.val()
            }
            $.ajax({
                type: "POST",
                url: '/Admin/CreateBranchAlloction',
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
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }
    });

});
