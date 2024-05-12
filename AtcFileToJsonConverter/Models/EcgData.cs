using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtcFileToJsonConverter.Models
{
    public class EcgData
    {
        public float Frequency { get; set; }
        public int AmplitudeResolution { get; set; }
        public int MainsFrequency { get; set; }
        public float Gain { get; set; }
        public EcgSamples Samples { get; set; }
        public InfoBlock Info { get; set; }
        public string Error { get; set; }
    }
}
