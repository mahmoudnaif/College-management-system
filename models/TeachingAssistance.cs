using System;
using System.Collections.Generic;

namespace College_managemnt_system.models;

public partial class TeachingAssistance //* TeachingAssistant
{
    public int AssistantId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string NationalNumber { get; set; } = null!;

    public DateTime HiringDate { get; set; }

    public long AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<AssistantsJoinscourseSemester> AssistantsJoinscourseSemesters { get; set; } = new List<AssistantsJoinscourseSemester>();
}
