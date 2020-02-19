using System.Drawing;

// Source: https://psycodedeveloper.wordpress.com/2019/08/03/my-custom-toolstripslider-windows-forms-control/
// Source code: https://drive.google.com/drive/folders/1BGKnttu1RbOpA-liID69IXYdW0lRd9nD?usp=sharing

namespace System.Windows.Forms
{

    public static class ControlExtensions
    {

        public static bool TryDispose(this Control control)
        {
            if (control != null)
            {
                if (control is IDisposable disposable)
                    return disposable.TryDispose();
            }
            return false;
        }

        public static bool TryDispose(this object control)
        {
            if (control != null)
            {
                if (control is IDisposable disposable)
                    return disposable.TryDispose();
            }
            return false;
        }

        public static bool TryDispose(this IDisposable control)
        {
            if (control != null)
            {
                control.Dispose();
                return true;
            }
            return false;
        }

        /// <summary>Performs a click conditionally, if the item is
        /// enabled, and returns whether the condition was met.</summary>
        public static bool PerformConditionalClick(this Button control)
        {
            if (control.Enabled)
                control.PerformClick();

            return control.Enabled;
        }

        /// <summary>Performs a click conditionally, if the item is
        /// enabled, and returns whether the condition was met.</summary>
        public static bool PerformConditionalClick(this ToolStripItem control)
        {
            if (control.Enabled)
                control.PerformClick();

            return control.Enabled;
        }

        /// <summary>Retrieves the lowest nested child control that is located at the specified screen coordinates.</summary>
        /// <remarks>This is used to get the contained control from the cursor location, useful when dragging and dropping.</remarks>
        public static Control GetLowestNestedChildAtPoint(this Control container, Point pt)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            Control child;
            while ((child = container.GetChildAtPoint(container.PointToClient(pt))) != null)
            {
                container = child;
            }

            return container;
        }

        /// <summary>Activates and sets input focus on the control.</summary>
        public static void SelectAndFocus(this Control control)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (control.CanSelect)
                control.Select();

            if (control.CanFocus)
                control.Focus();
        }

        /// <summary>Selects and gives focus to the hosted ToolStrip control.</summary>
        public static void SelectAndFocus(this ToolStripControlHost control)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (control.CanSelect)
                control.Select();

            control.Focus();
        }

        public static bool IsShowing(this Control control)
        {
            if (control.Visible)
            {
                Screen screen = Screen.FromControl(control);
                if (screen != null)
                {
                    Rectangle rect = control.RectangleToScreen(control.Bounds);

                    return rect.Left >= screen.Bounds.Left && rect.Top >= screen.Bounds.Top &&
                        rect.Right <= screen.Bounds.Right && rect.Bottom <= screen.Bounds.Bottom;
                }
            }
            return false;
        }

    }

}
