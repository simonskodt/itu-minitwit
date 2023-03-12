import {useLocation } from "react-router-dom";

function PrivateTimeline()
{
    let url = window.location.href;
    var parts = url.split("/");
    var userName = parts[parts.length - 1]; 
    console.log(userName);

    return(
        <view>
            HEJ MED JER {userName}
        </view>
    );
}

export default PrivateTimeline