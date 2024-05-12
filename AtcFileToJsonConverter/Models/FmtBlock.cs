using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtcFileToJsonConverter.Models
{
    public struct FmtBlock
    {
        public byte Format;
        public ushort Frequency;
        public ushort Resolution;
        public byte Flags;
        public ushort Reserved;
    }
}