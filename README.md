# Pan O Phobia

```
░█▀█░█▀█░█▀█░░░░░█▀█░░░░░█▀█░█░█░█▀█░█▀▄░▀█▀░█▀█
░█▀▀░█▀█░█░█░▄▄▄░█░█░▄▄▄░█▀▀░█▀█░█░█░█▀▄░░█░░█▀█
░▀░░░▀░▀░▀░▀░░░░░▀▀▀░░░░░▀░░░▀░▀░▀▀▀░▀▀░░▀▀▀░▀░▀
```

> ![NOTE]
> This section is a WIP

## Getting started

### Requirements

- `meld`

```sh
brew install --cask meld
```

> This aint working anymore because meld got deprecated on homebrew casks, try either finding a 3rd party .app of Meld, or just replace Meld with another app of your choice.

- `.gitconfig`

> [!TIP]
> Use `git config edit` in the root of the project to add these options at `<root>/.git/config`.

```toml
[core]
	repositoryformatversion = 0
	filemode = true
	bare = false
	logallrefupdates = true
	ignorecase = true
	precomposeunicode = true
[remote "origin"]
	url = https://github.com/K4N-Studios/pan-o-phobia.git
	fetch = +refs/heads/*:refs/remotes/origin/*
[branch "main"]
	remote = origin
	merge = refs/heads/main
	vscode-merge-base = origin/main
[branch "testing"]
	remote = origin
	vscode-merge-base = origin/testing
	merge = refs/heads/testing
[branch "develop"]
	remote = origin
	merge = refs/heads/develop
[merge]
	tool = meld
[mergetool "meld"]
	path = /opt/homebrew/bin/meld
```
