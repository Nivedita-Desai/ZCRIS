// Function that checks whether input text is numeric or not.
function isNumber(evt) {
    
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        //alert('Please Enter Numbers Only');
        return false;

    }
    return true;
}
function isNumberCode(evt) {
    
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    // if (charCode > 31 && (charCode < 48 || charCode > 57 )) {
    //if ((charCode > 31 || charCode < 42) && (charCode > 44 || charCode < 48) || charCode > 57) {
        //alert('Please Enter Numbers Only');
        if ((charCode > 47 && charCode < 58) || charCode==43 ) {
            return true;
        }
        

    //}
    return false;
}
function isNRCNumber(evt) {
    
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 47 || charCode > 57)) {
        //alert('Please Enter Numbers Only');
        return false;

    }
    return true;
}


// Function that checks whether input text is Alphabet or not.
function AllowAlphabet(e) {
    isIE = document.all ? 1 : 0
    keyEntry = !isIE ? e.which : event.keyCode;
    if (((keyEntry >= '65') && (keyEntry <= '90')) || ((keyEntry >= '97') && (keyEntry <= '122')) || (keyEntry == '32'))
        return true;
    else {
        //alert('Please Enter Only Character values.');
        return false;
    }
}

// Function that checks whether input text is Special Character or not.
function SpecialChar(e) {
    isIE = document.all ? 1 : 0
    code = !isIE ? e.which : event.keyCode;
    if (!(code >= 65 && code <= 91) && !(code >= 97 && code <= 121) && !(code >= 48 && code <= 57))
        return true;
    else {
        //alert('Please Enter Only Character values.');
        return false;
    }
}
// function to make the textboxes accept only decimals number
function decimalOnly(evt) {
    evt = (evt) ? evt : event;
    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode : ((evt.which) ? evt.which : 0));
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46) {
        return false;
    }
    return true;
}

// Function that checks whether input text is AlphaNumeric or not.
function isAlphaNumeric(e) {
    isIE = document.all ? 1 : 0
    code = !isIE ? e.which : event.keyCode;
    if ((code >= 48 && code <= 57) || (code >= 65 && code <= 90) || (code >= 97 && code <= 122) || (code == 32))

        return true;
    else {
        //alert('Please Enter Only Character values.');
        return false;
    }
}

// Function for max length.
function checkLength(el) {
    
    if (el.value.length != 20) {
        return true;
    }
    else {
        //alert("length must be exactly 6 characters");
        return false;
    }
}

function trim(s) {
    while (s.substring(0, 1) == ' ') {
        s = s.substring(1, s.length);
    }
    while (s.substring(s.length - 1, s.length) == ' ') {
        s = s.substring(0, s.length - 1);
    }
    return s;
}


function ValidateNID(chr) {
    var str;
    
    //str = document.getElementById('<%=txtcardNo.ClientID%>').value;
    str = chr;
    if (str.keyCode != 8) {

        if (str.length == 6 || str.length == 9 || str.length == 14) {
            //str.concat(str,"-")
            str += "/";
            //document.getElementById('<%=txtcardNo.ClientID%>').value = str;
            chr = str;
        }
    }
    
    return chr;
}


function ValidateCreditCardNo(chr) {
    var str;
    
    //str = document.getElementById('<%=txtcardNo.ClientID%>').value;
    str = chr;
    if (str.keyCode != 8) {

        if (str.length == 4 || str.length == 9 || str.length == 14) {
            //str.concat(str,"-")
            str += " ";
            //document.getElementById('<%=txtcardNo.ClientID%>').value = str;
            chr = str;
        }
    }

    return chr;
}

function checkLengthForPincode(el) {

    if (trim(el.value) != "") {
        if (trim(el.value).length == 5) {
            return true;
        }
        else {
            alert("Enter valid Pincode");
            el.value = null;
            el.focus();
            return false;
        }
    }
}


function checkLengthForContact(el) {
    
    if (trim(el.value) != "") {
        if (trim(el.value).length == 10) {
            return true;
        }
        else {
            alert("Enter valid Contact number");
            el.value = null;
            el.focus();
          
            return false;
        }
    }
}

function checkLengthForCode(el) {
    

    if (trim(el.value).length == 3 || trim(el.value).length == 4) {
        return true;
    }
    else {
        alert("Enter valid code for contact number");
        return false;
    }
}
function checkLengthForBankCode(el) {
    
    if (el.value != "") {
        //if (el.value.length >= 8 && el.value.length <= 11) {
        if (el.value.length == 11) {
            return true;
        }
        else {
            alert("Please Enter Bank Code.");
            el.value = null;
            return false;
        }
    }
}
function checkLengthForBranchCode(el) {
    
    if (el.value != "") {
        //if (el.value.length >= 8 && el.value.length <= 11) {
        if (el.value.length == 11) {
            return true;
        }
        else {
            alert("Please Enter Branch Code.");
            el.value = null;
            return false;
        }
    }
}
function checkLengthForSwiftCode(el) {
    
    if (el.value != "") {
        //if (el.value.length >= 8 && el.value.length <= 11) {
        if (el.value.length == 11) {
            return true;
        }
        else {
            alert("Please Enter Swift Code.");
            el.value = null;
            return false;
        }
    }
}
function ValidatePAN(Obj) {
    
    if (Obj.value != "") {
        ObjVal = Obj.value;
        var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
        if (ObjVal.search(panPat) == -1) {
            alert("Invalid Pan No");
            Obj.value = null;
            Obj.focus();
            return false;
        }
    }
}
function checkLengthForNRC(el) {
    if (el.value != "") {
        if (el.value.length == 13) {
            return true;
        }
        else {
            alert("Enter valid NID number");
            el.value="";
            return false;
        }


    }
}


function validateEmail(emailField) {
    
    if (emailField.value != "") {
        var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
        if (reg.test(emailField.value) == false) {
            alert('Invalid Email Address');
            emailField.value = null;
            emailField.focus();
            return false;
        }
    }
    return true;

}


function ToJavaScriptDate(value) {
    
    if (value != null) {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));
        var d = new Date(dt),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;

        return [year, month, day].join('-');
    }
    else {

        return '-';
    }
   
}

function ValidateDate(DOB) {
    
    var today = new Date();
    //    var birthday1 = $('#txtDisDt').val();
    var d1 = DOB.value;
    var d = new Date(d1);

    var age = (
          (today.getMonth() > d.getMonth())
          ||
          (today.getMonth() == d.getMonth() && today.getDate() >= d.getDate())
        ) ? today.getFullYear() - d.getFullYear() : today.getFullYear() - d.getFullYear() - 1;

    if (age >= 18) {
        //alert('18+');
    } else {
        alert("Age should be 18 year Completed");
        DOB.value = null;
    }
}
function isAddress(evt) {
    
    evt = (evt) ? evt : window.event;
    var code = (evt.which) ? evt.which : evt.keyCode;
    if ((code >= 44 && code <= 57) || (code >= 65 && code <= 90) || (code >= 97 && code <= 122) || (code == 32) || (code == 40) || (code == 41)) 

        return true;
    
    else {
        //alert('Please Enter Only Character values.');
        return false;
    }
}