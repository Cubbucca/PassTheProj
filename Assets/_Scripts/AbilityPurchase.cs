using TMPro;
using UnityEngine;

public class AbilityPurchase : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _name, _cost;

    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Material _unlockableMaterial, _tooExpensiveMaterial, _unlockedMaterial;

    private ScriptableAbility _scriptable;
    private bool _unlocked;

    [SerializeField] private TextMeshProUGUI _abilityText;
    [SerializeField] AudioSource _audSrc;

    public void Init(ScriptableAbility scriptable) {
        _scriptable = scriptable;

        _name.text = _scriptable.Name;
        _cost.text = $"${_scriptable.Cost}";
        
        _renderer.material =  _tooExpensiveMaterial;
    }

    private void OnEnable() {
        ScoreManager.OnNewScore += ScoreManagerOnOnNewScore;
    }
    
    private void OnDisable() {
        ScoreManager.OnNewScore -= ScoreManagerOnOnNewScore;
    }

    private void ScoreManagerOnOnNewScore(int obj) {
        if(_unlocked) return;
        _renderer.material = obj >= _scriptable.Cost ? _unlockableMaterial : _tooExpensiveMaterial;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out ShoppingCart cart)) {
            if (ScoreManager.Instance.Score >= _scriptable.Cost) {
                ScoreManager.Instance.AddScore(-_scriptable.Cost);
                
                cart.UnlockAbility(_scriptable.Type);
                _unlocked = true;
                _renderer.material = _unlockedMaterial;
                _cost.text = "UNLOCKED";
                _audSrc.pitch = UnityEngine.Random.Range(1f, 2f);
                _audSrc.Play();
            }
        }
    }

}
