#!/bin/bash
echo "Gathering values"
read -p "Enter Omega Async Task Processor User (default omegaservice): "  username
username=${username:-omegaservice}
password_default=$(uuidgen)
read -p "Enter Omega Async Task Processor Password (default $password_default): "  password
password=${password:$password_default}
read -p "Enter Omega Async Task Processor Working Directory (default $PWD): "  directory
directory=${directory:-$PWD}
echo "Values gathered"

echo "Creating user"
useradd -m $username -p $password
echo "User created"

echo "Creating service"
echo "[Unit]  
Description=Omega Async Task Processor
  
[Service]  
ExecStart=/bin/dotnet/dotnet MD.Tools.AsyncTask.Processor.dll  
WorkingDirectory=$directory
User=omegaservice  
Group=omegaservice  
Restart=on-failure  
SyslogIdentifier=dotnet-sample-service  
PrivateTmp=true  
  
[Install]  
WantedBy=multi-user.target" > /etc/systemd/system/omega-async-task-processor-service.service

systemctl daemon-reload  
systemctl enable omega-async-task-processor-service.service
systemctl start dotnet-sample-service.service  