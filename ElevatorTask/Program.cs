// See https://aka.ms/new-console-template for more information
using ElevatorTask;

public class MianClass
{
    public static void Main(string[] args) 
    {
        Elevator el1 = new Elevator(1);
        Elevator el2 = new Elevator(2);
        el1.MaxWeight = 400;
        el2.MaxWeight = 800;

        ElevatorController controller = new ElevatorController(el1,el2,1000);

        Floor[] floors = new Floor[20];
        for (int i = 0; i < 20; i++) 
        {
            floors[i] = new Floor(i+1,el1,el2,controller);
        }
        
 
        Person person1 = new Person("Passanger 1");
        Person person2 = new Person("Passenger 2");
        person1.Floor = floors[0];
        person2.Floor = floors[14];

        Thread person1Thread = new Thread(() =>
        {
            person1.PressElevatorButton();
            person1.WaitForElevator();
            person1.PressFloorNumberButton(14);
            person1.GetOffElevator();

        });

        Thread person2Thread = new Thread(() =>
        {
            person2.PressElevatorButton();
            person2.WaitForElevator();
            person2.PressFloorNumberButton(1);
            person2.GetOffElevator();

        });

        controller.Run();
        person1Thread.Start();
        person2Thread.Start();

    }
}
