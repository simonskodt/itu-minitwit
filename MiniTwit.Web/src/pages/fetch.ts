import { getMessageArray } from "../builders/functions";
import { useState } from "react";
import { MessagesContext } from "../contexts/messageContext";
import { useContext } from "react";
import axios, { AxiosRequestConfig } from 'axios';



export async function FetchPublicTimeline() {
    const config: AxiosRequestConfig = {
        method: 'GET',
        headers: {
        },
    };

    try {
        const a = await axios.get("https://localhost:7111/public", config).then((response) => response.data);
        return a;
    } catch (error) {
        console.log(error);
        return Promise.reject('fetch order history failed');
    }
}