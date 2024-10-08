﻿using System;
using System.Collections.Generic;

namespace College_managemnt_system.models;

public partial class Student
{
    public int StudentId { get; set; }

    public string FirstName { get; set; } = null!;

    public string FathertName { get; set; } = null!;

    public string GrandfatherName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string NationalNumber { get; set; } = null!;

    public float Cgpa { get; set; }

    public DateTime EnrollmentDate { get; set; }

    public long AccountId { get; set; }

    public int TotalHours { get; set; }

    public int? DepartmentId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Department? Department { get; set; }

    public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();

    public virtual ICollection<StudentsJoinsgroup> StudentsJoinsgroups { get; set; } = new List<StudentsJoinsgroup>();
}
