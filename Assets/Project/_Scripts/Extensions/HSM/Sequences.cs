using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoveStopMove.Extensions.HSM
{
    public interface ISequence
    {
        public bool IsDone { get; }
        public void Start();
        public bool Update();
    }

    /// <summary>
    /// Useful for no sequence step
    /// </summary>
    public class NoopPhase : ISequence
    {
        #region -- Properties --

        public bool IsDone { get; private set; }

        #endregion

        #region -- Methods --

        public void Start() => IsDone = true;
        public bool Update() => IsDone;

        #endregion
    }

    public delegate Task PhaseStep(CancellationToken cancelToken);

    /// <summary>
    /// Async task in PhaseStep
    /// </summary>
    public class SequentialPhase : ISequence
    {
        private readonly List<PhaseStep> m_phaseSteps;
        private readonly CancellationToken m_cancelToken;
        private int m_index = -1;
        private Task m_currentTask;

        public bool IsDone { get; private set; }

        public SequentialPhase(List<PhaseStep> steps, CancellationToken token)
        {
            m_phaseSteps = steps;
            m_cancelToken = token;
        }

        public void Start() => Next();

        public bool Update()
        {
            if (IsDone) return true;
            if (m_currentTask == null || m_currentTask.IsCompleted) Next();
            return IsDone;
        }

        private void Next()
        {
            m_index++;
            if (m_index >= m_phaseSteps.Count)
            {
                IsDone = true;
                return;
            }
            m_currentTask = m_phaseSteps[m_index](m_cancelToken);
        }
    }

    /// <summary>
    /// Parallel all task in PhaseStep
    /// </summary>
    public class ParallelPhase : ISequence
    {
        private readonly List<PhaseStep> m_phaseSteps;
        private readonly CancellationToken m_cancelToken;
        private List<Task> m_tasks;
        private Task m_currentTask;

        public bool IsDone { get; private set; }

        public ParallelPhase(List<PhaseStep> steps, CancellationToken token)
        {
            m_phaseSteps = steps;
            m_cancelToken = token;
        }

        public void Start()
        {
            if (m_phaseSteps == null || m_phaseSteps.Count == 0)
            {
                IsDone = true;
                return;
            }
            m_tasks = new List<Task>(m_phaseSteps.Count);
            for (int i = 0; i < m_phaseSteps.Count; i++)
            {
                m_tasks.Add(m_phaseSteps[i](m_cancelToken));
            }
        }

        public bool Update()
        {
            if (IsDone) return true;
            IsDone = m_tasks == null || m_tasks.TrueForAll(task => task.IsCompleted);
            return IsDone;
        }
    }
}