namespace College_managemnt_system.DTOS
{
    public class GroupDTO
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; } = null!;

        public int StudentsYear { get; set; }

        public int SemesterId { get; set; }
    }
}
