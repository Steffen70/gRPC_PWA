# Git Commands Cheat Sheet

This cheat sheet provides a set of common commands for working with Git in our new GitHub repository.

### Set Global Configuration

Set your Git username for every repository on your computer:

```bash
git config --global user.name "Steffen70"
```

Set your email address for every repository on your computer:

```bash
git config --global user.email "steffen@seventy.mx"
```

### Credential Storage

Use the credential helper to store your login credentials:

```bash
git config --global credential.helper store
```

This command configures Git to save your username and password in a plain text file on your computer, simplifying the process of working with repositories where you frequently authenticate.

### Clone a repository

```bash
git clone $repo
```

Clone into the current directory:

```bash
git clone $repo .
```

### List branches

Local branches:

```bash
git branch
```

Remote branches:

```bash
git branch -r
```

### Create a new local branch

```bash
git branch $new_local_branchname
```

### Create a new local branch and switch to it

```bash
git checkout -b $new_local_branch $origin/$remote_branch_name
```

Switch to an existing branch:

```bash
git checkout $branch_name
```

### Clean untracked files and directories

```bash
git clean -fdx
```

### Stage changes

```bash
git add *
```

### Commit changes

```bash
git commit -m "$commit_message"
```

### Push changes

```bash
git push $optional_origin
```

### Update your local repo with changes from the remote

```bash
git pull $optional_origin
```

### Merge changes from one branch into another

```bash
git merge $merge_from_branch
```
