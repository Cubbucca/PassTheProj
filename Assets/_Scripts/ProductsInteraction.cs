using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProductsInteraction : MonoBehaviour {
    [SerializeField] private List<ProductPlacement> _productPlacements = new List<ProductPlacement>();
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _pickupClip, _dropOffClip;

    public void SetHeldProduct(ProductType productType) {
        var freeSpot = _productPlacements.FirstOrDefault(h => h.Type == ProductType.None);
        
        if (freeSpot == null) {
            Debug.Log("Too many items!");
            // TODO "Too many items!" on screen
            return;
        }
        
        var scriptable = ProductManager.Instance.GetProduct(productType);
        
        freeSpot.Type = productType;
        Instantiate(scriptable.CartPrefab, freeSpot.ProductParent);
        _source.PlayOneShot(_pickupClip,1);
    }

    
    public IEnumerable<ProductPlacement> GetHeldProduct() {
        return _productPlacements.Where(p=>p.Type != ProductType.None);
    }

    public void ClearProductType(ProductType type) {
        var droppable = _productPlacements.Where(p => p.Type == type).ToList();
        
        foreach (var product in droppable) {
            product.Type = ProductType.None;
            Destroy(product.ProductParent.GetChild(0).gameObject);
            break;
        }

        if (droppable.Any()) _source.PlayOneShot(_dropOffClip,1);
    }

    [Serializable]
    public class ProductPlacement {
        public Transform ProductParent;
        public ProductType Type;
    }
}