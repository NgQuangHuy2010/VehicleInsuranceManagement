// site.js
document.addEventListener('DOMContentLoaded', function () {
    const pwd = document.getElementById("password");
    const chk = document.getElementById("showPassword");
    chk.onchange = function (e) {
        pwd.type = chk.checked ? "text" : "password";
    };
});


document.addEventListener('DOMContentLoaded', function () {
    const confirm = document.getElementById("confirm");
    const showconfirm = document.getElementById("showConfirm");
    showconfirm.onchange = function (e) {
        confirm.type = showconfirm.checked ? "text" : "password";
    };
});