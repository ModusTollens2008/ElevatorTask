using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorTask
{
    internal class Floor
    {

        public ElevatorController elevatorController;
        public int FloorNumber { get; }
        public Elevator FirstElevator { get; set; }
        public Elevator SecondElevator { get; set; }
        public Floor(int number)
        {
            FloorNumber = number;
        }
        public State FirstElevatorStatus { get { return FirstElevator.CurrentState; } }
        public State SecondElevatorStatus { get { return SecondElevator.CurrentState; } }
        public int FirstElevatorFloor { get { return FirstElevator.CurrentFloor; }  }
        public int SecondElevatorFloor { get { return SecondElevator.CurrentFloor; } }
        public bool CallButtonStatus { get; set; }

        public Floor(int number, Elevator firstElevator, Elevator secondElevator,ElevatorController ec)
        {
            elevatorController = ec;
            FloorNumber = number;
            FirstElevator = firstElevator;
            SecondElevator = secondElevator;
        }

        public  void FloorButtonPress()
        {
            elevatorController.AssignToElevator(FloorNumber);
            Console.WriteLine($"{FloorNumber} floor call button pressed");
            CallButtonStatus = true;
            
        }
    }
}
