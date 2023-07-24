using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorTask
{
    internal class Elevator
    {
        public int Number { get; set; }
        public double MaxWeight { get; set; } = 400;

        private List<Person> people = new List<Person>();

        public Floor[] Floors { get; set; }
        private double GetCurrentWeight() 
        {
            double weight = 0;
            foreach (Person person in people) 
            {
                weight+=person.Weight;
            }
            return weight;
        }


        private readonly object lockObject = new object();
        public ConcurrentQueue<int> DestinationFloors { get; set; } = new ConcurrentQueue<int>();
   
        int currentFloor = 1;
        public int CurrentFloor
        {
            get
            { return currentFloor; }
            private set
            {
                currentFloor = value;
                Console.WriteLine($"Elevator {Number} is on {currentFloor} floor"); ;
            }
        }

        State currentState = State.CLOSED;
        public State CurrentState
        {
            get
            {
                lock (lockObject)
                { return currentState; }
            }
            private set
            {
                lock (lockObject)
                {
                    currentState = value;
                    Console.WriteLine($"Elevator {Number} status - {value}");
                }
            }
        }
        bool isMoving = false;
        public bool IsMoving { get { lock (this) { return isMoving; } } set { isMoving = value; } }

        public Elevator(int number)
        {
            Number = number;
        }

        public void PersonGetOn(Person person) 
        {
            lock (lockObject) 
            {
                if(person!=null) 
                {
                    people.Add(person);
                }
            }
        }

        public void PersonGetOff(Person person)
        {
            lock (lockObject)
            {
                    people.Remove(person);
            }
        }

        public void AddToQueue(int floor)
        {
                if (!DestinationFloors.Contains(floor))
                {
                    IsMoving = true;
                    DestinationFloors.Enqueue(floor);

                }
        }

        public void MotionDetect(bool motion)
        {
            if (motion)
            {
               
                if (CurrentState != State.OPENED)
                {
                    CurrentState = State.OPENED;   
                }
                Thread.Sleep(2000);    
            }

        }
       
        public void NoMotionDetect(bool motion) 
        {
            if(!motion)
            {
                Thread.Sleep(2000);
                if(CurrentState==State.OPENED) 
                {
                    CurrentState = State.DOOR_CLOSING;
                    CurrentState = State.CLOSED;
                }
               
            }
        }

        public void PressFloorButton(int floor) 
        {
            AddToQueue(floor);
        }
        public void MoveToFloor() 
        {

            lock (lockObject)
            {

                if (!IsMoving && CurrentState != State.CLOSED)
                {
                    CurrentState = State.DOOR_CLOSING;
                    CurrentState = State.CLOSED;
                }
                if (DestinationFloors.Count == 0)
                {
                    return;
                }
                if(GetCurrentWeight()>MaxWeight) 
                {
                    Console.WriteLine("Max weight!");
                    return;
                }
                IsMoving = true;
                int destinationFloor;
                DestinationFloors.TryPeek(out destinationFloor);
                if (CurrentFloor > destinationFloor)
                {
                    CurrentState = State.DOWN;
                    CurrentFloor--;
                }
                if (CurrentFloor < destinationFloor)
                {
                    CurrentState = State.UP;
                    CurrentFloor++;
                }
                if (CurrentFloor == destinationFloor)
                {
                    CurrentState = State.DOOR_OPENING;
                    CurrentState = State.OPENED;        
                    IsMoving = false;
                    if(Floors!=null) 
                    {
                        Floors[CurrentFloor - 1].CallButtonStatus = false;
                    }
                    DestinationFloors.TryDequeue(out int i);

                }
            }
        }

        public void DoorOpenButton() 
        {
            IsMoving=false;
            CurrentState = State.DOOR_OPENING;
            CurrentState = State.OPENED;
        }

        public void DoorCloseButton()
        {
            if (CurrentState != State.CLOSED|| CurrentState != State.DOOR_CLOSING)
            {
                CurrentState = State.DOOR_CLOSING;
                CurrentState = State.CLOSED;
            }
        }
        public void CallElevatorManager() 
        {
            Console.WriteLine("Elevator manager called");
        }

    }
    public enum State { UP, DOWN, DOOR_OPENING, DOOR_CLOSING, OPENED,CLOSED };
}
