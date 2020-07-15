#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Services
{
    using Resources;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class HumanizationService : AbstractService, IHumanizationService
    {
        private readonly IStringLocalizer<HumanizationResource> _localizer;


        public HumanizationService(IStringLocalizer<HumanizationResource> localizer,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            _localizer = localizer.NotNull(nameof(localizer));
        }


        public Task<string> HumanizeAsync(DateTime dateTime, CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunOrCancelAsync(() =>
            {
                var now = DateTime.Now;
                if (now <= dateTime)
                {
                    Logger.LogWarning($"The {dateTime} is greater than {now}");
                    return now.ToString(CultureInfo.CurrentCulture);
                }

                return HumanizeCore(now - dateTime);
            });
        }

        public Task<string> HumanizeAsync(DateTimeOffset dateTimeOffset, CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunOrCancelAsync(() =>
            {
                var now = DateTimeOffset.Now;
                if (now <= dateTimeOffset)
                {
                    Logger.LogWarning($"The {dateTimeOffset} is greater than {now}");
                    return now.ToString(CultureInfo.CurrentCulture);
                }

                return HumanizeCore(now - dateTimeOffset);
            });
        }

        private string HumanizeCore(TimeSpan timeSpan)
        {
            int count = 0;
            var label = string.Empty;

            if (timeSpan.TotalDays > 365)
            {
                count = (int)Math.Round(timeSpan.TotalDays / 365);
                label = _localizer.GetString(r => r.HumanizedYearsAgo);
            }
            else if (timeSpan.TotalDays > 30)
            {
                count = (int)Math.Round(timeSpan.TotalDays / 30);
                label = _localizer.GetString(r => r.HumanizedMonthsAgo);
            }
            else if (timeSpan.TotalDays > 1)
            {
                count = (int)Math.Round(timeSpan.TotalDays / 1);
                label = _localizer.GetString(r => r.HumanizedDaysAgo);
            }
            else if (timeSpan.TotalHours > 1)
            {
                count = (int)Math.Round(timeSpan.TotalHours / 1);
                label = _localizer.GetString(r => r.HumanizedHoursAgo);
            }
            else if (timeSpan.TotalMinutes > 1)
            {
                count = (int)Math.Round(timeSpan.TotalMinutes / 1);
                label = _localizer.GetString(r => r.HumanizedMinutesAgo);
            }

            return $"{count} {label}";
        }

    }
}
