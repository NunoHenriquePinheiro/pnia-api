# pnia v1.1 - Resolution, by Nuno Pinheiro

This exercise was solved with:
- C# .NET Core v3.0
- Docker v19.03.2

# Phone number information aggregator API

**Endpoint:** `/aggregate` (POST)

**Requests:** The requests' body must be an array of phone numbers, as described by the exercise.

**Responses:**
1. Successful response, with content: HTTP status **200**, with the formatted data in the responses' body.
2. Successful response, without content: HTTP status **204**, without body. If no valid input is given in the request, there will be any matching content, so, the responses' body is empty.
3. Unsuccessful response, with error: HTTP status **500**, showing the occurring error within the application.

- A valid input was defined as any phone number that:
  - Is interpreted as it;
  - Matches a prefix in the list;
  - Matches a sector returned by the business sector API.

**Settings:**

In the project's file *appsettings.json*, some custom settings are available, in order to change behaviors within the application:

| Settings Section | Setting  | Meaning | Allowed values    | Default value |
|------------------|----------|---------|-------------------|---------------|
| CacheOptions | UseCache | Use cache or not | true/false (bool) | true |
|              | ExpireTimeMinutes | Cache expiration time (minutes) | double | 5 minutes |
| ServiceEndpoints | PhoneBusinessSector | Endpoint to search a phone number's business sector | string | --- |

- When the use of cache is set, the content from `prefixes.txt` file is cached.

# Deployment

- Please, use Docker, running: `docker-compose up pniaapi`
- The endpoint will be reachable through: `http://localhost:8080/aggregate`

