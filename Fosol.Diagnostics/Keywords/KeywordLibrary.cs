using Fosol.Common.Extensions.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    /// <summary>
    /// Contains a dictionary of currently configured 
    /// </summary>
    static class KeywordLibrary
    {
        #region Variables
        private static readonly Dictionary<string, Type> _Cache = new Dictionary<string, Type>();
        #endregion

        #region Properties
        /// <summary>
        /// get - An array of key names within the library.
        /// </summary>
        public static string[] Keys
        {
            get
            {
                return _Cache.Keys.ToArray();
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the library with 
        /// The Logger cannot use a keyword unless it's been registered.
        /// </summary>
        static KeywordLibrary()
        {
            foreach (var type in GetKeywordTypes(Assembly.GetCallingAssembly(), typeof(ITraceKeyword).Namespace))
            {
                Add(type);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add the TraceKeywordBase Type to the library.
        /// </summary>
        /// <param name="keywordType">TraceKeywordBase Type.</param>
        /// <returns>Number of items in library.</returns>
        public static int Add(Type keywordType)
        {
            Common.Validation.Assert.IsNotNull(keywordType, "keywordType");
            Common.Validation.Assert.IsAssignableFromType(keywordType, typeof(TraceKeywordBase), "keywordType");
            Common.Validation.Assert.HasAttribute(keywordType, typeof(TraceKeywordAttribute), "keywordType");

            var attr = keywordType.GetCustomAttribute(typeof(TraceKeywordAttribute)) as TraceKeywordAttribute;
            if (_Cache.ContainsKey(attr.Name))
                throw new InvalidOperationException(string.Format(Resources.Strings.Exception_KeyAlreadyExists, attr.Name));

            _Cache.Add(attr.Name, keywordType);
            return _Cache.Count;
        }

        /// <summary>
        /// Add all the TraceKeywordBase Types within the specified Assembly and Namespace.
        /// </summary>
        /// <param name="assemblyString">Fully qualified name of the assembly.</param>
        /// <param name="nameOrNamespace">Namespace or fully qualified name to the TraceKeywordBase(s).</param>
        /// <returns>Number of items in library.</returns>
        public static int Add(string assemblyString, string nameOrNamespace)
        {
            Common.Validation.Assert.IsNotNullOrEmpty(assemblyString, "assemblyString");
            Common.Validation.Assert.IsNotNullOrEmpty(nameOrNamespace, "nameOrNamespace");

            var assembly = Assembly.Load(assemblyString);
            if (assembly == null)
                throw new InvalidOperationException(string.Format(Resources.Strings.Exception_AssemblyIsInvalid, assemblyString));

            // Fetch every TraceKeywordBase in the specified namespacePath.
            foreach (var type in GetKeywordTypes(assembly, nameOrNamespace))
            {
                Add(type);
            }

            return _Cache.Count;
        }

        /// <summary>
        /// Fetch all the TraceKeywordBase Type objects in the specified Assembly and Namespace.
        /// </summary>
        /// <param name="assembly">Assembly containing TraceKeywordBase objects.</param>
        /// <param name="nameOrNamespace">Namespace or fully qualified name to the TraceKeywordBase(s).</param>
        /// <returns>Collection of TraceKeywordBase Types.</returns>
        static IEnumerable<Type> GetKeywordTypes(Assembly assembly, string nameOrNamespace)
        {
            var type = GetKeywordType(assembly, nameOrNamespace);
            if (type != null)
                return new List<Type>() { type };

            // The keywordNamespace is only a path to possibly numerous TraceKeywordBase objects.
            return (
                from t in assembly.GetTypes()
                where String.Equals(t.Namespace, nameOrNamespace, StringComparison.Ordinal)
                    && typeof(ITraceKeyword).IsAssignableFrom(t)
                    && t.HasAttribute(typeof(TraceKeywordAttribute))
                select t);
        }

        /// <summary>
        /// Checks to see if the fullyQualifiedTypeName is of Type TraceKeywordBase.
        /// </summary>
        /// <param name="assembly">Assembly containing TraceKeywordBase objects.</param>
        /// <param name="fullyQualifiedTypeName">Fully qualified name of the TraceKeywordBase.</param>
        /// <returns>TraceKeywordBase Type, or null if the fullyQualifiedTypeName was only a namespace.</returns>
        static Type GetKeywordType(Assembly assembly, string fullyQualifiedTypeName)
        {
            var type = assembly.GetType(fullyQualifiedTypeName, false);

            // The keywordNamespace pointed directly to a Type.
            // The keywordNamespace is a valid type.
            // And it has been marked with the TraceKeywordAttribute.
            if (type != null)
            {
                if (typeof(ITraceKeyword).IsAssignableFrom(type)
                    && type.HasAttribute(typeof(TraceKeywordAttribute)))
                    return type;

                throw new InvalidOperationException(string.Format(Resources.Strings.Exception_ConfigurationKeywordIsNotValid, fullyQualifiedTypeName));
            }

            return null;
        }

        /// <summary>
        /// Get the Keyword Type for the specified key name.
        /// First it will check the library for an existing TraceKeywordBase.
        /// Then it will check if the executing assembly contains a TraceKeywordBase with the specified name.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "typeName" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "typeName" cannot be null.</exception>
        /// <exception cref="System.InvalidOperation">Type must exist.</exception>
        /// <param name="typeName">Unique name to identify the Target Type.</param>
        /// <returns>Type of Target.</returns>
        public static Type Get(string typeName)
        {
            Common.Validation.Assert.IsNotNullOrEmpty(typeName, "name");
            // The cache contains the LogTarget so return it.
            if (_Cache.ContainsKey(typeName))
                return _Cache[typeName];

            // Check if the name is a fully qualified type name in the executing assembly.
            var type = GetKeywordType(Assembly.GetEntryAssembly(), typeName);
            if (type == null)
                type = GetKeywordType(Assembly.GetCallingAssembly(), typeName);
            if (type == null)
                type = GetKeywordType(Assembly.GetExecutingAssembly(), typeName);
            if (type == null)
                throw new InvalidOperationException(string.Format(Resources.Strings.Exception_KeywordDoesNotExist, typeName));
            return type;
        }

        /// <summary>
        /// Checks to see if the library contains the TraceKeywordBase with the specified name.
        /// </summary>
        /// <param name="name">Name of TraceKeywordBase.</param>
        /// <returns>True if exists.</returns>
        public static bool ContainsKey(string name)
        {
            return _Cache.ContainsKey(name);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
