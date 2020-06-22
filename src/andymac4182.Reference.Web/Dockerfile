FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
ARG version
ENV version=$version
WORKDIR /app
COPY . ./
RUN ./build.ps1

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/artifacts/publish/andymac4182.Reference.Web .
ENTRYPOINT ["dotnet", "andymac4182.Reference.Web.dll"]