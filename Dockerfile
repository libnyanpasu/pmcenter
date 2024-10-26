FROM mcr.microsoft.com/dotnet/aspnet:8.0

ARG LOCALE=en
WORKDIR /opt/pmcenter

COPY pmcenter/bin/Release/net8.0/ .

ADD locales/pmcenter_locale_$LOCALE.json ./pmcenter_locale.json

CMD ["dotnet", "./pmcenter.dll"]
