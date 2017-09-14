namespace Hey.Service
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.HeyServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.HeyServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // HeyServiceProcessInstaller
            // 
            this.HeyServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.HeyServiceProcessInstaller.Password = null;
            this.HeyServiceProcessInstaller.Username = null;
            // 
            // HeyServiceInstaller
            // 
            this.HeyServiceInstaller.DisplayName = "HeyService";
            this.HeyServiceInstaller.ServiceName = "HeyService";
            this.HeyServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.HeyServiceProcessInstaller,
            this.HeyServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller HeyServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller HeyServiceInstaller;
    }
}