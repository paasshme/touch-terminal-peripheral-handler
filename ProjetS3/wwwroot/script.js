function oui() {
    var getWebSocketMessages = function(onMessageReceived)
    {
        var url = `ws://localhost:5001/ws`
        console.log('url is: ' + url);

        var webSocket = new WebSocket(url);

        webSocket.onmessage = onMessageReceived;
    };

    var ulElement = document.getElementById('StreamToMe');

    getWebSocketMessages(function (message) {
        ulElement.innerHTML = ulElement.innerHTML += `<li>${message.data}</li>`
    });
}
oui();