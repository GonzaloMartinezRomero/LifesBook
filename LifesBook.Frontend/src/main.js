const {app, BrowserWindow, ipcMain} = require('electron');

let historyWindow = null;
let loginWindow = null;
let alertWindow = null;
 
app.on('ready',()=>{
    
    historyWindow = new BrowserWindow({
      show: false,
      webPreferences:{
        nodeIntegration: true,
        contextIsolation:false
      },
      width: 1250,
      height: 820,
      resizable:false,      
      autoHideMenuBar: true,
      frame: false
    });

    historyWindow.loadFile(__dirname + '/history/history.html');

    historyWindow.on('closed',()=>{
      historyWindow = null;
    });

    historyWindow.once('ready-to-show',()=>{

          loginWindow = new BrowserWindow({
            height:270,
            width:400,              
            resizable: false,
            parent: historyWindow,
            webPreferences:{
              nodeIntegration: true,              
              contextIsolation: false
            },
            autoHideMenuBar: true,
            frame: false               
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

ipcMain.on('alert-message',(event,data)=>{      

  alertWindow = new BrowserWindow({
    show: false,
    webPreferences:{
      nodeIntegration: true,
      contextIsolation:false
    },
    width: 400,
    height: 200,
    parent: historyWindow,
    alwaysOnTop:true,
    modal:true,    
    resizable:false,      
    autoHideMenuBar: true,
    frame: false
  });

  alertWindow.loadFile(__dirname + '/alert/alert.html');

  alertWindow.on('closed',()=>{
    alertWindow = null;
  }); 

  alertWindow.once('ready-to-show',()=>{
    
      alertWindow.webContents.send('alert-initialize',data);  
      alertWindow.show();
  });

});

ipcMain.on('close-alert-window',(event,data)=>alertWindow.close());

ipcMain.on('close-application',(event,data)=>app.quit());