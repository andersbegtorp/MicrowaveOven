using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NSubstitute.Core.Arguments;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Integration
{
    [TestFixture]
    class IT4_CookController_Display
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
            _powerTube = Substitute.For<IPowerTube>();
            _timer = Substitute.For<ITimer>();
            _cookController = new CookController(_timer, _display, _powerTube);
            _cookController.UI = _ui;

        }

        [TestCase(120000, 2, 0)]
        [TestCase(130000, 2, 10)]
        public void OnTimerTick_ShowsTimeRemaining_RemainingTimeIsCorrect(int totalSeconds, int min, int sec)
        {
            _timer.TimeRemaining.Returns(totalSeconds);
            
            _cookController.OnTimerTick(_timer, EventArgs.Empty);

            string compareString = $"Display shows: {min:D2}:{sec:D2}";
            _output.Received().OutputLine(compareString);

        }


    }
}
