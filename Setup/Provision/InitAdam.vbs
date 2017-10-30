set Args  = Wscript.Arguments
ouName  = Args(0)
' If the application OU DN is "ou=adamou,c=us" and the server is "adamhost" and the port is 389. Then this parameter should be passed
' as follows:  "LDAP://adamhost:389/ou=adamou,c=us"

set ou = GetObject(ouName)
Set objTenants = ou.Create("domainDNS", "dc=Tenants")
objTenants.Put "description", "Tenants Root Node"
objTenants.SetInfo

Set internal = ou.Create("container", "cn=Internal")
internal.setinfo
Set admin = internal.Create("user", "cn=Administrator")
admin.setinfo
admin.SetOption 6, 389
admin.SetOption 7, 1
admin.SetPassword "Pass@word"
admin.setinfo

set objGroup = GetObject("LDAP://localhost:389/cn=Administrators,cn=Roles,dc=saas,dc=com")
objGroup.Add(admin.ADsPath)
objGroup.SetInfo







