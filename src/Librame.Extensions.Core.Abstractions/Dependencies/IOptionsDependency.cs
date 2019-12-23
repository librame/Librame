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

namespace Librame.Extensions.Core.Dependencies
{
    using Serializers;

    /// <summary>
    /// 选项依赖接口。
    /// </summary>
    public interface IOptionsDependency : IDependency
    {
        /// <summary>
        /// 选项类型。
        /// </summary>
        SerializableObject<Type> OptionsType { get; }

        /// <summary>
        /// 自动配置选项。
        /// </summary>
        bool AutoConfigureOptions { get; set; }

        /// <summary>
        /// 自动后置配置选项。
        /// </summary>
        bool AutoPostConfigureOptions { get; set; }
    }
}