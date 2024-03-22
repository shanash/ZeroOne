@echo off
SETLOCAL

cd /d "%~dp0"
cd ..

svn revert -R .
svn update
svn resolve --accept=theirs-full -R

ENDLOCAL
PAUSE