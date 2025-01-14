# 初始化api專案
```sh
# 建立api 專案
dotnet new webapi --framework net8.0 -n api

# 加入Contrallers及Models
cd api
vim Program.cs
## // 啟用 Controller 支援
## builder.Services.AddControllers();
## // 啟用 Controller 路由
## app.MapControllers();

# 測試api建置
## Contrallers
mkdir Contrallers
vim Contrallers/WeatherController.cs
## Models
mkdir Models
mkdir Models/Entities
mkdir Models/ViewModels
vim Models/ViewModels/WeatherViewModel.cs


## dotnet-ef cli啟用
dotnet tool update --global dotnet-ef --version 8.0.0

## 專案中使用的EF Core package
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0

## 建立entities
vim api\Models\Entities\Currency.cs
## 建立dbcontext
vim api\MssqlDbContext.cs
## Program.cs加入MssqlDbContext
vim api\Program.cs
## appsettings加入db連線字串
vim api\appsettings.json

## 根據entities改動Migration DB
dotnet ef migrations add InitialCreate
dotnet ef database update

## Program.cs

```


# git
```sh
cd api
dotnet new gitignore

git init
git remote add .
```


# uni test
```sh
# 建立測試專案
mkdir tests
cd tests
dotnet new nunit --framework net8.0 -n api.UnitTests
cd api.UnitTests
dotnet add reference ../../api/api.csproj
rm UnitTest1.cs

# 添加必要的 NuGet 包：
dotnet add package Moq
dotnet add package Microsoft.EntityFrameworkCore.InMemory

# 運行測試
cd tests/api.UnitTests
dotnet test
```