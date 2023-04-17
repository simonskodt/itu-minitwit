import { MessageDTO } from "../models/MessageDTO";
import { fetchUserByid } from "../pages/fetch";

export async function makeMessageDTO(message: any): Promise<MessageDTO> {
    if (message.authorName === null) {
        const user = await fetchUserByid(message.authorId);
        message.authorName = user.username
    }

    const messageDTO: MessageDTO = {
        messageId: message.id,
        authorId: message.authorId,
        text: message.text,
        pubDate: message.pubDate,
        flagged: message.flagged,
        username: message.authorName
    }

    return messageDTO;
}

export function checkLogIn(): boolean {
    const isLoggedIn = sessionStorage.getItem('isLoggedIn')
    if (isLoggedIn === 'true') {
        return true
    } else {
        return false
    }
}

export function getCurrentUsername(): string {
    const username = sessionStorage.getItem('username') 
    
    if (username === null) {
        return ''
    }

    return username
}
