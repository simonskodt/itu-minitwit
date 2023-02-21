#!/bin/bash

# Colors
red='\033[0;31m'
green='\033[0;32m'
clear='\033[0m'

if [ "$#" -eq 0 ]; then
    printf "${green}Starting mongodb container...\n${clear}"
    password=$(<./.local/db_password.txt) # Load db password from file
    docker volume create mongodb_data_container_local
    docker run --rm --name mongodb -d -p 27018:27017 -v mongodb_data_container_local:/data/db -e "MONGO_INITDB_ROOT_USERNAME=radiator" -e "MONGO_INITDB_ROOT_PASSWORD=$password" -e "MONGO_INITDB_DATABASE=MiniTwit" mongo:latest
    printf "${green}To stop the container write 'docker stop mongodb'\n${clear}"
elif [ "$1" = "nosave" ]; then
    printf "${green}Starting mongodb container...\n${clear}"
    password=$(<./.local/db_password.txt) # Load db password from file
    docker run --rm --name mongodb -d -p 27018:27017 -e "MONGO_INITDB_ROOT_USERNAME=radiator" -e "MONGO_INITDB_ROOT_PASSWORD=$password" -e "MONGO_INITDB_DATABASE=MiniTwit" mongo:latest
    printf "${green}Starting backend...\n${clear}"
    dotnet run --project MiniTwit.Server
    printf "${red}Stopping and removing mongodb container...\n${clear}"
    docker stop mongodb
elif [ "$1" = "secrets" ]; then
    printf "${green}Creating secrets...\n${clear}"
    project="MiniTwit.Server"
    user="radiator"
    password=$(<./.local/db_password.txt) # Load db password from file
    connectionString="mongodb://$user:$password@localhost:27018"

    dotnet user-secrets init --project $project
    dotnet user-secrets set "ConnectionStrings:MiniTwit" "$connectionString" --project $project
else
    printf "${red}Error: Unknown command\n${clear}"
fi
