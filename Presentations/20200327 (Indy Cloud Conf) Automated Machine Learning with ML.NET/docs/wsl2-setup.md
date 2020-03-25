<!-- markdownlint-disable MD033 -->
# This is documenting my attempt to get Jupyter working in VSCode running against WSL2 so I can run Linux-only frameworks

Note: I got this working first then went back and documented this. I might have forgotten something important! I performed this on Windows 10 1903 (19013.1122).

1. [Install VSCode](https://code.visualstudio.com/) and other general things (git, etc) into Windows.
2. [Install WSL2](https://docs.microsoft.com/en-us/windows/wsl/wsl2-install) (requires Windows 10 18917+). General steps (not quite a step-by-step tutorial):
   1. Under Windows Features, enable "Virtual Machine Platform" and "Windows Subsystem for Linux"
   2. Install distributions via Windows Store. I chose Ubuntu.
   3. Switch WSL to v2: `wsl --set-version <distro> 2`.<br/>Ex: `wsl --set-version Ubuntu 2`
   4. There is a bug ([1](https://github.com/microsoft/WSL/issues/4275), [2](https://github.com/microsoft/WSL/issues/4285)) in WSL2 currently that breaks DNS. The fix is [documented here](https://gist.github.com/coltenkrauter/608cfe02319ce60facd76373249b8ca6#file-fix-wsl2-dns-resolution-L4). A quick `wsl ping google.com` will tell you if you have issues or not. I had to unlink a symlink on `resolv.conf` as part of my fix for this issue.
   3. Update and secure your Linux distro appropriately.
3. Install VSCode's [`Python` extension](https://marketplace.visualstudio.com/items?itemName=ms-python.pythond).
4. Install VSCode's [`Remote - WSL` extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-wsl).
5. Install any other extensions you wish to have.
6. Place code in appropriate location. My examples in this document will use `C:\Demo\src`.
7. Open a command prompt in the `C:\Demo\src` folder and type `wsl code .` and it will open the folder from Linux.
8. Observe that most/all of the extensions you installed have a button to "Install in WSL ...". You must re-install the Linux versions of these extensions.
9. In the bottom-left corner of the Status Bar, you can choose to connect to these files locally in Windows or remotely in WSL. This is how you toggle back and forth between a Windows and Linux environment. "Remote-WSL: Reopen Folder in WSL" and "Remote-WSL: Reopen Folder in Windows" are the two options I use. Likewise, if you run `code .` from a bash prompt vs Windows cmd prompt, it should connect to the appropriate OS (at least after the first time you open it in WSL).
10. Install Anaconda in WSL.
    1. Find the link to the latest version here: <https://www.anaconda.com/distribution/#linux>
    2. From a bash prompt, in the folder you wish to download Anaconda to: `wget {link}`<br/>Ex: `wget https://repo.anaconda.com/archive/Anaconda3-2019.10-Linux-x86_64.sh`
    3. Now run the installer: `bash Anaconda[YOUR VERSION].sh`<br/>Ex: `bash Anaconda3-2019.10-Linux-x86_64.sh`
    4. From here, you can use `conda create` to create environments as needed.

## Tips

1. Line endings are a pain with git. Line endings are especially a pain with git when you're editing the same files (i.e. same blocks on the same hard drive) in both Windows and Linux at the same time during the same editing session! Auto-converting line ending conversion seems nice but is your worst enemy here. Disable them! Details: <https://code.visualstudio.com/docs/remote/troubleshooting#_resolving-git-line-ending-issues-in-containers-resulting-in-many-modified-files><br>
**.gitattributes** file to fix:<br/>
    ```
    * text=auto eol=lf
    *.{cmd,[cC][mM][dD]} text eol=crlf
    *.{bat,[bB][aA][tT]} text eol=crlf
    ```
2. Whatever folder you run `code .` or `wsl code .` in will be your working folder. This is important for generated artifacts, including `.vscode` folders & contents.
3. Running `bash` will start your WSL VM up. Running `wsl --shutdown` will shut it down. Mine does not run automatically but is very fast to boot when I start it up.
4. If you don't have gcc installed, you must install it. I used: `sudo apt install g++` to install it. In order to tell if you have it installed, run: `g++ -v`
