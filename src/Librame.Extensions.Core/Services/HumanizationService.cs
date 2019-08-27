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
using System.Threading.Tasks;

namespace Librame.Extensions.Core
{
    class HumanizationService : AbstractService, IHumanizationService
    {
        private readonly IExpressionStringLocalizer<HumanizationResource> _localizer;


        public HumanizationService(IExpressionStringLocalizer<HumanizationResource> localizer,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            _localizer = localizer.NotNull(nameof(localizer));
        }


        public Task<string> HumanizeAsync(DateTime dateTime)
        {
            return Task.Run(() =>
            {
                var now = DateTime.Now;

                if (now <= dateTime)
                {
                    Logger.LogWarning($"The {dateTime} is greater than {now}");
                    return now.ToString();
                }

                return HumanizeCore(now - dateTime);
            });
        }

        public Task<string> HumanizeAsync(DateTimeOffset dateTimeOffset)
        {
            return Task.Run(() =>
            {
                var now = DateTimeOffset.Now;

                if (now <= dateTimeOffset)
                {
                    Logger.LogWarning($"The {dateTimeOffset} is greater than {now}");
                    return now.ToString();
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
