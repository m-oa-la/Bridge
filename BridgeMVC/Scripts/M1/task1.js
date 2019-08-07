// Change event functions

function task1CertChangeEvent(certActionDropTag, certTypeDropTag,
    mainDropTag, subDropTag, medDropTag, certTypeParaTag, medParaTag) {
    var certType = $(certTypeDropTag).find(":selected").text();
    var certAction = $(certActionDropTag).find(":selected").text();
    var root = getElementParent(certTypeParaTag.substr(1), 1);
    var siblings = getElementChildren(root.id, -1)[0];

    var paragraphs = [];
    for (var i = 0; i < siblings.length; i++) {
        paragraphs.push(siblings[i].id);
    }

    paragraphs = arrayRemoveEmpties(paragraphs);

    // Get invalid paragraphs
    var paragraphsToShow = [];
    var paragraphsToHide = [];
    var extension = [];
    extension = task1CertTypeGetInvalidParagraphs(certType);
    paragraphsToHide.push.apply(paragraphsToHide, extension);
    extension = task1CertActionGetInvalidParagraphs(certAction);
    paragraphsToHide.push.apply(paragraphsToHide, extension);

    paragraphsToShow = arrayApplyFilter(paragraphs, paragraphsToHide);

    // Render paragraphs
    displayElements(paragraphsToShow, show = true);
    displayElements(paragraphsToHide, show = false);

    // Call dependent change event
    task1CertTypeChangeEvent(certTypeDropTag, mainDropTag,
        subDropTag, medDropTag, medParaTag);
}

function task1CertTypeChangeEvent(certTypeDropTag, mainDropTag,
    subDropTag, medDropTag, medParaTag) {
    /* Enables or disables the MED item, main - and sub equipment 
     * paragraphs whenever the certificate type is changed.
     * :arg certTypeDropTag: string, cert type dropdown tag
     * :arg mainDropTag: string, main type dropdown tag
     * :arg subDropTag: string, sub type dropdown tag
     * :arg medDropTag: string, med dropdown tag
     * :arg mainParaTag: string, main type paragraph tag
     * :arg subParaTag: string, sub type paragraph tag
     * :arg medParaTag: string, med paragraph tag */
    var certType = $(certTypeDropTag).find(":selected").text();
    var medOptions = getDropdownMenuValues(medDropTag, false);
    var validMeds = task1CertTypeGetValidMeds(certType, medOptions);
    var invalidMeds = arrayApplyFilter(medOptions, validMeds);

    if (certType.toLocaleUpperCase().startsWith("MED")) {
        $(medParaTag).show();
        updateDropdownMenu(medDropTag, invalidMeds);
        task1MedChangeEvent(mainDropTag, subDropTag, medDropTag,
            mainParaTag, subParaTag);
    } else {
        $(medParaTag).hide();
        updateDropdownMenu(mainDropTag, []);
        updateDropdownMenu(subDropTag, []);
    }
}

function task1MedChangeEvent(mainDropTag, subDropTag, medDropTag,
    mainParaTag, subParaTag) {
    /* Enables the relevant main type - and sub type options based on
     * the currently selected MED item number. 
     * :arg mainDropTag: string, main type dropdown tag
     * :arg subDropTag: string, sub type dropdown tag
     * :arg medDropTag: string, med dropdown tag
     * :arg mainParaTag: string, main type paragraph tag
     * :arg subParaTag: string, sub type paragraph tag */
    var medNumber = $(medDropTag).find(":selected").text();
    var mainOptions = getDropdownMenuValues(mainDropTag, false);
    var subOptions = getDropdownMenuValues(subDropTag, false);
    var validMains = task1MedGetValidMains(medNumber);
    var validSubs = task1MedGetValidSubs(medNumber);
    var invalidMains = arrayApplyFilter(mainOptions, validMains);
    var invalidSubs = arrayApplyFilter(subOptions, validSubs);

    $(mainParaTag).show()
    $(subParaTag).show()

    if (medNumber == "N/A") {
        updateDropdownMenu(mainDropTag, []);
        updateDropdownMenu(subDropTag, []);
    } else {
        updateDropdownMenu(mainDropTag, invalidMains);
        updateDropdownMenu(subDropTag, invalidSubs);
    }
}

// TODO
function task1MainChangeEvent(mainDropTag, subDropTag, subItemList, subAttri) {
    var mainSelected = getDropdownSelected(mainDropTag);
    var allSubOptions = getDropdownMenuValues(subDropTag, false);
    var subObjects = getObjectsWithAttribute(subItemList, subAttri, mainSelected)
    var validSubOptions = getObjectAttributeValues(subObjects, "ListItem") // Incorrect output
    var invalidSubOptions = arrayApplyFilter(allSubOptions, validSubOptions);
    // arrayRemoveDuplicates()
    console.log("subObjects", subObjects);
    console.log("allSubOptions", allSubOptions);
    console.log("validSubOptions", validSubOptions);
    console.log("invalidSubOptions", invalidSubOptions);

    updateDropdownMenu(subDropTag, invalidSubOptions);
}

// Paragraph retrieval functions

function task1CertTypeGetInvalidParagraphs(certType) {
    var invalidParagraphs = [];
    switch (certType) {
        case "MED-B":
            invalidParagraphs = ["paraMEDFBNo", "paraMEDFBDue", "paraSerialNo",
                "paraCertAmount", "paraSurveyStation", "paraSurveyDate"];
            break;
        case "MED-D":
            invalidParagraphs = ["paraMEDFBNo", "paraMEDFBDue", "paraSerialNo",
                "paraCertAmount", "paraSurveyStation", "paraSurveyDate"];
            break;
        case "MED-F":
            invalidParagraphs = ["paraMWL"];
            break;
        case "MED-G":
            invalidParagraphs = ["paraMEDFBNo", "paraMEDFBDue", "paraCertAmount",
                "paraSurveyStation", "paraSurveyDate"];
            break;
        case "TA":
            invalidParagraphs = ["paraMEDFBNo", "paraMEDFBDue", "paraSerialNo",
                "paraCertAmount", "paraSurveyStation", "paraSurveyDate"];
            break;
        case "DVR":
            invalidParagraphs = ["paraMEDFBNo", "paraMEDFBDue", "paraCertAmount",
                "paraSurveyStation", "paraSurveyDate"];
            break;
        default:
            break;
    }

    return invalidParagraphs;
}

function task1CertActionGetInvalidParagraphs(certAction) {
    var invalidParagraphs = [];

    switch (certAction) {
        case "Modification":
            invalidParagraphs = [];
            break;
        case "Initial with reference":
            invalidParagraphs = ["paraModificationDesc"];
            break;
        case "Renewal with modification":
            invalidParagraphs = [];
            break;
        case "Renewal":
            invalidParagraphs = ["paraModificationDesc"];
            break;
        default:
            invalidParagraphs = ["paraExistingCertNo", "paraModificationDesc"];
            break;
    }

    return invalidParagraphs;
}

// Dropdown option retrieval functions

function task1CertTypeGetValidMeds(certType, medItems) {
    var valids = [];
    var invalids = [];
    switch (certType) {
        case "MED-B":
            invalids = ["N/A"];
            break;
        case "MED-D":
            invalids = [];
            break;
        case "MED-F":
            invalids = ["N/A"];
            break;
        case "MED-G":
            invalids = ["N/A"];
            break;
        default:
            break;
    }
    valids = arrayApplyFilter(medItems, invalids)
    return valids;
}

function task1MedGetValidMains(medNumber) {
    var valids = [];
    switch (medNumber) {
        default:
            valids = ["Life-Saving appliances"];
            break;
    }
    return valids;
}

function task1MedGetValidSubs(medNumber) {
    if (medNumber != "N/A") {
        var lastChar = medNumber.substr(medNumber.length - 1);
        var appendices = ["a", "b", "c", "d", "e", "f", "g"];
        if (appendices.includes(lastChar)) {
            medNumber = medNumber.slice(0, -1);
        }
    }

    var valids = [];
    switch (medNumber) {
        case "MED/1.21":
            valids = ["Launching appliance for life saving"];
            break;
        case "MED/1.23":
            valids = ["Launching appliance for life saving"];
            break;
        case "MED/1.24":
            valids = ["Launching appliance for life saving"];
            break;
        case "MED/1.25":
            valids = ["Launching appliance for life saving"];
            break;
        case "MED/1.26":
            valids = ["Release mechanism for life saving"];
            break;
        case "MED/1.41":
            valids = ["Winch for life saving"];
            break;
        default:
            break;
    }
    return valids;
}

// TODO
function task1MainGetValidSubs(mainType, subItems) {

}