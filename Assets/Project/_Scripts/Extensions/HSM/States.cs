using MoveStopMove.Core;
using MoveStopMove.SO;

namespace MoveStopMove.Extensions.HSM
{
    #region -- Player Root --

    public class PlayerRoot : State
    {
        public readonly Grounded Grounded;
        private readonly CharacterContext m_characterContext;

        public PlayerRoot(StateMachine machine, CharacterContext context) : base (machine, null)
        {
            this.m_characterContext = context;
            Grounded = new Grounded(machine, this, context);
        }

        protected override State GetInitialState() => Grounded;
    }

    #endregion

    #region -- Super States --

    public class Grounded : State
    {
        private readonly CharacterContext m_characterContext;

        public Grounded(StateMachine machine, State parent, CharacterContext context) : base(machine, parent)
        {
            this.m_characterContext = context;
        }
    }

    #endregion

    #region -- Sub States --

    public class Idle : State
    {
        private readonly CharacterContext m_characterContext;

        public Idle(StateMachine machine, State parent, CharacterContext context) : base(machine, parent)
        {
            this.m_characterContext = context;
        }
    }

    #endregion
}