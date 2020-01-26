using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ExcelDna.Integration;
using ExcelDna.Integration.Extensibility;
namespace YHExcelAddin
{
    [ComVisible(true)]
    [Guid("4AD5F2C5-0246-4C22-A69F-F0DCECABEA61")]
    [ProgId("YHExcelAddin.Connection")]
    class ComAddInConnection : ExcelComAddIn
    {
        #region IDTExensibility2
        /// <summary>
        /// Receives notification that the Add-in is being loaded.
        /// </summary>
        public override void OnConnection(object Application, ext_ConnectMode ConnectMode, object AddInInst, ref Array custom)
        {
            base.OnConnection(Application, ConnectMode, AddInInst, ref custom);
        }
        /// <summary>
        /// Receives notification that the Add-in is being unloaded.
        /// </summary>
        public override void OnDisconnection(ext_DisconnectMode RemoveMode, ref Array custom)
        {
            base.OnDisconnection(RemoveMode, ref custom);
        }
        /// <summary>
        /// Receives notification when the collection of Add-ins has changed.
        /// </summary>
        public override void OnAddInsUpdate(ref Array custom)
        {
            base.OnAddInsUpdate(ref custom);
        }
        /// <summary>
        /// Receives notification that the host application has completed loading.
        /// </summary>
        public override void OnStartupComplete(ref Array custom)
        {
            base.OnStartupComplete(ref custom);
        }
        /// <summary>
        /// Receives notification that the host application is being unloaded.
        /// </summary>
        public override void OnBeginShutdown(ref Array custom)
        {
            base.OnBeginShutdown(ref custom);
        }
        #endregion IDTExensibility2
    }
}
