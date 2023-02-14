export class AppService {
  
  public async insertUser() : Promise<any> {
    // Send data to the backend via POST
    const response = fetch('https://localhost:7111/Twitter', { 
      method: 'POST', 
      mode: 'cors', // body data type must match "Content-Type" header
    }) 
    return (await response).json();
  }

}
