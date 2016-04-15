using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Framework;

namespace Application
{
    class Program
    {
        static string GetSolutionDirectoryFromAssemblyDirectory(string assemblyDirectory)
        {
            return
                Path.GetDirectoryName(
                    Path.GetDirectoryName(
                        Path.GetDirectoryName(
                            Path.GetDirectoryName(assemblyDirectory))));
        }

        static void Main(string[] args)
        {
            var solutionDirectory = GetSolutionDirectoryFromAssemblyDirectory(Assembly.GetExecutingAssembly().Location);

            var pluginType = typeof (IPlugin);

            var plugins = Directory
                .GetFiles(solutionDirectory, "*.dll", SearchOption.AllDirectories)
                .Select(Assembly.LoadFile)
                .GroupBy(x => x.FullName)
                .Select(x => x.First())
                .SelectMany(x => x.GetExportedTypes())
                .Where(type => type.GetInterface(pluginType.FullName) != null)
                .Select(Activator.CreateInstance)
                .Cast<IPlugin>()
                .ToList();

            plugins.ForEach(x => Console.WriteLine(x.Name));
        }
    }
}
