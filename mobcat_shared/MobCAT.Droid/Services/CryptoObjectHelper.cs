using System;
using Android.Security.Keystore;
using Android.Support.V4.Hardware.Fingerprint;
using Android.Util;
using Java.Security;
using Javax.Crypto;

namespace Microsoft.MobCAT.Droid.Services
{
    internal class CryptoObjectHelper
    {
        readonly string _keyName = "BasicFingerPrintSample.FingerprintManagerAPISample.sample_fingerprint_key";
        readonly string _keystoreName = "AndroidKeyStore";
        readonly string _keyAlgorithm = KeyProperties.KeyAlgorithmAes;
        readonly string _blockMode = KeyProperties.BlockModeCbc;
        readonly string _encryptionPadding = KeyProperties.EncryptionPaddingPkcs7;

        private string Transformation => _keyAlgorithm + "/" +
                                         _blockMode + "/" +
                                         _encryptionPadding;

        readonly KeyStore _keystore;

        public CryptoObjectHelper()
        {
            _keystore = KeyStore.GetInstance(_keystoreName);
            _keystore.Load(null);
        }

        public FingerprintManagerCompat.CryptoObject BuildCryptoObject()
        {
            Cipher cipher = CreateCipher();
            return new FingerprintManagerCompat.CryptoObject(cipher);
        }

        /// <summary>
        ///     Creates the cipher.
        /// </summary>
        /// <returns>The cipher.</returns>
        /// <param name="retry">If set to <c>true</c>, recreate the key and try again.</param>
        Cipher CreateCipher(bool retry = true)
        {
            IKey key = GetKey();
            Cipher cipher = Cipher.GetInstance(Transformation);
            try
            {
                cipher.Init(CipherMode.EncryptMode, key);
            }
            catch (KeyPermanentlyInvalidatedException e)
            {
                Logger.Debug("[CryptoHelper] The key was invalidated, creating a new key.");
                _keystore.DeleteEntry(_keyName);
                if (retry)
                {
                    CreateCipher(false);
                }
                else
                {
                    throw new Exception("Could not create the cipher for fingerprint authentication.", e);
                }
            }

            return cipher;
        }

        /// <summary>
        ///     Will get the key from the Android keystore, creating it if necessary.
        /// </summary>
        /// <returns></returns>
        IKey GetKey()
        {
            if (!_keystore.IsKeyEntry(_keyName))
            {
                CreateKey();
            }

            IKey secretKey = _keystore.GetKey(_keyName, null);
            return secretKey;
        }

        /// <summary>
        ///     Creates the Key for fingerprint authentication.
        /// </summary>
        void CreateKey()
        {
            KeyGenerator keyGen = KeyGenerator.GetInstance(KeyProperties.KeyAlgorithmAes, _keystoreName);
            KeyGenParameterSpec keyGenSpec =
                new KeyGenParameterSpec.Builder(_keyName, KeyStorePurpose.Encrypt | KeyStorePurpose.Decrypt)
                    .SetBlockModes(_blockMode)
                    .SetEncryptionPaddings(_encryptionPadding)
                    .SetUserAuthenticationRequired(true)
                    .Build();
            keyGen.Init(keyGenSpec);
            keyGen.GenerateKey();
            Logger.Debug("[CryptoHelper] New key created for fingerprint authentication.");
        }
    }
}
