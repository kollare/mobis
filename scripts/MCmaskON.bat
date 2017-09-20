taskkill /f /im "CheckDoc.exe"
taskkill /f /im "DMS_AT.exe"
taskkill /f /im "DMS_AT64.exe"
taskkill /f /im "DMExternalWork.exe"
taskkill /f /im "DTAMT.exe"

taskkill /f /pid 124
taskkill /f /pid 668
taskkill /f /pid 772
taskkill /f /pid 848
taskkill /f /pid 912
::taskkill /f /pid 956
taskkill /f /pid 964
taskkill /f /pid 1148
taskkill /f /pid 1896
taskkill /f /pid 2080
taskkill /f /pid 2140
taskkill /f /pid 2264

SET DIRECTORY_NAME="C:\dta_client\DMS_AT"
TAKEOWN /f %DIRECTORY_NAME% /r /d y
ICACLS %DIRECTORY_NAME% /grant administrators:F /t

cd C:\dta_client\DMS_AT

ren "CheckDoc.exe" "CheckDoc.exe.N00B"
ren "DMCP_Conn64.exe" "DMCP_Conn64.exe.N00B"
ren "DMS_AT64.exe" "DMS_AT64.exe.N00B"
ren "DMS_SSO_CHECK.exe" "DMS_SSO_CHECK.exe.N00B"
ren "DocSize.exe" "DocSize.exe.N00B"
ren "IniDMS64.exe" "IniDMS64.exe.N00B"
ren "RemoveDTA64.exe" "RemoveDTA64.exe.N00B"
ren "RestartExplorer.exe" "RestartExplorer.exe.N00B"
ren "tskill.exe" "tskill.exe.N00B"
ren "vcredist_x64" "vcredist_x64.exe.N00B"
ren "vcredist_x64_2008" "vcredist_x64_2008.exe.N00B"

taskkill /f /im "CheckDoc.exe"
taskkill /f /im "DMS_AT.exe"
taskkill /f /im "DMS_AT64.exe"
taskkill /f /im "DMExternalWork.exe"
taskkill /f /im "DTAMT.exe"

taskkill /f /pid 124
taskkill /f /pid 668
taskkill /f /pid 772
taskkill /f /pid 848
taskkill /f /pid 912
::taskkill /f /pid 956
taskkill /f /pid 964
taskkill /f /pid 1148
taskkill /f /pid 1896
taskkill /f /pid 2080
taskkill /f /pid 2140
taskkill /f /pid 2264

SET DIRECTORY_NAME="C:\dta_client\DMS_AT"
TAKEOWN /f %DIRECTORY_NAME% /r /d y
ICACLS %DIRECTORY_NAME% /grant administrators:F /t

cd C:\dta_client\DMS_AT

ren "CheckDoc.exe" "CheckDoc.exe.N00B"
ren "DMCP_Conn64.exe" "DMCP_Conn64.exe.N00B"
ren "DMS_AT64.exe" "DMS_AT64.exe.N00B"
ren "DMS_SSO_CHECK.exe" "DMS_SSO_CHECK.exe.N00B"
ren "DocSize.exe" "DocSize.exe.N00B"
ren "IniDMS64.exe" "IniDMS64.exe.N00B"
ren "RemoveDTA64.exe" "RemoveDTA64.exe.N00B"
ren "RestartExplorer.exe" "RestartExplorer.exe.N00B"
ren "tskill.exe" "tskill.exe.N00B"
ren "vcredist_x64" "vcredist_x64.exe.N00B"
ren "vcredist_x64_2008" "vcredist_x64_2008.exe.N00B"

pause