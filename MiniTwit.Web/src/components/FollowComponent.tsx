import React, { useEffect, useState } from 'react';
import './MessageComponent.css'
import { UserService } from '../services/UserService';
import { FollowerService } from '../services/FollowerService';
import { getCurrentUsername } from '../state/SessionStorage';

interface Props {
    isLoggedIn: boolean;
    userToFollow: string
}

const FollowComponent: React.FC<Props> = ({ isLoggedIn, userToFollow }) => {
    const userService = new UserService();
    const followerService = new FollowerService()
    const username = getCurrentUsername();

    const [isFollowing, setIsFollowing] = useState(false);
    const [followButtonText, setFollowButtonText] = useState('');

    useEffect(() => {
        const fetchMessages = async () => {
            const user = await userService.getUserById(String(username));
            const isFollowing = await followerService.getIsFollowingUser(user.id, userToFollow);
            if (isFollowing === true) {
                setFollowButtonText('Unfollow');
            } else {
                setFollowButtonText('Follow');
            }
            setIsFollowing(isFollowing)
        };
        fetchMessages();
    }, [userToFollow, username]);

    function HandleFollow() {
        userService.getUserById(String(username)).then((user) => {
            const id = user.id;
            followerService.getIsFollowingUser(id, userToFollow).then((isFollowing) => {
                if (isFollowing) {
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
        userService.getUserById(username).then((user) => {
            followerService.followUser(userToFollow, user.id).then(() => {
                alert('You are now following ' + userToFollow);
            })
        })
    }

    function UnFollow(username: any) {
        userService.getUserById(username).then((user) => {
            followerService.unfollowUser(userToFollow, user.id).then(() => {
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