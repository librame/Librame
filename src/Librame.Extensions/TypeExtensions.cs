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
using System.Reflection;

namespace Librame.Extensions
{
    /// <summary>
    /// 对象静态扩展。
    /// </summary>
    public static class TypeExtensions
    {
        private static readonly Dictionary<Type, object> _commonTypeDictionary = new Dictionary<Type, object>
        {
            #pragma warning disable IDE0034 // Simplify 'default' expression - default causes default(object)
            { typeof(char), default(char) },
            { typeof(sbyte), default(sbyte) },
            { typeof(short), default(short) },
            { typeof(int), default(int) },
            { typeof(long), default(long) },
            { typeof(byte), default(byte) },
            { typeof(ushort), default(ushort) },
            { typeof(uint), default(uint) },
            { typeof(ulong), default(ulong) },
            { typeof(double), default(double) },
            { typeof(float), default(float) },
            { typeof(bool), default(bool) },
            { typeof(DateTime), default(DateTime) },
            { typeof(DateTimeOffset), default(DateTimeOffset) },
            { typeof(Guid), default(Guid) }
            #pragma warning restore IDE0034 // Simplify 'default' expression
        };

        /// <summary>
        /// 创建实例（引用类型）或默认值（值类型）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回对象。</returns>
        public static object CreateOrDefault(this Type type)
        {
            type.NotNull(nameof(type));

            // A bit of perf code to avoid calling Activator.CreateInstance for common types and
            // to avoid boxing on every call. This is about 50% faster than just calling CreateInstance
            // for all value types.
            return _commonTypeDictionary.TryGetValue(type, out var value)
                ? value : Activator.CreateInstance(type);
        }


        /// <summary>
        /// 解开可空类型。
        /// </summary>
        /// <param name="nullableType">给定的可空类型。</param>
        /// <returns>返回基础类型或可空类型本身。</returns>
        public static Type UnwrapNullableType(this Type nullableType)
        {
            return Nullable.GetUnderlyingType(nullableType) ?? nullableType;
        }


        /// <summary>
        /// 填入属性集合。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为空或默认值。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target"/> 为空或默认值。
        /// </exception>
        /// <param name="source">给定的来源类型实例。</param>
        /// <param name="target">给定的目标类型实例。</param>
        public static void PopulateProperties<TSource, TTarget>(this TSource source, TTarget target)
            where TSource : class
            where TTarget : class
        {
            source.NotNull(nameof(source));
            target.NotNull(nameof(target));

            var srcProperties = new List<PropertyInfo>(typeof(TSource).GetProperties());
            var tarProperties = new List<PropertyInfo>(typeof(TTarget).GetProperties());

            for (var s = 0; s < srcProperties.Count; s++)
            {
                for (var t = 0; t < tarProperties.Count; t++)
                {
                    var srcProperty = srcProperties[s];
                    var tarProperty = tarProperties[t];

                    if (srcProperty.Name == tarProperty.Name)
                    {
                        var value = srcProperty.GetValue(source);
                        tarProperty.SetValue(target, value);

                        tarProperties.Remove(tarProperty);
                        srcProperties.Remove(srcProperty);
                        
                        break;
                    }
                }
            }
        }

    }
}
