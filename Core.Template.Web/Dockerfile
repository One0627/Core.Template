FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Core.Template.Web/Core.Template.Web.csproj", "Core.Template.Web/"]
RUN dotnet restore "Core.Template.Web/Core.Template.Web.csproj"
COPY . .
WORKDIR "/src/Core.Template.Web"
RUN dotnet build "Core.Template.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Core.Template.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Core.Template.Web.dll"]