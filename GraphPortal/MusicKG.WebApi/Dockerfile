FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal

EXPOSE 5000

WORKDIR /MusicKG.WebApi/

CMD ["dotnet", "MusicKG.WebApi.dll"]

COPY bin/Release/net5.0/publish/ /MusicKG.WebApi/