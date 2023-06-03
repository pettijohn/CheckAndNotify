docker-compose.yml:
```
version: '3.2'

services:
  check-and-notify:
    image: ghcr.io/pettijohn/check-and-notify:latest # Supports arm64 and amd64
    container_name: check-and-notify
    restart: always
```

.env
```
TELEGRAM_URL="https://api.telegram.org/bot<id>:<secret>/sendMessage?chat_id=<id>"
COORDINATES=39.833333,-98.583333
```

## Docs
* https://www.weather.gov/documentation/services-web-api
* https://cdn.britannica.com/96/105496-050-2F0D405B/Mariner-compass-card.jpg 