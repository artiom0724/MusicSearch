var myHub;
window.onload = function () {    
    var myHub = $.connection.myHub;
    $.connection.hub.logging = true;
    $.connection.hub.start();
}

$(function () {
    var myHub = $.connection.myHub;
    $.connection.hub.logging = true;
    $.connection.hub.start();


    myHub.client.SendServerTime = function (serverTime) {
        $("#newTime").text(serverTime);
    };

});