@echo off
@REM We don't use XUnit.Runner.targets(Arcade) but here we also need support our XUnit Traits

setlocal enableDelayedExpansion
@REM set "OsEnvironment=windows"
set "MonoBuild="
set "XUnitConsole="
set _platform="Any CPU"

set _configuration=%~1
if not defined _configuration set _configuration=Release

:::~ 

if defined XUnitConsole ( @REM xunit.console.exe (xunit.runner.console package) -trait, -notrait

    set "_L=-notrait category="
    set "_R= "
    set cmd=xunit.console.exe TODO

) else ( @REM dotnet test --filter "..."

    set "_L=category^^^!="
    set "_R=&"
    set cmd=dotnet test -c %_configuration% /p:Platform=%_platform% --no-build --nologo --no-restore -v q %~2
)
set "_=!_R!!_L!"

:::~ Construct active traits

set "Args=!_L!failing!_!nonwindowstests"

if defined MonoBuild ( call :con
    set "Args=!Args!!_L!non-mono-tests!_!nonmonotests!_!mono-windows-failing"
)
call :con
set "ArgsNETFX=!Args!!_L!nonnetfxtests"
set "ArgsNETCORE=!Args!!_L!nonnetcoreapptests"

:::~ call

@echo Test using .NETFX based assemblies ...
echo net472... & call %cmd% --framework net472 --filter "%ArgsNETFX%" || goto err

@echo Test using .NET Core based assemblies ...
echo netcoreapp2.1... & call %cmd% --framework netcoreapp2.1 --filter "%ArgsNETCORE%" || goto err

exit /B 0
:::~ ~ ~ ~ ~ ~

:con
if defined Args set "Args=!Args!!_R!"
exit /B 0

:err
echo. Tests failed. 1>&2
exit /B 1