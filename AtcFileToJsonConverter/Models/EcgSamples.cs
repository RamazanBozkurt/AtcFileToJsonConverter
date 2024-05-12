using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtcFileToJsonConverter.Models
{
    public class EcgSamples
    {
        public short[] LeadI { get; set; }
        public short[] LeadII { get; set; }
        public short[] LeadIII { get; set; }
        public short[] AVR { get; set; }
        public short[] AVL { get; set; }
        public short[] AVF { get; set; }
    }
}