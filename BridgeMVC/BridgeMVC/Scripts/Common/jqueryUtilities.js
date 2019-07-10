function readBEmail(tn, bm) {
    return $.ajax({
        type: 'GET',
        url: '/API/ReadBEmail',
        data: { templateName: tn, bridgeModule: bm },
        cache: false,
        success: returnBEmail
    });
}

function returnBEmail(data) {
    BEmail = jQuery.parseJSON(data);
}

function resizeIframe(obj) {
    /*
    Resizes an <iframe> window.
    :arg obj: an <iframe> object
    */
    obj.style.height = obj.contentWindow.document.body.scrollHeight + 'px';
}

function showElement(id) {
    /*
    Shows a document element with a given id.
    :arg id: string, the id of the element
    */
    if (id) {
        document.getElementById(id).parentNode.style.display = "block";
    }
}

function hideElement(id) {
    /*
    Hides a document element with a given id.
    :arg id: string, the id of the element
    */
    if (id) {
        document.getElementById(id).parentNode.style.display = "none";
    }
}

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

    if (taskStatusFlag === "IORASentBy")
    {
        console.log("Send IORA to LU");
        $("#SendingFlag").val(9); 
        console.log($("#TaskHandler").val());
        $('#jobForm').submit();
    };
}

function readBUser(sig) {
    return $.ajax({
        type: 'GET',
        url: '/API/ReadBUser',
        data: { sig: sig },
        cache: false,
        success: returnTargetUser
    });
}

function returnTargetUser(data) {
    /*
    Add function description.
    */
    TargetUser = jQuery.parseJSON(data);
}

function renderTaskShowHide(taskNo, taskStatusFlag, taskCompleteDate, taskCompleteStr, userSignature) {
    /*
    Renders which elements to show and hide.
    :arg taskNo:
    :arg taskStatusFlag: string
    :arg taskCompleteDate: string
    :arg taskCompleteStr: string, 
    :arg userSignature: string, the signature of the user
    */
    var taskStatus = null;

    if ($("#" + taskStatusFlag).val().length > 0) {
        taskStatus = taskCompleteStr + $("#" + taskStatusFlag).val()
            + " on " + $("#" + taskCompleteDate).val();
        $("#TaskStatus").html(taskStatus);
        $("#saveButton").hide();
        $("#ReOpenTask").show();
        $("#TaskComplete").hide();
    } else {
        $("#saveButton").show();
        $("#ReOpenTask").hide();
        $("#TaskComplete").show();
    }

    // actions when taskcomplete button is clicked
    $("#TaskComplete").click(function () {
        taskComplete(taskNo, taskStatusFlag, taskCompleteDate, taskCompleteStr, userSignature);
    });

    // actions when reopen the job
    $("#ReOpenTask").click(function () {
        $("#Task" + taskNo).val("TASK");
        $("#" + taskCompleteDate).val(null);
        $("#" + taskStatusFlag).val(null);
        $("#TaskStatus").html(taskStatus);
        $("#saveButton").show();
        $("#ReOpenTask").hide();
        $("#TaskComplete").show();
    });
}

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
    var defaultHiddenList = ("MEDFBNo,MEDFBDue,SerialNo,CertAmount,MWL,MEDItemNo," +
        "ExistingCertNo,SurveyStation,SurveyDate,ModificationDesc").split(',');

    // Hides displays
    defaultHiddenList.forEach(hideElement);

    var toShow = "";

    switch (certType) {
        case "MED-F":
            toShow += "MEDFBNo,MEDFBDue,SerialNo,CertAmount,MEDItemNo,SurveyStation,SurveyDate,";
            $("#pMEDFBNo").html("Ref. MED-B certificate No.");
            $("#selectCertAction").val("Initial");
            $("#selectMainProdType").val("Life-Saving appliances");
            $("#selectSubProdType").append(new Option("Module F certification", "Module F certification"));
            $("#selectSubProdType").val("Module F certification");
            break;
        case "MED-G":
            toShow += "SerialNo,MWL,MEDItemNo,";
            break;
        case "MED-B":
            toShow += "MWL,MEDItemNo,";
            break;
        case "TA":
            toShow += "MWL,";
            break;
        case "DVR":
            toShow += "SerialNo,MWL,";
            break;
        case "MED-D":
            toShow += "MWL,";
            break;
        default:
            toShow += "";
    }

    switch (certAction) {
        case "Modification":
            toShow += "ExistingCertNo,ModificationDesc";
            break;
        case "Initial with reference":
            toShow += "ExistingCertNo,";
            break;
        case "Renewal with modification":
            toShow += "ExistingCertNo,ModificationDesc";
            break;
        case "Renewal":
            toShow += "ExistingCertNo,";
            break;
        default:
            toShow += "";
    }

    if (toShow) {
        var splits = toShow.split(',');
        splits.forEach(showElement);
    }
}

function budgetHourCallback(module, taskNo, certType) {
    /*
    Add function description.
    :arg module: string, the Bridge module
    :arg taskNo: int, the task number
    :arg certType: string, the certificate type
    */
    var validModules = ["M1"];
    var validTaskNos = [1];
    var urlString = "/Job/";

    if (validModules.includes(module) && validTaskNos.includes(taskNo)) {
        urlString += module + "_Task" + taskNo.toString(10) + "_BudgetHourCalc";
    } else {
        alert("Budget hour calculation is not available for module "
            + module + ", task " + taskNo.toString(10) + " yet.");
        return;
    }

    if (!certType) {
        alert('The Certitication Type has to be selected first.');
    } else {
        return $.ajax({
            type: 'GET',
            url: urlString,
            data: { bm_f: bm, ct_f: certType },
            cache: false,
            success: calcBudgetHour
        });
    }
}

function calcBudgetHour(data) {
    /*
    Calculates the budget hours based on the certificate type.
    :arg data: string, data in JSON format
    */
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

function appendElementOptions(id, obj) {
    /*
    Adds options from a json object to a html element.
    :arg id: the element id
    :arg obj: a json object with key-value pairs
    */
    id = "#" + id;

    if ($(id).length > 0) {
        $.each(obj, function (key, value) {
            if (value != null && value != "undefined") {
                option = new Option(value, value);
                $(id).append(option);
            }
        });
    }
}