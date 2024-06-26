//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SchoolSchedule
{
    using System;
    using System.Collections.Generic;

    public partial class Lesson
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Lesson()
        {
            this.Attendences = new HashSet<Attendence>();
        }

        public int ID { get; set; }
        public Nullable<int> SubjectID { get; set; }
        public string DayOfWeek { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public Nullable<System.TimeSpan> EndTime { get; set; }
        public Nullable<int> ScheduleId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendence> Attendences { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Schedule Schedule { get; set; }


        public override string ToString()
        {
            return $"{Subject.SubjectName}\n {StartTime?.ToString(@"hh\:mm")}-{EndTime?.ToString(@"hh\:mm")}";
        }
    }
}
