using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Librame.Extensions.Drawing.Tests
{
    using Core.Combiners;
    using Core.Utilities;
    using Drawing.Services;

    public class CaptchaServiceTests
    {
        private ICaptchaService _service = null;
        private string[] _captchas = null;

        public CaptchaServiceTests()
        {
            _service = TestServiceProvider.Current.GetRequiredService<ICaptchaService>();

            var now = DateTimeOffset.Now;
            _captchas = new string[] { now.ToString("yyyy"), now.ToString("HHmmss"), now.Ticks.ToString() };
            //var captcha = now.ToString("HHmmss");
        }


        [Fact]
        public async void DrawCaptchaBytesTest()
        {
            var captcha = string.Empty;
            RandomUtility.Run(r =>
            {
                captcha = _captchas[r.Next(0, _captchas.Length)];
            });

            var buffer = await _service.DrawBytesAsync(captcha).ConfigureAwait();
            Assert.NotNull(buffer);
        }

        [Fact]
        public async void DrawCaptchaFileTest()
        {
            var captcha = string.Empty;
            RandomUtility.Run(r =>
            {
                captcha = _captchas[r.Next(0, _captchas.Length)];
            });

            var saveFile = "captcha.png".AsFilePathCombiner(_service.Dependency.ResourceDirectory);
            var result = await _service.DrawFileAsync(captcha, saveFile).ConfigureAwait();
            Assert.True(result);
        }

    }
}
