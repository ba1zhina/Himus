using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himus_2._0
{
    static class Calculation
    {

        public static double calculationProtein(double Protein, double mass)
        {
            return Protein * mass * 0.01;
        }

        public static double calculationFat(double Fat, double mass)
        {
            return Fat * mass * 0.01;
        }

        public static double calculationCarbohydrates(double Carbohydrates, double mass)
        {
            return Carbohydrates * mass * 0.01;
        }
    }
}
