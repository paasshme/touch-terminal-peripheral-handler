
const port = 5001;
const socket = new WebSocket(`ws://localhost:${port}/ws`);

const testButton = document.getElementById('UseMethod').addEventListener('click', process)
const displayZone = document.getElementById('displayZone');

async function process()
{
    let object = document.getElementById('ObjectName').value
    let method = document.getElementById('MethodName').value

    await fetch (`https://localhost:${port}/api/${object}/${method}`,parameters)
}

function consoleMessage(message) {
    let msg = document.createElement("p");
    msg.innerText = message;
    displayZone.appendChild(msg);
}
const parameters = {
    method: 'GET',
    headers: new Headers(),
    mode: 'no-cors',
    cache: 'default'
};

socket.onopen = (e) => {
    consoleMessage("[open] Connection established");
};

socket.onmessage = (event) => {
  consoleMessage(`[message] Data received from server: ${event.data}`);
};

socket.onclose = () => {
  consoleMessage("[WebSocketClosed]");
};
