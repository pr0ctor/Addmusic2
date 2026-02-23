using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Addmusic2.Model.Constants;

namespace Addmusic2.Model
{
    internal class SampleInstrumentManager
    {
        public List<string> SampleNames { get; set; } = new();
        public List<Sample> Samples { get; set; } = new();
        public List<Sample> UsedSamples { get; set; } = new();
        public List<InstrumentInformation> Instruments { get; set; } = new();
        public List<InstrumentInformation> DefaultInstruments { get; set; } = new();
        public Dictionary<int, InstrumentInformation> UsedInstruments { get; set; } = new();

        public SampleInstrumentManager()
        {
            InitializeDefaultInstruments();
        }

        public void InitializeDefaultInstruments()
        {
            int[] removeableInstruments = { 19, 20 };
            var range = Enumerable.Range(0, MagicNumbers.StartingCustomInstrumentNumber)
                .Except(removeableInstruments);
            foreach (var number in range)
            {
                var defaultInstrumentInfo = new InstrumentInformation
                {
                    InstrumentNumber = number,
                    InstrumentData = MagicNumbers.InstrumentsToSample[number]
                };

                DefaultInstruments.Add(defaultInstrumentInfo);
            }
        }

        #region Sample Manger

        public void AddNewSampleName(string sampleName)
        {
            if(!ContainsSampleName(sampleName))
            {
                SampleNames.Add(sampleName);
            }
        }
        public void AddNewSample(Sample addmusicSample)
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

        public bool ContainsSample(Sample addmusicSample)
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

        public bool UseSample(Sample addmusicSample)
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
            return DefaultInstruments.Any(i => i.InstrumentNumber == instrumentNumber)
                || Instruments.Any(i => i.InstrumentNumber == instrumentNumber);
        }

        public bool ContainsInstrument(InstrumentInformation instrumentInformation)
        {
            return DefaultInstruments.Contains(instrumentInformation)
                || Instruments.Contains(instrumentInformation);
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
