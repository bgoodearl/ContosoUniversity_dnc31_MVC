# Contoso University - Migration Notes

=======================================<br/>
IMPORTANT - Set project "ContosoUniversity.DAL" as startup project before doing migration operations<br/>
		if you want to use the connection string from that project.<br/>
=======================================<br/>

Check out [EF Resources](../_docs/CU_EFResources.md)
 -- links to Entity Framework resources<br/>

[Command Line tool - ef6.exe instead of migrate.exe](https://github.com/NuGet/NuGetGallery/pull/7711)

## Initial Setup

Package Manager Console:
```powershell
Enable-Migrations -verbose
```

## Migrations

### Initial Migration

Package Manager Console:
```powershell
	Add-Migration Schema1 -verbose
```

Initial results:
```powershell
C:\Program Files\dotnet\dotnet.exe exec --depsfile D:\_dev\GitHub\bgoodearl\MVC\ContosoUniversity_dnc31_MVC\ContosoUniversity.DAL\bin\Debug\netcoreapp3.1\ContosoUniversity.DAL.deps.json --additionalprobingpath C:\Users\bobg\.nuget\packages --additionalprobingpath "C:\Program Files\dotnet\sdk\NuGetFallbackFolder" --runtimeconfig D:\_dev\GitHub\bgoodearl\MVC\ContosoUniversity_dnc31_MVC\ContosoUniversity.DAL\bin\Debug\netcoreapp3.1\ContosoUniversity.DAL.runtimeconfig.json C:\Users\bobg\.nuget\packages\entityframework\6.4.4\tools\netcoreapp3.0\any\ef6.dll migrations add Schema1 --json --verbose --no-color --prefix-output --assembly D:\_dev\GitHub\bgoodearl\MVC\ContosoUniversity_dnc31_MVC\ContosoUniversity.DAL\bin\Debug\netcoreapp3.1\ContosoUniversity.DAL.dll --project-dir D:\_dev\GitHub\bgoodearl\MVC\ContosoUniversity_dnc31_MVC\ContosoUniversity.DAL\ --language C# --root-namespace ContosoUniversity.DAL --config D:\_dev\GitHub\bgoodearl\MVC\ContosoUniversity_dnc31_MVC\ContosoUniversity.DAL\App.config
Scaffolding migration 'Schema1'.
The Designer Code for this migration file includes a snapshot of your current Code First model. This snapshot is used to calculate the changes to your model when you scaffold the next migration. If you make additional changes to your model that you want to include in this migration, then you can re-scaffold it by running 'Add-Migration Schema1' again.
Target database is: 'SchoolContext' (DataSource: (localdb)\mssqllocaldb, Provider: System.Data.SqlClient, Origin: Convention).
```
Note that connection does not come from App.config


#### Initial Migration workaround

**NOTE**: The following runs in a PowerShell command prompt, NOT in the Package Manager Console

**IMPORTANT**: update setting of **$devsolpath** and **$connstring** for your environment!<br/>
Command to run in powershell:
```powershell
$devsolpath = "D:\_dev\GitHub\bgoodearl\MVC\ContosoUniversity_dnc31_MVC"; $srcschema = "0"; $dstschema = "Schema1";
$connstring = "data source=.\SQLEXPRESS;initial catalog=ContosoUniv_c31_dev;Integrated Security=SSPI;MultipleActiveResultSets=True;"
$dalproj = "ContosoUniversity.DAL"

dotnet exec `
--depsfile $devsolpath\ContosoUniversity.DAL\bin\Debug\netcoreapp3.1\$dalproj.deps.json `
--additionalprobingpath C:\Users\$env:username\.nuget\packages `
--additionalprobingpath "C:\Program Files\dotnet\sdk\NuGetFallbackFolder" `
--runtimeconfig $devsolpath\$dalproj\bin\Debug\netcoreapp3.1\$dalproj.runtimeconfig.json `
C:\Users\$env:username\.nuget\packages\entityframework\6.4.4\tools\netcoreapp3.0\any\ef6.dll `
migrations add $dstschema `
--json --verbose --no-color --prefix-output --assembly $devsolpath\$dalproj\bin\Debug\netcoreapp3.1\$dalproj.dll `
--project-dir $devsolpath\$dalproj\ --language C# --root-namespace $dalproj `
--connection-string $connstring `
--connection-provider "System.Data.SqlClient"
```

Results:
```txt
info:    Scaffolding migration 'Schema1'.
warn:    The Designer Code for this migration file includes a snapshot of your current Code First model. This snapshot is used to calculate the changes to your model when you scaffold the next migration. If you make additional changes to your model that you want to include in this migration, then you can re-scaffold it by running 'Add-Migration Schema1' again.
verbose: Target database is: 'ContosoUniv_c31_dev' (DataSource: .\SQLEXPRESS, Provider: System.Data.SqlClient, Origin: Explicit).
data:    {
data:      "migration": "D:\\_dev\\GitHub\\bgoodearl\\MVC\\ContosoUniversity_dnc31_MVC\\ContosoUniversity.DAL\\Migrations\\202202242101280_Schema1.cs",
data:      "migrationResources": "D:\\_dev\\GitHub\\bgoodearl\\MVC\\ContosoUniversity_dnc31_MVC\\ContosoUniversity.DAL\\Migrations\\202202242101280_Schema1.resx",
data:      "migrationDesigner": "D:\\_dev\\GitHub\\bgoodearl\\MVC\\ContosoUniversity_dnc31_MVC\\ContosoUniversity.DAL\\Migrations\\202202242101280_Schema1.Designer.cs"
data:    }
```

##### SQL Script workaround

**IMPORTANT**: update setting of **$devsolpath** and **$connstring** for your environment!<br/>
Command to run in powershell:
```powershell
$devsolpath = "D:\_dev\GitHub\bgoodearl\MVC\ContosoUniversity_MVC"; $srcschema = "0"; $dstschema = "Schema1";
$connstring = "data source=.\SQLEXPRESS;initial catalog=ContosoUniv_Dev;Integrated Security=SSPI;MultipleActiveResultSets=True;"
$dalproj = "ContosoUniversity.DAL"

dotnet exec `
--depsfile $devsolpath\$dalproj\bin\Debug\netcoreapp3.1\$dalproj.deps.json `
--additionalprobingpath C:\Users\$env:username\.nuget\packages `
--additionalprobingpath "C:\Program Files\dotnet\sdk\NuGetFallbackFolder" `
--runtimeconfig $devsolpath\$dalproj\bin\Debug\netcoreapp3.1\$dalproj.runtimeconfig.json `
C:\Users\$env:username\.nuget\packages\entityframework\6.4.4\tools\netcoreapp3.0\any\ef6.dll `
database update `
--source $srcschema `
--target $dstschema `
--script --verbose --no-color --prefix-output `
--assembly $devsolpath\$dalproj\bin\Debug\netcoreapp3.1\$dalproj.dll `
--project-dir $devsolpath\$dalproj\ `
--language C# `
--root-namespace $dalproj `
--connection-string $connstring `
--connection-provider "System.Data.SqlClient" `
1> $devsolpath\SqlScripts\Schema\$dstschema.sql
```
output in Schema1.sql:
```sql
verbose: Target database is: 'ContosoUniv_c31_dev' (DataSource: .\SQLEXPRESS, Provider: System.Data.SqlClient, Origin: Explicit).
info:    Applying explicit migrations: [202202242101280_Schema1].
info:    Applying explicit migration: 202202242101280_Schema1.
data:    DECLARE @CurrentMigration [nvarchar](max)
data:    
```
(followed by the rest of the "data:" lines)

To get the actual SQL script, remove the first 3 lines, and replace "data:    " with "".

