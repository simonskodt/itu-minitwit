#!/bin/bash

# Colors
red='\033[0;31m'
green='\033[0;32m'
clear='\033[0m'

function println {
    printf "%b%b\n%b" "$2" "$1" "$clear"
}

if [ "$DIGITAL_OCEAN_TOKEN" == "" ]; then
    println "'DIGITAL_OCEAN_TOKEN' environment variable is not set!" "$red"
    exit
else
    # Digital Ocean
    export BEARER_AUTH_TOKEN="Authorization: Bearer $DIGITAL_OCEAN_TOKEN"
    export JSON_CONTENT="Content-Type: application/json"
fi

function get_ip {
    for (( ; ; ))
    do
        IP=$(curl -s GET "$DROPLETS_API"\
        -H "$BEARER_AUTH_TOKEN" -H "$JSON_CONTENT"\
        | jq -r "$JQFILTER")
        sleep 5
        if [ "$IP" != "" ]; then
            echo "$IP"
            break
        fi
    done
}

function createDockerSwarmClusterNode {
    export DIGITALOCEAN_PRIVATE_NETWORKING=true
    export DROPLETS_API="https://api.digitalocean.com/v2/droplets"

    CONFIG='{"name":"Swarm-Manager","tags":["demo"],
        "size":"s-1vcpu-1gb", "image":"docker-20-04",
        "ssh_keys":["35:27:02:78:dc:a4:80:b6:90:77:9d:19:f2:46:f4:13", "6b:53:ee:b5:06:24:9a:63:96:c4:04:40:db:10:db:2a", "fd:df:2a:4b:4e:06:ce:a7:97:11:e1:88:23:6c:6d:7b", "81:33:1f:de:81:9b:5d:bf:0e:92:41:a7:53:96:52:ba"]}'

    println "Creating Droplet..." "$green"
    SWARM_MANAGER_ID=$(curl -X POST $DROPLETS_API -d "$CONFIG"\
        -H "$BEARER_AUTH_TOKEN" -H "$JSON_CONTENT"\
        | jq -r .droplet.id ) && sleep 5 && echo "$SWARM_MANAGER_ID"

    export JQFILTER='.droplets | .[] | select (.name == "Swarm-Manager") | .networks.v4 | .[]| select (.type == "public") | .ip_address'

    SWARM_MANAGER_IP=$(get_ip)
    echo "Swarm Manager IP: $SWARM_MANAGER_IP"
    println "Done." "$green"
}

function createWorkerNodes {
    println "Creating worker1 Droplet..." "$green"
    WORKER1_ID=$(curl -X POST $DROPLETS_API\
        -d'{"name":"worker1","tags":["demo"],"region":"fra1",
        "size":"s-1vcpu-1gb","image":"docker-20-04",
        "ssh_keys":["35:27:02:78:dc:a4:80:b6:90:77:9d:19:f2:46:f4:13", "6b:53:ee:b5:06:24:9a:63:96:c4:04:40:db:10:db:2a", "fd:df:2a:4b:4e:06:ce:a7:97:11:e1:88:23:6c:6d:7b", "81:33:1f:de:81:9b:5d:bf:0e:92:41:a7:53:96:52:ba"]}'\
        -H "$BEARER_AUTH_TOKEN" -H "$JSON_CONTENT" | jq -r .droplet.id ) && sleep 3 && echo "$WORKER1_ID"

    export JQFILTER='.droplets | .[] | select (.name == "worker1") | .networks.v4 | .[]| select (.type == "public") | .ip_address'

    WORKER1_IP=$(get_ip)
    echo "Worker1 IP: $WORKER1_IP"
    println "Done." "$green"

    println "Creating worker2 Droplet..." "$green"
    WORKER2_ID=$(curl -X POST $DROPLETS_API\
       -d'{"name":"worker2","tags":["demo"],"region":"fra1",
       "size":"s-1vcpu-1gb","image":"docker-20-04",
       "ssh_keys":["35:27:02:78:dc:a4:80:b6:90:77:9d:19:f2:46:f4:13", "6b:53:ee:b5:06:24:9a:63:96:c4:04:40:db:10:db:2a", "fd:df:2a:4b:4e:06:ce:a7:97:11:e1:88:23:6c:6d:7b", "81:33:1f:de:81:9b:5d:bf:0e:92:41:a7:53:96:52:ba"]}'\
       -H "$BEARER_AUTH_TOKEN" -H "$JSON_CONTENT"\
       | jq -r .droplet.id )\
       && sleep 3 && echo "$WORKER2_ID"

    export JQFILTER='.droplets | .[] | select (.name == "worker2") | .networks.v4 | .[]| select (.type == "public") | .ip_address'

    WORKER2_IP=$(get_ip)
    echo "Worker2 IP: $WORKER2_IP"
    println "Done." "$green"
}

function makeSwarmManagerClusterManager {
    println "Open the ports that Docker needs..." "$green"
    ssh root@"$SWARM_MANAGER_IP" -i ~/.ssh/"$SSH_KEY" "ufw allow 22/tcp && ufw allow 2376/tcp &&\
    ufw allow 2377/tcp && ufw allow 7946/tcp && ufw allow 7946/udp &&\
    ufw allow 4789/udp && ufw reload && ufw --force  enable &&\
    systemctl restart docker"
    println "Done." "$green"

    println "Initialize the swarm..." "$green"
    ssh root@"$SWARM_MANAGER_IP" -i ~/.ssh/"$SSH_KEY" "docker swarm init --advertise-addr $SWARM_MANAGER_IP"
    ssh root@"$SWARM_MANAGER_IP" -i ~/.ssh/"$SSH_KEY" "docker swarm join-token worker -q"
    WORKER_TOKEN=`ssh root@"$SWARM_MANAGER_IP" -i ~/.ssh/"$SSH_KEY" "docker swarm join-token worker -q"`
    println "Done." "$green"

    println "Build a command that we can run on node-1 and node-2 to join the swarm...." "$green"
    REMOTE_JOIN_CMD="docker swarm join --token $WORKER_TOKEN $SWARM_MANAGER_IP:2377"
    ssh root@"$WORKER1_IP" -i ~/.ssh/"$SSH_KEY" "$REMOTE_JOIN_CMD"
    ssh root@"$WORKER2_IP" -i ~/.ssh/"$SSH_KEY" "$REMOTE_JOIN_CMD"
    println "Done." "$green"
}

function getClusterManagerState {
    println "Seeing the state of the cluster on the manager..." "$green"
    ssh root@"$SWARM_MANAGER_IP" -i ~/.ssh/"$SSH_KEY" "docker node ls"
    println "Done." "$green"
}

function startService {
    println "Running a service on our cluster..." "$green"
    ssh root@"$SWARM_MANAGER_IP" -i ~/.ssh/"$SSH_KEY" "docker service create -p 8080:8080 --name appserver stifstof/crashserver"
    println "Done." "$green"
}

function removeDroplets {
    println "Removing swarm droplets..." "$red"
    curl -X DELETE\
      -H "$BEARER_AUTH_TOKEN" -H "$JSON_CONTENT"\
      "https://api.digitalocean.com/v2/droplets?tag_name=demo"
    println "Done." "$red"
}

function print_help {
    println "--- RUN HELP ---" "$green"
    println "Available Commands:"
    println ">> -i <SSH key> - The private SSH key to use"
    println ">> clean - Remove remote Swarm Cluster Droplets"
    println ">> -h - Print this page"
    println "----------------" "$green"
}

if [ "$1" == "-i" ] && [ "$2" != "" ]; then
    export SSH_KEY="$2"
    createDockerSwarmClusterNode
    createWorkerNodes
    makeSwarmManagerClusterManager
    getClusterManagerState
elif [ "$1" == "clean" ]; then
    removeDroplets
elif [ "$1" == "-h" ]; then
    print_help
else
    println "Error: Unknown command" "$red"
    println "For a list of available commands type './swarm.sh -h'" "$red"
fi