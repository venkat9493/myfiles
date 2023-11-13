#!/bin/bash

set -x
cd $(System.DefaultWorkingDirectory)/_atomics-devops-code/pipelinecode


#echo "cat repolist.txt"
sed -i 's/\r//' repolist.txt
for repo in `cat repolist.txt`
do

if [[ $repo == "Meridian-Angular-UI" ]]
  then
    mkdir -p release-dir; cp -rf deployment-ingress-beta.yml release-dir/deployment-ingress.yml
    sed -i "s/#{accountId}#/$(accountId)/g"  release-dir/deployment-ingress.yml; 
    sed -i "s/#{certificate-id}#/$(certificate-id)/g" release-dir/deployment-ingress.yml; 
    sed -i "s/#{tenantId}#/$(tenantId)/g" release-dir/deployment-ingress.yml; 
    sed -i "s/#{envName}#/$(envName)/g" release-dir/deployment-ingress.yml;
    sed -i "s/#{ecrName}#/$(ecrName)/g" release-dir/deployment-ingress.yml;
    sed -i "s/#{aspEnvName}#/$(aspEnvName)/g" release-dir/deployment-ingress.yml;
	sed -i "s/#{buildNo}#/meridian-angular-ui-RC3B_dtone_AtoMx1.0_20220907.0-$(Build.BuildNumber)/g" release-dir/deployment-ingress.yml;
    sed -i "s/#{Build.BuildNumber}#/RC3B_dtone_AtoMx1.0_20220907.0-$(Build.BuildNumber)/g" release-dir/deployment-ingress.yml;
    sed -i "s/#{Build.BuildId}#/$(Build.BuildId)/g" release-dir/deployment-ingress.yml;
    sed -i "s/#{Release.ReleaseName}#/$(Release.ReleaseName)/g" release-dir/deployment-ingress.yml;

    echo "setting cluster mode"
    aws eks update-kubeconfig --name eks-usw2-atmx-hcl-beta-mayo-88a03 --region us-west-2
    echo "Applying manifest file for $repo"
    cp -rf release-dir/deployment-ingress.yml .
    kubectl apply -f deployment-ingress.yml -n atomx-env
    echo "Cleaning manifest file"
    rm -rf release-dir/deployment-ingress.yml deployment-ingress.yml
  else
    mkdir -p release-dir; 
     cp -rf deployment-beta.yml release-dir/$repo-deployment.yml
    #cp -rf release-dir/deployment.yml .;
    sed -i "s/#{accountId}#/$(accountId)/g"  release-dir/$repo-deployment.yml;
    sed -i "s/#{tenantId}#/$(tenantId)/g" release-dir/$repo-deployment.yml; 
    sed -i "s/#{envName}#/$(envName)/g"  release-dir/$repo-deployment.yml;
    sed -i "s/#{ecrName}#/$(ecrName)/g" release-dir/$repo-deployment.yml;
    sed -i "s/#{aspEnvName}#/$(aspEnvName)/g" release-dir/$repo-deployment.yml;
    sed -i "s/app-name/$repo/g"  release-dir/$repo-deployment.yml; 
	sed -i "s/#{buildNo}#/$repo-RC3B_dtone_AtoMx1.0_20220907.0-$(Build.BuildNumber)/g" release-dir/$repo-deployment.yml;
    sed -i "s/#{Build.BuildNumber}#/RC3B_dtone_AtoMx1.0_20220907.0-$(Build.BuildNumber)/g" release-dir/$repo-deployment.yml;
	sed -i "s/#{Build.BuildId}#/$(Build.BuildId)/g" release-dir/$repo-deployment.yml;
	sed -i "s/#{Release.ReleaseName}#/$(Release.ReleaseName)/g" release-dir/$repo-deployment.yml;
	
    echo "setting cluster mode"

    aws eks update-kubeconfig --name eks-usw2-atmx-hcl-beta-mayo-88a03 --region us-west-2

    echo "Applying manifest file for $repo"
    cp -rf release-dir/$repo-deployment.yml .
    kubectl apply -f $repo-deployment.yml -n atomx-env
    echo "Cleaning manifest file"
    rm -rf release-dir/$repo-deployment.yml $repo-deployment.yml
  fi
done