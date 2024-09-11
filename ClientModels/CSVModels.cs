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
}
