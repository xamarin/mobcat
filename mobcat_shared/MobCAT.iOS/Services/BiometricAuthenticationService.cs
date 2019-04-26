﻿using System;
using System.Threading.Tasks;
using Foundation;
using LocalAuthentication;
using Microsoft.MobCAT.Abstractions;
using Microsoft.MobCAT.Models;
using UIKit;

namespace Microsoft.MobCAT.iOS.Services
{
    public class BiometricAuthenticationService : IBiometricAuthenticationService
    {
        private bool _hasEvaluatedBiometricType;
        private BiometricType _biometricType;

        /// <inheritdoc />
        public Task<AuthenticationResult> AuthenticateAsync(string alertMessage = null)
        {
            if (AvailableBiometricType == BiometricType.None)
            {
                Logger.Debug("[BiometricAthenticationService] Authentication not available on this device");
                return Task.FromResult(new AuthenticationResult(false, "Authentication not available"));
            }

            var tcs = new TaskCompletionSource<AuthenticationResult>();

            var context = new LAContext();
            NSError authError;

            if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out authError)
                || context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthentication, out authError))
            {
                var replyHandler = new LAContextReplyHandler((success, error) =>
                {
                    //Make sure it runs on MainThread, not in Background
                    UIApplication.SharedApplication.InvokeOnMainThread(() =>
                    {
                        if (success)
                        {
                            System.Diagnostics.Debug.WriteLine("Authentication Success");
                            tcs.TrySetResult(new AuthenticationResult(true));
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Authentication Failure : " + error.Description);
                            tcs.TrySetResult(new AuthenticationResult(false, error.Description));
                        }
                    });
                });

                context.EvaluatePolicy(LAPolicy.DeviceOwnerAuthentication, alertMessage ?? "Please scan fingerprint", replyHandler);
            }
            else
            {
                //No Auth setup on Device
                Logger.Debug($"This device doesn't have authentication configured: {authError.ToString()}");
                tcs.TrySetResult(new AuthenticationResult(false, "This device does't have authentication configured."));
            }

            return tcs.Task;
        }

        /// <inheritdoc />
        public BiometricType AvailableBiometricType
        {
            get
            {
                if (_hasEvaluatedBiometricType) return _biometricType;

                var localAuthContext = new LAContext();
                NSError authError;

                if (localAuthContext.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthentication, out authError))
                {
                    if (localAuthContext.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out authError))
                    {
                        if (GetOsMajorVersion() >= 11 && localAuthContext.BiometryType == LABiometryType.FaceId)
                        {
                            _biometricType = BiometricType.Face;
                            _hasEvaluatedBiometricType = true;
                            return _biometricType;
                        }

                        _biometricType = BiometricType.Fingerprint;
                        _hasEvaluatedBiometricType = true;
                        return _biometricType;
                    }

                    _biometricType = BiometricType.Passcode;
                    _hasEvaluatedBiometricType = true;
                    return _biometricType;
                }

                Logger.Debug($"[BiometricAuthenticationService] Local Auth context failed with error: {authError}");

                _biometricType = BiometricType.None;
                _hasEvaluatedBiometricType = true;
                return _biometricType;
            }
        }

        private int GetOsMajorVersion()
        {
            return int.Parse(UIDevice.CurrentDevice.SystemVersion.Split('.')[0]);
        }
    }
}
