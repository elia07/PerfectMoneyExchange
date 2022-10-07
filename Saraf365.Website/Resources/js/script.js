/*----------------------------
 Loading Document
------------------------------*/
$(document).ready(function () {
    try {
        Themes();

       // $(".paymentType:first-child").click();

    } catch (e) {

    }

});

/*----------------------------
 All Require Function
------------------------------*/
function Themes() {
    // Tooltips
    $('[data-toggle="tooltip"]').tooltip();

    // Popover
    $('[data-toggle="popover"]').popover();


}

function OpenSideNavMenu() {
    document.getElementById("SideNavMenu").style.width = "250px";
}

function CloseSideNavMenu() {
    document.getElementById("SideNavMenu").style.width = "0";
}

function OpenSideNavHeros() {
    document.getElementById("SideNavHeros").style.width = "250px";
}

function CloseSideNavHeros() {
 
}

/*----------------------------
 Toggle View For List and Grid
------------------------------*/
function toggle_view_list() {
    $('#view-list').addClass('active');
    $('#view-grid').removeClass('active');
    $('#view-item').removeClass('view-grid');
    $('#view-item').addClass('view-list');
}

function toggle_view_grid() {
    $('#view-grid').addClass('active');
    $('#view-list').removeClass('active');
    $('#view-item').removeClass('view-list');
    $('#view-item').addClass('view-grid');
}

function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(function () {
    }, function (err) {
    });
}
$("#mainMenu ul li a").click(function () {


    var notifs = $(this).children("span:first");
    if (notifs != undefined) {
        notifs.remove();
    }
    $("#selectedSectionContainer").html('<p style="text-align:center;width:100%"><i class="mdi mdi-48px mdi-spin mdi-loading"></i></p>');
    $("#selectedMenu").html($(this).html());
    $("#mainMenu ul li a").removeClass("active");
    $(this).addClass("active");
    $.ajax({
        headers: { 'RequestVerificationToken': window.TokenHeaderValue },
        url: "/Profile/GetPartialView?viewName=" + $(this).attr('viewName'), type: "GET",
        error: function (request, status, error) {
            unhanddleError();
        },
        success: function (e) {
            if (e.CustomFields != undefined && e.CustomFields.Redirect != undefined) {
                window.location.href = e.CustomFields.Redirect;
                return;
            }
            $("#selectedSectionContainer").html(unescapeHtml(e));
            $('.main-menu').toggleClass('active');
        }
    });
    
});

function unescapeHtml(safe) {
    return safe
        .replace(/&amp;/g, "&")
        .replace(/&lt;/g, "<")
        .replace(/&gt;/g, ">")
        .replace(/&quot;/g, "\"")
        .replace(/&#039;/g, "'");
}

function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(function () {
    }, function (err) {
    });
}



$("#ForgetPassword").submit(function (event) {
    event.preventDefault();
    if (!$("#ForgetPassword").parsley().validate()) {
        return;
    }
    $.ajax({
        headers: { 'RequestVerificationToken': window.TokenHeaderValue },
        url: "/Home/RecoverPassword", data: $(this).serialize(), type: "POST", dataType: 'json',
        error: function (request, status, error) {
            unhanddleError();
        },
        success: function (e) {
            if (e.CustomFields != undefined && e.CustomFields.Redirect != undefined) {
                window.location.href = e.CustomFields.Redirect;
                return;
            }
            if (!e.Status) {
                $("#ForgetPasswordCaptcha_ReloadIcon").click();
                Swal({
                    title: e.CustomFields.title,
                    text: e.Message,
                    type: 'error',
                    confirmButtonText: e.CustomFields.confirmButtonText
                });
            }
            else {
                Swal({
                    title: e.CustomFields.title,
                    text: e.Message,
                    type: 'success',
                    timer: 2000,
                    showConfirmButton: false,
                    onClose: function () {
                        window.location.reload();
                    }
                });

            }
        }
    });

});



$("#SendForgetPasswordEmail").submit(function (event) {
    event.preventDefault();
    if (!$("#SendForgetPasswordEmail").parsley().validate()) {
        return;
    }
    $.ajax({
        headers: { 'RequestVerificationToken': window.TokenHeaderValue },
        url: "/Home/SendForgetPasswordEmail", data: $(this).serialize(), type: "POST", dataType: 'json',
        error: function (request, status, error) {
            unhanddleError();
        },
        success: function (e) {
            if (e.CustomFields != undefined && e.CustomFields.Redirect != undefined) {
                window.location.href = e.CustomFields.Redirect;
                return;
            }
            if (!e.Status) {
                $("#SendForgetPasswordEmailCaptcha_ReloadIcon").click();
                Swal({
                    title: e.CustomFields.title,
                    text: e.Message,
                    type: 'error',
                    confirmButtonText: e.CustomFields.confirmButtonText
                });
            }
            else {
                Swal({
                    title: e.CustomFields.title,
                    text: e.Message,
                    type: 'success',
                    timer: 2000,
                    showConfirmButton: false,
                    onClose: function () {
                        $('#SendForgetPasswordEmailModal').modal('hide'); $('#ForgetPasswordModal').modal('show');
                    }
                });

            }
        }
    });

});



$("#Signup").submit(function (event) {
    event.preventDefault();
    if (!$("#Signup").parsley().validate()) {
        return;
    }
    $.ajax({
        headers: { 'RequestVerificationToken': window.TokenHeaderValue },
        url: "/Home/Register", data: $(this).serialize(), type: "POST", dataType: 'json',
        error: function (request, status, error) {
            unhanddleError();
        },
        success: function (e) {
            if (e.CustomFields != undefined && e.CustomFields.Redirect != undefined) {
                window.location.href = e.CustomFields.Redirect;
                return;
            }
            if (!e.Status) {
                $("#CaptchaRegister_ReloadIcon").click();
                Swal({
                    title: e.CustomFields.title,
                    text: e.Message,
                    type: 'error',
                    confirmButtonText: e.CustomFields.confirmButtonText
                });
            }
            else {
                Swal({
                    title: e.CustomFields.title,
                    text: e.Message,
                    type: 'success',
                    timer: 2000,
                    showConfirmButton: false,
                    onClose: function () {
                        $("#RegisterModal").modal("hide");
                        $("#LoginModal").modal("show");
                    }
                });
              
            }
        }
    });

});

$("#Login").submit(function (event) {
    event.preventDefault();
    if (!$("#Login").parsley().validate()) {
        return;
    }
    $.ajax({
        headers: { 'RequestVerificationToken': window.TokenHeaderValue },
        url: "/Home/Login", data: $(this).serialize(), type: "POST", dataType: 'json',
        error: function (request, status, error) {
            unhanddleError();
        },
        success: function (e) {
            if (e.CustomFields != undefined && e.CustomFields.Redirect != undefined) {
                window.location.href = e.CustomFields.Redirect;
                return;
            }
            if (!e.Status) {
                $("#CaptchaLogin_ReloadIcon").click();
                Swal({
                    title: e.CustomFields.title,
                    text: e.Message,
                    type: 'error',
                    confirmButtonText: e.CustomFields.confirmButtonText
                });
            }
            else {
                if (e.CustomFields != undefined && e.CustomFields.unfilterDomainForThisuser != undefined) {
                    Swal({
                        title: e.CustomFields.title,
                        text: e.Message + "\r\n" + e.CustomFields.unfilterDomainForThisuser,
                        type: 'success',
                        showConfirmButton: true,
                        onClose: function () { window.location.href = window.ProfileAddress; }
                    });
                }
                else {
                    Swal({
                        title: e.CustomFields.title,
                        text: e.Message,
                        type: 'success',
                        timer: 2000,
                        showConfirmButton: false,
                        onClose: function () { window.location.href = window.ProfileAddress; }
                    });
                }
                
                
            }
        }
    });

});


Highcharts.setOptions({
    chart: {
        style: {
            fontFamily: 'IRANSans, Tahoma, aria, sans-serif'
        },
        backgroundColor: {
            linearGradient: [0, 0, 500, 500],
            stops: [
                [0, 'rgb(255, 255, 255)'],
                [1, 'rgb(240, 240, 255)']
            ]
        },
        borderWidth: 2,
        borderColor:'#410e10',
        plotBackgroundColor: 'rgba(255, 255, 255, .9)',
        plotShadow: true,
        plotBorderWidth: 1
    }
});


function PaymentRequest() {
    if ($("#paymentTypeContainer_" + window.selectedGateway).attr("paymentType") == "0") {
        if ($("#value_" + window.selectedGateway).parsley().validate() !== true) {
            return;
        }
    }
    else if ($("#paymentTypeContainer_" + window.selectedGateway).attr("paymentType") == "1") {
        if ($("#value_" + window.selectedGateway).parsley().validate() !== true) {
            return;
        }
        if ($("#payment_ToCartNumber").parsley().validate() !== true) {
            return;
        }
    }
    
    var args = {
        "amount": $("#value_" + window.selectedGateway).val(),
        "gateWayID": window.selectedGateway,
        "cardNumber": $("#payment_CartNumber").val(),
        "toCardNumber": $("#payment_ToCartNumber").val(),
        "voucherCode": $("#payment_VoucherCode").val(),
        "voucherActivationCode": $("#payment_VoucherActivationCode").val()
        

    }
    if (args.amount == undefined) {
        args.amount = 0;
    }


    $.ajax({
        headers: { 'RequestVerificationToken': window.TokenHeaderValue },
        url: "/Profile/PaymentRequest", data: JSON.stringify(args), type: "POST", dataType: 'json', contentType: "application/json; charset=utf-8",
        error: function (request, status, error) {
            unhanddleError();
        },
        success: function (e) {
            if (e.CustomFields != undefined && e.CustomFields.Redirect != undefined) {
                window.location.href = e.CustomFields.Redirect;
                return;
            }
            if (!e.Status) {
                Swal({
                    title: e.CustomFields.title,
                    text: e.Message,
                    type: 'error',
                    confirmButtonText: e.CustomFields.confirmButtonText
                });
            }
            else {
                if (e.CustomFields != undefined && e.CustomFields.Redirect != undefined) {
                    window.location.href = e.CustomFields.Redirect;
                }
                else {
                    if (e.CustomFields != undefined && e.CustomFields.Voucher != undefined) {
                        Swal({
                            title: e.CustomFields.title,
                            text: e.Message + " \r\n" + e.CustomFields.Voucher,
                            type: 'success',
                            onClose: function () {
                                window.location.reload();
                            }
                        });
                    }
                    else {
                        Swal({
                            title: e.CustomFields.title,
                            text: e.Message,
                            type: 'success',
                            onClose: function () {
                                window.location.reload();
                            }
                        });
                    }
                    
                }
                

            }
        }
    });
    
}

$(".paymentType").click(function () {
    $(".paymentType").removeClass("selected");
    $(this).addClass("selected");
    $(".paymentTypeContainer").css("display", "none");
    $("#" + $(this).attr("containerID")).css("display", "block");
    window.selectedGateway = $(this).attr("containerID").replace("paymentTypeContainer_","");
});

$(".paymentValue").click(function () {
    window.buyInValue = $(this).attr("value");
    $("#value_" + window.selectedGateway).val($(this).attr("value"));
    $(".paymentValue").removeClass("selected");
    $(this).addClass("selected");
    
});



function activationEmailResendHandler() {

    var i = parseInt($("#emailActivationResendIn").text().replace("(", "").replace(")", "")) - 1;
    if (i > 0) {
        $("#emailActivationResendIn").text("("+i+")");
        $("#resendEmailActivation").attr("disabled", "true");
    }
    else {
        $("#emailActivationResendIn").text('');
        $("#resendEmailActivation").removeAttr("disabled");
        clearTimeout(window.emailActivationResentTime);
    }
    
}

function SendActivationEmail() {
    $.ajax({
        headers: { 'RequestVerificationToken': window.TokenHeaderValue },
        url: "/Profile/SendActivationEmail", data: $(this).serialize(), type: "POST", dataType: 'json',
        error: function (request, status, error) {
            unhanddleError();
        },
        success: function (e) {
            if (e.CustomFields != undefined && e.CustomFields.Redirect != undefined) {
                window.location.href = e.CustomFields.Redirect;
                return;
            }
            if (!e.Status) {
                if (e.CustomFields.CountDown != undefined) {
                    $("#emailActivationResendIn").text("(" + e.CustomFields.CountDown + ")");
                    window.emailActivationResentTime = setInterval(activationEmailResendHandler, 1000);
                    Swal({
                        title: e.CustomFields.title,
                        text: e.Message,
                        type: 'warning',
                        confirmButtonText: e.CustomFields.confirmButtonText
                    });
                }
                else {
                    Swal({
                        title: e.CustomFields.title,
                        text: e.Message,
                        type: 'error',
                        confirmButtonText: e.CustomFields.confirmButtonText
                    });
                }
                
            }
            else {
                $("#emailActivationResendIn").text("(" + e.CustomFields.CountDown + ")");
                window.emailActivationResentTime = setInterval(activationEmailResendHandler, 1000);
                Swal({
                    title: e.CustomFields.title,
                    text: e.Message,
                    type: 'success',
                    timer: 2000,
                    showConfirmButton: false,
                });

            }
        }
    });
}








function activationCellphoneResendHandler() {

    var i = parseInt($("#cellphoneActivationResendIn").text().replace("(", "").replace(")", "")) - 1;
    if (i > 0) {
        $("#cellphoneActivationResendIn").text("(" + i + ")");
        $("#resendCellphoneActivation").attr("disabled", "true");
    }
    else {
        $("#cellphoneActivationResendIn").text('');
        $("#resendCellphoneActivation").removeAttr("disabled");
        clearTimeout(window.activationCellphoneResendTime);
    }

}

function SendActivationSms() {
    $.ajax({
        headers: { 'RequestVerificationToken': window.TokenHeaderValue },
        url: "/Profile/SendActivationSms", data: $(this).serialize(), type: "POST", dataType: 'json',
        error: function (request, status, error) {
            unhanddleError();
        },
        success: function (e) {
            if (e.CustomFields != undefined && e.CustomFields.Redirect != undefined) {
                window.location.href = e.CustomFields.Redirect;
                return;
            }
            if (!e.Status) {
                if (e.CustomFields.CountDown != undefined) {
                    $("#cellphoneActivationResendIn").text("(" + e.CustomFields.CountDown + ")");
                    window.activationCellphoneResendTime = setInterval(activationCellphoneResendHandler, 1000);
                    Swal({
                        title: e.CustomFields.title,
                        text: e.Message,
                        type: 'warning',
                        confirmButtonText: e.CustomFields.confirmButtonText
                    });
                }
                else {
                    Swal({
                        title: e.CustomFields.title,
                        text: e.Message,
                        type: 'error',
                        confirmButtonText: e.CustomFields.confirmButtonText
                    });
                }

            }
            else {
                $("#cellphoneActivationResendIn").text("(" + e.CustomFields.CountDown + ")");
                window.activationCellphoneResendTime = setInterval(activationCellphoneResendHandler, 1000);
                Swal({
                    title: e.CustomFields.title,
                    text: e.Message,
                    type: 'success',
                    timer: 2000,
                    showConfirmButton: false,
                });

            }
        }
    });
}

function unhanddleError() {
    Swal({
        title: 'Something went wrong !!!',
        text: 'Please try again or contact administration',
        type: 'error',
    });
}

