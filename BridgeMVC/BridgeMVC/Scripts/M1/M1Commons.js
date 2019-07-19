// TODO
function taskComplete(taskNo, taskStatusFlag, taskCompleteDate, taskCompleteStr, userSignature) {
    /*
    Add function description.
    :arg taskNo:
    :arg taskStatusFlag:
    :arg taskCompleteDate:
    :arg taskCompleteStr:
    :arg userSignature:
    */
    $("#Task" + taskNo).val("Y");
    var dt = getTodayDate();
    $("#" + taskCompleteDate).val(dt);

    console.log($("#selectCertType").val());

    if ($("#BridgeModule").val() === "M2") {

        if ($("#selectCertType").val() === "PA - TSA-funded") {
            $("#SendingFlag").val("TSAtoWB");
            console.log("TSAtoWB");
        } else {
            $("#SendingFlag").val("M2DraftIORA");
            console.log("m2draftiora");
        }
    }

    var taskStatus = taskCompleteStr + userSignature + " on " + dt;

    $("#" + taskStatusFlag).val(userSignature);
    $("#TaskStatus").html(taskStatus);
    $("#saveButton").click();
    $("#ReOpenTask").show();
    $("#TaskComplete").hide();
    $("#saveButton").hide();

    if (taskStatusFlag === "IORASentBy") {
        console.log("Send IORA to LU");
        $("#SendingFlag").val(9);
        console.log($("#TaskHandler").val());
        $('#jobForm').submit();
    }
}

function renderTaskActions(taskNo, taskStatusFlagId, taskCompleteDateId, taskCompleteStr, userSignature) {
    /*
    Renders task status and task action buttons like re-open, save and complete.
    :arg taskNo: int, the task number
    :arg taskStatusFlagId: string, the id of the document taskStatusFlag
    :arg taskCompleteDateId: string, the id of the document date element
    :arg taskCompleteStr: string, task complete message
    :arg userSignature: string, the signature of the user
    */
    var taskStatus = null;

    if ($("#" + taskStatusFlagId).val().length > 0) {
        taskStatus = taskCompleteStr + $("#" + taskStatusFlagId).val()
            + " on " + $("#" + taskCompleteDateId).val();
        $("#TaskStatus").html(taskStatus);
        $("#saveButton").hide();
        $("#ReOpenTask").show();
        $("#TaskComplete").hide();
    } else {
        $("#saveButton").show();
        $("#ReOpenTask").hide();
        $("#TaskComplete").show();
    }

    // Add event listener to TaskComplete element
    $("#TaskComplete").click(function () {
        taskComplete(taskNo, taskStatusFlagId, taskCompleteDateId, taskCompleteStr, userSignature);
    });

    // Add event listener to ReOpenTask element
    $("#ReOpenTask").click(function () {
        $("#Task" + taskNo).val("TASK");
        $("#" + taskCompleteDateId).val(null);
        $("#" + taskStatusFlagId).val(null);
        $("#TaskStatus").html(taskStatus);
        $("#saveButton").show();
        $("#ReOpenTask").hide();
        $("#TaskComplete").show();
    });
}

// TODO
function renderTaskHandling(LUser, bm) {
    /*
    Add function description.
    :arg LUser: List of users(?).
    :arg bm: The bridge module.
    */
    $("#selectListHandler").append(new Option("-- Please select --", null));
    $.each(LUser, function (key, data) {
        $("#selectListHandler").append(new Option(data.Signature, data.Signature));
    });

    // Set up selectList for variable items
    if (bm !== "M2") {
        $.each(["-- Please select --", "1.FEE", "2.AGR", "3.EXE", "4.FNL"], function (index, value) {
            $("#selectListTask").append(new Option(value, value));
        });
    } else if (bm === "M2") {
        $.each(["-- Please select --", "1.FEE", "2.VER", "3.AGR", "4.WHITEBOARD"], function (index, value) {
            $("#selectListTask").append(new Option(value, value));
        });
    }
}

function renderTaskInputFields(certType, certAction) {
    /*
    Renders input fields based on the type of certificate that is selected.
    :arg certType: string, the certificate type
    :arg certAction: string, the certificate action
    */
    var elemIds = getDisplayElements(certType, certAction);
    displayElements(elemIds[0], show = true);
    displayElements(elemIds[1], show = false);
}

function getDisplayElements(certType, certAction) {
    /*
    Gets the ids of the elements that should be shown.
    :arg certType: string, the certificate type
    :arg certAction: string, the certificate action
    :return: list of lists of strings, the element ids
    */

    // Default element ids to hide.
    var toHide = ["MEDFBNo", "MEDFBDue", "SerialNo", "CertAmount", "MWL", "MEDItemNo",
        "ExistingCertNo", "SurveyStation", "SurveyDate", "ModificationDesc"];
    var toShow = [];
    var extension = null;

    // Get display element ids based on certificate type
    extension = getCertificateTypeDisplayElements(certType);
    toShow.push.apply(toShow, extension);

    // Get display element ids based on certificate action
    extension = getCertificateActionDisplayElements(certAction);
    toShow.push.apply(toShow, extension);

    toHide = filterArray(toHide, toShow);
    return [toShow, toHide];
}

function getCertificateTypeDisplayElements(certType) {
    /*
    Module business logic. Returns the ids of the elements that should
    be shown based on the certificate type.
    :arg certType: string, the certificate type
    :return: list of strings, the element ids
    */
    var elemIds = [];

    switch (certType) {
        case "MED-F":
            elemIds = ["MEDFBNo", "MEDFBDue", "SerialNo", "CertAmount",
                "MEDItemNo", "SurveyStation", "SurveyDate"];
            break;
        case "MED-G":
            elemIds = ["SerialNo", "MWL", "MEDItemNo"];
            break;
        case "MED-B":
            elemIds = ["MWL", "MEDItemNo"];
            break;
        case "TA":
            elemIds = ["MWL"];
            break;
        case "DVR":
            elemIds = ["SerialNo", "MWL"];
            break;
        case "MED-D":
            elemIds = ["MWL"];
            break;
        default:
            break;
    }

    return elemIds;
}

function getCertificateActionDisplayElements(certAction) {
    /*
    Module business logic. Returns the ids of the elements that should
    be shown based on the certificate action.
    :arg certAction: string, the certificate action
    :return: list of strings, the element ids
    */
    var elemIds = [];

    switch(certAction) {
        case "Modification":
            elemIds = ["paraExistingCertNo", "paraModificationDesc"];
            break;
        case "Initial with reference":
            elemIds = ["paraExistingCertNo"];
            break;
        case "Renewal with modification":
            elemIds = ["paraExistingCertNo", "paraModificationDesc"];
            break;
        case "Renewal":
            elemIds = ["paraExistingCertNo"];
            break;
        default:
            break;
    }

    return elemIds;
}
