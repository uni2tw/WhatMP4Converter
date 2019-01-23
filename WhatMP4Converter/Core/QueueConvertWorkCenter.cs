using System;
using System.Linq;
using System.Collections.Generic;

namespace WhatMP4Converter.Core
{
    public class QueueConvertWorkCenter
    {
        public List<QueueConvertWork> WorkItems = new List<QueueConvertWork>();
        public QueueConvertWork StartWork(string srcFilePath, string destFlePath, AppConf conf)
        {
            var work = new QueueConvertWork(srcFilePath, destFlePath, conf);
            WorkItems.Add(work);
            return work;
        }
        public void Shutdown()
        {
            foreach(QueueConvertWork workItem in WorkItems)
            {
                if (workItem.IsClosed == false)
                {
                    workItem.Stop();
                }
            }
        }

        public bool Exist(string srcFilePath)
        {
            return WorkItems.Exists(t => t.SrcFilePath == srcFilePath && t.IsClosed == false);            
        }

        public bool AnyRun()
        {
            return WorkItems.Any(t=>t.IsClosed == false);
        }
    }
}
