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
  
  public async Follow(UserToFollow: string, id: string) {
    //https://localhost:7111/Eriksen/follow?userId=6410920135626df320346d7a

    const request: AxiosRequestConfig = {
      method: 'post',
      maxBodyLength: Infinity,
      url: API_URL + UserToFollow + "/follow?userId=" + id,
      headers: {}
    };

    try {
      const response = await axios(request).then((response: AxiosResponse) => response);
      return response;
    } catch (error) {
      alert("You are already following this user or trying to follow yourself");
      return Promise.reject();
    }

  }

  public async UnFollow(UserToUnFollow: string, id: string) {
    //https://localhost:7111/Eriksen/follow?userId=6410920135626df320346d7a

    const request: AxiosRequestConfig = {
      method: 'delete',
      maxBodyLength: Infinity,
      url: API_URL + UserToUnFollow + "/unfollow?userId=" + id,
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

  public async getIsFollowing(userid: string, username: string) {
    const request: AxiosRequestConfig = {
      method: 'get',
      maxBodyLength: Infinity,
      url: API_URL + "user/" + userid + "/isfollowedby/" + username,
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
