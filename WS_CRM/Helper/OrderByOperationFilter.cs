using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace WS_CRM.Helper
{
    public class OrderByOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var attribute = context.MethodInfo
                               .GetCustomAttributes(true)
                               .OfType<SwaggerOrderByAttribute>()
                               .FirstOrDefault();

            if (attribute == null) return;

            var properties = attribute.ModelType
                                  .GetProperties()
                                  .Select(p => p.Name)
                                  .ToList();

            var orderByParam = operation.Parameters
                .FirstOrDefault(p => p.Name.Equals("order_by", StringComparison.OrdinalIgnoreCase));

            if (orderByParam != null)
            {
                orderByParam.Schema.Enum = properties
                    .Select(p => (Microsoft.OpenApi.Any.IOpenApiAny)
                        new Microsoft.OpenApi.Any.OpenApiString(p))
                    .ToList();
            }
        }
   
        public static string ValidateOrderBy<T>(string? orderBy)
        {
            if (string.IsNullOrEmpty(orderBy))
                return "id"; // default column

            var properties = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => p.Name.ToLower())
                .ToList();

            if (!properties.Contains(orderBy.ToLower()))
                throw new ArgumentException("Invalid column name");

            return orderBy;
        }
   
    }

}