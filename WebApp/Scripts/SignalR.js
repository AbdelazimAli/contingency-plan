$(function () {


    var myHub = $.connection.myHub;
    //make connection to signalR
    $.connection.hub.start().done(function () {
            //  //invoke my hub server function , send data 
            //  var arr = [];
            //  arr.push("connected!!");
            //  arr.push("Hi Everone");
            //  arr.push("Bye Everyone");
            //  //myHub.server.announce(arr);
            ////  myHub.server.sendMessage(arr);
        }).fail(function () { });

    //client side function

    myHub.client.sendMessage = function (context) {
        // alert("Hi");
        alert(context);
        // writeToPage(context);
   };


    //$.connection.myHub.client.announce = function (message) {
    //    writeToPage(message);
    //    writeToNotify(message);
    //}

    var writeToPage = function (message) {
        for (var i = 0; i < message.length; i++)
            $("#welcome-message").append(message[i] + "<br/>");
    }

    //var writeToNotify = function (message) {
    //    drawItems('notification', message);
    //}
});