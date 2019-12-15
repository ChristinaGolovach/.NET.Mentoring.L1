## How to run app

### module_6
#### Using docker

Steps:
- build solution
- install [docker](https://docs.docker.com/docker-for-windows/install/)
- after instaling [switch to Windows containers](https://docs.docker.com/docker-for-windows/#docker-settings-dialog)
- open cmd in the folder with docker file and create docker image ```docker build -t your_image_name .```
##### example   
![alt-текст](https://github.com/ChristinaGolovach/.NET.Mentoring.L1/blob/module_6/Potestas/docker1.png) 

#### result
![alt-текст](https://github.com/ChristinaGolovach/.NET.Mentoring.L1/blob/module_6/Potestas/docker2.png) 

- run docker container based on the previously created image

```docker run -p 27017:27017 --name your_container_name your_image_name```

##### example
![alt-текст](https://github.com/ChristinaGolovach/.NET.Mentoring.L1/blob/module_6/Potestas/docker3.png) 

- run Potestas.App.Terminal project

links:
https://www.scalyr.com/blog/create-docker-image/
https://docs.docker.com/engine/reference/commandline/run/

#### Using direct mongo installation 

Steps:
- build solution
- download [mongodb](https://www.mongodb.com/download-center/community)
- after downloading the archive package, unpack it into the folder C:\mongodb
- create folders: C:\data\db
- run MongoDB server: open cmd and enter ```cd C:\mongodb\bin``` after enter ```mongod```
- replace 
**connectionString="mongodb://host.docker.internal:27017/observation"** to the **connectionString="mongodb://localhost/observation"**
in the Potestas.Apps.Terminal/App.config file
- run Potestas.App.Terminal project
