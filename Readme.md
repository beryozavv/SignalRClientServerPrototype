# Legend
- **Machine** – a separate physical client machine.
- **User** – a single user who can log in to their own machine or to a terminal server together with other users. In the latter case, multiple users share a single client process.
- When multiple users share a single client process, they can either use one SignalR connection for all users (recommended) or a separate connection per user (a wasteful approach).

# cURL Requests for Running Tests
## Broadcast Command to Multiple Machines with Multiple Users (each machine runs its own process)
```shell
curl --location 'http://localhost:5000/broadcast?machineIds=m1%2Cm2%2Cm3&userIds=%23iD200%23%2C1%2C2%2C3%2C2%2C3%2C1' \
--header 'Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7' \
--header 'Accept-Language: en-US,en;q=0.9,ru-RU;q=0.8,ru;q=0.7' \
--header 'Connection: keep-alive' \
--header 'Sec-Fetch-Dest: document' \
--header 'Sec-Fetch-Mode: navigate' \
--header 'Sec-Fetch-Site: none' \
--header 'Sec-Fetch-User: ?1' \
--header 'Upgrade-Insecure-Requests: 1' \
--header 'User-Agent: Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Mobile Safari/537.36' \
--header 'sec-ch-ua: "Chromium";v="134", "Not:A-Brand";v="24", "Google Chrome";v="134"' \
--header 'sec-ch-ua-mobile: ?1' \
--header 'sec-ch-ua-platform: "Android"'
```

## Send Command to a Single Machine with Multiple Users (the machine runs its own process)
```shell
curl --location 'http://localhost:5000/single?machineId=m1&userIds=Id1%2Cm1User%2CTest1' \
--header 'Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7' \
--header 'Accept-Language: en-US,en;q=0.9,ru-RU;q=0.8,ru;q=0.7' \
--header 'Connection: keep-alive' \
--header 'Sec-Fetch-Dest: document' \
--header 'Sec-Fetch-Mode: navigate' \
--header 'Sec-Fetch-Site: none' \
--header 'Sec-Fetch-User: ?1' \
--header 'Upgrade-Insecure-Requests: 1' \
--header 'User-Agent: Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Mobile Safari/537.36' \
--header 'sec-ch-ua: "Chromium";v="134", "Not:A-Brand";v="24", "Google Chrome";v="134"' \
--header 'sec-ch-ua-mobile: ?1' \
--header 'sec-ch-ua-platform: "Android"'
```

## Send Message to a Single Machine (process) with Multiple Users, where Each User Has Their Own SignalR Connection (many connections in one process)
> **This method results in increased resource consumption.**
```shell
curl --location 'http://localhost:5000/multipleConnections?message=HelloToAllUsers' \
--header 'Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7' \
--header 'Accept-Language: en-US,en;q=0.9,ru-RU;q=0.8,ru;q=0.7' \
--header 'Connection: keep-alive' \
--header 'Sec-Fetch-Dest: document' \
--header 'Sec-Fetch-Mode: navigate' \
--header 'Sec-Fetch-Site: none' \
--header 'Sec-Fetch-User: ?1' \
--header 'Upgrade-Insecure-Requests: 1' \
--header 'User-Agent: Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Mobile Safari/537.36' \
--header 'sec-ch-ua: "Chromium";v="134", "Not:A-Brand";v="24", "Google Chrome";v="134"' \
--header 'sec-ch-ua-mobile: ?1' \
--header 'sec-ch-ua-platform: "Android"'
```