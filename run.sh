#!/bin/bash

# Colors
red='\033[0;31m'
green='\033[0;32m'
clear='\033[0m'

function println {
    printf "%b%b\n%b" "$2" "$1" "$clear"
}

function start_backend {
    println "Starting mongodb container..." "$green"
    
    password=$(<./.local/db_password.txt) # Load db password
    volume=""
    
    if $1 ; then
        docker volume create mongodb_data_container_local
        volume="-v mongodb_data_container_local:/data/db"
    fi
    
    docker run --rm --name mongodb -d -p 27018:27017 "$volume" -e "MONGO_INITDB_ROOT_USERNAME=radiator" -e "MONGO_INITDB_ROOT_PASSWORD=$password" -e "MONGO_INITDB_DATABASE=MiniTwit" mongo:latest
    
    println "Starting backend..." "$green"
    dotnet run --project MiniTwit.Server

    println "Stopping and removing mongodb container..." "$red"
    docker stop mongodb
}

function setup_dotnet_secrets {
    println "Creating secrets..." "$green"

    password=$(<./.local/db_password.txt) # Load db password from file
    connectionString="mongodb://radiator:$password@localhost:27018"

    dotnet user-secrets init --project MiniTwit.Server
    dotnet user-secrets set "ConnectionStrings:MiniTwit" "$connectionString" --project MiniTwit.Server
}

function setup_elk {
    # Export env variables (set ELK_DIR to directory where this script is located)
    ELK_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )"
    ELK_USER="radiator"

    export ELK_DIR 
    export ELK_USER

    echo "export ELK_DIR=$ELK_DIR" >> ~/.profile
    echo "export ELK_USER=$ELK_USER" >> ~/.profile

    # Go to ELK stack folder
    (
        cd "$ELK_DIR"/Logging || exit
        # Change permissions on filebat config
        sudo chown root filebeat.yml 
        sudo chmod go-w filebeat.yml
    )
}

function print_help {
    println "--- RUN HELP ---"
    println "Available Commands:"
    println ">> Run the backend server and database with a mounted volume"
    println ">> nosave - Run the backend server and database with no mounted volume"
    println ">> secrets - Set up .NET user secrets for this project and saves a local developer connection string to the registry"
    println ">> elk - Set up ELK environment"
    println ">> inspectdb - Use flag_tool to retrieve all messages from the database"
    println ">> flag <message_id>... - Use flag_tool to flag one or more messages given their ids."
    println ">> -h - Print this page"
    println "----------------"
}

if [ "$#" -eq 0 ]; then
    start_backend true
elif [ "$1" = "nosave" ]; then
    start_backend false
elif [ "$1" = "secrets" ]; then
    setup_dotnet_secrets
elif [ "$1" = "elk" ]; then
    setup_elk
elif [ "$1" = "inspectdb" ]; then
    ./flag_tool -i | less
elif [ "$1" = "flag" ]; then
    ./flag_tool "${@:2}"
elif [ "$1" = "-h" ]; then
    print_help
else
    println "Error: Unknown command" "$red"
    println "For a list of available commands type './run.sh -h'" "$red"
fi
