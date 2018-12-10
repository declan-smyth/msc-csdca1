using System.ComponentModel.DataAnnotations;

namespace BPCalculator
{
   /// <summary>
   ///  Blood Pressure Category enumeration
   ///  Additional categories added based on the UK NHS breakdown
   /// </summary>
    public enum BPCategory
    {
        [Display(Name = "No Blood Pressure Calculated")] None,
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

        [Range(SystolicMin, SystolicMax, ErrorMessage = "Invalid Systolic Value provided")]
        public int Systolic { get; set; }                       // mmHG

        [Range(DiastolicMin, DiastolicMax, ErrorMessage = "Invalid Diastolic Value provided")]
        public int Diastolic { get; set; }                      // mmHG

        
        /// <summary>
        /// Returns the blood pressure category based on the 
        /// Systolic Value and Diastolic Value provided
        /// </summary>
        public BPCategory Category
        {
            get
            {
                BPCategory rtnValue = BPCategory.None;

                if (this.IsCrisisPressure()) { return BPCategory.Crisis; }
                if (this.IsHigh2Pressure()) { return  BPCategory.High2; }
                if (this.IsHigh1Pressure()) { return  BPCategory.High1; }
                if (this.IsElevatedPressure()) { return  BPCategory.Elevated; }
                if (this.IsNormalPressure()) { return BPCategory.Normal; }
                if (this.IsLowPressure()) { return BPCategory.Low; }

                return rtnValue;
            }
        }

        /// <summary>
        /// Method to indciate if the Blood Pressure is Low
        /// </summary>
        /// <returns>bool</returns>
        private bool IsLowPressure()
        {
            return (this.Diastolic < 60) || (this.Systolic < 90);
        }

        /// <summary>
        /// Method to indciate if the Blood Pressure is Normal
        /// </summary>
        /// <returns>bool</returns>
        private bool IsNormalPressure()
        {
            bool v = (this.Diastolic >= 60 && this.Diastolic < 80);
            return v && (this.Systolic > 80 && this.Systolic < 120);
        }

        /// <summary>
        /// Method to indiciate if the Blood Pressue is Elevated
        /// </summary>
        /// <returns>bool</returns>
        private bool IsElevatedPressure()
        {

            return ((this.Diastolic > 60 && this.Diastolic < 80) && (this.Systolic >= 120 && this.Systolic < 130));
        }

        /// <summary>
        /// Method to indciate if the Blood Pressure is High Blood Pressure
        /// </summary>
        /// <returns></returns>
        private bool IsHigh1Pressure()
        {
           
            if (this.Systolic >= 130 && this.Systolic < 140)
            {
                return true;
            }
            return (this.Diastolic >= 80 && this.Diastolic < 90);

        }

        /// <summary>
        /// Method to indciate if the Blood Pressure is High Blood Pressure
        /// </summary>
        /// <returns></returns>
        private bool IsHigh2Pressure()
        {
            
            if (this.Systolic >= 140 && this.Systolic <= 180)
            {
                return true;
            }
            return ((this.Systolic < 180) && (this.Diastolic >= 90 && this.Diastolic <= 120));
        }

        /// <summary>
        /// Method to indciate if the Blood Pressure is Critical
        /// </summary>
        /// <returns>bool</returns>
        private bool IsCrisisPressure()
        {
            return this.Diastolic > 120 || this.Systolic > 180;
        }
    }
}
