@echo off
title FYP Database Reset Utility
color 0A
echo ============================================================
echo      RESETTING AND SEEDING SMART FYP DATABASE (FYPDB)
echo ============================================================
echo.
echo Connecting to SQL Express (.\SQLEXPRESS) and executing database_setup.sql...
echo.

sqlcmd -S .\SQLEXPRESS -E -i "c:\Users\Mehal\OneDrive\Pictures\mehaalkhan_semester_data\semester 6\database labs\project\Database Project final\FYPManagementSystem\database_setup.sql"

if %ERRORLEVEL% NEQ 0 (
    color 0C
    echo.
    echo [ERROR] Database reset failed! Make sure SQL Server Express is running.
) else (
    echo.
    echo ============================================================
    echo [SUCCESS] Database dropped, recreated, and seeded successfully!
    echo ============================================================
)
echo.
pause
