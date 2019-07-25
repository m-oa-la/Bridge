

function refreshCertText(Job, Product, LAutoCertText, eid) {
    //Job: current job
    //Product: first product
    //LautoCertText: autotxt filtered by MEDItemNo. of Job
    var ct = document.getElementById(eid);
    ct.innerHTML = "";
    var chapterName = "";
    var p = document.createElement("p");
    $.each(LAutoCertText, function (key, data) {
        if (data.Chapter !== chapterName) {
            chapterName = data.Chapter;
            writeCertChapter(chapterName);
            writeCertBodyText(data, Job, Product);
        } else {
            writeCertBodyText(data, Job, Product);
        }
    });

    highlightTxt("XX", eid);
}

function writeCertBodyText(data, Job, Product) {

    var condition = data.Condition;
    var formula = data.Formula;
    var conditionC;
    var conditionE;
 
    try {
       conditionC = compileProdProp(condition);
       conditionE = eval(conditionC);

    } catch(err){
        conditionE = false;
    }
 
    try {
        if (formula !== null) {
            formula = compileProdProp(formula);

            if (conditionE) {
                var ct = document.getElementById("CertText");
                var p = document.createElement("p");
                var v = eval(formula);
                p.innerText = v;
                p.innerHTML = tuneXXvalue(p.innerHTML);
                p.className = "certBodyText";
                ct.appendChild(p);
            }
        }
    } catch(err){
        console.log("Error:" + formula);
    }
 }

function writeCertChapter(chapterName) {
        var ct = document.getElementById("CertText");
        var p = document.createElement("p");
    p.innerText = chapterName;
    p.className = "certChapter";
        ct.appendChild(p); 
}

function compileProdProp(expression) {
    var s = expression;
    if (expression.includes("[")) {
        var arr = expression.split("[");
        $.each(arr, function (key, data) {
            if (data.includes("]")) {
                var prodprop = data.split("]")[0];
                s = s.replace("[" + prodprop + "]", '"' + getProdProp(Product, prodprop) + '"');
            }
        });
    }
    return s;
}

function getProdProp(Product, prodprop) {
    var s = "";
    var ptps = Product.PTPs;
    $.each(ptps, function (key, data) {
        if (data.TechParaName === prodprop) {
            s = data.TechParaValue;
            return s;
        }
    });

    return s;
}

function highlightTxt(text, eid) {
    var inputText = document.getElementById(eid);
    var innerHTML = inputText.innerHTML;
    var index = innerHTML.indexOf(text);
    var i = 0;
    while ( index >= 0) {
         innerHTML = innerHTML.substring(0, index) + "<span class='highlight'>" + innerHTML.substring(index, index + text.length) + "</span>" + innerHTML.substring(index + text.length);
        index = innerHTML.indexOf(text, index + 31 + text.length);
        i++;
    }
    inputText.innerHTML = innerHTML;

}

function tuneXXvalue(str) {
    var s = str;
    s = s.split("null").join("XX");
    s = s.split("xx").join("XX");
    s = s.split("xX").join("XX");
    s = s.split("Xx").join("XX");

    return s;
}

function setupAutoFillingField(LVauleResource, LAutoCertText){

}

/* Create tables according to datasource and insert them into parentdiv
 * datasource: certain type of data (one type of class/model) !!sorted by main attr!!
 * parentdivid: Id of parent div
 * tablenameattr: the main attr. which will turns to be name of the table
 * arrtobelisted: the arrtributes to be listed in the table. It is an !!array type!!.
 */

