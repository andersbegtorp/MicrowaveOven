using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Integration
{
    [TestFixture]
    class IT13_CookController_Timer
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
            _timer = new Timer();
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

        [Test]
        public void StartCookingFromButtonPress_TimerStarts_TimeIsCorrect()
        {
            ManualResetEvent pause = new ManualResetEvent(false);

            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            pause.WaitOne(1000);
            _output.ClearReceivedCalls();
            pause.WaitOne(60000);

            _output.Received(1).OutputLine(Arg.Is<string>(s => s.Contains("cleared")));


        }
    }
}
