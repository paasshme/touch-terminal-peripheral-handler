set /p var=<"%~dp0path.txt"
start "%var%winLaunch.cmd" >> "%TEMP%\StartupLog.txt" 2>&1
cmd /k