using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomerController : MonoBehaviour
{
    [SerializeField] ProductType requiredProductType;
    ProductsInteraction playerProductsInteraction;
    [SerializeField] TextMeshProUGUI selectedProductText;

    [SerializeField] Slider timeAvailableSlider;
    float timeToComplete = 10;

    void Awake()
    {
        playerProductsInteraction = FindObjectOfType<ProductsInteraction>();
        SetTimeAvailable();
    }

    void Start() {
        SelectRequiredProduct();
    }

    private void Update()
    { 
        if (timeToComplete > 0)
        {
            timeAvailableSlider.value = timeToComplete;
            timeToComplete -= Time.deltaTime;
        }

        if (timeToComplete <= 0)
        {
            //failed!
            //for now, choose new time left available + new product
            SetTimeAvailable();
            SelectRequiredProduct();
        }
    }

    void SelectRequiredProduct()
    {
        // Sam: not sure why getting values then casting it to ienumerable to get the count of the enum types
        requiredProductType = (ProductType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(ProductType)).Cast<ProductType>().Count());
        if (requiredProductType == ProductType.None)
            SelectRequiredProduct();

        string productName = "";

        switch (requiredProductType)
        {
            case ProductType.LoafOfBread:
                productName = "Loaf of Bread";
                break;
            case ProductType.WaterBottle:
                productName = "Water Bottle";
                break;
            case ProductType.Banana:
                productName = "Banana";
                break;
            case ProductType.ToilerRoll:
                productName = "Toilet Roll";
                break;
            default:
                productName = "placeholder";
                break;
        }
        selectedProductText.text = productName;
        
        ProductManager.Instance.SpawnProduct(requiredProductType);
    }

    void SetTimeAvailable() 
    {
        timeToComplete = UnityEngine.Random.Range(25, 35);
        timeAvailableSlider.maxValue = timeToComplete;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ProductsInteraction interaction)) {
            foreach (var product in interaction.GetHeldProduct()) {
                if (product.Type == requiredProductType)
                {
                    ScoreManager.Instance.AddScore(100);
           
                    //choose new time left available + new product
                    SetTimeAvailable();
                    SelectRequiredProduct();

                    interaction.ClearProductType(product.Type);
                }
            }
        }
    }
}
