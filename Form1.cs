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
            NTextBox.Text = String.Format("{0:f3}", gtuModel.N);
            NuTextBox.Text = String.Format("{0:f3}", gtuModel.Nu);
            GTextBox.Text = String.Format("{0:f3}", gtuModel.G);
            ToutTextBox.Text = String.Format("{0:f3}", gtuModel.T);
            BTextBox.Text = String.Format("{0:f3}", gtuModel.B);

            //2 этап
            DvdTextBox.Text = String.Format("{0:f3}", recoveryBoiler.Dvd);
            TpevdTextBox.Text = String.Format("{0:f3}", recoveryBoiler.Tpevd);
            PbvdTextBox.Text = String.Format("{0:f3}", recoveryBoiler.Pbvd);
            DndTextBox.Text = String.Format("{0:f3}", recoveryBoiler.Dnd);
            TpendTextBox.Text = String.Format("{0:f3}", recoveryBoiler.Tpend);
            PbndTextBox.Text = String.Format("{0:f3}", recoveryBoiler.Pbnd);

            //3 этап
            PkTextBox.Text = String.Format("{0:f3}", capacitor.Pk);
            DtkTextBox.Text = String.Format("{0:f3}", capacitor.Dtk);

            //4 Этап
            NptTextBox.Text = String.Format("{0:f3}", ptModel.GrossPower);

            //5 Этап
            BoosterCompressorTextBox.Text = String.Format("{0:f3}", boosterCompressor.BoosterCompressorValue);

            //6 Этап
            NvdTextBox.Text = String.Format("{0:f3}", feedPump.N);

            // 7 Этап
            NpgubTextBox.Text = String.Format("{0:f3}", finalPower.Npgub);
            NpgunTextBox.Text = String.Format("{0:f3}", finalPower.Npgun);
            NupgubTextBox.Text = String.Format("{0:f3}", finalPower.Nupgub);
            NupgunTextBox.Text = String.Format("{0:f3}", finalPower.Nupgun);
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
