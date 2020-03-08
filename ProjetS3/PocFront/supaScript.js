const port = 5001;
console.log(document.getElementById('stop'))

let socket = new WebSocket("wss://localhost:5001/ws");


const parameters = {
  method: 'GET',
               headers: new Headers(),
               mode: 'no-cors',
               cache: 'default' };

 async function process (port, object, method)  {
  console.log("aaaaaa")
  
  let res = await fetch (`https://localhost:${port}/api/${object}/${method}`,parameters)
  if (res === 200)
    console.log("yay")

}
//document.getElementById('start').addEventListener('click', process(port, 'RandomDevice', 'Start'))
document.getElementById('start').addEventListener('click', test(1))
function test(a)
{
  console.log(a)
}
socket.onopen = (e) => {
  console.log("[open] Connection established");
};

socket.onmessage = (event) => {
  console.log(`[message] Data received from server: ${event.data}`);
};

socket.onclose = () => {
  console.log("CLOSED");
};