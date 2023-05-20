using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ASCOM.RemoteOpenMeteo.Properties;
using ASCOM.Utilities;

namespace ASCOM.RemoteOpenMeteo
{
    /// <summary>
    /// Formulaire des param�tres du p�riph�rique ASCOM ROM
    /// <para>Pas d'enregistrement du composant pour le formulaire</para>
    /// </summary>
    [ComVisible(false)]
    public partial class SetupDialogForm : Form
    {
        #region Champs

        /// <summary>
        /// Instance du TraceLogger en cours
        /// </summary>
        TraceLogger tl;

        #endregion

        #region Constructeur

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="tlDriver"></param>
        public SetupDialogForm(TraceLogger tlDriver)
        {
            InitializeComponent();

            // Instance du TraceLogger en cours
            tl = tlDriver;

            // Initialisation du formulaire
            InitUI();
        }

        #endregion

        #region M�thodes Private

        /// <summary>
        /// Initialisation du formulaire
        /// </summary>
        private void InitUI()
        {
            // Mode Trace activ�
            chkTrace.Checked = tl.Enabled;

            // Liste des ports s�rie disponibles
            comboBoxComPort.Items.Clear();
            comboBoxComPort.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());

            // On s�lectionne le port s�rie en cours s'il est pr�sent dans la liste
            if (!string.IsNullOrEmpty(ObservingConditions.comPort) && comboBoxComPort.Items.Contains(ObservingConditions.comPort))
            {
                comboBoxComPort.SelectedItem = ObservingConditions.comPort;
            }

            // Internationalisation des libell�s
            labelAuthor.Text = Resources.LeZavEtJuanitoDelPepito;
            groupBoxParametres.Text = Resources.ParametresDuPeripheriqueROM;
            labelPortSerie.Text = Resources.PortSerie;
            chkTrace.Text = Resources.ModeTraceActive;
        }

        /// <summary>
        /// Clic sur bouton OK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdOK_Click(object sender, EventArgs e)
        {
            try
            {
                // Positionnement des membres internes
                tl.Enabled = chkTrace.Checked;
                ObservingConditions.comPort = (string)comboBoxComPort.SelectedItem;
            }
            catch (Exception err)
            {
                // Trace
                tl.LogMessage("SetupDialogForm", "cmdOK_Click ERROR : " + err.Message);
            }
        }

        /// <summary>
        /// Clic sur bouton Cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdCancel_Click(object sender, EventArgs e)

        {
            Close();
        }

        #endregion
    }
}