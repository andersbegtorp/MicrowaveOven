using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;

namespace Microwave.Test.Integration
{
    public static class StateHelper
    {
        public static void SetState(UserInterface userInterface, string state)
        {
            switch (state)
            {
                case "Ready":             
                    break;
                case "SetPower":
                    userInterface.OnPowerPressed(new object(), EventArgs.Empty);
                    break;
                case "SetTime":
                    userInterface.OnPowerPressed(new object(), EventArgs.Empty);
                    userInterface.OnTimePressed(new object(), EventArgs.Empty);
                    break;
                case "Cooking":
                    userInterface.OnPowerPressed(new object(), EventArgs.Empty);
                    userInterface.OnTimePressed(new object(), EventArgs.Empty);
                    userInterface.OnStartCancelPressed(new object(), EventArgs.Empty);
                    break;
                case "DoorOpen":
                    userInterface.OnDoorOpened(new object(), EventArgs.Empty);
                    break;

            }
        }
    }
}
