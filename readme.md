# Contoso University

ASP.NET Core MVC with Entity Framework 6 - demo app with Blazor Server added

Adapted from Microsoft sample code - see [CU_Resources](./_docs/CU_Resources.md)

Blazor demos added after the Contoso University app was working

[Dev Notes](./_docs/CU_DevNotes.md)

[Blazor Resources](./_docs/CU_BlazorResources.md)

**Important Notes**<br/>
The projects in the solutions provided use Assembly references rather than Project references.

## Goals of this repo

Demonstrate how to take an ASP.NET Core 3.1 web app using Entity Framework 6 and a layered architecture
and replace server-side MVC with Blazor Server components.

## Projects

Project Name                 | Description
-------------                | ------------
ContosoUniversity.Models     | Persistent Data Object Models
ContosoUniversity.Common     | Interface definitions
ContosoUniversity.DAL        | Data Access Layer
