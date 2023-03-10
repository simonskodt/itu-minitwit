import axios, { AxiosError, AxiosRequestConfig, AxiosResponse } from 'axios';
import { API_URL } from '../App';

export class AppService {

  public async registerUser(username: string, email: string, pw: string): Promise<any> {
    var data = JSON.stringify({
      "username": username,
      "email": email,
      "pwd": pw
    });

    const request: AxiosRequestConfig = {
      method: 'post',
      maxBodyLength: Infinity,
      url: API_URL + 'register',
      headers: {
        'Content-Type': 'application/json'
      },
      data: data
    };
    try {
      const response = await axios(request).then((response) => response.data);
      return response;
    } catch (error) {
      console.log(error);
      return Promise.reject();
    }
  }

  public async Login(username: string, pw: string): Promise<any> {
    var data = JSON.stringify({
      "username": username,
      "password": pw
    });

    const request: AxiosRequestConfig = {
      method: 'post',
      maxBodyLength: Infinity,
      url: API_URL + 'login',
      headers: {
        'Content-Type': 'application/json'
      },
      data: data
    };

    try {
      const response = await axios(request).then((response: AxiosResponse) => response);
      return response;
    } catch (error) {
      const err = error as AxiosError
      console.log(err.response?.data);
      return Promise.reject();
    }
  }
}

export interface APIError {
  statis: number
  err_msg: string
}