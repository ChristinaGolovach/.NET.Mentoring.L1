## How to run app


#### Using docker

Steps:
- install [docker] (https://docs.docker.com/docker-for-windows/install/)
- after instaling [switch to Windows containers](https://docs.docker.com/docker-for-windows/#docker-settings-dialog)
- open cmd in the folder with docker file and create docker image ```docker build -t your_image_name .```
example   
![alt-текст](https://github.com/ChristinaGolovach/.NET.Mentoring.L1/blob/module_6/Potestas/docker1.png) 
result
![alt-текст](https://github.com/ChristinaGolovach/.NET.Mentoring.L1/blob/module_6/Potestas/docker2.png) 
- run docker container based on the previously created image ```docker run -p 27017:27014 --name your_container_name your_image_name```
example
![alt-текст](https://github.com/ChristinaGolovach/.NET.Mentoring.L1/blob/module_6/Potestas/docker3.png) 
