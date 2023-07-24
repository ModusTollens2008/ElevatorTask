using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorTask
{
    internal class ElevatorController
    {
        public Elevator FirstElevator { get; set; }
        public Elevator SecondElevator { get; set; }

        private int timeToMove = 1000;
        public int TimeToMove 
        {
            get 
            {
                return timeToMove;
            } 
            set
            {
                if (value>0) 
                {
                    timeToMove = value;
                }
            }
        } 
        public ElevatorController(Elevator firstElevator,Elevator secondElevator, int timeToMove)
        {
            FirstElevator = firstElevator;
            SecondElevator = secondElevator;
            TimeToMove = timeToMove;
        }

        public void AssignToElevator(int floor)
        {
            lock (this) {
                int distance1 = Math.Abs(floor - FirstElevator.CurrentFloor);
                int distance2 = Math.Abs(floor - SecondElevator.CurrentFloor);

                if (distance1 <= distance2 && (!FirstElevator.IsMoving))
                {
                    FirstElevator.AddToQueue(floor);
                }
                else
                {
                    if (distance1 > distance2 && (!SecondElevator.IsMoving))
                    {
                        SecondElevator.AddToQueue(floor);
                    }
                    else
                    {
                        SecondElevator.AddToQueue(floor);
                    }
                }
            }
        }

        public void Run()
        {
            Thread firstElevatorThread = new Thread(() =>
            {
                while (true)
                {
                    FirstElevator.MoveToFloor();
                    Thread.Sleep(TimeToMove);
                }
            });
            Thread secondElevatorThread = new Thread(() =>
            {
                while (true)
                {
                    SecondElevator.MoveToFloor();
                    Thread.Sleep(TimeToMove);
                }
            });
            firstElevatorThread.Start();
            secondElevatorThread.Start();
        }  
        
    }
}
