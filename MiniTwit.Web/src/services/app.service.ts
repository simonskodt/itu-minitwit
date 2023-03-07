import axios, { AxiosRequestConfig } from 'axios';
import { API_URL } from '../App';


export class AppService {
  
  public async registerUser(username : string, email : string, pw : string) : Promise<any> {
    var data = JSON.stringify({
      "username": username,
      "email": email,
      "password": pw
    });
    const config: AxiosRequestConfig = {
    method: 'post',
    maxBodyLength: Infinity,
      url: API_URL + 'register',
      headers: { 
        'Content-Type': 'application/json'
      },
      data : data
    };
    try {
      const a = await axios(config).then((response) => response.data);
      return a;
    } catch (error) {
        console.log(error);
        return Promise.reject();
    }
  }

  public async Login(username : string, pw : string) : Promise<any> {
    var data = JSON.stringify({
      "username":  username,
      "password": pw
    });

    const config: AxiosRequestConfig = {
      method: 'post',
      maxBodyLength: Infinity,
      url: API_URL + 'login',
      headers: { 
        'Content-Type': 'application/json'
      },
      data: data
    };

    try {
        const a = await axios(config).then((response) => response.data);
        return a;
    } catch (error) {
        console.log(error);
        return Promise.reject();
    }
  }
}