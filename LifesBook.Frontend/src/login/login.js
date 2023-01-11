const ipcRenderer = require('electron').ipcRenderer;

function loadHistoriesWindow(){
    
    var password = document.getElementById("passwordText").value;   

    if(passwordMeetsMinimumRequirement(password)){     
        ipcRenderer.send('load-histories-window', password);
    }
    else{
        showErrorMessage();
    }
    
} 

function passwordMeetsMinimumRequirement(password){

    const minLength = 5; 
    const maxLenghth = 10; 
   
    var passwordLenght = password.length;

    return passwordLenght>=minLength && passwordLenght<=maxLenghth;
}

function showErrorMessage(){

    document.getElementById('errorTextBox').hidden=false;
    document.getElementById('errorMessage').innerHTML='Password Invalid! Must to be 5-10 Characters';
}