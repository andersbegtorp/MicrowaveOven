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
    class IT8_UserInterface_Display
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
            _output = Substitute.For<IOutput>();
            _display = new Display(_output);
            _light = new Light(_output);
            _cookController = Substitute.For<ICookController>();
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
        }

        //OnPowerPressed
        [Test]
        public void OnPowerPressed_StateIsReady_DisplayShowsPowerLevel()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);

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
        public void OnPowerPressed_StateIsSetPower_DisplayShowsPowerLevel(int timesPressed, int expectedPower)
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _output.ClearReceivedCalls();

            for (int i = 0; i < timesPressed; i++)
            {
                _output.ClearReceivedCalls();
                _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            }
            string compareString = expectedPower + " W";
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains(compareString)));
        }
        [Test]
        public void OnPowerPressed_StateIsSetTime_DisplayShowsNothing()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);

            _output.DidNotReceive().OutputLine(Arg.Any<string>());

        }

        [Test]
        public void OnPowerPressed_StateIsCooking_DisplayShowsNothing()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);
            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);

            _output.ClearReceivedCalls();

            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

        [Test]
        public void OnPowerPressed_StateIsDoorOpen_DisplayShowsNothing()
        {
            _userInterface.OnDoorOpened(_door, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _output.DidNotReceive().OutputLine(Arg.Any<string>());

        }

        //On TimePressed
        [Test]
        public void OnTimePressed_StateIsReady_DisplayShowsNothing()
        {
            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);

            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

        [Test]
        public void OnTimePressed_StateIsSetPower_DisplayShowsTime()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("01:00")));
            
        }

        [TestCase(1, "02:00")]
        [TestCase(2, "03:00")]
        [TestCase(3, "04:00")]
        [TestCase(4, "05:00")]
        [TestCase(5, "06:00")]
        [TestCase(6, "07:00")]
        public void OnTimePressed_StateIsSetTime_DisplayShowsTime(int timesPressed, string expectedTime)
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);
            _output.ClearReceivedCalls();

            for (int i = 0; i < timesPressed; i++)
            {
                _output.ClearReceivedCalls();
                _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);

            }

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains(expectedTime)));
        }

        [Test]
        public void OnTimePressed_StateIsCooking_DisplaysShowsNothing()
        {
            StateHelper.SetState(_userInterface, "Cooking");
            _output.ClearReceivedCalls();

            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);

            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

        [Test]
        public void OnTimePressed_StateIsDoorOpen_DisplayShowsNothing()
        {
            StateHelper.SetState(_userInterface, "DoorOpen");
            _output.ClearReceivedCalls();

            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty );
            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

        //OnStartCancelPressed

        [Test]
        public void OnStartCancelPressed_StateIsReady_DisplayIsNotCleared()
        {
            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);
            _output.DidNotReceive().OutputLine(Arg.Is<string>(str => str.Contains("cleared")));
        }

        [Test]
        public void OnStartCancelPressed_StateIsSetPower_DisplayIsCleared()
        {
            StateHelper.SetState(_userInterface, "SetPower");
            _output.ClearReceivedCalls();
            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);
            _output.Received().OutputLine(Arg.Is<string>(s => s.Contains("cleared")));
        }

        [Test]
        public void OnStartCancelPressed_StateIsSetTime_DisplayIsCleared()
        {
            StateHelper.SetState(_userInterface, "SetTime");
            _output.ClearReceivedCalls();
            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);
            _output.Received().OutputLine(Arg.Is<string>(s => s.Contains("cleared")));
        }

        [Test]
        public void OnStartCancelPressed_StateIsCooking_DisplayIsCleared()
        {
            StateHelper.SetState(_userInterface, "Cooking");
            _output.ClearReceivedCalls();
            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);
            _output.Received().OutputLine(Arg.Is<string>(s => s.Contains("cleared")));
        }

        [Test]
        public void OnStartCancelPressed_StateIsDoorOpen_DisplayIsNotCleared()
        {
            StateHelper.SetState(_userInterface, "DoorOpen");
            _output.ClearReceivedCalls();
            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);
            _output.DidNotReceive().OutputLine(Arg.Is<string>(str => str.Contains("cleared")));
        }

        //OnDoorOpened
        [Test]
        public void OnDoorOpened_StateIsReady_DisplayIsNotCleared()
        {
            _userInterface.OnDoorOpened(_door, EventArgs.Empty);
            _output.DidNotReceive().OutputLine(Arg.Is<string>(str => str.Contains("cleared")));

        }

        [Test]
        public void OnDoorOpened_StateIsSetPower_DisplayIsCleared()
        {
            StateHelper.SetState(_userInterface, "SetPower");
            _output.ClearReceivedCalls();

            _userInterface.OnDoorOpened(_door, EventArgs.Empty);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("cleared")));

        }

        [Test]
        public void OnDoorOpened_StateIsSetTime_DisplayIsCleared()
        {
            StateHelper.SetState(_userInterface, "SetTime");
            _output.ClearReceivedCalls();

            _userInterface.OnDoorOpened(_door, EventArgs.Empty);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("cleared")));

        }

        [Test]
        public void OnDoorOpened_StateIsCooking_DisplayIsNotCleared()
        {
            StateHelper.SetState(_userInterface, "Cooking");
            _output.ClearReceivedCalls();

            _userInterface.OnDoorOpened(_door, EventArgs.Empty);
            _output.DidNotReceive().OutputLine(Arg.Is<string>(str => str.Contains("cleared")));

        }

        [Test]
        public void OnDoorOpened_StateIsDoorOpen_DisplayIsNotCleared()
        {
            StateHelper.SetState(_userInterface, "DoorOpen");
            _output.ClearReceivedCalls();

            _userInterface.OnDoorOpened(_door, EventArgs.Empty);
            _output.DidNotReceive().OutputLine(Arg.Is<string>(str => str.Contains("cleared")));

        }

        //CookingIsDone
        [Test]
        public void CookingIsDone_StateIsReady_DisplayIsNotCleared()
        {
            _userInterface.CookingIsDone();
            _output.DidNotReceive().OutputLine(Arg.Is<string>(str => str.Contains("cleared")));
        }

        [Test]
        public void CookingIsDone_StateIsSetPower_DisplayIsNotCleared()
        {
            StateHelper.SetState(_userInterface, "SetPower");
            _output.ClearReceivedCalls();

            _userInterface.CookingIsDone();
            _output.DidNotReceive().OutputLine(Arg.Is<string>(str => str.Contains("cleared")));
        }

        [Test]
        public void CookingIsDone_StateIsSetTime_DisplayIsNotCleared()
        {
            StateHelper.SetState(_userInterface, "SetTime");
            _output.ClearReceivedCalls();

            _userInterface.CookingIsDone();
            _output.DidNotReceive().OutputLine(Arg.Is<string>(str => str.Contains("cleared")));
        }

        [Test]
        public void CookingIsDone_StateIsCooking_DisplayIsCleared()
        {
            StateHelper.SetState(_userInterface, "Cooking");
            _output.ClearReceivedCalls();

            _userInterface.CookingIsDone();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("cleared")));
        }

        [Test]
        public void CookingIsDone_StateIsDoorOpen_DisplayIsNotCleared()
        {
            StateHelper.SetState(_userInterface, "DoorOpen");
            _output.ClearReceivedCalls();

            _userInterface.CookingIsDone();
            _output.DidNotReceive().OutputLine(Arg.Is<string>(str => str.Contains("cleared")));
        }
    }
}
