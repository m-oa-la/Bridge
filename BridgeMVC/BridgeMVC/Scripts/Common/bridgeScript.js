function getObject(obj, key, keyName) {
    var object = [];
    for (var i in obj) {
        if (!obj.hasOwnProperty(i)) continue;
        if (typeof obj[i] === 'object') {
            object = object.concat(getObject(obj[i], key, keyName));
        } else if (i === key && obj[key] === keyName) {
            object.push(obj);
            break;
        }
    }

    return object;
}

function hasDuplicates(array) {
    return (new Set(array)).size !== array.length;
}

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
    console.log(BEmail);
}

function getTodayDate() {
    /*
    Returns the current date and time.
    */
    var currentdate = new Date();
    var m = currentdate.getMonth() + 1;
    var dt = currentdate.getFullYear() + "." + m + "."
        + currentdate.getDate() + " "
        + currentdate.getHours() + ":"
        + currentdate.getMinutes() + ":"
        + currentdate.getSeconds();
    return dt;
}

function taskComplete(taskNo, taskStatusFlag, taskCompleteDate, taskCompleteStr, userSignature) {
    /*
    Add function description.

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

    if ($("#" + taskStatusFlag).val().length !== 0) {
        taskStatus = taskCompleteStr + $("#" + taskStatusFlag).val() + " on " + $("#" + taskCompleteDate).val();
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
    TargetUser = jQuery.parseJSON(data);
}

function renderTaskHandling(LUser, bm) {
    /*
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
