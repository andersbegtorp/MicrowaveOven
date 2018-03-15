using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT2_Light_Output
    {
        private ILight _light;
        private IOutput _output;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _light = new Light(_output);
        }

        [Test]
        public void TurnOn_WasOff_CorrectOutput()
        {
            _light.TurnOn();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }
        [Test]
        public void TurnOff_WasOn_CorrectOutput()
        {
            _light.TurnOn();
            _output.ClearReceivedCalls();
            _light.TurnOff();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }
        [Test]
        public void TurnOn_WasOn_NoRecievedOutput()
        {
            _light.TurnOn();
            _output.ClearReceivedCalls();
            _light.TurnOn();
            _output.DidNotReceive().OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }

        [Test]
        public void TurnOff_WasOff_NoRecievedOutput()
        {
            _light.TurnOff();
            _output.DidNotReceive().OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

    }
}
