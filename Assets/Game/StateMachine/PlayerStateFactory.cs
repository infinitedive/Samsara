using System.Collections.Generic;
using Game.Controllers;

namespace Game.StateMachine {
    public class PlayerStateFactory {
    
        enum State {
            neutral,
            air,
            grounded,
            grapple,
            grappleHang,
            melee,
            lunge,
            noop,
            dash,
            burst,
            clutch
        }
    
        SkateCharacterController _context;
        Dictionary<State, PlayerState> _states = new Dictionary<State, PlayerState>();
    
        public PlayerStateFactory(SkateCharacterController currentContext) {
            _context = currentContext;
    
            // root states
    
            _states[State.air] = new PlayerStateAir(_context, this);
            _states[State.grounded] = new PlayerStateGrounded(_context, this);
            _states[State.grapple] = new PlayerStateGrapple(_context, this);
            _states[State.grapple] = new PlayerStateGrappleHang(_context, this);
            _states[State.burst] = new PlayerStateBurst(_context, this);
            _states[State.clutch] = new PlayerStateClutch(_context, this);
    
            // sub states
    
            _states[State.neutral] = new PlayerStateNeutral(_context, this);
            
            // _states[State.melee] = new PlayerStateMelee(_context, this);
            // _states[State.lunge] = new PlayerStateLunge(_context, this);
            // _states[State.noop] = new PlayerStateNoop(_context, this);
            // _states[State.dash] = new PlayerStateDash(_context, this);
    
        }
    
        public PlayerState Neutral() {
            return _states[State.neutral];
        }
    
        public PlayerState Air() {
            return _states[State.air];
        }
    
        public PlayerState Grounded() {
            return _states[State.grounded];
        }
    
        public PlayerState Grapple() {
            return _states[State.grapple];
        }

        public PlayerState GrappleHang() {
            return _states[State.grappleHang];
        }

        public PlayerState Clutch() {
            return _states[State.clutch];
        }

        public PlayerState Burst() {
            return _states[State.burst];
        }
    
        public PlayerState Melee() {
            return _states[State.melee];
        }
    
        public PlayerState Lunge() {
            return _states[State.lunge];
        }
        
        public PlayerState Noop() {
            return _states[State.noop];
        }
    
        public PlayerState Dash() {
            return _states[State.dash];
        }
    }
}