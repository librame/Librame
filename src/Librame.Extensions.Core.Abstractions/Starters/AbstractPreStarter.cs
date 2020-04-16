﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;

namespace Librame.Extensions.Core.Starters
{
    /// <summary>
    /// 抽象预启动器。
    /// </summary>
    public abstract class AbstractPreStarter : AbstractSortable, IPreStarter
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractPreStarter"/>。
        /// </summary>
        /// <param name="priority">给定的优先级（可选；默认为 <see cref="AbstractSortable.DefaultPriority"/>）。</param>
        protected AbstractPreStarter(float? priority = null)
            : base(priority)
        {
        }


        /// <summary>
        /// 启动。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public abstract IServiceCollection Start(IServiceCollection services);
    }
}