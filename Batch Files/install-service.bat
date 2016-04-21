@echo off

set ServiceName=Dev.WinServiceBase
set DisplayName=Dev Win Service Base
set ServiceFileName=C:\Services\WinServiceBase\WinServiceBase.exe

sc create %ServiceName% binPath= "%ServiceFileName%" DisplayName= "%DisplayName%" type= own start= demand
sc failure %ServiceName% reset= 60 actions= restart/0/restart/30000//

set ServiceName=
set DisplayName=
set ServiceFileName=

pause
