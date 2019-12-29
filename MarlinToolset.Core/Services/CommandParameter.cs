using System;

namespace MarlinToolset.Core.Services
{
    public class CommandParameter
    {
        public string Token { get; }
        public Type ValueType { get; }
        public object Value { get; set; }
        public bool Optional { get; }
        public string Description { get; }

        public CommandParameter(
            string token)
        {
            Token = token;
        }

        public CommandParameter(
            string token,
            bool optional)
        {
            Token = token;
            Optional = optional;
        }

        public CommandParameter(
            string token,
            string description,
            bool optional)
        {
            Token = token;
            Description = description;
            Optional = optional;
        }

        public CommandParameter(
            string token,
            string description,
            Type valueType,
            bool optional)
        {
            Token = token;
            Description = description;
            ValueType = valueType;
            Optional = optional;
        }

        public CommandParameter(
            string token,
            string description,
            Type valueType,
            object value,
            bool optional)
        {
            Token = token;
            Description = description;
            ValueType = valueType;
            Value = value;
            Optional = optional;
        }

        public void SetValue(string value)
        {
            if(ValueType == typeof(int))
            {
                Value = int.Parse(value);
            }
            else if (ValueType == typeof(float))
            {
                Value = float.Parse(value);
            }
            else if (ValueType == typeof(bool))
            {
                Value = bool.Parse(value);
            }
        }
    }
}
