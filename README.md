[![GitHub license](https://img.shields.io/github/license/Naereen/StrapDown.js.svg)](https://github.com/simonskodt/itu-minitwit/blob/main/LICENSE)
![GitHub Release Date](https://img.shields.io/github/release-date/simonskodt/itu-minitwit)
![GitHub last commit](https://img.shields.io/github/last-commit/simonskodt/itu-minitwit)
[![.NET](https://img.shields.io/badge/--512BD4?logo=.net&logoColor=ffffff)](https://dotnet.microsoft.com/)
[![TypeScript](https://img.shields.io/badge/--3178C6?logo=typescript&logoColor=ffffff)](https://www.typescriptlang.org/)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=simonskodt_itu-minitwit&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=simonskodt_itu-minitwit)

# DevOps&mdash;Twitter Project

| :love_letter: Group k&mdash;Radiator | Link |
| ------------- | ------------- |
| Website | [http://164.92.167.188:3000](http://164.92.167.188:3000)  |
| Swagger  | [http://164.92.167.188/swagger/index.html](http://164.92.167.188/swagger/index.html)  |
| Monitoring (requires login) | [http://164.92.167.188:9091](http://164.92.167.188:9091) |
| Logging (requires login) |  |

## Contents

- [Missing Implementations](#missing-implementations)
- [Weeks](#weeks)
  - [Week 1](#week-1)
  - [Week 2](#week-2)
  - [Week 3](#week-3)
  - [Week 4](#week-4)
  - [Week 5](#week-5)
  - [Week 6](#week-6)
  - [Week 7](#week-7)
  - [Week 8](#week-8)
  - [Week 9](#week-9)
- [Course description](#course-description)

## Missing Implementations

We currently **lack the following features**:

- Deploy to DigitalOcean with Vagrant.
- Completing our front-end design.
- Refactor the tests to work with new design.
- Implement UI and end-to-end tests.
- Make UML-diagram of system.
- Add Maintainability and Technical Debt estimation tools (Code Climate).
- Add logging.

It would be **nice to have the following features**:

- Create automatic releases when merging into main branch.

## Weeks

### Week 1

Refactor ITU-MiniTwit to work on modern system.

- [x] 1) Adding Version Control.
- [x] 2) Try to develop a high-level understanding of ITU-MiniTwit.
- [x] 3) Migrate ITU-MiniTwit to run on a modern computer running Linux.
- [x] 4) Share your Work on GitHub.

### Week 2

Refactor ITU-MiniTwit in another programming language and tech stack.

- [x] 1) Refactor ITU-MiniTwit to another language and technology of your choice.
- [x] 2) Containerize ITU-MiniTwit with Docker.
- [x] 3) Describe Distributed Workflow.

### Week 3

Continue refactoring, introduction of DB abstraction layer, and deployment of your ITU-MiniTwit to a remote server.

- [x] 1) Implement an API for the simulator in your ITU-MiniTwit.
- [x] 2) Continue refactoring of your ITU-MiniTwit.

### Week 4

Continue refactoring, Setup CI & CD for reproducible builds, tests, delivery, and deployment.

- [x] 1) Complete implementing an API for the simulator in your ITU-MiniTwit.
- [x] 2) Creating a CI/CD setup for your ITU-MiniTwit.
- [x] 3) Continue refactoring of your ITU-MiniTwit.

### Week 5

Cleaning and polishing of your ITU-MiniTwit, introduction of DB abstraction layer, and entering maintenance (Simulator starts).

- Complete and polish your ITU-MiniTwit implementation
  - [x] Add missing features.
  - [x] Introduce a DB abstraction layer in your ITU-MiniTwit.
  
### Week 6

Add monitoring to your ITU-MiniTwit and peer-review.

- [x] Add Monitoring to Your Systems.
- [x] Software Maintenance II: Check the user interface of another group (Group m, *Jason Derulo*)
  - [x] Do you see a public timeline?
  - [x] Does the public timeline show messages that the application received from the simulator?
  - [x] Can you create a new user?
  - [x] Can you login as a new user?
  - [x] Can you write a message?
  - [x] After publishing a message, does it appear on your private timeline?
  - [x] Can you follow another user?

### Week 7

Enhancing CI/CD setup with test suite and static code analysis.

- [x] 1) Add tests to your CI chain (Selenium)
- [x] 2) Enhance your CI Pipelines with at least three static analysis tools
  - [x] eslint (typescript)
  - [x] codeql (csharp)
  - [x] Snyk (containers)
- [ ] 3) Add Maintainability and Technical Debt estimation tools to your projects
  - [x] Sonarqube
  - [ ] Code Climate
- [x] 4) Software Maintenance
  
### Week 8

Add logging to your ITU-MiniTwit.

- [x] 1) Add Logging to Your Systems
- [ ] 2) Test that Your Logging Works
  - The Devs introduce a bug; the Ops resolve the bug by using the logs.

### Week 9

Security Assessment & Pen Testing.

- [ ] 1) Perform a Security Assessment
  - [ ] A. Risk Identification
  - [ ] B. Risk Analysis
  - [ ] C. Pen-Test Your System
- [ ] 2) White Hat Attack The Next Team
  - Group k Radiator -[checks]-> Group m Jason Derulo

## Course description

As a participant of the DevOps course at the IT University of Copenhagen, we are in for a hands-on learning experience. Every week, we will make changes to a Twitter project to put into practice the concepts and tools that we learn in class. Here is what we will be covering during this course:

- Bash: We will learn the basics of the shell, navigate the file system, and execute commands.

- Packaging applications: We will get hands-on experience with packaging applications for distribution and deployment.

- Containerization: We will learn how to create and manage containers to isolate applications and their dependencies.

- Virtual Machines: We will explore the use of virtual machines for testing, development, and deployment.

- CI/CD: We will understand the concepts of Continuous Integration and Continuous Deployment, and implement them using popular tools such as Jenkins and GitLab.

- Monitoring: We will learn how to monitor the performance of our applications and infrastructure to ensure optimal operation.

- Maintainability: We will gain insight into making our applications maintainable, and understand the importance of clean code and testing.

- Log analysis: We will collect and analyze log data to identify and resolve issues in our applications.

- Web security: We will explore the importance of web security and the measures we can take to protect our applications and users.

- Scalability: We will understand the concept of scalability and learn how to design and implement scalable applications.

- Load balancing: We will balance loads across multiple servers to improve performance and reliability.

- Infrastructure as Code: We will manage and provision infrastructure using code, and understand how this can improve the consistency and reliability of our deployments.

By the end of this course, we will have developed a solid foundation in DevOps concepts and practices, and will have the skills and confidence to build, deploy, and manage applications at scale.
