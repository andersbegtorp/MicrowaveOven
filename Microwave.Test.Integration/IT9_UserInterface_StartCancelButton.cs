using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NSubstitute.Core.Arguments;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    class IT9_UserInterface_StartCancelButton
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
            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _startCancelButton = new Button();
            _door = new Door();
            _display = new Display(_output);
            _light = new Light(_output);
            _cookController = Substitute.For<ICookController>();
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
        }

        [Test]
        public void Press_myStateIsReady_NoOutput()
        {
            _startCancelButton.Press();
            _output.DidNotReceive().OutputLine(Arg.Any<string>());          
        }

        [Test]
        public void Press_myStateIsSetPower_LightIsTurnedOff()
        {
            StateHelper.SetState(_userInterface, "SetPower");
            _light.TurnOn();
            _output.ClearReceivedCalls();

            _startCancelButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        [Test]
        public void Press_myStateIsSetPower_DisplayIsCleared()
        {
            StateHelper.SetState(_userInterface, "SetPower");
            _output.ClearReceivedCalls();

            _startCancelButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("cleared")));
        }

        [Test]
        public void Press_myStateIsSetTime_LightIsTurnedOn()
        {
            StateHelper.SetState(_userInterface, "SetTime");
            _output.ClearReceivedCalls();

            _startCancelButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }

        [Test]
        public void Press_myStateIsSetTime_DisplayIsCleared()
        {
            StateHelper.SetState(_userInterface, "SetTime");
            _output.ClearReceivedCalls();

            _startCancelButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("cleared")));
        }

        [Test]
        public void Press_myStateIsCooking_LightIsTurnedOff()
        {
            StateHelper.SetState(_userInterface, "Cooking");
            _output.ClearReceivedCalls();

            _startCancelButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        [Test]
        public void Press_myStateIsCooking_DisplayIsCleared()
        {
            StateHelper.SetState(_userInterface, "Cooking");
            _output.ClearReceivedCalls();

            _startCancelButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("cleared")));
        }

        [Test]
        public void Press_myStateIsDoorOpen_NoOutput()
        {
            StateHelper.SetState(_userInterface, "DoorOpen");
            _output.ClearReceivedCalls();

            _startCancelButton.Press();
            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

    }

}
