using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtcFileToJsonConverter.Models
{
    public class AtcFileHeader
    {
        public byte[] FileSignature; // 8 bytes
        public uint FileVersion;
    }
}