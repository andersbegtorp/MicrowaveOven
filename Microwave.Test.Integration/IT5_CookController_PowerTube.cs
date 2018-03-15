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
    class IT5_CookController_PowerTube
    {
        private IUserInterface _ui;
        private IDisplay _display;
        private IOutput _output;
        private IPowerTube _powerTube;
        private ITimer _timer;
        private CookController _cookController;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _ui = Substitute.For<IUserInterface>();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _timer = Substitute.For<ITimer>();
            _cookController = new CookController(_timer, _display, _powerTube);
            _cookController.UI = _ui;

        }

        [TestCase(12,12)]
        public void StartCooking_OutputShowsPower_PowerIsCorrect(int power, int time)
        {
            _cookController.StartCooking(power, time);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains(power +" %")));
        }

        [Test]
        public void Stop_OutputShowsPowerStatus_PowerIsTurnedOff()
        {
            _cookController.Stop();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        [Test]
        public void OnTimerExpired_IsCooking_OutputShowsPowerTubeIsTurnedOff()
        {
            _cookController.StartCooking(20, 10);
            _output.ClearReceivedCalls();
            _cookController.OnTimerExpired(_timer, EventArgs.Empty);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        [Test]
        public void OnTimerExpired_IsNotCooking_OutputDidNotRecieveAnyCalls()
        {
            _cookController.OnTimerExpired(_timer, EventArgs.Empty);
            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

    }
}
