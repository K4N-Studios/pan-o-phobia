## Getting started

To get started a dev environment make sure to clone the repository first

```sh
git clone https://github.com/k4n-studios/pan-o-phobia.git pan-o-phobia-unity
```

Then make sure you met the next requirements

### Download the FMOD Project

FMOD project will be required to build the project with audio support.

> [!NOTE]
> Download links are WIP

```sh
unzip pan-o-phobia-fmod.zip
```

Then organise the project in the next structure (the project expects smth similar)

```sh
mkdir -pv ~/work/gamedev
mv -v ~/pan-o-phobia-unity ~/work/gamedev/pan-o-phobia
mkdir -pv `dirname !$`/fmod
mv -v ~/pan-o-phobia-fmod !$/pan-o-phobia
test -f !$/pan-o-phobia.fspro && echo "Project has been setup correctly"
```

This little sequence of command should generate a folder structure such as

```
ls -la ~/work/gamedev/{fmod,pan-o-phobia}
/Users/alpha/work/gamedev/fmod:
.rw-r--r--@ 6.1k alpha 24 Jan 17:49 .DS_Store
drwxr-xr-x@    - alpha 24 Jan 17:45 pan-o-phobia

/Users/alpha/work/gamedev/pan-o-phobia:
drwxr-xr-x@    - alpha 24 Jan 16:24 .git
.rw-r--r--@ 1.5k alpha 24 Jan 13:53 .gitignore
drwxr-xr-x@    - alpha 24 Jan 13:53 .scripts
drwxr-xr-x@    - alpha 24 Jan 13:51 .vscode
.rw-r--r--@  71k alpha 24 Jan 14:02 Assembly-CSharp.csproj
drwxr-xr-x@    - alpha 24 Jan 14:06 Assets
.rw-r--r--@  252 alpha 24 Jan 14:27 CHANGELOG.md
.rw-r--r--@  779 alpha 24 Jan 14:29 CONTRIBUTORS.md
drwxr-xr-x@    - alpha 24 Jan 14:18 doc
drwxr-xr-x@    - alpha 24 Jan 14:58 Library
.rw-r--r--@  21k alpha 24 Jan 13:52 LICENSE
drwxr-xr-x@    - alpha 24 Jan 14:01 Logs
drwxr-xr-x@    - alpha 24 Jan 13:51 Packages
.rw-r--r--@  438 alpha 24 Jan 14:06 pan-o-phobia.sln
drwxr-xr-x@    - alpha 24 Jan 13:52 ProjectSettings
.rw-r--r--@ 1.0k alpha 24 Jan 14:19 README.md
drwxr-xr-x@    - alpha 24 Jan 14:58 Temp
drwxr-xr-x@    - alpha 24 Jan 13:51 UserSettings
```

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
