using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AttendanceCRM.Models;

public partial class AttendanceCRMContext : DbContext
{
    public AttendanceCRMContext()
    {
    }

    public AttendanceCRMContext(DbContextOptions<AttendanceCRMContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Users> Users { get; set; }
    public virtual DbSet<Courses> Courses { get; set; }
    public virtual DbSet<CourseYear> CourseYear { get; set; }
    public virtual DbSet<CourseSubject> CourseSubject { get; set; }
    public virtual DbSet<CourseSemester> CourseSemester { get; set; }
    public virtual DbSet<CourseStudent> CourseStudent { get; set; }
    public virtual DbSet<AttendanceMonthYear> AttendanceMonthYear { get; set; }
    public virtual DbSet<Attendance> Attendance { get; set; }
    public virtual DbSet<Notification> Notification { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:AttendanceCRM");

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{

    //    OnModelCreatingPartial(modelBuilder);
    //}

    //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
