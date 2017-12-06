function Check_Click(objRef) {
    var row = objRef.parentNode.parentNode;
    var GridView = row.parentNode;
    var inputList = GridView.getElementsByTagName("input");
    var srcControlId = event.srcElement.id;
    var targetControlId = event.srcElement.id.replace('chkchild', 'txt');
    if (document.getElementById(srcControlId).checked) {
        document.getElementById(targetControlId).disabled = false;


    }
    else {
        document.getElementById(targetControlId).disabled = true;
        document.getElementById(targetControlId).value = "0";

    }
    for (var i = 0; i < inputList.length; i++) {
        var headerCheckBox = inputList[0];

        var checked = true;
        if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
            if (!inputList[i].checked) {
                checked = false;
                break;
            }
        }
    }
    headerCheckBox.checked = checked;

}

function checkAll(objRef) {
    var GridView = objRef.parentNode.parentNode.parentNode;
    var inputList = GridView.getElementsByTagName("input");
    var inputElements = document.getElementsByTagName('input');
    for (var i = 0; i < inputElements.length; i++) {
        var myElement = inputElements[i];
        if (objRef.checked) {
            if (myElement.type == "text") {
                myElement.disabled = false;

            }
        }
        else {
            if (myElement.type == "text") {
                myElement.disabled = true;
                myElement.value = "0";
            }

        }

    }
    for (var i = 0; i < inputList.length; i++) {
        if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
            if (objRef.checked) {
                inputList[i].checked = true;

            }
            else {
                inputList[i].checked = false;

            }


        }


    }
}
function isNumberKey(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
        
    return true;
}

function CalculateAge(delDate,day2) {

    var mm = new Date();
    var dd1 = delDate.value;
    var sDate = new Date(dd1);
    var day = 1000 * 60 * 60 * 24;
    day2  = (Math.ceil((sDate.getTime() - mm.getTime()) / day));
    alert(day2);
}

function CalDate() {
    days = document.getElementById('<%= txtDay.ClientID %>').value;
    var d = new Date();
    d.setDate(d.getDate() + parseInt(days));
    document.getElementById('<%= txtDeldate.ClientID %>').value = new Date(d).format("dd-MMM-yyyy");
   

}   
