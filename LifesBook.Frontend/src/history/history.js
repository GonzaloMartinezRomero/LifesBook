const {ipcRenderer} = require('electron');

let key = "";
let historyMap = new Map();
let selectedHistoryId = "0";

ipcRenderer.on('catch-key',(event,data)=>{

    key = data;
    loadAllHistories();
});

async function loadAllHistories(){

    const url = "https://localhost:7112/History";
    const options = {
        headers: {
          'Key': key,
          'Content-Type': 'text/plain; charset=UTF-8'
        },
      };

    resetEnvironment();

    await fetch(url,options).then(response => {
      
        switch(response.status){
            case 200:    

                response.arrayBuffer().then((buffer)=>{

                    let textDecoded = new TextDecoder("utf-8").decode(buffer);
                    let historyList = JSON.parse(textDecoded);
    
                    historyList.forEach(element => {
                    
                        historyMap.set(element.id, element);    
                        addHistoryButton(element);                                   
                    });
                })
                
                break;                          

            default:                            
                response.text().then(text => {
                    showAlertMessage(`Request Status ${response.status}`, text);
                });                
                break;

        }}).catch(error => showAlertMessage('Request Exception',error));
}

function resetEnvironment(){
    historyMap.clear();     
    selectedHistoryId = "0";
    document.getElementById('historyContent').readOnly = true;
}

function addHistoryButton(history){

    const historyButton = document.createElement("button");
    historyButton.innerHTML = new Date(history.date).toLocaleDateString();
    historyButton.className = 'list-group-item list-group-item-action';
    historyButton.addEventListener("click",()=>{
        let id = history.id;
        openHistory(id);
    });
    document.getElementById('historyList').appendChild(historyButton);
}

function openHistory(id){

    if(existPendingChanges()){        
        showAlertMessage('History','Changes not saved!');        
    }else{
        
        let history = historyMap.get(id);

        let historyTitle = new Date(history.date).toLocaleDateString("es-ES",{ weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' });

        document.getElementById('historyTitle').innerHTML = historyTitle;

        let historyContent =document.getElementById('historyContent');
        historyContent.value = history.content;
        historyContent.readOnly = false;

        selectedHistoryId = id;
    }
}

function existPendingChanges(){

    if(selectedHistoryId !== "0"){
        
        let historyContent = document.getElementById('historyContent').value;
        let savedContent = historyMap.get(selectedHistoryId).content;
        return historyContent !== savedContent;
    }

    return false;
}

function showAlertMessage(errorSource, message){
    
    ipcRenderer.send('alert-message',{errorSource,message});
}

async function newHistory(){

    let currentDate = new Date();
    let day= currentDate.getDate().toString().padStart(2,'0');
    let month= (currentDate.getMonth()+1).toString().padStart(2,'0');
    let year= currentDate.getFullYear().toString();

    let dateFormat = `${year}-${month}-${day}`; 
    
    const url = `https://localhost:7112/History?date=${dateFormat}`;
    const options = {
        method: 'POST',
        headers: {
          'Key': key,
          'Content-Type': 'application/json'
        },
        body: '""'
      };

    await fetch(url,options).then(response => {
      
        if(!response.ok){
            response.text().then(text => {
                showAlertMessage(`Request Status ${response.status}`, text);
            });     
        }
        else{
            response.arrayBuffer().then((buffer)=>{

                let textDecoded = new TextDecoder("utf-8").decode(buffer);
                let historyAdded = JSON.parse(textDecoded);

                historyMap.set(historyAdded.id,historyAdded);
                
                addHistoryButton(historyAdded);
                openHistory(historyAdded.id);               
            })
        }
        
        }).catch(error => showAlertMessage('Request Exception',error));
}

async function saveHistory(){
    
    let historyContent = document.getElementById('historyContent').value.replaceAll('\n','\\n')    
                                                                        .replaceAll('"','\\"');
    const url = `https://localhost:7112/History/${selectedHistoryId}`;
    const options = {
        method: 'PUT',
        headers: {
          'Key': key,
          'Content-Type': 'application/json'
        },
        body: `"${historyContent}"`
      };

    await fetch(url,options).then(response => {
      
        if(!response.ok){
            response.text().then(text => {
                showAlertMessage(`Request Status ${response.status}`, text);
            });     
        }
        else{
            response.arrayBuffer().then((buffer)=>{

                let textDecoded = new TextDecoder("utf-8").decode(buffer);
                let historyAdded = JSON.parse(textDecoded);
                
                historyMap.set(historyAdded.id,historyAdded);

                showAlertMessage("Saved successfully!","");
            })
        }
        
        }).catch(error => showAlertMessage('Request Exception',error));
}

function closeApplication(){
    ipcRenderer.send('close-application', '');
}