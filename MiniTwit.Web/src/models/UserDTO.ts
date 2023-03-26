export interface UserDTO {
    id: string
    username: string
    email: string
}

export interface UserCreateDTO {
    username: string
    email: string
    pwd: string
}
