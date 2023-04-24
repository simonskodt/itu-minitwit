import axios, { AxiosError, AxiosResponse } from "axios";
import { MessageDTO } from "../models/MessageDTO";
import axiosClient, { requestConfig } from "../AxiosConfig";

export class MesssageService {
    public async createMessage(text: string, userId: string): Promise<AxiosResponse> {
        return await axiosClient.post(`add_message?userId=${userId}&text=${text}`, null, requestConfig)
            .then(response => response)
            .catch((error: Error | AxiosError) => {
                if (axios.isAxiosError(error))
                    return Promise.reject(error.response)

                return Promise.reject(error.message)
            })
    }

    public async getPublicTimeline(pageNumber: number): Promise<MessageDTO[]> {
        return await axiosClient.get(`public/${pageNumber}`, requestConfig)
            .then(response => response.data)
            .catch((error: Error | AxiosError) => {
                if (axios.isAxiosError(error))
                    return Promise.reject(error.response?.data)

                return Promise.reject(error.message)
            })
    }

    public async getPrivateTimeline(username: string): Promise<MessageDTO[]> {
        return await axiosClient.get(username, requestConfig)
            .then(response => response.data)
            .catch((error: Error | AxiosError) => {
                if (axios.isAxiosError(error))
                    return Promise.reject(error.response)

                return Promise.reject(error)
            })
    }
}