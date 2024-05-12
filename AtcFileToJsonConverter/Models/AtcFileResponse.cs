using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtcFileToJsonConverter.Models
{
    public class AtcFileResponse
    {
        public EcgData EcgData { get; set; }
        public DecodedInfoBlock DecodedInfoBlock { get; set; }
    }
}