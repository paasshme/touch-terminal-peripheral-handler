@echo off
@echo %~dp0> .\path.txt
copy /Y .\path.txt "%appdata%\Microsoft\Windows\Start Menu\Programs\Startup"
copy /Y .\startup.cmd "%appdata%\Microsoft\Windows\Start Menu\Programs\Startup"
del .\path.txt