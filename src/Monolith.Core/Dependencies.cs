using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Monolith.Core
{
    public static class Dependencies
    {
        private static readonly string Prefix = GetPrefix();

        private static bool _loaded;

        private static IImmutableList<Assembly> _assemblies;

        private static IImmutableList<Type> _types;

        public static IImmutableList<Assembly> Assemblies
        {
            get
            {
                Ensure();

                return _assemblies;
            }
        }

        public static IImmutableList<Type> Types
        {
            get
            {
                Ensure();

                return _types;
            }
        }

        public static void Initialize()
        {
            if (_loaded)
            {
                return;
            }

            var assemblies = GetReferencedAssemblies().ToList();
            var sources = assemblies.Select(assembly => assembly.Location).Distinct();
            var targets = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, $"{Prefix}.*.dll")
                .Where(path => !sources.Contains(path, StringComparer.InvariantCultureIgnoreCase));

            assemblies.AddRange(targets.Select(path =>
                AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));

            _loaded = true;
            _assemblies = assemblies.ToImmutableList();
            _types = assemblies.SelectMany(assembly => assembly.GetTypes()).ToImmutableList();
        }

        #region Private Methods

        private static void Ensure()
        {
            if (!_loaded)
            {
                throw new Exception($"Assemblies have not been loaded, you must call '{nameof(Dependencies)}.{nameof(Initialize)}()', on the application startup");
            }
        }

        private static bool IsValidAssembly(AssemblyName name)
        {
            return name?.FullName.StartsWith(Prefix) ?? false;
        }

        private static bool IsValidAssembly(Assembly assembly)
        {
            return IsValidAssembly(assembly.GetName());
        }

        private static string GetPrefix()
        {
            var type = typeof(Dependencies);
            var name = type.FullName ?? string.Empty;
            var match = Regex.Match(name, @"^\w+");

            if (string.IsNullOrWhiteSpace(match.Value))
            {
                throw new Exception("Unable to determine assembly prefix");
            }

            return match.Value;
        }

        private static IEnumerable<Assembly> GetReferencedAssemblies()
        {
            var attendance = new List<string>();
            var stack = new Stack<Assembly>();

            stack.Push(Assembly.GetEntryAssembly());

            do
            {
                var reference = stack.Pop();

                if (IsValidAssembly(reference))
                {
                    yield return reference;
                }

                var referenced = reference.GetReferencedAssemblies().Where(IsValidAssembly);

                foreach (var name in referenced)
                {
                    if (attendance.Contains(name.FullName) || Assembly.Load(name) is not { IsDynamic: false } assembly)
                    {
                        continue;
                    }

                    attendance.Add(assembly.FullName);

                    stack.Push(assembly);
                }

            }
            while (stack.Count > 0);
        }

        #endregion
    }
}
