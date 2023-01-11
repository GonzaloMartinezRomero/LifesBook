const {ipcRenderer} = require('electron');

let key = "";
let historyMap = new Map();

ipcRenderer.on('catch-key',(event,data)=>{

    key = data;
    loadAllHistories();
});

async function loadAllHistories(){

    const url = "https://localhost:7112/History";
    const options = {
        headers: {
          'key': key,
          'Content-Type': 'text/plain; charset=UTF-8'
        },
      };

    await fetch(url,options).then(response => response.arrayBuffer())
                            .then(buffer => {

                                historyMap.clear();   

                                let decoder = new TextDecoder("utf-8");
                                let text = decoder.decode(buffer);
                                let historyList = JSON.parse(text);

                                historyList.forEach(element => {
                                    
                                    historyMap.set(element.id, element);    

                                    const historyButton = document.createElement("button");
                                    historyButton.innerHTML = element.id;
                                    historyButton.className = 'list-group-item list-group-item-action';
                                    historyButton.addEventListener("click",()=>{
                                        let id = element.id;
                                        openHistory(id);
                                    });

                                    document.getElementById('historyList').appendChild(historyButton);
                                });
                            })
                            .catch(error => showErrorMessage(error));
}

function openHistory(id){

    let history = historyMap.get(id);

    document.getElementById('historyTitle').innerHTML = history.date;
    document.getElementById('historyContent').value = history.content;
}

function showErrorMessage(message){
    document.getElementById('messagePopup').hidden = false;
    document.getElementById('messageError').value = message;
}