﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Librame.Extensions.Core.Localizers
{
    /// <summary>
    /// 字典字符串定位器。
    /// </summary>
    /// <typeparam name="TResource">指定的资源类型。</typeparam>
    public class DictionaryStringLocalizer<TResource> : IDictionaryStringLocalizer<TResource>
    {
        private readonly IDictionaryStringLocalizerFactory _factory;
        private IStringLocalizer _localizer;


        /// <summary>
        /// 构造一个字典字符串定位器。
        /// </summary>
        /// <param name="factory">给定的 <see cref="IDictionaryStringLocalizerFactory"/>。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public DictionaryStringLocalizer(IDictionaryStringLocalizerFactory factory)
        {
            _factory = factory.NotNull(nameof(factory));
            _localizer = factory.Create(typeof(TResource));
        }


        /// <summary>
        /// 获取指定名称的本地化字符串。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <returns>返回 <see cref="LocalizedString"/>。</returns>
        public LocalizedString this[string name]
            => _localizer[name];

        /// <summary>
        /// 获取指定名称的本地化字符串。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <param name="arguments">给定的参数数组。</param>
        /// <returns>返回 <see cref="LocalizedString"/>。</returns>
        public LocalizedString this[string name, params object[] arguments]
            => _localizer[name, arguments];


        /// <summary>
        /// 获取所有本地化的字符串。
        /// </summary>
        /// <param name="includeParentCultures">参数无效。</param>
        /// <returns>返回 <see cref="IEnumerable{LocalizedString}"/>。</returns>
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
            => _localizer.GetAllStrings(includeParentCultures);


        /// <inheritdoc />
        [Obsolete("This method is obsolete. Use `CurrentCulture` and `CurrentUICulture` instead.")]
        public IStringLocalizer WithCulture(CultureInfo culture)
            => _localizer.WithCulture(culture);


        /// <summary>
        /// 带有资源。
        /// </summary>
        /// <typeparam name="TNewResource">指定的新资源类型。</typeparam>
        /// <returns>返回 <see cref="DictionaryStringLocalizer{TNewResource}"/>。</returns>
        public IDictionaryStringLocalizer<TNewResource> WithResource<TNewResource>()
            => new DictionaryStringLocalizer<TNewResource>(_factory);
    }
}
