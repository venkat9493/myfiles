#!/bin/bash
for repo in `cat repolist.txt`
do
        docker build -f /home/vsts/work/1/s/$repo/Dockerfile -t $repo:latest --build-arg PAT=l72rov4eumjuayozyxro4yzgatj4anhqi2mmxgjrayur4ucbxkyq /home/vsts/work/1/s/$repo 
        docker tag $repo:latest $repo:$(Build.BuildNumber)
done