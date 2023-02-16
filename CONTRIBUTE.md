# Distributed Workflow

In this file, we outline the workflow for collaborating on this project using Git and GitHub.

**Which repository setup will we use?**

We will be using a single repository for all code.
The project’s components will be divided into subdirectories from the root directory.

**Which branching model will we use?**

We will be using Trunk based Development, where we work with three types of branches.

1. The main branch (long-lived)
2. The dev branch (long-lived)
3. Several feature branches (short-lived)

The main branch will contain the newest working instance of MiniTwit. Furthermore, it will be push protected, thereby enforcing pull request in order to update it.

The dev branch will serve as the newest working version of MiniTwit containing new features that haven’t been published yet. The group will branch out from this branch to create short-lived feature branches.

The feature branches are short-lived branches containing new features for MiniTwit. As soon as a feature is completed, the branch will be merged with the dev branch via pull requests. Once merged, the branch will be deleted. 

**Which distributed development workflow will we use?**

We will use a Kanban board, integrated in GitHub, to assign tasks to developers. This will help us stay organized and provide an overview of our progress on various tasks.

**How do we expect contributions to look like?**

We understand contributions as being one of the following two:

- Commits
- Pull Request

We expect commitments to be easy to understand through the use of conventions in the commit messages. A commit message can be one of the following types:

- **fix**: When a bug gets patched (repaired/fixed) .
- **feat**: When a new feature has been implemented.
- **chore**: When updating something of less importance.
- **merge**: When branches are merged.
- **docs**: Documentation has been created or changes in documentation has been made.
- **test**: A test has been implemented.
- **setup:** When a project has been setup up or important files required for a project has been setup.

A commit message will have the following format:

```txt
<commit type>([scope]): <message>
```

where `<commit type>` is one of the 7 commit types, `[scope]` (optional) being the name of the file changed, and `<message>` being the actual commit message body containing a description of what has changed.

Pull Requests, will contain a description of the most important features implemented in the feature branch to be merged into one of the long-lived branches.

**Who is responsible for integrating/reviewing contributions?**

We will all review different contributions. Each pull request will be assigned to a different developer for review and integration of the updated program.
