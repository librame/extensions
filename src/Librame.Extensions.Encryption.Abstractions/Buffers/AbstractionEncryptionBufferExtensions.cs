#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;

namespace Librame.Extensions.Encryption
{
    using Core;

    /// <summary>
    /// 抽象算法缓冲区静态扩展。
    /// </summary>
    public static class AbstractionEncryptionBufferExtensions
    {

        #region CiphertextString

        /// <summary>
        /// 转换为密文字符串。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IEncryptionBuffer{IPlaintextAlgorithmConverter, String}"/>。</param>
        /// <param name="converter">给定的密文转换器（可选；默认使用 <see cref="IEncryptionBuffer{IPlaintextAlgorithmConverter, String}.ServiceProvider"/> 解析）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsCiphertextString(this IEncryptionBuffer<IPlaintextConverter, string> buffer,
            ICiphertextConverter converter = null)
        {
            if (converter.IsNull())
                converter = buffer.ServiceProvider.GetRequiredService<ICiphertextConverter>();

            return converter.From(buffer);
        }

        /// <summary>
        /// 转换为密文字符串。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IEncryptionBuffer{ICiphertextAlgorithmConverter, String}"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsCiphertextString(this IEncryptionBuffer<ICiphertextConverter, string> buffer)
            => buffer.Converter.From(buffer);

        /// <summary>
        /// 还原密文字符串。
        /// </summary>
        /// <param name="str">给定的密文字符串。</param>
        /// <param name="buffer">给定的 <see cref="IEncryptionBuffer{ICiphertextAlgorithmConverter, String}"/>。</param>
        /// <returns>返回缓冲区。</returns>
        public static IByteBuffer FromCiphertextString(this string str, IEncryptionBuffer<ICiphertextConverter, string> buffer)
            => buffer.Converter.To(str);

        #endregion


        #region PlaintextString

        /// <summary>
        /// 转换为明文字符串。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IEncryptionBuffer{ICiphertextAlgorithmConverter, String}"/>。</param>
        /// <param name="converter">给定的明文转换器（可选；默认使用 <see cref="IEncryptionBuffer{ICiphertextAlgorithmConverter, String}.ServiceProvider"/> 解析）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsPlaintextString(this IEncryptionBuffer<ICiphertextConverter, string> buffer,
            IPlaintextConverter converter = null)
        {
            if (converter.IsNull())
                converter = buffer.ServiceProvider.GetRequiredService<IPlaintextConverter>();

            return converter.From(buffer);
        }

        #endregion


        #region Hash

        /// <summary>
        /// 计算 MD5。
        /// </summary>
        /// <typeparam name="TConverter">指定的转换器类型。</typeparam>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <param name="buffer">给定的算法缓冲区。</param>
        /// <param name="isSigned">是否使用 RSA 非对称算法签名（默认不签名）。</param>
        /// <returns>返回算法缓冲区。</returns>
        public static IEncryptionBuffer<TConverter, TSource> Md5<TConverter, TSource>(this IEncryptionBuffer<TConverter, TSource> buffer,
            bool isSigned = false)
            where TConverter : IAlgorithmConverter<TSource>
        {
            var hash = buffer.ServiceProvider.GetRequiredService<IHashService>();
            return buffer.Md5(hash, isSigned);
        }
        /// <summary>
        /// 计算 MD5。
        /// </summary>
        /// <typeparam name="TBuffer">指定的缓冲区类型。</typeparam>
        /// <param name="buffer">给定的缓冲区。</param>
        /// <param name="hash">给定的 <see cref="IHashService"/>。</param>
        /// <param name="isSigned">是否使用 RSA 非对称算法签名（默认不签名）。</param>
        /// <returns>返回缓冲区。</returns>
        public static TBuffer Md5<TBuffer>(this TBuffer buffer, IHashService hash, bool isSigned = false)
            where TBuffer : IByteBuffer
        {
            hash.Md5(buffer, isSigned);
            return buffer;
        }
        
        /// <summary>
        /// 计算 SHA1。
        /// </summary>
        /// <typeparam name="TConverter">指定的转换器类型。</typeparam>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <param name="buffer">给定的算法缓冲区。</param>
        /// <param name="isSigned">是否使用 RSA 非对称算法签名（默认不签名）。</param>
        /// <returns>返回算法缓冲区。</returns>
        public static IEncryptionBuffer<TConverter, TSource> Sha1<TConverter, TSource>(this IEncryptionBuffer<TConverter, TSource> buffer,
            bool isSigned = false)
            where TConverter : IAlgorithmConverter<TSource>
        {
            var hash = buffer.ServiceProvider.GetRequiredService<IHashService>();
            return buffer.Sha1(hash, isSigned);
        }
        /// <summary>
        /// 计算 SHA1。
        /// </summary>
        /// <typeparam name="TBuffer">指定的缓冲区类型。</typeparam>
        /// <param name="buffer">给定的缓冲区。</param>
        /// <param name="hash">给定的 <see cref="IHashService"/>。</param>
        /// <param name="isSigned">是否使用 RSA 非对称算法签名（默认不签名）。</param>
        /// <returns>返回缓冲区。</returns>
        public static TBuffer Sha1<TBuffer>(this TBuffer buffer, IHashService hash, bool isSigned = false)
            where TBuffer : IByteBuffer
        {
            hash.Sha1(buffer, isSigned);
            return buffer;
        }
        
        /// <summary>
        /// 计算 SHA256。
        /// </summary>
        /// <typeparam name="TConverter">指定的转换器类型。</typeparam>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <param name="buffer">给定的算法缓冲区。</param>
        /// <param name="isSigned">是否使用 RSA 非对称算法签名（默认不签名）。</param>
        /// <returns>返回算法缓冲区。</returns>
        public static IEncryptionBuffer<TConverter, TSource> Sha256<TConverter, TSource>(this IEncryptionBuffer<TConverter, TSource> buffer,
            bool isSigned = false)
            where TConverter : IAlgorithmConverter<TSource>
        {
            var hash = buffer.ServiceProvider.GetRequiredService<IHashService>();
            return buffer.Sha256(hash, isSigned);
        }
        /// <summary>
        /// 计算 SHA256。
        /// </summary>
        /// <typeparam name="TBuffer">指定的缓冲区类型。</typeparam>
        /// <param name="buffer">给定的缓冲区。</param>
        /// <param name="hash">给定的 <see cref="IHashService"/>。</param>
        /// <param name="isSigned">是否使用 RSA 非对称算法签名（默认不签名）。</param>
        /// <returns>返回缓冲区。</returns>
        public static TBuffer Sha256<TBuffer>(this TBuffer buffer, IHashService hash, bool isSigned = false)
            where TBuffer : IByteBuffer
        {
            hash.Sha256(buffer, isSigned);
            return buffer;
        }
        
        /// <summary>
        /// 计算 SHA384。
        /// </summary>
        /// <typeparam name="TConverter">指定的转换器类型。</typeparam>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <param name="buffer">给定的算法缓冲区。</param>
        /// <param name="isSigned">是否使用 RSA 非对称算法签名（默认不签名）。</param>
        /// <returns>返回算法缓冲区。</returns>
        public static IEncryptionBuffer<TConverter, TSource> Sha384<TConverter, TSource>(this IEncryptionBuffer<TConverter, TSource> buffer,
            bool isSigned = false)
            where TConverter : IAlgorithmConverter<TSource>
        {
            var hash = buffer.ServiceProvider.GetRequiredService<IHashService>();
            return buffer.Sha384(hash, isSigned);
        }
        /// <summary>
        /// 计算 SHA384。
        /// </summary>
        /// <typeparam name="TBuffer">指定的缓冲区类型。</typeparam>
        /// <param name="buffer">给定的缓冲区。</param>
        /// <param name="hash">给定的 <see cref="IHashService"/>。</param>
        /// <param name="isSigned">是否使用 RSA 非对称算法签名（默认不签名）。</param>
        /// <returns>返回缓冲区。</returns>
        public static TBuffer Sha384<TBuffer>(this TBuffer buffer, IHashService hash, bool isSigned = false)
            where TBuffer : IByteBuffer
        {
            hash.Sha384(buffer, isSigned);
            return buffer;
        }
        
        /// <summary>
        /// 计算 SHA512。
        /// </summary>
        /// <typeparam name="TConverter">指定的转换器类型。</typeparam>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <param name="buffer">给定的算法缓冲区。</param>
        /// <param name="isSigned">是否使用 RSA 非对称算法签名（默认不签名）。</param>
        /// <returns>返回算法缓冲区。</returns>
        public static IEncryptionBuffer<TConverter, TSource> Sha512<TConverter, TSource>(this IEncryptionBuffer<TConverter, TSource> buffer,
            bool isSigned = false)
            where TConverter : IAlgorithmConverter<TSource>
        {
            var hash = buffer.ServiceProvider.GetRequiredService<IHashService>();
            return buffer.Sha512(hash, isSigned);
        }
        /// <summary>
        /// 计算 SHA512。
        /// </summary>
        /// <typeparam name="TBuffer">指定的缓冲区类型。</typeparam>
        /// <param name="buffer">给定的缓冲区。</param>
        /// <param name="hash">给定的 <see cref="IHashService"/>。</param>
        /// <param name="isSigned">是否使用 RSA 非对称算法签名（默认不签名）。</param>
        /// <returns>返回缓冲区。</returns>
        public static TBuffer Sha512<TBuffer>(this TBuffer buffer, IHashService hash, bool isSigned = false)
            where TBuffer : IByteBuffer
        {
            hash.Sha512(buffer, isSigned);
            return buffer;
        }

        #endregion


        #region HMAC Hash

        /// <summary>
        /// 计算 HMACMD5。
        /// </summary>
        /// <typeparam name="TConverter">指定的转换器类型。</typeparam>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <param name="buffer">给定的算法缓冲区。</param>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回算法缓冲区。</returns>
        public static IEncryptionBuffer<TConverter, TSource> HmacMd5<TConverter, TSource>(this IEncryptionBuffer<TConverter, TSource> buffer,
            UniqueIdentifier? identifier = null)
            where TConverter : IAlgorithmConverter<TSource>
        {
            var keyedHash = buffer.ServiceProvider.GetRequiredService<IKeyedHashService>();
            return buffer.HmacMd5(keyedHash, identifier);
        }
        /// <summary>
        /// 计算 HMACMD5。
        /// </summary>
        /// <typeparam name="TBuffer">指定的缓冲区类型。</typeparam>
        /// <param name="buffer">给定的缓冲区。</param>
        /// <param name="keyedHash">给定的 <see cref="IKeyedHashService"/>。</param>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回缓冲区。</returns>
        public static TBuffer HmacMd5<TBuffer>(this TBuffer buffer, IKeyedHashService keyedHash, UniqueIdentifier? identifier = null)
            where TBuffer : IByteBuffer
        {
            keyedHash.HmacMd5(buffer, identifier);
            return buffer;
        }

        /// <summary>
        /// 计算 HMACSHA1。
        /// </summary>
        /// <typeparam name="TConverter">指定的转换器类型。</typeparam>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <param name="buffer">给定的算法缓冲区。</param>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回算法缓冲区。</returns>
        public static IEncryptionBuffer<TConverter, TSource> HmacSha1<TConverter, TSource>(this IEncryptionBuffer<TConverter, TSource> buffer,
            UniqueIdentifier? identifier = null)
            where TConverter : IAlgorithmConverter<TSource>
        {
            var keyedHash = buffer.ServiceProvider.GetRequiredService<IKeyedHashService>();
            return buffer.HmacSha1(keyedHash, identifier);
        }
        /// <summary>
        /// 计算 HMACSHA1。
        /// </summary>
        /// <typeparam name="TBuffer">指定的缓冲区类型。</typeparam>
        /// <param name="buffer">给定的缓冲区。</param>
        /// <param name="keyedHash">给定的 <see cref="IKeyedHashService"/>。</param>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回缓冲区。</returns>
        public static TBuffer HmacSha1<TBuffer>(this TBuffer buffer, IKeyedHashService keyedHash, UniqueIdentifier? identifier = null)
            where TBuffer : IByteBuffer
        {
            keyedHash.HmacSha1(buffer, identifier);
            return buffer;
        }

        /// <summary>
        /// 计算 HMACSHA256。
        /// </summary>
        /// <typeparam name="TConverter">指定的转换器类型。</typeparam>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <param name="buffer">给定的算法缓冲区。</param>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回算法缓冲区。</returns>
        public static IEncryptionBuffer<TConverter, TSource> HmacSha256<TConverter, TSource>(this IEncryptionBuffer<TConverter, TSource> buffer,
            UniqueIdentifier? identifier = null)
            where TConverter : IAlgorithmConverter<TSource>
        {
            var keyedHash = buffer.ServiceProvider.GetRequiredService<IKeyedHashService>();
            return buffer.HmacSha256(keyedHash, identifier);
        }
        /// <summary>
        /// 计算 HMACSHA256。
        /// </summary>
        /// <typeparam name="TBuffer">指定的缓冲区类型。</typeparam>
        /// <param name="buffer">给定的缓冲区。</param>
        /// <param name="keyedHash">给定的 <see cref="IKeyedHashService"/>。</param>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回缓冲区。</returns>
        public static TBuffer HmacSha256<TBuffer>(this TBuffer buffer, IKeyedHashService keyedHash, UniqueIdentifier? identifier = null)
            where TBuffer : IByteBuffer
        {
            keyedHash.HmacSha256(buffer, identifier);
            return buffer;
        }

        /// <summary>
        /// 计算 HMACSHA384。
        /// </summary>
        /// <typeparam name="TConverter">指定的转换器类型。</typeparam>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <param name="buffer">给定的算法缓冲区。</param>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回算法缓冲区。</returns>
        public static IEncryptionBuffer<TConverter, TSource> HmacSha384<TConverter, TSource>(this IEncryptionBuffer<TConverter, TSource> buffer,
            UniqueIdentifier? identifier = null)
            where TConverter : IAlgorithmConverter<TSource>
        {
            var keyedHash = buffer.ServiceProvider.GetRequiredService<IKeyedHashService>();
            return buffer.HmacSha384(keyedHash, identifier);
        }
        /// <summary>
        /// 计算 HMACSHA384。
        /// </summary>
        /// <typeparam name="TBuffer">指定的缓冲区类型。</typeparam>
        /// <param name="buffer">给定的缓冲区。</param>
        /// <param name="keyedHash">给定的 <see cref="IKeyedHashService"/>。</param>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回缓冲区。</returns>
        public static TBuffer HmacSha384<TBuffer>(this TBuffer buffer, IKeyedHashService keyedHash, UniqueIdentifier? identifier = null)
            where TBuffer : IByteBuffer
        {
            keyedHash.HmacSha384(buffer, identifier);
            return buffer;
        }

        /// <summary>
        /// 计算 HMACSHA512。
        /// </summary>
        /// <typeparam name="TConverter">指定的转换器类型。</typeparam>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <param name="buffer">给定的算法缓冲区。</param>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回算法缓冲区。</returns>
        public static IEncryptionBuffer<TConverter, TSource> HmacSha512<TConverter, TSource>(this IEncryptionBuffer<TConverter, TSource> buffer,
            UniqueIdentifier? identifier = null)
            where TConverter : IAlgorithmConverter<TSource>
        {
            var keyedHash = buffer.ServiceProvider.GetRequiredService<IKeyedHashService>();
            return buffer.HmacSha512(keyedHash, identifier);
        }
        /// <summary>
        /// 计算 HMACSHA512。
        /// </summary>
        /// <typeparam name="TBuffer">指定的缓冲区类型。</typeparam>
        /// <param name="buffer">给定的缓冲区。</param>
        /// <param name="keyedHash">给定的 <see cref="IKeyedHashService"/>。</param>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回缓冲区。</returns>
        public static TBuffer HmacSha512<TBuffer>(this TBuffer buffer, IKeyedHashService keyedHash, UniqueIdentifier? identifier = null)
            where TBuffer : IByteBuffer
        {
            keyedHash.HmacSha512(buffer, identifier);
            return buffer;
        }

        #endregion


        #region SymmetricAlgorithm

        /// <summary>
        /// AES 加密。
        /// </summary>
        /// <typeparam name="TConverter">指定的转换器类型。</typeparam>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <param name="buffer">给定的算法缓冲区。</param>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回算法缓冲区。</returns>
        public static IEncryptionBuffer<TConverter, TSource> AsAes<TConverter, TSource>(this IEncryptionBuffer<TConverter, TSource> buffer,
            UniqueIdentifier? identifier = null)
            where TConverter : IAlgorithmConverter<TSource>
        {
            var symmetric = buffer.ServiceProvider.GetRequiredService<ISymmetricService>();
            return buffer.AsAes(symmetric, identifier);
        }
        /// <summary>
        /// AES 加密。
        /// </summary>
        /// <typeparam name="TBuffer">指定的缓冲区类型。</typeparam>
        /// <param name="buffer">给定的缓冲区。</param>
        /// <param name="symmetric">给定的 <see cref="ISymmetricService"/>。</param>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回缓冲区。</returns>
        public static TBuffer AsAes<TBuffer>(this TBuffer buffer, ISymmetricService symmetric, UniqueIdentifier? identifier = null)
            where TBuffer : IByteBuffer
        {
            symmetric.ToAes(buffer, identifier);
            return buffer;
        }

        /// <summary>
        /// AES 解密。
        /// </summary>
        /// <typeparam name="TConverter">指定的转换器类型。</typeparam>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <param name="buffer">给定的算法缓冲区。</param>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回算法缓冲区。</returns>
        public static IEncryptionBuffer<TConverter, TSource> FromAes<TConverter, TSource>(this IEncryptionBuffer<TConverter, TSource> buffer,
            UniqueIdentifier? identifier = null)
            where TConverter : IAlgorithmConverter<TSource>
        {
            var symmetric = buffer.ServiceProvider.GetRequiredService<ISymmetricService>();
            return buffer.FromAes(symmetric, identifier);
        }
        /// <summary>
        /// AES 解密。
        /// </summary>
        /// <typeparam name="TBuffer">指定的缓冲区类型。</typeparam>
        /// <param name="buffer">给定的缓冲区。</param>
        /// <param name="symmetric">给定的 <see cref="ISymmetricService"/>。</param>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回缓冲区。</returns>
        public static TBuffer FromAes<TBuffer>(this TBuffer buffer, ISymmetricService symmetric, UniqueIdentifier? identifier = null)
            where TBuffer : IByteBuffer
        {
            symmetric.FromAes(buffer, identifier);
            return buffer;
        }


        /// <summary>
        /// DES 加密。
        /// </summary>
        /// <typeparam name="TConverter">指定的转换器类型。</typeparam>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <param name="buffer">给定的算法缓冲区。</param>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回算法缓冲区。</returns>
        public static IEncryptionBuffer<TConverter, TSource> AsDes<TConverter, TSource>(this IEncryptionBuffer<TConverter, TSource> buffer,
            UniqueIdentifier? identifier = null)
            where TConverter : IAlgorithmConverter<TSource>
        {
            var symmetric = buffer.ServiceProvider.GetRequiredService<ISymmetricService>();
            return buffer.AsDes(symmetric, identifier);
        }
        /// <summary>
        /// DES 加密。
        /// </summary>
        /// <typeparam name="TBuffer">指定的缓冲区类型。</typeparam>
        /// <param name="buffer">给定的缓冲区。</param>
        /// <param name="symmetric">给定的 <see cref="ISymmetricService"/>。</param>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回缓冲区。</returns>
        public static TBuffer AsDes<TBuffer>(this TBuffer buffer, ISymmetricService symmetric, UniqueIdentifier? identifier = null)
            where TBuffer : IByteBuffer
        {
            symmetric.ToDes(buffer, identifier);
            return buffer;
        }

        /// <summary>
        /// DES 解密。
        /// </summary>
        /// <typeparam name="TConverter">指定的转换器类型。</typeparam>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <param name="buffer">给定的算法缓冲区。</param>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回算法缓冲区。</returns>
        public static IEncryptionBuffer<TConverter, TSource> FromDes<TConverter, TSource>(this IEncryptionBuffer<TConverter, TSource> buffer,
            UniqueIdentifier? identifier = null)
            where TConverter : IAlgorithmConverter<TSource>
        {
            var symmetric = buffer.ServiceProvider.GetRequiredService<ISymmetricService>();
            return buffer.FromDes(symmetric, identifier);
        }
        /// <summary>
        /// DES 解密。
        /// </summary>
        /// <typeparam name="TBuffer">指定的缓冲区类型。</typeparam>
        /// <param name="buffer">给定的缓冲区。</param>
        /// <param name="symmetric">给定的 <see cref="ISymmetricService"/>。</param>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回缓冲区。</returns>
        public static TBuffer FromDes<TBuffer>(this TBuffer buffer, ISymmetricService symmetric, UniqueIdentifier? identifier = null)
            where TBuffer : IByteBuffer
        {
            symmetric.FromDes(buffer, identifier);
            return buffer;
        }


        /// <summary>
        /// TripleDES 加密。
        /// </summary>
        /// <typeparam name="TConverter">指定的转换器类型。</typeparam>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <param name="buffer">给定的算法缓冲区。</param>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回算法缓冲区。</returns>
        public static IEncryptionBuffer<TConverter, TSource> AsTripleDes<TConverter, TSource>(this IEncryptionBuffer<TConverter, TSource> buffer,
            UniqueIdentifier? identifier = null)
            where TConverter : IAlgorithmConverter<TSource>
        {
            var symmetric = buffer.ServiceProvider.GetRequiredService<ISymmetricService>();
            return buffer.AsTripleDes(symmetric, identifier);
        }
        /// <summary>
        /// TripleDES 加密。
        /// </summary>
        /// <typeparam name="TBuffer">指定的缓冲区类型。</typeparam>
        /// <param name="buffer">给定的缓冲区。</param>
        /// <param name="symmetric">给定的 <see cref="ISymmetricService"/>。</param>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回缓冲区。</returns>
        public static TBuffer AsTripleDes<TBuffer>(this TBuffer buffer, ISymmetricService symmetric, UniqueIdentifier? identifier = null)
            where TBuffer : IByteBuffer
        {
            symmetric.ToTripleDes(buffer, identifier);
            return buffer;
        }

        /// <summary>
        /// TripleDES 解密。
        /// </summary>
        /// <typeparam name="TConverter">指定的转换器类型。</typeparam>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <param name="buffer">给定的算法缓冲区。</param>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回算法缓冲区。</returns>
        public static IEncryptionBuffer<TConverter, TSource> FromTripleDes<TConverter, TSource>(this IEncryptionBuffer<TConverter, TSource> buffer,
            UniqueIdentifier? identifier = null)
            where TConverter : IAlgorithmConverter<TSource>
        {
            var symmetric = buffer.ServiceProvider.GetRequiredService<ISymmetricService>();
            return buffer.FromTripleDes(symmetric, identifier);
        }
        /// <summary>
        /// TripleDES 解密。
        /// </summary>
        /// <typeparam name="TBuffer">指定的缓冲区类型。</typeparam>
        /// <param name="buffer">给定的缓冲区。</param>
        /// <param name="symmetric">给定的 <see cref="ISymmetricService"/>。</param>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回缓冲区。</returns>
        public static TBuffer FromTripleDes<TBuffer>(this TBuffer buffer, ISymmetricService symmetric, UniqueIdentifier? identifier = null)
            where TBuffer : IByteBuffer
        {
            symmetric.FromTripleDes(buffer, identifier);
            return buffer;
        }

        #endregion


        #region RSA AsymmetricAlgorithm

        /// <summary>
        /// RSA 加密。
        /// </summary>
        /// <typeparam name="TConverter">指定的转换器类型。</typeparam>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <param name="buffer">给定的算法缓冲区。</param>
        /// <returns>返回算法缓冲区。</returns>
        public static IEncryptionBuffer<TConverter, TSource> AsRsa<TConverter, TSource>(this IEncryptionBuffer<TConverter, TSource> buffer)
            where TConverter : IAlgorithmConverter<TSource>
        {
            var rsa = buffer.ServiceProvider.GetRequiredService<IRsaService>();
            return buffer.AsRsa(rsa);
        }
        /// <summary>
        /// RSA 加密。
        /// </summary>
        /// <typeparam name="TBuffer">指定的缓冲区类型。</typeparam>
        /// <param name="buffer">给定的缓冲区。</param>
        /// <param name="rsa">给定的 <see cref="IRsaService"/>。</param>
        /// <returns>返回缓冲区。</returns>
        public static TBuffer AsRsa<TBuffer>(this TBuffer buffer, IRsaService rsa)
            where TBuffer : IByteBuffer
        {
            rsa.Encrypt(buffer);
            return buffer;
        }

        /// <summary>
        /// RSA 解密。
        /// </summary>
        /// <typeparam name="TConverter">指定的转换器类型。</typeparam>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <param name="buffer">给定的算法缓冲区。</param>
        /// <returns>返回算法缓冲区。</returns>
        public static IEncryptionBuffer<TConverter, TSource> FromRsa<TConverter, TSource>(this IEncryptionBuffer<TConverter, TSource> buffer)
            where TConverter : IAlgorithmConverter<TSource>
        {
            var rsa = buffer.ServiceProvider.GetRequiredService<IRsaService>();
            return buffer.FromRsa(rsa);
        }
        /// <summary>
        /// RSA 解密。
        /// </summary>
        /// <typeparam name="TBuffer">指定的缓冲区类型。</typeparam>
        /// <param name="buffer">给定的缓冲区。</param>
        /// <param name="rsa">给定的 <see cref="IRsaService"/>。</param>
        /// <returns>返回缓冲区。</returns>
        public static TBuffer FromRsa<TBuffer>(this TBuffer buffer, IRsaService rsa)
            where TBuffer : IByteBuffer
        {
            rsa.Decrypt(buffer);
            return buffer;
        }

        #endregion

    }
}
