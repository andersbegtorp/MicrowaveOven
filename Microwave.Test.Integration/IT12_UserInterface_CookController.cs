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

namespace Microwave.Test.Integration
{
    [TestFixture]
    class IT12_UserInterface_CookController
    {
        private UserInterface _userInterface;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IDoor _door;
        private IOutput _output;
        private IDisplay _display;
        private ILight _light;
        private ITimer _timer;
        private IPowerTube _powerTube;
        private ICookController _cookController;

        [SetUp]
        public void SetUp()
        {
            _timer = Substitute.For<ITimer>();
            _output = Substitute.For<IOutput>();
            _powerTube = new PowerTube(_output);
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();
            _display = new Display(_output);
            _light = new Light(_output);
            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
            _cookController.UI = _userInterface;
        }

        [TestCase(0, 50)]
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
        public void Press_StateIsSetTime_CookingStartsCookingWithCorrectPower(int timesPowerButtonPressed, int power)
        {
            StateHelper.SetState(_userInterface, "SetPower");
            for (int i = 0; i < timesPowerButtonPressed; i++)
            {
                _powerButton.Press();
            }
            _timeButton.Press();

            string compareString = $"PowerTube works with {power} W";
            _output.ClearReceivedCalls();
            
            _startCancelButton.Press();
            _output.Received().OutputLine(compareString);
        }

        [TestCase(0, 60000)]
        [TestCase(1, 120000)]
        [TestCase(2, 180000)]
        [TestCase(3, 240000)]
        [TestCase(4, 300000)]
        [TestCase(5, 360000)]
        public void Press_StateIsSetTime_CookingStartsWithCorrectTime(int timesTimeButtonPressed, int expectedTime)
        {
            StateHelper.SetState(_userInterface, "SetTime");
            for (int i = 0; i < timesTimeButtonPressed; i++)
            {
                _timeButton.Press();
            }
            _output.ClearReceivedCalls();

            _startCancelButton.Press();
            _timer.Received().Start(expectedTime);
        }

        [Test]
        public void Open_StateIsCooking_MyCookerStops()
        {
            StateHelper.SetState(_userInterface, "Cooking");
            _output.ClearReceivedCalls();
            _timer.ClearReceivedCalls();

            _door.Open();
            _timer.Received(1).Stop();

        }

        [Test]
        public void OnTimerExpired_StateIsCooking_DisplayIsCleared()
        {
            StateHelper.SetState(_userInterface, "Cooking");
            _output.ClearReceivedCalls();

            _timer.Expired += Raise.EventWith(new object(), new EventArgs());

            _output.Received().OutputLine(Arg.Is<string>(s => s.Contains("cleared")));
        }

        [Test]
        public void OnTimerExpired_StateIsCooking_LightIsTurnedOff()
        {
            StateHelper.SetState(_userInterface, "Cooking");
            _output.ClearReceivedCalls();

            _timer.Expired += Raise.EventWith(new object(), new EventArgs());

            _output.Received().OutputLine(Arg.Is<string>(s => s.Contains("off")));
        }

    }
}
