import axios, { AxiosError } from "axios";
import { LoginDTO } from "../models/LoginDTO";
import { UserDTO } from "../models/UserDTO";
import axiosClient, { requestConfig } from "../AxiosConfig";

export class AuthenticationService {
    public async login(username: string, password: string): Promise<UserDTO> {
        const loginDTO: LoginDTO = {
            username: username,
            password: password
        }

        return await axiosClient.post('login', loginDTO, requestConfig)
            .then(response => response.data)
            .catch((error: Error | AxiosError) => {
                if (axios.isAxiosError(error))
                    return Promise.reject(error.response?.data)
                
                return Promise.reject(error.message)
            })
    }
}