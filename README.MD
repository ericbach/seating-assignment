# Loyalty (L2)

This repository holds the main source code of the Loyalty (L2) system.

![Build Status](https://codebuild.us-west-2.amazonaws.com/badges?uuid=eyJlbmNyeXB0ZWREYXRhIjoiYnpMdFZwbWprTG5velNqUXgrN0tJcVFWaW5kQStiTE0wb05JQ1VDMldhcmJTSjF6WjJSTzV4bkNrWC9tWnRpNkRhTDVCcEhaN1V1RmRMaE1UeHFJQUlZPSIsIml2UGFyYW1ldGVyU3BlYyI6IlRYZmdMUFRodC9rMUtUQ0giLCJtYXRlcmlhbFNldFNlcmlhbCI6MX0%3D&branch=master)

# Get started Developer Stack in AWS

_This section is intended for developers new to this project. If you have previously deployed your Stack, please see the [Subsequent Deployments](#subsequent-deployments) section._

Sharing an AWS organization with multiple developers and a Staging environment means having to ensure there is no naming conflicts. The convention we chose is to prefix all resource names with the developers initials. For example the `loyalty-GetBalanceWebApi` of the Staging environment would be named `eb-loyalty-GetBalanceWebApi` when deployed to Eric Bach's stack etc.

To avoid having to manually edit the SAM Template there is a PowerShell script:

- **deploySamTemplate.ps1**: will take the samTemplate.yaml file and  prefix all resourceNames and update the API Gateway URL with the values specified in the **developers.json** file. The resources will then be deployed to AWS - creating or updating your AWS CloudFormation stack.

## Pre-requisites

- aws CLI installed and configured with green team account profiles

## Setup Ubuntu for local deployment (preferred)

**Warning:** This method is the simpliest to setup but it does not mirror the AWS environment exactly so there could be instances where things work here but not once deployed to AWS.

- Install Ubuntu from the Microsoft Store
- Launch Ubuntu
- Within Ubuntu run the following commands:

```
Update APT packages
$> sudo apt update

Install Python 3
$> sudo apt install python3-pip
$> pip3 --version

Install the AWS CLI
$> pip3 install awscli
$> aws configure (follow prompts to enter AWS credentials for each AWS account) 
```

- Follow the guide to install the .NET SDK
(https://dotnet.microsoft.com/download/linux-package-manager/ubuntu18-04/sdk-2.2.300)

```
Install the AWS dotnet CLI
$> dotnet tool install --global Amazon.Lambda.Tools

Install PowerShell
$> sudo apt install powershell

Setup your sybolic links to your repos (optional)
$> ln -s /mnt/c/AMAABCA /src
```

## Setup Docker for local deployment (optional)

**Warning:** This is the best option as it mirrors the AWS CodeBuild environment exactly so there will never be environmental discrepancies.
However, with this option you will need to configure the Docker image everytime you start it.

- Install Docker Desktop (https://www.docker.com/get-started)
- Build the AWS CodeBuild Docker Image (https://github.com/aws/aws-codebuild-docker-images) **Note:** AWS frequently updates this
- Run AWS CodeBuild Docker Image:

```
$> docker run -it --mount type=bind,source=<path to local Loyalty repo>,target=/Loyalty <your container ID> /bin/bash
```

- Within the AWS CodeBuild image run the following commands:

```
Install the AWS CLI
$> pip3 install awscli
$> aws configure (follow prompt to enter your AWS credentials)
```

- Follow the guide to install the .NET SDK
(https://dotnet.microsoft.com/download/linux-package-manager/ubuntu18-04/sdk-2.2.300)

```
Install the AWS dotnet CLI
$> dotnet tool install --global Amazon.Lambda.Tools

Install PowerShell
$> sudo apt install powershell
Install vim (or any text editor of your preference)
$> sudo apt install vim
```

- Configure your PATH (or export PATH="$PATH:/root/.dotnet/tools")
- Add PATH=$PATH:~/.dotnet/tools;export PATH; to the ~/.bashrc file
```
Re-initialize /bin/bash
$> source .bashrc
```

## Deploy a local stack

The **developers.json** allows each developer to create a configuration profile for their stack. The PowerShell deploy script reads the configuration values from this file.

This allows each developer to set values specific to their local stack.

```
    "Eric": {
        "Prefix": "eb",
        "ApiGateway": "https://rvq14wj7a7.execute-api.us-west-2.amazonaws.com/Prod"
    }
```

## Configure a local Stack

If you are just getting started and you have never deployed your own CloudFormation Stack to AWS, you will not yet have an `apiGateway` which you. Just leave it blank for now.

### First Deployment or Update to NuGet Packages

If this is your first Loyalty deployment or if you need to update the  NuGet packages of any project, you will need to run with `-updateLayers y`.  This requires running the deployment within Ubuntu or a local AWS CodeBuild Docker image.  The advantage of the AWS CodeBuild Docker image is that it mirrors the exact environment that the application will be running under so it minimizes the chance of environment discrepancies.

- Run the command lines to deploy the stack

```
$> pwsh ./deploySamTemplate.ps1 --configName <your config name> -updateLayers y
```

- This process will take 15+ minutes to finish. Feel free to get a coffee and a donut or two then come back.

```
$> .\deploySamTemplate.ps1
   configName: Eric

   Provided parameters:
      developerPrefix:       eb
      apiGatewayBasePath:    https://rvq14wj7a7.execute-api.us-west-2.amazonaws.com/Prod
```

- You now have a local sam template where all resources have been prefixed with your initials.
- Once the local sam template is created, press `[enter]` to continue deploying to AWS

After building, packaging and uploading to S3, CloudFormation will start to apply the SAM Template. If there are any errors they will show in the console but could also be found in the AWS Console > CloudFormation. 

The console will show: **YOUR STACK NAME IS:** `{yourPrefix}-app-stack`

- Navigate to https://us-west-2.console.aws.amazon.com/apigateway/home?region=us-west-2#
- On the left hand navigation pick your stack name
- Select `Stages`
- On the new side-bar select `Prod` (don't expand the item, but click the text)
- Copy the `Invoke URL` from the center top of the screen. i.e. https://q28x0v6266.execute-api.us-west-2.amazonaws.com/Prod

**Important** Be sure to update your own `apiGateway` in **developers.json** once deployment is complete.

### Subsequent Deployments

- run `.\deploySamTemplate.ps1` and enter your config name specified in the `developers.json` file

Now we need to update the **developers.json** with your configuration values and rerun the script.
