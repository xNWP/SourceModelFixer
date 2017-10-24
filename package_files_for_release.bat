ECHO OFF
CLS

DEL /P *.7z *.zip
CLS

SET "sevzip="C:\Program Files\7-Zip\7z.exe""
SET "version=1.0"
SET "filelist=SourceModelFixer.exe README.txt"

CD SourceModelFixer\bin\Release
%sevzip% a -t7z SMF-%version%.7z %filelist%
%sevzip% a -tzip SMF-%version%.zip %filelist%

MOVE SMF-%version%.7z ..\..\..\
MOVE SMF-%version%.zip ..\..\..\

ECHO.
ECHO Packaged.
PAUSE > nul