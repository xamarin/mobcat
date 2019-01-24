using System.Linq;
using System.Text;
using SampleLib;
using Xamarin.Forms;

namespace SampleApp
{
    public partial class MainPage : ContentPage
    {
        private SampleFuncs _sampleFuncs;

        public MainPage()
        {
            InitializeComponent();
            _sampleFuncs = new SampleFuncs();
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            int itemsInSequence = -1;

            if (!int.TryParse(ItemsInSequenceEntry.Text, out itemsInSequence) || itemsInSequence <= 0)
            {
                DisplayAlert("Error", "Unable to parse items in sequence", "OK");
                return;
            }

            var fibonnaci = _sampleFuncs.GetFibonacci(itemsInSequence)?.ToList();

            StringBuilder alertTextBuilder = new StringBuilder();
            var itemCount = fibonnaci.Count;

            for (var i = 0; i < itemCount; i++)
            {
                alertTextBuilder.Append(fibonnaci[i]);

                if (i != itemCount - 1)
                    alertTextBuilder.Append(", ");
            }

            DisplayAlert("Fibonacci", alertTextBuilder.ToString(), "OK");
        }
    }
}
