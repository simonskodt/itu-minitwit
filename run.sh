#!/bin/bash

# Colors
red='\033[0;31m'
green='\033[0;32m'
clear='\033[0m'

if [ "$#" -eq 0 ]; then
    password=$(<./.local/db_password.txt) # Load db password from file
    docker run --name mongodb -d -p 27018:27017 -e "MONGO_INITDB_ROOT_USERNAME=radiator" -e "MONGO_INITDB_ROOT_PASSWORD_FILE=$password" -e "MONGO_INITDB_DATABASE=MiniTwit" mongodb/mongodb-community-server:6.0-ubi8
elif [ "$1" = "stop" ]; then
    printf "${green}Stopping container\n${clear}"
    docker-compose stop
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
