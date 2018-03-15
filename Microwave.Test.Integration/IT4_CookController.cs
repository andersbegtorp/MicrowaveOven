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
using NSubstitute.Core.Arguments;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Integration
{
    [TestFixture]
    class IT4_CookController
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
            _timer = new Timer();
            _cookController = new CookController(_timer, _display, _powerTube);
            _cookController.UI = _ui;

        }

        [TestCase(50, 1000)]
        public void StartCooking_TimerGetsStarted_CorrectTime(int power, int time)
        {
            ManualResetEvent pause = new ManualResetEvent(false);
            _cookController.StartCooking(power, time);

            pause.WaitOne(time + 100);

            _ui.Received().CookingIsDone();

        }
    }
}
