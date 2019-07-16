function renderTask1Paragraphs(certType, certAction) {
    /* Renders the relevant paragraphs for task 1. */
    var elemIds = getTask1DisplayElements(certType, certAction);
    displayElements(elemIds[0], show = true);
    displayElements(elemIds[1], show = false);
}

function getTask1DisplayElements() {
    /* Add function description. */
}