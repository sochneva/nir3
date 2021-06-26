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
        GTUModel gtuModel;
        RecoveryBoiler recoveryBoiler;
        BoosterCompressor boosterCompressor;
        PTModel ptModel;
        public model()
        {
            InitializeComponent();
            // 1 этап гту модель
            gtuModel = new GTUModel();
            // 2 этап котел-утилизатор
            recoveryBoiler = new RecoveryBoiler();
            //4 'nfg gn ПТ
            //ptModel = new PTModel();

            // 5 этап дожимной компрессор
            boosterCompressor = new BoosterCompressor();
        }

        private void trackBarChange(object sender, EventArgs e)
        {
            TrackBar trackBar = (TrackBar)sender;
            gtuModel.updateParam(trackBar.Name, trackBar.Value);
            recoveryBoiler.updateParam(trackBar.Name, trackBar.Value, TnvTrackBar.Value, NgtuTrackBar.Value, gtuModel.G, gtuModel.T);
            boosterCompressor.updateParam(gtuModel.N);

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

            BoosterCompressorTextBox.Text = boosterCompressor.BoosterCompressorValue.ToString();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
