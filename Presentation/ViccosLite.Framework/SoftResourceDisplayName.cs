using System.ComponentModel;
using ViccosLite.Framework.Mvc;

namespace ViccosLite.Framework
{
    public class SoftResourceDisplayName : DisplayNameAttribute, IModelAttribute
    {
        private string _resourceValue = string.Empty;

        public SoftResourceDisplayName(string resourceKey)
            : base(resourceKey)
        {
            ResourceKey = resourceKey;
        }

        //private bool _resourceValueRetrived;
        public string ResourceKey { get; set; }

        public override string DisplayName
        {
            get
            {
                _resourceValue = ResourceKey;
                return _resourceValue;
            }
        }

        public string Name
        {
            get { return "SoftResourceDisplayName"; }
        }
    }
}