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
    [TestFixture]
    class IT10_UserInterface_PowerButton
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
            _timeButton = Substitute.For<IButton>();
            _startCancelButton = new Button();
            _door = new Door();
            _display = new Display(_output);
            _light = new Light(_output);
            _cookController = Substitute.For<ICookController>();
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
        }

        [Test]
        public void Press_StateIsReady_DisplayShowsPower()
        {
            _powerButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("50 W")));
        }

        [TestCase(1, 100)]
        [TestCase(2, 150)]
        [TestCase(3, 200)]
        [TestCase(4, 250)]
        [TestCase(5, 300)]
        [TestCase(6, 350)]
        [TestCase(7, 400)]
        [TestCase(8, 450)]
        [TestCase(13, 700)]
        [TestCase(14, 50)]
        public void Press_StateIsSetPower_DisplayShowsPower(int timesPressed, int expectedPower)
        {
            StateHelper.SetState(_userInterface, "SetPower");

            for (int i = 0; i < timesPressed ; i++)
            {
                _output.ClearReceivedCalls();
                _powerButton.Press();
            }
            string compareString = expectedPower + " W";
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains(compareString)));
        }

        [Test]
        public void Press_StateIsSetTime_NoOutput()
        {
            StateHelper.SetState(_userInterface, "SetTime");
            _output.ClearReceivedCalls();

            _powerButton.Press();
            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

        [Test]
        public void Press_StateIsCooking_NoOutput()
        {
            StateHelper.SetState(_userInterface, "Cooking");
            _output.ClearReceivedCalls();

            _powerButton.Press();
            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

        [Test]
        public void Press_StateIsDoorOpen_NoOutput()
        {
            StateHelper.SetState(_userInterface, "DoorOpen");
            _output.ClearReceivedCalls();

            _powerButton.Press();
            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }
    }
}
