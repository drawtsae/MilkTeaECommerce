﻿$(document).ready(function () {

    $('.js-example-basic-multiple').select2();

    if ($("#check-category").is(':checked')) {
        document.getElementById("select-category").disabled = false;
    }
    if ($("#check-product").is(':checked')) {
        document.getElementById("select-product").disabled = false;
    }
    if ($("#check-delivery").is(':checked')) {
        document.getElementById("select-delivery").disabled = false;
    }

    $('input').on('click', function () {
        if ($("#check-delivery").is(':checked')) {
            document.getElementById("select-delivery").disabled = false;

            //update here
            $("#check-category").prop('checked', !$("#check-delivery").prop('checked'));
            $("#check-product").prop('checked', !$("#check-delivery").prop('checked'));
        }
        else {
            document.getElementById("select-delivery").disabled = true;
        }
    });
    $('input').on('click', function () {
        if ($("#check-category").is(':checked')) {
            document.getElementById("select-category").disabled = false;
            //update here
            $("#check-delivery").prop('checked', !$("#check-category").prop('checked'));
        }
        else {
            document.getElementById("select-category").disabled = true;
        }
    });
    
    $('input').on('click', function () {
        if ($("#check-product").is(':checked')) {
            document.getElementById("select-product").disabled = false;
            //update here
            $("#check-delivery").prop('checked', !$("#check-product").prop('checked'));
        }
        else {
            document.getElementById("select-product").disabled = true;
        }
    });
   
});
function CheckSelect() {
    $("#check-category").is(':checked') = true;
    document.getElementById("select-category").disabled = false;
}

