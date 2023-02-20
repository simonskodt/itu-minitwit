import { getMessageArray } from "../builders/functions";

export async function fetchPublicTimeline() {
    var headers = new Headers();
    var requestOptions = {
        method: 'GET',
        headers: headers,
    };

    // var response = await fetch('api/timeline', request);
    var response = await fetch("https://localhost:7111/public", requestOptions)

    if (response.ok) { // if HTTP-status is 200-299
        // get the response body (the method explained below)
        let json = await response.json();        
        let messageArray = getMessageArray(json);
        console.log(messageArray);
      } else {
        alert("HTTP-Error: " + response.status);
      }

/* 
    fetch("https://localhost:7111/public", request)
        .then(response => response.text())
        .then(result => {
            console.log(result);
        })
        .catch(error => console.log('error', error)); */
}