using System;
using System.Collections.Generic;
using MoveStopMove.Utility.Extension;

namespace MoveStopMove.Utility
{
    public class CommandManager : Singleton<CommandManager>
    {
        #region -- Fields --

        private readonly Dictionary<string, Type> m_command = new();

        #endregion

        #region -- Methods --

        public void Register(string key, Type command)
        {
            if (m_command.ContainsKey(key)) return;

            m_command[key] = command;
        }

        public void Unregister(string key)
        {
            m_command.Remove(key);
        }

        public CommandNode Execute(string key, object data = null)
        {
            Command command = (Command)Activator.CreateInstance(m_command[key]);
            command.Data = data;
            command.Execute();

            return new(key, data);
        }

        public CommandNode Undo(string key, object data = null)
        {
            Command command = (Command)Activator.CreateInstance(m_command[key]);
            command.Data = data;
            command.Undo();

            return new(key, data);
        }

        #endregion
    }
}