FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src

RUN apk update && apk upgrade && apk add efs-utils git binutils tzdata icu-data-full icu-libs && ln -s /usr/share/zoneinfo/America/Sao_Paulo /etc/localtime

COPY ["Ourinvest.DD.MN.CD/Ourinvest.DD.MN.CD.csproj", "Ourinvest.DD.MN.CD/"]
COPY ["Ourinvest.DD.MN.CD.Core/Ourinvest.DD.MN.CD.Core.csproj", "Ourinvest.DD.MN.CD.Core/"]
COPY ["Ourinvest.DD.MN.CD.Infra/Ourinvest.DD.MN.CD.Infra.csproj", "Ourinvest.DD.MN.CD.Infra/"]
COPY ["Ourinvest.DD.MN.CD.SharedKernel/Ourinvest.DD.MN.CD.SharedKernel.csproj", "Ourinvest.DD.MN.CD.SharedKernel/"]
COPY ["Ourinvest.DD.MN.CD/nuget.config",""]

RUN dotnet restore "Ourinvest.DD.MN.CD/Ourinvest.DD.MN.CD.csproj"
COPY . .
WORKDIR "/src/Ourinvest.DD.MN.CD"
RUN dotnet build "Ourinvest.DD.MN.CD.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ourinvest.DD.MN.CD.csproj" -c Release -o /app/publish /p:UseAppHost=false
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN apk update && apk upgrade && apk add icu-data-full icu-libs

ARG TRACER_VERSION=2.11.0
RUN mkdir -p /var/log/datadog && \
    mkdir -p /opt/datadog && \
    wget --no-check-certificate https://github.com/DataDog/dd-trace-dotnet/releases/download/v${TRACER_VERSION}/datadog-dotnet-apm-${TRACER_VERSION}-musl.tar.gz && \
    tar -C /opt/datadog -xzf datadog-dotnet-apm-${TRACER_VERSION}-musl.tar.gz && sh /opt/datadog/createLogPath.sh

ENV CORECLR_ENABLE_PROFILING=1
ENV CORECLR_PROFILER={846F5F1C-F9AE-4B07-969E-05C26BC060D8}
ENV CORECLR_PROFILER_PATH=/opt/datadog/Datadog.Trace.ClrProfiler.Native.so
ENV DD_DOTNET_TRACER_HOME=/opt/datadog
ENV DD_SERVICE=Ourinvest.DD.MN.CD-webapi
ENV DD_SERVICE_NAME=Ourinvest.DD.MN.CD-webapi
ENV DD_VERSION=1.0.0
ENV DD_LOGS_INJECTION=true

RUN apk update && apk upgrade && apk add efs-utils git binutils tzdata
RUN apk update && apk add --no-cache tzdata && cp /usr/share/zoneinfo/America/Sao_Paulo /etc/localtime && echo "America/Sao_Paulo" > /etc/timezone

ENTRYPOINT ["dotnet", "Ourinvest.DD.MN.CD.dll"]