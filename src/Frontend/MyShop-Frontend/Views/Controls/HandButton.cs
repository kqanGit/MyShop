using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_Frontend.Views.Controls
{
    internal sealed partial class AppButton : Button
    {
        public AppButton()
        {
            // dùng style mặc định của Button
            this.DefaultStyleKey = typeof(Button);

            // set cursor hand khi pointer over
            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
        }
    }
}
