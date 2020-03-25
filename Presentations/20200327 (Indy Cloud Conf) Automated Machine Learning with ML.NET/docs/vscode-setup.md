<!-- markdownlint-disable MD033 -->
# This is documenting my attempt to get Jupyter working in VSCode and VSCode working with github repos

1. Install VSCode: <https://code.visualstudio.com/>
2. Install git: <https://git-scm.com/downloads>
3. Install VSCode's Python extension: <https://marketplace.visualstudio.com/items?itemName=ms-python.python>
   - Also consider IntelliCode, which works well with Python: <https://marketplace.visualstudio.com/items?itemName=VisualStudioExptTeam.vscodeintellicode>
4. Install newest version of Anaconda: <https://www.anaconda.com/distribution/>
5. Reboot
6. Switch Python version (bottom status bar) to the one that is installed with Anaconda. Mine is `~\Anaconda3\python.exe` and was selected by default (because I specified it to be the default during the Anaconda installation wizard), but others may be there if you have Python installed via other means.
7. Open Terminal: <kbd>CTRL</kbd> + <kbd>`</kbd>
8. Open a `.ipynb` file in VSCode or create a new notebook: <kbd>CTRL</kbd> + <kbd>SHIFT</kbd> + <kbd>P</kbd> -> "Python: Create New Blank Jupyter Notebook"
9. Try to run the notebook. If you created a new one, add a new cell with `print(1)` as the only line of code and it <kbd>SHIFT</kbd> + <kbd>ENTER</kbd> to run that cell, or use the toolbar icons. It should output "1". At this point, things work! Now we just need to create/setup our Anaconda environment(s).
10. Open an Anaconda console window: Start -> Anaconda PowerShell Prompt (Anaconda3)
11. Create a new environment: `conda create -n fluffybunnyname python=3.7` (You may need to use a different version of Python. 3.7 is what I installed with Anaconda.)
12. If you wish to do anything with this enviornment in this window, activate it: `conda activate fluffybunnyname`
13. In VSCode, set the Python Venv Path setting. Go to Settings (<kbd>CTRL</kbd> + <kbd>,</kbd>), search for `python.venvPath`, and set it to where the `envs` folder from Anaconda, which is displayed in the above console window. For me, it was: `C:\Users\Shane\Anaconda3\envs` or more simply `~\Anaconda3\envs`
14. In VSCode, you can select your environment via <kbd>CTRL</kbd> + <kbd>SHIFT</kbd> + <kbd>P</kbd> -> "Python: Select Interpreter". Note: It can take several minutes before a new environment can show up here. This is super annoying. If the "Starting Jupyter Server" message seems to spin forever or you never see your new environment, try some of the following things:
    - Restart VSCode
    - Open the Anaconda Navigator, specify the proper environment here (it will be listed), and Launch VSCode from here

MyBinder URL: <https://mybinder.org/v2/gh/jaxidian/zzz_bindertest/master> - NOTE: walltime lies here, as do MB/s measurements! Something's funny with timers/clocks here. Training also seems pretty buggy.
