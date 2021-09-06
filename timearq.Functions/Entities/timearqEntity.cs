using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace timearq.Functions.Entities
{
   public class timearqEntity : TableEntity
    {
        public int IdEmployee { get; set; }

        public DateTime Register { get; set; }

        public int Type { get; set; }

        public bool Consolidated { get; set; }
    }
}
