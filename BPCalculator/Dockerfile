FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ["BPCalculator/BPCalculator.csproj", "BPCalculator/"]
RUN dotnet restore "BPCalculator/BPCalculator.csproj"
COPY . .
WORKDIR "/src/BPCalculator"
RUN dotnet build "BPCalculator.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "BPCalculator.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BPCalculator.dll"]