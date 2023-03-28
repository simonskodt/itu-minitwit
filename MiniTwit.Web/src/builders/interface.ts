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
    userName:string
    text: string,
    pubDate: string,
    flagged: number,
  } 

  export interface User
  {
    id:string,
    username:string,
    email:string
  }