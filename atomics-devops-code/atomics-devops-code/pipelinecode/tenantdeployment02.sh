!/bin/bash
accountId="084273643838";
tenantawsId="140235737109";
certificateId="51b8c9b8-85e3-485b-a06a-1bd6b8fb3689";
tenantId="";
envName="dev";
ecrName="ecr-usw2-atmx-mt-dev-dev-hub-ah001-atomx-services";
aspEnvName="devhub";
buildNo="20221115.3";
BuildNumber="20221115.3";
BuildId="20221115.3";
ReleaseName="develop";
eksclustername="eks-usw2-hcl-dev-dev-hub-ah001";
ingclustername="eks-usw2-atmx-ingestion-dev-t-2-t002";
tenantName="tenant2"
tenantId="tenant02"
awsProfile="nstg-tenant02-dev"
set -x
#cd $System.DefaultWorkingDirectory/_atomics-devops-code/pipelinecode

export AWS_PROFILE=$awsProfile
#echo "cat repolist.txt"
sed -i 's/\r//' repolist.txt
for repo in `cat repolist.txt`
do

if [ $repo == "Meridian-Angular-UI" ]
then
    mkdir -p release-dir; cp -rf deployment-ingress-devhub.yml release-dir/deployment-ingress.yml
    sed -i "s/#{accountId}#/$accountId/g"  release-dir/deployment-ingress.yml;
    sed -i "s/#{certificate-id}#/$certificateid/g" release-dir/deployment-ingress.yml;
    # sed -i "s/#{tenantId}#/$tenantId/g" release-dir/deployment-ingress.yml;
    sed -i "s/#{envName}#/$envName/g" release-dir/deployment-ingress.yml;
    sed -i "s/#{ecrName}#/$ecrName/g" release-dir/deployment-ingress.yml;
    sed -i "s/#{aspEnvName}#/$aspEnvName/g" release-dir/deployment-ingress.yml;
    sed -i "s/#{buildNo}#/meridian-angular-ui-$ReleaseName-$buildNo/g" release-dir/deployment-ingress.yml;
    sed -i "s/#{Build.BuildNumber}#/$ReleaseName-$buildNo/g" release-dir/deployment-ingress.yml
    sed -i "s/#{Build.BuildId}#/$BuildId/g" release-dir/deployment-ingress.yml;
    sed -i "s/#{Release.ReleaseName}#/$ReleaseName/g" release-dir/deployment-ingress.yml;

    echo "setting cluster mode"
    aws eks update-kubeconfig --name $eksclustername --region us-west-2
    echo "Applying manifest file for $repo"
    cp -rf release-dir/deployment-ingress.yml .
    kubectl apply -f deployment-ingress.yml -n atomx-env
    echo "Cleaning manifest file"
    rm -rf release-dir/deployment-ingress.yml deployment-ingress.yml
else
    mkdir -p release-dir;
     cp -rf deployment-tenant01.yml release-dir/$repo-deployment.yml
    #cp -rf release-dir/deployment.yml .;
    sed -i "s/#{accountId}#/$accountId/g"  release-dir/$repo-deployment.yml;
    sed -i "s/#{tenantawsId}#/$tenantawsId/g"  release-dir/$repo-deployment.yml;
    # sed -i "s/#{tenantId}#/$tenantId/g" release-dir/$repo-deployment.yml;
    sed -i "s/#{certificateId}#/$certificateId/g" release-dir/$repo-deployment.yml;
    sed -i "s/#{envName}#/$envName/g"  release-dir/$repo-deployment.yml;
    sed -i "s/#{ecrName}#/$ecrName/g" release-dir/$repo-deployment.yml;
    sed -i "s/#{aspEnvName}#/$aspEnvName/g" release-dir/$repo-deployment.yml;
    sed -i "s/app-name/$repo/g"  release-dir/$repo-deployment.yml;
    sed -i "s/#{buildNo}#/$repo-$ReleaseName-$BuildNumber/g" release-dir/$repo-deployment.yml;
    sed -i "s/#{Build.BuildNumber}#/$ReleaseName-$BuildNumber/g" release-dir/$repo-deployment.yml
    sed -i "s/#{Build.BuildId}#/$BuildId/g" release-dir/$repo-deployment.yml;
    sed -i "s/#{Release.ReleaseName}#/$ReleaseName/g" release-dir/$repo-deployment.yml;
    sed -i "s/#{tenantName}#/$tenantName/g" release-dir/$repo-deployment.yml;
    sed -i "s/#{tenantId}#/$tenantId/g" release-dir/$repo-deployment.yml;
    echo "setting cluster mode"

    aws eks update-kubeconfig --name $ingclustername --region us-west-2

    echo "Applying manifest file for $repo"
    cp -rf release-dir/$repo-deployment.yml .
    kubectl apply -f $repo-deployment.yml -n atomx-env
    echo "Cleaning manifest file"
    rm -rf release-dir/$repo-deployment.yml $repo-deployment.yml
  fi
done
