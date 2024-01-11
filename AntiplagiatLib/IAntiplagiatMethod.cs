using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiplagiatLib
{
    internal interface IAntiplagiatMethod
    {
        static Dictionary<string, double> Calculate(string pathToFile, string pathToStandartFiles) => throw new NotImplementedException();
    }
}
