import { getMessageArray } from "../builders/functions";
import { useState } from "react";
import { useContext } from "react";
import axios, { AxiosRequestConfig } from 'axios';
import { LOCALHOST, PRODUCTION } from "../App";
import { buildUser } from "../builders/functions";
import { makeMessageObjectWithName } from "../builders/functions";
import { MessageObjectWithName } from "../builders/interface";


export async function FetchPublicTimeline() {
    const config: AxiosRequestConfig = {
        method: 'GET',
        headers: {
        },
    };
    const MesWithUsername : MessageObjectWithName[] = []; 
    try {
        const a = await axios.get(LOCALHOST + "public", config).then((response) => {
            response.data.forEach(element => {
                FetchUserByid(element.authorId).then((u)=>{
                    var user = buildUser(u);
                    var userWithName = makeMessageObjectWithName(element, user.username);
                    MesWithUsername.push(userWithName)
                    });
            });
        });
        return MesWithUsername
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