FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal

EXPOSE 5000

WORKDIR /MusicKG.Workflow/

CMD ["dotnet", "MusicKG.Workflow.dll"]

COPY bin/Release/net5.0/publish/ /MusicKG.Workflow/