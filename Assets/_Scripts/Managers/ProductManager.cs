using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProductManager : MonoBehaviour {
    public static ProductManager Instance;

    [SerializeField] private List<ScriptableProduct> _scriptableProducts;
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private int _maxOfProductType = 2;
    
    private readonly List<ProductController> _spawnedProducts = new List<ProductController>();
    public ScriptableProduct GetProduct(ProductType type) => _scriptableProducts.First(p => p.Type == type);

    void Awake() {
        Instance = this;

        foreach (var product in _scriptableProducts) SpawnProduct(product.Type);
    }

    public void SpawnProduct(ProductType type) {
        if (_spawnedProducts.Count(p => p.Type == type) >= _maxOfProductType) return;

        var spot = _spawnPoints.Where(s => s.childCount == 0).OrderBy(n => Random.value).FirstOrDefault();
        if (spot == null) {
            Debug.Log("No more available product spawns");
            return;
        }

        var scriptable = GetProduct(type);
        var product = Instantiate(scriptable.Prefab, spot);
        _spawnedProducts.Add(product);
    }

    public void ClearProduct(ProductController product) {
        _spawnedProducts.Remove(product);
    }
}