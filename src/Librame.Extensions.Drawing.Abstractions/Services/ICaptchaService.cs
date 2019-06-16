#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Drawing
{
    using Core;

    /// <summary>
    /// 图画服务接口。
    /// </summary>
    public interface ICaptchaService : IService
    {
        /// <summary>
        /// 绘制验证码文件。
        /// </summary>
        /// <param name="captcha">给定的验证码。</param>
        /// <param name="savePath">给定的保存路径。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否成功的异步操作。</returns>
        Task<bool> DrawFile(string captcha, string savePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// 绘制验证码流。
        /// </summary>
        /// <param name="captcha">给定的验证码。</param>
        /// <param name="target">给定的目标流。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否成功的异步操作。</returns>
        Task<bool> DrawStream(string captcha, Stream target, CancellationToken cancellationToken = default);

        /// <summary>
        /// 绘制验证码字节数组。
        /// </summary>
        /// <param name="captcha">给定的验证码。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含图像字节数组的异步操作。</returns>
        Task<byte[]> DrawBytes(string captcha, CancellationToken cancellationToken = default);
    }
}
