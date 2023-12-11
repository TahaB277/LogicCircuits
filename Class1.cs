using System;
using System.IO;
using System.Linq;

namespace LogicCircuits
{

    public class LogicGates
    {
        public bool[] Inputs { get; set; }
        public string type;
        public bool Out;
        

        public LogicGates(string type, bool[] n) //making simple logic gates 
        {
            this.type = type;
            Inputs = n;
            switch (type.ToLower())
            {
                case "and":
                    this.Out = AND(Inputs);
                    break;
                case "or":
                    this.Out = OR(Inputs);
                    break;
                case "xor":
                    this.Out = XOR(Inputs);
                    break;
                case "nand":
                    this.Out = !(AND(Inputs));
                    break;
                case "nor":
                    this.Out = !(OR(Inputs));
                    break;


            }
        }
        private bool AND(bool[] inputs)
        {
            //bool c = true;
            //if (inputs.Contains(false))
            //    c = false;
            //return c;
            return !inputs.Contains(false);
        }
        private bool OR(bool[] inputs)
        {
            bool c = false;
            if (inputs.Contains(true))
                c = true;
            return c;
        }
        private bool XOR(bool[] inputs)
        {
            bool c = false;
            if (inputs.Count(x => x == true) % 2 == 1) c = true;
            return c;
        }
    }

    // Multiplexer class
    // (MSB in selector is the last element in the array !!!!)
    public class MUX 
    {
            
        public bool[] Select { get; set; }
        public bool[] Inputs { get; set; }
        public bool Out { get; private set; }

        private int counter;

        public MUX(bool[] n, bool[] s) //default constructor
        {
        if ((n.Length % 2 == 1) || (Math.Pow(2,s.Length) != n.Length))
            throw new InvalidDataException("Inputs and Selects do not match !");
            Inputs= n;
            Select= s;
            Out= GetOutput(n);
        }
            
        private MUX(bool a,bool b,bool s) // MUX 2 to 1
        {
            Inputs = new bool[2];
            Inputs[0]=a;
            Inputs[1]=b;
                
            if (s == false)
                Out = Inputs[0];
            else
                Out = Inputs[1];
        }

        private bool GetOutput(bool[] n)
        //methode to get output of MUX with n inputs and s selects
        //helper methode
        {
            counter = 0;
            return GetOutput(n, Select[0]);
        }
            private bool GetOutput(bool[] n, bool s)
            {
                if(n.Length ==2) //trivial case
                {
                    MUX mux = new MUX(n[0], n[1], s);
                    return mux.Out;
                }
                MUX[] arr = new MUX[n.Length / 2];
                bool[] inputOfNext = new bool[arr.Length];
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = new MUX(n[i*2],n[(i*2)+1],s);
                    inputOfNext[i] = arr[i].Out;
                }
                counter++;
                return GetOutput(inputOfNext, Select[counter]);
            }
    }

    // Decoders class
    // (MSB in inputs is the last element in the array !!!!)
    public class Decoders
    {
        public bool[] Inputs;
        public bool[] Outputs;
        public bool Enable;

        private int counter;
        
        public Decoders(bool[] n,bool En) // Default constructor
        {
            Inputs = n;
            Enable = En;
            Outputs = GetOutputs(En);
        }

        // 1 to 2 Decoder
        private Decoders(bool a,bool En)
        {
            Inputs = new bool[1];
            Inputs[0]=a;
            Enable=En;
            Outputs = new bool[2];
            if (En)
            {
                if (!a)
                {
                    Outputs[0]=true;
                    Outputs[1]=false;
                }
                else
                {
                    Outputs[0] = false;
                    Outputs[1] = true;
                } 
            }
            else
            {
                Outputs[0] = false;
                Outputs[1] = false;
            }
                
        }
        
        //fct to get the outputs
        //helper method
        private bool[] GetOutputs(bool En)
        {
            counter = Inputs.Length;
            bool[] E = new bool[1];
            E[0]=En;
            return GetOutputs(E);
        }
        private bool[] GetOutputs(bool[] En)
        {
            if(counter == 1) //trivial case
            {
                Decoders[] decoders = new Decoders[En.Length];
                bool[] outputs = new bool[En.Length*2];
                for(int i=0;i< decoders.Length;i++)
                {
                    decoders[i] = new Decoders(Inputs[counter - 1],En[i]);
                    outputs[i*2]=decoders[i].Outputs[0];
                    outputs[i*2+1]=decoders[i].Outputs[1];
                }
                return outputs;
            }
            Decoders[] decoders1 = new Decoders[En.Length];
            bool[] outputs1 = new bool[En.Length * 2];
            for (int i = 0; i < decoders1.Length; i++)
            {
                decoders1[i] = new Decoders(Inputs[counter - 1], En[i]);
                outputs1[i * 2] = decoders1[i].Outputs[0];
                outputs1[i * 2 + 1] = decoders1[i].Outputs[1];
            }
            counter--;
            return GetOutputs(outputs1);
        }
        
         
    }
    
}
