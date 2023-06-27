using DiplomApplication.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomApplication.Classes
{
    internal class DataBaseClass
    {
        //public static readonly User051Context DataBase = new User051Context();
        public static readonly PostgresContext DataBase = new PostgresContext();

        public static bool isLogined;

        public static List<BasketClass> basketClassList = new List<BasketClass>();

        public static int currentUserId;

        public static int switchValue;

        public static int currentDir = 0;

        public static bool WasGuestRegistered;

    }
}
