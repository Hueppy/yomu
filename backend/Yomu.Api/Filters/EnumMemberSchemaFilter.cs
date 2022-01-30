using System.Text;
using System.Xml.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PfotenFreunde.Api.Filters;

public class EnumMemberSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
		var type = context.Type;
		if (!type.IsEnum)
		{
			return;
		}

		var sb = new StringBuilder();

		sb.AppendLine("| Value | Name |");
		sb.AppendLine("|-------|------|");
		foreach (var name in Enum.GetValues(type))
		{
			sb.AppendLine($"| {Convert.ToInt64(name)} | {name} |");
		}

		schema.Description = sb.ToString();
    }
}
