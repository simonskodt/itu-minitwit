import { MessageObject } from "./interface";


export function getMessageArray(json: any) : MessageObject[]{
    let returnArrayOfObject = new Array(json.length);

    for (let i = 0; i< json.length; i++){
        let messageId = json[i].messageId;
        let authorId = json[i].authorId;
        let text = json[i].text;
        let pubDate = json[i].pubDate;
        let flagged = json[i].flagged;

        let messageObj = {
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