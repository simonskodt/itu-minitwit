import axios, { AxiosRequestConfig } from 'axios';
import { buildUser } from "../builders/functions";
import { makeMessageObjectWithName } from "../builders/functions";
import { MessageObjectWithName } from "../builders/interface";
import { API_URL } from "../App";


export async function FetchPublicTimeline(pageNumber : number): Promise<MessageObjectWithName[]> {
    const config: AxiosRequestConfig = {
      method: 'GET',
      headers: {},
    };
    const MesWithUsername: MessageObjectWithName[] = [];
    try {
      const response = await axios.get(API_URL + 'public/' + pageNumber, config);
      for (const element of response.data) {
        const u = await FetchUserByid(element.authorId);
        const user = buildUser(u);
        const userWithName = makeMessageObjectWithName(element, user.username);
        MesWithUsername.push(userWithName);
      }
      return MesWithUsername;
    } catch (error) {
      console.log(error);
      return Promise.reject('fetch order history failed');
    }
  }
  

export async function FetchUserByid(userId: string) {
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

export async function FetchPrivateTimeLine(username: string){
    const config: AxiosRequestConfig = {
      method: 'GET',
      headers: {
      },
  };
  try {
    const a = await axios.get(API_URL + username, config).then((response) => response.data);
    return a;
} catch (error) {
    console.log(error);
    alert("User doesn't exist");
    return Promise.reject('fetch order history failed');
}


}