function sortTable(n) {
    // n is the column number to be sorted
    // Add onclick="sortTable(n)" in <th>
    // If sorted numerically, only number string inside <th>
    var table, rows, switching, i, num, x, y, shouldSwitch, dir, switchcount = 0;
    table = document.getElementById("sortTable");
    switching = true;
    //Set the sorting direction to ascending:
    dir = "asc";
    /*Make a loop that will continue until
    no switching has been done:*/
    while (switching) {
        //start by saying: no switching is done:
        switching = false;
        rows = table.rows;
        /*Loop through all table rows (except the
        first, which contains table headers):*/
        for (i = 1; i < (rows.length - 1); i++) {
            //start by saying there should be no switching:
            shouldSwitch = false;
            /*Get the two elements you want to compare,
            one from current row and one from the next:*/
            x = rows[i].getElementsByTagName("td")[n].innerHTML;
            y = rows[i + 1].getElementsByTagName("td")[n].innerHTML;
            /*Check if the column is numbers or letters */
            if (isNaN(x.replace(",", ".")) && isNaN(y.replace(",", "."))) {
                x = x.toLowerCase();
                y = y.toLowerCase();
            }
            else {
                x = Number(x.replace(",", "."));
                y = Number(y.replace(",", "."));
            }
            /*check if the two rows should switch place,
            based on the direction, asc or desc:*/
            if (dir == "asc" && x > y) {
                //if so, mark as a switch and break the loop:
                shouldSwitch = true;
                break;
            }
            else if (dir == "desc" && x < y) {
                //if so, mark as a switch and break the loop:
                shouldSwitch = true;
                break;
            }
        }

        if (shouldSwitch) {
            /*If a switch has been marked, make the switch
            and mark that a switch has been done:*/
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
            //Each time a switch is done, increase this count by 1:
            switchcount++;
        } else {
            /*If no switching has been done AND the direction is "asc",
            set the direction to "desc" and run the while loop again.*/
            if (switchcount == 0 && dir == "asc") {
                dir = "desc";
                switching = true;
            }
        }
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


