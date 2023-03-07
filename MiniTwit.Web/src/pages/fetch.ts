import { getMessageArray } from "../builders/functions";
import { useState } from "react";
import { useContext } from "react";
import axios, { AxiosRequestConfig } from 'axios';
import { API_URL } from "../App";


export async function FetchPublicTimeline() {
    const config: AxiosRequestConfig = {
        method: 'GET',
        headers: {
        },
    };

    try {
        const a = await axios.get(API_URL + "public", config).then((response) => response.data);
        return a;
    } catch (error) {
        console.log(error);
        return Promise.reject('fetch order history failed');
    }
}