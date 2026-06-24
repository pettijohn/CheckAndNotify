#!/usr/bin/env bash
set -euo pipefail

if [[ $# -ne 1 || -z ${1:-} ]]; then
    echo "Version number required" >&2
    exit 1
fi

VERSION=$1

if [[ $VERSION == v* ]]; then
    echo "Version must only be a number" >&2
    exit 1
fi

if ! command -v xmlstarlet >/dev/null 2>&1; then
    echo "xmlstarlet is required. Install it with: sudo apt-get install xmlstarlet" >&2
    exit 1
fi

if ! command -v dotnet >/dev/null 2>&1; then
    echo "dotnet is required" >&2
    exit 1
fi

if ! command -v docker >/dev/null 2>&1; then
    echo "docker is required" >&2
    exit 1
fi

if ! docker info >/dev/null 2>&1; then
    echo "Docker is not available. Rebuild the devcontainer so it can use the host Docker socket." >&2
    exit 1
fi

SCRIPT_DIR=$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)
PROJECT_PATH="$SCRIPT_DIR/src/CheckAndNotify.csproj"
IMAGE_NAME="check-and-notify"
REGISTRY_IMAGE="ghcr.io/pettijohn/check-and-notify"

xmlstarlet ed -P -O -L \
    -u "/Project/PropertyGroup/Version" -v "$VERSION" \
    -u "/Project/PropertyGroup/ContainerImageTags" -v "$VERSION;latest" \
    "$PROJECT_PATH"

# https://learn.microsoft.com/en-us/dotnet/core/docker/publish-as-container
dotnet publish "$PROJECT_PATH" --os linux --arch amd64 /t:PublishContainer -c Release

docker image tag "$IMAGE_NAME:$VERSION" "$REGISTRY_IMAGE:$VERSION"
docker image tag "$IMAGE_NAME:$VERSION" "$REGISTRY_IMAGE:latest"
docker image push "$REGISTRY_IMAGE:latest"
docker image push "$REGISTRY_IMAGE:$VERSION"

# Version it
#git tag $version
#git push --tags
#Write-Host "Run git push --tags"
