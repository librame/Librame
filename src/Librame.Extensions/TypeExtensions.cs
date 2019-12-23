﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Librame.Extensions
{
    /// <summary>
    /// 对象静态扩展。
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 序列元素属性值集合相等比较。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="sources">给定的源类型实例集合。</param>
        /// <param name="compares">给定的比较类型实例集合。</param>
        /// <param name="bindingFlags">给定的 <see cref="BindingFlags"/>（可选；默认为公共属性标记）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool SequencePropertyValuesEquals<T>(this IEnumerable<T> sources, IEnumerable<T> compares,
            BindingFlags bindingFlags = BindingFlags.Public)
        {
            sources.NotEmpty(nameof(sources));
            compares.NotEmpty(nameof(compares));

            var sequenceCount = sources.Count();
            if (sequenceCount != compares.Count())
                return false;

            var propertyInfos = typeof(T).GetProperties(bindingFlags);
            for (var i = 0; i < sequenceCount; i++)
            {
                var source = sources.ElementAt(i);
                var compare = compares.ElementAt(i);

                foreach (var info in propertyInfos)
                {
                    if (!info.GetValue(source).Equals(info.GetValue(compare)))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 属性值集合相等比较。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="source">给定的源类型实例。</param>
        /// <param name="compare">给定的比较类型实例。</param>
        /// <param name="bindingFlags">给定的 <see cref="BindingFlags"/>（可选；默认为公共属性标记）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool PropertyValuesEquals<T>(this T source, T compare, BindingFlags bindingFlags = BindingFlags.Public)
            where T : class
        {
            source.NotNull(nameof(source));
            compare.NotNull(nameof(compare));

            var propertyInfos = typeof(T).GetProperties(bindingFlags);
            foreach (var info in propertyInfos)
            {
                if (!info.GetValue(source).Equals(info.GetValue(compare)))
                    return false;
            }

            return true;
        }


        /// <summary>
        /// 获取所有字段集合（包括公开、非公开、实例、静态等）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字段信息数组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "type")]
        public static FieldInfo[] GetAllFields(this Type type)
            => type.NotNull(nameof(type)).GetFields(ExtensionSettings.CommonFlags);

        /// <summary>
        /// 获取所有非静态字段集合（包括公开、非公开、实例等）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字段信息数组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "type")]
        public static FieldInfo[] GetAllFieldsWithoutStatic(this Type type)
            => type.NotNull(nameof(type)).GetFields(ExtensionSettings.CommonFlagsWithoutStatic);


        /// <summary>
        /// 获取所有属性集合（包括公开、非公开、实例、静态等）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字段信息数组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "type")]
        public static PropertyInfo[] GetAllProperties(this Type type)
            => type.NotNull(nameof(type)).GetProperties(ExtensionSettings.CommonFlags);

        /// <summary>
        /// 获取所有非静态属性集合（包括公开、非公开、实例等）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字段信息数组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "type")]
        public static PropertyInfo[] GetAllPropertiesWithoutStatic(this Type type)
            => type.NotNull(nameof(type)).GetProperties(ExtensionSettings.CommonFlagsWithoutStatic);


        /// <summary>
        /// 获取泛型主体名称，普通类型直接返回类型名称（如：泛类型 IDictionary{string, IList{string}} 的主体名称为 IDictionary）。
        /// </summary>
        /// <param name="type">给定的名称。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "type")]
        public static string GetGenericBodyName(this Type type)
        {
            type.NotNull(nameof(type));

            if (type.IsGenericType)
                return type.Name.SplitPair('`').Key;

            return type.Name;
        }


        #region GetDisplayName

        /// <summary>
        /// 获取显示名称（参考 <see cref="AssemblyName.Name"/>）。
        /// </summary>
        /// <param name="assembly">给定的 <see cref="Assembly"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string GetDisplayName(this Assembly assembly)
            => assembly?.GetName()?.Name;

        /// <summary>
        /// 获取程序集显示名称（参考 <see cref="AssemblyName.Name"/>）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字符串。</returns>
        public static string GetAssemblyDisplayName(this Type type)
            => type?.Assembly.GetDisplayName();


        /// <summary>
        /// 获取带命名空间的程序集限定名，不包含版本及版本号（格式：System.Collections.Generic.IList`1[System.String], System.Private.CoreLib）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字符串。</returns>
        public static string GetAssemblyQualifiedNameWithoutVersion(this Type type)
        {
            type.NotNull(nameof(type));
            return Assembly.CreateQualifiedName(type.GetAssemblyDisplayName(), type.GetDisplayNameWithNamespace());
        }


        /// <summary>
        /// 获取显示名称（即 Type.Name。如：泛类型 IDictionary{string, IList{string}} 的名称为 IDictionary`2[String, IList`1[String]]）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字符串。</returns>
        public static string GetDisplayName(this Type type)
            => type.GetDisplayString(t => t.Name);

        /// <summary>
        /// 获取带命名空间的显示名称（如：泛类型 IDictionary{string, IList{string}} 的名称为 System.Collections.Generic.IDictionary`2[System.String, System.Collections.Generic.IList`1[System.String]]）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字符串。</returns>
        public static string GetDisplayNameWithNamespace(this Type type)
        {
            return type.GetDisplayString(t =>
            {
                if (!t.IsNested)
                    return $"{t.Namespace}.{t.Name}";

                // 支持嵌套类型：t.ReflectedType == t.DeclaringType
                return $"{t.Namespace}.{t.ReflectedType.GetDisplayName()}+{t.Name}";
            });
        }

        /// <summary>
        /// 获取自定义字符串。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <param name="factory">给定的类型字符串工厂方法。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "factory")]
        public static string GetDisplayString(this Type type, Func<Type, string> factory)
        {
            type.NotNull(nameof(type));

            var sb = new StringBuilder();
            sb.Append(factory.Invoke(type));

            if (type.IsGenericType)
            {
                var argumentTypes = type.GetGenericArguments();
                sb.Append("[");

                argumentTypes.ForEach((argType, i) =>
                {
                    sb.Append(argType.GetDisplayString(factory));

                    if (i < argumentTypes.Length - 1)
                        sb.Append(", ");
                });

                sb.Append("]");
            }

            return sb.ToString();
        }

        #endregion


        /// <summary>
        /// 解开可空类型。
        /// </summary>
        /// <param name="nullableType">给定的可空类型。</param>
        /// <returns>返回基础类型或可空类型本身。</returns>
        public static Type UnwrapNullableType(this Type nullableType)
            => Nullable.GetUnderlyingType(nullableType) ?? nullableType;


        /// <summary>
        /// 调用类型集合。
        /// </summary>
        /// <param name="assembly">给定的 <see cref="Assembly"/>。</param>
        /// <param name="action">给定的注册动作。</param>
        /// <param name="filterTypes">给定的类型过滤工厂方法（可选）。</param>
        /// <returns>返回已调用的类型集合数。</returns>
        public static int InvokeTypes(this Assembly assembly,
            Action<Type> action, Func<IEnumerable<Type>, IEnumerable<Type>> filterTypes = null)
        {
            assembly.NotNull(nameof(assembly));
            return assembly.YieldEnumerable().InvokeTypes(action, filterTypes);
        }

        /// <summary>
        /// 调用类型集合。
        /// </summary>
        /// <param name="assemblies">给定的 <see cref="IEnumerable{Assembly}"/>。</param>
        /// <param name="action">给定的注册动作。</param>
        /// <param name="filterTypes">给定的类型过滤工厂方法（可选）。</param>
        /// <returns>返回已调用的类型集合数。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "filterTypes")]
        public static int InvokeTypes(this IEnumerable<Assembly> assemblies,
            Action<Type> action, Func<IEnumerable<Type>, IEnumerable<Type>> filterTypes = null)
        {
            assemblies.NotEmpty(nameof(assemblies));
            action.NotNull(nameof(action));

            var allTypes = assemblies.SelectMany(a => a.ExportedTypes);

            if (filterTypes.IsNotNull())
                allTypes = filterTypes.Invoke(allTypes);

            foreach (var type in allTypes)
                action.Invoke(type);

            return allTypes.Count();
        }

    }
}
