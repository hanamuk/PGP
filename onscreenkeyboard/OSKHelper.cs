using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace OnScreenKeyboardSample
{
    public static class OSKHelper
    {

		// Target Control에 Focus 되었을 경우 실행할 Keyboard Application
        public static string ScreenKeyboardFile = "osk.exe";
        public static Process ScreenKeyboardInstance = null;

		// Target Control의 Focus이벤트를 처리하는 EventHandler
        private static RoutedEventHandler FocusEventHandler = new RoutedEventHandler(FocusHandler);

		// Attached DependencyProperty선언 부분
        public static readonly DependencyProperty IsSurpportScreenKeyboardProperty =
            DependencyProperty.RegisterAttached("IsSurpportScreenKeyboard", typeof(bool?), 
            typeof(OSKHelper), 
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(IsSurpportScreenKeyboardChanged)));

        public static bool GetIsSurpportScreenKeyboard(FrameworkElement Target)
        {            
            return (bool)Target.GetValue(IsSurpportScreenKeyboardProperty);
        }

        public static void SetIsSurpportScreenKeyboard(FrameworkElement Target, bool? Value)
        {
            Target.SetValue(IsSurpportScreenKeyboardProperty, Value);
        }

		// Target Control의 IsSurpportScreenKeyboard Property가 변경되었을때 처리
        public static void IsSurpportScreenKeyboardChanged(DependencyObject Sender, DependencyPropertyChangedEventArgs e)   
        {
            FrameworkElement Target = Sender as FrameworkElement;

			// IsSurpportScreenKeyboard Property가 True라면 EventHandler를 추가.
			if ((bool)e.NewValue )
            {
                Target.GotFocus += FocusEventHandler;
                Target.LostFocus += FocusEventHandler;
            }
			// IsSurpportScreenKeyboard Property가 True가 아니고, EventHandler를 추가되었었다면
			// Event Handler를 제거
            else if (FocusEventHandler.GetInvocationList().First().Target == Target)
            {
                Target.GotFocus -= FocusEventHandler;
                Target.LostFocus -= FocusEventHandler;    
            }   
        }


        static void FocusHandler(object sender, RoutedEventArgs e)
        {
            FrameworkElement Target = sender as FrameworkElement;
            FrameworkElement Source = e.Source as FrameworkElement;

			// IsSurpportScreenKeyboard Property가 False인데 EventHandler가 등록되었다면, 제거
            if ((bool?)Target.GetValue(IsSurpportScreenKeyboardProperty) == false)
            {
                Target.GotFocus -= FocusEventHandler;
                Target.GotFocus -= FocusEventHandler;
                return;
            }
			// IsSurpportScreenKeyboard Property가 False가 아니고 RoutedEvent가 GotFocus이며,
			// Source Control이 TextBox이거나 PasswordBox일때 Keyboard Application를 실행
            else if ((bool?)Source.GetValue(IsSurpportScreenKeyboardProperty) != false &&
                e.RoutedEvent == FocusManager.GotFocusEvent && (e.Source is TextBox || e.Source is PasswordBox))
            {
                ScreenKeyboardInstance = Process.Start(ScreenKeyboardFile);
            }
			// RoutedEvent가 LostFocus이고 ScreenKeyboard Application이 생성되어 있다면 종료,
            else if (e.RoutedEvent == FocusManager.LostFocusEvent && ScreenKeyboardInstance != null)
            {
                if (ScreenKeyboardInstance.HasExited == false)
                    ScreenKeyboardInstance.CloseMainWindow();
                ScreenKeyboardInstance = null;
            }

        }  

    }
}
