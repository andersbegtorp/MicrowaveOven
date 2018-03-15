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
using NUnit.Framework.Internal;

namespace Microwave.Test.Integration
{
    [TestFixture]
    class IT7_UserInterface_Door
    {
        private UserInterface _userInterface;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IDoor _door;
        private IDisplay _display;
        private ILight _light;
        private ICookController _cookController;
        private IOutput _output;

        [SetUp]
        public void SetUp()
        {
            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _startCancelButton = Substitute.For<IButton>();
            _door = new Door();
            _display = Substitute.For<IDisplay>();
            _output = Substitute.For<IOutput>();
            _light = new Light(_output);
            _cookController = Substitute.For<ICookController>();
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
        }

        [Test]
        public void OnDoorOpened_StateIsReady_LightIsTurnedOn()
        {
            _door.Open();

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }

        [Test]
        public void OnDoorOpened_StateIsSetPower_LightIsTurnedOn()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _door.Open();

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }

        [Test]
        public void OnDoorOpened_StateIsSetTime_LightIsTurnedOn()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _door.Open();

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }

        [Test]
        public void OnDoorOpened_StateIsCooking_LightRecievesNoCalls()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);
            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);

            _output.ClearReceivedCalls();

            _door.Open();

            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

        [Test]
        public void OnDoorOpened_StateIsDoorOpen_LightRecievesNoCalls()
        {
            _userInterface.OnDoorOpened(_door, EventArgs.Empty);
            _output.ClearReceivedCalls();


            _door.Open();

            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

        //DoorClosed

        [Test]
        public void OnDoorClosed_StateIsReady_LightRecievesNoCalls()
        {
            _door.Close();

            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

        [Test]
        public void OnDoorClosed_StateIsSetPower_LightRecievesNoCalls()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _door.Close();

            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

        [Test]
        public void OnDoorClosed_StateIsSetTime_LightRecievesNoCalls()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _door.Close();

            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

        [Test]
        public void OnDoorClosed_StateIsCooking_LightRecievesNoCalls()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);
            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);

            _output.ClearReceivedCalls();

            _door.Close();

            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

        [Test]
        public void OnDoorClosed_StateIsDoorOpen_LightIsTurnedOff()
        {
            _userInterface.OnDoorOpened(_door, EventArgs.Empty);
            _output.ClearReceivedCalls();


            _door.Close();

            _output.Received().OutputLine(Arg.Is<string>(s => s.Contains("off")));
        }


    }
}
