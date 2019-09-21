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
using System.Linq;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象文件名组合器静态扩展。
    /// </summary>
    public static class AbstractionFileNameCombinerExtensions
    {
        /// <summary>
        /// 转换为文件名组合器。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <returns>返回 <see cref="FileNameCombiner"/>。</returns>
        public static FileNameCombiner AsFileNameCombiner(this string fileName)
        {
            return (FileNameCombiner)fileName;
        }

        /// <summary>
        /// 转换为文件名组合器。
        /// </summary>
        /// <param name="baseName">给定的基础名。</param>
        /// <param name="extension">给定的扩展名。</param>
        /// <returns>返回 <see cref="FileNameCombiner"/>。</returns>
        public static FileNameCombiner AsFileNameCombiner(this string baseName, string extension)
        {
            return new FileNameCombiner(baseName, extension);
        }

        /// <summary>
        /// 将文件名数组转换为文件名组合器数组。
        /// </summary>
        /// <param name="fileNames">给定的文件名数组。</param>
        /// <returns>返回 <see cref="FileNameCombiner"/> 数组。</returns>
        public static FileNameCombiner[] AsFileNameCombiners(this string[] fileNames)
        {
            var combiners = new FileNameCombiner[fileNames.Length];

            for (var i = 0; i < fileNames.Length; i++)
                combiners[i] = (FileNameCombiner)fileNames[i];

            return combiners;
        }


        /// <summary>
        /// 当作数组。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FileNameCombiner"/>。</param>
        /// <returns>返回 <see cref="FileNameCombiner"/> 集合。</returns>
        public static FileNameCombiner[] AsArray(this FileNameCombiner combiner)
        {
            combiner.NotNull(nameof(combiner));
            return new FileNameCombiner[] { combiner };
        }

        /// <summary>
        /// 转换为字符串集合。
        /// </summary>
        /// <param name="combiners">给定的 <see cref="FileNameCombiner"/> 集合。</param>
        /// <returns>返回字符串集合。</returns>
        public static IEnumerable<string> ToStrings(this IEnumerable<FileNameCombiner> combiners)
        {
            combiners.NotNullOrEmpty(nameof(combiners));
            return combiners.Select(combiner => combiner?.ToString());
        }


        /// <summary>
        /// 改变基础名。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FileNameCombiner"/>。</param>
        /// <param name="newBaseNameFactory">给定的新基础名工厂方法（输入参数为当前基础名）。</param>
        /// <returns>返回 <see cref="FileNameCombiner"/>。</returns>
        public static FileNameCombiner ChangeBaseName(this FileNameCombiner combiner, Func<string, string> newBaseNameFactory)
            => combiner.NotNull(nameof(combiner))
            .ChangeBaseName(newBaseNameFactory?.Invoke(combiner.BaseName));

        /// <summary>
        /// 改变扩展名。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FileNameCombiner"/>。</param>
        /// <param name="newExtensionFactory">给定的新扩展名工厂方法（输入参数为当前扩展名）。</param>
        /// <returns>返回 <see cref="FileNameCombiner"/>。</returns>
        public static FileNameCombiner ChangeExtension(this FileNameCombiner combiner, Func<string, string> newExtensionFactory)
            => combiner.NotNull(nameof(combiner))
            .ChangeExtension(newExtensionFactory?.Invoke(combiner.Extension));

        /// <summary>
        /// 批量改变扩展名。
        /// </summary>
        /// <param name="combiners">给定的 <see cref="FileNameCombiner"/> 集合。</param>
        /// <param name="newExtension">给定的新扩展名。</param>
        /// <returns>返回 <see cref="FileNameCombiner"/> 集合。</returns>
        public static IEnumerable<FileNameCombiner> ChangeExtension(this IEnumerable<FileNameCombiner> combiners, string newExtension)
        {
            combiners.NotNull(nameof(combiners));

            foreach (var combiner in combiners)
                combiner?.ChangeExtension(newExtension);

            return combiners;
        }


        /// <summary>
        /// 依据当前文件组合器的扩展名与指定的基础名，新建一个 <see cref="FileNameCombiner"/>。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FileNameCombiner"/>。</param>
        /// <param name="newBaseNameFactory">给定的新基础名工厂方法（输入参数为当前基础名）。</param>
        /// <returns>返回 <see cref="FileNameCombiner"/>。</returns>
        public static FileNameCombiner NewBaseName(this FileNameCombiner combiner, Func<string, string> newBaseNameFactory)
            => combiner.NotNull(nameof(combiner))
            .NewBaseName(newBaseNameFactory?.Invoke(combiner.BaseName));

        /// <summary>
        /// 依据当前文件组合器的基础名与指定的扩展名，新建一个 <see cref="FileNameCombiner"/>。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FileNameCombiner"/>。</param>
        /// <param name="newExtensionFactory">给定的新扩展名工厂方法（输入参数为当前扩展名）。</param>
        /// <returns>返回 <see cref="FileNameCombiner"/>。</returns>
        public static FileNameCombiner NewExtension(this FileNameCombiner combiner, Func<string, string> newExtensionFactory)
            => combiner.NotNull(nameof(combiner))
            .NewExtension(newExtensionFactory?.Invoke(combiner.Extension));

    }
}