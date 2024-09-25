namespace College_managemnt_system.DTOS
{
    public class StudentSheet
    {
        
        public StudentSheet(int studentId,string firstName,string fatherName, string grandFatherName, string lastName) {
            this.studentId = studentId;
            fullName = $"{firstName} {fatherName} {grandFatherName} {lastName}";
        }
        public int studentId { get; }
        public string fullName { get; }

    }

    public class StudentSheetByGroup
    {
        public int groupId { get; set; }
        public string groupName { get; set; } = null!;
        public List<StudentSheet> studentsSheet { get; set; } = [];
    }


}
