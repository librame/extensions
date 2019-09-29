#region License

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
    class HumanizationService : AbstractConcurrentService, IHumanizationService
    {
        private readonly IExpressionLocalizer<HumanizationResource> _localizer;


        public HumanizationService(IExpressionLocalizer<HumanizationResource> localizer,
            IMemoryLocker locker, ILoggerFactory loggerFactory)
            : base(locker, loggerFactory)
        {
            _localizer = localizer.NotNull(nameof(localizer));
        }


        public Task<string> HumanizeAsync(DateTime dateTime, CancellationToken cancellationToken = default)
        {
            return Locker.WaitFactory(() =>
            {
                return cancellationToken.RunFactoryOrCancellationAsync(() =>
                {
                    var now = DateTime.Now;
                    if (now <= dateTime)
                    {
                        Logger.LogWarning($"The {dateTime} is greater than {now}");
                        return now.ToString();
                    }

                    return HumanizeCore(now - dateTime);
                });
            });
        }

        public Task<string> HumanizeAsync(DateTimeOffset dateTimeOffset, CancellationToken cancellationToken = default)
        {
            return Locker.WaitFactory(() =>
            {
                return cancellationToken.RunFactoryOrCancellationAsync(() =>
                {
                    var now = DateTimeOffset.Now;
                    if (now <= dateTimeOffset)
                    {
                        Logger.LogWarning($"The {dateTimeOffset} is greater than {now}");
                        return now.ToString();
                    }

                    return HumanizeCore(now - dateTimeOffset);
                });
            });
        }

        private string HumanizeCore(TimeSpan timeSpan)
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

            return $"{count} {label}";
        }

    }
}
