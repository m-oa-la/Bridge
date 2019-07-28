//define local constant values
//LProduct: list of product belongs to this job
//LPTP: list of tech. para.s for certain prodName
var _classth = "CellTh";
var _classtd = "CellContent";

function ptpCreateEditableTable(tableName) {
    var prodName = tableName.split("ø")[1];
    ptpCreateTH(tableName, prodName);
    ptpListProduct(tableName, prodName);
}

function ptpCreateTH(tableName, prodName) {
    var doc = document;
    var tbl = doc.getElementById(tableName);
    var th = doc.createElement("tr");
    th.classList.add(_classth);

    $.each(LPTP, function (key, data) {
        if (data.ProdName === prodName) {
            var td = doc.createElement("td");
            td.innerHTML = data.TechParaName;
            th.appendChild(td);
        }
    });
    tbl.appendChild(th);
}

/*
 * list all product
 * tableName: the target table id
 * prodName: the Product name
*/
function ptpListProduct(tableName, prodName) {
    var doc = document;
    var tbl = doc.getElementById(tableName);
    $.each(LProduct, function (key, prodData) {
        if (prodData.ProdName === prodName) {
            var tr = doc.createElement("tr");

            $.each(prodData.PTPs, function (key, ptpData) {
                var td = doc.createElement("td");
                var textarea = document.createElement("textarea");
                textarea.setAttribute("id", "saveProdø" + key + "ø" + prodData.Id);
                textarea.value = ptpData.TechParaValue;
                textarea.setAttribute("class", "form-control");

                td.setAttribute("class", "tblcell");
                td.appendChild(textarea);
                tr.appendChild(td);
            });

            var btnRemove = doc.createElement("button");
            btnRemove.innerHTML = "-";
            btnRemove.setAttribute("id", "removeProdø" + key + "ø"+ prodData.Id);
            btnRemove.setAttribute("class", "btn-danger ptpbtn");
            tr.appendChild(btnRemove);
            tbl.appendChild(tr);
        }
    });

}


$(document).ready(function () {
    //auto save Customer contact information when value is changed
    $('select[name^=ddedø]').change(function () {
        var navn = $(this).attr('name');
        var selectedv = $(this).find(":selected").text();
        var s = (selectedv + ";").split(";");
        $("#" + navn).val(s[0]);
        if (s.length > 2) {
            $("#dded_CustomerPhoneNo").val(s[1]);
            $("#dded_CustomerEmail").val(s[2]);
        }
        saveCustomer();
    });

    //Direct edit the value of customer contact as free text
    $('input[id^=ddedø]').change(function () {
        saveCustomer();
    });

    //add a new product
    $('button[id^=addProdø]').click(function () {
        var prodName = this.id.split("ø")[1];
        addProdbyProdName(Job.Id, prodName);
        window.location.reload(false);
    });

    $('button[id^=removeProdø]').click(function () {
        var prodid = this.id.split("ø")[1];
        DeleteProdById(prodid);
        window.location.reload(false);
    });
     //Change para. input when corresponding dropdown box selected changes
    $('select[id^=PEDDDESelectø]').change(function () {
        var s = this.id;
        var newValue = $(this).find(":selected").text();

        var prodid = s.split("ø")[2];
        var sn = s.split("ø")[1];

        $("#saveProdø" + sn + "ø" + prodid).val(newValue);
        SavePTPBySN(prodid, sn, newValue);
  
    });
    //Save product when value changes
    $('textarea[id^=saveProdø]').change(function () {

        var s = this.id;
        var prodid = s.split("ø")[2];
        var sn = s.split("ø")[1];
        var newValue = $("#" + s).val();
        SavePTPBySN(prodid, sn, newValue);
    });

});

function addProdbyProdName(jobid, prodName) {
    return $.ajax({
        type: 'POST',
        url: '/Job/AddPEDProductByProdName/',
        data: { jobid: jobid, prodName: prodName },
        cache: false,
        success: {
        }
    });
}

function DeleteProdById(prodid) {
    return $.ajax({
        type: 'POST',
        url: '/Job/DeleteProdById/',
        data: { prodid: prodid },
        cache: false,
        success: {
        }
    });
}

function SavePTPBySN(prodid, sn, newValue) {
    return $.ajax({
        type: 'POST',
        url: '/Job/SavePTPBySN/',
        data: { prodid: prodid, sn: sn, newValue: newValue },
        cache: false,
        success: {
        }
    });

}

function addDropdownEdit() {
    var doc = document;
    $.each(LPTP, function (keyptp, dataptp) {
        if ((dataptp.ValueSource + "x").includes("DropdownEdit_")) {

            $.each(LProduct, function (keyprod, dataprod) {
                if (dataprod.ProdName === dataptp.ProdName) {
                    //elem: the input filed. 
                    //assing PEDDDEInput class for it
                    elementid = "saveProdø" + dataptp.ViewSequence + "ø" + dataprod.Id;
                    var elem = doc.getElementById(elementid);
                    elem.setAttribute("class", "PEDDDEInput form-control");

                    //dde:  dropdwon editable select
                    var dde = doc.createElement("select");
                            var opt = doc.createElement("option");
                            opt.value = " ";
                            opt.text = " ";
                            dde.appendChild(opt);

                    $.each(LValueSource, function (key, data) {
                            if (data.ListType === dataptp.ValueSource) {
                            var opt = doc.createElement("option");
                            opt.value = data.ListItem;
                            opt.text= data.ListItem;
                            dde.appendChild(opt);
                        }
                    });

                    dde.setAttribute("class", "PEDDDESelect");
                    dde.setAttribute("id", "PEDDDESelectø" + + dataptp.ViewSequence + "ø" + dataprod.Id);
                    var td = elem.parentElement;
                    td.setAttribute("style", "max-height:38px");
                    td.insertBefore(dde, td.childNodes[0]);


                }
            });

       
        }
    });
}