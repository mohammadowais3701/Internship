using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegatesExample_15_1_2020_
{       public delegate Employee EmpDelg(int id,string name,int age,int sale=4);
    class Program
    {
       
        static void Main(string[] args)
        {
            try
            {
                EmpDelg emp_del = new EmpDelg(GetEmployee);
                Employee emp = emp_del(2, "Ali", 23);
                EmpDelg s_emp_dele = new EmpDelg(GetSalesEmployee);
                SalesEmployee s_emp = (SalesEmployee)s_emp_dele(2, "Ahmed", 21, 08);
                Console.WriteLine(emp.getEmployee());
                Console.WriteLine(s_emp.getSalesEmployee());
            }
            catch (Exception ex){
                Console.WriteLine(ex.Message);
            }
         }
        public static Employee GetEmployee(int id, string name,int age,int sale)
        {
                return new Employee(id, name, age); 
        }
        public static SalesEmployee GetSalesEmployee(int id, string name, int age,int saleNo)
        {
            return new SalesEmployee(id,name,age,saleNo);
        }
      
    }

  public  class Employee {
        int id;
        string name;
        int age;
        public Employee() { }
        public Employee(int id, string name, int age) {
            this.id = id;
            this.name = name;
            this.age = age;
        
        }

       public string getEmployee() { 
       return "Emp Name="+name+",Emp_Id="+id+",Emp_age="+age;
       }

       public string getEmployee(int s_no) {
           return "S_No="+s_no+","+getEmployee();
       }
    
    }

   public class SalesEmployee: Employee {
        int saleNumber;
        public SalesEmployee() { }
        public SalesEmployee(int id, string name, int age,int saleNumber):base( id, name, age)
        {
            this.saleNumber = saleNumber;       
        }

        public string getSalesEmployee()
        {
            return getEmployee(saleNumber);
        }
    
    }
}
