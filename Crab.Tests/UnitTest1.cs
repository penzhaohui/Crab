using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Crab.Runtime.Logic.Common;
using Crab.Runtime.Contract.Exceptions;
using System.Security.Principal;

namespace Crab.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CreateUser()
        {
            try
            {
                IsAdministrator();

                string tenantName = "missionsky5";
                string username = "missionsky24";
                string password = "missionsky24";
                string email = "missionsky19@missionsky19.com";

                using (new Impersonator("msadmin", "ms-dev3.cloudapp.net", "Msadm1n;"))
                {
                    AdamTenantManager adamManager = AdamConfigurationHelper.AdamManager;
                    adamManager.Username = "ms-dev3\\msadmin";
                    adamManager.Password = "Msadm1n;";
                    adamManager.SecureConnection = true;
                    
                    // adamManager.SecureConnection = true;
                    //adamManager.CreateTenant(tenantName);
                    adamManager.ValidateUser(tenantName, username, password);

                    if (adamManager.UserExists(tenantName, username))
                        throw new AuthenticationException(string.Format("The user with name {0} already exists!", username));
                    adamManager.CreateUser(tenantName, username, password, email);

                    adamManager.SetPassword(tenantName, username, password);
                    adamManager.ChangePassword(tenantName, username, password, password);
                }
            }
            catch (Exception ex)
            {
                throw new AuthenticationException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 确定当前主体是否属于具有指定 Administrator 的 Windows 用户组
        /// </summary>
        /// <returns>如果当前主体是指定的 Administrator 用户组的成员，则为 true；否则为 false。</returns>
        public static bool IsAdministrator()
        {
            bool result;
            try
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                result = principal.IsInRole(WindowsBuiltInRole.Administrator);

                //http://www.cnblogs.com/Interkey/p/RunAsAdmin.html
                //AppDomain domain = Thread.GetDomain();
                //domain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
                //WindowsPrincipal windowsPrincipal = (WindowsPrincipal)Thread.CurrentPrincipal;
                //result = windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                result = false;
            }
            return result;
        }
    }
}
