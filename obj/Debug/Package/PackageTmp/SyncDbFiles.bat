@echo off 
set target=G:\My Drive\2026\DatabaseBackup-UsedByChauSoftware
set source=C:\GianHangTet2026


SETLOCAL ENABLEDELAYEDEXPANSION
:: put your desired field delimiter here.
:: for example, setting DELIMITER to a hyphen will separate fields like so:
:: yyyy-MM-dd_hh-mm-ss
::
:: setting DELIMITER to nothing will output like so:
:: yyyyMMdd_hhmmss
::
SET DELIMITER=%1

SET DATESTRING=%date:~-4,4%%DELIMITER%%date:~-7,2%%DELIMITER%%date:~-10,2%
SET TIMESTRING=%TIME%
::TRIM OFF the LAST 3 characters of TIMESTRING, which is the decimal point and hundredths of a second
set TIMESTRING=%TIMESTRING:~0,-3%

:: Replace colons from TIMESTRING with DELIMITER
SET TIMESTRING=%TIMESTRING::=!DELIMITER!%

:: if there is a preceeding space substitute with a zero
echo DateTime Stamp: %DATESTRING%_%TIMESTRING: =0%


mkdir "%target%\%DATESTRING%_%TIMESTRING: =0%" 

REM now sync and copy the files/folders
xcopy /S /E "%source%\*.*" "%target%\%DATESTRING%_%TIMESTRING: =0%" 
