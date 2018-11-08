using System.ComponentModel.DataAnnotations;

namespace BPCalculator
{
    // BP categories
    public enum BPCategory
    {
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
               if (this.Diastolic < 80)
                {
                    if (this.Systolic < 120)
                    {
                        return BPCategory.Normal;
                    }
                    else if (this.Systolic >= 120 & this.Systolic < 130)
                    {
                        return BPCategory.Elevated;
                    }
                    else
                    {
                        if (this.Systolic >= 130 & this. Systolic < 140)
                        {
                            return BPCategory.High1;
                        }
                        else if (this.Systolic >= 140 & this.Systolic <= 180)
                        {
                            return BPCategory.High2;
                        }
                        else
                        {
                            return BPCategory.Crisis;
                        }
                    }
                }
               else if (this.Diastolic >= 80 & this.Diastolic < 90)
                {
                    return BPCategory.High1;
                }
                else if (this.Diastolic >= 90 & this.Diastolic <= 120)
                {
                    return BPCategory.High2;
                }
                else
                {
                    return BPCategory.Crisis;
                }

            }
        }
    }
}
