
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ViccosLite.Core.Configuration;

namespace ViccosLite.Services.Configuration
{
    public interface ISettingService
    {
        void InsertSetting(Core.Domain.Configuration.Setting setting, bool clearCache = true);
        void UpdateSetting(Core.Domain.Configuration.Setting setting, bool clearCache = true);
        Core.Domain.Configuration.Setting GetSettingById(int settingId);
        void DeleteSetting(Core.Domain.Configuration.Setting setting);
        Core.Domain.Configuration.Setting GetSetting(string key, int storeId = 0, bool loadSharedValueIfNotFound = false);
        T GetSettingByKey<T>(string key, T defaultValue = default(T),
            int storeId = 0, bool loadSharedValueIfNotFound = false);
        void SetSetting<T>(string key, T value, int storeId = 0, bool clearCache = true);
        IList<Core.Domain.Configuration.Setting> GetAllSettings();
        bool SettingExists<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector, int storeId = 0)
            where T : ISettings, new();
        T LoadSetting<T>(int storeId = 0) where T : ISettings, new();
        void SaveSetting<T>(T settings, int storeId = 0) where T : ISettings, new();
        void SaveSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector,
            int storeId = 0, bool clearCache = true) where T : ISettings, new();
        void DeleteSetting<T>() where T : ISettings, new();
        void DeleteSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector, int storeId = 0) where T : ISettings, new();
        void ClearCache();
    }
}