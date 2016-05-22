using System;
using System.Reflection;

namespace JordanHall.Ibm.Nlc.SiteApi.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}