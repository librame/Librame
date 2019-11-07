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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions
{
    using Resources;

    /// <summary>
    /// 默认值静态扩展。
    /// </summary>
    public static class DefaultValueExtensions
    {
        /// <summary>
        /// 得到不为 NULL 或默认值。
        /// </summary>
        /// <typeparam name="TStruct">指定的值类型。</typeparam>
        /// <param name="nullable">给定的当前可空值。</param>
        /// <param name="default">给定的默认值（如果可空值为空，则返回此值）。</param>
        /// <returns>返回存在值或默认值。</returns>
        public static TStruct NotNullOrDefault<TStruct>(this TStruct? nullable, TStruct @default)
            where TStruct : struct
            => nullable.NotNullOrDefault(() => @default);

        /// <summary>
        /// 得到不为 NULL 或默认值。
        /// </summary>
        /// <typeparam name="TStruct">指定的值类型。</typeparam>
        /// <param name="nullable">给定的当前可空值。</param>
        /// <param name="defaultFactory">给定的默认值工厂方法（如果可空值为空，则调用此方法）。</param>
        /// <returns>返回当前或默认值。</returns>
        public static TStruct NotNullOrDefault<TStruct>(this TStruct? nullable, Func<TStruct> defaultFactory)
            where TStruct : struct
            => nullable.HasValue ? nullable.Value : (TStruct)defaultFactory?.Invoke();


        /// <summary>
        /// 得到不为 NULL、空格或默认字符串。
        /// </summary>
        /// <param name="str">给定的当前字符串。</param>
        /// <param name="default">在未通过验证或验证异常时，要返回的默认字符串。</param>
        /// <param name="throwIfDefaultInvalid">如果默认字符串不满足必要条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认字符串。</returns>
        public static string NotWhiteSpaceOrDefault(this string str, string @default, bool throwIfDefaultInvalid = true)
            => str.ValidateOrDefault(@default, ValidationExtensions.IsNotWhiteSpace, throwIfDefaultInvalid);

        /// <summary>
        /// 得到不为 NULL、空格或默认字符串。
        /// </summary>
        /// <param name="str">给定的当前字符串。</param>
        /// <param name="defaultFactory">在未通过验证或验证异常时，要返回的默认结果工厂方法。</param>
        /// <param name="throwIfDefaultInvalid">如果默认字符串不满足必要条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认字符串。</returns>
        public static string NotWhiteSpaceOrDefault(this string str, Func<string> defaultFactory, bool throwIfDefaultInvalid = true)
            => str.ValidateOrDefault(defaultFactory, ValidationExtensions.IsNotWhiteSpace, throwIfDefaultInvalid);


        /// <summary>
        /// 得到不为 NULL、空或默认字符串。
        /// </summary>
        /// <param name="str">给定的当前字符串。</param>
        /// <param name="default">在未通过验证或验证异常时，要返回的默认字符串。</param>
        /// <param name="throwIfDefaultInvalid">如果默认字符串不满足必要条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认字符串。</returns>
        public static string NotEmptyOrDefault(this string str, string @default, bool throwIfDefaultInvalid = true)
            => str.ValidateOrDefault(@default, ValidationExtensions.IsNotEmpty, throwIfDefaultInvalid);

        /// <summary>
        /// 得到不为 NULL、空或默认字符串。
        /// </summary>
        /// <param name="str">给定的当前字符串。</param>
        /// <param name="defaultFactory">在未通过验证或验证异常时，要返回的默认字符串工厂方法。</param>
        /// <param name="throwIfDefaultInvalid">如果默认字符串不满足必要条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认字符串。</returns>
        public static string NotEmptyOrDefault(this string str, Func<string> defaultFactory, bool throwIfDefaultInvalid = true)
            => str.ValidateOrDefault(defaultFactory, ValidationExtensions.IsNotEmpty, throwIfDefaultInvalid);


        /// <summary>
        /// 得到不为 NULL、空或默认集合实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前集合实例。</param>
        /// <param name="default">在未通过验证或验证异常时，要返回的默认结果。</param>
        /// <param name="throwIfDefaultInvalid">如果默认集合实例不满足必要条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认集合实例。</returns>
        public static IEnumerable<T> NotEmptyOrDefault<T>(this IEnumerable<T> current, IEnumerable<T> @default, bool throwIfDefaultInvalid = true)
            => current.ValidateOrDefault(@default, ValidationExtensions.IsNotEmpty, throwIfDefaultInvalid);

        /// <summary>
        /// 得到不为 NULL、空或默认集合实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前集合实例。</param>
        /// <param name="defaultFactory">在未通过验证或验证异常时，要返回的默认结果工厂方法。</param>
        /// <param name="throwIfDefaultInvalid">如果默认集合实例不满足必要条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认集合实例。</returns>
        public static IEnumerable<T> NotEmptyOrDefault<T>(this IEnumerable<T> current, Func<IEnumerable<T>> defaultFactory, bool throwIfDefaultInvalid = true)
            => current.ValidateOrDefault(defaultFactory, ValidationExtensions.IsNotEmpty, throwIfDefaultInvalid);


        /// <summary>
        /// 得到不为 NULL、空或默认集合实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前集合实例。</param>
        /// <param name="default">在未通过验证或验证异常时，要返回的默认集合实例。</param>
        /// <param name="throwIfDefaultInvalid">如果默认集合实例不满足必要条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认集合实例。</returns>
        public static T NotEmptyOrDefault<T>(this T current, T @default, bool throwIfDefaultInvalid = true)
            where T : IEnumerable
            => current.ValidateOrDefault(@default, ValidationExtensions.IsNotEmpty, throwIfDefaultInvalid);

        /// <summary>
        /// 得到不为 NULL、空或默认集合实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前集合实例。</param>
        /// <param name="defaultFactory">在未通过验证或验证异常时，要返回的默认集合实例工厂方法。</param>
        /// <param name="throwIfDefaultInvalid">如果默认集合实例不满足必要条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认集合实例。</returns>
        public static T NotEmptyOrDefault<T>(this T current, Func<T> defaultFactory, bool throwIfDefaultInvalid = true)
            where T : IEnumerable
            => current.ValidateOrDefault(defaultFactory, ValidationExtensions.IsNotEmpty, throwIfDefaultInvalid);


        /// <summary>
        /// 得到不为 NULL 或默认实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前实例。</param>
        /// <param name="default">在未通过验证或验证异常时，要返回的默认实例。</param>
        /// <param name="throwIfDefaultInvalid">如果默认实例不满足必要条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认实例。</returns>
        public static T NotNullOrDefault<T>(this T current, T @default, bool throwIfDefaultInvalid = true)
            => current.ValidateOrDefault(@default, ValidationExtensions.IsNotNull, throwIfDefaultInvalid);

        /// <summary>
        /// 得到不为 NULL 或默认实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前实例。</param>
        /// <param name="defaultFactory">在未通过验证或验证异常时，要返回的默认实例工厂方法。</param>
        /// <param name="throwIfDefaultInvalid">如果默认实例不满足验证条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认实例。</returns>
        public static T NotNullOrDefault<T>(this T current, Func<T> defaultFactory, bool throwIfDefaultInvalid = true)
            => current.ValidateOrDefault(defaultFactory, ValidationExtensions.IsNotNull, throwIfDefaultInvalid);


        /// <summary>
        /// 得到不大于的当前或默认实例（不验证比较实例也不抛出异常）。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前实例。</param>
        /// <param name="compare">给定的比较实例。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <returns>返回当前或默认实例。</returns>
        public static T NotGreaterOrDefault<T>(this T current, T compare, bool equals = false)
            where T : IComparable<T>
            => current.ValidateOrDefault(compare, _current => _current.IsLesser(compare, equals), throwIfDefaultInvalid: false);

        /// <summary>
        /// 得到不大于的当前或默认实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前结果。</param>
        /// <param name="compare">给定的比较结果。</param>
        /// <param name="default">在未通过验证或验证异常时，要返回的默认结果。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <param name="throwIfDefaultInvalid">如果默认实例不满足验证条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认实例。</returns>
        public static T NotGreaterOrDefault<T>(this T current, T compare, T @default, bool equals = false, bool throwIfDefaultInvalid = true)
            where T : IComparable<T>
            => current.ValidateOrDefault(@default, _current => _current.IsLesser(compare, equals), throwIfDefaultInvalid);

        /// <summary>
        /// 得到不大于的当前或默认实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前实例。</param>
        /// <param name="compare">给定的比较实例。</param>
        /// <param name="defaultFactory">在未通过验证或验证异常时，要返回的默认实例工厂方法。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <param name="throwIfDefaultInvalid">如果默认实例不满足验证条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认实例。</returns>
        public static T NotGreaterOrDefault<T>(this T current, T compare, Func<T> defaultFactory, bool equals = false, bool throwIfDefaultInvalid = true)
            where T : IComparable<T>
            => current.ValidateOrDefault(defaultFactory, _current => _current.IsLesser(compare, equals), throwIfDefaultInvalid);


        /// <summary>
        /// 得到不小于的当前或默认实例（不验证比较实例也不抛出异常）。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前实例。</param>
        /// <param name="compare">给定的比较实例。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <returns>返回当前或默认实例。</returns>
        public static T NotLesserOrDefault<T>(this T current, T compare, bool equals = false)
            where T : IComparable<T>
            => current.ValidateOrDefault(compare, _current => _current.IsGreater(compare, equals), throwIfDefaultInvalid: false);

        /// <summary>
        /// 得到不小于的当前或默认实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前实例。</param>
        /// <param name="compare">给定的比较实例。</param>
        /// <param name="default">在未通过验证或验证异常时，要返回的默认实例。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <param name="throwIfDefaultInvalid">如果默认实例不满足验证条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认实例。</returns>
        public static T NotLesserOrDefault<T>(this T current, T compare, T @default, bool equals = false, bool throwIfDefaultInvalid = true)
            where T : IComparable<T>
            => current.ValidateOrDefault(@default, _current => _current.IsGreater(compare, equals), throwIfDefaultInvalid);

        /// <summary>
        /// 得到不小于的当前或默认实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前实例。</param>
        /// <param name="compare">给定的比较实例。</param>
        /// <param name="defaultFactory">在未通过验证或验证异常时，要返回的默认实例工厂方法。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <param name="throwIfDefaultInvalid">如果默认实例不满足验证条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认实例。</returns>
        public static T NotLesserOrDefault<T>(this T current, T compare, Func<T> defaultFactory, bool equals = false, bool throwIfDefaultInvalid = true)
            where T : IComparable<T>
            => current.ValidateOrDefault(defaultFactory, _current => _current.IsGreater(compare, equals), throwIfDefaultInvalid);


        /// <summary>
        /// 得到不超出范围的当前或默认实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前实例。</param>
        /// <param name="compareMinimum">给定的最小比较值。</param>
        /// <param name="compareMaximum">给定的最大比较值。</param>
        /// <param name="default">在未通过验证或验证异常时，要返回的默认实例。</param>
        /// <param name="equalMinimum">是否比较等于最小值（可选；默认不比较）。</param>
        /// <param name="equalMaximum">是否比较等于最大值（可选；默认不比较）。</param>
        /// <param name="throwIfDefaultInvalid">如果默认实例不满足验证条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认实例。</returns>
        public static T NotOutOfRangeOrDefault<T>(this T current, T compareMinimum, T compareMaximum,
            T @default, bool equalMinimum = false, bool equalMaximum = false, bool throwIfDefaultInvalid = true)
            where T : IComparable<T>
            => current.ValidateOrDefault(@default, _current => _current.IsNotOutOfRange(compareMinimum, compareMaximum, equalMinimum, equalMaximum), throwIfDefaultInvalid);

        /// <summary>
        /// 得到不超出范围的当前或默认实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前实例。</param>
        /// <param name="compareMinimum">给定的最小比较值。</param>
        /// <param name="compareMaximum">给定的最大比较值。</param>
        /// <param name="defaultFactory">在未通过验证或验证异常时，要返回的默认实例工厂方法。</param>
        /// <param name="equalMinimum">是否比较等于最小值（可选；默认不比较）。</param>
        /// <param name="equalMaximum">是否比较等于最大值（可选；默认不比较）。</param>
        /// <param name="throwIfDefaultInvalid">如果默认实例不满足验证条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认实例。</returns>
        public static T NotOutOfRangeOrDefault<T>(this T current, T compareMinimum, T compareMaximum,
            Func<T> defaultFactory, bool equalMinimum = false, bool equalMaximum = false, bool throwIfDefaultInvalid = true)
            where T : IComparable<T>
            => current.ValidateOrDefault(defaultFactory, _current => _current.IsNotOutOfRange(compareMinimum, compareMaximum, equalMinimum, equalMaximum), throwIfDefaultInvalid);


        /// <summary>
        /// 验证并得到当前或默认实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前实例。</param>
        /// <param name="default">在未通过验证或验证异常时，要返回的默认实例。</param>
        /// <param name="validFactory">验证当前（或默认）实例是否有效的工厂方法。</param>
        /// <param name="throwIfDefaultInvalid">如果默认实例不满足验证条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认实例。</returns>
        public static T ValidateOrDefault<T>(this T current, T @default,
            Func<T, bool> validFactory, bool throwIfDefaultInvalid = true)
            => current.ValidateOrDefault(() => @default, validFactory, throwIfDefaultInvalid);

        /// <summary>
        /// 验证并得到当前或默认实例。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// The default value did not pass the specified validation condition.
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前实例。</param>
        /// <param name="defaultFactory">在未通过验证或验证异常时，要返回的默认实例工厂方法。</param>
        /// <param name="validFactory">验证当前（或默认）实例是否有效的工厂方法。</param>
        /// <param name="throwIfDefaultInvalid">如果默认实例不满足验证条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认实例。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static T ValidateOrDefault<T>(this T current, Func<T> defaultFactory,
            Func<T, bool> validFactory, bool throwIfDefaultInvalid = true)
            => current.ValidateOrDefault<T, Exception>(defaultFactory, validFactory, throwIfDefaultInvalid);

        /// <summary>
        /// 验证并得到当前或默认实例。
        /// </summary>
        /// <typeparam name="TException">指定要捕获的异常类型。</typeparam>
        /// <exception cref="ArgumentException">
        /// The default value did not pass the specified validation condition.
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前实例。</param>
        /// <param name="defaultFactory">在未通过验证或验证异常时，要返回的默认实例工厂方法。</param>
        /// <param name="validationFactory">验证当前（或默认）实例是否有效的工厂方法。</param>
        /// <param name="throwIfDefaultInvalid">如果默认实例不满足验证条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认实例。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static T ValidateOrDefault<T, TException>(this T current, Func<T> defaultFactory,
            Func<T, bool> validationFactory, bool throwIfDefaultInvalid = true)
            where TException : Exception
        {
            defaultFactory.NotNull(nameof(defaultFactory));
            validationFactory.NotNull(nameof(validationFactory));

            T @default;

            try
            {
                if (validationFactory.Invoke(current))
                    return current;

                @default = defaultFactory.Invoke();
            }
            catch (TException)
            {
                @default = defaultFactory.Invoke();
            }

            if (throwIfDefaultInvalid && !validationFactory.Invoke(@default))
                throw new ArgumentException(InternalResource.ArgumentExceptionDefaultValue);

            return @default;
        }

    }
}