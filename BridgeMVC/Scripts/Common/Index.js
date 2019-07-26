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

    var arrows = document.getElementsByClassName("arrows");
    for (i = 0; i < arrows.length; i++) {
        if (arrows[i].innerHTML.length > 0) {
            arrows[i].innerHTML = "&#11046"; // 11045 for all black diamond
        }
    }
    if (arrows[n].innerHTML.length > 0) {
        if (dir == "asc") {
            arrows[n].innerHTML = "&#9652";
        }
        else if (dir == "desc") {
            arrows[n].innerHTML = "&#9662" // "&#9660";
        }
    }
}