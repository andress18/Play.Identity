# Play.Identity.Contracts
Identity libraries used by Play Economy services

## Create and publish package
```powershell
$version="1.0.5"
$owner="dotnetMicroservicesCourseASGX"
$gh_pat="[PATHERE]"

dotnet pack src\Play.Identity.Contracts\ --configuration Release -p:PackageVersion=$version -p:RepositoryUrl=https://github.com/$owner/Play.Identity -o ..\packages.
dotnet nuget push ..\packages\Play.Identity.Contracts.$version.nupkg --api-key $gh_pat --source "github"
```

## Buid the docker image
```powershell
$env:GH_OWNER="dotnetMicroservicesCourseASGX"
$env:GH_PAT="[PAT HERE]"
$appname="playeconomyaxsg"
docker build --secret id=GH_OWNER --secret id=GH_PAT -t "$appname.azurecr.io/play.identity:$version" .
```

## Run the docker image
```powershell
$adminPass="[PASS HERE]"
$cosmosDbConnString="[CONN STRING HERE]"
$serviceBusConnString="[CONN STRING HERE]"
docker run -it --rm -p 5002:5002 --name identity -e MongoDbSettings__ConnectionString=$cosmosDbConnString -e ServiceBusSettings__ConnectionString=$serviceBusConnString -e ServiceSettings__MessageBroker="SERVICEBUS" -e IdentitySettings__AdminUserPassword=$adminPass play.identity:$version
```

## Publish the docker image
```powershell
az acr login --name $appname
docker push "$appname.azurecr.io/play.identity:$version"
```
## Create the kubernetes namespace
```powershell
$namespace="identity"
kubectl create namespace $namespace
```

## Create the kubernetes secrets
```powershell
kubectl create secret generic identity-secrets --namespace $namespace --from-literal=cosmosdb-connectionstring=$cosmosDbConnString --from-literal=servicebus-connectionstring=$serviceBusConnString --from-literal=admin-user-password=$adminPass -n $namespace
```
## delete the kubernetes secrets
```powershell
kubectl delete secret identity-secrets --namespace $namespace
```
## Create the kubernetes pod
```powershell
kubectl apply -f .\kubernetes\identity.yaml --namespace $namespace
```

## Get the kubernetes pod
```powershell
kubectl get pods --namespace $namespace
```

## Delete the kubernetes pod
```powershell
kubectl delete pod identity --namespace $namespace
```