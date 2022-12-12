## Programación Avanzada en Web (3101)
### Proyecto 1 - Alberto Sierra Reales
[Video de Presentación](https://youtu.be/vsYgIvDoBVc)

## Manual de instalación
Esta aplicación fue desarrollada en .NET 6 utilizando el sistema operativo MacOS y desplegado para producción en servidores Linux, por lo que este manual está limitado
a la instalación de la aplicación en un ambiente Linux con un proxy inverso en Nginx.

### Pre-requisitos
- Motor de base de datos MS SQL 2019 o superior
- Linux Debian versión 10 o superior
- Certificado SSL
- Conexión a internet
- Acceso a la consola del sistema operativo con acceso root

### Instalación del .NET runtime
Para el despliegue de aplicaciones en producción, .NET permite la construcción de binarios con todas las dependencias incorporadas o sin ellas.
Para este proyecto se ha elegido un despliegue sin dependencias, por lo que es necesario instalar el `runtime` .NET para la correcta ejecucción de la aplicación.

1. Configuración del depositorio de Microsoft

Ejecute los siguientes comandos como root en la consola:
```
wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
```

3. Instalación del `runtime` .NET
Ejecute los siguientes comandos como root en la consola:
```
apt update && \
apt install -y aspnetcore-runtime-6.0 nginx
```

4. Crear los script de systemd
Ejecute los siguientes comandos como root en la consola:
```
cat << EOF > /etc/systemd/system/api.service
[Unit]
After=network.service

[Service]
WorkingDirectory=/opt/webapp
ExecStart=/bin/dotnet API.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=dotnet-webapp
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
EOF
```

5. Configurar el proxy inverso
```
cat << EOF > /etc/nginx/conf.d/<servicio>.conf
server {
  listen 443 ssl http2;
  server_name _;

  ssl_certificate /path/to/certificate/fullchain.pem;
  ssl_certificate_key /path/to/key/privkey.pem;

  location / {
    proxy_read_timeout 3600;
    proxy_pass http://localhost:5000;
  }

}
EOF
```

6. Habilitar e iniciar los servicios
```
systemctl daemon-reload
systemct enable <servicio> 
systemctl start <servicio>

systemctl enable nginx
systemctl start nginx
```

7. Repetir los pasos 4-6 para cada uno de los módulos del proyecto (API, Frontend, Backend)

### SQL Server en Docker
Para el ambiente de desarrollo se recomienda utilizar una instancia de SQL Server Express en Docker. Un comando como el siguiente sería suficiente:
```
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=MyStrongPassword" -e "MSSQL_PID=Express" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
```

Para ambientes de producción hay que observar la persistencia de los datos con Docker.
