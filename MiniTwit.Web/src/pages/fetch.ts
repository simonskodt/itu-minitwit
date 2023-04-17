import axios, { AxiosRequestConfig } from 'axios';
import { makeMessageDTO } from "../builders/functions";
import { API_URL } from "../App";
import { MessageDTO } from '../models/MessageDTO';

export async function fetchPublicTimeline(pageNumber: number): Promise<MessageDTO[]> {
  const config: AxiosRequestConfig = {
    method: 'GET',
    headers: {},
  };
  const MesWithUsername: MessageDTO[] = [];
  try {
    const response = await axios.get(API_URL + 'public/' + pageNumber, config);
    for (const element of response.data) {
      const userWithName = await makeMessageDTO(element);
      MesWithUsername.push(userWithName);
    }
    return MesWithUsername;
  } catch (error) {
    console.log(error);
    return Promise.reject('fetch order history failed');
  }
}

export async function fetchUserByid(userId: string) {
  const config: AxiosRequestConfig = {
    method: 'GET',
    headers: {
    },
  };

  try {
    const a = await axios.get(API_URL + "user/" + userId, config).then((response) => response.data);
    return a;
  } catch (error) {
    console.log(error);
    return Promise.reject('fetch order history failed');
  }
}

export async function fetchPrivateTimeLine(username: string) {
  const config: AxiosRequestConfig = {
    method: 'GET',
    headers: {
    },
  };
  const MesWithUsername: MessageDTO[] = [];
  try {
    const response = await axios.get(API_URL + username, config);
    for (const element of response.data) {
      const userWithName = await makeMessageDTO(element);
      MesWithUsername.push(userWithName);
    }
    return MesWithUsername;
  } catch (error) {
    console.log(error);
    return Promise.reject('fetch order history failed');
  }

}
