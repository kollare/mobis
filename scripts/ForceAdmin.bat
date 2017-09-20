SET DIRECTORY_NAME="C:\dta_client\DMS_AT"
TAKEOWN /f %DIRECTORY_NAME% /r /d y
ICACLS %DIRECTORY_NAME% /grant administrators:F /t
PAUSE