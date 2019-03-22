using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using HospitalAPI.Models;

namespace HospitalAPI.DAL
{
    public class HospitalContext:DbContext
    {
        public HospitalContext() : base("HospitalContext")
        {

        }

        public DbSet<AboutAdvantage> AboutAdvantages { get; set; }
        public DbSet<AboutBody> AboutBodies { get; set; }
        public DbSet<AboutCard> AboutCards { get; set; }
        public DbSet<AboutIntro> AboutIntros { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<CareCenter> CareCenters { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Counter> Counters { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentCard> DepartmentCards { get; set; }
        public DbSet<DescCard> DescCards { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Number> Numbers { get; set; }
        public DbSet<OpeningHour> OpeningHours { get; set; }
        public DbSet<PhoneView> PhoneViews { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Email> Emails { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Number>().Property(n => n.DoctorSliders)

        //    base.OnModelCreating(modelBuilder);
        //}
    }
}