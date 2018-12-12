using System;
using System.Net;
using System.Security;
using Microsoft.SharePoint.Client;

namespace ConsoleApplication1
{
  internal class Program
  {
    public static void Main(string[] args)
    {
      using (var context = new ClientContext("https://sharepoint_site")) {
        var password = new SecureString();
        
        // #nemt
        foreach (var c in "secretpassword".ToCharArray()) password.AppendChar(c); 
        context.Credentials = new SharePointOnlineCredentials("account@domain.com", password);
        
        var web = context.Web;
        
        context.Load(web);
        context.ExecuteQuery();

        var group = web.SiteGroups.GetById(5);
        context.ExecuteQuery();
        
        var topfolder = web.GetFolderByServerRelativeUrl("Ejendomme");
        context.Load(topfolder);
        context.ExecuteQuery();

        var folder = topfolder.Folders.Add("Test");
        context.ExecuteQuery();

        var list = folder.ListItemAllFields;
        context.ExecuteQuery();
        list.BreakRoleInheritance(false, true);
        var role = new RoleDefinitionBindingCollection(context);
        role.Add(web.RoleDefinitions.GetByType(RoleType.Contributor));
        list.RoleAssignments.Add(group, role);
        list.Update();
        context.ExecuteQuery();
        
        Console.WriteLine(web.Title);
      }
    }
  }
}