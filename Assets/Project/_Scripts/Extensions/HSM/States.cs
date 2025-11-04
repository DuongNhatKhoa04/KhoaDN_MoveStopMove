/*using MoveStopMove.Core;
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
        public readonly Idle Idle;
        public readonly Move Move;
        public readonly Attack Attack;
        public readonly Ultimate Ultimate;
        public readonly Dead Dead;
        public readonly Dance Dance;
        public readonly Win Win;

        public Grounded(StateMachine machine, State parent, CharacterContext context) : base(machine, parent)
        {
            this.m_characterContext = context;
            Idle = new Idle(machine, this, context);
            Move = new Move(machine, this, context);
        }

        protected override State GetInitialState() => Idle;
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

    public class Move : State
    {
        private readonly CharacterContext m_characterContext;

        public Move(StateMachine machine, State parent, CharacterContext context) : base(machine, parent)
        {
            this.m_characterContext = context;
        }
    }

    public class Attack : State
    {
        private readonly CharacterContext m_characterContext;

        public Attack(StateMachine machine, State parent, CharacterContext context) : base(machine, parent)
        {
            this.m_characterContext = context;
        }
    }

    public class Ultimate : State
    {
        private readonly CharacterContext m_characterContext;

        public Ultimate(StateMachine machine, State parent, CharacterContext context) : base(machine, parent)
        {
            this.m_characterContext = context;
        }
    }

    public class Dead : State
    {
        private readonly CharacterContext m_characterContext;

        public Dead(StateMachine machine, State parent, CharacterContext context) : base(machine, parent)
        {
            this.m_characterContext = context;
        }
    }

    public class Dance : State
    {
        private readonly CharacterContext m_characterContext;

        public Dance(StateMachine machine, State parent, CharacterContext context) : base(machine, parent)
        {
            this.m_characterContext = context;
        }
    }

    public class Win : State
    {
        private readonly CharacterContext m_characterContext;

        public Win(StateMachine machine, State parent, CharacterContext context) : base(machine, parent)
        {
            this.m_characterContext = context;
        }
    }

    #endregion
}*/