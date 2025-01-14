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

```


# git
```sh
cd api
dotnet new gitignore

git init
git remote add .
```