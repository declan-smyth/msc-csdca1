using System.ComponentModel.DataAnnotations;

namespace BPCalculator
{
    // BP categories
    public enum BPCategory
    {
        [Display (Name = "")] None,
        [Display(Name="Low Blood Pressure")] Low,
        [Display(Name="Normal Blood Pressure")]  Normal,
        [Display(Name="Elevated Blood Pressure")] Elevated,
        [Display(Name ="High Blood Pressure - Stage 1")]  High1,
        [Display(Name ="High Blood Pressure - Stage 2")] High2,
        [Display(Name ="Hypertensive Crisis")] Crisis
    };

    public class BloodPressure
    {
        public const int SystolicMin = 70;
        public const int SystolicMax = 190;
        public const int DiastolicMin = 40;
        public const int DiastolicMax = 130;

        [Range(SystolicMin, SystolicMax, ErrorMessage = "Invalid Systolic Value")]
        public int Systolic { get; set; }                       // mmHG

        [Range(DiastolicMin, DiastolicMax, ErrorMessage = "Invalid Diastolic Value")]
        public int Diastolic { get; set; }                      // mmHG

        // calculate BP category
        public BPCategory Category
        {
            get
            {
              return this.IsCrisisPressure() ? BPCategory.Crisis :
                     this.IsHigh2Pressure() ? BPCategory.High2 :
                     this.IsHigh1Pressure() ? BPCategory.High1 :
                     this.IsElevatedPressure() ? BPCategory.Elevated :
                     this.IsNormalPressue() ? BPCategory.Normal : BPCategory.None;
            }
        }

        private bool IsLowPressure()
        {
            //this.browser = TestContext.Properties["browser"].ToString() != null ? this.TestContext.Properties["browser"].ToString() : default_browser;
            //bool result = (this.Systolic)
            return false;
        }

        private bool IsNormalPressue()
        {
           return (this.Diastolic < 80) && (this.Systolic < 120) ? true : false;
        }

        private bool IsElevatedPressure()
        {
            return (this.Diastolic < 80) && (this.Systolic >= 120 && this.Systolic < 130) ? true : false;        
        }

        private bool IsHigh1Pressure()
        {
            return (this.Diastolic < 80) && (this.Systolic >= 130 && this.Systolic < 140) ? true : ( this.Systolic < 180 ) && (this.Diastolic >= 80 && this.Diastolic < 90) ? true : false;
            
        }

        private bool IsHigh2Pressure()
        {
            return (this.Diastolic < 80) && (this.Systolic >= 140 && this.Systolic <= 180) ? true : (this.Systolic < 180) && (this.Diastolic >= 90 && this.Diastolic <= 120) ? true : false;
            
        }

        private bool IsCrisisPressure()
        {
            return (this.Diastolic > 120 || this.Systolic > 180) ? true : false;
        }
    }
}
