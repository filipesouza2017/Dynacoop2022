using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject.Models
{
    public class Enumerator
    {
        public enum PluginStages
        {
            PreValidation = 10,
            PreOperation = 20,
            PostOperation = 30
        }
    }
}
