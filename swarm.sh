#!/bin/bash

# Colors
red='\033[0;31m'
green='\033[0;32m'
clear='\033[0m'

# Digital Ocean
export BEARER_AUTH_TOKEN="Authorization: Bearer $DIGITAL_OCEAN_TOKEN"
export JSON_CONTENT="Content-Type: application/json"

function println {
    printf "%b%b\n%b" "$2" "$1" "$clear"
}

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
        "ssh_keys":["35:27:02:78:dc:a4:80:b6:90:77:9d:19:f2:46:f4:13"]}'

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
        "ssh_keys":["35:27:02:78:dc:a4:80:b6:90:77:9d:19:f2:46:f4:13"]}'\
        -H "$BEARER_AUTH_TOKEN" -H "$JSON_CONTENT" | jq -r .droplet.id ) && sleep 3 && echo "$WORKER1_ID"

    export JQFILTER='.droplets | .[] | select (.name == "worker1") | .networks.v4 | .[]| select (.type == "public") | .ip_address'

    WORKER1_IP=$(get_ip)
    echo "Worker1 IP: $WORKER1_IP"
    println "Done." "$green"

    println "Creating worker2 Droplet..." "$green"
    WORKER2_ID=$(curl -X POST $DROPLETS_API\
       -d'{"name":"worker2","tags":["demo"],"region":"fra1",
       "size":"s-1vcpu-1gb","image":"docker-20-04",
       "ssh_keys":["35:27:02:78:dc:a4:80:b6:90:77:9d:19:f2:46:f4:13"]}'\
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
    ssh root@"$SWARM_MANAGER_IP" -i ~/.ssh/devops "ufw allow 22/tcp && ufw allow 2376/tcp &&\
    ufw allow 2377/tcp && ufw allow 7946/tcp && ufw allow 7946/udp &&\
    ufw allow 4789/udp && ufw reload && ufw --force  enable &&\
    systemctl restart docker"
    println "Done." "$green"

    println "Initialize the swarm..." "$green"
    ssh root@"$SWARM_MANAGER_IP" -i ~/.ssh/devops "docker swarm init --advertise-addr $SWARM_MANAGER_IP"
    ssh root@"$SWARM_MANAGER_IP" -i ~/.ssh/devops "docker swarm join-token worker -q"
    WORKER_TOKEN=`ssh root@"$SWARM_MANAGER_IP" -i ~/.ssh/devops "docker swarm join-token worker -q"`
    println "Done." "$green"

    println "Build a command that we can run on node-1 and node-2 to join the swarm...." "$green"
    REMOTE_JOIN_CMD="docker swarm join --token $WORKER_TOKEN $SWARM_MANAGER_IP:2377"
    ssh root@"$WORKER1_IP" -i ~/.ssh/devops "$REMOTE_JOIN_CMD"
    ssh root@"$WORKER2_IP" -i ~/.ssh/devops "$REMOTE_JOIN_CMD"
    println "Done." "$green"
}

function getClusterManagerState {
    println "Seeing the state of the cluster on the manager..." "$green"
    ssh root@"$SWARM_MANAGER_IP" -i ~/.ssh/devops "docker node ls"
    println "Done." "$green"
}

function startService {
    println "Running a service on our cluster..." "$green"
    ssh root@"$SWARM_MANAGER_IP" -i ~/.ssh/devops "docker service create -p 8080:8080 --name appserver stifstof/crashserver"
    println "Done." "$green"
}

function checkService {
    println "Checking the state of the service..." "$green"
    ssh root@"$SWARM_MANAGER_IP" -i ~/.ssh/devops "docker service ls"
    println "Done." "$green"

    println "Checking the state of the service (appserver)..." "$green"
    ssh root@"$SWARM_MANAGER_IP" -i ~/.ssh/devops "docker service ps appserver"
    println "Done." "$green"

    println "Open instance of manager ip in browser..." "$green"
    open http://"$SWARM_MANAGER_IP":8080
    println "Done." "$green"
}

function addScaling {
    println "Adding scaling..." "$green"
    ssh root@"$SWARM_MANAGER_IP" -i ~/.ssh/devops "docker service scale appserver=5"
    ssh root@"$SWARM_MANAGER_IP" -i ~/.ssh/devops "docker service ls"
    println "Done." "$green"

    println "See current services running on the appserver..." "$green"
    ssh root@"$SWARM_MANAGER_IP" -i ~/.ssh/devops "docker service ps appserver"
    println "Done." "$green"
}

function routingMeshWorksAsAdvertised {
    println "Open worker 1 browser..." "$green"
    open http://"$WORKER1_IP":8080
    println "Done." "$green"
    
    println "Open worker 2 browser..." "$green"
    open http://"$WORKER2_IP":8080
    println "Done." "$green"
}

function removeDroplets {
    println "Removing swarm droplets..." "$red"
    curl -X DELETE\
      -H "$BEARER_AUTH_TOKEN" -H "$JSON_CONTENT"\
      "https://api.digitalocean.com/v2/droplets?tag_name=demo"
    println "Done." "$red"
}

if [ "$#" -eq 0 ]; then
    createDockerSwarmClusterNode
    createWorkerNodes
    makeSwarmManagerClusterManager
    # getClusterManagerState
    # startService
    # checkService
    # addScaling
elif [ "$1" == "clean" ]; then
    removeDroplets
else
    println "Error: Unknown command" "$red"
    println "For a list of available commands type './run.sh -h'" "$red"
fi