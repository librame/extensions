#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Algorithm
{
    /// <summary>
    /// 哈希代码助手。
    /// </summary>
    public class HashCodeHelper
    {
        /// <summary>
        /// 转换为 BKDR。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回 BKDR 哈希代码。</returns>
        public static int ToBkdr(string str)
        {
            int seed = 131; // 31 131 1313 13131 131313 etc..  
            int hash = 0;
            int count;

            var buffer = str.ToCharArray();
            count = buffer.Length;

            while (count > 0)
            {
                hash = hash * seed + (buffer[buffer.Length - count]);
                count--;
            }

            return (hash & 0x7FFFFFFF);
        }


        /// <summary>
        /// 转换为 AP。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回 AP 哈希代码。</returns>
        public static int ToAp(string str)
        {
            int hash = 0;
            int i;
            int count;

            var buffer = str.ToCharArray();
            count = buffer.Length;

            for (i = 0; i < count; i++)
            {
                if ((i & 1) == 0)
                    hash ^= ((hash << 7) ^ (buffer[i]) ^ (hash >> 3));
                else
                    hash ^= (~((hash << 11) ^ (buffer[i]) ^ (hash >> 5)));

                count--;
            }

            return (hash & 0x7FFFFFFF);
        }


        /// <summary>
        /// 转换为 SDBM。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回 SDBM 哈希代码。</returns>
        public static int ToSdbm(string str)
        {
            int hash = 0;
            //int i;
            int count;

            var buffer = str.ToCharArray();
            count = buffer.Length;

            while (count > 0)
            {
                // equivalent to: hash = 65599*hash + (*str++);  
                hash = (buffer[buffer.Length - count]) + (hash << 6) + (hash << 16) - hash;
                count--;
            }

            return (hash & 0x7FFFFFFF);
        }


        /// <summary>
        /// 转换为 RS。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回 RS 哈希代码。</returns>
        public static int ToRs(string str)
        {
            int b = 378551;
            int a = 63689;
            int hash = 0;

            //int i;
            int count;
            var buffer = str.ToCharArray();
            count = buffer.Length;

            while (count > 0)
            {
                hash = hash * a + (buffer[buffer.Length - count]);
                a *= b;
                count--;
            }

            return (hash & 0x7FFFFFFF);
        }


        /// <summary>
        /// 转换为 JS。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回 JS 哈希代码。</returns>
        public static int ToJs(string str)
        {
            int hash = 1315423911;
            int count;

            var buffer = str.ToCharArray();
            count = buffer.Length;

            while (count > 0)
            {
                hash ^= ((hash << 5) + (buffer[buffer.Length - count]) + (hash >> 2));
                count--;
            }

            return (hash & 0x7FFFFFFF);
        }


        /// <summary>
        /// 转换为 P.J. Weinberger。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回 P.J. Weinberger 哈希代码。</returns>
        public static int ToWeinberger(string str)
        {
            int BitsInUnignedInt = (sizeof(int) * 8);
            int ThreeQuarters = ((BitsInUnignedInt * 3) / 4);
            int OneEighth = (BitsInUnignedInt / 8);
            int hash = 0;

            unchecked
            {
                int HighBits = (int)(0xFFFFFFFF) << (BitsInUnignedInt - OneEighth);
                int test = 0;
                int count;
                char[] bitarray = str.ToCharArray();
                count = bitarray.Length;
                while (count > 0)
                {
                    hash = (hash << OneEighth) + (bitarray[bitarray.Length - count]);
                    if ((test = hash & HighBits) != 0)
                    {
                        hash = ((hash ^ (test >> ThreeQuarters)) & (~HighBits));
                    }
                    count--;
                }
            }

            return (hash & 0x7FFFFFFF);
        }


        /// <summary>
        /// 转换为 ELF。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回 ELF 哈希代码。</returns>
        public static int ToElf(string str)
        {
            int hash = 0;
            int x = 0;

            //int i;
            int count;
            var buffer = str.ToCharArray();
            count = buffer.Length;

            unchecked
            {
                while (count > 0)
                {
                    hash = (hash << 4) + (buffer[buffer.Length - count]);
                    if ((x = hash & (int)0xF0000000) != 0)
                    {
                        hash ^= (x >> 24);
                        hash &= ~x;
                    }
                    count--;
                }
            }

            return (hash & 0x7FFFFFFF);
        }


        /// <summary>
        /// 转换为 DJB。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回 DJB 哈希代码。</returns>
        public static int ToDjb(string str)
        {
            int hash = 5381;
            //int i;
            int count;

            var buffer = str.ToCharArray();
            count = buffer.Length;

            while (count > 0)
            {
                hash += (hash << 5) + (buffer[buffer.Length - count]);
                count--;
            }

            return (hash & 0x7FFFFFFF);
        }

    }
}
