<h1 align="center">GitLabKit</h1>
<p align="center">Toolkit for GitLab server admins and GitLab feature developers!</p>

## Runner Admin
Better way to see and control your GitLab self-hosted runners.

### Why?
Take a look at current GitLab runner page in group's CI/CD settings.
You can see all runner names and basic infomation but that's it.
You can rename runners, pause/resume/delete them only individually.
Most importantly, it could only display 4 runners at a time. (as of version 14.10)

Some organizations might have 100+ runners and want to take actions
with all of them or at least runners with some specific tags at once.
Some also embrace ephemeral runners where they are periodically destroyed and respawned. 
This helps mitigate "state drift" issue on servers.
*(The state drift issue might be addressed by using docker runner but some still prefer VM/baremetal runner
for performance gain or job startup time.)*

Status report is another area that's missing.
GitLab doesn't display if a runner is running a job or being idle.
Let alone job history for each runner that could help identifying faulty runners.

GitLabKit Runner Admin can help improve experience of managing runners.

### How to use?
#### Main page
Put your group ID in text box and hit enter to go to group page. Simple enough.
![main-page](/doc/images/main-page.png)

#### Group page
Displays all runner in the group
- Orange row mean the runner is running a job displaying on the right
- Red row mean the runner is offline
- Use the toggle in front of runner name to individually enable/disable it
- Click on job name on the right to navigate back to job page on GitLab
- Click on runner name for runner history page
![group-page](/doc/images/group-page.png)

##### Filters!
You could filter runners by tags, online status and active/paused status.
Filters can be combined too! 
And number in brackets show you runner counts for that tag/status given current active filter criteria.

##### Sort
Simple enough

##### Action buttons
At the bottom of group page you can take action on runners being displayed.
So you can filter with some criteria and enable, disable or delete them.

![group-page-in-action](/doc/images/group-page-in-action.gif)


#### Runner page
From group page, click on any runner name to see its job history.
- Green row = Succeeded job
- Red row = Failed job
- Orange row = Running job
- Neutral row = Cancelled job
- Time in bracket on the far left = Job duration
- Click on job name on the right to navigate back to job page on GitLab

![runner-page](/doc/images/runner-page.png)


### Installation
#### Prerequiresite
GitLabKit Runner Admin requires `redis` for caching result from GitLab server. This helps reduce loads to server and improve user experience.

#### Run!
GitLabKit Runner Admin is published as a docker image.

You can try running by 2 docker commands
```shell
$ docker run -p 16379:6379 redis
```
```shell
$ docker run \
  -e CONNECTIONS__GITLABSERVER="https://<your-gitlab-server>" \
  -e SECRETS__GITLABTOKEN="<gitlab-token>" \
  -e CONNECTIONS__REDISSERVER="localhost:16379" \
  -p 80:80 \
  encx/gitlabkit-runner
```

or `docker-compose` file
```yaml
version: '3.8'

services:
  gitlabkit-runner-admin:
    image: encx/gitlabkit-runner
    environment:
      - CONNECTIONS__GITLABSERVER="https://<your-gitlab-server>"
      - SECRETS__GITLABTOKEN="<gitlab-token>"
      - CONNECTIONS__REDISSERVER="redis:6379"
      - ASPNETCORE_URLS=http://+:80
    ports: ["80:80"]

  redis:
    image: redis
    ports: [6379]
```

of course, you could also run this in Kubernetes environment and have multiple pods of it since the Runner Admin doesn't hold any state.


#### Configuration
Settings could be passed to Runner Admin's container using these environment variables

| variable | example | required | description |
|---|---|---|---|
| `CONNECTIONS__GITLABSERVER` | `https://gitlab.yourcompany.com` | yes | GitLab server URL |
| `CONNECTIONS__REDISSERVER` | `redis-server:16379` | yes | Redis host |
| `SECRETS__GITLABTOKEN` | `rG2f93ddaz` | yes | GitLab token. Could be a personal token or group token that has sufficient permission to view CI/CD settings in the group |
| `LOGTARGETS__SEQ` | `http://seq-server:5341` | no | Runner Admin supports [Seq](https://datalust.co/seq) logger. Use this to set log ingestion URL. |
| `APPLICATIONINSIGHTS__CONNECTIONSTRING` | - | no | Runner Admin supports [Application Insights](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview) to see application diagnostics. Use this to set connection string from Azure dashboard. |


---

## REST API Client (coming soon)
Full suite GitLab REST API client.
No more manual HTTP clients.

Will be available on .net, TypeScript, JVM languages and more.

---

*This project is in its early phase. More modules to come.*

For contributors, please read [CONTRIBUTING.md](CONTRIBUTING.md)