using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

//Hoe to create TagHelpers
//https://docs.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/authoring?view=aspnetcore-3.1
//Tag helpers use a naming convention that targets elements of the root class name 
//(minus the TagHelper portion of the class name). In this example, the root name of 
//EmailTagHelper is email, so the <email> tag will be targeted. This naming convention 
//should work for most tag helpers, later on I'll show how to override it.

//To make the EmailTagHelper class available to all our Razor views, 
//add the addTagHelper directive to the Views/_ViewImports.cshtml file:

namespace ASPNETCoreWebApp.Helpers
{
    //The EmailTagHelper class derives from TagHelper. The TagHelper class provides methods 
    //and properties for writing Tag Helpers.

    //If you were to write the email tag self-closing(<email mail-to= "Rick" />), 
    //the final output would also be self-closing.To enable the ability to write 
    //the tag with only a start tag(<email mail-to= "Rick" >) you must mark the class with the following:
    //[HtmlTargetElement("email", TagStructure = TagStructure.WithoutEndTag)]
    public class EmailTagHelper : TagHelper
    {
        private const string EmailDomain = "contoso.com";

        // Can be passed via <email mail-to="..." />. 
        // PascalCase gets translated into kebab-case.
        public string MailTo { get; set; }

        // Can be passed via <email mail-to="..." />. 
        // PascalCase gets translated into kebab-case.
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            Trace.WriteLine("EmailTagHelper.Process(TagHelperContext context, TagHelperOutput output)");
            output.TagName = "a";    // Replaces <email> with <a> tag

            var address = MailTo + "@" + EmailDomain;
            output.Attributes.SetAttribute("href", "mailto:" + address);
            output.Content.SetContent(address);
        }
    }
}
