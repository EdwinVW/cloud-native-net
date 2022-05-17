Push-Location
Set-Location ..\ContractManagement\Infrastructure\
dotnet ef database drop -f
dotnet ef database update
Pop-Location