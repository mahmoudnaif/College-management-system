namespace College_managemnt_system.ClientModels
{
    public class ClassRoomInputModel
    {
        public string RoomNumber { get; set; } = null!;

        public string Building { get; set; } = null!;

        public int Capacity { get; set; }
    }

    public class CapacityEditModel
    {
        public int classRoomId { get; set; }
        public int classRoomCapacity { get; set; }
    }
}
