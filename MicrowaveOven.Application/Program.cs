using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;

namespace MicrowaveOven.Application
{
    class Program
    {
        private static IButton _powerButton;
        private static IButton _startCancelButton;
        private static IButton _timeButton;
        private static IDisplay _display;
        private static IDoor _door;
        private static ILight _light;
        private static IOutput _output;
        private static IPowerTube _powerTube;
        private static ITimer _timer;

        private static ICookController _cookController;
        private static IUserInterface _userInterface;

        static void Main(string[] args)
        {
            _powerButton = new Button();
            _startCancelButton = new Button();
            _timeButton = new Button();
            _output = new Output();
            _display = new Display(_output);
            _door = new Door();
            _light = new Light(_output);
            _powerTube = new PowerTube(_output);
            _timer = new Timer();

            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light,
                _cookController);
            _cookController.UI = _userInterface;

            bool running = true;
            Console.WriteLine("Press P for powerbutton");
            Console.WriteLine("Press T for timebutton");
            Console.WriteLine("Press S for Start-Cancelbutton");
            Console.WriteLine("Press O for Open door");
            Console.WriteLine("Press C for Close door");
            Console.WriteLine("Press E for Exit application");


            while (running)
            {
                var key = Console.ReadKey(true);

                switch (key.KeyChar)
                {
                    case 'p':
                        _powerButton.Press();
                        break;
                    case 't':
                        _timeButton.Press();
                        break;
                    case 's':
                        _startCancelButton.Press();
                        break;
                    case 'o':
                        _door.Open();
                        break;
                    case 'c':
                        _door.Close();
                        break;
                    case 'e':
                        running = false;
                        break;
                }
            }
        }
    }
}
