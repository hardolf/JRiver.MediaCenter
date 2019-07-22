using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MediaCenter.SharedComponents
{
#pragma warning disable CA1062 // Validate arguments of public methods

    /// <summary>
    /// Undo/redo command interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>Inspired by: <see href="https://www.cambiaresearch.com/articles/82/generic-undoredo-stack-in-csharp"/>.</remarks>
    public interface ICommand<T>
    {

        T Do(T input);

        T Undo(T input);

    }



    /// <summary>
    /// Add integer command.
    /// </summary>
    /// <seealso cref="MediaCenter.SharedComponents.ICommand{int}" />
    /// <remarks>Inspired by: <see href="https://www.cambiaresearch.com/articles/82/generic-undoredo-stack-in-csharp"/>.</remarks>
    public class AddIntCommand : ICommand<int>
    {

        private int _value;


        public int Value
        {
            get => _value;
            set => _value = value;
        }


        public AddIntCommand()
        {
            _value = 0;
        }


        public AddIntCommand(int value)
        {
            _value = value;
        }


        public int Do(int input)
        {
            return input + _value;
        }


        public int Undo(int input)
        {
            return input - _value;
        }

    }



    /// <summary>
    /// Set string command.
    /// </summary>
    /// <seealso cref="MediaCenter.SharedComponents.ICommand{int}" />
    /// <remarks>Inspired by: <see href="https://www.cambiaresearch.com/articles/82/generic-undoredo-stack-in-csharp"/>.</remarks>
    public class SetStringCommand : ICommand<string>
    {
        private string _value;


        public string Value
        {
            get => _value;
            set => _value = value;
        }


        public SetStringCommand()
        {
            _value = string.Empty;
        }


        public SetStringCommand(string value)
        {
            _value = value;
        }


        public string Do(string input)
        {
            _value = input;
            return _value;
        }


        public string Undo(string input)
        {
            _value = input;
            return _value;
        }

    }



    /// <summary>
    /// Undo/redo stack.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>Inspired by: <see href="https://www.cambiaresearch.com/articles/82/generic-undoredo-stack-in-csharp"/>.</remarks>
    public class UndoRedoStack<T>
    {
        private Stack<ICommand<T>> _undo;
        private Stack<ICommand<T>> _redo;


        public int UndoCount => _undo.Count;

        public int RedoCount => _redo.Count;


        public UndoRedoStack()
        {
            Reset();
        }


        public void Reset()
        {
            _undo = new Stack<ICommand<T>>();
            _redo = new Stack<ICommand<T>>();
        }


        public T Do(ICommand<T> cmd, T input)
        {
            T output = cmd.Do(input);
            _undo.Push(cmd);
            _redo.Clear(); // Once we issue a new command, the redo stack clears
            return output;
        }


        public void Push(ICommand<T> cmd)
        {
            _undo.Push(cmd);
            _redo.Clear(); // Anytime we push a new command, the redo stack clears
        }


        public T Redo(T input)
        {
            if (_redo.Count > 0)
            {
                ICommand<T> cmd = _redo.Pop();
                T output = cmd.Do(input);
                _undo.Push(cmd);
                return output;
            }
            else
            {
                return input;
            }
        }


        public ICommand<T> RePush()
        {
            if (_redo.Count > 0)
            {
                ICommand<T> cmd = _redo.Pop();
                _undo.Push(cmd);
                return cmd;
            }
            else
            {
                return null;
            }
        }


        public T Undo(T input)
        {
            if (_undo.Count > 0)
            {
                ICommand<T> cmd = _undo.Pop();
                T output = cmd.Undo(input);
                _redo.Push(cmd);
                return output;
            }
            else
            {
                return input;
            }
        }


        public ICommand<T> UnPush()
        {
            if (_undo.Count > 0)
            {
                ICommand<T> cmd = _undo.Pop();
                _redo.Push(cmd);
                return cmd;
            }
            else
            {
                return null;
            }
        }

    }

#pragma warning restore CA1062 // Validate arguments of public methods
}
