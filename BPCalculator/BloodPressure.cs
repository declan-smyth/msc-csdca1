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
                BPCategory rtnValue = BPCategory.None;

                if (this.IsCrisisPressure()) { rtnValue =  BPCategory.Crisis; }
                if (this.IsHigh2Pressure()) { rtnValue =  BPCategory.High2; }
                if (this.IsHigh1Pressure()) { rtnValue =  BPCategory.High1; }
                if (this.IsElevatedPressure()) { rtnValue =  BPCategory.Elevated; }
                if (this.IsNormalPressure()) { rtnValue = BPCategory.Normal; }
                if (this.IsLowPressure()) { rtnValue = BPCategory.Low; }


                return rtnValue;
            }
        }

        private bool IsLowPressure()
        {
            return (this.Diastolic < 60) || (this.Systolic < 90);
        }

        private bool IsNormalPressure()
        {
            bool v = (this.Diastolic > 60 && this.Diastolic < 80);
            return v && (this.Systolic > 80 && this.Systolic < 120);
        }

        private bool IsElevatedPressure()
        {

            return ((this.Diastolic > 60 && this.Diastolic < 80) && (this.Systolic >= 120 && this.Systolic < 130));
        }

        private bool IsHigh1Pressure()
        {
           
            if ((this.Diastolic < 80) && (this.Systolic >= 130 && this.Systolic < 140))
            {
                return true;
            }
            return (this.Systolic < 180) && (this.Diastolic >= 80 && this.Diastolic < 90);

        }

        private bool IsHigh2Pressure()
        {
            
            if ((this.Diastolic < 80) && (this.Systolic >= 140 && this.Systolic <= 180))
            {
                return true;
            }
            return (this.Systolic < 180) && (this.Diastolic >= 90 && this.Diastolic <= 120);
        }

        private bool IsCrisisPressure()
        {
            return this.Diastolic > 120 || this.Systolic > 180;
        }
    }
}
