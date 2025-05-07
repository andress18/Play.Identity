# Play.Identity.Contracts
Identity libraries used by Play Economy services

## Create and publish package
```powershell
$version="1.0.1"
$owner="dotnetMicroservicesCourseASGX"
$gh_pat="[PAT HERE]"

dotnet pack src\Play.Identity.Contracts\ --configuration Release -p:PackageVersion=$version -p:RepositoryUrl=https://github.com/$owner/Play.Identity -o ..\packages
dotnet nuget push ..\packages\Play.Identity.Contracts.$version.nupkg --api-key $gh_pat --source "github"
```