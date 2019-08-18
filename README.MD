# ProAgil Eventos

## Para utilizar o SQLServer
### Verifique se os serviços do SQL Server estão Habilitados:
    - SQLServer Agent
    - SQL Server Browser

## Gerar o banco com comando do Entity
- para Gerar o banco no SQLServer será necessário utilizar os CLI no terminal do VSCode:
    Abra o diretório repository: 
    - "cd .\ProAgil.Repository\" (Note que será necessário alterar as config no appsettings.json e no appsetings.development.json).
    - No diretório utilize o seguinte comando: "dotnet ef --startup-project ..\ProAgil.API\ database update"
    caso haja algum erro na geração do banco como "error 40" será necessário ativar os pipes com gerenciador de banco do SQL Server: 
    - .[Link].(https://stackoverflow.com/questions/9945409/how-do-i-fix-the-error-named-pipes-provider-error-40-could-not-open-a-connec)


