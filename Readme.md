# Play.Identity.Contracts
Identity libraries used by Play Economy services

## Create and publish package
```powershell
$version="1.0.7"
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
docker run -it --rm -p 5002:5002 --name identity 
-e MongoDbSettings__ConnectionString=$cosmosDbConnString 
-e ServiceBusSettings__ConnectionString=$serviceBusConnString 
-e ServiceSettings__MessageBroker="SERVICEBUS" 
-e IdentitySettings__AdminUserPassword=$adminPass play.identity:$version
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

## Creating the Azure Managed Identity and grantinng access to key vault secrets
```powershell 
az identity create --resource-group $appname --name $namespace
$IDENTITY_CLIENT_ID=az identity show -g $appname -n $namespace --query clientId -otsv
az keyvault set-policy -n $appname --secret-permissions get list --spn $IDENTITY_CLIENT_ID

``` 

<!-- # Creating the Azure Managed Identity and grantinng access to key vault secrets
```powershell
az identity create --resource-group $appname --name $namespace
$IDENTITY_CLIENT_ID=az identity show -g $appname -n $namespace --query clientId -otsv
# az keyvault set-policy -n $appname --secret-permissions get list --spn $IDENTITY_CLIENT_ID

# Get the principal ID of the managed identity
$IDENTITY_PRINCIPAL_ID = az identity show -g $appname -n $namespace --query principalId -otsv

# Get the Key Vault ID
$KEYVAULT_ID = az keyvault show --name $appname --query id -otsv

# Assign the "Key Vault Secrets User" role to the managed identity
az role assignment create --role "Key Vault Secrets User" --assignee $IDENTITY_PRINCIPAL_ID --scope $KEYVAULT_ID
``` -->

## Establish the federeated identity credential
```powershell
$AKS_OIDC_ISSUER=az aks show -g $appname -n $appname --query "oidcIssuerProfile.issuerUrl" -otsv

az identity federated-credential create --name $namespace --identity-name $namespace --resource-group $appname --issuer $AKS_OIDC_ISSUER --subject "system:serviceaccount:${namespace}:${namespace}-serviceaccount" 
```
