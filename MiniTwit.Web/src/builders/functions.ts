import { MessageObject, MessageObjectWithName, User } from "./interface";

export function getMessageArray(json: any): MessageObject[] {
    const returnArrayOfObject = new Array(json.length);

    for (let i = 0; i < json.length; i++) {
        const messageId = json[i].messageId;
        const authorId = json[i].authorId;
        const text = json[i].text;
        const pubDate = json[i].pubDate;
        const flagged = json[i].flagged;

        const messageObj = {
            messageId: messageId,
            authorId: authorId,
            text: text,
            pubDate: pubDate,
            flagged: flagged
        }
        returnArrayOfObject[i] = messageObj;
    }
    return returnArrayOfObject;
}

export function makeMessageObjectWithName(message: any, name: string): MessageObjectWithName {
    let messageId: string;
    let authorId: string;
    let text: string;
    let pubDate: string;
    let flagged: number;
    let userName: string;

    const messageObjwithName = {
        messageId: message.id,
        authorId: message.authorId,
        text: message.text,
        pubDate: message.pubDate,
        flagged: message.flagged,
        userName: name
    }
    return messageObjwithName;
}

export function buildUser(json: any): User {
    let id: any;
    let username: any;
    let email: any;

    const user = {
        id: json.id,
        username: json.username,
        email: json.email
    }

    return user
}

export function checkLogIn(): boolean {
    const isLoggedIn = sessionStorage.getItem('isLoggedIn')
    if (isLoggedIn == 'true') {
        return true
    } else {
        return false
    }
}

export function getCurrentUsername(): any {
    const username = sessionStorage.getItem('username')
    return username
}
