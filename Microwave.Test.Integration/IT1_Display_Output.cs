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
    public class IT1_Display_Output
    {
        private IDisplay _display;
        private IOutput _output;
        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _display = new Display(_output);
        }

        [TestCase(1, 2)]
        [TestCase(7, 12)]
        public void ShowTime_ShowsDisplay_CorrectDisplay(int min, int sec)
        {
            _display.ShowTime(min, sec);
            string compareString = $"Display shows: {min:D2}:{sec:D2}";
            _output.Received().OutputLine(compareString);
        }

        [TestCase(12)]
        public void ShowPower_ShowsPower_CorrectDisplay(int power)
        {
            _display.ShowPower(power);
            string compareString = $"Display shows: {power} W";
            _output.Received().OutputLine(compareString);
        }

        [Test]
        public void Clear_ShowsClearCommand_CorrectDisplay()
        {
            _display.Clear();
            string compareString = $"Display cleared";
            _output.Received().OutputLine(compareString);
        }
    }

 


}
