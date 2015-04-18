using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BassUtils.Registration
{
    /*
    /// <summary>
    /// Provide support for convention-based DI registration scenarios: Get all interfaces exported by an assembly,
    /// get all types that implement a specific set of interfaces (not necessarily from that assembly).
    /// </summary>
    public class ExportedTypeView
    {
        public Assembly Assembly { get; private set; }
        public bool IncludeAbstract { get; set; }
        public bool IncludeGenericTypeDefinitions { get; set; }

        readonly Dictionary<Type, IEnumerable<Type>> InterfaceTypes;
        readonly List<Type> ImplementationTypes;
        readonly List<Type> ExportedTypes;

        public ExportedTypeView(Assembly assembly)
        {
            Assembly = assembly.ThrowIfNull("assembly");

            InterfaceTypes = new Dictionary<Type, IEnumerable<Type>>();
            ImplementationTypes = new List<Type>();

            ExportedTypes = Assembly.GetExportedTypes().ToList();
            SummariseExportedTypes();
        }

        /// <summary>
        /// Returns the set of exported interfaces, with the interfaces and
        /// their implementations filtered according to the object properties.
        /// </summary>
        public IDictionary<Type, IEnumerable<Type>> Interfaces
        {
            get
            {
                var result = new Dictionary<Type, IEnumerable<Type>>();

                // The extra object churn is to help with debugging.
                foreach (var kvp in InterfaceTypes)
                {
                    Type interfaceType = kvp.Key;
                    IEnumerable<Type> implementations = kvp.Value;

                    if (IncludeAbstract)
                        result[interfaceType] = implementations;
                    else
                        result[interfaceType] = implementations.Where(t => !t.IsAbstract);
                }

                return result;
            }
        }

        //public IEnumerable<Type> Implementations { get; }
        //public IEnumerable<Type> ImplementationsOf(Type interfaceType);

        void SummariseExportedTypes()
        {
            foreach (var type in ExportedTypes)
            {
                if (type.IsInterface)
                {
                    // For an interface, find all the types that implement it.
                    //var etv = new ExportedInterface() { InterfaceType = type };
                    var potentialImplementers = ExportedTypes.Where(t => t != type);
                    var implementers = new List<Type>();
                    foreach (var pimpl in potentialImplementers)
                    {
                        var ifaces = pimpl.GetInterfaces();
                        if (ifaces == null || ifaces.Count() == 0)
                            continue;

                        if (ifaces.Contains(type))
                            implementers.Add(pimpl);
                    }

                    InterfaceTypes[type] = implementers;
                }
                else
                {
                    // For a non-interface, find all the interfaces that it implements.
                    ImplementationTypes.Add(type);
                }
            }
        }
    }



        //public static IEnumerable<Tuple<Type, Type>> GetInterfaceImplementers(this Assembly assembly, IEnumerable<Type> interfaceTypes)
        //{
        //    assembly.ThrowIfNull("assembly");
        //    interfaceTypes.ThrowIfNull("interfaceTypes");

        //    return from type in assembly.GetExportedTypes()
        //           where !type.IsInterface &&
        //                 !type.IsAbstract
        //           let ifaces = type.GetInterfaces().Intersect(interfaceTypes)
        //           where ifaces.Count() == 1
        //           orderby type.FullName
        //           select Tuple.Create<Type, Type>(ifaces.ElementAt(0), type);
        //}
    */
}
