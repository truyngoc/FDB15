﻿jQuery(function ($) {   
    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy', changeYear: true });    
});


/*
    datepicker cua Jquery voi cau hinh globalize = 'en-AU' dd/MM/yyyy lam viec tot tren IE
    nhung tren Chrome thi ko dc.
    -> Them doan duoi de cho no lam viec voi ca 2 loai trinh duyet
*/
jQuery.validator.methods["date"] = function (value, element) { return true; }




































//$(function () {
//    //$.validator.addMethod('date',
//    //function (value, element) {
//    //    if (this.optional(element)) {
//    //        return true;
//    //    }
//    //    var ok = true;
//    //    try {
//    //        $.datepicker.parseDate('dd/mm/yy', value);
//    //    }
//    //    catch (err) {
//    //        ok = false;
//    //    }
//    //    return ok;
//    //});

//    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy', changeYear: true });
//});

//$(function () {
   

//    $('.datepicker').datepicker({
//        format: 'dd/mm/yyyy'
//    }); //Initialise any date pickers

//});


//$(function () {
//    $.validator.addMethod('date',
//    function (value, element) {
//        if (this.optional(element)) {
//            return true;
//        }
//        var ok = true;
//        try {
//            $.datepicker.parseDate('dd/mm/yyyy', value);
//        }
//        catch (err) {
//            ok = false;
//        }
//        return ok;
//    });
//    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yyyy', changeYear: true });
//});