using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FOCA.ModifiedComponents;
using System.Threading;

namespace FOCA.TaskManager
{
    public class Tasker : IDisposable
    {

        private Thread tProcess;

        private ThreadSafeList<TaskFOCA> lstTasks = new ThreadSafeList<TaskFOCA>();
        private ThreadSafeList<TaskFOCA> lstRunningTasks = new ThreadSafeList<TaskFOCA>();
        private ThreadSafeList<TaskFOCA> lstFinishedTasks = new ThreadSafeList<TaskFOCA>();

        protected event EventHandler OnTaskAdded;
        protected event EventHandler OnTaskStarting;
        protected event EventHandler OnTaskFinished;

        private ulong idCount = 0;

        public Tasker()
        {
            tProcess = new Thread(new ThreadStart(Process));
            tProcess.IsBackground = true;
            tProcess.Start();
        }

        ~Tasker()
        {
            Dispose(false);
        }
        public void AsociaEventosTareas()
        {
            if (Program.FormMainInstance == null)
                return;
            if (Program.data.tasker == null)
                return;

            if (Program.data.tasker.OnTaskAdded == null)
                Program.data.tasker.OnTaskAdded += new EventHandler(Program.FormMainInstance.tasker_OnTaskAdded);
            if (Program.data.tasker.OnTaskFinished == null)
                Program.data.tasker.OnTaskFinished += new EventHandler(Program.FormMainInstance.tasker_OnTaskFinished);
            if (Program.data.tasker.OnTaskStarting == null)
                Program.data.tasker.OnTaskStarting += new EventHandler(Program.FormMainInstance.tasker_OnTaskStarting);
        }

        private void Process()
        {
            TaskFOCA task;

            while (true)
            {
                System.Threading.Thread.Sleep(50);

                for (int iTaskRunning = 0 ; iTaskRunning < lstRunningTasks.Count ; iTaskRunning++)
                {
                    TaskFOCA taskRunning = lstRunningTasks[iTaskRunning];

                    if (!taskRunning.IsAlive())
                    {
                        lstRunningTasks.Remove(taskRunning);

                        if (OnTaskFinished != null)
                            OnTaskFinished(taskRunning, null);
                    }
                }

                if (lstRunningTasks.Count >= Program.cfgCurrent.NumberOfTasks)
                    continue;

                for (int iTask = lstRunningTasks.Count; iTask < Program.cfgCurrent.NumberOfTasks; iTask++)
                {
                    if (lstTasks.Count == 0)
                        continue;

                    task = lstTasks.First();
                    lstTasks.Remove(task);
                    lstRunningTasks.Add(task);
                    task.Start();

                    if (OnTaskStarting != null)
                        OnTaskStarting(task, null);
                }

            }
        }

        public void RemoveAllTasks()
        {
            lstTasks.Clear();
            lstRunningTasks.Clear();
        }

        public void AddTask(TaskFOCA task)
        {
            lstTasks.Add(task);

            task.id = idCount;

            try
            {
                idCount++;
            }
            catch
            {
                idCount = 0;
            }

            if (OnTaskAdded != null)
                OnTaskAdded(task, null);
        }

        public int countFinishedTasks()
        {
            return lstFinishedTasks.Count;
        }

        public int countPendingTasks()
        {
            return lstTasks.Count;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    lstTasks.Dispose();
                    lstRunningTasks.Dispose();
                    lstFinishedTasks.Dispose();
                }

                lstTasks.Dispose();
                lstRunningTasks.Dispose();
                lstFinishedTasks.Dispose();

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }

    public class TaskFOCA
    {

        private Thread t;
        private object paramStart;
        public bool Finish = false;
        public string description;
        public ulong id;

        public TaskFOCA()
        {
        }

        public TaskFOCA(Thread t, object paramStart, string description)
        {
            this.t = t;
            this.paramStart = paramStart;
            this.description = description;
        }

        public void Start()
        {
            try
            {
                t.IsBackground = true;
                if (t.IsAlive || paramStart != null) return;
                if (t.ThreadState == ThreadState.Stopped || paramStart == null)
                    t.Start();
                else
                    t.Start(paramStart);
            }
            catch (Exception)
            {
            }
        }

        public bool IsAlive()
        {
            return t.IsAlive;
        }

        public override string ToString()
        {
            return description;
        }
    }
}
