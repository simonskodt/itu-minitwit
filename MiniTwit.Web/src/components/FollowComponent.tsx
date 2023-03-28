import React, { Component, useEffect, useState } from 'react';
import './MessageComponent.css'
import { API_URL } from '../App';
import { AppService } from '../services/app.service'
import { ButtonGroup } from '@mui/material';

interface Props {
    isLoggedIn: boolean;
    userToFollow: string
}

const FollowComponent: React.FC<Props> = ({ isLoggedIn, userToFollow }) => {
    const appService = new AppService();
    const userName = sessionStorage.getItem('username');

    const [isFollowing, setIsFollowing] = useState(false);
    const [followButtonText, setFollowButtonText] = useState('');

    useEffect(() => {
        const fetchMessages = async () => {
            const res = await appService.getUserId(String(userName));
            const isFollowing = await appService.getIsFollowing(res.data.id, userToFollow);
            if (isFollowing.data == true) {
                setFollowButtonText('Unfollow');
            } else {
                setFollowButtonText('Follow');
            }
            setIsFollowing(isFollowing.data)
        };
        fetchMessages();
    }, []);

    function HandleFollow() {
        appService.getUserId(String(userName)).then((res) => {
            const id = res.data.id;
            appService.getIsFollowing(id, userToFollow).then((isFollowing) => {
                if (isFollowing.data) {
                    UnFollow(userName);
                    setFollowButtonText('Follow');
                    setIsFollowing(false);
                } else {
                    Follow(userName);
                    setFollowButtonText('Unfollow');
                    setIsFollowing(true);
                }
            });
        });
    }

    function Follow(username: any) {
        appService.getUserId(username).then((result) => {
            const id = result.data.id;
            appService.Follow(userToFollow, id).then((fol) => {
                alert('You are now following ' + userToFollow);
            })
        })
    }

    function UnFollow(username: any) {
        appService.getUserId(username).then((result) => {
            const id = result.data.id;
            appService.UnFollow(userToFollow, id).then((fol) => {
                alert('You are no longer following ' + userToFollow);
            })
        })
    }

    if (isLoggedIn && !(userName == userToFollow)) {
        return (
            <div>
                <button onClick={() => HandleFollow()}>{followButtonText}</button>
            </div>
        );
    } else {
        return (
            <div></div>
        )
    }
}

export default FollowComponent;