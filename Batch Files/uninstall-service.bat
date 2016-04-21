@echo off

set ServiceName=Dev.WinServiceBase
set DisplayName=Dev Win Service Base
set ServiceFileName=C:\Services\WinServiceBase\WinServiceBase.exe

sc delete %ServiceName% binPath= "%ServiceFileName%" DisplayName= "%DisplayName%" type= own start= demand

set ServiceName=
set DisplayName=
set ServiceFileName=

pause