set netpath=%windir%\Microsoft.NET\Framework\v2.0.50727
sqlcmd -S localhost -E -i WebAppDatabase.sql
%netpath%\aspnet_regsql.exe -E -d CrabWebApp -A all