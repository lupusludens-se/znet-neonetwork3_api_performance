FROM mcr.microsoft.com/dotnet/sdk:6.0

WORKDIR /app

RUN apt-get update && apt-get upgrade -y && \
    useradd -c 'Application User' -U -d /app -s /bin/bash appuser && \
    chown -R appuser:appuser /app
COPY --chown=appuser:appuser . .

USER appuser

ENV PATH=/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin:/app/.dotnet/tools

RUN dotnet tool install --global dotnet-ef --version 6.* && \
    cd SE.Neo.WebAPI && \
    dotnet tool restore

WORKDIR /app/SE.Neo.WebAPI

CMD ["dotnet", "ef", "database", "update"]