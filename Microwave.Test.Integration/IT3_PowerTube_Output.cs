using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Microwave.Test.Integration
{
    [TestFixture]
    class IT3_PowerTube_Output
    {
        private PowerTube _powerTube;
        private IOutput _output;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _powerTube = new PowerTube(_output);
        }

        [Test]
        public void TurnOn_WasOff_CorrectOutput()
        {
            _powerTube.TurnOn(50);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("50 W")));
        }


        [Test]
        public void TurnOff_WasOff_NoOutput()
        {
            _powerTube.TurnOff();
            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

        [Test]
        public void TurnOn_WasOn_ThrowsException()
        {
            _powerTube.TurnOn(50);
            Assert.Throws<System.ApplicationException>(() => _powerTube.TurnOn(60));
        }

        [Test]
        public void TurnOn_NegativePower_ThrowsException()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() => _powerTube.TurnOn(-1));
        }

        [Test]
        public void TurnOn_HighPower_ThrowsException()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() => _powerTube.TurnOn(701));
        }

        [Test]
        public void TurnOn_ZeroPower_ThrowsException()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() => _powerTube.TurnOn(0));
        }

    }
}
