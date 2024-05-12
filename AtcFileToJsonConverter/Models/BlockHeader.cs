using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtcFileToJsonConverter.Models
{
    public struct BlockHeader
    {
        public byte[] BlockId; // 4 bytes
        public uint Length;
    }
}