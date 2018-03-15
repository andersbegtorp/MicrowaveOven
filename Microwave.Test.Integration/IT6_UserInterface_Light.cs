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
    class IT6_UserInterface_Light
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
            _door = Substitute.For<IDoor>();
            _display = Substitute.For<IDisplay>();
            _output = Substitute.For<IOutput>();
            _light = new Light(_output);
            _cookController = Substitute.For<ICookController>();
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
        }

        //StartCancel
        [Test]
        public void OnStartCancelPressed_MyStateIsReady_LightRecievesNoCalls()
        {
            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);

            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

        [Test]
        public void OnStartCancelPressed_MyStateIsDoorOpen_LightRecievesNoCalls()
        {
            _userInterface.OnDoorOpened(_door, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);
            
            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

        [Test]
        public void OnStartCancelPressed_MyStateIsSetPower_LightGetsTurnedOff()
        {
            //Arrange
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _light.TurnOn();

            //Act
            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);

            //Assert
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        [Test]
        public void OnStartCancelPressed_MyStateIsSetTime_LightGetsTurnedOn()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);
            
            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }

        [Test]
        public void OnStartCancelPressed_MyStateIsCooking_LightGetsTurnedOff()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);
            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);
            _light.TurnOn();

            _output.ClearReceivedCalls();

            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);
            
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        //OnDoorOpened
        [Test]
        public void OnDoorOpened_StateIsReady_LightTurnsOn()
        {
            _userInterface.OnDoorOpened(_door, EventArgs.Empty);

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("on")));

        }

        [Test]
        public void OnDoorOpened_StateIsSetPower_LightIsTurnedOn()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _userInterface.OnDoorOpened(_door, EventArgs.Empty);

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("on")));

        }

        [Test]
        public void OnDoorOpened_StateIsSetTime_LightIsTurnedOn()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);
            _output.ClearReceivedCalls();
            
            _userInterface.OnDoorOpened(_door, EventArgs.Empty);

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("on")));

        }

        [Test]
        public void OnDoorOpened_StateIsCooking_LightIsNotCalled()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);
            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);

            _output.ClearReceivedCalls();

            _userInterface.OnDoorOpened(_door, EventArgs.Empty);

            _output.DidNotReceive().OutputLine(Arg.Any<string>());

        }

        [Test]
        public void OnDoorOpened_StateIsDoorOpen_LightIsNotCalled()
        {
            _userInterface.OnDoorOpened(_door, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _userInterface.OnDoorOpened(_door, EventArgs.Empty);

            _output.DidNotReceive().OutputLine(Arg.Any<string>());

        }

        //OnDoorClosed
        [Test]
        public void OnDoorClosed_StateIsReady_LightIsNotCalled()
        {
            _userInterface.OnDoorClosed(_door, EventArgs.Empty);

            _output.DidNotReceive().OutputLine(Arg.Any<string>());

        }

        [Test]
        public void OnDoorClosed_StateIsSetPower_LightIsNotCalled()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _userInterface.OnDoorClosed(_door, EventArgs.Empty);

            _output.DidNotReceive().OutputLine(Arg.Any<string>());

        }

        [Test]
        public void OnDoorClosed_StateIsSetTime_LightIsNotCalled()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _userInterface.OnDoorClosed(_door, EventArgs.Empty);

            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

        [Test]
        public void OnDoorClosed_StateIsCooking_LightIsNotCalled()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);
            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);

            _output.ClearReceivedCalls();

            _userInterface.OnDoorClosed(_door, EventArgs.Empty);

            _output.DidNotReceive().OutputLine(Arg.Any<string>());

        }

        [Test]
        public void OnDoorClosed_StateIsDoorOpen_LightIsTurnedOff()
        {
            _userInterface.OnDoorOpened(_door, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _userInterface.OnDoorClosed(_door, EventArgs.Empty);

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("off")));

        }


        //CookingIsDone

        [Test]
        public void CookingIsDone_StateIsReady_LightIsNotCalled()
        {
            _userInterface.CookingIsDone();

            _output.DidNotReceive().OutputLine(Arg.Any<string>());

        }

        [Test]
        public void CookingIsDone_StateIsSetPower_LightIsNotCalled()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _userInterface.CookingIsDone();

            _output.DidNotReceive().OutputLine(Arg.Any<string>());

        }

        [Test]
        public void CookingIsDone_StateIsSetTime_LightIsNotCalled()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _userInterface.CookingIsDone();

            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

        [Test]
        public void CookingIsDone_StateIsCooking_LightIsTurnedOff()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);
            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);

            _output.ClearReceivedCalls();

            _userInterface.CookingIsDone();

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("off")));

        }

        [Test]
        public void CookingIsDone_StateIsDoorOpen_LightIsNotCalled()
        {
            _userInterface.OnDoorOpened(_door, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _userInterface.CookingIsDone();

            _output.DidNotReceive().OutputLine(Arg.Any<string>());

        }

    }
}
