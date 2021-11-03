@echo off

set reltype=%~1
if not defined reltype set reltype=Release
set _platform="Any CPU"

echo Restore packages ...
call hMSBuild -t:restore /v:q /m:8 /p:Configuration="%reltype%" /p:Platform=%_platform% /nologo || goto err

echo Build IeXod ...
call hMSBuild IeXod.sln /t:Build /v:m /m:6 /p:Configuration="%reltype%" /p:Platform=%_platform% /nologo || goto err

exit /B 0

:err
echo. Build failed. 1>&2
exit /B 1