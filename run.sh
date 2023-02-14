#!/bin/bash

# Colors
red='\033[0;31m'
green='\033[0;32m'
clear='\033[0m'

if [ "$#" -eq 0 ]; then
    docker-compose -f 'docker-compose.yml' up -d --build
    docker-compose -f 'docker-compose.yml' ps
    printf "\n${green}To stop the project write:\n"
    printf "'run stop'${clear}\n"
elif [ "$1" = "stop" ]; then
    printf "${green}Stopping container\n${clear}"
    docker-compose stop
else
    printf "${red}Error: Unknown command\n${clear}"
fi
