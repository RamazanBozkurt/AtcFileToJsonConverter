using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtcFileToJsonConverter.Models
{
    public class InfoBlock
    {
        public byte[] DateRecorded; // 32 bytes
        public byte[] RecordingUUID; // 40 bytes
        public byte[] PhoneUDID; // 44 bytes
        public byte[] PhoneModel; // 32 bytes
        public byte[] RecorderSoftware; // 32 bytes
        public byte[] RecorderHardware; // 32 bytes
        public byte[] Location; // 52 bytes

        public InfoBlock()
        {
            DateRecorded = new byte[32];
            RecordingUUID = new byte[40];
            PhoneUDID = new byte[44];
            PhoneModel = new byte[32];
            RecorderSoftware = new byte[32];
            RecorderHardware = new byte[32];
            Location = new byte[52];
        }
    }
}