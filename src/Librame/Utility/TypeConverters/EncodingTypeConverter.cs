﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Librame.Utility.TypeConverters
{
    /// <summary>
    /// <see cref="Encoding"/> 类型转换器。
    /// </summary>
    public class EncodingTypeConverter : TypeConverter
    {
        /// <summary>
        /// 能否转换为字符串。
        /// </summary>
        /// <param name="context">给定的 <see cref="ITypeDescriptorContext"/>。</param>
        /// <param name="destinationType">给定的目标类型。</param>
        /// <returns>返回布尔值。</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <param name="context">给定的 <see cref="ITypeDescriptorContext"/>。</param>
        /// <param name="culture">给定的 <see cref="CultureInfo"/>。</param>
        /// <param name="value">给定的 <see cref="Encoding"/>。</param>
        /// <param name="destinationType">给定的目标类型。</param>
        /// <returns>返回字符串。</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // check that the value we got passed on is of type Encoding
            if (value != null)
            {
                if (!(value is Encoding))
                    throw new Exception(string.Format(Properties.Resources.TypeNoSupportConversionExceptionFormat, value.GetType()));
            }

            // convert to a string
            if (destinationType == typeof(string))
            {
                // no value so we return an empty string
                if (value == null)
                    return string.Empty;

                // strongly typed
                var encoding = (value as Encoding);

                // convert to a string and return
                return encoding.EncodingName;
            }

            // call the base converter
            return base.ConvertTo(context, culture, value, destinationType);
        }


        /// <summary>
        /// 能否还原为 <see cref="Encoding"/>。
        /// </summary>
        /// <param name="context">给定的 <see cref="ITypeDescriptorContext"/>。</param>
        /// <param name="sourceType">给定的来源类型。</param>
        /// <returns>返回布尔值。</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// 还原为 <see cref="Encoding"/>。
        /// </summary>
        /// <param name="context">给定的 <see cref="ITypeDescriptorContext"/>。</param>
        /// <param name="culture">给定的 <see cref="CultureInfo"/>。</param>
        /// <param name="value">给定的字符串值。</param>
        /// <returns>返回 <see cref="Encoding"/>。</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var defaultEncoding = Encoding.UTF8;

            // no value so we return a new Encoding instance
            if (value == null)
                return defaultEncoding;

            // convert from a string
            if (value is string)
            {
                // get strongly typed value
                string str = (value as string);

                // empty string so we return a new Encoding instance
                if (str.Length <= 0)
                    return defaultEncoding;

                try
                {
                    // create a new Encoding instance with these values and return it
                    return Encoding.GetEncoding(str);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            // otherwise call the base converter
            return base.ConvertFrom(context, culture, value);
        }

    }
}
