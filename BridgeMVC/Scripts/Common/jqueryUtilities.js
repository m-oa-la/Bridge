function resizeIframe(obj) {
    /* Resizes an <iframe> window.
     * arg obj: an <iframe> object */
    obj.style.height = obj.contentWindow.document.body.scrollHeight + 'px';
}

function showElement(id) {
    if (id) {
        document.getElementById(id).style.display = "block";
    }
}

function hideElement(id) {
    /* Hides a document element.
     * arg id: string, the element id */
    if (id) {
        document.getElementById(id).style.display = "none";
    }
}

function getElementParent(id, lvl) {
    /* Gets the parent element n levels up the element tree.
     * arg id: string, the root element id
     * arg lvl: int, the number of levels to ascend
     * return: a html element */
    var elem = document.getElementById(id);
    for (var i = 0; i < lvl; i++) {
        if (elem != null && typeof elem !== 'undefined') {
            if (typeof elem.parentElement !== 'undefined') {
                elem = elem.parentElement;
            } else {
                return null;
            }
        } else {
            return null;
        }
    }
    return elem;
}

function getElementChildren(id, lvl) {
    /* Gets the children elements n levels down the element tree.
     * arg id: string, the root element id
     * arg lvl: int, the number of levels to descend
     * return: a list of html elements */
    var elems = [document.getElementById(id)];
    var elem = null;
    var temp = [];
    // Validity check
    if (elems[0] == null) {
        return [];
    } else if (elems[0].children.length <= 0) {
        return [];
    }
    // Traverse child tree for lvl steps
    for (var i = 0; i > lvl; i--) {
        for (var j = 0; j < elems.length; j++) {
            elem = elems[j];
            temp.push(elem.children);
        }

        temp = temp.filter(function (el) {
            return el != null && el != undefined;
        });

        if (temp.length == 0) {
            return [];
        } else {
            elems = temp;
            temp = [];
        }
    }
    return elems;
}

function displayElements(elemIds, show) {
    /* Renders elements.
     * arg elemIds: list of strings
     * arg show: boolean */
    for (var i = 0; i < elemIds.length; i++) {
        id = "#" + elemIds[i];

        if (show) {
            $(id).show();
        } else {
            $(id).hide();
        }
    }
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
    /* Add function description. */
    TargetUser = jQuery.parseJSON(data);
}

function appendElementOptions(id, obj) {
    /* Adds options from a json object to a html element.
     * arg id: the element id
     * arg obj: a json object with key-value pairs */
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

function budgetHourCallback(module, taskNo, certType) {
    /* Add function description.
     * arg module: string, the Bridge module
     * arg taskNo: int, the task number
     * arg certType: string, the certificate type */
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
    /* Calculates the budget hours based on the certificate type.
    :arg data: string, data in JSON format */
    var f = jQuery.parseJSON(data);
    var dis = (1 - f.allocationFee) * (1 - f.tsa - f.msa);
    var feee = document.getElementById("Fee").value;
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

function SetupDropdownList(dropdownid, dropdownsourcelist, propName) {
    if (propName == null) {
        $.each(dropdownsourcelist, function (key, data) {
            data = data;
            $("#" + dropdownid).append(new Option(data, data));
        });
    } else {
        $.each(dropdownsourcelist, function (key, data) {
            data = data;
            var s = eval("data." + propName);
            $("#" + dropdownid).append(new Option(s, s));
        });
    }

}

function isValidJQueryEvent(event) {
    /* Checks if an event is a valid jQuery event.
     * arg event: string
     * return: boolean */
    var eventOptions = ["click", "dblclick", "mouseenter", "mouseleave",
        "keypress", "keydown", "keyup", "submit", "change", "focus",
        "blur", "load", "resize", "scroll", "unload"];

    return arrayHasElement(eventOptions, event);
}

function htmlElementExists(tag) {
    /* Checks if a html element exists. 
     * arg tag: string, jQuery tag (# + id) */
    return Boolean($(tag).length);
}

function htmlElementIsSelect(tag) {
    /* Checks if a html element is a select element.
     * arg tag: string, jQuery tag (# + id) */
    return $(tag).is('select');
}

function initializeDropdownMenu(tag, itemList, attribute) {
    /* Populates a dropdown menu with options from an item list.
     * arg tag: string, jQuery tag (# + id)
     * arg itemList: list of objects
     * arg attribute: string, object attribute name */
    if (!htmlElementExists(tag)) {
        console.log("Html element with tag " + tag + " does not exist...");
        return;
    } else if (!htmlElementIsSelect(tag)) {
        console.log("Html element with tag " + tag + " is not a select element...");
        return;
    }
    // Extract valid options
    var selected = $(tag + " :selected").val();
    var item = null;
    var optionList = [];
    for (var i in itemList) {
        item = itemList[i];
        if (!item.hasOwnProperty(attribute)) {
            continue;
        } else if (item[attribute] == selected) {
            continue;
        } else {
            optionList.push(item[attribute]);
        }
    }
    // Remove duplicates
    optionList = arrayRemoveDuplicates(optionList);
    // Add options to dropdown menu
    for (var i in optionList) {
        $(tag).append(new Option(optionList[i], optionList[i]));
    }
    // Sort dropdown menu options
    sortDropdownMenuOptions(tag);
}

function updateDropdownMenu(tag, excludes) {
    /* Enables and shows select options whos text is not in excludes.
     * arg tag: string, jQuery element tag (# + id)
     * arg excludes: list of strings */
    if (!htmlElementExists(tag)) {
        console.log("Html element with tag " + tag + " does not exist...");
        return;
    } else if (!htmlElementIsSelect(tag)) {
        console.log("Html element with tag " + tag + " is not a select element...");
        return;
    }
    excludes = arrayToLower(excludes);
    var selected = $(tag + " :selected").val();
    var isInvalid = arrayHasElement(excludes, selected.toLowerCase());
    var options = $(tag).find("option");
    var option = null;
    for (var i = 0; i < options.length; i++) {
        option = options[i];
        if (arrayHasElement(excludes, option.text.toLowerCase())) {
            //option.hidden = true;
            option.disabled = true;
        } else {
            if (isInvalid) {
                $(tag).val(option.value);
                isInvalid = false;
            }
            //option.hidden = false;
            option.disabled = false;
        }
    }
}

function sortDropdownMenuOptions(tag) {
    /* Sort the options of a dropdown menu in alphabetical order.
     * arg tag: string */
    if (!htmlElementExists(tag)) {
        console.log("Html element with tag " + tag + " does not exist...");
        return;
    }
    var menu = $(tag);
    menu.html(menu.find("option").sort(function (x, y) {
        return $(x).text() > $(y).text() ? 1 : -1;
    }));
}

function getDropdownMenuValues(tag, onlyVisible) {
    /* Returns the option values from a dropdown menu.
     * arg tag: string
     * arg onlyVisible: boolean
     * return: list */
    var values = [];
    $(tag + " option").each(function () {
        if (onlyVisible) {
            if (!$(this).hidden) {
                values.push($(this).val());
            }
        } else {
            values.push($(this).val());
        }
    });
    return values;
}

function getDropdownSelected(tag) {
    /* Returns the value of the selected dropdown option.
     * arg tag: string 
     * return: - */
    return $(tag).find(":selected").text();
}