# How to contribute to GitLabKit Runner Admin?

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
  "Connections": {
    "GitLabServer": "https://<your-gitlab-server>"
  },
  "Secrets": {
    "GitLabToken": "<your-gitlab-token>"
  }
}
```
This file will be ignored by git. 
Make sure you have sufficient permission to get CI/CD settings for your group.
Fail to provide valid API key will make API not able to fetch data from GitLab.
(you can also store other settings you don't want to include in git too!, see `appsettings.json` for complete configs)

Then, there's a launch setting called `Dev` for project `GitLabKit.Runner.Web`.
Start it and API will be served on port `8888`

#### Client
1. Open `src/clientside` with JavaScript editor of choice
1. Run `yarn` to install dependencies
1. Run `yarn start` to start the dev website on port `3000`

Then the clientside will forward API requests to `localhost:8888` automatically

#### Mock GitLab server
In case if you need to not connect to GitLab.
You can run mock server by start the launch setting `Mock` for project `GitLabKit.Runner.Mock`.
Mock will be served on port `9999` by default, you can change this in `launchSettings` if needed.

Don't forget to change gitlab server settings in web project to call the mock.

#### Docker
As an alteranative to setup above, you can also run the project using predefined `docker-compose.yml` file. Update settings in `env` and just `docker-compose up`.
This will build production-like image and stand everything up incluing mock.

### Passing env to appsettings
This project use .net env config provider.
So everything you see in appsettings can be passed from env.
But it requires double underscores for setting properties in objects.
For example `{ foo: { bar: "value" } }` becomes `FOO__BAR=value`. 
Read the [doc](https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration-providers#environment-variable-configuration-provider) to learn more.

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