using UnityEngine;

public class ProductController : MonoBehaviour
{
    [SerializeField] private ProductType _productType;
    [SerializeField] private Transform _model;
    [SerializeField] private float _hoverSpeed = 2;
    [SerializeField] private float _hoverAmplitude = 2;
    [SerializeField] private float _ogy = 1;

    public ProductType Type => _productType;
    
    private void Update() {
        _model.localPosition = new Vector3(_model.localPosition.x, Mathf.Sin(Time.time*_hoverSpeed)*_hoverAmplitude+_ogy, _model.localPosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player is the only interactable layer, so we can be sure
        var interaction = other.GetComponent<ProductsInteraction>();
        interaction.SetHeldProduct(_productType);
        
        ProductManager.Instance.ClearProduct(this);
        Destroy(gameObject);
    }
}

