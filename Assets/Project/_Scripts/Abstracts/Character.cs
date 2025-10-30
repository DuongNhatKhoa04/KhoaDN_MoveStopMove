using System;
using MoveStopMove.Interfaces;
using UnityEngine;

namespace MoveStopMove.Abstracts
{
    public abstract class Character : IInitializable, IPausable, IResettable
    {
        #region -- Fields --

        protected Rigidbody Rb;
        protected Animator Anim;

        #endregion

        #region -- Methods --

        public virtual void Initialize()
        {
            throw new NotImplementedException();
        }

        public virtual void Pause()
        {
            throw new NotImplementedException();
        }

        public virtual void Resume()
        {
            throw new NotImplementedException();
        }

        public virtual void Reset()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}