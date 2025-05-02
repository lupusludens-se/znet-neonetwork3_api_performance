FROM mcr.microsoft.com/dotnet/sdk:6.0

WORKDIR /app

RUN apt-get update && apt-get upgrade -y && \
    useradd -c 'Application User' -U -d /app -s /bin/bash appuser && \
    chown -R appuser:appuser /app
COPY --chown=appuser:appuser . .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

USER appuser
WORKDIR /app
CMD ["dotnet", "SE.Neo.WebAPI.dll"]