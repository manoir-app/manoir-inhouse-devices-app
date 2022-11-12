using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Java.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HomeAutomationServerApp
{
    internal class JSBridge : Java.Lang.Object
    {
        [JavascriptInterface]
        [Export("setApplication")]
        public void SetApplication(Java.Lang.String url)
        {
            SaveString("url", url.ToString());
        }

        [JavascriptInterface]
        [Export("saveDeviceId")]
        public void SaveDeviceId(Java.Lang.String deviceId)
        {
            SaveString("deviceId", deviceId.ToString());
        }

        [JavascriptInterface]
        [Export("getDeviceId")]
        public Java.Lang.String GetDeviceId()
        {
            return new Java.Lang.String(GetString("deviceId"));

        }

        public static string GetApplication()
        {
            var t = GetString("url");
            if (t == null)
                return "https://home.anzin.carbenay.manoir.app/devicehome/"; // pour l'instant
            return t;
        }

        public static void Init(Context ctx)
        {
            preferences = ctx.GetSharedPreferences("myapp.settings", FileCreationMode.Private);
        }

        private static ISharedPreferences preferences;
        private const string initVector = "manoirhomeautoma";
        private const string passPhrase = "thisisalongphraseusedtogetrandomthings";
        private const int keysize = 256;

        public static string GetString(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if (!preferences.Contains(key))
            {
                return null;
            }

            return DecryptString(preferences.GetString(key, string.Empty));
            //return null;
        }

        public static void SaveString(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            var editor = preferences.Edit();
            if (value == null)
            {
                if (preferences.Contains(key))
                    editor.Remove(key);
            }
            else
            {
                var encryptedValue = EncryptString(value);
                editor.PutString(key, encryptedValue);
            }
            editor.Apply();
        }

        // basic encryption, not for real security
        // but for not letting everything unencrypted
        // on persistent memory
        private static string EncryptString(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }

        private static string DecryptString(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return string.Empty;

            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
    }
}