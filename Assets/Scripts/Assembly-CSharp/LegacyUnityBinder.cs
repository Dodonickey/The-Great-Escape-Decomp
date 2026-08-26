using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using UnityEngine;

public sealed class LegacyUnityBinder : SerializationBinder
{
    private static readonly Dictionary<string, Type> TypeCache = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
    private static readonly Regex CleanRegex = new Regex(@",\s*(Version|Culture|PublicKeyToken)=[^,\]]+", RegexOptions.Compiled);
    private static bool _isCacheInitialized = false;

    static LegacyUnityBinder()
    {
        InitializeCache();
    }

    private static void InitializeCache()
    {
        if (_isCacheInitialized) return;

        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        for (int i = 0; i < assemblies.Length; i++)
        {
            Type[] types = null;
            try
            {
                types = assemblies[i].GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types;
            }
            catch
            {
                continue;
            }

            if (types == null) continue;

            for (int j = 0; j < types.Length; j++)
            {
                Type t = types[j];
                if (t == null) continue;

                // Cache by FullName (e.g. Namespace.TypeName)
                if (!string.IsNullOrEmpty(t.FullName) && !TypeCache.ContainsKey(t.FullName))
                {
                    TypeCache[t.FullName] = t;
                }
                // Cache by short Name (e.g. TypeName) to handle namespace migrations
                if (!string.IsNullOrEmpty(t.Name) && !TypeCache.ContainsKey(t.Name))
                {
                    TypeCache[t.Name] = t;
                }
            }
        }
        _isCacheInitialized = true;
    }

    public override Type BindToType(string assemblyName, string typeName)
    {
        if (string.IsNullOrEmpty(typeName)) return null;

        string cleanTypeName = CleanRegex.Replace(typeName, string.Empty).Trim();
        string cacheKey = assemblyName + "::" + cleanTypeName;

        Type resolved;
        if (TypeCache.TryGetValue(cacheKey, out resolved))
        {
            return resolved;
        }

        resolved = ResolveTypeRecursive(cleanTypeName);

        if (resolved != null)
        {
            TypeCache[cacheKey] = resolved;
        }
        else
        {
            // This will print the EXACT class type that failed to load in your levels
            Debug.LogError(string.Format("<color=red><b>[LegacyUnityBinder] Missing Type:</b></color> Could not find class <b>'{0}'</b> anywhere in the project. (Original Assembly: '{1}')", cleanTypeName, assemblyName));
        }

        return resolved;
    }

    private Type ResolveTypeRecursive(string typeName)
    {
        // 1. Handle standard arrays (e.g., "EIC[]")
        if (typeName.EndsWith("[]"))
        {
            string elementTypeName = typeName.Substring(0, typeName.Length - 2);
            Type elementType = ResolveTypeRecursive(elementTypeName);
            return elementType != null ? elementType.MakeArrayType() : null;
        }

        // 2. Handle 2D arrays (e.g., "EIC[,]")
        if (typeName.EndsWith("[,]"))
        {
            string elementTypeName = typeName.Substring(0, typeName.Length - 3);
            Type elementType = ResolveTypeRecursive(elementTypeName);
            return elementType != null ? elementType.MakeArrayType(2) : null;
        }

        // 3. Handle generics (e.g., "List`1[[EIC, Assembly-CSharp]]")
        if (typeName.Contains("[["))
        {
            Type genericType = ResolveGenericType(typeName);
            if (genericType != null) return genericType;
        }

        // 4. Try Direct Cache Lookups
        Type directType;
        if (TypeCache.TryGetValue(typeName, out directType))
        {
            return directType;
        }

        // 5. Try resolving by stripping the namespace (Fuzzy match)
        int lastDot = typeName.LastIndexOf('.');
        if (lastDot >= 0)
        {
            string simpleName = typeName.Substring(lastDot + 1);
            if (TypeCache.TryGetValue(simpleName, out directType))
            {
                return directType;
            }
        }

        // 6. Direct system lookup
        Type systemType = Type.GetType(typeName, false, true);
        if (systemType != null) return systemType;

        // 7. Refresh cache if assemblies were loaded dynamically
        _isCacheInitialized = false;
        InitializeCache();
        if (TypeCache.TryGetValue(typeName, out directType))
        {
            return directType;
        }

        return null;
    }

    private Type ResolveGenericType(string typeName)
    {
        try
        {
            int firstBracket = typeName.IndexOf("[[", StringComparison.Ordinal);
            if (firstBracket < 0) return null;

            string genericDefName = typeName.Substring(0, firstBracket);
            Type genericDefType = ResolveTypeRecursive(genericDefName);
            if (genericDefType == null) return null;

            int lastBracket = typeName.LastIndexOf("]]", StringComparison.Ordinal);
            if (lastBracket < 0) return null;

            string argsContent = typeName.Substring(firstBracket + 2, lastBracket - firstBracket - 2);
            string[] rawArgs = argsContent.Split(new string[] { "],[" }, StringSplitOptions.None);

            Type[] typeArgs = new Type[rawArgs.Length];
            for (int i = 0; i < rawArgs.Length; i++)
            {
                string argTypeName = rawArgs[i].Trim('[', ']');
                int commaIndex = argTypeName.IndexOf(',');
                if (commaIndex > 0)
                {
                    argTypeName = argTypeName.Substring(0, commaIndex);
                }

                typeArgs[i] = ResolveTypeRecursive(argTypeName.Trim());
                if (typeArgs[i] == null) return null;
            }

            return genericDefType.MakeGenericType(typeArgs);
        }
        catch
        {
            return null;
        }
    }

    public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
    {
        typeName = serializedType.FullName;

        string asmName = serializedType.Assembly.GetName().Name;
        if (asmName == "Assembly-CSharp")
        {
            assemblyName = "Assembly-CSharp";
        }
        else if (asmName == "mscorlib")
        {
            assemblyName = "mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
        }
        else if (asmName.StartsWith("UnityEngine"))
        {
            assemblyName = "UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
        }
        else
        {
            assemblyName = asmName;
        }
    }
}