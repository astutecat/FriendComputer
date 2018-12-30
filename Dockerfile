FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY FriendComputer/FriendComputer.csproj FriendComputer/
RUN dotnet restore FriendComputer/FriendComputer.csproj
COPY . .
WORKDIR /src/FriendComputer
RUN dotnet build FriendComputer.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish FriendComputer.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
RUN mkdir /app/db
VOLUME /app/db
ENTRYPOINT ["dotnet", "FriendComputer.dll"]
