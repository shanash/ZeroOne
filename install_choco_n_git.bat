@echo off
SETLOCAL

echo Checking for Chocolatey installation...
IF EXIST "C:\ProgramData\chocolatey" (
    echo Chocolatey is already installed.
) ELSE (
    echo Installing Chocolatey...
    PowerShell -Command "echo 'Starting Chocolatey installation...'; Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1')); echo 'Chocolatey installation completed.'"
)

echo Installing Git...
PowerShell -Command "echo 'Starting Git installation...'; choco install git -y; echo 'Git installation completed.'"

echo Installation process complete.

ENDLOCAL
PAUSE
