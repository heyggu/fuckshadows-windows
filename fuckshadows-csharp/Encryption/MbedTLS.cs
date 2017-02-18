﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using Fuckshadows.Controller;
using Fuckshadows.Properties;
using Fuckshadows.Util;

namespace Fuckshadows.Encryption
{
    public class MbedTLS
    {
        const string DLLNAME = "libfscrypto";

        public const int MBEDTLS_ENCRYPT = 1;
        public const int MBEDTLS_DECRYPT = 0;

        static MbedTLS()
        {
            string dllPath = Utils.GetTempPath("libfscrypto.dll");
            try
            {
                FileManager.UncompressFile(dllPath, Resources.libfscrypto_dll);
            }
            catch (IOException)
            {
            }
            catch (Exception e)
            {
                Logging.LogUsefulException(e);
            }
            LoadLibrary(dllPath);
        }

        public static byte[] MD5(byte[] input)
        {
            byte[] output = new byte[16];
            md5(input, (uint) input.Length, output);
            return output;
        }

        [DllImport("Kernel32.dll")]
        private static extern IntPtr LoadLibrary(string path);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void md5(byte[] input, uint ilen, byte[] output);

        /// <summary>
        /// Get cipher ctx size for memory allocation
        /// </summary>
        /// <returns></returns>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int cipher_get_size_ex();

        #region Cipher layer wrappers

        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr cipher_info_from_string(string cipher_name);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void cipher_init(IntPtr ctx);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int cipher_setup(IntPtr ctx, IntPtr cipher_info);

        // XXX: Check operation before using it
        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int cipher_setkey(IntPtr ctx, byte[] key, int key_bitlen, int operation);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int cipher_set_iv(IntPtr ctx, byte[] iv, int iv_len);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int cipher_reset(IntPtr ctx);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int cipher_update(IntPtr ctx, byte[] input, int ilen, byte[] output, ref int olen);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void cipher_free(IntPtr ctx);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int cipher_auth_encrypt(IntPtr ctx,
            byte[] iv, int iv_len,
            IntPtr ad, int ad_len,
            byte[] input, int ilen,
            byte[] output, ref int olen,
            byte[] tag, int tag_len);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int cipher_auth_decrypt(IntPtr ctx,
            byte[] iv, int iv_len,
            IntPtr ad, int ad_len,
            byte[] input, int ilen,
            byte[] output, ref int olen,
            byte[] tag, int tag_len);

        #endregion
    }
}