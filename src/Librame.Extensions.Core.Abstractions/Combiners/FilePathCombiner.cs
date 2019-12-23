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
using System.IO;

namespace Librame.Extensions.Core.Combiners
{
    /// <summary>
    /// 文件路径组合器。
    /// </summary>
    public class FilePathCombiner : AbstractCombiner<string>
    {
        /// <summary>
        /// 构造一个 <see cref="FilePathCombiner"/>。
        /// </summary>
        /// <param name="filePath">给定的文件路径。</param>
        public FilePathCombiner(string filePath)
            : base(filePath)
        {
            FileName = CreateFileNameCombiner(filePath);
            BasePath = Path.GetDirectoryName(filePath);
        }

        /// <summary>
        /// 构造一个 <see cref="FilePathCombiner"/>。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <param name="basePath">给定的基础路径（可空）。</param>
        public FilePathCombiner(string fileName, string basePath)
            : base(CombineString(fileName, basePath))
        {
            FileName = CreateFileNameCombiner(fileName);
            BasePath = basePath;
        }

        /// <summary>
        /// 构造一个 <see cref="FilePathCombiner"/>。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <param name="basePath">给定的基础路径（可空）。</param>
        public FilePathCombiner(FileNameCombiner fileName, string basePath)
            : base(CombineString(fileName, basePath))
        {
            FileName = fileName;
            BasePath = basePath;
        }


        /// <summary>
        /// 基础路径。
        /// </summary>
        public string BasePath { get; private set; }

        /// <summary>
        /// 文件名。
        /// </summary>
        public FileNameCombiner FileName { get; private set; }


        /// <summary>
        /// 重写源实例。
        /// </summary>
        public override string Source
            => BasePath.CombinePath(FileName);


        /// <summary>
        /// 如果当前基础路径为空，则改变基础路径。
        /// </summary>
        /// <param name="defaultBasePath">给定的默认基础路径。</param>
        /// <returns>返回当前 <see cref="FilePathCombiner"/>。</returns>
        public FilePathCombiner ChangeBasePathIfEmpty(string defaultBasePath)
        {
            if (BasePath.IsEmpty())
                BasePath = defaultBasePath.NotEmpty(nameof(defaultBasePath));

            return this;
        }

        /// <summary>
        /// 改变基础路径。
        /// </summary>
        /// <param name="newBasePath">给定的新基础路径。</param>
        /// <returns>返回当前 <see cref="FilePathCombiner"/>。</returns>
        public FilePathCombiner ChangeBasePath(string newBasePath)
        {
            BasePath = newBasePath.NotEmpty(nameof(newBasePath));
            return this;
        }

        /// <summary>
        /// 改变文件名。
        /// </summary>
        /// <param name="newFileName">给定的新文件名。</param>
        /// <returns>返回当前 <see cref="FilePathCombiner"/>。</returns>
        public FilePathCombiner ChangeFileName(string newFileName)
        {
            var newFileNameCombiner = CreateFileNameCombiner(newFileName);
            return ChangeFileName(newFileNameCombiner);
        }

        /// <summary>
        /// 改变文件名。
        /// </summary>
        /// <param name="newFileName">给定的新 <see cref="FileNameCombiner"/>。</param>
        /// <returns>返回当前 <see cref="FilePathCombiner"/>。</returns>
        public FilePathCombiner ChangeFileName(FileNameCombiner newFileName)
        {
            FileName = newFileName.NotNull(nameof(newFileName));
            return this;
        }


        /// <summary>
        /// 依据当前文件组合器的文件名与指定的基础路径，新建一个 <see cref="FilePathCombiner"/>。
        /// </summary>
        /// <param name="newBasePath">给定的新基础路径。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        public FilePathCombiner NewBasePath(string newBasePath)
            => new FilePathCombiner(FileName, newBasePath);

        /// <summary>
        /// 依据当前文件组合器的基础路径与指定的文件名，新建一个 <see cref="FilePathCombiner"/>。
        /// </summary>
        /// <param name="newFileName">给定的新文件名。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        public FilePathCombiner NewFileName(string newFileName)
            => new FilePathCombiner(newFileName, BasePath);

        /// <summary>
        /// 依据当前文件组合器的基础路径与指定的文件名，新建一个 <see cref="FilePathCombiner"/>。
        /// </summary>
        /// <param name="newFileName">给定的新 <see cref="FileNameCombiner"/>。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        public FilePathCombiner NewFileName(FileNameCombiner newFileName)
            => new FilePathCombiner(newFileName, BasePath);


        /// <summary>
        /// 创建 <see cref="FileNameCombiner"/>。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <returns>返回 <see cref="FileNameCombiner"/>。</returns>
        private static FileNameCombiner CreateFileNameCombiner(string fileName)
            => new FileNameCombiner(fileName);


        /// <summary>
        /// 是指定的文件名（忽略大小写）。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <returns>返回布尔值。</returns>
        public bool IsFileName(string fileName)
            => IsFileName(CreateFileNameCombiner(fileName));

        /// <summary>
        /// 是指定的文件名（忽略大小写）。
        /// </summary>
        /// <param name="fileName">给定的 <see cref="FileNameCombiner"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool IsFileName(FileNameCombiner fileName)
            => FileName == fileName;


        /// <summary>
        /// 是否相等（忽略大小写）。
        /// </summary>
        /// <param name="other">给定的域名。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(string other)
            => Source.Equals(other, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// 是否相等（忽略大小写）。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => (obj is FilePathCombiner other) ? Equals(other) : false;


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => Source.CompatibleGetHashCode();


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => Source;


        /// <summary>
        /// 是否相等（忽略大小写）。
        /// </summary>
        /// <param name="a">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="b">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(FilePathCombiner a, FilePathCombiner b)
            => (a?.Equals(b)).Value;

        /// <summary>
        /// 是否不等（忽略大小写）。
        /// </summary>
        /// <param name="a">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="b">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(FilePathCombiner a, FilePathCombiner b)
            => !(a?.Equals(b)).Value;


        /// <summary>
        /// 隐式转换为字符串。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        public static implicit operator string(FilePathCombiner combiner)
            => combiner?.ToString();


        /// <summary>
        /// 组合字符串。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <paramref name="fileName"/> is null or empty.
        /// </exception>
        /// <param name="fileName">给定的文件名。</param>
        /// <param name="basePath">给定的基础路径（可选）。</param>
        /// <returns>返回字符串。</returns>
        public static string CombineString(string fileName, string basePath = null)
        {
            fileName.NotEmpty(nameof(fileName));
            return basePath.IsNotEmpty() ? basePath.CombinePath(fileName) : fileName;
        }
    }
}
