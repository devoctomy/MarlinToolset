using System;

namespace MarlinToolset.Core.Services
{
    public class CommandParameter
    {
        public string Token { get; set; }
        public string Description { get; set; }
        public Type ValueType { get; set; }
        public object Value { get; set; }
        public bool Optional { get; set; }
        public string Requires { get; set; }

        public void SetValue(string value)
        {
            if(ValueType != null)
            {
                if (ValueType == typeof(int))
                {
                    Value = int.Parse(value);
                }
                else if (ValueType == typeof(float))
                {
                    Value = float.Parse(value);
                }
            }
            else
            {
                Value = bool.Parse(value);
            }
        }
    }
}
