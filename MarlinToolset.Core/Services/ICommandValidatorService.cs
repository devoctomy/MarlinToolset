using System;
using System.Collections.Generic;
using System.Text;

namespace MarlinToolset.Core.Services
{
    public interface ICommandValidatorService
    {
        public void Validate(
            CommandDefinition commandDefinition,
            string commandText);
    }
}
