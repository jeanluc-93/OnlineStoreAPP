## Overview

This is an implementation of an OnlineStore is a dotnet 6 ASP.NET Web App.

## How to run

# Copy to local machine.
Make a local copy of the repository on your local machine.

# Build docker container

Navigate to the root of the solution.

Run the following command.

`docker build -t onlinestoreapp .`

## Run container

`docker run -d -p 8080:80 your-image-name`

## Access the web app

The application should now be running.
You can access it by opening a web browser and navigating to http://localhost:8080.