# Git Commands Cheat Sheet

This cheat sheet provides a set of common commands for working with Git in our new GitHub repository.

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
