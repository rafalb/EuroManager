using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.WorldSimulator.DataAccess;

namespace EuroManager.WorldSimulator.Services
{
    public class ApplicationService : IDisposable
    {
        private WorldContext context = null;

        protected WorldContext Context
        {
            get
            {
                if (context == null)
                {
                    context = new WorldContext();
                }

                return context;
            }
        }

        public void Dispose()
        {
            if (context != null)
            {
                context.Dispose();
                context = null;
            }
        }
    }
}
