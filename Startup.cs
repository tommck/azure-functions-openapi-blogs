using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using AzureFunctions.Extensions.Swashbuckle;
using System.Reflection;
using Bmazon.Services;
using Microsoft.Extensions.DependencyInjection;
using AzureFunctions.Extensions.Swashbuckle.Settings;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

[assembly: FunctionsStartup(typeof(Bmazon.Startup))]
namespace Bmazon
{
  public class Startup : FunctionsStartup
  {
    public override void Configure(IFunctionsHostBuilder builder)
    {
      builder.AddSwashBuckle(Assembly.GetExecutingAssembly(), opts =>
      {
        // incorporate the XML documentation
        opts.XmlPath = "Bmazon.xml";

        // set up the docs with the same names as the group names used in the code
        opts.Documents = new SwaggerDocument[] {
          new SwaggerDocument()
          {
            Name = "Everything",
            Title = "Bmazon All APIs",
            Description = "All APIs",
            Version = "1.0"
          },
          new SwaggerDocument()
          {
            Name = "Shopping",
            Title = "Bmazon Shopping API",
            Description = "API to handle Orders and shipping information for the Shopping Department",
            Version = "1.0"
          },
          new SwaggerDocument()
          {
            Name = "Warehouse",
            Title = "Bmazon Warehouse API",
            Description = "API to receive shipping formation from the Bmazon Warehouse",
            Version = "1.0"
          }
        };

        opts.ConfigureSwaggerGen = genOpts =>
        {
          // configure the separate document inclusion logic
          genOpts.DocInclusionPredicate((docName, apiDesc) =>
          {
            // if we're generating the "everything" document, then include this method
            if (docName == "Everything")
            {
              return true;
            }

            if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

            // pull the value(s) of the [ApiExplorerSettings(GroupName= "foo")] attribute
            var attr = methodInfo.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().FirstOrDefault();

            var groupName = attr?.GroupName;

            // always return it if it's shared. Otherwise compare doc names
            return groupName == "Shared" || groupName == docName;
          });
        };
      });

      builder.Services.AddSingleton<OrderService>();
    }
  }
}
