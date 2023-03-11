import { getMessageArray } from "../builders/functions";
import { useState } from "react";
import { useContext } from "react";
import axios, { AxiosRequestConfig } from 'axios';
import { LOCALHOST, PRODUCTION } from "../App";
import { buildUser } from "../builders/functions";
import { makeMessageObjectWithName } from "../builders/functions";
import { MessageObjectWithName } from "../builders/interface";


export async function FetchPublicTimeline(): Promise<MessageObjectWithName[]> {
    const config: AxiosRequestConfig = {
      method: 'GET',
      headers: {},
    };
    const MesWithUsername: MessageObjectWithName[] = [];
    try {
      const response = await axios.get(LOCALHOST + 'public', config);
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
        const a = await axios.get(LOCALHOST + "user/" + userId, config).then((response) => response.data);
        return a;
    } catch (error) {
        console.log(error);
        return Promise.reject('fetch order history failed');
    }
}