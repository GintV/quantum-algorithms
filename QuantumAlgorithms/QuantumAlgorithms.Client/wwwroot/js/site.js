
//$(document).ready(function () {
//    $("#success-alert").hide();
//    $("#info-alert").hide();
//    $("#danger-alert").hide();
//    //$("#myWish").click(function showAlert() {
//    //    $("#success-alert").fadeTo(2000, 500).slideUp(500, function () {
//    //        $("#success-alert").slideUp(500);
//    //    });
//    //});
//});

function showSuccessAlert() {
    //$("#success-alert").fadeTo(4000, 1000).slideUp(1000, function () {
    //    $("#success-alert").slideUp(1000);
    //});
    $("#success-alert").show();
}

function showInfoAlert() {
    $("#info-alert").fadeTo(4000, 1000).slideUp(1000, function () {
        $("#info-alert").slideUp(1000);
    });
}

function showDangerAlert() {
    //$("#danger-alert").fadeTo(4000, 1000).slideUp(1000, function () {
    //    $("#danger-alert").slideUp(1000);
    //});
    $("#danger-alert").show();
}

var repeat = true;

function getStatusAuth(url, page = 1, deepClone = false) {
    /*var pageSize = 2;
    var statsHeader = resource + "s";
    if (statsHeader === "Countrys")
        statsHeader = "Countries";
    if (!deepClone && document.getElementById('statsHeader').innerHTML === statsHeader)
        return;*/

    if (!repeat)
        return repeat;

    $.get(
        "/Api/AccessToken",
        function (token) {
            $.ajaxSetup({
                headers: {
                    'Authorization': "Bearer " + token,
                    'Content-Type': "application/json",
                    'Access-Control-Allow-Origin': true
                }
            });

            $.get(
                url,
                function (data) {
                    getStatusCore(url, data);
                }
            );
        }
    );

    return repeat;
}

function getStatus(url, page = 1, deepClone = false) {
    /*var pageSize = 2;
    var statsHeader = resource + "s";
    if (statsHeader === "Countrys")
        statsHeader = "Countries";
    if (!deepClone && document.getElementById('statsHeader').innerHTML === statsHeader)
        return;*/

    if (!repeat)
        return repeat;

    $.ajaxSetup({
        headers: {
            'Content-Type': "application/json",
            'Access-Control-Allow-Origin': true
        }
    });

    $.get(
        url,
        function (data) {
            getStatusCore(url, data);
        }
    );

    return repeat;
}

function getStatusCore(url, data) {
    //console.log(data['status']);

    //console.log(data['messages']);

    var printedMessages = document.getElementById('messages');
    var allMessages = data['messages'];

    //console.log(printedMessages.childElementCount);
    //console.log(allMessages.length);

    var panelHeading = document.createAttribute("class");
    panelHeading.value = "panel-heading";
    var panelBody = document.createAttribute("class");
    panelBody.value = "panel-body";

    //var style = document.createAttribute("style");
    //style.value = "display:none";

    var locale = window.navigator.userLanguage || window.navigator.language;
    moment.locale(locale);

    var update = false;
    for (var i = printedMessages.childElementCount; i < allMessages.length; ++i) {
        update = true;
        printedMessages = document.getElementById('messages');
        var newPrintedMessages = printedMessages.cloneNode(true);

        var mainDiv = document.createElement('div');
        var mainClass = document.createAttribute("class");
        var mainId = document.createAttribute("id");
        mainId.value = "message " + i;

        if (allMessages[i]["severity"] === 0 && i === allMessages.length - 1 && (data["status"] === 2 || data["status"] === 3))
            mainClass.value = "panel panel-success";
        else if (allMessages[i]["severity"] === 0)
            mainClass.value = "panel panel-info";
        else if (allMessages[i]["severity"] === 1)
            mainClass.value = "panel panel-warning";
        else if (allMessages[i]["severity"] === 2)
            mainClass.value = "panel panel-danger";

        mainDiv.setAttributeNode(mainClass.cloneNode(true));
        //mainDiv.setAttributeNode(style.cloneNode(true));
        mainDiv.setAttributeNode(mainId.cloneNode(true));

        var panelHeadingDiv = document.createElement('div');
        panelHeadingDiv.setAttributeNode(panelHeading.cloneNode(true));
        panelHeadingDiv.innerHTML = moment(allMessages[i]['timeStamp']).format("L") + " " + moment(allMessages[i]['timeStamp']).format("LTS");

        var panelBodyDiv = document.createElement('div');
        panelBodyDiv.setAttributeNode(panelBody.cloneNode(true));
        panelBodyDiv.innerHTML = parseMessage(allMessages[i]['message']);
        //panelBodyDiv.innerHTML = allMessages[i]['message'];

        mainDiv.appendChild(panelHeadingDiv);
        mainDiv.appendChild(panelBodyDiv);

        //console.log(mainDiv);

        newPrintedMessages.insertAdjacentElement("afterbegin", mainDiv);//appendChild(mainDiv);

        printedMessages.parentNode.replaceChild(newPrintedMessages, printedMessages);
        //$("#message " + i).fadeIn("slow");
    }

    if (data["status"] === 0 || data["status"] === 1) {
        if (update) {
            showInfoAlert();
        }
    } else {
        if (data["status"] === 4) {
            showDangerAlert();
        } else if (data["status"] === 2 || data["status"] === 3) {
            showSuccessAlert();
        }

        repeat = false;

        var row = document.createAttribute("class");
        row.value = "row";

        document.getElementById('statusString').innerHTML =
            "<label class='col-md-5'>Status:</label>" + data['statusString'];
        var details = document.getElementById('details');
        var newDetails = details.cloneNode(true);

        var finishTimeDiv = document.createElement('div');
        finishTimeDiv.setAttributeNode(row.cloneNode(true));

        var finishTime = moment(data['finishTime']).format("L") + " " + moment(data['finishTime']).format("LTS");
        finishTimeDiv.innerHTML = "<div class='form-group' id='finishTime'>" +
            "<label class='col-md-5'>Finish Time:</label>" +
            finishTime +
            "</div>";
        newDetails.appendChild(finishTimeDiv);

        details.parentNode.replaceChild(newDetails, details);

        if (data["status"] !== 4 && data["status"] !== 5) {
            var output = document.getElementById('output');
            var newOutput = output.cloneNode(true);
            if (url.includes("IntegerFactorization")) {

                var pDiv = document.createElement('div');
                pDiv.setAttributeNode(row.cloneNode(true));

                pDiv.innerHTML = "<div class='form-group' id='finishTime'>" +
                    "<label class='col-md-5'>Factor (P):</label>" +
                    data['output']['p'] +
                    "</div>";
                newOutput.appendChild(pDiv);

                var qDiv = document.createElement('div');
                qDiv.setAttributeNode(row.cloneNode(true));

                qDiv.innerHTML = "<div class='form-group' id='finishTime'>" +
                    "<label class='col-md-5'>Factor (Q):</label>" +
                    data['output']['q'] +
                    "</div>";
                newOutput.appendChild(qDiv);

                output.parentNode.replaceChild(newOutput, output);
            } else if (url.includes("DiscreteLogarithm")) {

                var dlDiv = document.createElement('div');
                dlDiv.setAttributeNode(row.cloneNode(true));

                dlDiv.innerHTML = "<div class='form-group' id='discreteLogarithm'>" +
                    "<label class='col-md-7'>Discrete logarithm:</label>" +
                    data['output']["discreteLogarithm"] +
                    "</div>";
                newOutput.appendChild(dlDiv);
            }
        }
    }
}

function parseMessage(message) {
    message = message.split("*").join("&sdot;");
    message = message.split("/").join("&frasl;");
    message = message.split("=>").join("&rarr;");

    var powerIndex;
    while ((powerIndex = message.indexOf('^')) !== -1) {
        var subPower = message.substring(powerIndex, message.indexOf(')', powerIndex) + 1);
        message = message.split(subPower).join("<sup>" + subPower.substring(2, subPower.length - 1) + "</sup>");
        //console.log(subPower);
        //console.log(subPower.substring(2, subPower.length - 1));
    }

    return message;
}

var page = 1;

function getSolutions(url, meniuItem, deepClone = false) {
    var pageSize = 10;
    //var statsHeader = resource + "s";
    //if (statsHeader === "Countrys")
    //    statsHeader = "Countries";
    //if (!deepClone && document.getElementById('statsHeader').innerHTML === statsHeader)
    //    return;

    var statuses = "";

    if (meniuItem === 1)
        statuses = "0";
    else if (meniuItem === 2)
        statuses = "1";
    else if (meniuItem === 3)
        statuses = "[2, 3]";
    else if (meniuItem === 4)
        statuses = "4";
    else if (meniuItem === 5)
        statuses = "5";
    else
        statuses = meniuItem;

    $("#prev").hide();
    $("#next").hide();
    $("#spinner").show();

    $.get(
        "/Api/AccessToken",
        function (token) {
            $.ajaxSetup({
                headers: {
                    'Authorization': "Bearer " + token,
                    'Content-Type': "application/json",
                    'Access-Control-Allow-Origin': true
                }
            });

            $.get(
                url + "?page=" + page + "&pageSize=" + pageSize + "&statuses=" + statuses,
                function (data) {

                    $("#spinner").hide();

                    if (data.length === 0) {
                        $("#solution-alert").show();
                    }

                    if (page !== 1) {
                        $("#prev").show();
                    }

                    if (data.length === pageSize) {
                        $("#next").show();
                    }

                    if (data.length === 0) {
                        page = page - 1;
                        return;
                    }

                    //document.getElementById('statsHeader').innerHTML = statsHeader;

                    //var titleString = 'title';
                    //document.getElementById('titleString').innerHTML = "Title";
                    //if (resource === "Player") {
                    //    titleString = 'nickname';
                    //    document.getElementById('titleString').innerHTML = "Nickname";
                    //}
                    var oldBody = document.getElementById('statsBody');
                    var newBody = oldBody.cloneNode(deepClone);

                    while (newBody.firstChild) {
                        newBody.removeChild(newBody.firstChild);
                    }

                    //data = JSON.stringify(data);
                    console.log(data);
                    //console.log(data.size);

                    var locale = window.navigator.userLanguage || window.navigator.language;
                    moment.locale(locale);

                    var style = document.createAttribute("style");
                    style.value = "line-height: 40px; min-height: 40px; height: 40px";

                    var positionPlus = pageSize * (page - 1) + 1;
                    for (var i = 0; i < data.length; ++i) {
                        var tr = document.createElement('tr');
                        tr.setAttributeNode(style.cloneNode(true));

                        var tdPos = document.createElement('td');
                        var colspanPos = document.createAttribute("colspan");
                        colspanPos.value = 1;
                        tdPos.appendChild(document.createTextNode(i + positionPlus));
                        tdPos.setAttributeNode(colspanPos);
                        tr.appendChild(tdPos);

                        var solutionValue, detailsLink;
                        if (typeof data[i]["input"]["number"] === 'undefined') {
                            solutionValue = "Discrete logarithm";
                            detailsLink = "/Solve/DiscreteLogarithm/" + data[i]["id"];
                        } else {
                            solutionValue = "Integer factorization";
                            detailsLink = "/Solve/IntegerFactorization/" + data[i]["id"];
                        }

                        var tdSolution = document.createElement('td');
                        var colspanSolution = document.createAttribute("colspan");
                        colspanSolution.value = meniuItem > 2 ? 4 : 5;
                        tdSolution.appendChild(document.createTextNode(solutionValue));
                        tdSolution.setAttributeNode(colspanSolution);
                        tr.appendChild(tdSolution);

                        var tdStartTime = document.createElement('td');
                        var colspanStartTime = document.createAttribute("colspan");
                        colspanStartTime.value = meniuItem > 2 ? 4 : 5;
                        var startTimeValue = moment(data[i]['startTime']).format("L") + " " + moment(data[i]['startTime']).format("LTS");
                        tdStartTime.appendChild(document.createTextNode(startTimeValue));
                        tdStartTime.setAttributeNode(colspanStartTime);
                        tr.appendChild(tdStartTime);

                        if (meniuItem > 2) {
                            var tdDuration = document.createElement('td');
                            var colspanDuration = document.createAttribute("colspan");
                            colspanDuration.value = 2;
                            var duration = moment.duration(moment(data[i]['finishTime']).diff(moment(data[i]['startTime'])));
                            var durationValue = moment.utc(duration.asMilliseconds()).format('HH:mm:ss');
                            tdDuration.appendChild(document.createTextNode(durationValue));
                            tdDuration.setAttributeNode(colspanDuration);
                            tr.appendChild(tdDuration);
                        }

                        var tdDetailsLink = document.createElement('td');
                        var colspanDetailsLink = document.createAttribute("colspan");
                        colspanDetailsLink.value = 1;
                        var detailsLinkValue = "<a href='" + detailsLink + "'<span class='glyphicon glyphicon-info-sign'></span></a>";
                        tdDetailsLink.innerHTML = detailsLinkValue;
                        tdDetailsLink.setAttributeNode(colspanDetailsLink);
                        tr.appendChild(tdDetailsLink);

                        newBody.appendChild(tr);
                    }

                    oldBody.parentNode.replaceChild(newBody, oldBody);

                    //console.log('page content: ' + JSON.stringify(data));

                    //if (data.length < pageSize)
                    //    document.getElementById("loadMoreButtonPlace").innerHTML = "";
                    //else {
                    //    var funcCall = "getResources('" + resource + "'" + "," + (page + 1) + "," + true + ")";
                    //    //console.log(funcCall);mo
                    //    document.getElementById("loadMoreButtonPlace").innerHTML =
                    //        "<div class='col-md-2 col-md-offset-5'>" +
                    //        "<button class='btn btn-primary btn-block' onclick=" + funcCall + ">Load more</button>" +
                    //        "</div>";
                    //}
                }
            );
        }
    );
}

function clockFunction() {
    var locale = window.navigator.userLanguage || window.navigator.language;
    moment.locale(locale);
    var update;
    (update = function () {
        document.getElementById("clock")
            .innerHTML = moment().format("L") + " " + moment().format("LTS");
    })();
    setInterval(update, 1000);
}