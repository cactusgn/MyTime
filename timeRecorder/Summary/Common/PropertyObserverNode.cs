using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Common
{
  
        internal class PropertyObserverNode
    {
        private readonly Action _action;

        private INotifyPropertyChanged _inpcObject;

        public PropertyInfo PropertyInfo { get; }

        public PropertyObserverNode Next { get; set; }

        public PropertyObserverNode(PropertyInfo propertyInfo, Action action)
        {
            PropertyObserverNode propertyObserverNode = this;
            PropertyInfo = propertyInfo ?? throw new ArgumentNullException("propertyInfo");
            _action = delegate
            {
                action?.Invoke();
                if (propertyObserverNode.Next != null)
                {
                    propertyObserverNode.Next.UnsubscribeListener();
                    propertyObserverNode.GenerateNextNode();
                }
            };
        }

        public void SubscribeListenerFor(INotifyPropertyChanged inpcObject)
        {
            _inpcObject = inpcObject;
            _inpcObject.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
            if (Next != null)
            {
                GenerateNextNode();
            }
        }

        private void GenerateNextNode()
        {
            object value = PropertyInfo.GetValue(_inpcObject);
            if (value != null)
            {
                INotifyPropertyChanged notifyPropertyChanged = value as INotifyPropertyChanged;
                if (notifyPropertyChanged == null)
                {
                    throw new InvalidOperationException("Trying to subscribe PropertyChanged listener in object that owns '" + Next.PropertyInfo.Name + "' property, but the object does not implements INotifyPropertyChanged.");
                }

                Next.SubscribeListenerFor(notifyPropertyChanged);
            }
        }

        private void UnsubscribeListener()
        {
            if (_inpcObject != null)
            {
                _inpcObject.PropertyChanged -= new PropertyChangedEventHandler(OnPropertyChanged);
            }

            Next?.UnsubscribeListener();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e?.PropertyName == PropertyInfo.Name || string.IsNullOrEmpty(e?.PropertyName))
            {
                _action?.Invoke();
            }
        }
    }
    }
