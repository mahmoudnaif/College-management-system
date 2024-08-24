namespace College_managemnt_system.ClientModels
{
    public class NameInputModel
    {
        public string firstName { get; set; } = null!;
        public string lastName { get; set; } = null!;
    }

    public class EditDepartmentInputModle
    {
        public int id { get; set; }
        public int departmentId { get; set; }
    }

    public class EditDateInputModel
    {
        public int id { get; set; }
        public DateTime date { get; set; }
    }

    public class EditPhoneNumberInputModel { 
        public int id { get; set; }
        public string phoneNumber { get; set; } = null!;
    }
}
