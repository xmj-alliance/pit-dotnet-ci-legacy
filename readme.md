# Dotnet CI Legacy

This cog presents the previous ideas of CI pipeline design, Kubernetes, docker-compose and dockerfile for dotnet core projects

The sample projects are provided under `server` folder. Try to keep them as simple as possible.

## Console App

### Docker
`consoleApp.dockerfile` shows a way of building a dotnet console application.

This app requires neither volumes to mount nor ports to open.

``` shell

# Build
docker build -f ./consoleApp.dockerfile -t dotnet-console-app .

# Run
docker run dotnet-console-app

```

## Web App

### Docker
`webApp.dockerfile` includes a common way of building an ASP.Net web server image.

Two config files can be provided: `appsettings.json` and `secrets.json`.

Both of them may be optional according to your project.

However, the server's host and port usually need to be configured. Otherwise, you may have trouble accessing your server inside a docker container. To do this, add the following sections inside your `appsettings.json`.

``` json
{
  "AllowedHosts": "*",
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://0.0.0.0:5000"
      },
    }
  }
}
```

When your app is behind a reverse proxy (e.g. nginx), the `basePath` field should be specified. 

For example, if your nginx server is configured like this:

``` conf
location /dwa {
    proxy_pass http://dotnet-web-app-c1:5000;
    proxy_http_version 1.1;
    proxy_set_header Upgrade $http_upgrade;
    proxy_set_header Connection 'upgrade';
    proxy_set_header Host $server_name;
    proxy_set_header X-Real-IP $remote_addr;
    proxy_cache_bypass $http_upgrade;
}
```

Then, you need to add the following sections inside your `appsettings.json`.

``` json
{
  "basePath": "/dwa"
}
```

(Note: This is not an ASP.Net official feature. This is achieved via the following code in `Startup.cs`:)

``` cs
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // ...
    string basePath = Configuration.GetSection("basePath").Value;
    if (basePath is null)
    {
        basePath = "/";
    }
    app.UsePathBase(basePath);
    // ...
}
```

The commands below display the common way of building the docker image and run it.

``` shell

# Build
docker build -f ./webApp.dockerfile -t dotnet-web-app .

# Run
docker run -d \
--name dotnet-web-app-c1 \
--network vps-main-network \
-p [port]:5000 \
-v /path/2/appsettings.json:/workspace/www/WebApp/appsettings.json \
-v /path/2/secrets.json:/workspace/www/WebApp/secrets.json \
dotnet-web-app

```