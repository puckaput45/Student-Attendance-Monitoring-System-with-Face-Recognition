using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhomIsThisFace.Model
{
    class Student
    {
        public Student(String ID, String StudentID, String Name, String LastName)
        {
            this.ID = ID;
            this.StudentID = StudentID;
            this.Name = Name;
            this.LastName = LastName;
        }

        public String ID { get; set; }

        public String StudentID { get; set; }

        public String Name { get; set; }

        public String LastName { get; set; }

        public Image<Bgr, Byte>[] Image { get; set; }

        public int NumImage { get; set; }
    }
}
