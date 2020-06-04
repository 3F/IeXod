@echo off

set reltype=%~1
if not defined reltype (
    set reltype=Release
)
set platform="Any CPU"

echo Restore packages ...
call hMSBuild -t:restore /v:q /m:8 /p:Configuration="%reltype%" /p:Platform=%platform% /nologo || goto err

echo Build IeXod ...
call hMSBuild IeXod.sln /t:Rebuild /v:m /m:6 /p:Configuration="%reltype%" /p:Platform=%platform% /nologo || goto err

goto exit

:err

echo. Build failed. 1>&2
exit /B 1

:exit
exit /B 0