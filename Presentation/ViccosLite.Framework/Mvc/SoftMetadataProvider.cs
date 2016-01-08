using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ViccosLite.Core;

namespace ViccosLite.Framework.Mvc
{
    public class SoftMetadataProvider: DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(
            IEnumerable<Attribute> attributes,
            Type containerType,
            Func<object> modelAccessor,
            Type modelType,
            string propertyName)
        {
            var metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
            var additionalValues = attributes.OfType<IModelAttribute>().ToList();
            foreach (var additionalValue in additionalValues)
            {
                if (metadata.AdditionalValues.ContainsKey(additionalValue.Name))
                    throw new KsException("Ya existe un atributo con el nombre de \"" + additionalValue.Name +
                                            "\" en este modelo.");
                metadata.AdditionalValues.Add(additionalValue.Name, additionalValue);
            }
            return metadata;
        }
    
    }
}