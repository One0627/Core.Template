FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Core.Template.Api/Core.Template.Api.csproj", "Core.Template.Api/"]
COPY ["Core.Template.Services/Core.Template.Services.csproj", "Core.Template.Services/"]
COPY ["Core.Template.IServices/Core.Template.IServices.csproj", "Core.Template.IServices/"]
COPY ["Core.Template.Models/Core.Template.Models.csproj", "Core.Template.Models/"]
COPY ["Core.Template.Utils/Core.Template.Utils.csproj", "Core.Template.Utils/"]
RUN dotnet restore "Core.Template.Api/Core.Template.Api.csproj"
COPY . .
WORKDIR "/src/Core.Template.Api"
RUN dotnet build "Core.Template.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Core.Template.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Core.Api.Template.dll"]