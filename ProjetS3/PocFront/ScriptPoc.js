
const port = 5001;
const socket = new WebSocket(`wss://localhost:${port}/ws`);

const testButton = document.getElementById('UseMethod').addEventListener('click',process)

async function process()
{
    let object = document.getElementById('ObjectName').value
    let method = document.getElementById('MethodName').value

    await fetch (`https://localhost:${port}/api/${object}/${method}`,parameters)
}

const parameters = {
    method: 'GET',
    headers: new Headers(),
    mode: 'no-cors',
    cache: 'default'
};

socket.onopen = (e) => {
  console.log("[open] Connection established");
};

socket.onmessage = (event) => {
  console.log(`[message] Data received from server: ${event.data}`);
};

socket.onclose = () => {
  console.log("[WebSocketClosed]");
};