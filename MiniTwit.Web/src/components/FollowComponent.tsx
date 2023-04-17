import React, { useEffect, useState } from 'react';
import './MessageComponent.css'
import { AppService } from '../services/app.service'

interface Props {
    isLoggedIn: boolean;
    userToFollow: string
}

const FollowComponent: React.FC<Props> = ({ isLoggedIn, userToFollow }) => {
    const appService = new AppService();
    const username = sessionStorage.getItem('username');

    const [isFollowing, setIsFollowing] = useState(false);
    const [followButtonText, setFollowButtonText] = useState('');

    useEffect(() => {
        const fetchMessages = async () => {
            const res = await appService.getUserId(String(username));
            const isFollowing = await appService.getIsFollowing(res.data.id, userToFollow);
            if (isFollowing.data === true) {
                setFollowButtonText('Unfollow');
            } else {
                setFollowButtonText('Follow');
            }
            setIsFollowing(isFollowing.data)
        };
        fetchMessages();
    }, [userToFollow, username]);

    function HandleFollow() {
        appService.getUserId(String(username)).then((res) => {
            const id = res.data.id;
            appService.getIsFollowing(id, userToFollow).then((isFollowing) => {
                if (isFollowing.data) {
                    UnFollow(username);
                    setFollowButtonText('Follow');
                    setIsFollowing(false);
                } else {
                    Follow(username);
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

    if (isLoggedIn && !(username === userToFollow)) {
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