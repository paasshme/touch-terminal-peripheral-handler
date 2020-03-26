@echo off
set /p var=<"%~dp0path.txt"
"%var%winLaunch.cmd" >> "%TEMP%\StartupLog.txt" 2>&1
