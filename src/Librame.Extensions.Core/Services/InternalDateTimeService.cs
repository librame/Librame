﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 内部日期与时间服务。
    /// </summary>
    internal class InternalDateTimeService : AbstractService<InternalDateTimeService>, IDateTimeService
    {
        private readonly IExpressionStringLocalizer<DateTimeResource> _localizer;

        /// <summary>
        /// 构造一个 <see cref="InternalDateTimeService"/> 实例。
        /// </summary>
        /// <param name="localizer">给定的 <see cref="IExpressionStringLocalizer{DateTimeResource}"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalDateTimeService}"/>。</param>
        public InternalDateTimeService(IExpressionStringLocalizer<DateTimeResource> localizer,
            ILogger<InternalDateTimeService> logger)
            : base(logger)
        {
            _localizer = localizer.NotNull(nameof(localizer));
        }


        /// <summary>
        /// 异步人性化。
        /// </summary>
        /// <param name="dateTime">给定的 <see cref="DateTime"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public Task<string> HumanizeAsync(DateTime dateTime, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var now = DateTime.Now;

            if (now <= dateTime)
            {
                Logger.LogWarning($"The {dateTime} is greater than {now}");
                return Task.FromResult(now.ToString());
            }

            return HumanizeCoreAsync(now - dateTime);
        }

        /// <summary>
        /// 异步人性化。
        /// </summary>
        /// <param name="dateTimeOffset">给定的 <see cref="DateTimeOffset"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public Task<string> HumanizeAsync(DateTimeOffset dateTimeOffset, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var utcNow = DateTimeOffset.Now;

            if (utcNow <= dateTimeOffset)
            {
                Logger.LogWarning($"The {dateTimeOffset} is greater than {utcNow}");
                return Task.FromResult(utcNow.ToString());
            }

            return HumanizeCoreAsync(utcNow - dateTimeOffset);
        }

        private Task<string> HumanizeCoreAsync(TimeSpan timeSpan)
        {
            int count = 0;
            var label = string.Empty;

            if (timeSpan.TotalDays > 365)
            {
                count = (int)Math.Round(timeSpan.TotalDays / 365);
                label = _localizer[r => r.HumanizedYearsAgo];
            }
            else if (timeSpan.TotalDays > 30)
            {
                count = (int)Math.Round(timeSpan.TotalDays / 30);
                label = _localizer[r => r.HumanizedMonthsAgo];
            }
            else if (timeSpan.TotalDays > 1)
            {
                count = (int)Math.Round(timeSpan.TotalDays / 1);
                label = _localizer[r => r.HumanizedDaysAgo];
            }
            else if (timeSpan.TotalHours > 1)
            {
                count = (int)Math.Round(timeSpan.TotalHours / 1);
                label = _localizer[r => r.HumanizedHoursAgo];
            }
            else if (timeSpan.TotalMinutes > 1)
            {
                count = (int)Math.Round(timeSpan.TotalMinutes / 1);
                label = _localizer[r => r.HumanizedMinutesAgo];
            }

            return Task.FromResult($"{count} {label}");
        }
    }
}
