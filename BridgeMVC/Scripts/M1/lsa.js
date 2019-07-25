function findValidLSACertEntries(Job, cert) {
    /*
    Finds the number of chapter entries in a certificate.
    :arg cert: array of objects
    :return: dictionary of string array pairs
    */
    var entries = {};

    for (var i = 0; i < cert.length; i++) {
        var entry = cert[i];
        var isValid = false;

        if (entry.hasOwnProperty("Chapter") && entry.hasOwnProperty("Condition")) {
            var chapters = Object.keys(entries);
            var chapter = entry.Chapter;
            var condition = entry.Condition;

            if (condition != null) {
                isValid = eval(condition); // Job object is used here
            } else {
                isValid = true;
            }

            if (isValid && chapters.includes(chapter)) {
                entries[chapter].push(entry);
            } else if (isValid) {
                entries[chapter] = [entry];
            }
        }
    }

    return entries;
}

function readLSACert(Job, cert) {
    /*
    Reads the information from a BLSACert and formates it into
    HTML paragraphs.
    :arg job: json object
    :arg cert: array of json objects
    */
    var bulletPoint = "	•	";
    var entries = findValidLSACertEntries(Job, cert);
    var chapters = Object.keys(entries);
    var chapter;
    var paragraphs;
    var paragraph;
    var priority;
    var line;

    for (var i = 0; i < chapters.length; i++) {
        chapter = chapters[i];
        paragraphs = entries[chapter];
        (priorities = []).length = paragraphs.length;
        priorities.fill(-1);

        // Find the paragraph priorities
        for (var j = 0; j < paragraphs.length; j++) {
            paragraph = paragraphs[j];
            priority = Number(paragraph.BookMarkName.split("_")[1]);
            priorities[j] = priority;
        }

        sortingIndices = findSortingIndices(priorities);

        // Pick paragraph according to the sorting indices
        for (var j = 0; j < sortingIndices.length; j++) {
            k = sortingIndices[j];
            paragraph = paragraphs[k];

            try {
                line = eval(paragraph.Formula);
            } catch (err) {
                line = "";
            }


            if (line.startsWith(bulletPoint)) {
                line = line.slice(bulletPoint.length);
                $("#" + chapter).append("<ul><li>" + line + "<br>");
            } else {
                $("#" + chapter).append(line + "<br>");
            }
        }
    }
}

function renderLSAParagraphs(certType, certAction) {
    /*
    Renders paragraphs based on type and action of a LSA
    certificate.
    :arg certType: string, certificate type
    :arg certAction: string, certificate action
    */
    var elemIds = getLSADisplayElements(certType, certAction);
    displayElements(elemIds[0], show = true);
    displayElements(elemIds[1], show = false);
}

function getLSADisplayElements(certType, certAction) {
    /*
    Gets the ids of the elements that should be shown.
    :arg certType: string, the certificate type
    :arg certAction: string, the certificate action
    :return: list of lists of strings, the element ids
    */

    // Default element ids to hide.
    var toHide = ["paraMEDFBNo", "paraMEDFBDue", "paraSerialNo", "paraCertAmount",
        "paraMWL", "paraMEDItemNo", "paraExistingCertNo", "paraSurveyStation",
        "paraSurveyDate", "paraModificationDesc"];
    var toShow = [];
    var extension = null;

    // Get display element ids based on certificate type
    extension = getCertificateTypeDisplayElements(certType);
    toShow.push.apply(toShow, extension);
    // Get display element ids based on certificate action
    extension = getCertificateActionDisplayElements(certAction);
    toShow.push.apply(toShow, extension);

    toHide = arrayApplyFilter(toHide, toShow);
    return [toShow, toHide];
}

function getLSACertTypeDisplays(certType) {
    /*
    Submethod of getLSADisplayElements. Returns element ids
    based on the certificate type.
    :arg certType: string, certificate type
    :return: list of strings, element ids
    */
    var elemIds = [];

    switch (certType) {
        case "MED-F":
            elemIds = ["paraMEDFBNo", "paraMEDFBDue", "paraSerialNo", "paraCertAmount",
                "paraMEDItemNo", "paraSurveyStation", "paraSurveyDate"];
            break;
        case "MED-G":
            elemIds = ["paraSerialNo", "paraMWL", "paraMEDItemNo"];
            break;
        case "MED-B":
            elemIds = ["paraMWL", "paraMEDItemNo"];
            break;
        case "TA":
            elemIds = ["paraMWL"];
            break;
        case "DVR":
            elemIds = ["paraSerialNo", "paraMWL"];
            break;
        case "MED-D":
            elemIds = ["paraMWL"];
            break;
        default:
            break;
    }

    return elemIds;
}

function getLSACertActionDisplays(certAction) {
    /*
    Submethod of getLSADisplayElements. Returns element ids
    to be shown based on the certificate action.
    :arg certAction: string, certificate action
    :return: list of string, element ids
    */
    var elemIds = [];

    switch (certAction) {
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