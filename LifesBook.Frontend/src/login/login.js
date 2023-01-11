const ipcRenderer = require('electron').ipcRenderer;

function loadHistoriesWindow(){
    
    var password = document.getElementById("passwordText").value;   
    ipcRenderer.send('load-histories-window', password);
} 