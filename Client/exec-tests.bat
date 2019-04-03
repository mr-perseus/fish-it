dotnet test Client\Fishit.BusinessLayer.Testing --collect:"Code Coverage" -l:trx;LogFileName=TestOutput.xml
dotnet test Client\Fishit.Common.Testing --collect:"Code Coverage" -l:trx;LogFileName=TestOutput.xml
dotnet test Client\Fishit.Presentation.Testing --collect:"Code Coverage" -l:trx;LogFileName=TestOutput.xml