namespace College_managemnt_system.DTOS
{
    public class TeachingAssistanceDTO
    {
        public int AssistantId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string NationalNumber { get; set; } = null!;

        public DateTime HiringDate { get; set; }

        public long AccountId { get; set; }
    }
}
