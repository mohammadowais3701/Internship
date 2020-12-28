using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace List
{
    class StudentDetails
    {
        
        public int Id
        
        { 
            
            get;
            
            set;
        }
      

        public String Name
        {
            get;
            set;

        }
        public void CompareTo(List<StudentDetails> stdDetails, List<StudentDetails> stdDeatils1)
        {


            for (int i = 0; i < stdDeatils1.Count; i++)
            {
                if (stdDeatils1[i].Id == stdDetails[i].Id)
                {
                    Console.WriteLine(stdDetails[i].Name);

                }

            }

        }
    }
}
