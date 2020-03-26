@echo off
set /p var=< "%~dp0path.txt"
PowerShell -Command "Set-ExecutionPolicy Unrestricted" >> "%TEMP%\StartupLog.txt" 2>&1
PowerShell %var%\winLaunch.ps1 >> "%TEMP%\StartupLog.txt" 2>&1
