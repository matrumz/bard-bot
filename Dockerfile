ARG DOTNET_VERSION="8.0"

#### SDK #######################################################################
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS sdk

#### Runtime ###################################################################
FROM mcr.microsoft.com/dotnet/runtime:${DOTNET_VERSION} AS runtime

#### Devcontainer ##############################################################
FROM sdk AS devcontainer
SHELL ["/bin/bash", "-ex", "-c"]

# Dev tools
RUN <<DOCKERFILE_EOF
apt-get update
apt-get install -y --no-install-recommends \
	fish \
	git \
	openssh-client \
	sudo \
	uuid-runtime \
	vim
DOCKERFILE_EOF

# RUN --mount=type=bind,source=./dockerfile.d/mongodb.sh,target=/tmp/mongodb.sh \
# 	/tmp/mongodb.sh install latest

# Create developer user
ARG DEVCONTAINER_USER_NAME=dev
ARG DEVCONTAINER_USER_UID=1000
ARG DEVCONTAINER_USER_GID=$DEVCONTAINER_USER_UID
RUN <<DOCKERFILE_EOF
groupadd --gid $DEVCONTAINER_USER_GID $DEVCONTAINER_USER_NAME
groupadd --force docker
useradd --uid $DEVCONTAINER_USER_UID --gid $DEVCONTAINER_USER_GID --groups docker --shell /usr/bin/fish --create-home $DEVCONTAINER_USER_NAME
echo "$DEVCONTAINER_USER_NAME ALL=(ALL) NOPASSWD: ALL" > /etc/sudoers.d/$DEVCONTAINER_USER_NAME
chmod 0440 /etc/sudoers.d/$DEVCONTAINER_USER_NAME
DOCKERFILE_EOF
USER $DEVCONTAINER_USER_NAME

# #### Build #####################################################################
# FROM sdk AS build
# ARG ARTIFACTORY_PASSWORD
# ARG ARTIFACTORY_USERNAME
# SHELL ["/bin/bash", "-ex", "-c"]
# WORKDIR /app
# # Copy csproj and restore as distinct layers
# COPY *.sln .
# COPY nuget.config .
# COPY --parent src/**/*.csproj ./src/
# RUN dotnet restore
# # Copy everything else and build
# COPY . .
# RUN dotnet build

# #### Testrunner ################################################################
# FROM build AS testrunner
# WORKDIR /app/src/... #TODO
# ENTRYPOINT ["dotnet", "test", "--logger:trx"]
# ARG REPO_URL=""
# ARG VCS_REF=""
# LABEL org.opencontainers.image.revision=$VCS_REF
# LABEL org.opencontainers.image.source=$REPO_URL

# #### Test ######################################################################
# # Used to stop the build process if the tests fail
# FROM build AS test
# WORKDIR /app/src/... #TODO
# RUN dotnet test

# #### Publish ###################################################################
# FROM test AS publish
# WORKDIR /app
# RUN dotnet publish -c Release -o /out

# #### Final/Default #############################################################
# FROM runtime
# WORKDIR /app
# COPY --from=publish /out ./
# ENTRYPOINT ["dotnet", "TODO.dll"]
# ARG REPO_URL=""
# ARG VCS_REF=""
# LABEL org.opencontainers.image.revision=$VCS_REF
# LABEL org.opencontainers.image.source=$REPO_URL
