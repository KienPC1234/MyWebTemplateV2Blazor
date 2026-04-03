@echo off
echo Installing dependencies...
call npm install
echo.
echo Building Tailwind CSS...
call npm run build
echo.
echo Build completed!
pause
