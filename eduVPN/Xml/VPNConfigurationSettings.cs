﻿/*
    eduVPN - End-user friendly VPN

    Copyright: 2017, The Commons Conservancy eduVPN Programme
    SPDX-License-Identifier: GPL-3.0+
*/

using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace eduVPN.Xml
{
    /// <summary>
    /// VPN configuration as persisted to settings base class
    /// </summary>
    [Obsolete]
    public class VPNConfigurationSettings : IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// Popularity factor in the [0.0, 1.0] range (default 0.5)
        /// </summary>
        public float Popularity { get; set; } = 0.5f;

        #endregion

        #region Methods

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return true;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region IXmlSerializable Support

        public XmlSchema GetSchema()
        {
            return null;
        }

        public virtual void ReadXml(XmlReader reader)
        {
            string v;

            Popularity = (v = reader[nameof(Popularity)]) != null && float.TryParse(v, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var v_popularity) ? Popularity = v_popularity : 1.0f;
        }

        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString(nameof(Popularity), Popularity.ToString(CultureInfo.InvariantCulture));
        }

        #endregion
    }
}