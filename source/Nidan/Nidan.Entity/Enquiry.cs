namespace Nidan.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Enquiry")]
    public partial class Enquiry
    {
        [Key]
        public int EnquiryId { get; set; }

        [Required]
        [StringLength(500)]
        public string CandidateName { get; set; }

        public long ContactNo { get; set; }

        [StringLength(500)]
        public string EmailId { get; set; }

        public int Age { get; set; }

        [Required]
        [StringLength(500)]
        public string Qualification { get; set; }

        [Required]
        public string Address { get; set; }

        [StringLength(500)]
        public string GuardianName { get; set; }

        public long? GuardianContactNo { get; set; }

        [StringLength(500)]
        public string Occupation { get; set; }

        [StringLength(500)]
        public string Religion { get; set; }

        [StringLength(100)]
        public string CatagoryCode { get; set; }

        [Required]
        [StringLength(100)]
        public string Gender { get; set; }

        [Required]
        [StringLength(500)]
        public string EducationalQualification { get; set; }

        [StringLength(100)]
        public string YearOFPassOut { get; set; }

        [StringLength(100)]
        public string Marks { get; set; }

        [StringLength(500)]
        public string AreaOfInterest { get; set; }

        [StringLength(500)]
        public string HowDidYouKnowAboutUs { get; set; }

        [StringLength(100)]
        public string PreTrainingStatus { get; set; }

        [StringLength(100)]
        public string EmploymentStatus { get; set; }

        [StringLength(100)]
        public string Promotional { get; set; }

        [Column(TypeName = "date")]
        public DateTime EnquiryDate { get; set; }

        [Required]
        [StringLength(100)]
        public string Place { get; set; }

        [Required]
        [StringLength(500)]
        public string CounselledBy { get; set; }

        [Required]
        [StringLength(500)]
        public string CourseOffered { get; set; }

        public DateTime? PreferTiming { get; set; }

        public string Remarks { get; set; }

        public int CentreId { get; set; }

        public int OrganisationId { get; set; }

        public virtual Organisation Organisation { get; set; }

        public virtual Centre Centre { get; set; }
    }

}
