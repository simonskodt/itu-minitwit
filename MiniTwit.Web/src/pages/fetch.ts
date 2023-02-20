export async function fetchPublicTimeline() {
    var headers = new Headers();
    var request = {
        method: 'GET',
        headers: headers,
    };

    // var response = await fetch('api/timeline', request);

    fetch("https://localhost:7111/public", request)
        .then(response => response.text())
        .then(result => {
            console.log(result);
        })
        .catch(error => console.log('error', error));
}