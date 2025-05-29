# Play.Identity.Contracts
Identity libraries used by Play Economy services

## Create and publish package
```powershell
$version="1.0.3"
$owner="dotnetMicroservicesCourseASGX"
$gh_pat="[PATHERE]"

dotnet pack src\Play.Identity.Contracts\ --configuration Release -p:PackageVersion=$version -p:RepositoryUrl=https://github.com/$owner/Play.Identity -o ..\packages
dotnet nuget push ..\packages\Play.Identity.Contracts.$version.nupkg --api-key $gh_pat --source "github"
```

## Buid the docker image
```powershell
$env:GH_OWNER="dotnetMicroservicesCourseASGX"
$env:GH_PAT="[PAT HERE]"
docker build --secret id=GH_OWNER --secret id=GH_PAT -t play.identity:$version .
```

## Run the docker image
```powershell
$adminPass="[PASS HERE]"
$cosmosDbConnString="[CONN STRING HERE]"
docker run -it --rm -p 5002:5002 --name identity -e MongoDbSettings__ConnectionString=$cosmosDbConnString -e RabbitMqSettings__Host=rabbitmq -e IdentitySettings__AdminUserPassword=$adminPass --network playinfra_default play.identity:$version
```