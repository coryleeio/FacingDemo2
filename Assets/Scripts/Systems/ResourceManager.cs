using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class ResourceManager
    {
        private Dictionary<UniqueIdentifier, IResource> _prototypesByUniqueIdentifier = new Dictionary<UniqueIdentifier, IResource>();
        private Dictionary<Type, List<IResource>> _prototypesByType = new Dictionary<Type, List<IResource>>();
        private bool hasInit = false;

        public ResourceManager() {}

        public void LoadAllPrototypes()
        {
            if(!hasInit)
            {
                _prototypesByUniqueIdentifier.Clear();
                CacheResources(Tilesets.LoadAll());
                CacheResources(ItemAppearances.LoadAll());
                CacheResources(ProjectileAppearances.LoadAll());
            }
            hasInit = true;
        }

        private void CacheResources<TResource>(List<TResource> prototypes) where TResource : IResource
        {
            foreach(var prototype in prototypes)
            {
                if (_prototypesByUniqueIdentifier.ContainsKey(prototype.UniqueIdentifier))
                {
                    throw new DuplicatePrototypeIdException(string.Format("Duplicate prototype: {0}", prototype.UniqueIdentifier));
                }
                Debug.Log("Found " + prototype.GetType().Name.ToString() + ": " + prototype.UniqueIdentifier);
                _prototypesByUniqueIdentifier[prototype.UniqueIdentifier] = prototype;
                if (!_prototypesByType.ContainsKey(prototype.GetType()))
                {
                    _prototypesByType[prototype.GetType()] = new List<IResource>();
                }
                _prototypesByType[prototype.GetType()].Add(prototype);
            }
        }

        public TPrototype GetPrototype<TPrototype>(UniqueIdentifier uniqueIdentifier) where TPrototype : IResource
        {
            try
            {

                if (!_prototypesByUniqueIdentifier.ContainsKey(uniqueIdentifier))
                {
                    throw new CouldNotResolvePrototypeException(string.Format("Failed to lookup prototype: {0}", uniqueIdentifier));
                }

                return (TPrototype)_prototypesByUniqueIdentifier[uniqueIdentifier];
                
            }
            catch (InvalidCastException ex)
            {
                throw new CouldNotResolvePrototypeException(string.Format("Found prototype for {0}, but it is not a {1}", uniqueIdentifier, typeof(TPrototype).Name));
            }
        }

        public List<TPrototype> GetPrototypes<TPrototype>() where TPrototype : IResource
        {
            if(!_prototypesByType.ContainsKey(typeof(TPrototype)))
            {
                return new List<TPrototype>(0);
            }
            var returnList = new List<TPrototype>();
            foreach(var ires in _prototypesByType[typeof(TPrototype)])
            {
                returnList.Add((TPrototype)ires);
            }
            return returnList;
        }
    }
}
