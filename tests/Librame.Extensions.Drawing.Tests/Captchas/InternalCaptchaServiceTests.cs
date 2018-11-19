using System;
using Xunit;

namespace Librame.Extensions.Drawing.Tests
{
    public class InternalCaptchaServiceTests
    {
        private ICaptchaService _drawing = null;
        private string[] _captchas = null;

        public InternalCaptchaServiceTests()
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

            var buffer = await _drawing.DrawBytes(captcha);
            Assert.NotNull(buffer);
        }

        [Fact]
        public async void DrawCaptchaFileTest()
        {
            var captcha = _captchas[new Random().Next(0, _captchas.Length)];
            var saveFile = "captcha.png".AsFileLocator(TestServiceProvider.ResourcesPath);

            var succeed = await _drawing.DrawFile(captcha, saveFile.ToString());
            Assert.True(succeed);
        }

    }
}
