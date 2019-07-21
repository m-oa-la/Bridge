

function refreshCertText(Job, Product, LAutoCertText) {
    //Job: current job
    //Product: first product
    //LautoCertText: autotxt filtered by MEDItemNo. of Job
    var ct = document.getElementById("CertText");
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
                p.innerText = eval(formula);
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

function setupAutoFillingField(LVauleResource, LAutoCertText){

}