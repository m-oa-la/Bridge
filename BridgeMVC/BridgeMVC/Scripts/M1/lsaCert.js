function readLSACert(Job, cert) {
    /*
    Reads the information from a BLSACert and formates it into
    HTML paragraphs.
    :arg job: object
    :arg cert: array of objects
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
            line = eval(paragraph.Formula);

            if (line.startsWith(bulletPoint)) {
                line = line.slice(bulletPoint.length);
                $("#" + chapter).append("<ul><li>" + line + "<br>");
            } else {
                $("#" + chapter).append(line + "<br>");
            }
        }
    }
}

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