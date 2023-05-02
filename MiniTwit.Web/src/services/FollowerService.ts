import axios, { AxiosError, AxiosResponse } from "axios";
import axiosClient, { requestConfig } from "../AxiosConfig";

export class FollowerService {
    public async followUser(usernameToFollow: string, id: string): Promise<AxiosResponse> {
        return await axiosClient.post(`${usernameToFollow}/follow?userId=${id}`, null, requestConfig)
            .then(response => response)
            .catch((error: Error | AxiosError) => {
                if (axios.isAxiosError(error))
                    return Promise.reject(error.response)
                
                return Promise.reject(error.message)
            })
    }

    public async unfollowUser(usernameToUnfollow: string, id: string): Promise<AxiosResponse> {
        return await axiosClient.delete(`${usernameToUnfollow}/unfollow?userId=${id}`, requestConfig)
            .then(response => response)
            .catch((error: Error | AxiosError) => {
                if (axios.isAxiosError(error))
                    return Promise.reject(error.response?.data)
                
                return Promise.reject(error.message)
            })

    }
    
    public async getIsFollowingUser(userId: string, username: string): Promise<boolean> {
        return await axiosClient.get(`user/${userId}/isfollowedby/${username}`, requestConfig)
            .then(response => response.data)
            .catch((error: Error | AxiosError) => {
                if (axios.isAxiosError(error))
                    return Promise.reject(error.response)
                
                return Promise.reject(error)
            })
    }
}