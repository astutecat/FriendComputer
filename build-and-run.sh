#!/bin/bash

sudo adduser --disabled-password --gecos "" --shell /sbin/nologon friend-computer
sudo mkdir -p /home/friend-computer/db
sudo cp ~/.friend-computer.environment /home/friend-computer/.friend-computer.environment
sudo chown -R friend-computer:friend-computer /home/friend-computer
docker build . -t friend-computer
docker stop friend-computer && docker rm friend-computer
docker run -d --user $(id -u friend-computer):$(id -g friend-computer) --name friend-computer -v /home/friend-computer/db:/app/db  --env-file /home/friend-computer/.friend-computer.environment friend-computer
