﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 扩展构建器依赖选项接口。
    /// </summary>
    public interface IExtensionBuilderDependencyOptions : IOptionsConfigurator
    {
        /// <summary>
        /// 基础目录。
        /// </summary>
        string BaseDirectory { get; set; }
    }
}
