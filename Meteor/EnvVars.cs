﻿using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Meteor
{
    public static partial class EnvVars
    {
        private static readonly IDictionary KeyValues = new Dictionary<object, object>();
        public static string DbUri { get; private set; }

        static EnvVars()
        {
            DbUri = "";
            foreach (DictionaryEntry kv in Environment.GetEnvironmentVariables())
                Set(kv.Key, kv.Value);
        }

        public static string? Get(string key) => Get<string>(key);
        public static T? Get<T>(string key)
        {
            if (!KeyValues.Contains(key))
                return default;

            return (T)KeyValues[key];
        }

        public static void Set(object key, object value)
        {
            if (KeyValues.Contains(key))
                KeyValues[key] = value;
            else
                KeyValues.Add(key, value);
            
            switch (key)
            {
                case EnvVarKeys.DbUri:
                    DbUri = (string)value;
                    break;
            }
        }

        public static void SetDefaultValue(object key, object defaultValue)
        {
            if (!KeyValues.Contains(key))
                Set(key, defaultValue);
        }
        
        public static void SetDefaultValues(IConfigurationSection section)
        {
            foreach (var (key, value) in section.AsEnumerable())
            {
                if (key.Contains(':'))
                    SetDefaultValue(key.Split(':')[1], value);
            }
        }

        public static bool IsDevelopment() =>
            Get<string>(EnvVarKeys.AspNetCoreEnvironment) == "Development";

        public static bool IsProduction() =>
            !IsDevelopment();
    }
}
