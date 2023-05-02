import axios, { AxiosRequestConfig } from "axios"
// process.env.REACT_APP_DEV_BASE_URL
const PRODUCTION_BASE_URL = "http://164.92.167.188:80/"
const API_URL = process.env.NODE_ENV === 'development' ? "http://localhost:80" : PRODUCTION_BASE_URL

export const requestConfig: AxiosRequestConfig = {
    maxBodyLength: Infinity,
    headers: {
        'Content-Type': 'application/json'
    }
}

const axiosClient = axios.create({
    baseURL: API_URL
})

export default axiosClient