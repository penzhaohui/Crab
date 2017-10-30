;%windir%\adam\adaminstall.exe /answer:answer.txt
set adamroot=%windir%\adam
%adamroot%\dsmgmt "ds behavior" "connections" "connect to server localhost:389" "q" "allow passwd op on unsecured connection" "q" "q"
cscript initadam.vbs "LDAP://localhost:389/dc=saas,dc=com"
