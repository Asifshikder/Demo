using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class School
    {
        [Key]
        public int SchoolID { get; set; }
        public string SchoolName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int CourseStructure { get; set; }
        public string ApplicationPA { get; set; }
        public string AcceptedPA { get; set; }
        public decimal SuccessRatio { get; set; }
        public bool IsGraduateEntryAvailable { get; set; }
        public bool FoundationOrAccessCoursesAvailable  { get; set; }
        public int IntercalatedBSc   { get; set; }
        public string GCSESubjectsRequired { get; set; }
        public string A_LevelSubjectsRequired  { get; set; }
        public string A_LevelGradesRequired   { get; set; }
        public string ScottishHighersSubjectsRequired   { get; set; }
        public string ScottishHigherGradesRequired    { get; set; }
        public string ScottishAdvancedHighersSubjectsRequired { get; set; }
        public string ScottishAdvancedHighersGradesRequired { get; set; }
        public string IBSubjectsRequired { get; set; }
        public string IBGradesRequired { get; set; }
        public bool UCATRequired  { get; set; }
        public string HowisUCATused  { get; set; }
        public bool BMATRequired  { get; set; }
        public string HowisBMATUsed   { get; set; }
        public int InterviewStyle   { get; set; }
        public string Fees_EUStudents { get; set; }
        public string Fees_NonEUStudents { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}