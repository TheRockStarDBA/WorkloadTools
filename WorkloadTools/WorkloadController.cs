﻿using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WorkloadTools
{
    public class WorkloadController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private WorkloadListener listener;
        private List<WorkloadConsumer> consumers = new List<WorkloadConsumer>();
        private bool stopped = false;
        private bool disposed = false;
        private const int MAX_DISPOSE_TIMEOUT_SECONDS = 5;


        public WorkloadController(WorkloadListener listener)
        {
            this.listener = listener;
        }

        public void RegisterConsumer(WorkloadConsumer consumer)
        {
            consumers.Add(consumer);
        }


        public void Run()
        {
            listener.Initialize();
            while (!stopped)
            {
                var evt = listener.Read();
                Parallel.ForEach(consumers, (cons) =>
                {
                    cons.Consume(evt);
                });
            }
            if (!disposed)
            {
                disposed = true;
                listener.Dispose();
                foreach (var cons in consumers)
                {
                    cons.Dispose();
                }
            }
        }

        public Task Start()
        {
            return Task.Factory.StartNew(() => Run());
        }

        public void Stop()
        {
            stopped = true;
            int timeout = 0;
            while(!disposed && timeout < (MAX_DISPOSE_TIMEOUT_SECONDS * 1000))
            {
                Thread.Sleep(100);
                timeout += 100;
            }
            if (!disposed)
            {
                disposed = true;
                listener.Dispose();
                foreach (var cons in consumers)
                {
                    cons.Dispose();
                }
            }
        }

    }
}