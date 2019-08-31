using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Librame.Extensions.Drawing.Tests
{
    using Core;

    public class CaptchaServiceTests
    {
        private ICaptchaService _drawing = null;
        private string[] _captchas = null;

        public CaptchaServiceTests()
        {
            _drawing = TestServiceProvider.Current.GetRequiredService<ICaptchaService>();

            var now = DateTimeOffset.Now;
            _captchas = new string[] { now.ToString("yyyy"), now.ToString("HHmmss"), now.Ticks.ToString() };
            //var captcha = now.ToString("HHmmss");
        }


        [Fact]
        public async void DrawCaptchaBytesTest()
        {
            var captcha = _captchas[new Random().Next(0, _captchas.Length)];
            var buffer = await _drawing.DrawBytesAsync(captcha);
            Assert.NotNull(buffer);
        }

        [Fact]
        public async void DrawCaptchaFileTest()
        {
            var captcha = _captchas[new Random().Next(0, _captchas.Length)];
            var saveFile = "captcha.png".AsFileLocator(TestServiceProvider.ResourcesPath);
            var result = await _drawing.DrawFileAsync(captcha, saveFile);
            Assert.True(result);
        }

    }
}
