using Microsoft.AspNetCore.Routing;

namespace UserMasterCategory.API.Extensions;

public static class EndpointExtensions
{
    public static void LogAllEndpoints(this IEndpointRouteBuilder endpoints)
    {
        Console.WriteLine("\n");
        Console.WriteLine("==========================================");
        Console.WriteLine("🚀 USER MASTER CATEGORY API ENDPOINTS:");
        Console.WriteLine("==========================================");
        
        foreach (var dataSource in endpoints.DataSources)
        {
            foreach (var endpoint in dataSource.Endpoints)
            {
                if (endpoint is RouteEndpoint routeEndpoint)
                {
                    var method = routeEndpoint.Metadata.GetMetadata<HttpMethodMetadata>()?.HttpMethods.FirstOrDefault() ?? "ANY";
                    var route = routeEndpoint.RoutePattern.RawText ?? string.Empty;
                    
                    if (!string.IsNullOrEmpty(route) && !route.Contains("swagger") && !route.Contains("."))
                    {
                        Console.WriteLine($"{method,-7} http://localhost:5266/{route}");
                    }
                }
            }
        }
        Console.WriteLine("==========================================");
        Console.WriteLine("\n");
    }
}