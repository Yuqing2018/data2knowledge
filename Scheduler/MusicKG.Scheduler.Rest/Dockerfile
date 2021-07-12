FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal

EXPOSE 80

ARG BUILD_FOLDER

WORKDIR /MusicKG.Scheduler.Rest/

CMD ["dotnet", "MusicKG.Scheduler.Rest.dll"]

COPY ${BUILD_FOLDER} .