namespace College_managemnt_system.DTOS
{
    public class ClassRoomDTO
    {
        public int ClassroomId { get; set; }

        public string RoomNumber { get; set; } = null!;

        public string Building { get; set; } = null!;

        public int Capacity { get; set; }
    }
}