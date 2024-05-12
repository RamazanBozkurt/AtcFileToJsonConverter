using AtcFileToJsonConverter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AtcFileToJsonConverter
{
    public class AtcParser
    {
        private const int _checkSumLength = 4;
        private readonly byte[] _atcFileSignature = new byte[] { (byte)'A', (byte)'L', (byte)'I', (byte)'V', (byte)'E', 0, 0, 0 };
        public EcgData Parse(byte[] atcData)
        {
            string error = string.Empty;
            using var reader = new BinaryReader(new MemoryStream(atcData));

            var header = new AtcFileHeader();
            header.FileSignature = reader.ReadBytes(8);
            header.FileVersion = reader.ReadUInt32();

            if (!CompareByteArrays(header.FileSignature, _atcFileSignature))
            {
                throw new InvalidDataException("Wrong file signature");
            }

            BlockHeader blockHeader;

            var leadISamples = new short[0];
            var leadIISamples = new short[0];
            var leadIIISamples = new short[0];
            var aVRSamples = new short[0];
            var aVLSamples = new short[0];
            var aVFSamples = new short[0];
            FmtBlock fmtBlock = new FmtBlock();
            InfoBlock infoBlock = null;

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                long blockStart = reader.BaseStream.Position;

                blockHeader.BlockId = reader.ReadBytes(4);
                blockHeader.Length = reader.ReadUInt32();

                string blockType = Encoding.ASCII.GetString(blockHeader.BlockId);

                switch (blockType)
                {
                    case "fmt ":
                        fmtBlock = new FmtBlock();
                        fmtBlock.Format = reader.ReadByte();
                        fmtBlock.Frequency = reader.ReadUInt16();
                        fmtBlock.Resolution = reader.ReadUInt16();
                        fmtBlock.Flags = reader.ReadByte();
                        fmtBlock.Reserved = reader.ReadUInt16();
                        VerifyChecksum(atcData, blockStart, blockHeader.Length, reader);

                        break;
                    case "info":
                        infoBlock = new InfoBlock();
                        infoBlock.DateRecorded = GetBytesWithoutNull(reader.ReadBytes(32));
                        infoBlock.RecordingUUID = GetBytesWithoutNull(reader.ReadBytes(40));
                        infoBlock.PhoneUDID = GetBytesWithoutNull(reader.ReadBytes(44));
                        infoBlock.PhoneModel = GetBytesWithoutNull(reader.ReadBytes(32));
                        infoBlock.RecorderSoftware = GetBytesWithoutNull(reader.ReadBytes(32));
                        infoBlock.RecorderHardware = GetBytesWithoutNull(reader.ReadBytes(32));
                        infoBlock.Location = GetBytesWithoutNull(reader.ReadBytes(52));
                        VerifyChecksum(atcData, blockStart, blockHeader.Length, reader);

                        break;
                    case "ecg ":
                    case "ecg2":
                    case "ecg3":
                    case "ecg4":
                    case "ecg5":
                    case "ecg6":
                        var samples = new short[blockHeader.Length / 2];

                        for (int i = 0; i < samples.Length; i++)
                        {
                            samples[i] = reader.ReadInt16();
                        }

                        VerifyChecksum(atcData, blockStart, blockHeader.Length, reader);
                        switch (blockType)
                        {
                            case "ecg ": leadISamples = samples; break;
                            case "ecg2": leadIISamples = samples; break;
                            case "ecg3": leadIIISamples = samples; break;
                            case "ecg4": aVRSamples = samples; break;
                            case "ecg5": aVLSamples = samples; break;
                            case "ecg6": aVFSamples = samples; break;
                        }

                        break;
                    default:
                        reader.BaseStream.Position += blockHeader.Length + _checkSumLength;

                        break;
                }
            }

            var result = new EcgData
            {
                Gain = 1e6f / fmtBlock.Resolution,
                Frequency = fmtBlock.Frequency,
                AmplitudeResolution = fmtBlock.Resolution,
                MainsFrequency = (fmtBlock.Flags & 2) != 0 ? 60 : 50,
                Samples = new EcgSamples
                {
                    LeadI = leadISamples,
                    LeadII = leadIISamples,
                    LeadIII = leadIIISamples,
                    AVR = aVRSamples,
                    AVL = aVLSamples,
                    AVF = aVFSamples
                },
                Info = infoBlock,
                Error = error
            };

            return result;
        }

        public string Convert(byte[] atcData)
        {
            var ecgData = Parse(atcData);
            return JsonSerializer.Serialize(ecgData);
        }

        public void VerifyChecksum(byte[] data, long blockStart, uint blockLen, BinaryReader reader)
        {
            uint checksum = reader.ReadUInt32();
            uint sum = CalcChecksum(data, blockStart, blockLen);
            if (checksum != sum)
            {
                throw new InvalidDataException($"Checksum does not match. Expected: [{checksum}] Calculated: [{sum}]");
            }
        }
        public uint CalcChecksum(byte[] data, long blockStart, uint blockLen)
        {
            uint sum = 0;
            for (long i = blockStart; i < blockStart + 8 + blockLen; i++)
            {
                sum += data[i];
            }
            return sum;
        }

        public float[] CalcMillivolts(short[] data, float scale)
        {
            float[] result = new float[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                result[i] = (float)data[i] / scale;
            }
            return result;
        }

        public bool CompareByteArrays(byte[] arr1, byte[] arr2)
        {
            if (arr1 == null || arr2 == null || arr1.Length != arr2.Length)
            {
                return false;
            }

            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != arr2[i])
                {
                    return false;
                }
            }
            return true;
        }

        public byte[] GetBytesWithoutNull(byte[] data)
        {
            return data.Where(b => b != 0).ToArray();
        }
    }
}