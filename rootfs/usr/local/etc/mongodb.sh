#!/usr/bin/env bash

set -ex

DEBIAN_FRONTEND=noninteractive

function source_os_release() {
	# Specify the prefix for the environment variables
	local prefix="OS_RELEASE_"

	# Read the key-value pairs from /etc/os-release
	while IFS='=' read -r key value; do
		# Remove surrounding quotes from the value
		value="${value%\"}"
		value="${value#\"}"
		# Check if the key is non-empty
		if [[ -n $key ]]; then
			# Set the environment variable with the specified prefix
			export "${prefix}${key}"="$value"
		fi
	done < /etc/os-release
}
source_os_release

function install() {
	local mongo_version=${1:-latest}
	if [ "$mongo_version" = "latest" ]; then
		local mongo_minor_version=7.0
	else
		local mongo_minor_version=$(echo "$mongo_version" | cut -d'.' -f1-2)
	fi

	# Instructions:
	# https://www.mongodb.com/docs/manual/tutorial/install-mongodb-on-debian/#std-label-install-mdb-community-debian

	# 1
	apt-get update
	apt-get install --yes gnupg curl
	curl -fsSL https://www.mongodb.org/static/pgp/server-${mongo_minor_version}.asc \
		| gpg -o /usr/share/keyrings/mongodb-server-${mongo_minor_version}.gpg --dearmor

	# 2
	case $OS_RELEASE_VERSION_CODENAME in
		buster)
			echo "deb [ signed-by=/usr/share/keyrings/mongodb-server-${mongo_minor_version}.gpg ] http://repo.mongodb.org/apt/debian buster/mongodb-org/${mongo_minor_version} main" \
				| tee /etc/apt/sources.list.d/mongodb-org-${mongo_minor_version}.list
			;;
		bullseye)
			echo "deb [ signed-by=/usr/share/keyrings/mongodb-server-${mongo_minor_version}.gpg ] http://repo.mongodb.org/apt/debian bullseye/mongodb-org/${mongo_minor_version} main" \
				| tee /etc/apt/sources.list.d/mongodb-org-${mongo_minor_version}.list
			;;
		*)
			echo "Unsupported version: $OS_RELEASE_VERSION_CODENAME"
			exit 1
			;;
	esac

	# 3
	apt-get update

	# 4
	if [ "$mongo_version" = "latest" ]; then
		apt-get install --yes mongodb-org
	else
		sudo apt-get install --yes \
			mongodb-org=${mongo_version} \
			mongodb-org-database=${mongo_version} \
			mongodb-org-server=${mongo_version} \
			mongodb-mongosh=${mongo_version} \
			mongodb-org-mongos=${mongo_version} \
			mongodb-org-tools=${mongo_version}
	fi
}

function start() {
	mongod --config /etc/mongod.conf &
}

# Parse the command line arguments
while [[ "$#" -gt 0 ]]; do
	case $1 in
		i|install)
			shift
			install $@
			;;
		s|start)
			shift
			start $@
			;;
		*) echo "Unknown command passed: $1"; exit 1 ;;
	esac
	shift
done
