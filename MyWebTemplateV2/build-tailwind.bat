@echo off
cd /d "%~dp0"
cd MyWebTemplateV2
echo Installing dependencies in MyWebTemplateV2 project...
call npm install
echo.
echo Building Tailwind CSS...
call npm run build
echo.
echo Build completed!
pause
