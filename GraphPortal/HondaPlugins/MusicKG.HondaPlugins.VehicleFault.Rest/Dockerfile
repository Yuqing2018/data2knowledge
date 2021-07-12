FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal

EXPOSE 80

WORKDIR /app

CMD ["dotnet", "MusicKG.HondaPlugins.VehicleFault.Rest.dll"]

COPY bin/Release/net5.0/publish/ /app/