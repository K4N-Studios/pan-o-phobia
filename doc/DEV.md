## Getting started

To get started a dev environment make sure to clone the repository first

```sh
git clone https://github.com/k4n-studios/pan-o-phobia.git
cd pan-o-phobia
```

Then make sure you met the next requirements

### Requirements

- `meld`

```sh
brew install --cask meld
```

> This aint working anymore because meld got deprecated recently on homebrew casks formulaes, try either finding a 3rd party .app of Meld, or just replace Meld with another app of your choice.

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

After this you will have to tryna import the project using unity hub and open it with the recommended editor version.
