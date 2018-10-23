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
using System.IO;
using System.Security.Cryptography;

namespace Librame.Extensions.Encryption.RsaKeySerializers
{
    using Services;

    /// <summary>
    /// PEM 文件 RSA 密钥序列化器。
    /// </summary>
    public class PemFileRsaKeySerializer : AbstractService<PemFileRsaKeySerializer>, IRsaKeySerializer
    {
        private const string PUBLIC_KEY_CLAIM = "RSA PUBLIC KEY";
        private const string PRIVATE_KEY_CLAIM = "RSA PRIVATE KEY";


        /// <summary>
        /// 构造一个 <see cref="PemFileRsaKeySerializer"/> 实例。
        /// </summary>
        /// <param name="logger">给定的 <see cref="ILogger{PemFileRsaKeySerializer}"/>。</param>
        public PemFileRsaKeySerializer(ILogger<PemFileRsaKeySerializer> logger)
            : base(logger)
        {
            FileExtension = ".pem";
        }


        /// <summary>
        /// 文件扩展名。
        /// </summary>
        public virtual string FileExtension { get; }


        /// <summary>
        /// 反序列化。
        /// </summary>
        /// <param name="fileName">给定要读取的文件名。</param>
        /// <returns>返回 <see cref="RsaKeyParameters"/>。</returns>
        public virtual RsaKeyParameters Deserialize(string fileName)
        {
            RSAParameters parameters;
            Logger.LogInformation($"Load rsa key pem file: {fileName}");

            try
            {
                var content = File.ReadAllText(fileName);
                Logger.LogDebug($"Load rsa key: {content}");

                if (content.Contains(PUBLIC_KEY_CLAIM))
                {
                    // Public Key
                    parameters = FromPemPublicKeyString(content);
                }
                else if (content.Contains(PRIVATE_KEY_CLAIM))
                {
                    // Private Key
                    parameters = FromPemPrivateKeyString(content);
                }
                else
                {
                    // Not Support
                    parameters = new RSAParameters();
                }

                return new RsaKeyParameters
                {
                    KeyId = UniqueIdentifier.NewByRng(16).ToString(),
                    Parameters = parameters
                };
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.AsInnerMessage());
                throw ex;
            }
        }


        /// <summary>
        /// 序列化密钥参数。
        /// </summary>
        /// <param name="keyParameters">给定的 <see cref="RsaKeyParameters"/>。</param>
        /// <param name="fileName">给定要保存的文件名。</param>
        /// <returns>返回序列化字符串。</returns>
        public virtual string Serialize(RsaKeyParameters keyParameters, string fileName)
        {
            Logger.LogWarning("Save rsa key: no support");
            return string.Empty;
        }


        /// <summary>
        /// 从 PEM 格式的公钥字符串还原 RSA 参数。
        /// </summary>
        /// <param name="pemPublicKeyString">给定的 PEM 格式公钥字符串。</param>
        /// <returns>返回 <see cref="RSAParameters"/>。</returns>
        protected virtual RSAParameters FromPemPublicKeyString(string pemPublicKeyString)
        {
            pemPublicKeyString = pemPublicKeyString
                .Replace($"-----BEGIN {PUBLIC_KEY_CLAIM}-----", string.Empty)
                .Replace($"-----END {PUBLIC_KEY_CLAIM}-----", string.Empty);

            var x509Key = Convert.FromBase64String(pemPublicKeyString);

            // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
            byte[] seqOid = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];

            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            using (var ms = new MemoryStream(x509Key))
            using (var br = new BinaryReader(ms))  //wrap Memory Stream with BinaryReader for easy reading
            {
                byte bt = 0;
                ushort twoBytes = 0;

                twoBytes = br.ReadUInt16();
                if (twoBytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    br.ReadByte();    //advance 1 byte
                else if (twoBytes == 0x8230)
                    br.ReadInt16();   //advance 2 bytes
                else
                    return default;

                seq = br.ReadBytes(15);       //read the Sequence OID
                if (!CompareByteArrays(seq, seqOid))    //make sure Sequence for OID is correct
                    return default;

                twoBytes = br.ReadUInt16();
                if (twoBytes == 0x8103) //data read as little endian order (actual data order for Bit String is 03 81)
                    br.ReadByte();    //advance 1 byte
                else if (twoBytes == 0x8203)
                    br.ReadInt16();   //advance 2 bytes
                else
                    return default;

                bt = br.ReadByte();
                if (bt != 0x00)     //expect null byte next
                    return default;

                twoBytes = br.ReadUInt16();
                if (twoBytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    br.ReadByte();    //advance 1 byte
                else if (twoBytes == 0x8230)
                    br.ReadInt16();   //advance 2 bytes
                else
                    return default;

                twoBytes = br.ReadUInt16();
                byte lowbyte = 0x00;
                byte highbyte = 0x00;

                if (twoBytes == 0x8102) //data read as little endian order (actual data order for Integer is 02 81)
                {
                    lowbyte = br.ReadByte();  // read next bytes which is bytes in modulus
                }
                else if (twoBytes == 0x8202)
                {
                    highbyte = br.ReadByte(); //advance 2 bytes
                    lowbyte = br.ReadByte();
                }
                else
                {
                    return default;
                }

                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
                int modsize = BitConverter.ToInt32(modint, 0);

                int firstByte = br.PeekChar();
                if (firstByte == 0x00)
                {   //if first byte (highest order) of modulus is zero, don't include it
                    br.ReadByte();    //skip this null byte
                    modsize -= 1;   //reduce modulus buffer size by 1
                }

                byte[] modulus = br.ReadBytes(modsize);   //read the modulus bytes

                if (br.ReadByte() != 0x02)            //expect an Integer for the exponent data
                    return default;

                int expbytes = br.ReadByte();        // should only need one byte for actual exponent data (for all useful values)
                byte[] exponent = br.ReadBytes(expbytes);

                return new RSAParameters
                {
                    Modulus = modulus,
                    Exponent = exponent
                };
            }
        }

        /// <summary>
        /// 从 PEM 格式的私钥字符串还原 RSA 参数。
        /// </summary>
        /// <param name="pemPrivateKeyString">给定的 PEM 格式私钥字符串。</param>
        /// <returns>返回 <see cref="RSAParameters"/>。</returns>
        protected virtual RSAParameters FromPemPrivateKeyString(string pemPrivateKeyString)
        {
            pemPrivateKeyString = pemPrivateKeyString
                .Replace($"-----BEGIN {PRIVATE_KEY_CLAIM}-----", string.Empty)
                .Replace($"-----END {PRIVATE_KEY_CLAIM}-----", string.Empty);

            var buffer = Convert.FromBase64String(pemPrivateKeyString);
            var parameters = new RSAParameters();

            using (var ms = new MemoryStream(buffer))
            using (var br = new BinaryReader(ms))
            {
                byte b = 0;
                ushort twoBytes = 0;

                twoBytes = br.ReadUInt16();

                if (twoBytes == 0x8130)
                    br.ReadByte();
                else if (twoBytes == 0x8230)
                    br.ReadInt16();
                else
                    throw new Exception("Unexpected value read br.ReadUInt16()");

                twoBytes = br.ReadUInt16();
                if (twoBytes != 0x0102)
                    throw new Exception("Unexpected version");

                b = br.ReadByte();
                if (b != 0x00)
                    throw new Exception("Unexpected value read binr.ReadByte()");

                parameters.Modulus = br.ReadBytes(GetIntegerSize(br));
                parameters.Exponent = br.ReadBytes(GetIntegerSize(br));
                parameters.D = br.ReadBytes(GetIntegerSize(br));
                parameters.P = br.ReadBytes(GetIntegerSize(br));
                parameters.Q = br.ReadBytes(GetIntegerSize(br));
                parameters.DP = br.ReadBytes(GetIntegerSize(br));
                parameters.DQ = br.ReadBytes(GetIntegerSize(br));
                parameters.InverseQ = br.ReadBytes(GetIntegerSize(br));
            }

            return parameters;
        }

        /// <summary>
        /// 获取整数大小。
        /// </summary>
        /// <param name="br">给定的 <see cref="BinaryReader"/>。</param>
        /// <returns>返回整数大小。</returns>
        private static int GetIntegerSize(BinaryReader br)
        {
            byte b = 0;
            int count = 0;

            b = br.ReadByte();
            if (b != 0x02)
                return 0;

            b = br.ReadByte();

            if (b == 0x81)
            {
                count = br.ReadByte();
            }
            else if (b == 0x82)
            {
                var highByte = br.ReadByte();
                var lowByte = br.ReadByte();
                byte[] modint = { lowByte, highByte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = b;
            }

            while (br.ReadByte() == 0x00)
            {
                count -= 1;
            }

            br.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }

        /// <summary>
        /// 比较两个字节数组是否相等。
        /// </summary>
        /// <param name="a">给定的数组。</param>
        /// <param name="b">给定要比较的数组。</param>
        /// <returns>返回是否相等的布尔值。</returns>
        private static bool CompareByteArrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;

            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;

                i++;
            }

            return true;
        }

    }
}
