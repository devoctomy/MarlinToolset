using System;
using System.Collections.Generic;
using System.Text;

namespace MarlinToolset.Core.Services
{
    public class CommandParameter
    {
        public string Token { get; }
        public string Value { get; set; }
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
            string value,
            bool optional)
        {
            Token = token;
            Description = description;
            Value = value;
            Optional = optional;
        }
    }
}
