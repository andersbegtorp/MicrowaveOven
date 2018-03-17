using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Microwave.Test.Integration
{
    [TestFixture()]
    class IT11_UserInterface_TimeButton
    {
        private UserInterface _userInterface;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IDoor _door;
        private IOutput _output;
        private IDisplay _display;
        private ILight _light;
        private ICookController _cookController;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();
            _display = new Display(_output);
            _light = new Light(_output);
            _cookController = Substitute.For<ICookController>();
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
        }

        [Test]
        public void Press_MyStateIsReady_NoOutput()
        {
            _timeButton.Press();
            
            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }
        [Test]
        public void Press_MyStateIsSetPower_DisplayShowsTime()
        {
            StateHelper.SetState(_userInterface, "SetPower");
            _output.ClearReceivedCalls();
            _timeButton.Press();

            _output.Received().OutputLine(Arg.Is<string>(s => s.Contains("01:00")));
        }

        [TestCase(1, "02:00")]
        [TestCase(2, "03:00")]
        [TestCase(3, "04:00")]
        [TestCase(4, "05:00")]
        [TestCase(5, "06:00")]
        [TestCase(6, "07:00")]
        public void Press_MyStateIsSetTime_DisplayShowsTime(int timesPressed, string expectedTime)
        {
            StateHelper.SetState(_userInterface, "SetTime");

            for (int i = 0; i < timesPressed; i++)
            {
                _output.ClearReceivedCalls();
                _timeButton.Press();
            }

            _output.Received().OutputLine(Arg.Is<string>(s => s.Contains(expectedTime)));
        }

        [Test]
        public void Press_MyStateIsCooking_NoOutput()
        {
            StateHelper.SetState(_userInterface, "Cooking");
            _output.ClearReceivedCalls();
            _timeButton.Press();

            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

        [Test]
        public void Press_MyStateIsDoorOpen_NoOutput()
        {
            StateHelper.SetState(_userInterface, "DoorOpen");
            _output.ClearReceivedCalls();
            _timeButton.Press();

            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }
    }
}
