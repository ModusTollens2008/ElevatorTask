using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorTask
{
    internal class Person
    {
        public string Name { get; set; }

        public bool IsMoVing = false;
        public double Weight { get; set; } = 75;
        public Floor Floor { get; set; }
        public Elevator Elevator { get; set; }

        bool entered = false;
        int targetfloor;

        public Person(string name,Floor floor) 
        {
            Name = name;
            Floor = floor;
        }
        public Person(string name)
        {
            Name=name;
        }
        public void WaitForElevator() 
        {
            while (Elevator == null)
            {

                if ((Floor.FirstElevator.CurrentFloor == Floor.FloorNumber)
                   && (Floor.FirstElevator.CurrentState == State.OPENED))
                {
                    Elevator = Floor.FirstElevator;
                }
                else
                {
                    if ((Floor.SecondElevator.CurrentFloor == Floor.FloorNumber)
                    && (Floor.SecondElevator.CurrentState == State.OPENED))
                    {
                        Elevator = Floor.SecondElevator;
                    }
                    else
                        Elevator = null;
                }
            }
        }
        public void PressElevatorButton() 
        {
            Floor.FloorButtonPress();
        }
        public void PressFloorNumberButton(int floorNumber) 
        {
            entered = GetInElevator();
            if(entered) 
            {
                IsMoVing = false;
                Console.WriteLine($"{Name} pressed {floorNumber} floor button in elevator number {Elevator.Number}");
                Elevator.PressFloorButton(floorNumber);
                Elevator.NoMotionDetect(IsMoVing);            
                targetfloor = floorNumber;
            }
        }

        public void GetOffElevator() 
        {
            if(Elevator!=null) 
            {
                while (Elevator.CurrentFloor != targetfloor && Elevator.CurrentState != State.OPENED) { } 
                Console.WriteLine($"{Name} get off elevator number {Elevator.Number}");
                Elevator.PersonGetOff(this);
                Elevator=null;
            }
        }
        public bool GetInElevator() 
        {
            if (Elevator != null)
             {
                
                if (Elevator.CurrentFloor == Floor.FloorNumber&&Elevator.CurrentState==State.OPENED)
                {
                    IsMoVing = true;
                    Console.WriteLine($"{Name} get on elevator number {Elevator.Number}");
                    Elevator.MotionDetect(IsMoVing);
                    Elevator.PersonGetOn(this);
                          
                    return true;
                }
                else
                    return false;
            }
            else
               return false;
        }
    }
}
