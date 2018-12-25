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
    var currentdate = new Date();
    var m = currentdate.getMonth() + 1;
    var dt = currentdate.getFullYear() + "." + m + "."
        + currentdate.getDate() + " "
        + currentdate.getHours() + ":"
        + currentdate.getMinutes() + ":"
        + currentdate.getSeconds();
    return dt;
}

function taskComplete(taskCompleteStr) {
    $("#Task" + taskNo).val("Y");
     dt = getTodayDate();
     $("#" + taskCompleteDate).val(dt);

    taskStatus = taskCompleteStr + userSignature + " on " + dt;
    $("#" + taskStatusFlag).val(userSignature);
    $("#TaskStatus").html(taskStatus);
    $("#saveButton").hide();
    $("#ReOpenTask").show();
    $("#TaskComplete").hide();
    $("#saveButton").click();
}


function renderTaskShowHide() {
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
        taskComplete(taskCompleteStr);
    });
    // actions when reopen the job
    $("#ReOpenTask").click(function () {
        $("#Task" + taskNo).val("TASK");
        console.log("#Task" + taskNo);

        $("#" + taskCompleteDate).val(null);
        taskStatus = null;
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

function renderTaskHandling() {

    $("#selectListHandler").append(new Option("-- Please select --", null));
    $.each(LUser, function (key, data) {
        $("#selectListHandler").append(new Option(data.Signature, data.Signature));
    });

    // Set up selectList for variable items
    $.each(["-- Please select --", "FEE", "AGR", "EXE"], function (index, value) {
        $("#selectListTask").append(new Option(value, value));
    });
    //read the info. of new task handler
    $("#selectListHandler").on("change", function () {
        var usig = $(this).val();
        readBUser(usig).done(returnTargetUser);
    });
    //read the email template
    $("#selectListTask").on("change", function () {
        var tn = "for" + $(this).val();
        readBEmail(tn, bm).done(returnBEmail);
    });
    //Send email

    $('#emailLink').on('click', function (event) {

        $("#taskHandler").html(TargetUser.uniqueKey);

        var newTask = 0;
        switch (BEmial.TemplateName + BEmail.BridgeModule) {
            case "forFeeM1":
                newTask = 1;
                break;
            case "forEXEM1":
                newTask = 2;
                break;
            case "forAGRM1":
                newTask = 3;
                break;
            case "forFNLM1":
                newTask = 4;
                break;
            default:
                newTask = 1;
        }

        $("Task" + newTask).val("TASK");

        event.preventDefault();
        var email = eval(BEmail.mailTo);
        var subject = eval(BEmail.mailTitle);
        var emailBody = eval(BEmail.mailBody);
        window.location = 'mailto:' + email + '?subject=' + subject + '&body=' + emailBody;
    });

}