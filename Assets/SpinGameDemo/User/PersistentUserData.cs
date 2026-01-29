using System;
using UnityEditor;
using UnityEngine;

namespace SpinGameDemo.User
{
    public static class PersistentUserData
    {
        private const string InventoryKey = "UserInventory";
        private static UserData _data;

        private static UserData Data
        {
            get
            {
                if (_data == null)
                {
                    TryLoad();
                }
                return _data;
            }
        }
        private static void TryLoad()
        {
            string json = PlayerPrefs.GetString(InventoryKey, string.Empty);
            if (!string.IsNullOrEmpty(json))
            {
                _data = JsonUtility.FromJson<UserData>(json);
            }
            else
            {
                _data = new UserData();
            }
        }

        private static void Save()
        {
            string json = JsonUtility.ToJson(Data);
            PlayerPrefs.SetString(InventoryKey, json);
            PlayerPrefs.Save();
        }
        public static void SetCash(int amount)
        {
            Data.Cash = amount;
            Save();
        }
        public static void AddCash(int amount)
        {
            Data.Cash += amount;
            Save();
        }
        public static int GetCash()
        {
            return Data.Cash;
        }
        
        public static void SetZone(int zone)
        {
            Data.Zone = zone;
            Save();
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