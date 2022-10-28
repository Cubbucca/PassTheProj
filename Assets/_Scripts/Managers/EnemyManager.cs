using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    
    [SerializeField] private float _timeBetweenSpawns = 0.2f;
    [SerializeField] private AudioClip[] _launchClips;
    
    private BeanSpawner[] _spawners;
    private int _spawnedIndex;
    private WaitForSeconds _wait;
    private ShoppingCart _cart;
    //Would love to get this to a menu...HINT HINT
    [Range(5, 250)]public int maxNumberOfEnemies;

    public int currentNumberOfEnemies;

    void Start() {
        // This is bad, but I didn't want to go through them 1 by 1
        _spawners = FindObjectsOfType<BeanSpawner>();
        _cart = FindObjectOfType<ShoppingCart>();
        
        _wait = new WaitForSeconds(_timeBetweenSpawns);
        StartCoroutine(SpawnBeans());
    }

    IEnumerator SpawnBeans() {
        while (true) {
            yield return _wait;
            if(_cart.InSafeZone) continue;
            if (currentNumberOfEnemies >= maxNumberOfEnemies) continue;
            _spawners[_spawnedIndex].SpawnEnemy(this);
            currentNumberOfEnemies++;
            // This would be better on the spawner itself, but they are not prefabs so I'd have to do them all individually.
            if(Vector3.Distance(_spawners[_spawnedIndex].transform.position,_cart.transform.position) < 15) 
                AudioSource.PlayClipAtPoint(_launchClips[Random.Range(0,_launchClips.Length)],_spawners[_spawnedIndex].transform.position);
            
            if (++_spawnedIndex >= _spawners.Length) _spawnedIndex = 0;
        }
    }
}
