using UnityEngine;


// A Gear is a global state change in the camera and character position and movement parameters
// In other words, from first person to third person, or at a top down view, or from platformer to tank or car controls.
// Basically, a gear changes how the controller interprets the input into character and camera movement.

namespace Game {
    [CreateAssetMenu(fileName = "Gear", menuName = "Samsara/Gear", order = 0)]
    public abstract class Gear : ScriptableObject {
    
        public abstract void Shift(Transform origin);
        
    }
    
    
}