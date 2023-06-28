using ConsoleTables;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Oakton;

namespace DebugEnvVariables;

[Description("List all routes in web api", Name = "list-routes")]
public class ListRoutesCommand : OaktonCommand<NetCoreInput>
{
    public override bool Execute(NetCoreInput input)
    {
        using var host = input.BuildHost();
        var serviceType = typeof(IApiDescriptionGroupCollectionProvider);

        if (host
                .Services
                .GetService(serviceType) is not IApiDescriptionGroupCollectionProvider explorer)
            return true;

        var results = explorer
            .ApiDescriptionGroups
            .Items
            .SelectMany(x => x.Items)
            .Select(x => new
            {
                x.RelativePath,
                x.HttpMethod,
                Controller = x.ActionDescriptor.RouteValues["controller"],
                Action = x.ActionDescriptor.RouteValues["action"],
                Parameters = string.Join(", ", x.ParameterDescriptions.Select(p => $"{p.Name} ({p.Type.Name})"))
            })
            .ToList();

        Console.Clear();
        Console.WriteLine();

        ConsoleTable
            .From(results)
            .Write();

        return true;
    }
}