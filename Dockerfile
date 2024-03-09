ARG RUNTIME_SDK="runtime"
ARG DOTNET_VERSION="6.0"
# Currently limited to .NET 6.0 by Discord.Net

FROM mcr.microsoft.com/dotnet/${RUNTIME_SDK}:${DOTNET_VERSION} AS base
SHELL ["/bin/bash", "-ex", "-c"]

# Devcontainer image
FROM base AS devcontainer
SHELL ["/bin/bash", "-ex", "-c"]

# Dev tools
RUN <<DOCKERFILE_EOF
apt-get update
apt-get install -y --no-install-recommends \
	fish \
	git \
	openssh-client \
	sudo \
	vim
DOCKERFILE_EOF

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

# Default target
FROM base
SHELL ["/bin/bash", "-ex", "-c"]
