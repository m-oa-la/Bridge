
function showHideFields() {

    $("#TaskComplete").val("Confirm input and Draft IORA");
    document.getElementById("TaskComplete").style.width = "400px";

    var defaultHiddenList = "MEDFBNo,MEDFBDue,SerialNo,CertAmount,MWL,MEDItemNo,ExistingCertNo,ModificationDesc".split(',');
    defaultHiddenList.forEach(hideElement);

    var toShow = "";

    switch ($("#selectCertType").val()) {
        case "MED-F":
            toShow += "MEDFBNo,MEDFBDue,SerialNo,CertAmount,MEDItemNo,SurveyStation,SurveyDate,";
            $("#pMEDFBNo").html("Ref. MED-B certificate No.");
            $("#selectCertAction").val("Initial");
            $("#selectMainProdType").val("Life-Saving appliances");
            $("#selectSubProdType").append(new Option("Module F certification", "Module F certification"));
            $("#selectSubProdType").val("Module F certification");
            //$("#SubProdType").val("Module F certification");
            break;
        case "MED-G":
            toShow += "SerialNo,MEDItemNo,";
            break;
        case "MED-B":
            toShow += "MEDItemNo,";
            break;
        case "DVR":
            toShow += "SerialNo,";
            break;
        case "MED-D":
            toShow += "";
            break;
        default:
            toShow += "";
    }

    if ($("#selectCertType").val() == "PA - TSA-funded") {
        $("#TaskComplete").val("Confirm & send job to Whiteboard");
        //$("#TaskComplete").style.backgroundColor = "pink";
    } else if ($("#selectCertType").val() == "PED" || $("#selectCertType").val() == "TPED") {
        document.getElementById("TaskComplete").style.display = "none";
    } else {
        $("#TaskComplete").val("Confirm & draft IORA");
        //$("#TaskComplete").style.backgroundColor = "darkblue";
    }


    switch ($("#selectCertAction").val()) {
        case "Modification":
            toShow += "ExistingCertNo,";
            break;
        case "Initial with reference":
            toShow += "ExistingCertNo,";
            break;
        case "Renewal with modification":
            toShow += "ExistingCertNo,";
            break;
        case "Renewal":
            toShow += "ExistingCertNo,";
            break;
        default:
            toShow += "";
    }

    if (toShow) {
        var splitS = toShow.split(',');
        splitS.forEach(showElement);

    }
}

function showElement(value) {
    if (value) {
        document.getElementById(value).parentNode.style.display = "block";
    }
}
function hideElement(value) {
    if (value) {
        console.log(value);
        document.getElementById(value).parentNode.style.display = "none";
    }
}

//$("#selectCertType").change();

$("#selectCertType").on("change", function () {
    showHideFields()
});
$("#selectCertAction").on("change", function () {
    showHideFields()
});






//set up select list
$.each(LCertType, function (key, data) {

    $("#selectCertType").append(new Option(data.CertType, data.CertType));
});

$.each(LCertAction, function (key, data) {
    $("#selectCertAction").append(new Option(data.ListItem, data.ListItem));
});

$.each(LMainProdType, function (key, data) {
    $("#selectMainProdType").append(new Option(data.ListItem, data.ListItem));
});

var mval = $("#selectMainProdType :selected").text();
$.each(LSubProdType, function (key, data) {
    if (data.UpperLvl == mval) {
        $("#selectSubProdType").append(new Option(data.ListItem, data.ListItem));
    }
});

//When main prodtype changes, refesh the sub prod type.
$("#selectMainProdType").on("change", function () {
    var mval = $("#selectMainProdType :selected").text();

    $('#selectSubProdType option').each(function () {
        if ($(this).val() != 'X') {
            $(this).remove();
        }
    });

    $("#selectSubProdType option").remove();
    $.each(LSubProdType, function (key, data) {
        if (data.UpperLvl == mval) {
            $("#selectSubProdType").append(new Option(data.ListItem, data.ListItem));
        }
    });
});

function budgetHourCalc() {

    if ($("#selectCertType").val() + "x" == "x") {
        alert('The Certitication Type has to be selected first.');
    } else {
        ct1 = $("#selectCertType").val();

        return $.ajax({
            type: 'GET',
            url: '/Job/M1_Task1_BudgetHourCalc',
            data: { bm_f: bm, ct_f: ct1 },
            cache: false,
            success: calcBudgetHour
        });
    }

}

function calcBudgetHour(data) {

    var f = jQuery.parseJSON(data);
    var dis = (1 - f.allocationFee) * (1 - f.tsa - f.msa);
    var feee = document.getElementById("Fee").value;;
    var internalFee = Math.round(feee * dis);
    var bh = Math.round(internalFee * 0.74 / 1200);
    $("#BudgetHour").val(bh);
    alert("External fee: " + feee +
        ";\nAllocation Fee: " + f.allocationFee +
        ";\nTSA: " + f.tsa +
        ";\nMSA: " + f.msa +
        ";\nInternal Fee: " + internalFee +
        ";\nBudgetHour = InternalFee * 0,74 / 1200 = " + bh);
}


