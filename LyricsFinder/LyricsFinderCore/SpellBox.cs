using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Forms.Design;
using System.Windows.Media;

using MediaCenter.SharedComponents;
using System.Windows.Input;
using System.Windows.Interop;

namespace MediaCenter.LyricsFinder
{

    /// <summary>
    /// WPF TextBox with spelling capabilities.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Integration.ElementHost" />
    /// <remarks>
    /// <para>Source inspired by: https://stackoverflow.com/questions/4024798/trying-to-use-the-c-sharp-spellcheck-class/4026132#4026132</para>
    /// <para>Author's remarks:</para>
    /// <para>You have to use a WPF TextBox to make spell checking work. 
    /// You can embed one in a Windows Forms form with the ElementHost control.
    /// It works pretty similar to a UserControl. 
    /// Here's a control that you can drop straight from the toolbox. 
    /// To get started, you need Project + Add Reference and select: 
    /// WindowsFormsIntegration, System.Design and the WPF assemblies PresentationCore, PresentationFramework and WindowsBase.
    /// </para>
    /// <para>
    /// Add a new class to your project and paste the code shown below.
    /// Compile.
    /// Drop the SpellBox control from the top of the toolbox onto a form. 
    /// It supports the TextChanged event and the Multiline and WordWrap properties. 
    /// There's a nagging problem with the Font, there is no easy way to map a WF Font to the WPF font properties. 
    /// The easiest workaround for that is to set the form's Font to "Segoe UI", the default for WPF.
    /// </para>
    /// <para>
    ///[DesignerSerializer("System.Windows.Forms.Design.ControlCodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    /// </para>
    /// </remarks>
    [Designer(typeof(ControlDesigner))]
    public class SpellBox : ElementHost
    {

        private readonly TextBox _textBox;


        /// <summary>
        /// Initializes a new instance of the <see cref="SpellBox"/> class.
        /// </summary>
        public SpellBox()
        {
            _textBox = new TextBox();

            base.Child = _textBox;

            _textBox.AutoWordSelection = false;
            _textBox.Background = Brushes.White;
            _textBox.ContextMenu = null;
            _textBox.Focusable = true;
            _textBox.Foreground = Brushes.Black;
            _textBox.IsInactiveSelectionHighlightEnabled = true;
            _textBox.SpellCheck.IsEnabled = false;

            _textBox.Loaded += TextBox_Loaded;
            _textBox.MouseEnter += (s, e) => OnEnterWpf(EventArgs.Empty);
            _textBox.MouseLeave += (s, e) => OnLeaveWpf(EventArgs.Empty);
            _textBox.PreviewKeyDown += (s, e) => OnPreviewKeyDownWpf(e);
            _textBox.TextChanged += (s, e) => OnTextChangedWpf(EventArgs.Empty);

            _textBox.Resources[SystemColors.InactiveSelectionHighlightBrushKey] = SystemColors.HighlightBrush;
            _textBox.Resources[SystemColors.InactiveSelectionHighlightTextBrushKey] = SystemColors.HighlightTextBrush;

            // PreviewKeyDown += (s, e) => OnPreviewKeyDown(e);

            InstalledInputLanguages = System.Windows.Forms.InputLanguage.InstalledInputLanguages;
            InstalledCultures = new List<CultureInfo>();

            foreach (System.Windows.Forms.InputLanguage lang in InstalledInputLanguages)
            {
                InstalledCultures.Add(lang.Culture);
            }
        }


        /**********************/
        /***** Properties *****/
        /**********************/

        /// <summary>
        /// Gets a value that indicates whether the most recent undo action can be redone.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can redo; otherwise, <c>false</c>.
        /// </value>
        public virtual bool CanRedo { get => _textBox.CanRedo; }

        /// <summary>
        /// Gets a value that indicates whether the most recent action can be undone.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can undo; otherwise, <c>false</c>.
        /// </value>
        public virtual bool CanUndo { get => _textBox.CanUndo; }

        /// <summary>
        /// Gets or sets the <see cref="System.Windows.UIElement" /> hosted by the <see cref="System.Windows.Forms.Integration.ElementHost" /> control.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new UIElement Child
        {
            get { return base.Child; }
            set { /* Do nothing to solve a problem with the serializer !! */ }
        }

        /// <summary>
        /// Gets a value that indicates whether the control has input focus.
        /// </summary>
        public override bool Focused { get => _textBox.IsFocused; }

        /// <summary>
        /// Gets the installed input languages.
        /// </summary>
        /// <value>
        /// The installed input languages.
        /// </value>
        protected System.Windows.Forms.InputLanguageCollection InstalledInputLanguages { get; private set; }

        /// <summary>
        /// Gets the installed cultures.
        /// </summary>
        /// <value>
        /// The installed cultures.
        /// </value>
        public List<CultureInfo> InstalledCultures { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SpellBox"/> is multiline.
        /// Gets or sets a value that indicates how the text editing control responds when the user presses the ENTER key.
        /// </summary>
        /// <value>
        ///   <c>true</c> if pressing the ENTER key inserts a new line at the current cursor position; 
        ///   otherwise, the ENTER key is ignored. 
        ///   The default value is <c>false</c>.
        /// </value>
        [DefaultValue(false)]
        public virtual bool Multiline
        {
            get => _textBox.AcceptsReturn;
            set => _textBox.AcceptsReturn = value;
        }

        /// <summary>
        /// Gets or sets the parent WinForms form.
        /// </summary>
        /// <value>
        /// The parent WinForms form.
        /// </value>
        public virtual System.Windows.Forms.Form ParentForm { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the text editing control is read-only to a user interacting with the control.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the contents of the text editing control are read-only to a user; 
        ///   otherwise, the contents of the text editing control can be modified by the user. 
        ///   The default value is <c>false</c>.
        /// </value>
        [DefaultValue(false)]
        public virtual bool ReadOnly
        {
            get => _textBox.IsReadOnly;
            set
            {
                _textBox.IsReadOnly = value;
                _textBox.IsReadOnlyCaretVisible = value;
                _textBox.Background = (value) ? Brushes.WhiteSmoke : Brushes.White;
            }
        }

        /// <summary>
        /// Gets or sets which scroll bars should appear in a multiline TextBox control.
        /// </summary>
        /// <value>
        /// One of the ScrollBars enumeration values that indicates whether a multiline TextBox control appears with no scroll bars, 
        /// a horizontal scroll bar, a vertical scroll bar, or both. 
        /// The default is <c>ScrollBars.None</c>.
        /// </value>
        public virtual System.Windows.Forms.ScrollBars ScrollBars
        {
            get
            {
                var h = (_textBox.HorizontalScrollBarVisibility == ScrollBarVisibility.Auto)
                    || (_textBox.HorizontalScrollBarVisibility == ScrollBarVisibility.Visible);

                var v = (_textBox.VerticalScrollBarVisibility == ScrollBarVisibility.Auto)
                    || (_textBox.VerticalScrollBarVisibility == ScrollBarVisibility.Visible);

                if (h && v)
                    return System.Windows.Forms.ScrollBars.Both;
                else if (h)
                    return System.Windows.Forms.ScrollBars.Horizontal;
                else if (v)
                    return System.Windows.Forms.ScrollBars.Vertical;
                else
                    return System.Windows.Forms.ScrollBars.None; // We should never get here!
            }
            set
            {
                if (value == System.Windows.Forms.ScrollBars.Both)
                {
                    _textBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                    _textBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                }
                else if (value == System.Windows.Forms.ScrollBars.Horizontal)
                {
                    _textBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                    _textBox.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                }
                else if (value == System.Windows.Forms.ScrollBars.Vertical)
                {
                    _textBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                    _textBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                }
                else // (value == System.Windows.Forms.ScrollBars.None)
                {
                    _textBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    _textBox.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the currently selected text in the control.
        /// </summary>
        /// <value>
        /// The selected text.
        /// </value>
        public virtual string SelectedText
        {
            get => _textBox.Text.Substring(_textBox.SelectionStart, _textBox.SelectionLength);
            set
            {
                if (_textBox.SelectionStart < 0) return;

                var selStart = _textBox.SelectionStart;
                var txt1 = _textBox.Text.Substring(0, selStart);
                var txt2 = _textBox.Text.Substring(selStart + _textBox.SelectionLength);

                _textBox.Clear();
                _textBox.AppendText(txt1);
                _textBox.AppendText(value);
                _textBox.AppendText(txt2);
                _textBox.UndoLimit = -1;
            }
        }

        /// <summary>
        /// Gets or sets the number of characters selected in the text box.
        /// </summary>
        /// <value>
        /// The length of the selection.
        /// </value>
        [DefaultValue(0)]
        public virtual int SelectionLength
        {
            get => _textBox.SelectionLength;
            set => _textBox.SelectionLength = value;
        }

        /// <summary>
        /// Gets or sets a character index for the beginning of the current selection.
        /// </summary>
        /// <value>
        /// The character index for the beginning of the current selection.
        /// </value>
        [DefaultValue(-1)]
        public virtual int SelectionStart
        {
            get => _textBox.SelectionStart;
            set => _textBox.SelectionStart = value;
        }

        /// <summary>
        /// Gets or sets a value that determines whether the spelling checker is enabled on this text-editing control, such as TextBox or RichTextBox.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the spelling checker is enabled on the control; otherwise, <c>false</c>. 
        ///   The default value is <c>false</c>.
        /// </value>
        public virtual bool SpellCheckEnabled
        {
            get => _textBox.SpellCheck.IsEnabled;
            set => _textBox.SpellCheck.IsEnabled = value;
        }

        /// <summary>
        /// Gets or sets the text contents of the text box.
        /// </summary>
        /// <value>
        /// A string containing the text contents of the text box. 
        /// The default is an empty string ("").
        /// </value>
        public override string Text
        {
            get => _textBox.Text;
            set => _textBox.Text = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use the wait cursor for the current control and all child controls.
        /// </summary>
        /// <value>
        ///   <c>true</c> if a wait cursor should be used in the control; otherwise, <c>false</c>.
        /// </value>
        [DefaultValue(false)]
        public new bool UseWaitCursor
        {
            get { return (_textBox.Cursor == Cursors.Wait) ? true : false; }
            set { _textBox.Cursor = (value) ? Cursors.Wait : null; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether word wrap is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if word wrap is enabled; otherwise, <c>false</c>.
        /// </value>
        [DefaultValue(false)]
        public virtual bool WordWrap
        {
            get => _textBox.TextWrapping != TextWrapping.NoWrap;
            set => _textBox.TextWrapping = value ? TextWrapping.Wrap : TextWrapping.NoWrap;
        }


        /*******************/
        /***** Methods *****/
        /*******************/

        /// <summary>
        /// Clears information about the most recent operation from the undo buffer of the text box.
        /// </summary>
        public virtual void ClearUndo()
        {
            _textBox.UndoLimit = -1;
        }


        /// <summary>
        /// Copies the current selection of the text editing control to the Clipboard.
        /// </summary>
        public virtual void Copy()
        {
            _textBox.Copy();
        }


        /// <summary>
        /// Removes the current selection from the text editing control and copies it to the Clipboard.
        /// </summary>
        public virtual void Cut()
        {
            _textBox.Cut();
        }


        /// <summary>
        /// Specifies that the value of the SelectionLength property is zero so that no characters are selected in the control.
        /// </summary>
        public virtual void DeselectAll()
        {
            _textBox.SelectionLength = 0;
        }


        /// <summary>
        /// Focuses this instance.
        /// </summary>
        public new void Focus()
        {
            _textBox.Focus();
        }


        /// <summary>
        /// Pastes the contents of the Clipboard over the current selection in the text editing control.
        /// </summary>
        public virtual void Paste()
        {
            _textBox.Paste();
        }


        /// <summary>
        /// Undoes the most recent undo command. In other words, redoes the most recent undo unit on the undo stack.
        /// </summary>
        /// <returns><c>true</c> if the redo operation was successful; otherwise, <c>false</c>. 
        /// This method returns false if there is no undo command available (the undo stack is empty).
        /// </returns>
        public virtual bool Redo()
        {
            return _textBox.Redo();
        }


        /// <summary>
        /// Scrolls to the home or end of the text.
        /// </summary>
        /// <param name="isHome">if set to <c>true</c> scroll to home; else scroll to the end.</param>
        public virtual void ScrollHomeOrEnd(bool isHome)
        {
            if (isHome)
                _textBox.ScrollToHome();
            else
                _textBox.ScrollToEnd();
        }


        /// <summary>
        /// Scrolls left or right.
        /// </summary>
        /// <param name="isLeft">if set to <c>true</c> scroll left; else scroll right.</param>
        /// <param name="isPage">if set to <c>true</c> scroll a full page; else scroll 1 line.</param>
        public virtual void ScrollHorizontal(bool isLeft, bool isPage)
        {
            if (isLeft)
            {
                if (isPage)
                    _textBox.PageLeft();
                else
                    _textBox.LineLeft();
            }
            else
            {
                if (isPage)
                    _textBox.PageRight();
                else
                    _textBox.LineRight();
            }
        }


        /// <summary>
        /// Scrolls up or down.
        /// </summary>
        /// <param name="isDown">if set to <c>true</c> scroll down; else scroll up.</param>
        /// <param name="isPage">if set to <c>true</c> scroll a full page; else scroll 1 line.</param>
        public virtual void ScrollVertical(bool isDown, bool isPage)
        {
            if (isDown)
            {
                if (isPage)
                    _textBox.PageDown();
                else
                    _textBox.LineDown();
            }
            else
            {
                if (isPage)
                    _textBox.PageUp();
                else
                    _textBox.LineUp();
            }
        }


        /// <summary>
        /// Scrolls the contents of the control to the current caret position.
        /// </summary>
        public virtual void ScrollToCaret()
        {
            var idx = _textBox.GetLineIndexFromCharacterIndex(_textBox.CaretIndex);

            _textBox.ScrollToLine(idx);
        }


        /// <summary>
        /// Selects a range of text in the text box.
        /// </summary>
        /// <param name="start">The position of the first character in the current text selection within the text box.</param>
        /// <param name="length">The number of characters to select.</param>
        public virtual void Select(int start, int length)
        {
            _textBox.Select(start, length);
        }


        /// <summary>
        /// Sets the selection text.
        /// </summary>
        /// <param name="selectionOperation">The selection operation to be used.</param>
        /// <param name="cultureInfo">The culture information.</param>
        public virtual void SetSelectionText(SelectionOperation selectionOperation, CultureInfo cultureInfo = null)
        {
            var ci = cultureInfo ?? CultureInfo.CurrentCulture;
            var selStart = SelectionStart;
            var selLength = SelectionLength;
            var selText = SelectedText;

            if (selLength < 1)
                SelectionLength = 1;

            switch (selectionOperation)
            {
                case SelectionOperation.Copy:
                    Copy();
                    break;

                case SelectionOperation.Cut:
                case SelectionOperation.Delete:
                    Cut();
                    selLength = 0;
                    break;

                case SelectionOperation.LowerCase:
                    SelectedText = selText.ToLower(ci);
                    break;

                case SelectionOperation.Paste:
                    // Determine if there is any text in the Clipboard to paste into the text box.
                    if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
                        Paste();
                    break;

                case SelectionOperation.ProperCase:
                    SelectedText = selText.ToProperCase(ci);
                    break;

                case SelectionOperation.RemoveDoubleLineEndings:
                    SelectedText = selText.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
                    break;

                case SelectionOperation.SentenceCase:
                    SelectedText = selText.ToSentenceCase(ci);
                    break;

                case SelectionOperation.TitleCase:
                    SelectedText = selText.ToTitleCase(ci);
                    break;

                case SelectionOperation.Trim:
                    var tmp = selText.TrimStringLines();
                    SelectedText = tmp;
                    selLength = tmp.Length;
                    break;

                case SelectionOperation.UpperCase:
                    SelectedText = selText.ToUpper(ci);
                    break;

                default:
                    break;
            }

            SelectionStart = selStart;
            SelectionLength = selLength;
        }


        /// <summary>
        /// Undoes this instance.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the undo operation was successful; otherwise, <c>false</c>. 
        /// This method returns false if the undo stack is empty.
        /// </returns>
        public virtual bool Undo()
        {
            return _textBox.Undo();
        }


        /// <summary>
        /// Selects all the contents of the text editing control.
        /// </summary>
        public virtual void SelectAll()
        {
            _textBox.SelectAll();
        }


        /********************************/
        /***** Events and Delegates *****/
        /********************************/

        /// <summary>
        /// Occurs when the control is entered.
        /// </summary>
        [Browsable(true)]
        public new event EventHandler Enter;

        /// <summary>
        /// Raises the <see cref="System.Windows.Forms.Control.Enter" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnEnterWpf(EventArgs e)
        {
            Enter?.Invoke(this, e);
        }


        /// <summary>
        /// Occurs when a key is pressed while the control has focus.
        /// </summary>
        [Browsable(true)]
        public new event EventHandler<System.Windows.Forms.KeyEventArgs> KeyDown;

        /// <summary>
        /// Raises the <see cref="KeyDown" /> event with WPF event args converted to WinForms event args.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs" /> instance containing the event data.</param>
        /// <exception cref="ArgumentNullException">e</exception>
        /// <remarks>
        /// Source inspired by: https://stackoverflow.com/questions/1153009/how-can-i-convert-system-windows-input-key-to-system-windows-forms-keys
        /// </remarks>
        protected virtual void OnPreviewKeyDownWpf(System.Windows.Input.KeyEventArgs e)
        {
            if (e is null) throw new ArgumentNullException(nameof(e));

            System.Windows.Input.Key wpfKey = e.Key;
            System.Windows.Forms.Keys wfKeys;

            // wpfKey = System.Windows.Input.KeyInterop.KeyFromVirtualKey((int)winFormsKeys);
            wfKeys = (System.Windows.Forms.Keys)System.Windows.Input.KeyInterop.VirtualKeyFromKey(wpfKey);

            if ((System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Alt) > 0)
                wfKeys |= System.Windows.Forms.Keys.Alt;

            if ((System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) > 0)
                wfKeys |= System.Windows.Forms.Keys.Control;

            if ((System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Shift) > 0)
                wfKeys |= System.Windows.Forms.Keys.Shift;

            var wfKeyEventArgs = new System.Windows.Forms.KeyEventArgs(wfKeys);

            // For some reason Ctrl+Tab needs extra handling, otherwise focus cannot be moved away from the SpellBox.
            // So we simulate the Shift key being pressed also.
            if (wfKeyEventArgs.Control && !wfKeyEventArgs.Shift && (wfKeyEventArgs.KeyCode == System.Windows.Forms.Keys.Tab))
            {
                wfKeys |= System.Windows.Forms.Keys.Shift;
                wfKeyEventArgs = new System.Windows.Forms.KeyEventArgs(wfKeys);
                e.Handled = true;
            }

            KeyDown?.Invoke(this, wfKeyEventArgs);
        }


        /// <summary>
        /// Occurs when the input focus leaves the control.
        /// </summary>
        [Browsable(true)]
        public new event EventHandler Leave;

        /// <summary>
        /// Raises the <see cref="System.Windows.Forms.Control.Leave" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnLeaveWpf(EventArgs e)
        {
            Leave?.Invoke(this, e);
        }


        /// <summary>
        /// Occurs when a key is pressed while the control has focus.
        /// </summary>
        [Browsable(true)]
        public new event EventHandler<System.Windows.Forms.PreviewKeyDownEventArgs> PreviewKeyDown;

        /// <summary>
        /// Raises the <see cref="System.Windows.Forms.Control.PreviewKeyDown" /> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.PreviewKeyDownEventArgs"/> instance containing the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected override void OnPreviewKeyDown(System.Windows.Forms.PreviewKeyDownEventArgs e)
        {
            PreviewKeyDown?.Invoke(this, e);
        }


        /// <summary>
        /// Occurs when the <see cref="System.Windows.Forms.Control.Text" /> property value changes.
        /// </summary>
        [Browsable(true)]
        public new event EventHandler TextChanged;

        /// <summary>
        /// Raises the <see cref="TextChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnTextChangedWpf(EventArgs e)
        {
            TextChanged?.Invoke(this, e);
        }


        /// <summary>
        /// Handles the Loaded event of the TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (_textBox.Focusable)
                _textBox.Focus();

            e.Handled = true;
        }

    }



    /// <summary>
    /// Selection operations.
    /// </summary>
    public enum SelectionOperation
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Copy,
        Cut,
        Delete,
        LowerCase,
        Paste,
        ProperCase,
        RemoveDoubleLineEndings,
        SentenceCase,
        TitleCase,
        Trim,
        UpperCase
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

}
