using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace List
{
    class Program
    {
        static void Main(string[] args)
        {
           ArrayList list1 = new ArrayList();

           ArrayList list2 = new ArrayList() {
           "ali",
           1,
           "",
           null,

           
           };
           string[] str = { "abc", "def", "ghi" };
           int a = 78;
           list1.AddRange(list2);
           list1.AddRange(str);
           list1.Add(a);
           Queue q = new Queue();
           q.Enqueue("Hello");
           q.Enqueue(3);
           list1.AddRange(q);
           list1.Insert(3, "happy");
            
      /*     for (int i = 0; i < list1.Count; i++) {
               Console.WriteLine(list1[i]);
           
           }*/

           List<int> numbers = new List<int>();

           numbers.Add(7);
           numbers.Add(3);
           numbers.Add(4);
           numbers.Add(5);
           numbers.Add(5);

           try
           {
               List<StudentDetails> std = new List<StudentDetails>(){
          new StudentDetails() { Id = 2, Name = "Ali" },
          new StudentDetails() { Id = 3, Name = "Ahmed" },
          new StudentDetails() { Id = 4, Name = "Aslam" },
          new StudentDetails() { Id = 5, Name = "Ali" },
       
        
         

      
      };
               List<StudentDetails> stds = new List<StudentDetails>(){
          new StudentDetails() { Id = 3, Name = "Ali" },
          new StudentDetails() { Id = 4, Name = "Ahmed" },
          new StudentDetails() { Id = 4, Name = "Aslam" },
          new StudentDetails() { Id = 5, Name = "Ali" },
        
         

      
      };
               StudentDetails s1 = new StudentDetails();
               s1.CompareTo(std, stds);
            

           }
           catch(Exception e) {
               Console.WriteLine("Something was wrong");
               Console.WriteLine(e.Message);
           
           }
         
            
    /*  var stdNames = from s in std
                     where s.Name == "Ali"
                     select s.Id;


      foreach (var i in stdNames)
      {
          Console.WriteLine(i);
      }*/
        }
    
    }
}
