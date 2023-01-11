const ipcRenderer = require('electron').ipcRenderer;

function closeWindow(){
        
    ipcRenderer.send('close-alert-window', '');
} 

ipcRenderer.on('alert-initialize',(event,data)=>{

   document.getElementById('title').innerHTML = data.errorSource;
   document.getElementById('alertText').innerHTML = data.message;
   
});