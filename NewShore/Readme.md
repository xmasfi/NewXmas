# NewShore <!-- omit in toc -->

- [Overview](#overview)
- [EF Migrations](#ef-migrations)


## Overview

## EF Migrations

The following table lists important migration commands in EF Core.

| Action               | Package Manager Console tools     | .NET Core CLI tools                         |
| -------------------- | --------------------------------- | ------------------------------------------- |
| Create a migration   | Add-Migration InitialCreate       | dotnet ef migrations add InitialCreate      |
| Update the database  | Update-Database                   | dotnet ef database update                   |
| Remove a migration   | Remove-Migration                  | dotnet ef migrations remove                 |
| Revert a migration   | Update-Database LastGoodMigration | dotnet ef database update LastGoodMigration |
| Generate SQL scripts | Script-Migration                  | dotnet ef migrations script                 |

More info: https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/


