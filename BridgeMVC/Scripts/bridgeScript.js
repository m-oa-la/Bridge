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


function renderTaskShowHide() {
    if ($("#" + taskStatusFlag).val().length !== 0) {
        console.log("taskcompletedate: " + taskCompleteDate);
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
    if (bm === "M1") {
        $.each(["-- Please select --", "1.FEE", "2.AGR", "3.EXE", "4.FNL"], function (index, value) {
            $("#selectListTask").append(new Option(value, value));
        });
    } else if (bm === "M2") {
        $.each(["-- Please select --", "1.FEE", "2.VER", "3.AGR", "4.WHITEBOARD"], function (index, value) {
            $("#selectListTask").append(new Option(value, value));
        });
    }

    

    $("#selectListHandler").on('change', function () {
        UpdateSendingInfo();
    });


    $("#selectListTask").on('change', function () {
        UpdateSendingInfo();
     });

    function UpdateSendingInfo() {
        var vtask = $("#selectListTask").val();
        var vhandler = $("#selectListHandler").val();

        var newTaskNo = vtask.slice(0, 1);
        $("#Task" + newTaskNo).val("TASK");



        if (vtask !== null) {
            return $.ajax({
                type: 'GET',
                url: '/Job/SetTaskSendingFlag',
                data: { newHandler: vhandler, newTask: newTaskNo },
                cache: false,
                success: console.log(vhandler + " " + newTaskNo)
            });
        }
    }




}
