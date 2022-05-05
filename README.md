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
GitLabKit Runner Admin is published as a docker image.
You can try running it by docker command
```shell
docker run \
-p 80:80 \
```
or docker-compose file
```yaml
```
#### Configuration

---

## REST API Client (coming soon)
Full suite GitLab REST API client.
No more manual HTTP clients.

Will be available on .net, TypeScript, JVM languages and more.

---

*This project is in its early phase. More modules to come.*

For contributors, please read [CONTRIBUTING.md](CONTRIBUTING.md)