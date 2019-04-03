dotnet test Fishit.BusinessLayer.Testing --collect:"Code Coverage" -l:trx;LogFileName=TestOutput.xml
dotnet test Fishit.Common.Testing --collect:"Code Coverage" -l:trx;LogFileName=TestOutput.xml
dotnet test Fishit.Presentation.Testing --collect:"Code Coverage" -l:trx;LogFileName=TestOutput.xml