# Contoso University

ASP.NET Core 3.1 MVC with Entity Framework 6 - demo app with Blazor Server added

Adapted from Microsoft sample code - see [CU_Resources](./_docs/CU_Resources.md)

Blazor demos added after the Contoso University app was working

This app was intentionally built with a layered architecture.

[Dev Notes](./_docs/CU__DevNotes.md)

[EF Resources](./_docs/CU_EFResources.md)<br/>
[Other Resources](./_docs/CU_Resources.md)<br/>
[Tools](./_docs/CU_Tools.md)<br/>

**Important Notes**<br/>
The projects in the solutions provided use Assembly references rather than Project references.

Be sure to copy `_ConfigSource\ContosoUniversity.DAL\App.config` 
to `ContosoUniversity.DAL`
and correct connection string before starting work on the app.

Copy `_ConfigSource\ContosoUniversity\appsettings.Development_user_xxxx.json`
to `ContosoUniversity` replacing "xxxx" in the file name with the 
username of the account from which you are running Visual Studio, and
update the paths in the file to match your solution path.  Also,
and correct connection string for your environment.

## Goals of this repo

Demonstrate how to take an ASP.NET Core 3.1 web app using Entity Framework 6 and a layered architecture
and replace server-side MVC with Blazor Server components.

## Projects

Project Name                 | Description
-------------                | ------------
ContosoUniversity.Models     | Persistent Data Object Models
ContosoUniversity.Shared     | Shared definitions and View Models
ContosoUniversity.Common     | Interface definitions
ContosoUniversity.DAL        | Data Access Layer
ContosoUniversity            | Contoso University Web Application
