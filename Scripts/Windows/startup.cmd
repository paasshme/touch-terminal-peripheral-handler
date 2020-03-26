set /p var=<"%~dp0path.txt"
"%var%winLaunch.cmd" >> "%TEMP%\StartupLog.txt" 2>&1
cmd /k
