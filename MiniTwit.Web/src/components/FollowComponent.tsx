import React, { Component, useState } from 'react';
import './MessageComponent.css'
import { API_URL } from '../App';
import { AppService } from '../services/app.service'


interface Props {
    isLoggedIn: boolean;
    userToFollow: string
}

const FollowComponent: React.FC<Props> = ({ isLoggedIn, userToFollow }) => {

    const appService = new AppService();
    const userName = sessionStorage.getItem('username')

    function Follow(username: any){
        appService.getUserId(username).then((result)=>{
            const id = result.data.id
            appService.follow(userToFollow, id).then((fol) =>{
                console.log(fol)
            })
        })

    }


    if(isLoggedIn){
        return (
            <div>
            <button onClick={()=> Follow(userName)}>FOLLOW</button>
            <button>UNFOLLOW</button>
            </div>
        );
    }else{
        return(
        <div> NOT LOGGED IN</div>
        )
    }
}

export default FollowComponent;