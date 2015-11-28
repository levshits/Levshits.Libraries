using System;
using System.Threading;
using Levshits.Data.Common;
using NHibernate;

namespace Levshits.Data.Storage
{
    public class ThreadSessionStorage:ISessionStorage
    {
        private const string SessionKey = "9C04AB24-18EE-488B-917D-66D2CA19E36D";
        public LocalDataStoreSlot DataStoreSlot { get; set; } = Thread.AllocateNamedDataSlot(SessionKey);

        /// <summary>
        /// 	Gets/Sets current NHibernate session.
        /// </summary>
        public ISession CurrentSession
        {
            get
            {
                return (ISession)Thread.GetData(DataStoreSlot);
            }
            set
            {
                Thread.SetData(DataStoreSlot, value);
            }
        }
    }
}