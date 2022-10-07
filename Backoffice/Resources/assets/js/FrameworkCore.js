var AjaxRequestError = "خطادرازتباط با سرور.لطفادوباره تلاش کنید";
function DownloadReport() {
    window.location.href = window.location.href.replace("Index", "Report");
}
function GetDate(selector) {
    georgianValuef = $("#datepicker_" + selector).data('datepicker').date.getGregorianDate();

    var dd = georgianValuef.getDate();
    var mm = georgianValuef.getMonth() + 1; //January is 0!

    var yyyy = georgianValuef.getFullYear();
    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }
    return yyyy + '-' + mm + '-' + dd;
}
function getFormData($form) {
    var unindexed_array = $form.serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}
function GetReport(url)
{
    $.ajax({
        url: url,
        success: function (e) {
           $.notific8(e, { life: 5000, horizontalEdge: "top", theme: "success" });
        },
        statusCode: { 404: ajaxError_404, 505: ajaxError_505 }, error: ajaxError_error
    });
}
function removeUrlParameter(url, parameter) {
    var urlParts = url.split('?');

    if (urlParts.length >= 2) {
        // Get first part, and remove from array
        var urlBase = urlParts.shift();

        // Join it back up
        var queryString = urlParts.join('?');

        var prefix = encodeURIComponent(parameter) + '=';
        var parts = queryString.split(/[&;]/g);

        // Reverse iteration as may be destructive
        for (var i = parts.length; i-- > 0;) {
            // Idiom for string.startsWith
            if (parts[i].lastIndexOf(prefix, 0) !== -1) {
                parts.splice(i, 1);
            }
        }

        url = urlBase + '?' + parts.join('&');
    }

    return url;
}
function UpdateRender(RowID, Url) {

    var urlParts = removeUrlParameter(window.location.href,"xID").split("?");
    var queryString = "";
    if (urlParts.length > 1) {
        queryString = urlParts[1];
    }
    
    Url = Url + "?" + "xID=" + RowID;
    if (queryString != "") {
        if (queryString[0] == "&") {
            Url = Url + queryString;
        }
        else {
            Url = Url + "&" + queryString;
        }
    }
    window.location.href = Url;


    /*Url = removeUrlParameter(Url,"xID");
    if (window.location.href.indexOf("?") < 0) {
        Url = Url + "?xID=" + RowID;
    }
    else {
        Url = Url + "&xID=" + RowID;
    }
    window.location.href = Url;*/


   /* var sepratorIndex = 0;
    if (window.location.href.indexOf("xID") == -1) {
        sepratorIndex = window.location.href.indexOf("?");
    }
    else {
        sepratorIndex = window.location.href.indexOf("&");
    }
    if (sepratorIndex == -1) {
        window.location.href = Url + "?xID=" + RowID;
    }
    else {
        window.location.href = Url + "?xID=" + RowID + "&" + window.location.href.substr(sepratorIndex + 1, window.location.href.length - sepratorIndex - 1);
    }*/
    
    
}

function Delete(RowID, Url) {
    var args = {
        "xID": RowID,
    }
    $.ajax({
        url: Url, data: JSON.stringify(args), type: "POST", dataType: 'json', contentType: "application/json; charset=utf-8",
        headers: {
            'RequestVerificationToken': TokenHeaderValue
        },
        success: function (e) {
            if (e.Status == true) {
                $.notific8(e.Message, { life: 5000, horizontalEdge: "top", theme: "success" });
                setTimeout("window.location.reload()",1000);
            }
            else {
                $.notific8(e.Message, { life: 5000, horizontalEdge: "top", theme: "danger" });
            }
        },
        statusCode: { 404: ajaxError_404, 505: ajaxError_505 }, error: ajaxError_error
    });
}

function NavigationRoute(Path, target, breadCrumbData) {
    canvasLoader();
    $.ajax({
        url: Path, type: "POST",
        success: function (data) {
            $(target).html(data);
            FillBreadCrumb(breadCrumbData);
            RemoveNotific8();
        },
        statusCode: { 404: ajaxError_404, 505: ajaxError_505 }, error: ajaxError_error
    });
}
function getStringifyEnum(targetElmID,total)
{
    var days = $("#" + targetElmID).val();
    var binaryString = "";
    for (var day in days)
    {
        binaryString = "1" + binaryString;
    }
    for (var index = total - binaryString.length; index > 0; index--)
        binaryString = "0" + binaryString;
    return binaryString;
}
function getSelectedKeywords()
{
    /*var str = '[';
    $(".keywordTag").each(function () {
        str += '"' + $(this).text() + '",';
    });
    if(str.length>1)
        str = str.substr(0, str.length - 1);
    str += ']';
    return str;*/
    var keywords = new Array();
    $(".keywordTag").each(function () {
        keywords.push($(this).text());
    });
    return keywords;

}
function SyncAjax(path, jsonStringData)
{
    var response;
    $.ajax({
        url: path, data: jsonStringData, type: "POST", dataType: 'json', contentType: "application/json; charset=utf-8", async: false,
        success: function (result) {
            response = result;
        },
        statusCode: { 404: ajaxError_404, 505: ajaxError_505 }, error: ajaxError_error
    });
    return response;
}

function showNotific(msg,theme)
{
    if (theme == undefined)
        theme = "warning"
    $.notific8(msg, { life: 6000, horizontalEdge: "top", verticalEdge: "right", theme: theme});
}
function canvasLoader()
{
    
    $.notific8('<div id="canvas_loading"></div>', {life:60000, horizontalEdge: "top", verticalEdge: "left", theme: "theme-inverse", heading: "در حال بازیابی اطلاعات ..." }, 1000);
    var throbber = new Throbber({ size: 32, padding: 17, strokewidth: 2.8, lines: 12, rotationspeed: 0, fps: 15 });
    throbber.appendTo(document.getElementById('canvas_loading'));
    throbber.start();

}
function RemoveNotific8()
{
    $(".jquery-notific8-notification").removeClass("in");
    setTimeout('$(".jquery-notific8-notification").remove()', 500);
}

function LoadAjax(Path,target,breadCrumbData)
{
    canvasLoader();
    $.ajax({
        url: Path, data: '', type: "POST", dataType: 'json',
        success: function (result) {
            RemoveNotific8();
            $(target).html(result.html);
            FillBreadCrumb(breadCrumbData);
        },
        statusCode: { 404: ajaxError_404, 505: ajaxError_505 }, error: ajaxError_error
    });
}
function FillBreadCrumb(breadCrumbData)
{
    var result = "";
    if (breadCrumbData != undefined) {
        var sections = breadCrumbData.split(",");
        for (var section in sections) {
            if(section==sections.length-1)
                result+="<li class='active'>"+sections[section]+"</li>";
            else
                result+="<li><a href='#'>"+sections[section]+"</a></li>";
        }   
    }
    $("#main .breadcrumb").html(result);
}

function LoadAjaxModal(Path, viewPath, header)
{
    var args = {
        "viewPath": viewPath,
        "header": header
    };
    $.ajax({
        url: Path, data: JSON.stringify(args), type: "Post", dataType: 'json', contentType: "application/json; charset=utf-8",
        success: function (result) {
            $("#md-ajax .modal-header h3").html(result.Header);
            $("#md-ajax .modal-body").html(result.Html);
            $("#md-ajax").modal();
        },
        statusCode: { 404: ajaxError_404, 505: ajaxError_505 }, error: ajaxError_error
    });
}



//AjaxErrorHandlers
    function ajaxError_404() {
        RemoveNotific8();
        setTimeout('$.notific8(AjaxRequestError, { life: 4000, horizontalEdge: "top", theme: "danger" });', 600);
    }
    function ajaxError_505() {
        RemoveNotific8();
        setTimeout('$.notific8(AjaxRequestError, { life: 4000, horizontalEdge: "top", theme: "danger" });', 600);
    }
    function ajaxError_error() {
        RemoveNotific8();
        setTimeout('$.notific8(AjaxRequestError, { life: 4000, horizontalEdge: "top", theme: "danger" });', 600);
    }
//End of Ajax Error Handlers