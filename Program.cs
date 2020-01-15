using System;
using static System.Console;

namespace _Trains
{
    class MyError: Exception
         {
        public MyError(string message) : base(message) { }
         }


    abstract class  RailCar
    {
        string name;
        RailCar next;

        

        public RailCar(string name)
        {
            if ( name.Length < 2 ) 
                throw (new MyError("The name of a Rail Car has to be at least 2 characters long."));
            else
            {
                this.name = name;
                this.next = null;
            }
            
        }

        public abstract bool isProfitable();
        public abstract string toString();
        public abstract RailCar clone();

        public RailCar getNext(){return this.next;}
        public void setNext(RailCar nextCar)
        {
            this.next = nextCar;
        }
        public string getName(){return this.name;}

    }

    class PassengerCar : RailCar
    {
        int capacity;

        public PassengerCar(string name, int c): base(name)
        {
            if ( c <= 0 )
                throw (new MyError("The capacity of a Passanger Car must be strictly positive."));
            else capacity = c;
        }

        public override bool isProfitable() => capacity>40 ? true:false;
        public override string toString() {return (this.getName() + capacity.ToString());}
        public override RailCar clone() {return (PassengerCar) this.MemberwiseClone();}
    }
    
    class RestaurantCar : RailCar
    {
        int numberOfTables=1;

        public RestaurantCar(string name, int c): base(name)
        {
            if ( c > 0 ) numberOfTables = c;
        }

        public override bool isProfitable()=> numberOfTables>=20 ? true:false;
        public override string toString(){return (this.getName() + numberOfTables.ToString());}
        public override RailCar clone(){return (RestaurantCar) this.MemberwiseClone();}
    }
    
    class Locomotive 
    {
        RailCar first;

        public void attachRailCar(RailCar newCar)
        {
            if (first == null) first = newCar;
            else 
            {
                RailCar append = first;
                bool insert = false;

                // while (append.getNext() != null) append=append.getNext();
                //append.setNext(newCar);

                if (string.Compare(newCar.toString(), append.toString()) == -1)
                {
                    newCar.setNext(append);
                    first = newCar;
                }
                else
                while (append != null && !insert)
                {
                    if (append.getNext() == null ) 
                    {
                        insert = true;
                        append.setNext(newCar);
                    }
                    else if ( string.Compare(newCar.toString(), append.getNext().toString()) == -1) 
                    {
                        insert = true;
                        newCar.setNext(append.getNext());
                        append.setNext(newCar);
                    }
                    append = append.getNext();
                }
            
            }
        }

        public RailCar getFirstRailCar(){return first;}
        public void setFirstRailCar(RailCar R) => first = R;
        
    }
       
    class Test
    {
        static Locomotive RemoveUnprofitable (Locomotive L)
        {
        RailCar A = L.getFirstRailCar();
        RailCar B = A.getNext();

        if (B == null) 
        {
            if (!A.isProfitable()) L.setFirstRailCar(null);
        }

        else 
        {
            while (A != null && B != null)
        {
            if (!A.isProfitable())
            { 
                L.setFirstRailCar(B); 
                A = B;
                B = B.getNext();
            }  
            else if (!B.isProfitable())
                 {
                    B = B.getNext();
                    A.setNext(B);
                 }  
                 else 
                 {
                    A = B;
                    B = B.getNext();
                 } 
           
        }
        if (A != null) {if (!A.isProfitable()) L.setFirstRailCar(null);}
        }
       
        return L;
        }

        static void PrintTrain(Locomotive train)
        {
            Write("\n");
            RailCar railCar = train.getFirstRailCar();
            Write("L");
            while (railCar != null) 
            {
              Write($"-{railCar.toString()}");
              railCar = railCar.getNext();
            }
            
        }

        static void Main()
        {
            Locomotive l = new Locomotive();
           
            PassengerCar pc1 = new PassengerCar("PC",50);
            PassengerCar pc2 = new PassengerCar("PC",55);
            PassengerCar pc3 = new PassengerCar("PC",40);
                     
            RestaurantCar rc1 = new RestaurantCar("RC",5);
            RestaurantCar rc2 = new RestaurantCar("RCZ",45);

           // WriteLine($"{pc3.toString()}, {pc3.isProfitable()}");
           // pc3 = (PassengerCar) pc1.clone();
           // WriteLine($"{pc3.toString()}, {pc3.isProfitable()}");
            
            l.attachRailCar(rc1);
            l.attachRailCar(rc2);

            l.attachRailCar(pc1);
            l.attachRailCar(pc2);
            l.attachRailCar(pc3);

            PrintTrain(l);
            PrintTrain(RemoveUnprofitable(l));

        
        }
    }
}
