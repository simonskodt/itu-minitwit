import axios, { AxiosError, AxiosRequestConfig, AxiosResponse } from 'axios';
import { url } from 'inspector';
import { API_URL } from '../App';
import { LoginDTO } from '../models/Login';
import { UserDTO } from '../models/User';

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

  public async Login(username: string, pw: string): Promise<AxiosResponse<UserDTO>> {
    var loginDTO: LoginDTO = {
      "username": username,
      "password": pw
    };

    const request: AxiosRequestConfig = {
      method: 'post',
      maxBodyLength: Infinity,
      url: API_URL + 'login',
      headers: {
        'Content-Type': 'application/json'
      },
      data: loginDTO
    };

    try {
      return await axios(request).then((response: AxiosResponse) => response);
    } catch (error) {
      const err = error as AxiosError
      console.log(err.response?.data);
      return Promise.reject();
    }
  }

  public async sendMessage(text: string, userId : string): Promise<any> {
    const request: AxiosRequestConfig = {
      method: 'post',
      maxBodyLength: Infinity,
        url: API_URL + 'add_message?userId=' + userId + '&text=' + text,
        headers: { }
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

  public async getUserId(username: string): Promise<any> {
    const request: AxiosRequestConfig = {
      method: 'get',
      maxBodyLength: Infinity,
        url: API_URL + 'username/' + username,
        headers: { }
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
