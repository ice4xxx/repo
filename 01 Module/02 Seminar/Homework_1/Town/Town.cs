using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using System.Transactions;

namespace Town
{
    [Serializable]
    public class Street
    {
        //Название улицы
        private string mName;

        //массив номеров домов
        private List<int> mHouses;


        public string name
        {
            get { return mName; }
            set { mName = value; }
        }

        public List<int> houses
        {
            get { return mHouses; }
            set { mHouses = value; }
        }


        public static int operator ~(Street street) => street.mHouses.Count;

        public static bool operator !(Street street)
        {
            bool isThere = false;
            for (int i = 0; i < street.mHouses.Count; i++)
            {
                if (street.mHouses[i] == 7)
                {
                    isThere = true;
                }
            }

            return isThere;
        }

        public Street(string name, List<int> houses)
        {
            this.mName = name;
            this.mHouses = houses;
        }

        public Street()
        {

        }

        public string ToString()
        {
            string returned = mName;

            for (int i = 0; i < mHouses.Count; i++)
            {
                returned += $" {mHouses[i]}";
            }

            return returned;
        }

    }
}