﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;

namespace ViccosLite.Core.Caching
{
    public class PerRequestCacheManager : ICacheManager
    {
        private readonly HttpContextBase _context;

        public PerRequestCacheManager(HttpContextBase context)
        {
            _context = context;
        }

        protected virtual IDictionary GetItems()
        {
            return _context != null ? _context.Items : null;
        }

        public virtual T Get<T>(string key)
        {
            var items = GetItems();
            return items == null ? default(T) : (T)items[key];
        }

        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            var items = GetItems();
            if (items == null)
                return;

            if (items.Contains(key))
                items[key] = data;
            else
                items.Add(key, data);
        }

        public virtual bool IsSet(string key)
        {
            var items = GetItems();
            return items != null && items[key] != null;
        }

        public virtual void Remove(string key)
        {
            var items = GetItems();
            if (items != null)
                items.Remove(key);
        }

        public virtual void RemoveByPattern(string pattern)
        {
            var items = GetItems();
            if (items == null)
                return;

            var enumerator = items.GetEnumerator();
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = new List<String>();

            while (enumerator.MoveNext())
            {
                if (regex.IsMatch(enumerator.Key.ToString()))
                    keysToRemove.Add(enumerator.Key.ToString());
            }


            foreach (var key in keysToRemove)
                items.Remove(key);
        }

        public virtual void Clear()
        {
            var items = GetItems();
            if (items == null)
                return;

            var enumerator = items.GetEnumerator();
            var keysToRemove = new List<String>();

            while (enumerator.MoveNext())
                keysToRemove.Add(enumerator.Key.ToString());

            foreach (var key in keysToRemove)
                items.Remove(key);
        }
    }
}