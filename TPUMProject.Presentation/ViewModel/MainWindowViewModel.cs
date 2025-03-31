using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TPUMProject.Presentation.Model;

namespace TPUMProject.Presentation.ViewModel
{
    public class MainWindowViewModel : IDisposable, INotifyPropertyChanged
    {
        private ModelAbstractAPI ModelLayer;
        private string _testString = "test1";

        public string TestString
        {
            get { return _testString; }
            set
            {
                _testString = value;
                RaisePropertyChanged(nameof(_testString));
            }
        }

        public MainWindowViewModel() : this(null)
        { }

        internal MainWindowViewModel(ModelAbstractAPI modelLayerAPI)
        {
            ModelLayer = modelLayerAPI == null ? ModelAbstractAPI.CreateModel() : modelLayerAPI;
        }






        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
