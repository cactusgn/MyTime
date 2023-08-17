using Summary.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Summary
{
    public class MyCommand : ICommand
    {
        Action<object> executeAction;
        Action executeActionwithoutParams;
        public MyCommand(Action<object> action)
        {
            executeAction = action;
        }
        
        public event EventHandler? CanExecuteChanged;

        public virtual bool CanExecute(object? parameter)
        {
            return true;
        }

        public virtual void Execute(object? parameter)
        {
            
                executeAction(parameter);
            
            
        }
       
    }
    internal class PropertyObserver
    {
        private readonly Action _action;

        private PropertyObserver(Expression propertyExpression, Action action)
        {
            _action = action;
            SubscribeListeners(propertyExpression);
        }

        private void SubscribeListeners(Expression propertyExpression)
        {
            Stack<PropertyInfo> stack = new Stack<PropertyInfo>();
            while (true)
            {
                MemberExpression memberExpression = propertyExpression as MemberExpression;
                if (memberExpression == null)
                {
                    break;
                }

                propertyExpression = memberExpression.Expression;
                stack.Push(memberExpression.Member as PropertyInfo);
            }

            ConstantExpression constantExpression = propertyExpression as ConstantExpression;
            if (constantExpression == null)
            {
                throw new NotSupportedException("Operation not supported for the given expression type. Only MemberExpression and ConstantExpression are currently supported.");
            }

            PropertyObserverNode propertyObserverNode = new PropertyObserverNode(stack.Pop(), _action);
            PropertyObserverNode propertyObserverNode2 = propertyObserverNode;
            using (Stack<PropertyInfo>.Enumerator enumerator = stack.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    PropertyObserverNode propertyObserverNode4 = (propertyObserverNode2.Next = new PropertyObserverNode(enumerator.Current, _action));
                    propertyObserverNode2 = propertyObserverNode4;
                }
            }

            INotifyPropertyChanged notifyPropertyChanged = constantExpression.Value as INotifyPropertyChanged;
            if (notifyPropertyChanged == null)
            {
                throw new InvalidOperationException("Trying to subscribe PropertyChanged listener in object that owns '" + propertyObserverNode.PropertyInfo.Name + "' property, but the object does not implements INotifyPropertyChanged.");
            }

            propertyObserverNode.SubscribeListenerFor(notifyPropertyChanged);
        }

        //
        // 摘要:
        //     Observes a property that implements INotifyPropertyChanged, and automatically
        //     calls a custom action on property changed notifications. The given expression
        //     must be in this form: "() => Prop.NestedProp.PropToObserve".
        //
        // 参数:
        //   propertyExpression:
        //     Expression representing property to be observed. Ex.: "() => Prop.NestedProp.PropToObserve".
        //
        //   action:
        //     Action to be invoked when PropertyChanged event occurs.
        internal static PropertyObserver Observes<T>(Expression<Func<T>> propertyExpression, Action action)
        {
            return new PropertyObserver(propertyExpression.Body, action);
        }
    }
    public abstract class DelegateCommandBase : ICommand
    {
        private bool _isActive;

        private SynchronizationContext _synchronizationContext;

        private readonly HashSet<string> _observedPropertiesExpressions = new HashSet<string>();

        //
        // 摘要:
        //     Gets or sets a value indicating whether the object is active.
        //
        // 值:
        //     true if the object is active; otherwise false.
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnIsActiveChanged();
                }
            }
        }

        //
        // 摘要:
        //     Occurs when changes occur that affect whether or not the command should execute.
        public virtual event EventHandler CanExecuteChanged;

        //
        // 摘要:
        //     Fired if the Prism.Commands.DelegateCommandBase.IsActive property changes.
        public virtual event EventHandler IsActiveChanged;

        //
        // 摘要:
        //     Creates a new instance of a Prism.Commands.DelegateCommandBase, specifying both
        //     the execute action and the can execute function.
        protected DelegateCommandBase()
        {
            _synchronizationContext = SynchronizationContext.Current;
        }

        //
        // 摘要:
        //     Raises System.Windows.Input.ICommand.CanExecuteChanged so every command invoker
        //     can requery System.Windows.Input.ICommand.CanExecute(System.Object).
        protected virtual void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChanged;
            if (handler == null)
            {
                return;
            }

            if (_synchronizationContext != null && _synchronizationContext != SynchronizationContext.Current)
            {
                _synchronizationContext.Post(delegate
                {
                    handler(this, EventArgs.Empty);
                }, null);
            }
            else
            {
                handler(this, EventArgs.Empty);
            }
        }

        //
        // 摘要:
        //     Raises Prism.Commands.DelegateCommandBase.CanExecuteChanged so every command
        //     invoker can requery to check if the command can execute.
        //
        // 言论：
        //     Note that this will trigger the execution of Prism.Commands.DelegateCommandBase.CanExecuteChanged
        //     once for each invoker.
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        void ICommand.Execute(object parameter)
        {
            Execute(parameter);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(parameter);
        }

        //
        // 摘要:
        //     Handle the internal invocation of System.Windows.Input.ICommand.Execute(System.Object)
        //
        // 参数:
        //   parameter:
        //     Command Parameter
        protected abstract void Execute(object parameter);

        //
        // 摘要:
        //     Handle the internal invocation of System.Windows.Input.ICommand.CanExecute(System.Object)
        //
        // 参数:
        //   parameter:
        //
        // 返回结果:
        //     true if the Command Can Execute, otherwise false
        protected abstract bool CanExecute(object parameter);

        //
        // 摘要:
        //     Observes a property that implements INotifyPropertyChanged, and automatically
        //     calls DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
        //
        // 参数:
        //   propertyExpression:
        //     The property expression. Example: ObservesProperty(() => PropertyName).
        //
        // 类型参数:
        //   T:
        //     The object type containing the property specified in the expression.
        protected internal void ObservesPropertyInternal<T>(Expression<Func<T>> propertyExpression)
        {
            if (_observedPropertiesExpressions.Contains(propertyExpression.ToString()))
            {
                throw new ArgumentException(propertyExpression.ToString() + " is already being observed.", "propertyExpression");
            }

            _observedPropertiesExpressions.Add(propertyExpression.ToString());
            PropertyObserver.Observes(propertyExpression, RaiseCanExecuteChanged);
        }

        //
        // 摘要:
        //     This raises the Prism.Commands.DelegateCommandBase.IsActiveChanged event.
        protected virtual void OnIsActiveChanged()
        {
            this.IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public class DelegateCommand<T> : DelegateCommandBase
    {
        private readonly Action<T> _executeMethod;

        private Func<T, bool> _canExecuteMethod;

        //
        // 摘要:
        //     Initializes a new instance of Prism.Commands.DelegateCommand`1.
        //
        // 参数:
        //   executeMethod:
        //     Delegate to execute when Execute is called on the command. This can be null to
        //     just hook up a CanExecute delegate.
        //
        // 言论：
        //     Prism.Commands.DelegateCommand`1.CanExecute(`0) will always return true.
        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, (Func<T, bool>)((T o) => true))
        {
        }

        //
        // 摘要:
        //     Initializes a new instance of Prism.Commands.DelegateCommand`1.
        //
        // 参数:
        //   executeMethod:
        //     Delegate to execute when Execute is called on the command. This can be null to
        //     just hook up a CanExecute delegate.
        //
        //   canExecuteMethod:
        //     Delegate to execute when CanExecute is called on the command. This can be null.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     When both executeMethod and canExecuteMethod are null.
        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            if (executeMethod == null || canExecuteMethod == null)
            {
                throw new ArgumentNullException("executeMethod");
            }

            TypeInfo typeInfo = typeof(T).GetTypeInfo();
            if (typeInfo.IsValueType && (!typeInfo.IsGenericType || !typeof(Nullable<>).GetTypeInfo().IsAssignableFrom(typeInfo.GetGenericTypeDefinition().GetTypeInfo())))
            {
                throw new InvalidCastException();
            }

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        //
        // 摘要:
        //     Executes the command and invokes the System.Action`1 provided during construction.
        //
        // 参数:
        //   parameter:
        //     Data used by the command.
        public void Execute(T parameter)
        {
            _executeMethod(parameter);
        }

        //
        // 摘要:
        //     Determines if the command can execute by invoked the System.Func`2 provided during
        //     construction.
        //
        // 参数:
        //   parameter:
        //     Data used by the command to determine if it can execute.
        //
        // 返回结果:
        //     true if this command can be executed; otherwise, false.
        public bool CanExecute(T parameter)
        {
            return _canExecuteMethod(parameter);
        }

        //
        // 摘要:
        //     Handle the internal invocation of System.Windows.Input.ICommand.Execute(System.Object)
        //
        // 参数:
        //   parameter:
        //     Command Parameter
        protected override void Execute(object parameter)
        {
            Execute((T)parameter);
        }

        //
        // 摘要:
        //     Handle the internal invocation of System.Windows.Input.ICommand.CanExecute(System.Object)
        //
        // 参数:
        //   parameter:
        //
        // 返回结果:
        //     true if the Command Can Execute, otherwise false
        protected override bool CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        //
        // 摘要:
        //     Observes a property that implements INotifyPropertyChanged, and automatically
        //     calls DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
        //
        // 参数:
        //   propertyExpression:
        //     The property expression. Example: ObservesProperty(() => PropertyName).
        //
        // 类型参数:
        //   TType:
        //     The type of the return value of the method that this delegate encapsulates
        //
        // 返回结果:
        //     The current instance of DelegateCommand
        public DelegateCommand<T> ObservesProperty<TType>(Expression<Func<TType>> propertyExpression)
        {
            ObservesPropertyInternal(propertyExpression);
            return this;
        }

        //
        // 摘要:
        //     Observes a property that is used to determine if this command can execute, and
        //     if it implements INotifyPropertyChanged it will automatically call DelegateCommandBase.RaiseCanExecuteChanged
        //     on property changed notifications.
        //
        // 参数:
        //   canExecuteExpression:
        //     The property expression. Example: ObservesCanExecute(() => PropertyName).
        //
        // 返回结果:
        //     The current instance of DelegateCommand
        public DelegateCommand<T> ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {
            Expression<Func<T, bool>> expression = Expression.Lambda<Func<T, bool>>(canExecuteExpression.Body, new ParameterExpression[1] { Expression.Parameter(typeof(T), "o") });
            _canExecuteMethod = expression.Compile();
            ObservesPropertyInternal(canExecuteExpression);
            return this;
        }
    }
}
