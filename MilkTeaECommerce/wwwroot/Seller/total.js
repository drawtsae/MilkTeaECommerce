﻿$(document).ready(function () {

    // total - products
    $.ajax({
        method:'GET',
        url: '/seller/home/TotalProducts',
        success: function (data) {
            console.log(data);
            $('#total-products').text(data);
        }
    });
    $.ajax({
        method: 'GET',
        url: '/seller/home/Earnings',
        success: function (data) {
            console.log(data);
            $('#total-price').text(data);
        }
    });

    $.ajax({
        method: 'GET',
        url: '/seller/home/TotalCustomer',
        success: function (data) {
            console.log(data);
            $('#total-customer').text(data);
        }
    });
});