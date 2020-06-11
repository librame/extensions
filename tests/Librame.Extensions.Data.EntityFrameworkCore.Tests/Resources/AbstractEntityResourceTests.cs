using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit;
using System.Globalization;

namespace Librame.Extensions.Data.Tests
{
    using Core.Utilities;
    using Data.Resources;

    public class AbstractEntityResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IStringLocalizer<AbstractEntityResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }

        private void RunTest(IStringLocalizer<AbstractEntityResource> localizer, string cultureName)
        {
            CultureInfoUtility.Register(new CultureInfo(cultureName));

            // GlobalGroup
            var localized = localizer.GetString(r => r.GlobalGroup);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.Id);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.TenantId);
            Assert.False(localized.ResourceNotFound);

            // DataGroup
            localized = localizer.GetString(r => r.DataGroup);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.Rank);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.Status);
            Assert.False(localized.ResourceNotFound);

            // Properties
            localized = localizer.GetString(r => r.ConcurrencyStamp);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.CreatedTime);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.CreatedTimeTicks);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.CreatedBy);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.UpdatedTime);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.UpdatedTimeTicks);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.UpdatedBy);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.PublishedTime);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.PublishedTimeTicks);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.PublishedBy);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.PublishedAs);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.IsDefaultValue);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.LockoutEnabled);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.LockoutEnd);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.ParentId);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.RowVersion);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.SupporterCount);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.ObjectorCount);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.FavoriteCount);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.RetweetCount);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.CommentCount);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.CommenterCount);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.VisitCount);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.VisitorCount);
            Assert.False(localized.ResourceNotFound);
        }

    }
}
