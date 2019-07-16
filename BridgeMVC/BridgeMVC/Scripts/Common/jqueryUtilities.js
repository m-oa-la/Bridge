function resizeIframe(obj) {
    /*
    Resizes an <iframe> window.
    :arg obj: an <iframe> object
    */
    obj.style.height = obj.contentWindow.document.body.scrollHeight + 'px';
}

function showElement(id, lvl) {
    if (id) {
        document.getElementById(id).style.display = "block";
    }
}

function hideElement(id, lvl) {
    /*
    Hides a document element.
    :arg id: string, the element id
    */
    if (id) {
        document.getElementById(id).style.display = "none";
    }
}

function getElementParent(id, lvl) {
    /*
    Gets the parent element n levels up the element tree.
    :arg id: string, the root element id
    :arg lvl: int, the number of levels to ascend
    :return: a html element
    */
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
    /*
    Gets the children elements n levels down the element tree.
    :arg id: string, the root element id
    :arg lvl: int, the number of levels to descend
    :return: a list of html elements
    */
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
    /*
    Renders elements
    :arg elemIds: list of strings, the element ids
    :arg show: boolean, whether the elements shall be shown (default: true)
    */
    
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
    /*
    Add function description.
    */
    TargetUser = jQuery.parseJSON(data);
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