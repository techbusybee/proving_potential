using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bughound.Models
{
    public class BugModel
    {
        private int defectId;
        private string program;
        private string reporttype;
        private string severity;
        private string problemSummary;
        private bool reproducible;
        private string problem;
        private string suggestedFix;
        private string reportedBy;
        private DateTime sqlDate;
        private string functionalArea;
        private string assignedTo;
        private string comments;
        private string sqlStatus;
        private string priority;
        private string resolution;
        private string resolutionVersion;
        private string resolvedBy;
        private DateTime resolvedByDate;
        private string testedBy;
        private DateTime testedbyDate;
        private bool treatAsDeferred;

        public int DefectId
        {
            get { return defectId; }
            set { defectId = value; }
        }
        
        public string Program
        {
            get { return program; }
            set { program = value; }
        }
       
        public string Reporttype
        {
            get { return reporttype; }
            set { reporttype = value; }
        }
       
        public string Severity
        {
            get { return severity; }
            set { severity = value; }
        }
       
        public string ProblemSummary
        {
            get { return problemSummary; }
            set { problemSummary = value; }
        }
        
        public bool Reproducible
        {
            get { return reproducible; }
            set { reproducible = value; }
        }
       
        public string Problem
        {
            get { return problem; }
            set { problem = value; }
        }
         
        public string SuggestedFix
        {
            get { return suggestedFix; }
            set { suggestedFix = value; }
        }
      
        public string ReportedBy
        {
            get { return reportedBy; }
            set { reportedBy = value; }
        }
        
        public DateTime SqlDate
        {
            get { return sqlDate; }
            set { sqlDate = value; }
        }
       
        public string FunctionalArea
        {
            get { return functionalArea; }
            set { functionalArea = value; }
        }
       
        public string AssignedTo
        {
            get { return assignedTo; }
            set { assignedTo = value; }
        }
        
        public string Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        
        public string SqlStatus
        {
            get { return sqlStatus; }
            set { sqlStatus = value; }
        }
        
        public string Priority
        {
            get { return priority; }
            set { priority = value; }
        }
       
        public string Resolution
        {
            get { return resolution; }
            set { resolution = value; }
        }
       
        public string ResolutionVersion
        {
            get { return resolutionVersion; }
            set { resolutionVersion = value; }
        }
       
        public string ResolvedBy
        {
            get { return resolvedBy; }
            set { resolvedBy = value; }
        }
       
        public DateTime ResolvedByDate
        {
            get { return resolvedByDate; }
            set { resolvedByDate = value; }
        }
       
        public string TestedBy
        {
            get { return testedBy; }
            set { testedBy = value; }
        }
       
        public DateTime TestedbyDate
        {
            get { return testedbyDate; }
            set { testedbyDate = value; }
        }       

        public bool TreatAsDeferred
        {
            get { return treatAsDeferred; }
            set { treatAsDeferred = value; }
        }
    }
}