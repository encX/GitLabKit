# How to contribute to GitLab Runner Admin?

## Dev environment setup
### Prerequiresite
1. .net6 SDK
1. Docker
1. node.js (recommended version 16+)

### Setup steps
1. Clone project (of course)

Done!

### How to run the project
#### Server
1. Start redis by `cd src` then `docker-compose up -d redis`
1. Open `src/GitLabKit.sln` with .net IDE of choice.
1. Provide GitLab token to the app by add file in project `GitLabKit.Runner.Web` called `appsettings.Secrets.json` with content.
```json
{
  "Secrets": {
    "GitLabToken": "place your gitlab token here"
  }
}
```
This file will be ignored by git. 
Make sure you have sufficient permission to get CI/CD settings for your group.
Fail to provide valid API key will make API not able to fetch data from GitLab.

Then, there's only 1 launch setting so start it and it will serve API on port `8888`

#### Client
1. Open `src/clientside` with JavaScript editor of choice
1. Run `yarn` to install dependencies
1. Run `yarn start` to start the dev website on port `3000`

Then the clientside will forward API requests to `localhost:8888` automatically

### How to make endpoint changes
- Client code that fetch data from server side is generated using `openapi-generator`.
- Server is serving openapi/swagger specs generated from controllers code using  `swashbuckle`
- So you have to make changes to controllers first.
- Then start the server
- And finally, in `src/clientside` run `yarn gen-client` and you'll see code changes in `src/clientside/src/api-client` (DO NOT MAKE ANY CHANGES MANUALLY IN THIS FOLDER!)
- Then, you can use new/modified endpoints/models in client side code
- Don't forget to include generated change in commit

## Design and Architecture
TODO