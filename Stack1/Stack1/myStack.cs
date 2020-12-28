using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack1
{
    class myStack
    {
       
        List<int> stack;


    public  myStack() {
       
           
            stack = new List<int>();
        }
    public  void push(int n) {
          
            stack.Insert(0,n);
        
        }
   public  int pop() {

       int k = stack[0];
       stack.RemoveAt(0);
       return k;

   


        }

   public void printList() {
       foreach (var i in stack) {
           Console.WriteLine(i);
       }
   
   }

    }
}
