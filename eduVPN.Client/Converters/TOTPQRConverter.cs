﻿/*
    eduVPN - End-user friendly VPN

    Copyright: 2017, The Commons Conservancy eduVPN Programme
    SPDX-License-Identifier: GPL-3.0+
*/

using eduVPN.Models;
using QRCoder;
using System;
using System.Globalization;
using System.Web;
using System.Windows.Data;

namespace eduVPN.Client.Converters
{
    /// <summary>
    /// Returns TOTP QR code generated from TOTP secret and authenticating instance host name and display name.
    /// </summary>
    public class TOTPQRConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var secret   = values[0] as string;
                var user     = values[1] as UserInfo;
                var instance = values[2] as Instance;
                var otp_uri = string.Format(user.ID != null ? "otpauth://totp/{1}:{2}?secret={0}&issuer={3}" : "otpauth://totp/{1}?secret={0}&issuer={3}",
                    secret,
                    instance.Base.Host,
                    user.ID,
                    HttpUtility.UrlEncode(instance.ToString()));

                var qr_generator = new QRCodeGenerator();
                var qr_code_data = qr_generator.CreateQrCode(otp_uri, QRCodeGenerator.ECCLevel.Q);
                var qr_code = new XamlQRCode(qr_code_data);
                return qr_code.GetGraphic(3, true);
            }
            catch { }

            // Fallback to blank image.
            return new Uri("pack://application:,,,/Resources/Blank.png");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}