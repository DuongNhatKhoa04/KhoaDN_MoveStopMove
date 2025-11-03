using System;
using System.Threading;
using System.Threading.Tasks;

namespace MoveStopMove.Extensions.HSM
{
    public enum EActivityMode
    {
        Inactive,
        Activating,
        Active,
        Deactivating
    }

    public interface IActivity
    {
        public EActivityMode Mode { get; }
        public Task ActivateAsync(CancellationToken cancelToken);
        public Task DeactivateAsync(CancellationToken cancelToken);
    }

    public abstract class Activity : IActivity
    {
        #region -- Properties --

        public EActivityMode Mode { get; private set; } = EActivityMode.Inactive;

        #endregion

        #region -- Methods --

        public virtual async Task ActivateAsync(CancellationToken cancelToken)
        {
            if (Mode != EActivityMode.Inactive) return;

            Mode = EActivityMode.Activating;
            await Task.CompletedTask;
            Mode = EActivityMode.Active;
        }

        public virtual async Task DeactivateAsync(CancellationToken cancelToken)
        {
            if (Mode != EActivityMode.Active) return;

            Mode = EActivityMode.Deactivating;
            await Task.CompletedTask;
            Mode = EActivityMode.Inactive;
        }

        #endregion
    }

    public class DelayActivationActivity : Activity
    {
        public float seconds = 0.2f;

        public override async Task ActivateAsync(CancellationToken cancelToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds), cancelToken);
            await base.ActivateAsync(cancelToken);
        }
    }
}