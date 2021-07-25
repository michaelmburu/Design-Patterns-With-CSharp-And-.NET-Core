using System;
using System.Collections.Generic;


namespace Hands_On_Design_Patterns_With_CSharp_And_.NET_Core
{
    public class Program
    {
          #region 1. Decorator Pattern
        //    public static void Main(string[] args)
        //    {
        //        var messages = new List<IMessage>
        //        {
        //            new NormalDecorator(new SimpleMessage("Frist Message")),
        //            new NormalDecorator(new AlertMessage("Second Message With A Beep")),
        //            new ErrorDecorator(new AlertMessage("Third Message With A Beep And In Red")),
        //            new SimpleMessage("Not Decorated.....")
        //        };

        //        foreach(var message in messages)
        //        {
        //            message.PrintMessage();
        //        }

        //        Console.Read();

        //    }

          
        // }



        //interface IMessage
        //{
        //    void PrintMessage();
        //}

        //abstract class Message : IMessage
        //{
        //    protected string _text;
        //    public Message(string text)
        //    {
        //        _text = text;
        //    }

        //    abstract public void PrintMessage();

        //    public class SimpleMessage : Message
        //    {
        //        public SimpleMessage(string text) : base(text)
        //        {

        //        }

        //        public override void PrintMessage()
        //        {
        //            Console.WriteLine(_text);
        //        }
        //    }

        //    public class AlertMessage : Message
        //    {
        //        public AlertMessage(string text) : base(text)
        //        {

        //        }

        //        public override void PrintMessage()
        //        {
        //            Console.Beep();
        //            Console.WriteLine(_text);
        //        }
        //    }

        //    public abstract class MessageDecorator : IMessage
        //    {
        //        protected Message _message;
        //        public MessageDecorator(Message message)
        //        {
        //            _message = message;
        //        }

        //        public abstract void PrintMessage();
        //    }


        //    public class NormalDecorator : MessageDecorator
        //    {
        //        public NormalDecorator(Message message) : base(message)
        //        {

        //        }

        //        public override void PrintMessage()
        //        {
        //            Console.ForegroundColor = ConsoleColor.Green;
        //            _message.PrintMessage();
        //            Console.ForegroundColor = ConsoleColor.White;
        //        }
        //    }

        //    public class ErrorDecorator : MessageDecorator
        //    {
        //        public ErrorDecorator(Message message) : base(message)
        //        {

        //        }

        //        public override void PrintMessage()
        //        {
        //            Console.ForegroundColor = ConsoleColor.Red;
        //            _message.PrintMessage();
        //            Console.ForegroundColor = ConsoleColor.White;
        //        }
        //    }

        #endregion

        #region. 2 Behavioral Pattern, Chain Of Responsibility
    
        abstract class ServiceHandler
        {

            public static void Main(string[] args)
            {
                var mechanic = new Mechanic();
                var detailer = new Detailer();
                var wheels = new WheelSpecialist();
                var qc = new QualityControl();

                qc.SetNextServiceHandler(detailer);
                wheels.SetNextServiceHandler(qc);
                mechanic.SetNextServiceHandler(wheels);


                Console.WriteLine("Car 1 is dirty");
                mechanic.Service(new Car { Requirements = ServiceRequirements.Dirty });
                Console.WriteLine();

                Console.WriteLine("Car 2 requires full service");
                mechanic.Service(new Car { Requirements = ServiceRequirements.Dirty | ServiceRequirements.EngineTune | ServiceRequirements.WheelAlignment });

                Console.Read();
            }

            [Flags]
            enum ServiceRequirements
            {
                None = 0,
                WheelAlignment = 1,
                Dirty = 2,
                EngineTune = 4,
                TestDrive
            }

            class Car
            {
                public ServiceRequirements Requirements { get; set; }

                public bool IsServiceComplete
                {
                    get
                    {
                        return Requirements == ServiceRequirements.None;
                    }
                }

            }
            protected ServiceHandler _nextServiceHandler;
            ServiceRequirements _servicesProvided;

             ServiceHandler(ServiceRequirements servicesProvided)
            {
                _servicesProvided = servicesProvided;
            }

            void Service(Car car)
            {
                if (_servicesProvided == (car.Requirements & _servicesProvided))
                {
                    Console.WriteLine($"{this.GetType().Name} providing {this._servicesProvided} services.");
                    car.Requirements &= ~_servicesProvided;
                }

                if (car.IsServiceComplete || _nextServiceHandler == null) return;
                else _nextServiceHandler.Service(car);
            }

            public void SetNextServiceHandler(ServiceHandler handler)
            {
                _nextServiceHandler = handler;
            }

            public class Detailer : ServiceHandler
            {
                public Detailer() : base(ServiceRequirements.Dirty) { }
            }

            public class Mechanic : ServiceHandler
            {
                public Mechanic() : base(ServiceRequirements.EngineTune) { }
            }

            public class WheelSpecialist : ServiceHandler
            {
                public WheelSpecialist() : base(ServiceRequirements.WheelAlignment) { }
            }

            public class QualityControl : ServiceHandler
            {
                public QualityControl() : base(ServiceRequirements.TestDrive) { }
            }
        }
        #endregion

        #region. 

    }
}

