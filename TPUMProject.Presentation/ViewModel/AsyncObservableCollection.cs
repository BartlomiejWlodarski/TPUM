using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;


namespace TPUMProject.Presentation.ViewModel
{
    public class AsyncObservableCollection<T> : ObservableCollection<T>
    {
        private readonly SynchronizationContext _syncContext;

        public AsyncObservableCollection()
        {
            _syncContext = SynchronizationContext.Current;
        }

        public AsyncObservableCollection(IEnumerable<T> collection) : base(collection) 
        {
             _syncContext = SynchronizationContext.Current;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if(SynchronizationContext.Current == _syncContext)
            {
                RaiseCollectionChanged(e);
            }else
            {
                _syncContext.Send(RaiseCollectionChanged, e);
            }
        }

        private void RaiseCollectionChanged(object param)
        {
            // We are in the creator thread, call the base implementation directly
            base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            if (SynchronizationContext.Current == _syncContext)
            {
                // Execute the PropertyChanged event on the current thread
                RaisePropertyChanged(eventArgs);
            }
            else
            {
                // Raises the PropertyChanged event on the creator thread
                _syncContext.Send(RaisePropertyChanged, eventArgs);
            }
        }

        private void RaisePropertyChanged(object param)
        {
            // We are in the creator thread, call the base implementation directly
            base.OnPropertyChanged((PropertyChangedEventArgs)param);
        }

        public void AddRange(IEnumerable<T> elements)
        {
            foreach (var element in elements)
            {
                Add(element);
            }
        }
    }
}
