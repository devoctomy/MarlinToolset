using System;

namespace MarlinToolset.Core.Services
{
    public class CommandParameter
    {
        public string Token { get; set; }
        public string Description { get; set; }
        public string ValueType { get; set; }
        public object Value { get; set; }
        public bool Optional { get; set; }
        public string Requires { get; set; }
        public object DefaultValue { get; set; }
        public string Choices { get; set; }

        public void SetValue(string value)
        {
            if(ValueType != null)
            {
                if (ValueType.Equals("int", StringComparison.InvariantCultureIgnoreCase))
                {
                    Value = string.IsNullOrWhiteSpace(value) ? DefaultValue : int.Parse(value);
                }
                else if (ValueType.Equals("float", StringComparison.InvariantCultureIgnoreCase))
                {
                    Value = string.IsNullOrWhiteSpace(value) ? DefaultValue : float.Parse(value);
                }
            }
            else
            {
                Value = bool.Parse(value);
            }
        }
    }
}
