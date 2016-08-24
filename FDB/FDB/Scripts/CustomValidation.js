var curVal;

$.validator.addMethod("min", function (value, element, params) {
    // them doan nay thi ajax moi co tac dung => "this.optional(element) ||"
    // all this.optional does is say "if the field is optional, return true if it is blank"
    //return this.optional(element) || (parseInt(value) < parseInt($(params).val()));

    // value : gia tri nhap vao
    // params. : gia tri cua bien truyen vao attribute
    return this.optional(element) || (parseInt(value) >= parseInt(curVal));
});

$.validator.unobtrusive.adapters.add("min", ["minvalue"], function (options) {
    // lay gia tri hien thoi
    curVal = options.params.minvalue;

    var params = {
        minvalue: options.params.minvalue
    };
    options.rules['min'] = params;
    options.messages['min'] = options.message;

   
    // cai nay dung de lay gia tri property so sanh thuoc tinh
    //options.rules["min"] = "#" + options.params.minvalue;
    //options.messages["min"] = options.message;
});



// DATE GREATER THAN
// Value is the element to be validated, params is the array of name/value pairs of the parameters extracted from the HTML, element is the HTML element that the validator is attached to
$.validator.addMethod("dategreaterthan", function (value, element, params) {
    // mac dinh Date.parse se dinh dang ngay thang theo RFC2822 or ISO 8601 date
    // -> phai dung moment.js de convert date
    return this.optional(element) || moment(value,'DD/MM/YYYY') > moment($(params).val(),'DD/MM/YYYY');
});

$.validator.unobtrusive.adapters.add("dategreaterthan", ["otherpropertyname"], function (options) {
    options.rules["dategreaterthan"] = "#" + options.params.otherpropertyname;
    options.messages["dategreaterthan"] = options.message;
});

// DATE GREATER THAN OR EQUAL TO
// Value is the element to be validated, params is the array of name/value pairs of the parameters extracted from the HTML, element is the HTML element that the validator is attached to
$.validator.addMethod("dategreaterthanorequalto", function (value, element, params) {
    // mac dinh Date.parse se dinh dang ngay thang theo RFC2822 or ISO 8601 date
    // -> phai dung moment.js de convert date

    return this.optional(element) || moment(value, 'DD/MM/YYYY') >= moment($(params).val(), 'DD/MM/YYYY');
});

$.validator.unobtrusive.adapters.add("dategreaterthanorequalto", ["otherpropertyname"], function (options) {
    options.rules["dategreaterthanorequalto"] = "#" + options.params.otherpropertyname;
    options.messages["dategreaterthanorequalto"] = options.message;
});


// DATE LESS THAN OR EQUAL TO
// Value is the element to be validated, params is the array of name/value pairs of the parameters extracted from the HTML, element is the HTML element that the validator is attached to
$.validator.addMethod("datelessthanorequalto", function (value, element, params) {
    // mac dinh Date.parse se dinh dang ngay thang theo RFC2822 or ISO 8601 date
    // -> phai dung moment.js de convert date
    return this.optional(element) || moment(value, 'DD/MM/YYYY') <= moment($(params).val(), 'DD/MM/YYYY');
});

$.validator.unobtrusive.adapters.add("datelessthanorequalto", ["otherpropertyname"], function (options) {
    options.rules["datelessthanorequalto"] = "#" + options.params.otherpropertyname;
    options.messages["datelessthanorequalto"] = options.message;
});