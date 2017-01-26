using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalPollenProject.Core.Concepts
{
    // Location in Decimal Degrees
    [ComplexType]
    public class Age
    {
        public Age(int ageCalYear)
        {
            this.YearsBeforePresent = ageCalYear - 1950;
            this.CalendarAge = ageCalYear;
        }

        public int YearsBeforePresent { get; private set; }
        public int CalendarAge { get; private set; }
    }
}