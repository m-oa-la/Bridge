function showHideFields() {

    var defaultHiddenList = "MEDFBNo,MEDFBDue,SerialNo,CertAmount,MWL,MEDItemNo,ExistingCertNo,SurveyStation,SurveyDate_Input,ModificationDesc".split(',');
    defaultHiddenList.forEach(hideElement);

    var toShow = "";

    switch (Job.CertType) {
        case "MED-F":
            toShow += "MEDFBNo,MEDFBDue,SerialNo,CertAmount,MEDItemNo,SurveyStation,SurveyDate_Input,";
            $("#pMEDFBNo").html("Ref. MED-B certificate No.");
            $("#selectCertAction").val("Initial");
            $("#selectMainProdType").val("Life-Saving appliances");
            $("#selectSubProdType").append(new Option("Module F certification", "Module F certification"));
            $("#selectSubProdType").val("Module F certification");
            document.getElementById("signatures").style.display = "none";
            //$("#SubProdType").val("Module F certification");
            break;
        case "MED-G":
            toShow += "SerialNo,MWL,MEDItemNo,SurveyStation,SurveyDate_Input,";
            break;
        case "MED-B":
            toShow += "MWL,MEDItemNo,SurveyStation,SurveyDate_Input,";
            break;
        case "TA":
            toShow += "MWL,SurveyStation,SurveyDate_Input,";
            break;
        case "DVR":
            toShow += "SerialNo,MWL,";
            break;
        case "MED-D":
            toShow += "MWL,SurveyStation,SurveyDate_Input,";
            break;
        default:
            toShow += "";
    }

    switch (Job.CertAction) {
        case "Modification":
            toShow += "ExistingCertNo,ModificationDesc,";
            break;
        case "Initial with reference":
            toShow += "ExistingCertNo,";
            break;
        case "Renewal with modification":
            toShow += "ExistingCertNo,ModificationDesc,";
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

// Possibly add to common
function showElement(value) {
    if (value) {
        document.getElementById(value).parentNode.parentNode.style.display = "block";
    }
}

// Possibly add to common
function hideElement(value) {
    if (value) {
        document.getElementById(value).parentNode.parentNode.style.display = "none";
    }
}

//$("#selectCertType").change();

// Look into creating generalized version
$("#selectCertType").on("change", function () {
    showHideFields()
});

$("#selectCertAction").on("change", function () {
    showHideFields()
});

// Look into creating generalized version
$.each(LCertType, function (key, data) {
    $("#selectCertType").append(new Option(data.ListItem, data.ListItem));
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

// Look into creating generalized version
function resizeIframe(obj) {
    obj.style.height = obj.contentWindow.document.body.scrollHeight + 'px';
}