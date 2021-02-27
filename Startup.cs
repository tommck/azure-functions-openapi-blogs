using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using AzureFunctions.Extensions.Swashbuckle;
using System.Reflection;
using Bmazon.Services;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Bmazon.Startup))]
namespace Bmazon
{
  public class Startup : FunctionsStartup
  {
    public override void Configure(IFunctionsHostBuilder builder)
    {
      builder.AddSwashBuckle(Assembly.GetExecutingAssembly());

      builder.Services.AddSingleton<OrderService>();
    }
  }
}
