function sortTable(n) {
    // n is the column number to be sorted
    // Add onclick="sortTable(n)" in <th>
    // If to be sorted numerically, only number-string inside <td>
    var table = $("#sortTable");
    // Get rows greater than 0. Not header
    var rows = table.find("tr:gt(0)").toArray()
    var firstVal = getCellValue(rows[0], n);
    // Sort ascending
    rows = rows.sort(comparer(n))

    if (firstVal == getCellValue(rows[0], n)) {
        // Sort descending 
        rows = rows.reverse()
    }

    for (var i = 0; i < rows.length; i++) {
        table.append(rows[i])
    }

       // If you want to have arrows as well
        //var arrow = document.getElementsByClassName("arrows");
        //for (i = 0; i < arrow.length; i++) {
        //    if (arrow[i].innerHTML.length > 0) {
        //        arrow[i].innerHTML = "&#11046"; // 11045 for all black diamond
        //    }
        //}
        //if (arrow[n].innerHTML.length > 0) {
        //    if (dir == "asc") {
        //        arrow[n].innerHTML = "&#9652";
        //    }
        //    else if (dir == "desc") {
        //        arrow[n].innerHTML = "&#9662" // "&#9660";
        //    }
        //}
}

function comparer(index) {
    return function (a, b) {
        var valA = getCellValue(a, index), valB = getCellValue(b, index)
        return $.isNumeric(valA) && $.isNumeric(valB) ? valA - valB : valA.toString().localeCompare(valB)
    }
}

function getCellValue(row, index) {
    return $(row).children('td').eq(index).text()
}


function stats(jobs, fee) {
    var stringFee = fee.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1 ');
    var stats = $("#stats").html().replace("Fee: 0 NOK", "Fee: " + stringFee + " NOK");
    stats = stats.replace("Jobs: 0", "Jobs: " + jobs)
    $("#stats").html(stats);
}

function startPause(clickedId) {
    var newNote = "";
    var clickedItem = clickedId.split("_")[0];
    var jid = clickedId.split('_')[1];
    var statusid = "status_" + jid;
    document.getElementById(clickedId).style.display = 'none';
    if (clickedItem == "start") {
        newNote = "Resumed_on:" + formateDateNow() + " by " + us + ";";
    }
    else if (clickedItem == "pause") {
        newNote = "OH_from:" + formateDateNow() + " by " + us + ";";
    }
    updateOhNote(jid, newNote)
    document.getElementById(statusid).innerHTML = newNote;
}

function updateOhNote(jid, newNote) {
    $.ajax(
        {
            type: 'GET',
            url: '/Job/UpdateOnHoldNote',
            data: { id: jid, newNote: newNote },
            success: function (data) {
                console.log(data);
            },
            error: function (e) {
                alert('Error: ' + e);
            }
        });
}

function formateDateNow() {
    var d = new Date();
    var day = d.getDay();
    var hr = d.getHours();
    var min = d.getMinutes();
    var ss = d.getSeconds();
    if (min < 10) {
        min = "0" + min;
    }
    //var ampm = "am";
    //if (hr > 12) {
    //    hr -= 12;
    //    ampm = "pm";
    //}
    var date = d.getDate();
    var month = d.getMonth();
    var year = d.getFullYear();
    return year + '-' + month + '-' + date + ' ' + hr + ':' + min + ':00';
}

$(document).ready(function () {
    if (typeof LUser !== "undefined" && typeof us !== "undefined") {
        $("#userSig").append(new Option(us, us));
        $.each(LUser, function (key, data) {
            $("#userSig").append(new Option(data.Signature, data.Signature));
        });
    }
});


