import axios, { AxiosError, AxiosRequestConfig, AxiosResponse } from 'axios';
import { API_URL } from '../App';
import { LoginDTO } from '../models/LoginDTO';
import { UserCreateDTO, UserDTO } from '../models/UserDTO';

const requestConfig: AxiosRequestConfig = {
  maxBodyLength: Infinity,
  headers: {
    'Content-Type': 'application/json'
  }
}

export class AppService {

  public async registerUser(username: string, email: string, pw: string): Promise<any> {
    const userCreateDTO: UserCreateDTO = {
      username: username,
      email: email,
      pwd: pw
    }

    return await axios.post<any>(API_URL + 'register', userCreateDTO, requestConfig)
      .then(response => response)
      .catch((error: Error | AxiosError) => {
        if (axios.isAxiosError(error)) {
          return Promise.reject(error.response?.data.error_msg)
        }

        return Promise.reject(error)
      })
  }

  public async Login(username: string, pw: string): Promise<UserDTO> {
    const loginDTO: LoginDTO = {
      username: username,
      password: pw
    };

    return await axios.post<UserDTO>(API_URL + 'login', loginDTO, requestConfig)
      .then(response => response.data)
      .catch((error: Error | AxiosError) => {
        if (axios.isAxiosError(error)) {
          return Promise.reject(error.response?.data.error_msg)
        }

        return Promise.reject(error)
      })
  }

  public async sendMessage(text: string, userId: string): Promise<any> {
    const request: AxiosRequestConfig = {
      method: 'post',
      maxBodyLength: Infinity,
      url: API_URL + 'add_message?userId=' + userId + '&text=' + text,
      headers: {}
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
      headers: {}
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
