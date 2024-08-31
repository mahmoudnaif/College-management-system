namespace College_managemnt_system.ClientModels
{
    public class ClassRoomInputModel
    {
        public int RoomNumber { get; set; }

        public int Building { get; set; }

        public int Capacity { get; set; }
    }

    public class CapacityEditModel
    {
        public int RoomNumber { get; set; }
        public int classRoomCapacity { get; set; }
    }
}
