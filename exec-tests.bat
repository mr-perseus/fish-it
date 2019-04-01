dotnet test Fishit.BusinessLayer.Testing /p:CollectCoverage=true /p:CoverletOutputFormat=opencover -l:trx;LogFileName=TestOutput.xml
dotnet test Fishit.Common.Testing /p:CollectCoverage=true /p:CoverletOutputFormat=opencover -l:trx;LogFileName=TestOutput.xml
dotnet test Fishit.Presentation.Testing /p:CollectCoverage=true /p:CoverletOutputFormat=opencover -l:trx;LogFileName=TestOutput.xml
