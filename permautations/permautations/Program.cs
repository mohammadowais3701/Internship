using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace permautations
{
    class Program
    {
        static void swap(ref char a, ref char b) {
            char temp;
            temp = a;
            a = b;
            b = temp;
        
        
        }

    static void calculatePermutation(char[] arr,int s, int e){
          int i;
          char temp;
          if (s == e)
          {
              for (i = 0; i <= e; i++)
              {
                  Console.Write(arr[i]);

              }
              Console.WriteLine();
             
          }
          else {
              for (i = s; i <= e; i++) {
                  swap( ref arr[s],ref  arr[i]);
                  calculatePermutation(arr, s + 1, e);
                  swap(ref arr[s], ref  arr[i]);
              
              
              }
          
          }
        
        }

        static void Main(string[] args)
        {
         /*   int n;
            Console.WriteLine("Enter Number of Elements");
            n = Convert.ToInt16(Console.ReadLine());


            string[] str=new string[n];
            char ch;
            for (int i = 0; i < n; i++) {
                ch = Convert.ToChar(Console.Read());
                str[i] = Convert.ToString(ch);
            }*/
            String s;
            Console.WriteLine("Enter String");

            s=Console.ReadLine();

            Char[] str = new Char[s.Length];
            str = s.ToArray();
            calculatePermutation(str,0,s.Length-1);

            

         //   a = s.ToArray();
           
            
         /*   Console.WriteLine("\n");
            for (int i = 0; i < str.Length; i++)
            {
                

                    for (int j = 0; j < str.Length; j++)
                    {
                        char[] temp=new char[str.Length];
                        str.CopyTo(temp, 0);
                       
                        
                        
                        char t;
                        if (i == j)
                        {
                            continue;
                        }
                        t = temp[i];
                        temp[i] = temp[j];
                        temp[j] = t;

                        for (int k = 0; k < str.Length; k++)
                        {
                            Console.Write(temp[k]);

                        }
                        Console.WriteLine();
                        




                    }

            }*/
        }
    }

      /*    for(int i =0; i<str.Length;i++){
              string[] temp =new string[2];
              int k=0;
             for(int j=0;j<str.Length;j++){
                 if (i == j) {
                     continue;
                 }
                 temp[k] = str[j];
                 k++;
              
              }
              
            
              Console.WriteLine(str[i]+""+temp[0]+""+temp[0+1]);
              Console.WriteLine(str[i] + "" + temp[1] + "" + temp[0]);*/
              

      
}
