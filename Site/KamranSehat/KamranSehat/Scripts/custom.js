function InsertUser() {

    $('#btnCreateOtp').css('display', 'none');
    $('#loading-img').css('display', 'block');

    var fullnameVal = $("#txtFullname").val();
    var mobileVal = $("#txtMobile").val();

    if (fullnameVal !== "" && mobileVal !== "" ) {
        $.ajax(
            {
                url: "/cart/InsertUser",
                data: { fullname: fullnameVal, mobile: mobileVal},
                type: "GET"
            }).done(function (result) {
                if (result === "true") {
                    $('#Register').removeClass('active');
                    $('#activate').css('display','block');
                }
                
                else if (result === "InvalidMobile") {
                    $('#errorDivC').css('display', 'block');
                    $('#SuccessDivC').css('display', 'none');
                    $('#errorDivC').html('شماموبایل وارد شده صحیح نمی باشد.');
                }
            });

    }
    else {
        $('#SuccessDivC').css('display', 'none');
        $('#errorDivC').html('موارد درخواستی را تکمیل نمایید.');
        $('#errorDivC').css('display', 'block');
    }


    $('#btnCreateOtp').css('display', 'block');
    $('#loading-img').css('display', 'none');
}

function changeMobile() {
    $('#Register').addClass('active');
    $('#activate').css('display', 'none');
}

function InsertOrder() {
    var activationVal = $("#txtactivatecode").val();
    var mobileVal = $("#txtMobile").val();

    if (activationVal !== "") {
        $.ajax(
            {
                url: "/cart/InsertOrder",
                data: { activationCode: activationVal, mobile: mobileVal},
                type: "GET"
            }).done(function(result) {
            if (result.includes("paymentrequest")) {
                window.location.replace(result);
                //$('#errorDivC').css('display', 'none');
                //$('#SuccessDivC').css('display', 'block');
            } else if (result === "empty") {
                $('#errorDivC').css('display', 'block');
                $('#SuccessDivC').css('display', 'none');
                $('#errorDivC').html('سبد خرید شما خالی است.');
                }
            else if (result === "invalidCode") {
                $('#errorDivC').css('display', 'block');
                $('#SuccessDivC').css('display', 'none');
                $('#errorDivC').html('کد وارد شده صحیح نمی باشد. لطفا مجدد تلاش کنید.');
            }
            else if (result === "error") {
                $('#errorDivC').css('display', 'block');
                $('#SuccessDivC').css('display', 'none');
                $('#errorDivC').html('خطایی در فرایند ثبت سفارش به وجود امده است. لطفا مجددا وارد وب سایت شده و خرید را انجام دهید.');
            }
        });

    } else {
        $('#errorDivC').css('display', 'block');
        $('#SuccessDivC').css('display', 'none');
        $('#errorDivC').html('کد فعال سازی را وارد کنید.');
    }
}

function removeFromBasket(value) {
    var current = getCookie('basket');

    if (current.includes(value + '/'))
        value = value + '/';

   else if (current.includes('/' + value))
        value = '/' + value;


    if (current.includes(value))
        current = current.replace(value, "");

    var newValue = current;

    setCookie('basket', newValue, '1');
    location.reload();
}

function setCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
}
function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) === 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function CheckLoginInfo() {
    var mobileVal = $("#txtLoginMobile").val();
    var passwordVal = $("#txtLoginPass").val();
    if (mobileVal != "", passwordVal != "") {
        $.ajax(
            {
                url: "/cart/CheckLogin",
                data: {  mobile: mobileVal, password: passwordVal },
                type: "GET"
            }).done(function (result) {
                if (result === "true") {
                    InsertOrder();
                    //$('#errorDivC').css('display', 'none');
                    //$('#SuccessDivC').css('display', 'block');
                }
                else if (result === "false") {
                    $('#errorDivC').css('display', 'block');
                    $('#SuccessDivC').css('display', 'none');
                    $('#errorDivC').html('نام کاربری یا کلمه عبور صحیح نمی باشد.');
                }
               
            });

    }
    else {
        $('#SuccessDivC').css('display', 'none');
        $('#errorDivC').html('موارد درخواستی را تکمیل نمایید.');
        $('#errorDivC').css('display', 'block');
    }
}

function FirstOrder()
{
    InsertUser(); 
}

function UserOrder() {
    CheckLoginInfo(); 
}
 