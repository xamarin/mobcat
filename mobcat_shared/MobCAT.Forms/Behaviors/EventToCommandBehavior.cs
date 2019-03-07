using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace Microsoft.MobCAT.Forms.Behaviors
{
    public class EventToCommandBehavior : BehaviorBase<View>
    {
        View _parentConverterParameter;
        Delegate _eventHandler;

        public static readonly BindableProperty EventNameProperty = BindableProperty.Create(nameof(EventName), typeof(string), typeof(EventToCommandBehavior), null, propertyChanged: OnEventNameChanged);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(EventToCommandBehavior), null);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(EventToCommandBehavior), null);
        public static readonly BindableProperty InputConverterProperty = BindableProperty.Create(nameof(Converter), typeof(IValueConverter), typeof(EventToCommandBehavior), null);
        public static readonly BindableProperty InputConverterParameterProperty = BindableProperty.Create(nameof(ConverterParameter), typeof(IValueConverter), typeof(EventToCommandBehavior), null);
        public static readonly BindableProperty UseParentAsConverterParameterProperty = BindableProperty.Create(nameof(UseParentAsConverterParameter), typeof(bool), typeof(EventToCommandBehavior), false);

        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public IValueConverter Converter
        {
            get { return (IValueConverter)GetValue(InputConverterProperty); }
            set { SetValue(InputConverterProperty, value); }
        }

        public object ConverterParameter
        {
            get { return GetValue(InputConverterParameterProperty); }
            set { SetValue(InputConverterParameterProperty, value); }
        }

        public bool UseParentAsConverterParameter
        {
            get
            {
                return (bool)GetValue(UseParentAsConverterParameterProperty);
            }

            set
            {
                SetValue(UseParentAsConverterParameterProperty, value);
            }
        }

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);
            RegisterEvent(EventName);

            if (this.UseParentAsConverterParameter)
            {
                this._parentConverterParameter = bindable;
            }
        }

        protected override void OnDetachingFrom(View bindable)
        {
            DeregisterEvent(EventName);
            base.OnDetachingFrom(bindable);

            if (this._parentConverterParameter.Equals(bindable))
            {
                this._parentConverterParameter = null;
            }
        }

        void RegisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);

            if (eventInfo == null)
            {
                throw new ArgumentException($"{nameof(EventToCommandBehavior)}: Unable to register the '{EventName}' event.");
            }

            MethodInfo methodInfo = typeof(EventToCommandBehavior).GetTypeInfo().GetDeclaredMethod(nameof(OnEvent));
            _eventHandler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
            eventInfo.AddEventHandler(AssociatedObject, _eventHandler);
        }

        void DeregisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;

            if (_eventHandler == null)
                return;

            EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);

            if (eventInfo == null)
                throw new ArgumentException($"{nameof(EventToCommandBehavior)}: Unable to de-register the '{EventName}' event.");

            eventInfo.RemoveEventHandler(AssociatedObject, _eventHandler);
            _eventHandler = null;
        }

        void OnEvent(object sender, object eventArgs)
        {
            if (Command == null)
                return;

            object resolvedParameter;

            if (CommandParameter != null)
            {
                resolvedParameter = CommandParameter;
            }
            else if (Converter != null)
            {
                var converterParameter = this.ConverterParameter != null ? this.ConverterParameter : this.UseParentAsConverterParameter ? this._parentConverterParameter : null;
                resolvedParameter = Converter.Convert(eventArgs, typeof(object), converterParameter, null);
            }
            else
            {
                resolvedParameter = eventArgs;
            }

            if (Command.CanExecute(resolvedParameter))
                Command.Execute(resolvedParameter);
        }

        static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (EventToCommandBehavior)bindable;

            if (behavior.AssociatedObject == null)
                return;

            string oldEventName = (string)oldValue;
            string newEventName = (string)newValue;

            behavior.DeregisterEvent(oldEventName);
            behavior.RegisterEvent(newEventName);
        }
    }
}