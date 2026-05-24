@echo off
title SaaS - Crear SuperAdmin
color 0B
echo ============================================
echo   SaaS - Crear SuperAdmin
echo ============================================
echo.

set API_URL=http://localhost:5203
set EMAIL=admin@saas.com
set PASSWORD=Admin123!

:: 1. Verificar conexion
echo [1/3] Verificando conexion al backend...
curl -s "%API_URL%/api/setup/status" >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo [ERROR] No se puede conectar a %API_URL%
    echo Asegurate de que el backend este corriendo.
    echo.
    pause
    exit /b 1
)
echo [OK] Backend conectado

:: 2. Verificar si ya existe SuperAdmin
echo [2/3] Verificando si el SuperAdmin ya existe...
curl -s "%API_URL%/api/setup/status" > "%TEMP%\saas-status.json"
findstr "true" "%TEMP%\saas-status.json" | findstr "superAdminExists" >nul
if %ERRORLEVEL% equ 0 (
    echo [OK] El SuperAdmin ya existe
    echo.
    type "%TEMP%\saas-status.json"
    echo.
    goto :login_test
)

:: 3. Ejecutar seed
echo [3/3] Ejecutando seed de datos...
curl -s -X POST "%API_URL%/api/setup/seed" -H "Content-Type: application/json"
echo.
echo [OK] Seed completado

:login_test
echo.
echo ============================================
echo   Prueba de login
echo ============================================
echo.
curl -s -X POST "%API_URL%/api/auth/login" -H "Content-Type: application/json" -d "{\"email\":\"%EMAIL%\",\"password\":\"%PASSWORD%\"}"
echo.
echo.
echo ============================================
echo   Credenciales del SuperAdmin
echo ============================================
echo   Email:    %EMAIL%
echo   Password: %PASSWORD%
echo   URL:      %API_URL%/swagger
echo   Frontend: http://localhost:5173/login
echo ============================================
echo.
pause
