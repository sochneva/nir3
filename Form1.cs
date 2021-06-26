using dip3.Stages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dip3
{
    public partial class model : Form
    {
        private GTUModel gtuModel;                               // 1 этап гту модель             
        private RecoveryBoiler recoveryBoiler;                   // 2 этап котел-утилизатор
        private Capacitor capacitor;                             // 3 этап конденсатор
        private PTModel ptModel;                                 // 4 Этап ПТ  
        private BoosterCompressor boosterCompressor;             // 5 этап дожимной компрессор
        private FeedPump feedPump;                               // 6 этап ПН
        private FinalPower finalPower;                           // 7 этап подсчет мощностей
        public model()
        {
            InitializeComponent();
            InitializeStages();
        }

        private void trackBarChange(object sender, EventArgs e)
        {
            TrackBar trackBar = (TrackBar)sender;
            gtuModel.updateParam(trackBar.Name, trackBar.Value);
            recoveryBoiler.updateParam(trackBar.Name, trackBar.Value, TnvTrackBar.Value, NgtuTrackBar.Value, gtuModel.G, gtuModel.T);
            capacitor.updateParam(trackBar.Name, trackBar.Value, recoveryBoiler.Dvd, recoveryBoiler.Dnd);
            ptModel.updateParam(recoveryBoiler.Dvd, recoveryBoiler.Tpevd, recoveryBoiler.Pbvd, recoveryBoiler.Dnd, recoveryBoiler.Tpend, capacitor.Pk);
            boosterCompressor.updateParam(gtuModel.N);
            feedPump.updateParam(trackBar.Name, trackBar.Value, recoveryBoiler.Pbvd, recoveryBoiler.Pbnd);
            finalPower.updateParam(gtuModel.N, ptModel.GrossPower, feedPump.N, boosterCompressor.BoosterCompressorValue, gtuModel.B, GTUModel.DefQ);

            //1 этап
            NTextBox.Text = gtuModel.N.ToString();
            NuTextBox.Text = gtuModel.Nu.ToString();
            GTextBox.Text = gtuModel.G.ToString();
            ToutTextBox.Text = gtuModel.T.ToString();
            BTextBox.Text = gtuModel.B.ToString();

            //2 этап
            DvdTextBox.Text = recoveryBoiler.Dvd.ToString();
            TpevdTextBox.Text = recoveryBoiler.Tpevd.ToString();
            PbvdTextBox.Text = recoveryBoiler.Pbvd.ToString();
            DndTextBox.Text = recoveryBoiler.Dnd.ToString();
            TpendTextBox.Text = recoveryBoiler.Tpend.ToString();
            PbndTextBox.Text = recoveryBoiler.Pbnd.ToString();

            //3 этап
            PkTextBox.Text = capacitor.Pk.ToString();
            DtkTextBox.Text = capacitor.Dtk.ToString();

            //4 Этап
            NptTextBox.Text = ptModel.GrossPower.ToString();

            //5 Этап
            BoosterCompressorTextBox.Text = boosterCompressor.BoosterCompressorValue.ToString();

            //6 Этап
            NvdTextBox.Text = feedPump.N.ToString();

            // 7 Этап
            NpgubTextBox.Text = finalPower.Npgub.ToString();
            NpgunTextBox.Text = finalPower.Npgun.ToString();
            NupgubTextBox.Text = finalPower.Nupgub.ToString();
            NupgunTextBox.Text = finalPower.Nupgun.ToString();
        }

        private void InitializeStages()
        {
            gtuModel = new GTUModel();
            recoveryBoiler = new RecoveryBoiler();
            capacitor = new Capacitor();
            ptModel = new PTModel();
            boosterCompressor = new BoosterCompressor();
            feedPump = new FeedPump();
            finalPower= new FinalPower();
        }
    }
}
