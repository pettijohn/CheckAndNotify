# https://learn.microsoft.com/en-us/dotnet/core/docker/publish-as-container

dotnet publish --os linux --arch arm64 /t:PublishContainer -c Release
git tag v0.1.0
git push --tags
docker image tag check-and-notify:0.1.0 ghcr.io/pettijohn/check-and-notify:latest-arm64
docker image push ghcr.io/pettijohn/check-and-notify:latest-arm64

# dotnet publish --os linux --arch x64 /t:PublishContainer -c Release
# docker image tag check-and-notify:1.0.0 ghcr.io/pettijohn/check-and-notify:latest-amd64
# docker image push ghcr.io/pettijohn/check-and-notify:latest-amd64

# # Merge both into a multi-platform image
# docker buildx imagetools create --tag ghcr.io/pettijohn/check-and-notify:latest \
#     ghcr.io/pettijohn/check-and-notify:latest-arm64 \
#     ghcr.io/pettijohn/check-and-notify:latest-amd64
