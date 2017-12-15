﻿/*
    eduVPN - End-user friendly VPN

    Copyright: 2017, The Commons Conservancy eduVPN Programme
    SPDX-License-Identifier: GPL-3.0+
*/

using eduVPN.ViewModels.Windows;

namespace eduVPN.ViewModels.Pages
{
    /// <summary>
    /// Institute selection wizard page
    /// </summary>
    public class AuthenticatingInstituteSelectPage : AuthenticatingInstanceSelectPage
    {
        #region Constructors

        /// <summary>
        /// Constructs an institute selection wizard page.
        /// </summary>
        /// <param name="wizard">The connecting wizard</param>
        public AuthenticatingInstituteSelectPage(ConnectWizard wizard) :
            base(wizard)
        {
        }

        #endregion
    }
}