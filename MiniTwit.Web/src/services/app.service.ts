export class AppService {
  
  public async registerUser(username : string, email : string, pw : string) : Promise<any> {
    // Send data to the backend via POST
    const response = fetch('https://localhost:7111/Register', { 
      method: 'POST',
      body: username,
      mode: 'cors', // body data type must match "Content-Type" header
    }) 
    return (await response).json();
  }

  public async Login(username: string, password : string ) : Promise<any> {
    const data = { username: username, pw:password };
    const response = fetch('https://localhost:7111/Login', {
      method: 'POST', // or 'PUT'
      body: JSON.stringify(data),
      mode: 'cors',
    })
    return (await response).json();
  }
}
