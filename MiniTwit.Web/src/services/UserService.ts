import axios, { AxiosError, AxiosResponse } from "axios";
import { UserCreateDTO, UserDTO } from "../models/UserDTO";
import axiosClient, { requestConfig } from "../AxiosConfig";

export class UserService {
    public async registerUser(username: string, email: string, password: string): Promise<AxiosResponse> {
        const userCreateDTO: UserCreateDTO = {
            username: username,
            email: email,
            pwd: password
        }

        return await axiosClient.post('register', userCreateDTO, requestConfig)
            .then(response => response)
            .catch((error: Error | AxiosError) => {
                if (axios.isAxiosError(error))
                    return Promise.reject(error.response?.data)

                return Promise.reject(error.message)
            })
    }

    public async getUserById(userId: string): Promise<UserDTO> {
        return await axiosClient.get(`username/${userId}`, requestConfig)
            .then(response => response.data)
            .catch((error: Error | AxiosError) => {
                if (axios.isAxiosError(error))
                    return Promise.reject(error.response?.data)

                return Promise.reject(error.message)
            })
    }
}
