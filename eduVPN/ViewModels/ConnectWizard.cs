﻿/*
    eduVPN - End-user friendly VPN

    Copyright: 2017, The Commons Conservancy eduVPN Programme
    SPDX-License-Identifier: GPL-3.0+
*/

using eduOAuth;
using Prism.Mvvm;
using System;
using System.Threading;
using System.Windows.Threading;

namespace eduVPN.ViewModels
{
    /// <summary>
    /// Connect wizard
    /// </summary>
    public class ConnectWizard : BindableBase, IDisposable
    {
        #region Properties

        /// <summary>
        /// UI thread's dispatcher
        /// </summary>
        /// <remarks>
        /// Background threads must raise property change events in the UI thread.
        /// </remarks>
        public Dispatcher Dispatcher { get; }

        /// <summary>
        /// Token used to abort unfinished background processes in case of application shutdown.
        /// </summary>
        public static CancellationTokenSource Abort { get => _abort; }
        private static CancellationTokenSource _abort = new CancellationTokenSource();

        /// <summary>
        /// Selected instance group
        /// </summary>
        public Models.InstanceGroupInfo InstanceGroup
        {
            get { return _instance_group; }
            set { _instance_group = value; RaisePropertyChanged(); }
        }
        private Models.InstanceGroupInfo _instance_group;

        /// <summary>
        /// Authenticating eduVPN instance
        /// </summary>
        /// <remarks><c>null</c> if none selected.</remarks>
        public Models.InstanceInfo AuthenticatingInstance
        {
            get { return _authenticating_instance; }
            set { _authenticating_instance = value; RaisePropertyChanged(); }
        }
        private Models.InstanceInfo _authenticating_instance;

        /// <summary>
        /// OAuth access token
        /// </summary>
        public AccessToken AccessToken
        {
            get { return _access_token; }
            set { _access_token = value; RaisePropertyChanged(); }
        }
        private AccessToken _access_token;

        /// <summary>
        /// Connecting eduVPN instance
        /// </summary>
        /// <remarks><c>null</c> if none selected.</remarks>
        public Models.InstanceInfo ConnectingInstance
        {
            get { return _connecting_instance; }
            set { _connecting_instance = value; RaisePropertyChanged(); }
        }
        private Models.InstanceInfo _connecting_instance;

        /// <summary>
        /// Connecting eduVPN instance profile
        /// </summary>
        /// <remarks><c>null</c> if none selected.</remarks>
        public Models.ProfileInfo ConnectingProfile
        {
            get { return _connecting_profile; }
            set { _connecting_profile = value; RaisePropertyChanged(); }
        }
        private Models.ProfileInfo _connecting_profile;

        #region Pages

        /// <summary>
        /// The page the wizard is currently displaying
        /// </summary>
        public ConnectWizardPage CurrentPage
        {
            get { return _current_page; }
            set
            {
                _current_page = value;
                RaisePropertyChanged();
                _current_page.OnActivate();
            }
        }
        private ConnectWizardPage _current_page;

        /// <summary>
        /// Access type page
        /// </summary>
        public AccessTypePage AccessTypePage
        {
            get
            {
                if (_access_type_page == null)
                    _access_type_page = new AccessTypePage(this);
                return _access_type_page;
            }
        }
        private AccessTypePage _access_type_page;

        /// <summary>
        /// Instance selection page
        /// </summary>
        public InstanceSelectPage InstanceSelectPage
        {
            get
            {
                if (_instance_select_page == null)
                    _instance_select_page = new InstanceSelectPage(this);
                return _instance_select_page;
            }
        }
        private InstanceSelectPage _instance_select_page;

        /// <summary>
        /// Custom instance page
        /// </summary>
        public CustomInstancePage CustomInstancePage
        {
            get
            {
                if (_custom_instance_page == null)
                    _custom_instance_page = new CustomInstancePage(this);
                return _custom_instance_page;
            }
        }
        private CustomInstancePage _custom_instance_page;

        /// <summary>
        /// Authorization wizard page
        /// </summary>
        public AuthorizationPage AuthorizationPage
        {
            get
            {
                if (_authorization_page == null)
                    _authorization_page = new AuthorizationPage(this);
                return _authorization_page;
            }
        }
        private AuthorizationPage _authorization_page;

        /// <summary>
        /// Profile selection wizard page
        /// </summary>
        public ProfileSelectPage ProfileSelectPage
        {
            get
            {
                if (_profile_select_page == null)
                    _profile_select_page = new ProfileSelectPage(this);
                return _profile_select_page;
            }
        }
        private ProfileSelectPage _profile_select_page;

        /// <summary>
        /// Instance and profile selection wizard page (for federated authentication)
        /// </summary>
        public InstanceAndProfileSelectPage InstanceAndProfileSelectPage
        {
            get
            {
                if (_instance_and_profile_select_page == null)
                    _instance_and_profile_select_page = new InstanceAndProfileSelectPage(this);
                return _instance_and_profile_select_page;
            }
        }
        private InstanceAndProfileSelectPage _instance_and_profile_select_page;

        /// <summary>
        /// Status wizard page
        /// </summary>
        public StatusPage StatusPage
        {
            get
            {
                if (_status_page == null)
                    _status_page = new StatusPage(this);
                return _status_page;
            }
        }
        private StatusPage _status_page;

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs the wizard
        /// </summary>
        public ConnectWizard()
        {
            // Save UI thread's dispatcher.
            Dispatcher = Dispatcher.CurrentDispatcher;

            Dispatcher.ShutdownStarted += (object sender, EventArgs e) => {
                // Raise the abort flag to gracefully shutdown all background threads.
                Abort.Cancel();

                // Persist settings to disk.
                Properties.Settings.Default.Save();
            };

            CurrentPage = AccessTypePage;
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_status_page != null)
                        _status_page.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
