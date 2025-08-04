using UnityEngine;


namespace Game {
    [CreateAssetMenu(fileName = "Behavior", menuName = "Samsara/Behavior", order = 0)]
    public abstract class Behavior : ScriptableObject {
    
        public abstract void Activate(Transform origin);
        
    }
    
    public class GrappleBehavior : Behavior {
        public float duration = 10f;
    
        public override void Activate(Transform origin) {

        }
    }
    
}