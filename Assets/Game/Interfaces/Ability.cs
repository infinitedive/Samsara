using UnityEngine;


namespace Game {
    [CreateAssetMenu(fileName = "Ability", menuName = "Samsara/Ability", order = 0)]
    public abstract class Ability : ScriptableObject {
    
        public abstract void Activate(Transform origin);
        
    }
    
    public class ShieldAbility : Ability {
        public GameObject shieldPrefab;
        public float duration = 10f;
    
        public override void Activate(Transform origin) {
            var shield = Instantiate(shieldPrefab, origin.position, Quaternion.identity, origin);
            Destroy(shield, duration);
        }
    }
}