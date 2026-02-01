using System;
using UnityEditor;
using UnityEngine;

namespace SpinGameDemo.User
{
    public static class PersistentUserData
    {
        private static UserData _data = new UserData();

        private static UserData Data
        {
            get
            {
                return _data;
            }
        }

        public static void SetCash(int amount)
        {
            Data.Cash = amount;
        }

        public static void AddCash(int amount)
        {
            Data.Cash += amount;
        }

        public static int GetCash()
        {
            return Data.Cash;
        }
        
        public static void SetZone(int zone)
        {
            Data.Zone = zone;
        }

        public static int GetZone()
        {
            return Data.Zone;
        }

#if UNITY_EDITOR
        [MenuItem("Spin/User/Reset Zone")]
        public static void ResetZone()
        {
            SetZone(0);
            Debug.Log("User zone reset to 0");
        }

        [MenuItem("Spin/User/Reset Cash")]
        public static void ResetCash()
        {
            SetCash(0);
            Debug.Log("User cash reset to 0");
        }
#endif
    }
    
    [Serializable]
    public class UserData
    {
        public int Cash;
        public int Zone;
    }
}