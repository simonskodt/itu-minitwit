export interface MessageObject
{
    messageId: string,
    authorId: string,
    text: string,
    pubDate: string,
    flagged: number
  } 

  export interface MessageObjectWithName
{
    messageId: string,
    authorId: string,
    text: string,
    pubDate: string,
    flagged: number,
    userName:string
  } 

  export interface User
  {
    id:string,
    username:string,
    email:string
  }