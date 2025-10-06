using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class SampleInstrumentManager
    {
        public List<string> SampleNames { get; set; } = new();
        public List<AddmusicSample> Samples { get; set; } = new();
        public List<AddmusicSample> UsedSamples { get; set; } = new();
        public List<InstrumentInformation> Instruments { get; set; } = new();
        public Dictionary<int, InstrumentInformation> UsedInstruments { get; set; } = new();

        public SampleInstrumentManager() { }

        #region Sample Manger

        public void AddNewSampleName(string sampleName)
        {
            if(!ContainsSampleName(sampleName))
            {
                SampleNames.Add(sampleName);
            }
        }
        public void AddNewSample(AddmusicSample addmusicSample)
        {
            if(!ContainsSample(addmusicSample))
            {
                Samples.Add(addmusicSample);
            }
        }

        public bool ContainsSampleName(string sampleName)
        {
            return SampleNames.Contains(sampleName);
        }

        public bool ContainsSample(AddmusicSample addmusicSample)
        {
            return Samples.Contains(addmusicSample);
        }

        public bool UseSampleName(string sampleName)
        {
            if (!ContainsSampleName(sampleName))
            {
                return false;
            }

            var foundSample = Samples.Find(s => s.Name == sampleName);
            if (foundSample == null)
            {
                return false;
            }

            return UseSample(foundSample);
        }

        public bool UseSample(AddmusicSample addmusicSample)
        {
            if(!ContainsSample(addmusicSample))
            {
                return false;
            }

            if (!UsedSamples.Contains(addmusicSample))
            {
                UsedSamples.Add(addmusicSample);
            }
            return true;
        }

        #endregion

        #region Instrument Manager

        public void AddInstrument(InstrumentInformation instrumentInformation)
        {
            if(!ContainsInstrument(instrumentInformation))
            {
                Instruments.Add(instrumentInformation);
            }
        }

        public bool ContainsInstrument(int instrumentNumber)
        {
            return Instruments.Any(i => i.InstrumentNumber == instrumentNumber);
        }

        public bool ContainsInstrument(InstrumentInformation instrumentInformation)
        {
            return Instruments.Contains(instrumentInformation);
        }

        public bool UseInstrument(int instrumentNumber)
        {
            if (!ContainsInstrument(instrumentNumber))
            {
                return false;
            }

            var instrumentInformation = Instruments.Find(i => i.InstrumentNumber == instrumentNumber);

            return UseInstrument(instrumentNumber, instrumentInformation!);
        }

        public bool UseInstrument(int instrumentNumber, InstrumentInformation instrumentInformation)
        {
            if(!ContainsInstrument(instrumentInformation))
            {
                return false;
            }

            if(!UsedInstruments.Keys.Contains(instrumentNumber))
            {
                UsedInstruments.Add(instrumentNumber, instrumentInformation);
            }

            return true;
        }


        #endregion


        #region Helpers

        public int GetTotalInstrumentSpace()
        {
            return (Instruments.Count == 0) ? 0 : Instruments.Select(i => 1 + i.HexComponents.Count).Sum();
        }

        #endregion


    }
}
