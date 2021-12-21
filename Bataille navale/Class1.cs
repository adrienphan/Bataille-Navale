using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bataille_navale
{
    internal class GameMethods
    {
        public static int lineCharacterToInt(char charToUnicode)
        {
            return (int)charToUnicode - (int)'A';
        }
        
    }
}
