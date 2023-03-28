# Logging Approach

## Logging Requirements

The following topics must be logged in our application:

- Business-related topics (event logs)
- Input validation failures
- Output validation failures (e.g. database record set mismatch, invalid data encoding)
- Authentication successes and failures

Specifically, we need to log the following information in the TwitterController and SimController classes:

- Client error messages (response codes from 400-499)
- User registrations
- User logins and logouts with timestamps
- User posts
- User follows and unfollows
