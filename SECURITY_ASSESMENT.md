# Security Assessment

For a more detailed description, please refer to this page: <https://github.com/itu-devops/lecture_notes/blob/master/sessions/session_09/README_TASKS.md>.

## A. Risk Identification

| | Description |
| --- | --- |
| Identify assets | The assets for this web application include a C# backend, a MongoDB database, and a React frontend. Our application also exposes Prometheus for collecting metrics, and Grafana for visualizing the metrics and logging via Loki.
| Identify threat sources | SnykCode has detected that the application is vulnerable to Cross-Site Scripting (XSS) and Log Forging. We currently do not sanitize the log input. Additionally, we are vulnerable to a Cross Site Request Forgery (CSRF) attack on various APIs, which allows attackers to use a user's authorized credentials to impersonate that user, violating data integrity. <br><br>Our services run on the HTTP protocol, which poses a threat since transmitted data is not encrypted between the client and server. This allows an attacker to use Wireshark to see the unencrypted password. Furthermore, we only have basic authentication and do not use digital certificates to verify user identities and establish a secure connection. This makes the application vulnerable to man-in-the-middle attacks and other impersonation attacks. Lastly, we cannot guarantee that data has not been tampered with during transmission, which would be ensured via Message Authentication Codes (MACs).
| Construct risk scenarios | **S1:** Attacker performs Cross-Site Scripting (XSS) on web application to steal sensitive user data. <br>**S2:** Attacker performs Log Forging on web application to modify logs and cover tracks.<br>**S3:** Attacker performs Cross Site Request Forgery (CSRF) on various APIs to impersonate a user and violate data integrity.<br>**S4:** Attacker intercepts unencrypted password transmitted between client and server using Wireshark.<br>**S5:** Attacker performs man-in-the-middle attack on unsecured connection to impersonate a user and compromise data.<br>**S6:** Attacker tampers with data during transmission due to lack of Message Authentication Codes (MACs). |

## B. Risk Analysis

### Likelihood

| Risk scenario | Likelihood |
| --- | --- |
| S1 | Almost certain
| S2 | Possible
| S3 | Almost certain
| S4 | Likely
| S5 | Possible
| S6 | Possible

### Impact

| Risk scenario | Impact |
| --- | --- |
| S1 | Information -> Significant
| S2 | Information -> Extensive
| S3 | Capability -> Significant
| S4 | Information -> Significant
| S5 | Information and Capability -> Significant
| S6 | Capability -> Extensive

### Risk Matrix

- x-axis: Properbility of risk (increases to the right as the risk becomes greater)
- y-axis: Impact of risk (increases upwards)

|  |  |  |
| --- | --- | --- |
| **Medium**<br>: | **High**<br>: | **High**<br>:S1, S3
| **Low**<br>: | **Medium**<br>:S2, S6 | **High**<br>:S4, S5
| **Low**<br>: | **Low**<br>: | **Medium**<br>:

### How we will resolve these vulnerabilities

| Risk scenario | To-do |
| --- | --- |
| S1 | Sanitize input.
| S2 | Sanitize input.
| S3 | Implement authentication.
| S4 | Enable HTTPS.
| S5 | Enable HTTPS.
| S6 | Enable HTTPS.

## C. Pen-Test Your System

We will fix the vulnerability in scenario 3, which involves an attacker performing Cross Site Request Forgery (CSRF) on various APIs to impersonate a user and violate data integrity. This vulnerability has a high probability and impact of risk.