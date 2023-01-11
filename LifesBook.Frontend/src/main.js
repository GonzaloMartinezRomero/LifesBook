const {app, BrowserWindow, ipcMain} = require('electron');

let historyWindow = null;
let loginWindow = null;
 
app.on('ready',()=>{
    
    historyWindow = new BrowserWindow({
      show: false,
      webPreferences:{
        nodeIntegration: true,
        contextIsolation:false
      },
      width: 1250,
      height: 850,
      //resizable:false,      
      //autoHideMenuBar: true
    });

    historyWindow.loadFile(__dirname + '/history/history.html');

    historyWindow.on('closed',()=>{
      historyWindow = null;
    });

    historyWindow.once('ready-to-show',()=>{

          loginWindow = new BrowserWindow({
            height:300,
            width:400,              
            resizable: false,
            parent: historyWindow,
            webPreferences:{
              nodeIntegration: true,              
              contextIsolation: false
            },
            autoHideMenuBar: true               
          });

          loginWindow.loadFile(__dirname + '/login/login.html')  

          loginWindow.on('closed',()=>{
            loginWindow = null;
          });
          
          loginWindow.once('ready-to-show',()=>{ loginWindow.show(); });
    });
});

ipcMain.on('load-histories-window',(event,data)=>{      

  loginWindow.close();     
  historyWindow.show();       

  historyWindow.webContents.send('catch-key',data);     
});