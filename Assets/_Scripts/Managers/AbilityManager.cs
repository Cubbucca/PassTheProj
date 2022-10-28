using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private List<ScriptableAbility> _scriptableAbilities;
    [SerializeField] private List<Transform> _spawns;
    [SerializeField] private AbilityPurchase _prefab;

    [SerializeField] private List<AbilityText> _abilityTexts;

    private void Start() {
        for (var i = 0; i < _scriptableAbilities.Count; i++) {
            var ability = _scriptableAbilities[i];
            var spawn = Instantiate(_prefab, _spawns[i]);
            spawn.Init(ability);
        }

        foreach (var text in _abilityTexts)  ShoppingCartOnOnAbilityUsable(text.Type,false);
    }
    
    private void OnEnable() {
        ShoppingCart.OnAbilityUsable += ShoppingCartOnOnAbilityUsable;
    }
    
    private void OnDisable() {
        ShoppingCart.OnAbilityUsable -= ShoppingCartOnOnAbilityUsable;
    }

    private void ShoppingCartOnOnAbilityUsable(AbilityType type, bool usable) {
        var abilityText = _abilityTexts.FirstOrDefault(t => t.Type == type);

        ScriptableAbility currentAbility = null;
        foreach(ScriptableAbility ability in _scriptableAbilities)
        {
            if(ability.Type == type)
            {
                currentAbility = ability;
            }
        }

        abilityText.Text.GetComponent<TextMeshProUGUI>().text = $"Press {currentAbility.Key.ToString()} to use '{currentAbility.Name}'!";
        abilityText.Text.SetActive(usable);
    }

    [Serializable]
    public struct AbilityText {
        public AbilityType Type;
        public GameObject Text;
    }
}
