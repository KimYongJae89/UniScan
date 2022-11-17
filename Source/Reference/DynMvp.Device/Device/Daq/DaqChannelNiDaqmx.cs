using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using DynMvp.Base;
using DynMvp.Device.Device.Daq;
using NationalInstruments;
using NationalInstruments.DAQmx;

namespace DynMvp.Devices.Daq
{
    public class DaqChannelNiDaqmx : DaqChannel
    {
        string nameToAssignChannel = "";

        public DaqChannelNiDaqmx() : base(DaqChannelType.Daqmx)
        {

        }

        public override void Initialize(DaqChannelProperty daqChannelProperty)
        {

        }

        public override double[] ReadData(int numSamples)
        {
            return ReadVoltage(numSamples);
        }

        public override double[] ReadVoltage(int numSamples)
        {
            double[] values = null;

            for (int i = 0; i < 3; i++)
            {
                values = ReadVoltageOnce(numSamples);
                if (values != null && values.Count() > 0)
                    break;

                Thread.Sleep(50);
            }

            if (values == null || values.Count() == 0)
            {
                ErrorManager.Instance().Report(ErrorCodeDaq.Instance.FailToReadValue, ErrorLevel.Error,
                    this.name, "Can't read values", null);
            }

            return values;
        }

        private double[] ReadVoltageOnce(int numSamples)
        {
            try
            {
                Task analogInTask = new Task();

                AIChannel aiChannel = analogInTask.AIChannels.CreateVoltageChannel(
                                ChannelProperty.ChannelName, nameToAssignChannel,
                                AITerminalConfiguration.Rse, ChannelProperty.MinValue, ChannelProperty.MaxValue, AIVoltageUnits.Volts);

                analogInTask.Timing.ConfigureSampleClock(nameToAssignChannel, ChannelProperty.SamplingHz,
                        SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, numSamples);

                analogInTask.Stream.Timeout = 1000;

                AnalogSingleChannelReader reader = new AnalogSingleChannelReader(analogInTask.Stream);
                return reader.ReadMultiSample(numSamples);
            }
            catch (DaqException exception)
            {
                ErrorManager.Instance().Report(ErrorCodeDaq.Instance.FailToReadValue, ErrorLevel.Error,
                    this.name, exception.Message, null);
            }

            return null;
        }
    }
}
