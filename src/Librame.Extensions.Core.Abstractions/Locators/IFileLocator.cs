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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 文件定位器接口。
    /// </summary>
    public interface IFileLocator : ILocator<string>, IEquatable<IFileLocator>
    {
        /// <summary>
        /// 文件名。
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// 基础路径。
        /// </summary>
        string BasePath { get; }
    }
}