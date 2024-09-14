namespace College_managemnt_system.ClientModels
{
    public class StudentErrorSheet
    {
        public StudentErrorSheet()
        {
            
        }
        public StudentErrorSheet(int rowNumber)
        {
            this.rowNumber = rowNumber;
        }

        public int rowNumber { get; set; }
        public bool FirstName { get; set; } = true;
        public bool FathertName { get; set; } = true;
        public bool GrandfatherName { get; set; } = true;
        public bool  LastName { get; set; } = true;
        public bool Phone { get; set; } = true;
        public bool NationalNumber { get; set; } = true;
        public bool Email { get; set; } = true;
        public int numberOfErrors {  get; set; } = 0; //can be auto incremneted using a backing field and automatic setters and getters
                                                      //but TBH I don't have the ernergy to rn :)
    }

    public class CourseErrorSheet
    {

        public CourseErrorSheet(int rowNumber,int prereqsLength)
        {
            this.rowNumber= rowNumber;
            PrereqsCoursesCodes = prereqsLength > 0 ? Enumerable.Repeat(true, prereqsLength).ToList() : [];
        }

        public int rowNumber { get; set; }
        public bool CourseName { get; set; } = true;
        public bool CourseCode { get; set; } = true;
        public bool Credits { get; set; } = true;
        public bool DepartmentName { get; set; } = true;
        public bool DepartmentId { get; set; } = true;
        public List<bool> PrereqsCoursesCodes { get; set; }
        public int numberOfErrors { get; set; } = 0;

    }


    public class ProfErrorSheet 
    {
        public ProfErrorSheet(int rowNumber)
        {
            this.rowNumber = rowNumber;
        }

        public int rowNumber { get; set; }
        public bool FirstName { get; set; } = true;
        public bool LastName { get; set; } = true;
        public bool  NationalNumber { get; set; } = true;
        public bool Phone { get; set; } = true;
        public bool HiringDate { get; set; } = true;
        public bool DepartmentName { get; set; } = true;
        public bool DepartmentId { get; set; } = true;
        public bool email { get; set; } = true;
        public int numberOfErrors { get; set; } = 0;
    }



}



