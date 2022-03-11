using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace Bmazon.OpenApi;

public class OpenApiConfigurationOptions : IOpenApiConfigurationOptions
{
  public OpenApiInfo Info { get; set; } = new OpenApiInfo
  {
    Title = "Bmazon APIs",
    Version = "1.0"
  };

  public List<OpenApiServer> Servers { get; set; } = new();

  public OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V3;

  public bool IncludeRequestingHostName { get; set; } = false;
  public bool ForceHttp { get; set; } = true;
  public bool ForceHttps { get; set; } = false;
}
