using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtcFileToJsonConverter.Models
{
    public class DecodedInfoBlock
    {
        public string DateRecorded;
        public string RecordingUUID;
        public string PhoneUDID;
        public string PhoneModel;
        public string RecorderSoftware;
        public string RecorderHardware;
        public string Location;

        public DecodedInfoBlock(InfoBlock info)
        {
            DateRecorded = System.Text.Encoding.UTF8.GetString(info.DateRecorded);
            RecordingUUID = System.Text.Encoding.UTF8.GetString(info.RecordingUUID);
            PhoneUDID = System.Text.Encoding.UTF8.GetString(info.PhoneUDID);
            PhoneModel = System.Text.Encoding.UTF8.GetString(info.PhoneModel);
            RecorderSoftware = System.Text.Encoding.UTF8.GetString(info.RecorderSoftware);
            RecorderHardware = System.Text.Encoding.UTF8.GetString(info.RecorderHardware);
            Location = System.Text.Encoding.UTF8.GetString(info.Location);
            //Location = System.Text.Encoding.GetEncoding("ISO - 8859 - 1").GetString(info.Location);
        }
    }
}