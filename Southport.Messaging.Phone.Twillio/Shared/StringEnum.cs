using System;

namespace Southport.Messaging.Phone.Vonage.Shared
{
    public abstract class StringEnum
    {
        private string _value;

        /// <summary>Generic constructor</summary>
        protected StringEnum()
        {
        }

        /// <summary>Create from string</summary>
        /// <param name="value">String value</param>
        protected StringEnum(string value) => _value = value;

        /// <summary>Generate from string</summary>
        /// <param name="value">String value</param>
        public void FromString(string value) => _value = value;

        /// <summary>Convert to string</summary>
        /// <returns>String representation</returns>
        public override string ToString() => _value;

        public override int GetHashCode() => _value.GetHashCode();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            StringEnum stringEnum = (StringEnum) Convert.ChangeType(obj, GetType());
            return !(stringEnum == (StringEnum) null) && stringEnum._value.Equals(_value);
        }

        public static bool operator ==(StringEnum a, StringEnum b)
        {
            if ((object) a == (object) b)
            {
                return true;
            }

            return (object) a != null && (object) b != null && a.Equals(b);
        }

        public static bool operator != (StringEnum a, StringEnum b) => !(a == b);
    }
}
